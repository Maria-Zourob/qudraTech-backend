namespace QudraTech.Domain.Entities;

public class BudgetItem : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public string Category { get; set; } = string.Empty; // Internet / Printing / Transport / Logistics
    public string ItemNameAr { get; set; } = string.Empty;
    public string ItemNameEn { get; set; } = string.Empty;
    public decimal PlannedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public string? FundingSource { get; set; }
}