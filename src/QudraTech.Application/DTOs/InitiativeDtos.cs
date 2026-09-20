namespace QudraTech.Application.DTOs;

public record InitiativeListItemDto(
    string Slug,
    string TitleAr,
    string TitleEn,
    string DescriptionAr,
    string DescriptionEn,
    string Status,
    string Location,
    string TargetGroupAr,
    string TargetGroupEn
);

public record CreateInitiativeDto(
    string Slug,
    string TitleAr,
    string TitleEn,
    string DescriptionAr,
    string DescriptionEn,
    string TargetGroupAr,
    string TargetGroupEn,
    string Location,
    Guid CategoryId
);