using eMobileShop.Data;
using eMobileShop.Helper;
using eMobileShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eMobileShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppConstants.ADMIN)]
public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ProductController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
    }

    // GET: Admin/Product
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Products.Include(p => p.Category);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Admin/Product/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Admin/Product/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description");
        return View();
    }

    // POST: Admin/Product/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Price,ProductColor,IsAvailable,CategoryId")] Product product, IFormFile image)
    {
        if (ModelState.IsValid)
        {
            if (image != null)
            {
                var fileName = DateTime.Now.Ticks.ToString() + image.FileName;
                var name = Path.Combine(_hostingEnvironment.WebRootPath + "/Images/Product", Path.GetFileName(fileName));
                await image.CopyToAsync(new FileStream(name, FileMode.Create));
                product.Image = "Images/Product/" + fileName;
            }

            if (product == null)
            {
                product.Image = "Images/noimage.PNG";
            }
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
        return View(product);
    }

    // GET: Admin/Product/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
        return View(product);
    }

    // POST: Admin/Product/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Image,ProductColor,IsAvailable,CategoryId")] Product product, IFormFile image)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (image != null)
                {
                    var fileName = DateTime.Now.Ticks.ToString() + image.FileName;
                    var name = Path.Combine(_hostingEnvironment.WebRootPath + "/Images/Product", Path.GetFileName(fileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "Images/Product/" + fileName;
                }

                if (product == null)
                {
                    product.Image = "Images/noimage.PNG";
                }
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
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
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
        return View(product);
    }

    // GET: Admin/Product/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Admin/Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Products == null)
        {
            return Problem("Entity set 'AppDbContext.Products'  is null.");
        }
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
      return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
