using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models.ViewModels;

public class RegisterViewModel
{
    [Required, StringLength(100)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Prototype only: lets the user pick Donor or Employee at sign-up so both
    // roles are easy to demo. In production, Employee accounts would be
    // provisioned by an administrator instead of self-selected.
    [Required]
    [Display(Name = "Register as")]
    public string Role { get; set; } = "Donor";
}
