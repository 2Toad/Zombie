using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Barricade;
using Zombie.Models;
using Zombie.Services;
using Zombie.ViewModels;

namespace Zombie.Controllers
{
    [RoutePrefix("api/oauth")]
    public class OAuthController : BaseApiController
    {
        private readonly IUserService _userService;

        public OAuthController(IUserService userService, SiteContext siteContext)
            : base(siteContext)
        {
            _userService = userService;
        }

        /// <summary>
        /// Client submits credentials required for authentication. Upon
        /// success an access token is returned.
        /// </summary>
        /// <remarks>
        /// Resource Owner Password Credentials Grant
        /// http://tools.ietf.org/html/rfc6749#section-4.3
        /// </remarks>
        /// <param name="accessTokenRequest">The access token request.</param>
        /// <returns>The access token upon success; otherwise status code <c>404</c>.</returns>
        [HttpPost, Route("token"), AllowAnonymous]
        public async Task<HttpResponseMessage> RequestToken(AccessTokenRequest accessTokenRequest)
        {
            SecurityContext.MitigateBruteForceAttacks();

            var user = await _userService.Get(accessTokenRequest.Username, x => x.Language);
            if (user == null || !SecurityContext.ValidatePassword(user, accessTokenRequest))
                throw NotFoundException(Resources.Security.UnknownUsernamePassword);

            ValidateUserAccess(user);
            SecurityContext.Logout(user.AccessToken);

            user.AccessToken = SecurityContext.GenerateAccessToken();
            user.AccessTokenExpiration = DateTime.UtcNow.AddDays(7);
            await Context.SaveChangesAsync();

            var accessTokenResponse = SecurityContext.Login(user);
            return OkResponse(RequestTokenVm.MapFrom(accessTokenResponse, user));
        }

        /// <summary>
        /// Invalidates an access token, and removes it from the application cache.
        /// </summary>
        /// <returns>Status code <c>200</c> upon success; otherwise status code <c>404</c>.</returns>
        [HttpDelete, Route("invalidate")]
        public async Task<HttpResponseMessage> InvalidateToken()
        {
            var accessToken = SecurityContext.GetAccessToken(Request.Headers.Authorization);
            if (accessToken == null) return NotFoundResponse();

            var user = await _userService.GetUserByAccessToken(accessToken);
            if (user == null) return NotFoundResponse();

            SecurityContext.Logout(accessToken);

            user.AccessToken = null;
            user.AccessTokenExpiration = null;
            await Context.SaveChangesAsync();

            return OkResponse();
        }

        /// <summary>
        /// Additional, non-OAuth, validation checks are performed within this method. 
        /// </summary>
        /// <remarks>An HttpResponseException should be thrown to prevent access.</remarks>
        /// <param name="user">The user to validate.</param>
        private void ValidateUserAccess(User user)
        {
            if (user.ActivationCode != null) throw BadRequestException(new
            {
                Activation = new
                {
                    Required = true,
                    Code = user.ActivationCode,
                    user.Email
                }
            }, false);
        }
    }
}
