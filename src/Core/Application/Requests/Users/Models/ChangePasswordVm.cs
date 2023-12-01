using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Users.Models;

public class ChangePasswordVm
{

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "requiredField")]
    [Display(Name = "currentPassword")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "requiredField")]
    [Display(Name = "newPassword")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "requiredField")]
    [Display(Name = "confirmNewPassword")]
    [Compare("NewPassword", ErrorMessage = "New  Password and Confirm New Password don't match")]
    public string ConfirmPassword { get; set; }
}