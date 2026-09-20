namespace QudraTech.Application.DTOs;

public record RiskDto(
    Guid Id, string DescriptionAr, string DescriptionEn,
    string Probability, string Impact, string Mitigation,
    string? ContingencyPlan, string Status
);

public record CreateRiskDto(
    string DescriptionAr, string DescriptionEn,
    string Probability, string Impact, string Mitigation, string? ContingencyPlan
);

public record UpdateRiskStatusDto(string Status);