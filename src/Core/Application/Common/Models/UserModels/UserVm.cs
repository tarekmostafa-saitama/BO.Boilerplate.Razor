using Application.Requests.Roles.Models;

namespace Application.Common.Models.UserModels;

public class UserVm
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }


    public List<RoleVm> RoleVms { get; set; }
}