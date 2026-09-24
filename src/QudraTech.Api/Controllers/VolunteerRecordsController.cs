using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

public record VolunteerRecordDto(
    Guid Id, string FullName, string Team, string RoleInTeam,
    string Phone, string Email, string Skills, string Experience, DateTime JoinDate
);

public record CreateVolunteerRecordDto(
    string FullName, string Team, string RoleInTeam,
    string Phone, string Email, string Skills, string Experience, DateTime JoinDate
);

[ApiController]
[Authorize(Roles = "SuperAdmin,InitiativeManager")]
public class VolunteerRecordsController : ControllerBase
{
    private readonly QudraTechDbContext _context;

    public VolunteerRecordsController(QudraTechDbContext context)
    {
        _context = context;
    }

    [HttpGet("api/volunteer-records")]
    public async Task<IActionResult> GetAll()
    {
        var records = await _context.VolunteerRecords
            .OrderByDescending(v => v.JoinDate)
            .Select(v => new VolunteerRecordDto(
                v.Id, v.FullName, v.Team, v.RoleInTeam,
                v.Phone, v.Email, v.Skills, v.Experience, v.JoinDate
            ))
            .ToListAsync();
        return Ok(records);
    }

    [HttpPost("api/volunteer-records")]
    public async Task<IActionResult> Create(CreateVolunteerRecordDto dto)
    {
        var record = new VolunteerRecord
        {
            FullName = dto.FullName,
            Team = dto.Team,
            RoleInTeam = dto.RoleInTeam,
            Phone = dto.Phone,
            Email = dto.Email,
            Skills = dto.Skills,
            Experience = dto.Experience,
            JoinDate = dto.JoinDate
        };

        _context.VolunteerRecords.Add(record);
        await _context.SaveChangesAsync();

        return Ok(new VolunteerRecordDto(
            record.Id, record.FullName, record.Team, record.RoleInTeam,
            record.Phone, record.Email, record.Skills, record.Experience, record.JoinDate
        ));
    }

    [HttpDelete("api/volunteer-records/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var record = await _context.VolunteerRecords.FindAsync(id);
        if (record is null) return NotFound();

        _context.VolunteerRecords.Remove(record);
        await _context.SaveChangesAsync();
        return Ok(new {message = "Deleted"});
    }
}