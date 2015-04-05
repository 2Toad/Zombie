using System.Net.Http;
using System.Web.Http;
using Barricade;
using System.Threading.Tasks;
using Zombie.Email;
using Zombie.Models;
using Zombie.Services;
using Zombie.ViewModels;

namespace Zombie.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        private readonly IClaimService _claimService;
        private readonly IUserService _userService;

        public UserController(IClaimService claimService, IUserService userService, SiteContext context) 
            : base(context)
        {
            _claimService = claimService;
            _userService = userService;
        }

        /// <summary>
        /// Gets the profile of the user making the request.
        /// </summary>
        /// <returns>The user's profile.</returns>
        [HttpGet, Route("profile")]
        public async Task<HttpResponseMessage> GetCurrentProfile()
        {
            var accessToken = SecurityContext.GetAccessToken(Request.Headers.Authorization);
            var user = await _userService.GetUserByAccessToken(accessToken, x => x.Language);
            return user != null ? OkResponse(UserConfigVm.MapFrom(user)) : NotFoundResponse();
        }

        [HttpGet, Route("usernameExists"), AllowAnonymous]
        public async Task<HttpResponseMessage> UsernameExists(string username)
        {
            return OkResponse(new { Exists = await _userService.UsernameExists(username) });
        }

        [HttpPost, Route("register"), AllowAnonymous]
        public async Task<HttpResponseMessage> Register(RegistrationPostVm registrationPost)
        {
            if (!ModelState.IsValid) return ModelErrorResponse();
            if (await _userService.UsernameExists(registrationPost.Username)) return BadRequestResponse(Resources.Registration.UsernameExists);

            var user = registrationPost.MapToUser();
            try
            {
                var claim = await _claimService.Get("Role", "User");
                _userService.Register(user, registrationPost.Password, claim);
                await Context.SaveChangesAsync();
            }
            catch
            {
                return ServiceUnavailableResponse(Resources.General.PleaseTryAgain);
            }

            await SendActivationEmail(user);

            return OkResponse(new { user.ActivationCode });
        }

        [HttpPut, Route("changeActivationEmail"), AllowAnonymous]
        public async Task<HttpResponseMessage> ChangeActivationEmail(ChangeActivationEmailPostVm activationPost)
        {
            if (!ModelState.IsValid) return ModelErrorResponse();

            User user;
            try
            {
                user = await _userService.ChangeActivationEmail(activationPost.Code, activationPost.Email);
                if (user == null) return BadRequestResponse(Resources.Activation.CodeUnknown);

                await Context.SaveChangesAsync();
            }
            catch
            {
                return ServiceUnavailableResponse(Resources.General.PleaseTryAgain);
            }

            await SendActivationEmail(user);

            return OkResponse(new { user.ActivationCode });
        }

        [HttpGet, Route("resendActivationEmail"), AllowAnonymous]
        public async Task<HttpResponseMessage> ResendActivationEmail(string activationCode)
        {
            User user;
            try
            {
                user = await _userService.RenewActivationCode(activationCode);
                if (user == null) return BadRequestResponse(Resources.Activation.CodeUnknown);

                await Context.SaveChangesAsync();
            }
            catch
            {
                return ServiceUnavailableResponse(Resources.General.PleaseTryAgain);
            }

            if (!await SendActivationEmail(user))
            {
                return ServiceUnavailableResponse(new {
                    Errors = Resources.General.PleaseTryAgain,
                    user.ActivationCode
                }, false);
            }

            return OkResponse(new { user.ActivationCode });
        }

        [HttpGet, Route("activateAccount"), AllowAnonymous]
        public async Task<HttpResponseMessage> ActivateAccount(string activationCode)
        {
            try
            {
                var success = await _userService.ActivateAccount(activationCode);
                if (!success) return BadRequestResponse(Resources.Activation.CodeUnknown);

                await Context.SaveChangesAsync();
            }
            catch
            {
                return ServiceUnavailableResponse(Resources.General.PleaseTryAgain);
            }

            return OkResponse();
        }

        private async Task<bool> SendActivationEmail(User user)
        {
            var siteUrl = Url.Link("Default", new {
                controller = "Home",
                action = "Index"
            });

            try
            {
                await Smtp.Send(
                    Config.SupportEmailAddress,
                    user.Email,
                    Resources.Activation.EmailTemplateSubject,
                    "Activation",
                    Handlebars.New("username", user.Username),
                    Handlebars.New("activation_url", siteUrl + "accountActivation?activationCode=" + user.ActivationCode)
                );

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
