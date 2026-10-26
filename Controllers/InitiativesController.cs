
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouthHub.Data;
using YouthHub.Models;
using YouthHub.Services;
using YouthHub.ViewModels;

namespace YouthHub.Controllers;

public class InitiativesController : Controller
{
    private readonly IInitiativeService _initiativeService;
    private readonly ApplicationDbContext _context;

    public InitiativesController(
        IInitiativeService initiativeService,
        ApplicationDbContext context)
    {
        _initiativeService = initiativeService;
        _context = context;
    }

    public async Task<IActionResult> Index(
        string? searchString,
        int? categoryId)
    {
        var initiatives = await _initiativeService.SearchAsync(
            searchString,
            categoryId);

        ViewData["SearchString"] = searchString;
        ViewData["CategoryId"] = categoryId;

        ViewBag.Categories = new SelectList(
            await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync(),
            "Id",
            "Name",
            categoryId);

        return View(initiatives);
    }

    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();

        return View(new InitiativeFormViewModel
        {
            EventDate = DateTime.Today
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        InitiativeFormViewModel model)
    {
        if (model.EventDate.Date < DateTime.Today)
        {
            ModelState.AddModelError(
            nameof(model.EventDate),
            "Датата на инициативата не може да бъде в миналото.");
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(model);
        }

        if (model.CategoryId.HasValue &&
            !await _initiativeService.CategoryExistsAsync(
                model.CategoryId.Value))
        {
            ModelState.AddModelError(
                nameof(model.CategoryId),
                "Избраната категория не съществува.");

            await LoadCategoriesAsync();
            return View(model);
        }

        var initiative = new Initiative
        {
            Title = model.Title,
            Description = model.Description,
            Location = model.Location,
            EventDate = model.EventDate,
            Organizer = model.Organizer,
            CategoryId = model.CategoryId
        };

        await _initiativeService.AddAsync(initiative);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var initiative = await _initiativeService.GetByIdAsync(
            id.Value);

        if (initiative == null)
        {
            return NotFound();
        }

        await LoadCategoriesAsync();

        var model = new InitiativeFormViewModel
        {
            Id = initiative.Id,
            Title = initiative.Title,
            Description = initiative.Description,
            Location = initiative.Location,
            EventDate = initiative.EventDate,
            Organizer = initiative.Organizer,
            CategoryId = initiative.CategoryId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        InitiativeFormViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var existingInitiative =
            await _initiativeService.GetByIdAsync(id);

        if (existingInitiative == null)
        {
            return NotFound();
        }

        if (model.EventDate.Date < DateTime.Today)
        {
            ModelState.AddModelError(
                nameof(model.EventDate),
                "Датата на инициативата не може да бъде в миналото.");
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(model);
        }

        if (model.CategoryId.HasValue &&
            !await _initiativeService.CategoryExistsAsync(
                model.CategoryId.Value))
        {
            ModelState.AddModelError(
                nameof(model.CategoryId),
                "Избраната категория не съществува.");

            await LoadCategoriesAsync();
            return View(model);
        }

        existingInitiative.Title = model.Title;
        existingInitiative.Description = model.Description;
        existingInitiative.Location = model.Location;
        existingInitiative.EventDate = model.EventDate;
        existingInitiative.Organizer = model.Organizer;
        existingInitiative.CategoryId = model.CategoryId;

        await _initiativeService.UpdateAsync(existingInitiative);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var initiative = await _initiativeService.GetByIdAsync(
            id.Value);

        if (initiative == null)
        {
            return NotFound();
        }

        return View(initiative);
    }


[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var initiative = await _initiativeService.GetByIdAsync(id);

    if (initiative == null)
    {
        return NotFound();
    }

    var hasApplications = await _context.Applications
        .AnyAsync(a => a.InitiativeId == id);

    if (hasApplications)
    {
        TempData["ErrorMessage"] =
            "Инициативата не може да бъде изтрита, защото има подадени кандидатури.";

        return RedirectToAction(nameof(Delete), new { id });
    }

    await _initiativeService.DeleteAsync(initiative);

    return RedirectToAction(nameof(Index));
}


    private async Task LoadCategoriesAsync()
    {
        ViewBag.Categories = new SelectList(
            await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync(),
            "Id",
            "Name");
    }
}
