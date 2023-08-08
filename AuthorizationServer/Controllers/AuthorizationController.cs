using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using AuthorizationServer.Constants;

namespace AuthorizationServer.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpPost("~/connect/token")]
        public IActionResult Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException(Message.INVALID_REQ);

            ClaimsPrincipal claimsPrincipal;

            if (request.IsClientCredentialsGrantType())
            {
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());
 
                identity.AddClaim(new Claim("some-claim", "some-value", OpenIddictConstants.Destinations.AccessToken));

                claimsPrincipal = new ClaimsPrincipal(identity);

                claimsPrincipal.SetScopes(request.GetScopes());
            }

            else
            {
                throw new InvalidOperationException(Message.TYPE_NOT_SUPPORTED);
            }
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}
