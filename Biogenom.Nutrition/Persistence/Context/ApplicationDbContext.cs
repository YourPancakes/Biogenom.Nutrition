using Biogenom.Nutrition.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Biogenom.Nutrition.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<NutritionAssessment> NutritionAssessments { get; set; }
    public DbSet<Nutrient> Nutrients { get; set; }
    public DbSet<NutrientBalance> NutrientBalances { get; set; }
    public DbSet<PersonalizedSet> PersonalizedSets { get; set; }
    public DbSet<Supplement> Supplements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NutritionAssessment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Gender).HasConversion<int>();
            entity.Property(e => e.ActivityLevel).HasConversion<int>();
            entity.Property(e => e.SleepQuality).HasConversion<int>();
            entity.Property(e => e.StressLevel).HasConversion<int>();

        });

        modelBuilder.Entity<Nutrient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Unit).HasMaxLength(20).IsRequired();
            entity.Property(e => e.DailyNormMin).IsRequired();
            entity.Property(e => e.DailyNormMax).IsRequired();
        });

        modelBuilder.Entity<NutrientBalance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CurrentValue).IsRequired();
            entity.Property(e => e.FromDiet).IsRequired();
            entity.Property(e => e.FromSupplements).IsRequired();
            entity.Property(e => e.Status).HasConversion<int>();
            entity.HasOne(e => e.NutritionAssessment)
                  .WithMany(a => a.NutrientBalances)
                  .HasForeignKey(e => e.NutritionAssessmentId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Nutrient)
                  .WithMany()
                  .HasForeignKey(e => e.NutrientId);
        });

        modelBuilder.Entity<PersonalizedSet>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.HasOne(e => e.NutritionAssessment)
                  .WithMany(a => a.PersonalizedSets)
                  .HasForeignKey(e => e.NutritionAssessmentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Supplement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Dosage).HasMaxLength(100);
            entity.Property(e => e.WhenToTake).HasMaxLength(200);
        });

        modelBuilder
            .Entity<PersonalizedSet>()
            .HasMany(ps => ps.Supplements)
            .WithMany(s => s.PersonalizedSets)
            .UsingEntity(j => j.ToTable("PersonalizedSetSupplements"));
    }
} 