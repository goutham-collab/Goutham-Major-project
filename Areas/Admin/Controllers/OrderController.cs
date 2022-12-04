using eMobileShop.Data;
using eMobileShop.Helper;
using eMobileShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace eMobileShop.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = AppConstants.ADMIN)]
public class OrderController : Controller
{
    private readonly AppDbContext _context;    

    public OrderController(AppDbContext context)
    {
        _context = context;        
    }
    public IActionResult Index()
    {
        var orders = _context.Orders
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .ToList();
        return View(orders);
    }

    [HttpPost]
    public IActionResult ChangeOrder(ChangeOrderModel model)
    {
        bool isSuccess = false;
        var order = _context.Orders.Where(x => x.Id == model.OrderId && !x.IsCancelled && !x.IsCompleted).FirstOrDefault();
        if (Enum.IsDefined(typeof(OrderChangeType), model.OrderChangeType) && order != null)
        {
            if (model.OrderChangeType == OrderChangeType.Complete)
                order.IsCompleted = true;
            else
                order.IsCancelled = true;

            _context.Orders.Update(order);
            _context.SaveChanges();
            isSuccess = true;
        }

        return Json(new { 
            success = isSuccess            
        });
    }
}
