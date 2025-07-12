using System.ComponentModel.DataAnnotations;

namespace ContactManagementProject.Models
{
    public class Contact
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters long.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Contact number must be exactly 9 digits.")]
        public string ContactNumber { get; set; } = "";

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = "";

        public bool IsDeleted { get; set; }
    }
}
