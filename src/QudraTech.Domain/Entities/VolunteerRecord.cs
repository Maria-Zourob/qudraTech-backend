namespace QudraTech.Domain.Entities;

public class VolunteerRecord : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Team { get; set; } = string.Empty;
    public string RoleInTeam { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public DateTime JoinDate { get; set; }
}