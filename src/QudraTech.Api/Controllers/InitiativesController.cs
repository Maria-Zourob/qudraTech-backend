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
            i.Status.ToString(), i.Location
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
            initiative.Status.ToString(), initiative.Location
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
}