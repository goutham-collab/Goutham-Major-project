using System.ComponentModel.DataAnnotations;

namespace eMobileShop.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Category Logo")]
    public string? Logo { get; set; }

    [Display(Name = "Category Name")]
    [Required(ErrorMessage = "Category name is required")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    [Required(ErrorMessage = "Category description is required")]
    public string Description { get; set; }

    //Relationships
    public List<Product>? Products { get; set; }
}
