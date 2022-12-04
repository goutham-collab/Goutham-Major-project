using Microsoft.AspNetCore.Mvc;

namespace eMobileShop.ViewComponents;

public class ShoppingCartSummary : ViewComponent
{    
    public IViewComponentResult Invoke()
    {        
        return View(1);
    }
}
