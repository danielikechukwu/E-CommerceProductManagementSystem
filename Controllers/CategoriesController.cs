using E_CommerceProductManagementSystem.DTOs;
using E_CommerceProductManagementSystem.Models;
using E_CommerceProductManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProductManagementSystem.Controllers;

// Controller using generic repository
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    
    // Repository for Category entity operations
    // private readonly IRepository<Category> _categoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    public CategoriesController(ICategoryRepository categoryRepository)
    {
        // Assign injected repository instance to private field
        _categoryRepository = categoryRepository;
    }

    [HttpGet("GetCategories")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
        // _logger.LogInformation("Products list requested at {Time}", DateTime.UtcNow);
        
        // Fetch all Category entities from repository
        var categories = await _categoryRepository.GetAllAsync();

        // Map each Category entity to a CategoryDTO for data transfer to clients
        var dto = categories.Select(c => new CategoryDTO()
        {
            Id = c.Id, // Map primary key
            Name = c.Name, // Map category name
            Description = c.Description // Map category description
        });
        
        // Return HTTP 200 OK with list of category DTOs as JSON response
        return  Ok(dto);
    }

    // HTTP GET api/categories/{id} - Retrieves a category by ID
    [HttpGet("GetCategoryById/{id}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        // Fetch Category entity by primary key asynchronously
        var category = await _categoryRepository.GetByIdAsync(id);
        
        // If category not found, return HTTP 404 Not Found
        if (category == null)
            return NotFound();

        // Map the entity to CategoryDTO for response
        var dto = new CategoryDTO()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        
        // Return HTTP 200 OK with the single category DTO
        return Ok(dto);
    }

    [HttpPost("CreateCategory")]
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

    [HttpPut("UpdateCategory/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO dto)
    {
        if (id != dto.Id)
            return BadRequest("Id mismatch");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingCategory = await _categoryRepository.GetByIdAsync(id);

        if (existingCategory == null)
            return NotFound();
        
        existingCategory.Name = dto.Name;
        existingCategory.Description = dto.Description;
        
        _categoryRepository.Update(existingCategory);
        
        await _categoryRepository.SaveAsync();
        
        return Ok();
        
    }

    [HttpDelete("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(id);

        if (existingCategory == null)
            return NotFound();
        
        _categoryRepository.Delete(existingCategory);
        
        await  _categoryRepository.SaveAsync();
        
        // Return 204 No Content indicating successful deletion
        return NoContent();
    }
}