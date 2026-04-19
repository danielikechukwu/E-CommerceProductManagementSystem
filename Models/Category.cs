using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceProductManagementSystem.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    public string Name { get; set; }
    
    [StringLength(1000, ErrorMessage = "Category description cannot exceed 1000 characters")]
    public string? Description { get; set; }
    
    public ICollection<Product>? Products { get; set; }
    
}