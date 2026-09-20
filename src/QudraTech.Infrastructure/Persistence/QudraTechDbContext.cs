using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QudraTech.Domain.Entities;

namespace QudraTech.Infrastructure.Persistence;

public class QudraTechDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public QudraTechDbContext(DbContextOptions<QudraTechDbContext> options)
        : base(options)
    {
    }

    public DbSet<Initiative> Initiatives => Set<Initiative>();
    public DbSet<InitiativeActivity> InitiativeActivities => Set<InitiativeActivity>();
    public DbSet<InitiativeBeneficiary> InitiativeBeneficiaries => Set<InitiativeBeneficiary>();
    public DbSet<InitiativeCategory> InitiativeCategories => Set<InitiativeCategory>();
    public DbSet<InitiativeKpi> InitiativeKpis => Set<InitiativeKpi>();
    public DbSet<InitiativeObjective> InitiativeObjectives => Set<InitiativeObjective>();
    public DbSet<KpiEvidence> KpiEvidences => Set<KpiEvidence>();
    public DbSet<KpiUpdate> KpiUpdates => Set<KpiUpdate>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentVersion> DocumentVersions => Set<DocumentVersion>();
    public DbSet<Media> Media => Set<Media>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<TimelineEvent> TimelineEvents => Set<TimelineEvent>();
    public DbSet<SafeguardingReport> SafeguardingReports => Set<SafeguardingReport>();
    public DbSet<LessonLearned> LessonsLearned => Set<LessonLearned>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Partner> Partners => Set<Partner>();
    public DbSet<InitiativePartner> InitiativePartners => Set<InitiativePartner>();
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<VolunteerAssignment> VolunteerAssignments => Set<VolunteerAssignment>();
    public DbSet<BudgetItem> BudgetItems => Set<BudgetItem>();
    public DbSet<Risk> Risks => Set<Risk>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // مهم جدًا: لازم تنادى عشان جداول Identity تتبنى صح
        modelBuilder.Entity<BudgetItem>()
            .Property(b => b.PlannedAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<BudgetItem>()
            .Property(b => b.ActualAmount)
            .HasPrecision(18, 2);
        modelBuilder.Entity<InitiativeKpi>()
            .Property(k => k.Baseline)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InitiativeKpi>()
            .Property(k => k.Target)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InitiativeKpi>()
            .Property(k => k.Actual)
            .HasPrecision(18, 2);

        modelBuilder.Entity<KpiUpdate>()
            .Property(k => k.Value)
            .HasPrecision(18, 2);
    }
}