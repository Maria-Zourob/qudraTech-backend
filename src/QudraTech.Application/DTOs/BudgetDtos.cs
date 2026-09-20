namespace QudraTech.Application.DTOs;

public record BudgetSummaryDto(decimal TotalPlanned, decimal TotalActual);

public record BudgetItemDto(
    Guid Id, string Category, string ItemNameAr, string ItemNameEn,
    decimal PlannedAmount, decimal ActualAmount, string? FundingSource
);

public record CreateBudgetItemDto(
    string Category, string ItemNameAr, string ItemNameEn,
    decimal PlannedAmount, decimal ActualAmount, string? FundingSource
);