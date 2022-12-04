using eMobileShop.Data;
using eMobileShop.Helper;
using eMobileShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eMobileShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppConstants.ADMIN)]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public CategoryController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
    }

    // GET: Admin/Category
    public async Task<IActionResult> Index()
    {
          return _context.Categories != null ? 
                      View(await _context.Categories.ToListAsync()) :
                      Problem("Entity set 'AppDbContext.Categories'  is null.");
    }

    // GET: Admin/Category/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Categories == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // GET: Admin/Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Category/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] Category category, IFormFile logo)
    {
        if (ModelState.IsValid)
        {
            if (logo != null)
            {
                var fileName = DateTime.Now.Ticks.ToString() + logo.FileName;
                var name = Path.Combine(_hostingEnvironment.WebRootPath + "/Images/Category", Path.GetFileName(fileName));
                await logo.CopyToAsync(new FileStream(name, FileMode.Create));
                category.Logo = "Images/Category/" + fileName;
            }

            if (logo == null)
            {
                category.Logo = "Images/noimage.PNG";
            }
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // GET: Admin/Category/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Categories == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // POST: Admin/Category/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category, IFormFile logo)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (logo != null)
                {
                    var fileName = DateTime.Now.Ticks.ToString() + logo.FileName;
                    var name = Path.Combine(_hostingEnvironment.WebRootPath + "/Images/Category", Path.GetFileName(fileName));
                    await logo.CopyToAsync(new FileStream(name, FileMode.Create));
                    category.Logo = "Images/Category/" + fileName;
                }

                if (logo == null)
                {
                    category.Logo = "Images/noimage.PNG";
                }
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // GET: Admin/Category/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Categories == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: Admin/Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Categories == null)
        {
            return Problem("Entity set 'AppDbContext.Categories'  is null.");
        }
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
      return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
