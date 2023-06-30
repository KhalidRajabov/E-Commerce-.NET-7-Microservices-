using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class SigninInput
    {
        [Display(Name="Email address")]
        public string Email { get; set; }
        [Display(Name="Your password")]
        public string Password { get; set; }
        [Display(Name="Remember me")]
        public bool IsRemember { get; set; }

    }
}
