using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using MovieCritiqueAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebAPI.Models;

namespace MovieCritiqueAPI
{
	public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
	{  
		  public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
			//context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
			context.Validated();
        }
		public override Task MatchEndpoint(OAuthMatchEndpointContext context)
		{
			if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
			{
				context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
				context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
				context.RequestCompleted();
				return Task.FromResult(0);
			}

			return base.MatchEndpoint(context);
		}
		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
			
			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			//context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
			using (var db = new MovieCritiqueEntities())
			{
				if (db != null)
				{
					var user = db.Users.ToList();
					if (user != null)
					{
						if (!string.IsNullOrEmpty(user.Where(u => u.userName == context.UserName && u.password == context.Password).FirstOrDefault().userName))
						{
							var currentUser = user.Where(u => u.userName == context.UserName && u.password == context.Password).FirstOrDefault();
							identity.AddClaim(new Claim("Id", Convert.ToString(currentUser.userId)));
							identity.AddClaim(new Claim("userName", Convert.ToString(currentUser.userName)));
							identity.AddClaim(new Claim("password", Convert.ToString(currentUser.password)));
							identity.AddClaim(new Claim("LoggedOn",DateTime.Now.ToString()));




							var props = new AuthenticationProperties(new Dictionary<string, string>
{
{
"DisplayName", context.UserName
}
});
							var ticket = new AuthenticationTicket(identity, props);
							context.Validated(ticket);
						}
						else
						{
							context.SetError("invalid_grant", "Provided username and password is not matching, Please retry.");
							context.Rejected();
						}
					}
				}
				else
				{
					context.SetError("invalid_grant", "Provided username and password is not matching, Please retry.");
					context.Rejected();
				}
				return;
			}
		}
	}
}
    