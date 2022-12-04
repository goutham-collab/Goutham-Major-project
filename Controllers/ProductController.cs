using eMobileShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eMobileShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly AppDbContext _context;

        public ProductController(ILogger<ProductController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/product/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpGet("/product/ProductListByCategory")]
        public async Task<IActionResult> ProductListByCategory([FromQuery]int product,[FromQuery]int id)
        {
            var products = await _context.Products
                .Include(x => x.Category)
                .Where(x => x.Id != product && x.CategoryId == id)
                .ToListAsync();
            return Json(products);
        }
    }
}
