namespace QudraTech.Domain.Entities;

public class InitiativeCategory : BaseEntity
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;

    public ICollection<Initiative> Initiatives { get; set; } = new List<Initiative>();
}