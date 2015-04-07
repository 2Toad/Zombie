using Zombie.Models;

namespace Zombie.ViewModels
{
    public class UserProfileVm
    {
        public string Username { get; set; }
        public string Locale { get; set; }

        internal static UserProfileVm MapFrom(User user)
        {
            return new UserProfileVm {
                Username = user.Username,
                Locale = user.Language.Locale
            };
        }
    }
}
