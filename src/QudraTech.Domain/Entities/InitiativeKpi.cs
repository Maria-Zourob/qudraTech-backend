namespace QudraTech.Domain.Entities;

public class InitiativeKpi : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;

    public decimal Baseline { get; set; }
    public decimal Target { get; set; }
    public decimal Actual { get; set; }
    public string Unit { get; set; } = string.Empty;

    public string? DataSource { get; set; }
    public string? CollectionMethod { get; set; }
    public string? Frequency { get; set; }
    public string? ResponsiblePerson { get; set; }

    public ICollection<KpiUpdate> Updates { get; set; } = new List<KpiUpdate>();
}