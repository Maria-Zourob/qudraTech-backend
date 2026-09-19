namespace QudraTech.Domain.Entities;

public class DocumentVersion : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public string VersionNumber { get; set; } = "1.0";
    public string FileUrl { get; set; } = string.Empty;
    public string UploadedBy { get; set; } = string.Empty;
}