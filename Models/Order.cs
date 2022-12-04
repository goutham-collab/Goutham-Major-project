using System.ComponentModel.DataAnnotations;

namespace eMobileShop.Models;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    [Display(Name = "Order No")]
    public string OrderNo { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Phone No")]
    public string PhoneNo { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string ZipCode { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime OrderDate { get; set; }
    public decimal NetAmount { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCancelled { get; set; }

    public virtual List<OrderDetail> OrderDetails { get; set; }
    
    public Order()
    {
        OrderDetails=new List<OrderDetail>();
    }
}
