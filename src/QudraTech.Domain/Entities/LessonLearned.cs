namespace QudraTech.Domain.Entities;

public class LessonLearned : BaseEntity
{
    public Guid InitiativeId { get; set; }
    public Initiative? Initiative { get; set; }

    public string WhatWorked { get; set; } = string.Empty;
    public string WhatDidntWork { get; set; } = string.Empty;
    public string Recommendations { get; set; } = string.Empty;
}