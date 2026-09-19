using QudraTech.Domain.Enums;

namespace QudraTech.Domain.Entities;

public class Media : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public MediaType Type { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string? CaptionAr { get; set; }
    public string? CaptionEn { get; set; }
}