using Application.Requests.Users.Commands;
using Application.Requests.Users.Models;
using FormHelper;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace UI.Razor.Controllers;

public class AccountController : Controller
{
    private readonly ISender _sender;
    private readonly IStringLocalizer<AccountController> _stringLocalizer;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        ISender sender, IStringLocalizer<AccountController> stringLocalizer,
        UserManager<ApplicationUser> userManager)
    {
        _sender = sender;
        _stringLocalizer = stringLocalizer;
        _userManager = userManager;
    }

    [HttpGet("Account/Login")]
    public async Task<IActionResult> Login()
    {
        if (User.Identity is { IsAuthenticated: true }) return RedirectToAction("Home", "Home", new { area = "Admin" });

        return View();
    }

    [Route("Account/Login")]
    [HttpPost]
    [FormValidator]
    public async Task<IActionResult> Login(LoginUserRequest model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var result = await _sender.Send(new LoginUserCommand(model));
            if (result.Succeeded)
                return FormResult.CreateSuccessResult(_stringLocalizer["loginPage_successLogin"].Value,
                    Url.Action("Home", "Home", new { area = "Admin" }));

            ModelState.AddModelError("", result.Errors[0]);
        }

        return View(model);
    }


    [Route("Account/Logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _sender.Send(new LogOutCommand());
        return RedirectToAction(nameof(Login));
    }
}