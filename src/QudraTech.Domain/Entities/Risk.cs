namespace QudraTech.Domain.Entities;

public class Risk : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public string DescriptionAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string Probability { get; set; } = "Medium"; // Low / Medium / High
    public string Impact { get; set; } = "Medium";
    public string Mitigation { get; set; } = string.Empty;
    public string? ContingencyPlan { get; set; }
    public string Status { get; set; } = "Open"; // Open / Mitigated / Closed
}