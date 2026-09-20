namespace QudraTech.Domain.Entities;

public class Partner : BaseEntity
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Partner / Donor / Community Partner
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }

    public ICollection<InitiativePartner> InitiativePartners { get; set; } = new List<InitiativePartner>();
}