using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

public record CategoryDto(Guid Id, string NameAr, string NameEn);
public record CreateCategoryDto(string NameAr, string NameEn);

[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly QudraTechDbContext _context;

    public CategoriesController(QudraTechDbContext context)
    {
        _context = context;
    }

    [HttpGet("api/categories")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.InitiativeCategories
            .Select(c => new CategoryDto(c.Id, c.NameAr, c.NameEn))
            .ToListAsync();
        return Ok(categories);
    }

    [HttpPost("api/categories")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = new InitiativeCategory {NameAr = dto.NameAr, NameEn = dto.NameEn};
        _context.InitiativeCategories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(new CategoryDto(category.Id, category.NameAr, category.NameEn));
    }
}