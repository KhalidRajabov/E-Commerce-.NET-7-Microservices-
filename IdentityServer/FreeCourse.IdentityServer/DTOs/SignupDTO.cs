using System.ComponentModel.DataAnnotations;

namespace FreeCourse.IdentityServer.DTOs
{
    public class SignupDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string City { get; set; }
    }
}
