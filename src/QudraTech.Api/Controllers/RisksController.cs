using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QudraTech.Application.DTOs;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

[ApiController]
public class RisksController : ControllerBase
{
    private readonly QudraTechDbContext _context;

    public RisksController(QudraTechDbContext context)
    {
        _context = context;
    }

    // عام — الوثيقة نفسها بتاكد أهمية الشفافية حول المخاطر وخطط التخفيف
    [HttpGet("api/public/initiatives/{slug}/risks")]
    public async Task<IActionResult> GetPublicList(string slug)
    {
        var initiative = await _context.Initiatives.FirstOrDefaultAsync(i => i.Slug == slug);
        if (initiative is null) return NotFound();

        var risks = await _context.Risks
            .Where(r => r.InitiativeId == initiative.Id)
            .Select(r => new RiskDto(
                r.Id, r.DescriptionAr, r.DescriptionEn, r.Probability,
                r.Impact, r.Mitigation, r.ContingencyPlan, r.Status
            ))
            .ToListAsync();
        return Ok(risks);
    }

    // محمي — إضافة خطر جديد
    [HttpPost("api/initiatives/{id}/risks")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> Create(Guid id, CreateRiskDto dto)
    {
        var risk = new Risk
        {
            InitiativeId = id,
            DescriptionAr = dto.DescriptionAr,
            DescriptionEn = dto.DescriptionEn,
            Probability = dto.Probability,
            Impact = dto.Impact,
            Mitigation = dto.Mitigation,
            ContingencyPlan = dto.ContingencyPlan
        };

        _context.Risks.Add(risk);
        await _context.SaveChangesAsync();

        return Ok(new RiskDto(
            risk.Id, risk.DescriptionAr, risk.DescriptionEn, risk.Probability,
            risk.Impact, risk.Mitigation, risk.ContingencyPlan, risk.Status
        ));
    }

    [HttpPatch("api/risks/{id}/status")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateRiskStatusDto dto)
    {
        var risk = await _context.Risks.FindAsync(id);
        if (risk is null) return NotFound();

        risk.Status = dto.Status;
        risk.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new {message = "Status updated", status = risk.Status});
    }
}