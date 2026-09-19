namespace QudraTech.Domain.Entities;

public class InitiativeActivity : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public DateTime? PlannedDate { get; set; }
    public DateTime? ActualDate { get; set; }
    public string? Location { get; set; }
}