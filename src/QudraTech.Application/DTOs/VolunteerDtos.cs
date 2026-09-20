namespace QudraTech.Application.DTOs;

public record VolunteerDto(
    Guid Id, string Email, string FullNameAr, string FullNameEn,
    string Status, bool SafeguardingTrainingCompleted
);

public record UpdateVolunteerStatusDto(string Status);