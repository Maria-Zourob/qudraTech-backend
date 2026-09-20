using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

public record SubmitContactDto(string Name, string Email, string Subject, string Message);
public record ContactMessageDto(Guid Id, string Name, string Email, string Subject, string Message, bool IsRead, DateTime CreatedAt);

[ApiController]
public class ContactController : ControllerBase
{
    private readonly QudraTechDbContext _context;

    public ContactController(QudraTechDbContext context)
    {
        _context = context;
    }

    // عام — أي زائر يقدر يبعت رسالة، بلا تسجيل دخول
    [HttpPost("api/public/contact")]
    public async Task<IActionResult> Submit(SubmitContactDto dto)
    {
        var message = new ContactMessage
        {
            Name = dto.Name,
            Email = dto.Email,
            Subject = dto.Subject,
            Message = dto.Message
        };

        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();

        return Ok(new {message = "Thank you, your message has been received."});
    }

    // محمي — الأدمين بس يشوف الرسائل
    [HttpGet("api/contact-messages")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> GetAll()
    {
        var messages = await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new ContactMessageDto(m.Id, m.Name, m.Email, m.Subject, m.Message, m.IsRead, m.CreatedAt))
            .ToListAsync();
        return Ok(messages);
    }

    [HttpPatch("api/contact-messages/{id}/read")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message is null) return NotFound();

        message.IsRead = true;
        await _context.SaveChangesAsync();

        return Ok(new {message = "Marked as read"});
    }
}