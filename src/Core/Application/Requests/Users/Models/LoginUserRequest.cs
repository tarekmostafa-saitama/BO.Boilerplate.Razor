using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Users.Models;

public class LoginUserRequest
{
    [Display(Name = "loginPage_email")] public string Email { get; set; }

    [Display(Name = "loginPage_password")] public string Password { get; set; }
}