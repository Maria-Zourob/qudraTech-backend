using QudraTech.Domain.Enums;

namespace QudraTech.Domain.Entities;

public class TimelineEvent : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public TimelineEventType EventType { get; set; }
    public DateTime EventDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }
}