using Application.Requests.Users.Models;
using MediatR;
using Shared.ServiceContracts;

namespace Application.Common.Interfaces;

public interface IAuthService : IScopedService
{
    Task<IResponse<Unit>> SignInUserASync(LoginUserRequest loginUserRequest);
    Task<IResponse<Unit>> SignOutUserASync();
}