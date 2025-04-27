// RoomViewModel.cs
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public class RoomViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Room class is required")]
        [Display(Name = "Room Type")]
        public int RoomClassID { get; set; }

        [Required(ErrorMessage = "Floor is required")]
        [Range(1, 10, ErrorMessage = "Floor must be between 1 and 10")]
        public int Floor { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 1000000, ErrorMessage = "Price must be at least $1")]
        [Display(Name = "Price per night")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rate { get; set; }

        [Display(Name = "Room View Description")]
        [StringLength(100, ErrorMessage = "View description cannot exceed 100 characters")]
        public string? View { get; set; }

        [Display(Name = "Room Status")]
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        [Display(Name = "Room Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Current Image")]
        public string? ExistingImageUrl { get; set; }
    }
}