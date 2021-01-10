using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Vorlesung.Client.Authentication
{
    public class StaticWebAppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _http;

        public StaticWebAppAuthenticationStateProvider(IConfiguration config, IWebAssemblyHostEnvironment environment)
        {
            _config = config;
            _http = new HttpClient { BaseAddress = new Uri(environment.BaseAddress) };
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var url = _config.GetValue<string>("StaticWebAppAuthentication:AuthenticationDataUrl", "/.auth/me");
                var data = await _http.GetFromJsonAsync<AuthenticationData>(url);
                var principal = data.ClientPrincipal;

                principal.UserRoles = principal.UserRoles.Except(new string[] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase);

                if (!principal.UserRoles.Any())
                {
                    return new AuthenticationState(new ClaimsPrincipal());
                }
                else
                {
                    var identity = new ClaimsIdentity(principal.IdentityProvider);

                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
                    identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails));
                    identity.AddClaims(principal.UserRoles.Select(x => new Claim(ClaimTypes.Role, x)));

                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }
    }
}
