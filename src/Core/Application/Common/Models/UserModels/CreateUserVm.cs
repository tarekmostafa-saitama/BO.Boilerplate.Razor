using Application.Requests.Roles.Models;

namespace Application.Common.Models.UserModels;

public class CreateUserVm : UserVm
{
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

}