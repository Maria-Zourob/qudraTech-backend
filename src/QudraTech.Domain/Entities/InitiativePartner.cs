namespace QudraTech.Domain.Entities;

public class InitiativePartner : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public Guid PartnerId { get; set; }
    public Partner? Partner { get; set; }
}