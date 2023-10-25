using Application.Common.Models.UserModels;
using Shared.Models.PaginateModels;
using Shared.ServiceContracts;

namespace Application.Common.Interfaces;

public interface IUserService : IScopedService
{
    Task<PaginateResponseModel<UserVm>> GetUsersAsync(PaginateModel<string> model);
    Task<PaginateResponseModel<UserVm>> GetUsersInRoleAsync(PaginateModel<string> model);
    Task<UserVm> GetUserByIdAsync(string userId);
    Task<UserVm> GetUserByEmailAsync(string email);

    Task<IResponse<string>> CreateUserAsync(CreateUserVm userVm);
    Task<IResponse<string>> UpdateUserAsync(string userId, UpdateUserVm userVm);
    Task DeleteUserAsync(string userId);

    Task ReplaceRolesToUserAsync(string userId, List<string> roles);
}