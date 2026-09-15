using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GiftOfTheGivers.Controllers;

public class DonateController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DonateController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Donations are open to everyone - logged-in Donors, or guests choosing
    // to give anonymously, per the brief.
    [HttpGet]
    public IActionResult Index() => View(new DonationViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DonationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = User.Identity?.IsAuthenticated == true
            ? await _userManager.GetUserAsync(User)
            : null;

        var donorName = model.IsAnonymous
            ? "Anonymous Donor"
            : (!string.IsNullOrWhiteSpace(model.DonorName)
                ? model.DonorName
                : user?.FullName ?? "Anonymous Donor");

        var donation = new Donation
        {
            DonorName = donorName,
            IsAnonymous = model.IsAnonymous,
            ApplicationUserId = user?.Id,
            Amount = model.Amount,
            Currency = model.Currency,
            IsRecurring = model.IsRecurring,
            DonationDate = DateTime.UtcNow,
            CertificateNumber = $"GOTG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpperInvariant()}"
        };

        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ThankYou), new { id = donation.Id });
    }

    [HttpGet]
    public async Task<IActionResult> ThankYou(int id)
    {
        var donation = await _context.Donations.FindAsync(id);
        if (donation is null)
        {
            return NotFound();
        }

        return View(donation);
    }

    // Generates a placeholder (non-legal) tax certificate PDF for the donation,
    // as required by the brief.
    [HttpGet]
    public async Task<IActionResult> Certificate(int id)
    {
        var donation = await _context.Donations.FindAsync(id);
        if (donation is null)
        {
            return NotFound();
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Column(col =>
                {
                    col.Item().Text("Gift of the Givers Foundation").FontSize(20).Bold();
                    col.Item().Text("Placeholder Tax Certificate (Prototype)").FontSize(12).Italic();
                });

                page.Content().PaddingVertical(20).Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text($"Certificate Number: {donation.CertificateNumber}");
                    col.Item().Text($"Date: {donation.DonationDate:dd MMMM yyyy}");
                    col.Item().Text($"Donor: {donation.DonorName}");
                    col.Item().Text($"Amount: {donation.Amount:0.00} {donation.Currency}");
                    col.Item().Text($"Type: {(donation.IsRecurring ? "Recurring (Monthly)" : "One-time")}");
                    col.Item().PaddingTop(20).Text(
                            "This is a placeholder certificate generated for prototype purposes only. " +
                            "It does not constitute a valid tax document.")
                        .Italic().FontSize(10);
                });

                page.Footer().AlignCenter().Text("Gift of the Givers Foundation - Web Application Prototype");
            });
        });

        var pdfBytes = document.GeneratePdf();
        return File(pdfBytes, "application/pdf", $"TaxCertificate_{donation.CertificateNumber}.pdf");
    }
}
