using Microsoft.AspNetCore.Mvc;
using QudraTech.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
}