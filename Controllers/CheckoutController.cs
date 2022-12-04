using eMobileShop.Data;
using eMobileShop.Helper;
using eMobileShop.Models;
using eMobileShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace eMobileShop.Controllers;

[Authorize(Roles = AppConstants.USER)]
public class CheckoutController : Controller
{
    private readonly ILogger<CheckoutController> _logger;
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckoutController(ILogger<CheckoutController> logger, AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomerShippingDetail()
    {
        var shippingDetail = _context.Orders.Where(x => x.UserId == _userManager.GetUserId(User))
            .OrderByDescending(x => x.Id).Select(x => new ShippingVM() { 
                Address = x.Address,
                Name = x.Name,
                Email = x.Email,
                PhoneNo = x.PhoneNo,
                Zip = x.ZipCode
            }).FirstOrDefault();

        if (shippingDetail == null)
        {
            var user = await _userManager.GetUserAsync(User);
            shippingDetail = new ShippingVM()
            {                
                Name = user.FullName,
                Email = user.Email,
                Address = user.Address,
                PhoneNo = user.PhoneNumber
            };            
        }

        return Json(shippingDetail);
    }

    [HttpPost]
    public IActionResult AddOrder(ShippingVM shippingDetail)
    {
        try
        {
            var cart = _context.CartDetails
                .Include(x => x.Product)
                .Where(x => x.UserId == _userManager.GetUserId(User)).ToList();
            
            var orderNo = _context.Orders.OrderByDescending(x => x.Id).FirstOrDefault()?.OrderNo;            
            if (orderNo == null)
                orderNo = "EMS1";
            else
                orderNo = $"EMS{Convert.ToInt32(orderNo.Substring(3)) + 1}";

            var order = new Order();
            order.OrderNo = orderNo;
            order.OrderDate = DateTime.Now;
            order.Name = shippingDetail.Name;
            order.Email = shippingDetail.Email;
            order.PhoneNo = shippingDetail.PhoneNo;
            order.Address = shippingDetail.Address;
            order.ZipCode = shippingDetail.Zip;
            order.UserId = _userManager.GetUserId(User);

            var orderDetailList = new List<OrderDetail>();
            cart.ForEach(x =>
            {
                orderDetailList.Add(new OrderDetail()
                {
                    ProductId = x.ProductId,
                    Price = x.Product.Price,
                    Qty = x.Qty
                });
            });

            order.OrderDetails = orderDetailList;
            order.NetAmount = orderDetailList.Sum(x => (x.Price * x.Qty));
            _context.Orders.Add(order);

            var removeCart = _context.CartDetails                
                .Where(x => x.UserId == _userManager.GetUserId(User)).ToList();
            _context.CartDetails.RemoveRange(removeCart);
            _context.SaveChanges();

            return Json(new { success = true, message = "Order added!" });
        }
        catch (Exception ex)
        { 
            return Json(new { success = false, message = ex.Message });
        }
    }
}
