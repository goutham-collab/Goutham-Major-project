using eMobileShop.Data;
using eMobileShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using eMobileShop.Models.ViewModels;

namespace eMobileShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("/AddInquiry")]
        public IActionResult AddInquiry(InquiryModel model)
        {
            var inquiry = new Inquiry()
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message
            };

            _context.InquiryDetail.Add(inquiry);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> RecentProductList()
        {
            var recentProduct = await _context.Products
                .Include(x => x.Category)
                .Take(10)
                .OrderByDescending(x => x.Id)
                .ToListAsync();
            return Json(recentProduct);
        }


    }
}