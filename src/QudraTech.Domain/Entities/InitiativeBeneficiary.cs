namespace QudraTech.Domain.Entities;

public class InitiativeBeneficiary : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    // معرّف داخلي فقط — لا اسم كامل، لا صورة
    public string InternalCode { get; set; } = string.Empty;

    public int AgeGroupMin { get; set; }
    public int AgeGroupMax { get; set; }
    public string? Camp { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
}