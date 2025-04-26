using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Feedback
    {
        public int ID { get; set; }

        [ForeignKey("Booking")]
        public int BookingID { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string? Comment { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        public DateTime FeedbackDate { get; set; } = DateTime.Now;

        public virtual Booking Booking { get; set; }
    }
}
