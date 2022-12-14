using System.ComponentModel.DataAnnotations.Schema;

namespace eMobileShop.Models;

public class CartDetails
{
    public int Id { get; set; }        
    public int ProductId { get; set; }
    public string UserId { get; set; }
    public int Qty { get; set; } = 1;

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

}
