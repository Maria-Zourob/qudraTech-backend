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
}