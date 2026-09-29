using Microsoft.AspNetCore.Mvc;
using QudraTech.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace QudraTech.Api.Controllers;

[ApiController]
[Route("api/public")]
public class PublicController : ControllerBase
{
    private readonly QudraTechDbContext _context;

    public PublicController(QudraTechDbContext context)
    {
        _context = context;
    }

    [HttpGet("impact")]
    public async Task<IActionResult> GetImpact()
    {
        var activeOrCompleted = new[]
        {
            Domain.Enums.InitiativeStatus.Active,
            Domain.Enums.InitiativeStatus.Completed
        };

        var initiativesCount = await _context.Initiatives
            .CountAsync(i => activeOrCompleted.Contains(i.Status));

        var beneficiariesCount = await _context.InitiativeBeneficiaries.CountAsync();
        var volunteersCount = await _context.Users.CountAsync();
        var activitiesCount = await _context.InitiativeActivities.CountAsync();

        return Ok(new
        {
            initiatives = initiativesCount,
            beneficiaries = beneficiariesCount,
            volunteers = volunteersCount,
            activities = activitiesCount
        });
    }
    
    [HttpGet("~/api/admin/dashboard-summary")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var totalInitiatives = await _context.Initiatives.CountAsync();
        var activeInitiatives = await _context.Initiatives
            .CountAsync(i => i.Status == Domain.Enums.InitiativeStatus.Active);
        var completedInitiatives = await _context.Initiatives
            .CountAsync(i => i.Status == Domain.Enums.InitiativeStatus.Completed);
        var beneficiaries = await _context.InitiativeBeneficiaries.CountAsync();
        var volunteers = await _context.Users.CountAsync();
        var partners = await _context.Partners.CountAsync();
        var unreadMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead);
        var pendingVolunteers = await _context.Volunteers.CountAsync(v => v.Status == "Pending");

        var byStatus = await _context.Initiatives
            .GroupBy(i => i.Status)
            .Select(g => new {status = g.Key.ToString(), count = g.Count()})
            .ToListAsync();

        return Ok(new
        {
            totalInitiatives,
            activeInitiatives,
            completedInitiatives,
            beneficiaries,
            volunteers,
            partners,
            unreadMessages,
            pendingVolunteers,
            byStatus
        });
    }
    [HttpGet("~/api/admin/recent-activity")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> GetRecentActivity()
    {
        var recentInitiatives = await _context.Initiatives
            .OrderByDescending(i => i.CreatedAt)
            .Take(3)
            .Select(i => new
            {
                type = "initiative",
                text = "مبادرة جديدة: " + i.TitleAr,
                date = i.CreatedAt
            })
            .ToListAsync();

        var recentVolunteers = await _context.VolunteerRecords
            .OrderByDescending(v => v.CreatedAt)
            .Take(3)
            .Select(v => new
            {
                type = "volunteer",
                text = "متطوّع جديد: " + v.FullName,
                date = v.CreatedAt
            })
            .ToListAsync();

        var recentMessages = await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .Take(3)
            .Select(m => new
            {
                type = "message",
                text = "رسالة تواصل من: " + m.Name,
                date = m.CreatedAt
            })
            .ToListAsync();

        var combined = recentInitiatives
            .Concat(recentVolunteers)
            .Concat(recentMessages)
            .OrderByDescending(x => x.date)
            .Take(6)
            .ToList();

        return Ok(combined);
    }
}