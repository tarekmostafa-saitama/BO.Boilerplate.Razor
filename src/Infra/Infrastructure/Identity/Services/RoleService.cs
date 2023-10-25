using System.Security.Claims;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Requests.Roles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<List<RoleVm>> GetRolesAsync()
    {
        return await _roleManager.Roles.Select(x => new RoleVm { Id = x.Id, Name = x.Name }).ToListAsync();
    }

    public async Task<List<RoleVm>> GetRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found with id= " + userId);

        var rolesNames = await _userManager.GetRolesAsync(user);
        return await _roleManager.Roles.Where(x => rolesNames.Contains(x.Name))
            .Select(x => new RoleVm { Id = x.Id, Name = x.Name }).ToListAsync();
    }

    public async Task<RoleVm> GetRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        return role == null
            ? throw new NotFoundException("Role not found with id= " + roleId)
            : new RoleVm { Id = roleId, Name = role.Name };
    }

    public async Task<IResponse<string>> CreateRoleAsync(string roleName)
    {
        var role = new IdentityRole(roleName);
        var result = await _roleManager.CreateAsync(role);
        return result.Succeeded
            ? Response.Success(role.Id)
            : Response.Fail<string>(result.Errors.Select(x => x.Description).ToList());
    }

    public async Task<IResponse<string>> UpdateRoleAsync(string roleId, string roleName)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role != null)
        {
            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded
                ? Response.Success(roleId)
                : Response.Fail<string>(result.Errors.Select(x => x.Description).ToList());
        }

        throw new NotFoundException("Role not found with id= " + roleId);
    }

    public async Task DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId) ??
                   throw new NotFoundException("Role not found with id= " + roleId);
        await _roleManager.DeleteAsync(role);
    }

    public async Task ReplacePermissionsToRoleAsync(string roleId, List<string> permissions)
    {
        var role = await _roleManager.FindByIdAsync(roleId) ??
                   throw new NotFoundException("Role not found with id= " + roleId);
        var claims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in claims) await _roleManager.RemoveClaimAsync(role, claim);

        foreach (var permission in permissions)
            await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
    }

    public async Task<List<string>> GetRolePermissionsAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId) ??
                   throw new NotFoundException("Role not found with id= " + roleId);
        var claims = await _roleManager.GetClaimsAsync(role);
        return claims.Select(x => x.Value).ToList();
    }
}