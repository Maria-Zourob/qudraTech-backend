using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QudraTech.Application.DTOs;
using QudraTech.Application.Interfaces;
using QudraTech.Domain.Entities;
using QudraTech.Domain.Enums;

namespace QudraTech.Api.Controllers;

[ApiController]
public class InitiativesController : ControllerBase
{
    private readonly IInitiativeRepository _repository;

    public InitiativesController(IInitiativeRepository repository)
    {
        _repository = repository;
    }

    // عام — أي زائر يقدر يشوفها، بس فقط المبادرات النشطة/المكتملة
    [HttpGet("api/public/initiatives")]
    public async Task<IActionResult> GetPublicList()
    {
        var initiatives = await _repository.GetPublicListAsync();
                var result = initiatives.Select(i => new InitiativeListItemDto(
            i.Slug, i.TitleAr, i.TitleEn, i.DescriptionAr, i.DescriptionEn,
            i.Status.ToString(), i.Location, i.TargetGroupAr, i.TargetGroupEn
        ));
        return Ok(result);
    }

    // عام — تفاصيل مبادرة واحدة بالـ slug
    [HttpGet("api/public/initiatives/{slug}")]
    public async Task<IActionResult> GetPublicBySlug(string slug)
    {
        var initiative = await _repository.GetBySlugAsync(slug);
        if (initiative is null) return NotFound();

                var result = new InitiativeListItemDto(
            initiative.Slug, initiative.TitleAr, initiative.TitleEn,
            initiative.DescriptionAr, initiative.DescriptionEn,
            initiative.Status.ToString(), initiative.Location,
            initiative.TargetGroupAr, initiative.TargetGroupEn
        );
        return Ok(result);
    }

    // محمي — بس Admin/Manager يقدروا يضيفوا مبادرة جديدة
    [HttpPost("api/initiatives")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> Create(CreateInitiativeDto dto)
    {
        var initiative = new Initiative
        {
            Slug = dto.Slug,
            TitleAr = dto.TitleAr,
            TitleEn = dto.TitleEn,
            DescriptionAr = dto.DescriptionAr,
            DescriptionEn = dto.DescriptionEn,
            TargetGroupAr = dto.TargetGroupAr,
            TargetGroupEn = dto.TargetGroupEn,
            Location = dto.Location,
            CategoryId = dto.CategoryId,
            Status = InitiativeStatus.Draft
        };

        await _repository.AddAsync(initiative);
        await _repository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPublicBySlug), new {slug = initiative.Slug}, initiative);
    }
        public record UpdateStatusDto(string Status);

    [HttpPatch("api/initiatives/{id}/status")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto dto)
    {
        var initiative = await _repository.GetByIdAsync(id);
        if (initiative is null) return NotFound();

        if (!Enum.TryParse<InitiativeStatus>(dto.Status, out var newStatus))
        {
            return BadRequest(new {message = "Invalid status value"});
        }

        initiative.Status = newStatus;
        initiative.UpdatedAt = DateTime.UtcNow;
        _repository.Update(initiative);
        await _repository.SaveChangesAsync();

        return Ok(new {message = "Status updated", status = initiative.Status.ToString()});
    }

    // كمان نحتاج endpoint يرجع كل المبادرات (مش بس Active/Completed) للوحة التحكم
    [HttpGet("api/initiatives")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> GetAllForAdmin()
    {
        var initiatives = await _repository.GetAllAsync();
        var result = initiatives.Select(i => new
        {
            id = i.Id,
            i.Slug,
            i.TitleAr,
            i.TitleEn,
            Status = i.Status.ToString()
        });
        return Ok(result);
    }
    public record CreateKpiDto(
        string NameAr, string NameEn, string Definition,
        decimal Baseline, decimal Target, decimal Actual, string Unit
    );

    [HttpGet("api/public/initiatives/{slug}/kpis")]
    public async Task<IActionResult> GetKpisBySlug(string slug)
    {
        var initiative = await _repository.GetBySlugAsync(slug);
        if (initiative is null) return NotFound();

        var full = await _repository.GetWithDetailsAsync(initiative.Id);
        var kpis = full!.Kpis.Select(k => new
        {
            k.NameAr, k.NameEn, k.Baseline, k.Target, k.Actual, k.Unit
        });
        return Ok(kpis);
    }
    [HttpGet("api/initiatives/{id}")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> GetByIdForAdmin(Guid id)
    {
        var initiative = await _repository.GetByIdAsync(id);
        if (initiative is null) return NotFound();

        return Ok(new
        {
            initiative.Id,
            initiative.Slug,
            initiative.TitleAr,
            initiative.TitleEn,
            initiative.DescriptionAr,
            initiative.DescriptionEn,
            Status = initiative.Status.ToString(),
            initiative.Location,
            initiative.TargetGroupAr,
            initiative.TargetGroupEn
        });
    }
    [HttpPost("api/initiatives/{id}/kpis")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> AddKpi(Guid id, CreateKpiDto dto)
    {
        var initiative = await _repository.GetByIdAsync(id);
        if (initiative is null) return NotFound();

        var kpi = new InitiativeKpi
        {
            InitiativeId = id,
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            Definition = dto.Definition,
            Baseline = dto.Baseline,
            Target = dto.Target,
            Actual = dto.Actual,
            Unit = dto.Unit
        };

        // بما إن Repository الحالي خاص بـ Initiative بس، منستخدم DbContext هون مباشرة لبساطة الوقت
        var context = HttpContext.RequestServices.GetRequiredService<Infrastructure.Persistence.QudraTechDbContext>();
        context.InitiativeKpis.Add(kpi);
        await context.SaveChangesAsync();

        return Ok(new
        {
            kpi.Id, kpi.NameAr, kpi.NameEn, kpi.Baseline, kpi.Target, kpi.Actual, kpi.Unit
        });
    }
}