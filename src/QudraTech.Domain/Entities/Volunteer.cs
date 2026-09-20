namespace QudraTech.Domain.Entities;

public class Volunteer : BaseEntity
{
    public Guid UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected
    public bool SafeguardingTrainingCompleted { get; set; } = false;
    public DateTime? ApprovedAt { get; set; }
}