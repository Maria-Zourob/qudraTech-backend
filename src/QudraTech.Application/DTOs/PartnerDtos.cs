namespace QudraTech.Application.DTOs;

public record PartnerDto(Guid Id, string NameAr, string NameEn, string Type, string? Website, string? LogoUrl);

public record CreatePartnerDto(string NameAr, string NameEn, string Type, string? Website, string? LogoUrl);