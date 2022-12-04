using System.ComponentModel.DataAnnotations;

namespace eMobileShop.Models.ViewModels;

public class ShippingVM
{
    public string Name { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }
    [MaxLength(20)]
    public string PhoneNo { get; set; }
    [MaxLength(500)]
    public string Address { get; set; }
    [MaxLength(10)]
    public string Zip { get; set; }
}
