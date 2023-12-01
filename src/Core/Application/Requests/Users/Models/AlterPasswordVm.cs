using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Users.Models;

public class AlterPasswordVm
{
    public string UserId { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [Display(Name = "users_NewPassword")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "This field is required.")]
    [Display(Name = "users_confirmPassword")]
    [Compare("NewPassword", ErrorMessage = "New Password and Confirm New Password don't match")]
    public string ConfirmPassword { get; set; }
}