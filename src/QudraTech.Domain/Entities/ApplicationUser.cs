using Microsoft.AspNetCore.Identity;

namespace QudraTech.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullNameAr { get; set; } = string.Empty;
    public string FullNameEn { get; set; } = string.Empty;
}