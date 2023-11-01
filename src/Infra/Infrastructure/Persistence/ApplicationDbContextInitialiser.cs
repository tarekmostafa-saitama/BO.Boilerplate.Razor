using System.Security.Claims;
using Application.Common.Tenants;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Permissions;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TenantSettings _tenantSettings;


    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager, IOptions<TenantSettings> tenantSettings)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _tenantSettings = tenantSettings.Value;
    }


    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var adminRole = new IdentityRole(DefaultRoles.Admin);

        if (_roleManager.Roles.All(r => r.Name != adminRole.Name))
            await _roleManager.CreateAsync(adminRole);
        else
            adminRole = await _roleManager.FindByNameAsync(adminRole.Name);

        // Default users
        var administrator = new ApplicationUser
            { FullName = "Admin", UserName = "admin@email.com", Email = "admin@email.com", EmailConfirmed = true, TenantId = _tenantSettings.Default.Id};

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "admin@email.com");
            if (!string.IsNullOrWhiteSpace(adminRole.Name))
                await _userManager.AddToRolesAsync(administrator, new[] { adminRole.Name });
        }
        else
        {
            administrator = await _userManager.FindByNameAsync(administrator.UserName);
            if (!await _userManager.IsInRoleAsync(administrator, adminRole.Name))
                await _userManager.AddToRolesAsync(administrator, new[] { adminRole.Name });
        }

        var allAdminRoleClaims = await _roleManager.GetClaimsAsync(adminRole);
        foreach (var claim in allAdminRoleClaims) await _roleManager.RemoveClaimAsync(adminRole, claim);
        foreach (var permission in Permissions.Admin)
            await _roleManager.AddClaimAsync(adminRole, new Claim("Permission", permission.Name));
    }
}