using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using LiveAuth.Admin.EntityFramework.Shared.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LiveAuth.STS.Identity.Services;

public class LiveProfileService : IProfileService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly RoleManager<UserIdentityRole> _roleManager;

    public LiveProfileService(
        UserManager<UserIdentity> userManager, 
        RoleManager<UserIdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>();
        
        claims.AddRange(context.Subject.FindAll(JwtClaimTypes.Name));
        claims.AddRange(context.Subject.FindAll(JwtClaimTypes.Email));
        claims.AddRange(context.Subject.FindAll(JwtClaimTypes.Role));
        

        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                claims.AddRange(await _roleManager.GetClaimsAsync(role));
            }
        }
        context.IssuedClaims.AddRange(claims);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}