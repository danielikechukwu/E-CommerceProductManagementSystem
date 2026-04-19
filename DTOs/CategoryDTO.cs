using System.ComponentModel.DataAnnotations;

namespace E_CommerceProductManagementSystem.DTOs;

public class CategoryDTO
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100,  ErrorMessage = "Category name cannot exceed 100 characters")]
    public string Name { get; set; } = String.Empty;
    
    [StringLength(500, ErrorMessage = "Category name cannot exceed 500 characters")]
    public string? Description { get; set; }
}