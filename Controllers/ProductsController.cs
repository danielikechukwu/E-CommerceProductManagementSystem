using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;
using E_CommerceProductManagementSystem.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProductManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    private readonly ICategoryRepository _categoryRepository;

    public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;

        _categoryRepository = categoryRepository;
    }

    [HttpGet("GetProducts")]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productRepository.GetAllAsync();

        var dtos = products.Select(p => new ProductDTO()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name
        });

        return Ok(dtos);
    }

    [HttpPost("CreateProducts")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        bool categoryExists = await _categoryRepository.ExistsAsync(dto.CategoryId);

        if (!categoryExists)
            return BadRequest("Invalid CategoryId");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };

        await _productRepository.AddAsync(product);

        await _productRepository.SaveAsync();

        dto.Id = product.Id; // set generated ID

        return Ok(dto);
    }

    [HttpPut("UpdateProduct/{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO dto)
    {
        if (id != dto.Id)
            return BadRequest("Id mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _productRepository.GetByIdAsync(id);

        if (existing == null)
            return NotFound();

        bool categoryExists = await _categoryRepository.ExistsAsync(dto.CategoryId);

        if (!categoryExists)
            return BadRequest("Invalid CategoryId");

        existing.Name = dto.Name;
        existing.Description = dto.Description;
        existing.Price = dto.Price;
        existing.CategoryId = dto.CategoryId;

        _productRepository.Update(existing);

        await _productRepository.SaveAsync();

        return NoContent();
    }

    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existing = await _productRepository.GetByIdAsync(id);

        if (existing == null) return NotFound();

        _productRepository.Delete(existing);
        await _productRepository.SaveAsync();

        return NoContent();
    }
}