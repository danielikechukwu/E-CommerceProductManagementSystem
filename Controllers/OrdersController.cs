using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;
using E_CommerceProductManagementSystem.Repositories;
using E_CommerceProductManagementSystem.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProductManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    // private readonly IOrderRepository _orderRepository;

    // Declare UnitOfWork dependency in place of the IOrderRepository declarations above
    private readonly IUnitOfWork _unitOfWork;

    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public OrdersController(IProductRepository productRepository,
        ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }

    [HttpGet("GetOrders")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _unitOfWork.OrderRepository.GetAllAsync();
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

    [HttpGet("GetOrderById/{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var o = await _unitOfWork.OrderRepository.GetByIdAsync(id);

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

    // [HttpPost("CreateOrder")]
    // public async Task<IActionResult> CreateOrder([FromBody] OrderDTO dto)
    // {
    //     if (!ModelState.IsValid)
    //         return BadRequest(ModelState);
    //
    //     bool customerExists = await _customerRepository.ExistsAsync(dto.CustomerId);
    //
    //     if (!customerExists)
    //         return BadRequest("Invalid CustomerId");
    //
    //     foreach (var item in dto.OrderItems)
    //     {
    //         bool productExists = await _productRepository.ExistsAsync(item.ProductId);
    //
    //         if (!productExists)
    //             return BadRequest($"Invalid ProductId: {item.ProductId}");
    //     }
    //
    //     var order = new Order()
    //     {
    //         CustomerId = dto.CustomerId,
    //         Date = DateTime.UtcNow,
    //         Amount = dto.OrderItems.Sum(i => i.Quantity * i.UnitPrice),
    //         Items = dto.OrderItems.Select(i => new OrderItem
    //         {
    //             ProductId = i.ProductId,
    //             Quantity = i.Quantity,
    //             UnitPrice = i.UnitPrice
    //         }).ToList()
    //     };
    //
    //     await _unitOfWork.OrderRepository.AddAsync(order);
    //     await _unitOfWork.OrderRepository.SaveAsync();
    //
    //     dto.Id = order.Id;
    //     dto.OrderDate = order.Date;
    //
    //     return Ok(dto);
    // }

    [HttpPut("UpdateOrder/{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDTO dto)
    {
        if (id != dto.Id)
            return BadRequest("Id mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _unitOfWork.OrderRepository.GetByIdAsync(id);

        if (existing == null)
            return NotFound();

        bool customerExists = await _customerRepository.ExistsAsync(dto.CustomerId);

        if (!customerExists)
            return BadRequest("Invalid CustomerId");

        await _unitOfWork.BeginTransactionAsync();

        foreach (var item in dto.OrderItems)
        {
            bool productExists = await _productRepository.ExistsAsync(item.ProductId);

            if (!productExists)
                return BadRequest($"Invalid ProductId: {item.ProductId}");
        }

        try
        {
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

            _unitOfWork.OrderRepository.Update(existing);
            // await _unitOfWork.OrderRepository.SaveAsync();

            // Commit changes
            await _unitOfWork.CommitAsync();

            // Return 204 No Content for successful update
            return NoContent();
        }
        catch (Exception ex)
        {
            // Roleback on error
            await _unitOfWork.RollbackAsync();

            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete("DeleteOrder/{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var existing = await _unitOfWork.OrderRepository.GetByIdAsync(id);

        if (existing == null)
            return NotFound();

        // Start transaction 
        try
        {
// Delete order entity
            _unitOfWork.OrderRepository.Delete(existing);

            // Commit deletion
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            // Roll back actions on error
            await _unitOfWork.RollbackAsync();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("CreateOrders")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDto)
    {
        // Validate incoming request body against DTO validation attributes
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Start a database transaction via UnitOfWork
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Map OrderDTO to Order entity
            var order = new Order()
            {
                Date = orderDto.OrderDate,
                CustomerId = orderDto.CustomerId,
                Amount = orderDto.OrderAmount,
                Items = new List<OrderItem>() // Initialize collection
            };

            foreach (var itemDto in orderDto.OrderItems)
            {
                var orderItem = new OrderItem()
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice
                };

                order.Items.Add(orderItem);
            }

            // Add order entity asynchronously through repository
            await _unitOfWork.OrderRepository.AddAsync(order);

            // Commit the transaction (save changes + commit)
            await _unitOfWork.CommitAsync();

            // Return 200 OK with created order entity (can map to DTO if preferred)
            return Ok(order);
        }
        catch (Exception ex)
        {
            // Rollback transaction if any error occurs
            await _unitOfWork.RollbackAsync();

            // Log error here as needed

            // Return 500 Internal Server Error with message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}