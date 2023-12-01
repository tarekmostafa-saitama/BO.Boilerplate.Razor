using Application.Requests.Users.Models;
using MediatR;
using Shared.ServiceContracts;

namespace Application.Common.Interfaces;

public interface IAuthService : IScopedService
{
    Task<IResponse<Unit>> SignInUserASync(LoginUserRequest loginUserRequest);
    Task<IResponse<Unit>> SignOutUserASync();
    Task<IResponse<Unit>> ChangePasswordAsync(ChangePasswordVm changePasswordVm);
    Task<IResponse<Unit>> AlterPasswordAsync(AlterPasswordVm alterPasswordVm); 
    Task<IResponse<Unit>> FastLogin(string id);
    Task<IResponse<Unit>> ToggleUserActivation(string id);

}