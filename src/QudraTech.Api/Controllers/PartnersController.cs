using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QudraTech.Application.DTOs;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

[ApiController]
public class PartnersController : ControllerBase
{
    private readonly QudraTechDbContext _context;

    public PartnersController(QudraTechDbContext context)
    {
        _context = context;
    }

    // عام — كل الشركاء يظهروا للزوار (صفحة "شركاؤنا")
    [HttpGet("api/public/partners")]
    public async Task<IActionResult> GetPublicList()
    {
        var partners = await _context.Partners
            .Select(p => new PartnerDto(p.Id, p.NameAr, p.NameEn, p.Type, p.Website, p.LogoUrl))
            .ToListAsync();
        return Ok(partners);
    }

    // محمي — إضافة شريك جديد
    [HttpPost("api/partners")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> Create(CreatePartnerDto dto)
    {
        var partner = new Partner
        {
            NameAr = dto.NameAr,
            NameEn = dto.NameEn,
            Type = dto.Type,
            Website = dto.Website,
            LogoUrl = dto.LogoUrl
        };

        _context.Partners.Add(partner);
        await _context.SaveChangesAsync();

        return Ok(new PartnerDto(partner.Id, partner.NameAr, partner.NameEn, partner.Type, partner.Website, partner.LogoUrl));
    }
        // عام — شركاء مبادرة معيّنة بالتحديد
    [HttpGet("api/public/initiatives/{slug}/partners")]
    public async Task<IActionResult> GetByInitiative(string slug)
    {
        var initiative = await _context.Initiatives.FirstOrDefaultAsync(i => i.Slug == slug);
        if (initiative is null) return NotFound();

        var partners = await _context.InitiativePartners
            .Where(ip => ip.InitiativeId == initiative.Id)
            .Select(ip => new PartnerDto(
                ip.Partner!.Id, ip.Partner.NameAr, ip.Partner.NameEn,
                ip.Partner.Type, ip.Partner.Website, ip.Partner.LogoUrl
            ))
            .ToListAsync();
        return Ok(partners);
    }

    // محمي — ربط شريك موجود بمبادرة
    [HttpPost("api/initiatives/{initiativeId}/partners/{partnerId}")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> LinkToInitiative(Guid initiativeId, Guid partnerId)
    {
        var exists = await _context.InitiativePartners
            .AnyAsync(ip => ip.InitiativeId == initiativeId && ip.PartnerId == partnerId);
        if (exists) return Ok(new {message = "Already linked"});

        _context.InitiativePartners.Add(new InitiativePartner
        {
            InitiativeId = initiativeId,
            PartnerId = partnerId
        });
        await _context.SaveChangesAsync();

        return Ok(new {message = "Partner linked to initiative"});
    }
}