using Zombie.Models;

namespace Zombie.ViewModels
{
    public class UserConfigVm
    {
        public UserProfileVm Profile { get; set; }

        internal static UserConfigVm MapFrom(User user)
        {
            return new UserConfigVm {
                Profile = UserProfileVm.MapFrom(user)
            };
        }
    }
}
