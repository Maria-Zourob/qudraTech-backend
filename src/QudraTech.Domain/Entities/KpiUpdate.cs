namespace QudraTech.Domain.Entities;

public class KpiUpdate : BaseEntity
{
    public Guid InitiativeKpiId { get; set; }
    public InitiativeKpi? InitiativeKpi { get; set; }

    public decimal Value { get; set; }
    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public ICollection<KpiEvidence> Evidence { get; set; } = new List<KpiEvidence>();
}