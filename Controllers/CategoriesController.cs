using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;
using E_CommerceProductManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProductManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    
    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();

        var dto = categories.Select(c => new CategoryDTO()
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        });
        
        return  Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        
        if (category == null)
            return NotFound();

        var dto = new CategoryDTO()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = new Category()
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _categoryRepository.AddAsync(category);

        await _categoryRepository.SaveAsync();
        
        dto.Id = category.Id;
        return Ok(dto);
    }
}