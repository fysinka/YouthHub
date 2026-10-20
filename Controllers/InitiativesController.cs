using Microsoft.EntityFrameworkCore;
using YouthHub.Data;
using YouthHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YouthHub.Controllers;

public class InitiativesController : Controller
{
    private readonly ApplicationDbContext _context;

    public InitiativesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
{
    var initiatives = await _context.Initiatives
        .Include(i => i.Category)
        .ToListAsync();

    return View(initiatives);
}

public async Task<IActionResult> Create()
{
    await LoadCategoriesAsync();
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Initiative initiative)
{
    if (!ModelState.IsValid)
    {
            await LoadCategoriesAsync();
        return View(initiative);
    }

    _context.Initiatives.Add(initiative);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}



public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var initiative = await _context.Initiatives.FindAsync(id);

    if (initiative == null)
    {
        return NotFound();
    }

    await LoadCategoriesAsync();
    return View(initiative);
}


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Initiative initiative)
{
    if (id != initiative.Id)
    {
        return NotFound();
    }

    if (!ModelState.IsValid)
{
    await LoadCategoriesAsync();
    return View(initiative);
}

    _context.Update(initiative);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}

public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var initiative = await _context.Initiatives
        .FirstOrDefaultAsync(i => i.Id == id);

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
    var initiative = await _context.Initiatives.FindAsync(id);

    if (initiative != null)
    {
        _context.Initiatives.Remove(initiative);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}
private async Task LoadCategoriesAsync()
{
    ViewBag.Categories = new SelectList(
        await _context.Categories.ToListAsync(),
        "Id",
        "Name");
}

}
