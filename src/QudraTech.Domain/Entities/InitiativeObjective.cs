namespace QudraTech.Domain.Entities;

public class InitiativeObjective : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
}