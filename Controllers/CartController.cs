using eMobileShop.Data;
using eMobileShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eMobileShop.Controllers;

public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ILogger<CartController> logger, AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CartMasterDisplay()
    {        
        var cartProducts = _context.CartDetails
            .Where(x => x.UserId == _userManager.GetUserId(User))
            .Include(x => x.Product)            
            .ToList();
        return Json(cartProducts);
    }

    [HttpPost]
    public IActionResult AddToCart(int productID, int qty)
    {
        var cart = _context.CartDetails.Where(x => x.UserId == _userManager.GetUserId(User) && x.ProductId == productID).FirstOrDefault();
        if (cart == null)
        {

            CartDetails newCart = new CartDetails();
            newCart.ProductId = productID;
            newCart.Qty = qty;
            newCart.UserId = _userManager.GetUserId(User);
            _context.CartDetails.Add(newCart);
        } else
        {
            cart.Qty += qty;
            _context.CartDetails.Update(cart);
        }
        _context.SaveChanges();
        return Json(new { message = "Item added!"});
    }

    [HttpPut]
    public IActionResult UpdateToCart(int cartId, int qty)
    {
        var cart = _context.CartDetails.FirstOrDefault(x => x.Id == cartId);
        if (cart == null)
            return Json(new { message = "Item not found" });

        cart.Qty = qty;        
        _context.CartDetails.Update(cart);
        _context.SaveChanges();
        return Json(new { message = "Item changed!" });
    }

    [HttpDelete]
    public IActionResult DeleteCart(int cartId)
    {
        var cart = _context.CartDetails.FirstOrDefault(x => x.Id == cartId);
        if (cart == null)
            return Json(new { message = "Item not found" });
        
        _context.CartDetails.Remove(cart);
        _context.SaveChanges();
        return Json(new { message = "Item deleted!" });
    }
}
