using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Validations
{
    public class CurrentOrFutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date.Date >= DateTime.Today;
            }
            return false;
        }
    }
}
