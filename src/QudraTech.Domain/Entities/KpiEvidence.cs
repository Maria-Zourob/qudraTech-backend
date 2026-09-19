namespace QudraTech.Domain.Entities;

public class KpiEvidence : BaseEntity
{
    public Guid KpiUpdateId { get; set; }
    public KpiUpdate? KpiUpdate { get; set; }

    public string Description { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
}