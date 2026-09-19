using QudraTech.Domain.Enums;

namespace QudraTech.Domain.Entities;

public class Initiative : BaseEntity
{
    // مثال: FS-2026-001 — يُولَّد تلقائيًا لاحقًا بطبقة Application
    public string InitiativeCode { get; set; } = string.Empty;

    // مثال: gaza-qudratech — يُستخدم بالرابط /initiatives/[slug]
    public string Slug { get; set; } = string.Empty;

    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;

    public string DescriptionAr { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public InitiativeStatus Status { get; set; } = InitiativeStatus.Draft;

    public Guid CategoryId { get; set; }
    public InitiativeCategory? Category { get; set; }

    public string TargetGroupAr { get; set; } = string.Empty;
    public string TargetGroupEn { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    // Navigation properties — تُضاف تدريجيًا كل ما بنينا كيان جديد
    public ICollection<InitiativeObjective> Objectives { get; set; } = new List<InitiativeObjective>();
    public ICollection<InitiativeActivity> Activities { get; set; } = new List<InitiativeActivity>();
    public ICollection<InitiativeBeneficiary> Beneficiaries { get; set; } = new List<InitiativeBeneficiary>();
    public ICollection<InitiativeKpi> Kpis { get; set; } = new List<InitiativeKpi>();
}