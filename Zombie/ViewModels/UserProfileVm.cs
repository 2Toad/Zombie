using Zombie.Models;

namespace Zombie.ViewModels
{
    public class UserProfileVm
    {
        public string Username { get; set; }
        public LanguageVm Language { get; set; }

        internal static UserProfileVm MapFrom(User user)
        {
            return new UserProfileVm {
                Username = user.Username,
                Language = LanguageVm.MapFrom(user.Language)
            };
        }
    }
}
