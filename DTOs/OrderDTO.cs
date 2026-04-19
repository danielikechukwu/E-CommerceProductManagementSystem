using System.ComponentModel.DataAnnotations;

namespace E_CommerceProductManagementSystem.DTOs;

public class OrderDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Order Date is required.")]
    public DateTimeOffset OrderDate { get; set; }

    [Required(ErrorMessage = "CustomerId is required.")]
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    [Required(ErrorMessage = "Order Amount is required.")]
    [Range(0.0, 99999999.99, ErrorMessage = "Order Amount must be positive.")]
    public decimal OrderAmount { get; set; }

    [Required(ErrorMessage = "Order Items are required.")]
    public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
}