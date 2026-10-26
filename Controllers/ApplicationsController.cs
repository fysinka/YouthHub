
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouthHub.Data;
using YouthHub.Models;
using YouthHub.ViewModels;

namespace YouthHub.Controllers;

public class ApplicationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ApplicationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Create(int initiativeId)
    {
        var initiative = await _context.Initiatives
            .FindAsync(initiativeId);

        if (initiative == null)
        {
            return NotFound();
        }

        ViewBag.InitiativeTitle = initiative.Title;

        return View(new ApplicationFormViewModel
        {
            InitiativeId = initiativeId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ApplicationFormViewModel model)
    {
        var initiative = await _context.Initiatives
            .FindAsync(model.InitiativeId);

        if (initiative == null)
        {
            return NotFound();
        }

        model.Email = model.Email?.Trim() ?? string.Empty;

        if (!ModelState.IsValid)
        {
            ViewBag.InitiativeTitle = initiative.Title;
            return View(model);
        }

        var email = model.Email.ToLower();

        var alreadyApplied = await _context.Applications
            .AnyAsync(a =>
                a.InitiativeId == model.InitiativeId &&
                a.Email.ToLower() == email);

        if (alreadyApplied)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Вече има кандидатура с този имейл за инициативата.");

            ViewBag.InitiativeTitle = initiative.Title;
            return View(model);
        }

        var application = new Application
        {
            ApplicantName = model.ApplicantName.Trim(),
            Email = model.Email,
            InitiativeId = model.InitiativeId,
            AppliedOn = DateTime.UtcNow
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Initiatives");
    }

    public async Task<IActionResult> Index()
    {
        var applications = await _context.Applications
            .Include(a => a.Initiative)
            .OrderByDescending(a => a.AppliedOn)
            .ToListAsync();

        return View(applications);
    }
}
