using System.ComponentModel.DataAnnotations;

namespace E_CommerceProductManagementSystem.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Fullname is required")]
    [MaxLength(200, ErrorMessage = "Fullname must be less than 200 characters")]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [MaxLength(200, ErrorMessage = "Email must be less than 200 characters")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    public ICollection<Order>? Orders { get; set; }
}