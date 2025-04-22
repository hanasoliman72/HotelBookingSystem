using BookingSystem.Data;
using BookingSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly BookingContext _context; // DbContext for EndUser table

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger,
        BookingContext context) // Injecting ApplicationDbContext
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _context = context; // Initialize the DbContext
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }
    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public bool IsFirstUser { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        // Check if this is the first user
        var userCount = await _userManager.Users.CountAsync();
        IsFirstUser = userCount == 0; // If no users are registered, this is the first user
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            var user = CreateUser();
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.UserName = Input.UserName;
            user.PhoneNumber = Input.PhoneNumber;

            // Ensure the provided email is set for the user
            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                // Check if this is the first user to register
                var userCount = await _userManager.Users.CountAsync();
                if (userCount == 1) // Only assign the SuperAdmin role for the first user
                {
                    // Assign "SuperAdmin" role to the first user
                    if (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                    {
                        await _userManager.AddToRoleAsync(user, "SuperAdmin");
                    }
                }
                else
                {
                    // Assign "User" role to other users
                    await _userManager.AddToRoleAsync(user, "User");
                }

                // Save the user data to the EndUser table
                var endUser = new EndUser
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    UserName = Input.UserName,
                    Email = Input.Email,
                    Password = Input.Password, // You may not want to save this directly; consider hashing or other security measures
                    PhoneNumber = Input.PhoneNumber,
                    Role = (userCount == 1) ? "SuperAdmin" : "User" // Set role as SuperAdmin for the first user, otherwise User
                };
                _context.endUsers.Add(endUser);
                await _context.SaveChangesAsync();  // Commit to the database

                // Sign the user in
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }



    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
}
