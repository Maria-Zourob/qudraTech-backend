using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QudraTech.Application.DTOs;
using QudraTech.Application.Interfaces;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

[ApiController]
public class BudgetController : ControllerBase
{
    private readonly QudraTechDbContext _context;
    private readonly IInitiativeRepository _initiativeRepository;

    public BudgetController(QudraTechDbContext context, IInitiativeRepository initiativeRepository)
    {
        _context = context;
        _initiativeRepository = initiativeRepository;
    }

    // عام — ملخص مجمّع بس، بلا تفاصيل بنود
    [HttpGet("api/public/initiatives/{slug}/budget-summary")]
    public async Task<IActionResult> GetPublicSummary(string slug)
    {
        var initiative = await _initiativeRepository.GetBySlugAsync(slug);
        if (initiative is null) return NotFound();

        var items = await _context.BudgetItems
            .Where(b => b.InitiativeId == initiative.Id)
            .ToListAsync();

        var summary = new BudgetSummaryDto(
            items.Sum(i => i.PlannedAmount),
            items.Sum(i => i.ActualAmount)
        );
        return Ok(summary);
    }

    // محمي — كل التفاصيل، Admin/Manager/Finance بس
    [HttpGet("api/initiatives/{id}/budget-items")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> GetItems(Guid id)
    {
        var items = await _context.BudgetItems
            .Where(b => b.InitiativeId == id)
            .Select(b => new BudgetItemDto(
                b.Id, b.Category, b.ItemNameAr, b.ItemNameEn,
                b.PlannedAmount, b.ActualAmount, b.FundingSource
            ))
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost("api/initiatives/{id}/budget-items")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> AddItem(Guid id, CreateBudgetItemDto dto)
    {
        var item = new BudgetItem
        {
            InitiativeId = id,
            Category = dto.Category,
            ItemNameAr = dto.ItemNameAr,
            ItemNameEn = dto.ItemNameEn,
            PlannedAmount = dto.PlannedAmount,
            ActualAmount = dto.ActualAmount,
            FundingSource = dto.FundingSource
        };

        _context.BudgetItems.Add(item);
        await _context.SaveChangesAsync();

        return Ok(new BudgetItemDto(
            item.Id, item.Category, item.ItemNameAr, item.ItemNameEn,
            item.PlannedAmount, item.ActualAmount, item.FundingSource
        ));
    }
}