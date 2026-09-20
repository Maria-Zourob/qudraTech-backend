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
}