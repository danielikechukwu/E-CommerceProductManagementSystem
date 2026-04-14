using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceProductManagementSystem.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Order Date is required")]
    public DateTime OrderDate { get; set; }
    
    [ForeignKey(nameof(Customer))]
    [Required(ErrorMessage = "CustomerId is required")]
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;
    
    [Required(ErrorMessage = "Order amount is required")]
    [Range(0.0, 99999999.99, ErrorMessage = "Amount must be greater than zero")]
    [Column(TypeName = "decimal(12,2)")]
    public decimal OrderAmount { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}