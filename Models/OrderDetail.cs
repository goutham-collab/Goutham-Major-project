using System.ComponentModel.DataAnnotations.Schema;

namespace eMobileShop.Models;

public class OrderDetail
{
    public int Id { get; set; }        
    public int OrderId { get; set; }        
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public decimal Price { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

}
