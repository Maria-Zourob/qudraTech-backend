using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Api.Controllers;

[ApiController]
[Authorize(Roles = "SuperAdmin,InitiativeManager")]
public class VolunteerRecordsImportController : ControllerBase
{
    private readonly QudraTechDbContext _context;
    private static readonly string[] Headers =
    {
        "الاسم الكامل", "الفريق", "الدور داخل الفريق", "رقم الهاتف",
        "البريد الإلكتروني", "المهارات", "الخبرة", "تاريخ الانضمام (yyyy-mm-dd)"
    };

    public VolunteerRecordsImportController(QudraTechDbContext context)
    {
        _context = context;
    }

    // تحميل قالب Excel فارغ
    [HttpGet("api/volunteer-records/template")]
    public IActionResult DownloadTemplate()
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("المتطوعون");

        for (int i = 0; i < Headers.Length; i++)
        {
            sheet.Cell(1, i + 1).Value = Headers[i];
            sheet.Cell(1, i + 1).Style.Font.Bold = true;
        }
        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "volunteer-template.xlsx"
        );
    }

    // رفع ملف Excel معبّى واستيراد المتطوعين دفعة وحدة
    [HttpPost("api/volunteer-records/import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new {message = "لم يتم اختيار ملف"});
        }

        using var stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        var sheet = workbook.Worksheets.First();

        var imported = 0;
        var errors = new List<string>();

        var lastRow = sheet.LastRowUsed()?.RowNumber() ?? 1;

        for (int row = 2; row <= lastRow; row++) // بندأ من الصف 2 (نتخطى العناوين)
        {
            var fullName = sheet.Cell(row, 1).GetString().Trim();
            if (string.IsNullOrWhiteSpace(fullName)) continue; // صف فاضي، نتخطاه

            try
            {
                var joinDateCell = sheet.Cell(row, 8);
                DateTime joinDate = joinDateCell.DataType == XLDataType.DateTime
                    ? joinDateCell.GetDateTime()
                    : DateTime.Parse(joinDateCell.GetString());

                var record = new VolunteerRecord
                {
                    FullName = fullName,
                    Team = sheet.Cell(row, 2).GetString().Trim(),
                    RoleInTeam = sheet.Cell(row, 3).GetString().Trim(),
                    Phone = sheet.Cell(row, 4).GetString().Trim(),
                    Email = sheet.Cell(row, 5).GetString().Trim(),
                    Skills = sheet.Cell(row, 6).GetString().Trim(),
                    Experience = sheet.Cell(row, 7).GetString().Trim(),
                    JoinDate = joinDate
                };

                _context.VolunteerRecords.Add(record);
                imported++;
            }
            catch
            {
                errors.Add($"صف {row}: تعذّر قراءة البيانات (تحققي من تنسيق التاريخ)");
            }
        }

        await _context.SaveChangesAsync();

        return Ok(new {imported, errors});
    }
}