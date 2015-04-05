using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Barricade;
using System.Threading.Tasks;
using Zombie.Services;

namespace Zombie.Controllers
{
    [Claim]
    public abstract class BaseApiController : ApiController, IClaimController
    {
        protected SiteContext Context { get; private set; }

        protected BaseApiController(SiteContext context)
        {
            Context = context;
        }

        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
            base.Dispose(disposing);
        }

        public async Task<IClaimUser> GetUserByAccessToken(string accessToken)
        {
            return await new UserService(Context).GetUserByAccessToken(accessToken, x => x.Claims);
        }

        public HttpResponseMessage ModelErrorResponse()
        {
            var errors = new List<string>();
            foreach (var kv in ModelState) errors.AddRange(kv.Value.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
            return BadRequestResponse(errors);
        }

        /// <summary>
        /// Creates an HttpResponseException with a <c>400</c> status and the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The content of the HTTP response message.</param>
        /// <param name="error">When <c>true</c> the <paramref name="value"/> is wrapped in an errors object.</param>
        /// <returns>An HttpResponseException.</returns>
        public HttpResponseException BadRequestException(object value = null, bool error = true)
        {
            return ResponseException(HttpStatusCode.BadRequest, value, error);
        }

        /// <summary>
        /// Creates an HttpResponseException with a <c>404</c> status and the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The content of the HTTP response message.</param>
        /// <param name="error">When <c>true</c> the <paramref name="value"/> is wrapped in an errors object.</param>
        /// <returns>An HttpResponseException.</returns>
        public HttpResponseException NotFoundException(object value = null, bool error = true)
        {
            return ResponseException(HttpStatusCode.NotFound, value, error);
        }

        /// <summary>
        /// Creates an HttpResponseException with the specified <paramref name="status"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="status">The HTTP response status code.</param>
        /// <param name="value">The content of the HTTP response message.</param>
        /// <param name="error">When <c>true</c> the <paramref name="value"/> is wrapped in an errors object.</param>
        /// <returns>An HttpResponseException.</returns>
        public HttpResponseException ResponseException(HttpStatusCode status, object value, bool error)
        {
            return new HttpResponseException(Response(status, value, error));
        }

        /// <summary>
        /// Creates an HttpResponseMessage with a <c>200</c> status and the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The content of the HTTP response message.</param>
        /// <returns>An HttpResponseException.</returns>
        public HttpResponseMessage OkResponse(object value = null)
        {
            return Response(HttpStatusCode.OK, value, false);
        }

        /// <summary>
        /// Creates an HttpResponseMessage with a <c>400</c> status and the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The content of the HTTP response message.</param>
        /// <param name="error">When <c>true</c> the <paramref name="value"/> is wrapped in an errors object.</param>
        /// <returns>An HttpResponseException.</returns>
        public HttpResponseMessage BadRequestResponse(object value = null, bool error = true)
        {
            return Response(HttpStatusCode.BadRequest, value, error);
        }

        /// <summary>
        /// Creates an HttpResponseMessage with a <c>404</c> status and the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The content of the HTTP response message.</param>
        /// <param name="error">When <c>true</c> the <paramref name="value"/> is wrapped in an errors object.</param>
        /// <returns>An HttpResponseException.</returns>
        public HttpResponseMessage NotFoundResponse(object value = null, bool error = true)
        {
            return Response(HttpStatusCode.NotFound, value, error);
        }

        /// <summary>
        /// Creates an HttpResponseMessage with a <c>503</c> status and the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The content of the HTTP response message.</param>
        /// <param name="error">When <c>true</c> the <paramref name="value"/> is wrapped in an errors object.</param>
        /// <returns>An HttpResponseException.</returns>
        public HttpResponseMessage ServiceUnavailableResponse(object value = null, bool error = true)
        {
            return Response(HttpStatusCode.ServiceUnavailable, value, error);
        }

        /// <summary>
        /// Creates an HttpResponseMessage with the specified <paramref name="status"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="status">The HTTP response status code.</param>
        /// <param name="value">The content of the HTTP response message.</param>
        /// <param name="error">When <c>true</c> the <paramref name="value"/> is wrapped in an errors object.</param>
        /// <returns>An HttpResponseMessage.</returns>
        public HttpResponseMessage Response(HttpStatusCode status, object value, bool error)
        {
            return value == null
                ? Request.CreateResponse(status)
                : Request.CreateResponse(status, error ? new { errors = value } : value);
        }
    }
}
