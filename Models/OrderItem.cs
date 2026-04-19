using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceProductManagementSystem.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Order))]
    [Required(ErrorMessage = "Order Id is required")]
    public int OrderId { get; set; }
    
    public Order Order { get; set; } = null!;
    
    [ForeignKey(nameof(Product))]
    [Required(ErrorMessage = "Product Id is required")]
    public int ProductId { get; set; }
    
    public Product Product { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 1000, ErrorMessage = "Quantity must be greater than or equal to 1")]
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Unit price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Unit price must be between 0.01 and 999,999.99")]
    [Column(TypeName = "decimal(12,2)")]
    public decimal UnitPrice { get; set; }
    
}