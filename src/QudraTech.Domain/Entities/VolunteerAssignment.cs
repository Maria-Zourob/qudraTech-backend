namespace QudraTech.Domain.Entities;

public class VolunteerAssignment : BaseEntity
{
    public Guid VolunteerId { get; set; }
    public Volunteer? Volunteer { get; set; }

    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }
}