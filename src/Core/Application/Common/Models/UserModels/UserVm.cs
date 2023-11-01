using Application.Requests.Roles.Models;
using Shared.Contracts;

namespace Application.Common.Models.UserModels;

public class UserVm: IMustHaveTenant
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public Guid TenantId { get; set; }


    public List<RoleVm> RoleVms { get; set; } 
}