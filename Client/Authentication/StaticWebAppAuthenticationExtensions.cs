using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Authentication
{
    public static class StaticWebAppAuthenticationExtensions
    {
        public static IServiceCollection AddStaticWebAppAuthentication(this IServiceCollection services)
        {
            return services
                .AddAuthorizationCore()
                .AddScoped<AuthenticationStateProvider, StaticWebAppAuthenticationStateProvider>();
        }
    }
}
