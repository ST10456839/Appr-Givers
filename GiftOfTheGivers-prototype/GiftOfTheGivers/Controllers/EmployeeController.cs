using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers;

[Authorize(Roles = "Employee")]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Employees can view volunteer sign-ups and recent donations, and post
    // updates on ongoing relief projects.
    public async Task<IActionResult> Dashboard()
    {
        ViewBag.Volunteers = await _context.VolunteerSignups
            .OrderByDescending(v => v.SubmittedDate)
            .ToListAsync();

        ViewBag.Updates = await _context.ProjectUpdates
            .OrderByDescending(u => u.PostedDate)
            .ToListAsync();

        ViewBag.Donations = await _context.Donations
            .OrderByDescending(d => d.DonationDate)
            .Take(10)
            .ToListAsync();

        return View();
    }

    [HttpGet]
    public IActionResult PostUpdate() => View(new ProjectUpdate());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostUpdate([Bind("Title,Content")] ProjectUpdate model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        model.PostedBy = user?.FullName ?? "Employee";
        model.PostedDate = DateTime.UtcNow;

        _context.ProjectUpdates.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Dashboard));
    }
}
