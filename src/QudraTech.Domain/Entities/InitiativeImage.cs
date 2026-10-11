namespace QudraTech.Domain.Entities;

public class InitiativeImage : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public byte[] Data { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "image/jpeg";
    public string FileName { get; set; } = string.Empty;

    public bool IsCover { get; set; }
    public int SortOrder { get; set; }

    public string CaptionAr { get; set; } = string.Empty;
    public string CaptionEn { get; set; } = string.Empty;
}