namespace QudraTech.Domain.Entities;

public class Report : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = false;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
}