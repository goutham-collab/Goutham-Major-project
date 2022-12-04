using eMobileShop.Data;
using eMobileShop.Models;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eMobileShop.Controllers;

public class ShopController : Controller
{
    private readonly ILogger<ShopController> _logger;
    private readonly AppDbContext _context;

    public ShopController(ILogger<ShopController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ProductList(int pageIndex, string categoryList)
    {
        var predicate = BuildFilterExpression(categoryList);

        var products = await _context.Products
                    .Include(x => x.Category)
                    .Where(predicate)
                    .OrderByDescending(x => x.Id)
                    .Skip((pageIndex - 1) * 10).Take(10)
                    .ToListAsync();
        return Json(products);
    }

    private Expression<Func<Product, bool>> BuildFilterExpression(string categoryList) {
        var categoryIds = categoryList != null ? categoryList.Split(',') : new string[0];
        var predicate = PredicateBuilder.New<Product>(true);
        if (categoryIds.Length > 0) {
            predicate.And(x => categoryIds.Contains(x.CategoryId.ToString()));
        }
        return predicate;
    }

    public async Task<IActionResult> ListAllCategory()
    {
        var categories = await _context.Categories                    
                    .OrderBy(x => x.Name)
                    .ToListAsync();
        return Json(categories);
    }

    [HttpGet]
    public async Task<IActionResult> GetSearchedProduct(string search)
    {        
        var products = await _context.Products
                    .Include(x => x.Category)
                    .Where(x => x.Name.ToLower().Contains(search))
                    .OrderByDescending(x => x.Id)                    
                    .ToListAsync();
        return Json(products);
    }
}
