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
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, 
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
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
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("tenant",user.TenantId.ToString(),new CookieOptions(){Expires = DateTimeOffset.MaxValue});
            return Response.Success(Unit.Value);
        }
       
        return Response.Fail<Unit>("Failed login attempt");
    }

    public async Task<IResponse<Unit>> SignOutUserASync()
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("tenant");
        await _signInManager.SignOutAsync();
        return Response.Success(Unit.Value);
    }



    public async Task<IResponse<Unit>> ChangePasswordAsync(ChangePasswordVm changePasswordVm)
    {
        var id = _currentUserService.Id;
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return Response.Fail<Unit>("Failed while getting the user");

        // ChangePasswordAsync changes the user password
        var result = await _userManager.ChangePasswordAsync(user,
            changePasswordVm.CurrentPassword, changePasswordVm.NewPassword);

        if (!result.Succeeded)
            return Response.Fail<Unit>(result.Errors.Select(x => x.Description).ToList());

        await _signInManager.RefreshSignInAsync(user);
        return Response.Success(Unit.Value);

    }

    public async Task<IResponse<Unit>> AlterPasswordAsync(AlterPasswordVm alterPasswordVm)
    {
        var id = _currentUserService.Id;
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return Response.Fail<Unit>("Failed while getting the user");

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result =
            await _userManager.ResetPasswordAsync(user, token, alterPasswordVm.NewPassword);


        if (!result.Succeeded)
            return Response.Fail<Unit>(result.Errors.Select(x => x.Description).ToList());

        return Response.Success(Unit.Value);
    }

    public async Task<IResponse<Unit>> FastLogin(string id)
    {
        var  user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return Response.Fail<Unit>($"No user found with this id = {id}");

        await _signInManager.SignInAsync(user, true);
        return Response.Success(Unit.Value);

    }

    public async Task<IResponse<Unit>> ToggleUserActivation(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return Response.Fail<Unit>($"No user found with this id = {id}");
        user.IsLockedDown = !user.IsLockedDown;
        await _userManager.UpdateAsync(user);

        return Response.Success(Unit.Value);
    }
}