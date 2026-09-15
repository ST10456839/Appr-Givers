using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models;

public enum DonationCurrency
{
    ZAR,
    USD,
    EUR
}

public class Donation
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string DonorName { get; set; } = "Anonymous Donor";

    public bool IsAnonymous { get; set; }

    // Null for guest / anonymous donations.
    public string? ApplicationUserId { get; set; }

    [Range(1, 1000000)]
    public decimal Amount { get; set; }

    public DonationCurrency Currency { get; set; } = DonationCurrency.ZAR;

    public bool IsRecurring { get; set; }

    public DateTime DonationDate { get; set; } = DateTime.UtcNow;

    public string CertificateNumber { get; set; } = string.Empty;
}
