using Application.Requests.Roles.Models;
using Shared.ServiceContracts;

namespace Application.Common.Interfaces;

public interface IRoleService : IScopedService
{
    Task<List<RoleVm>> GetRolesAsync();
    Task<List<RoleVm>> GetRolesAsync(string userId);
    Task<RoleVm> GetRoleAsync(string roleId);

    Task<IResponse<string>> CreateRoleAsync(string roleName);
    Task<IResponse<string>> UpdateRoleAsync(string roleId, string roleName);
    Task DeleteRoleAsync(string roleId);

    Task ReplacePermissionsToRoleAsync(string roleId, List<string> permissions);
    Task<List<string>> GetRolePermissionsAsync(string roleId);
}