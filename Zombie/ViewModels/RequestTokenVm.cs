using Barricade;
using Zombie.Models;

namespace Zombie.ViewModels
{
    public class RequestTokenVm : AccessTokenResponse
    {
        public UserConfigVm Config { get; set; }

        internal static RequestTokenVm MapFrom(AccessTokenResponse accessTokenResponse, User user)
        {
            return new RequestTokenVm {
                Config = UserConfigVm.MapFrom(user),
                access_token = accessTokenResponse.access_token,
                token_type = accessTokenResponse.token_type,
                expires_in = accessTokenResponse.expires_in
            };
        }
    }
}
