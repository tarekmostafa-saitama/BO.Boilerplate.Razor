using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.UserModels;
using Application.Requests.Users.Models;
using k8s.KubeConfigModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResponse<Unit>> SignInUserASync(LoginUserRequest loginUserRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginUserRequest.Email);
        if (user == null)
            return Response.Fail<Unit>("Failed login attempt");
        var signInResult =
            await _signInManager.PasswordSignInAsync(user, loginUserRequest.Password, false, false);
        if (signInResult.Succeeded)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("tenant",user.TenantId.ToString(),new CookieOptions(){Expires = DateTimeOffset.MaxValue});
            return Response.Success(Unit.Value);
        }
       
        return Response.Fail<Unit>("Failed login attempt");
    }

    public async Task<IResponse<Unit>> SignOutUserASync()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("tenant");
        await _signInManager.SignOutAsync();
        return Response.Success(Unit.Value);
    }

    public async Task<IResponse<string>> CreateUserASync(CreateUserVm userRequest)
    {
        var user = new ApplicationUser
        {
            Email = userRequest.Email,
            FullName = userRequest.FullName
        };
        var createResult = await _userManager.CreateAsync(user, userRequest.Password);
        return createResult.Succeeded
            ? Response.Success(user.Id)
            : Response.Fail<string>(createResult.Errors.Select(error => error.Description).ToList());
    }
}