using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

public record InitiativeImageDto(Guid Id, bool IsCover, string CaptionAr, string CaptionEn, int SortOrder);
public record InitiativeCoverDto(string Slug, Guid ImageId);
public record UpdateImageCaptionDto(string CaptionAr, string CaptionEn);

[ApiController]
public class InitiativeImagesController : ControllerBase
{
    private const long MaxBytes = 6 * 1024 * 1024;
    private static readonly string[] AllowedTypes = {"image/jpeg", "image/png", "image/webp"};

    private readonly QudraTechDbContext _context;

    public InitiativeImagesController(QudraTechDbContext context)
    {
        _context = context;
    }

    // ---------- الإدارة ----------

    [HttpPost("api/initiatives/{initiativeId:guid}/images")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    [RequestSizeLimit(8 * 1024 * 1024)]
    public async Task<IActionResult> Upload(Guid initiativeId, IFormFile file, [FromForm] bool isCover = false)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new {message = "لم يتم اختيار ملف"});
        if (file.Length > MaxBytes)
            return BadRequest(new {message = "حجم الصورة أكبر من 6MB"});
        if (!AllowedTypes.Contains(file.ContentType))
            return BadRequest(new {message = "الصيغة غير مدعومة (JPG أو PNG أو WebP فقط)"});

        var exists = await _context.Initiatives.AnyAsync(i => i.Id == initiativeId);
        if (!exists) return NotFound();

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var hasCover = await _context.InitiativeImages
            .AnyAsync(i => i.InitiativeId == initiativeId && i.IsCover);

        var maxOrder = await _context.InitiativeImages
            .Where(i => i.InitiativeId == initiativeId)
            .Select(i => (int?)i.SortOrder)
            .MaxAsync() ?? -1;

        // أول صورة تصير غلاف تلقائيًا
        var makeCover = isCover || !hasCover;
        if (makeCover)
        {
            await _context.InitiativeImages
                .Where(i => i.InitiativeId == initiativeId && i.IsCover)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsCover, false));
        }

        var image = new InitiativeImage
        {
            InitiativeId = initiativeId,
            Data = ms.ToArray(),
            ContentType = file.ContentType,
            FileName = Path.GetFileName(file.FileName),
            IsCover = makeCover,
            SortOrder = maxOrder + 1
        };

        _context.InitiativeImages.Add(image);
        await _context.SaveChangesAsync();

        return Ok(new InitiativeImageDto(image.Id, image.IsCover, image.CaptionAr, image.CaptionEn, image.SortOrder));
    }

    [HttpGet("api/initiatives/{initiativeId:guid}/images")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> List(Guid initiativeId)
    {
        var images = await _context.InitiativeImages
            .Where(i => i.InitiativeId == initiativeId)
            .OrderBy(i => i.SortOrder)
            .Select(i => new InitiativeImageDto(i.Id, i.IsCover, i.CaptionAr, i.CaptionEn, i.SortOrder))
            .ToListAsync();

        return Ok(images);
    }

    [HttpPatch("api/initiative-images/{id:guid}/cover")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> SetCover(Guid id)
    {
        var initiativeId = await _context.InitiativeImages
            .Where(i => i.Id == id)
            .Select(i => (Guid?)i.InitiativeId)
            .FirstOrDefaultAsync();

        if (initiativeId is null) return NotFound();

        await _context.InitiativeImages
            .Where(i => i.InitiativeId == initiativeId)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsCover, false));

        await _context.InitiativeImages
            .Where(i => i.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsCover, true));

        return Ok(new {message = "Updated"});
    }

    [HttpPatch("api/initiative-images/{id:guid}")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> UpdateCaption(Guid id, UpdateImageCaptionDto dto)
    {
        var rows = await _context.InitiativeImages
            .Where(i => i.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(i => i.CaptionAr, dto.CaptionAr ?? string.Empty)
                .SetProperty(i => i.CaptionEn, dto.CaptionEn ?? string.Empty));

        return rows == 0 ? NotFound() : Ok(new {message = "Updated"});
    }

    [HttpDelete("api/initiative-images/{id:guid}")]
    [Authorize(Roles = "SuperAdmin,InitiativeManager")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var info = await _context.InitiativeImages
            .Where(i => i.Id == id)
            .Select(i => new {i.InitiativeId, i.IsCover})
            .FirstOrDefaultAsync();

        if (info is null) return NotFound();

        await _context.InitiativeImages.Where(i => i.Id == id).ExecuteDeleteAsync();

        // لو انحذف الغلاف، أول صورة باقية تصير الغلاف
        if (info.IsCover)
        {
            var nextId = await _context.InitiativeImages
                .Where(i => i.InitiativeId == info.InitiativeId)
                .OrderBy(i => i.SortOrder)
                .Select(i => (Guid?)i.Id)
                .FirstOrDefaultAsync();

            if (nextId is not null)
            {
                await _context.InitiativeImages
                    .Where(i => i.Id == nextId)
                    .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsCover, true));
            }
        }

        return Ok(new {message = "Deleted"});
    }

    // ---------- عام ----------

    [HttpGet("api/public/initiatives/{slug}/images")]
    [AllowAnonymous]
    public async Task<IActionResult> PublicList(string slug)
    {
        var images = await _context.InitiativeImages
            .Where(i => i.Initiative!.Slug == slug
                        && i.Initiative!.Status != Domain.Enums.InitiativeStatus.Draft)
            .OrderBy(i => i.SortOrder)
            .Select(i => new InitiativeImageDto(i.Id, i.IsCover, i.CaptionAr, i.CaptionEn, i.SortOrder))
            .ToListAsync();

        return Ok(images);
    }

    [HttpGet("api/public/initiative-covers")]
    [AllowAnonymous]
    public async Task<IActionResult> Covers()
    {
        var covers = await _context.InitiativeImages
            .Where(i => i.IsCover && i.Initiative!.Status != Domain.Enums.InitiativeStatus.Draft)
            .Select(i => new InitiativeCoverDto(i.Initiative!.Slug, i.Id))
            .ToListAsync();

        return Ok(covers);
    }

    [HttpGet("api/public/images/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Serve(Guid id)
    {
        var image = await _context.InitiativeImages
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => new {i.Data, i.ContentType})
            .FirstOrDefaultAsync();

        if (image is null) return NotFound();

        Response.Headers.CacheControl = "public, max-age=31536000, immutable";
        return File(image.Data, image.ContentType);
    }
}