using System.ComponentModel.DataAnnotations;
using Zombie.Models;

namespace Zombie.ViewModels
{
    public class RegistrationPostVm
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Registration), ErrorMessageResourceName = "UsernameRequired")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,15}$", ErrorMessageResourceType = typeof(Resources.Registration), ErrorMessageResourceName = "UsernamePattern")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Registration), ErrorMessageResourceName = "PasswordRequired")]
        [MinLength(8, ErrorMessageResourceType = typeof(Resources.Registration), ErrorMessageResourceName = "PasswordMinLength")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Registration), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Registration), ErrorMessageResourceName = "EmailPattern")]
        public string Email { get; set; }

        internal User MapToUser()
        {
            return new User {
                Username = Username,
                Email = Email
            };
        }
    }
}
