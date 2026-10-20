
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouthHub.Data;
using YouthHub.Models;

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

        return View(new Application
        {
            InitiativeId = initiativeId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Application application)
    {
        var initiative = await _context.Initiatives
            .FindAsync(application.InitiativeId);

        if (initiative == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.InitiativeTitle = initiative.Title;
            return View(application);
        }

        application.AppliedOn = DateTime.UtcNow;

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return RedirectToAction(
            "Index",
            "Initiatives");
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

