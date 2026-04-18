using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;
using E_CommerceProductManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProductManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public OrdersController(IProductRepository productRepository, IOrderRepository orderRepository,
        ICustomerRepository customerRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        var dtos = orders.Select(o => new OrderDTO()
        {
            Id = o.Id,
            OrderDate = o.Date,
            CustomerId = o.CustomerId,
            CustomerName = o.Customer?.FullName,
            OrderAmount = o.Amount,
            OrderItems = o.Items?.Select(oi => new OrderItemDTO
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList() ?? new List<OrderItemDTO>()
        });
        return Ok(dtos);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var o = await _orderRepository.GetByIdAsync(id);
        
        if (o == null) 
            return NotFound();
        
        var dto = new OrderDTO
        {
            Id = o.Id,
            OrderDate = o.Date,
            CustomerId = o.CustomerId,
            CustomerName = o.Customer?.FullName,
            OrderAmount = o.Amount,
            OrderItems = o.Items?.Select(oi => new OrderItemDTO
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList() ?? new List<OrderItemDTO>()
        };
        
        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDTO dto)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        bool customerExists = await _customerRepository.ExistsAsync(dto.CustomerId);
        
        if (!customerExists) 
            return BadRequest("Invalid CustomerId");
        
        foreach (var item in dto.OrderItems)
        {
            bool productExists = await _productRepository.ExistsAsync(item.ProductId);
            
            if (!productExists) 
                return BadRequest($"Invalid ProductId: {item.ProductId}");
        }
        
        var order = new Order()
        {
            CustomerId = dto.CustomerId,
            Date = DateTime.UtcNow,
            Amount = dto.OrderItems.Sum(i => i.Quantity * i.UnitPrice),
            Items = dto.OrderItems.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
        
        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveAsync();
        
        dto.Id = order.Id;
        dto.OrderDate = order.Date;
        
        return Ok(dto);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDTO dto)
    {
        if (id != dto.Id) 
            return BadRequest("Id mismatch");
        
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        
        var existing = await _orderRepository.GetByIdAsync(id);
        
        if (existing == null) 
            return NotFound();
        
        bool customerExists = await _customerRepository.ExistsAsync(dto.CustomerId);
        
        if (!customerExists) 
            return BadRequest("Invalid CustomerId");
        
        foreach (var item in dto.OrderItems)
        {
            bool productExists = await _productRepository.ExistsAsync(item.ProductId);
            
            if (!productExists) 
                return BadRequest($"Invalid ProductId: {item.ProductId}");
        }
        
        existing.CustomerId = dto.CustomerId;
        existing.Date = dto.OrderDate;
        existing.Amount = dto.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
        
        // Update OrderItems: Clear existing and add new (simplified)
        existing.Items.Clear();
        
        existing.Items = dto.OrderItems.Select(i => new OrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList();
        
        _orderRepository.Update(existing);
        await _orderRepository.SaveAsync();
        
        return NoContent();
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var existing = await _orderRepository.GetByIdAsync(id);
        
        if (existing == null) 
            return NotFound();
        
        _orderRepository.Delete(existing);
        
        await _orderRepository.SaveAsync();
        
        return NoContent();
    }
}