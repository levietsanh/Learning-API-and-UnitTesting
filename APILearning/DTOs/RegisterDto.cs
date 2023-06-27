using System.ComponentModel.DataAnnotations;

namespace APILearning.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
