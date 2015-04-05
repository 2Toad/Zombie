using System.ComponentModel.DataAnnotations;

namespace Zombie.ViewModels
{
    public class ChangeActivationEmailPostVm
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Activation), ErrorMessageResourceName = "CodeRequired")]
        [MinLength(32, ErrorMessageResourceType = typeof(Resources.Activation), ErrorMessageResourceName = "CodePattern")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resources.Activation), ErrorMessageResourceName = "CodePattern")]
        public string Code { get; set; }

        /// <summary>
        /// WARNING: HERE BE DRAGONS!
        /// There is a known issue with data annotation attributes in .NET 4.5.1
        /// The workound requires explicitly setting ErrorMessage = null
        /// http://connect.microsoft.com/VisualStudio/feedback/details/757298/emailaddress-attribute-is-unable-to-load-error-message-from-resource-mvc
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Activation), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Activation), ErrorMessageResourceName = "EmailPattern", ErrorMessage = null)]
        public string Email { get; set; }
    }
}
