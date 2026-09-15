using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers;

public class VolunteerController : Controller
{
    private readonly ApplicationDbContext _context;

    public VolunteerController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index() => View(new VolunteerSignup());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        [Bind("FullName,Email,Phone,Skills,Availability")] VolunteerSignup model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        model.SubmittedDate = DateTime.UtcNow;
        _context.VolunteerSignups.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ThankYou));
    }

    [HttpGet]
    public IActionResult ThankYou() => View();
}
