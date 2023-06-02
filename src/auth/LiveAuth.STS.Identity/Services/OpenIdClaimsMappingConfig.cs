using Duende.IdentityServer.Hosting.DynamicProviders;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;

namespace LiveAuth.STS.Identity.Services
{
    public class OpenIdClaimsMappingConfig : ConfigureAuthenticationOptions<OpenIdConnectOptions, OidcProvider>
    {
        public OpenIdClaimsMappingConfig(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        protected override void Configure(ConfigureAuthenticationContext<OpenIdConnectOptions, OidcProvider> context)
        {
            var oidcProvider = context.IdentityProvider;

            context.IdentityProvider.Properties.TryGetValue("MapInboundClaims", out var resultMapInboundClaims);

            var mapInboundClaims = resultMapInboundClaims == null || "true".Equals(resultMapInboundClaims);

            context.AuthenticationOptions.MapInboundClaims = mapInboundClaims;
        }
    }
}







