using System.ComponentModel.DataAnnotations;

namespace E_CommerceProductManagementSystem.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(100, ErrorMessage = "Category description cannot exceed 100 characters")]
    public string? Description { get; set; } = string.Empty;
    
    public ICollection<Product>?  Products { get; set; }
}