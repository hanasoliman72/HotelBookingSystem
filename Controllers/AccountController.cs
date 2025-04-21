using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View(new EndUser()); 
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new EndUser());
    }
    [HttpPost]
    public IActionResult Login(EndUser model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Fake user validation (replace with real DB logic)
        if (model.UserName == "admin" && model.Password == "admin")
        {
            // Save user to session
            HttpContext.Session.SetString("UserName", model.UserName);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid username or password.");
        return View(model);
    }
}
