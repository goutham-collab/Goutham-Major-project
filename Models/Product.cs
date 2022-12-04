using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMobileShop.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; }
    public string? Image { get; set; }
    
    [Display(Name = "Product Color")]
    [Required(ErrorMessage = "Product color is required")]
    public string ProductColor { get; set; }
    
    [Required]
    
    [Display(Name = "Available")]
    public bool IsAvailable { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    
    [ForeignKey(nameof(CategoryId))]
    public virtual Category? Category { get; set; }
}