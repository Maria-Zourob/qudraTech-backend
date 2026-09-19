namespace QudraTech.Application.DTOs;

public record RegisterRequestDto(string Email, string Password, string FullNameAr, string FullNameEn);

public record LoginRequestDto(string Email, string Password);

public record AuthResponseDto(string AccessToken, string RefreshToken, string Email, IList<string> Roles);