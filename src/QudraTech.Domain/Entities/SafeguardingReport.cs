using QudraTech.Domain.Enums;

namespace QudraTech.Domain.Entities;

public class SafeguardingReport : BaseEntity
{
    public Guid? InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // Complaint/Suggestion/SafeguardingConcern/General
    public SafeguardingReportStatus Status { get; set; } = SafeguardingReportStatus.Submitted;
    public string Description { get; set; } = string.Empty;
}