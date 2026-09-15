using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models.ViewModels;

public class DonationViewModel
{
    [StringLength(100)]
    [Display(Name = "Your Name (optional if anonymous)")]
    public string? DonorName { get; set; }

    [Display(Name = "Donate anonymously")]
    public bool IsAnonymous { get; set; }

    [Required]
    [Range(10, 1000000, ErrorMessage = "Please enter an amount of at least 10.")]
    public decimal Amount { get; set; } = 100;

    [Required]
    [Display(Name = "Currency")]
    public DonationCurrency Currency { get; set; } = DonationCurrency.ZAR;

    [Display(Name = "Make this a recurring monthly donation")]
    public bool IsRecurring { get; set; }
}
