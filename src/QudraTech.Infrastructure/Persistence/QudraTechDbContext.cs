using Microsoft.EntityFrameworkCore;
using QudraTech.Domain.Entities;

namespace QudraTech.Infrastructure.Persistence;

public class QudraTechDbContext : DbContext
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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