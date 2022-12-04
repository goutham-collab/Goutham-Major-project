using eMobileShop.Data;
using eMobileShop.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eMobileShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppConstants.ADMIN)]
public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.ToListAsync();
        return View(users);
    }

    public async Task<IActionResult> InquiryDetails()
    {
        var inquiries = await _context.InquiryDetail.ToListAsync();
        return View(inquiries);
    }
}
