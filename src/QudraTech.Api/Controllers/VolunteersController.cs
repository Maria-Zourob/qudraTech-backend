using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QudraTech.Application.DTOs;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin,InitiativeManager")]
public class VolunteersController : ControllerBase
{
    private readonly QudraTechDbContext _context;

    public VolunteersController(QudraTechDbContext context)
    {
        _context = context;
    }

    // محمي بالكامل — بيانات المتطوعين حساسة، مش للعرض العام أبدًا
    [HttpGet("api/volunteers")]
    public async Task<IActionResult> GetAll()
    {
        var volunteers = await _context.Volunteers
            .Include(v => v.User)
            .Select(v => new VolunteerDto(
                v.Id, v.User!.Email!, v.User.FullNameAr, v.User.FullNameEn,
                v.Status, v.SafeguardingTrainingCompleted
            ))
            .ToListAsync();
        return Ok(volunteers);
    }

    [HttpPatch("api/volunteers/{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateVolunteerStatusDto dto)
    {
        var volunteer = await _context.Volunteers.FindAsync(id);
        if (volunteer is null) return NotFound();

        volunteer.Status = dto.Status;
        if (dto.Status == "Approved")
        {
            volunteer.ApprovedAt = DateTime.UtcNow;
        }
        volunteer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new {message = "Status updated", status = volunteer.Status});
    }
}