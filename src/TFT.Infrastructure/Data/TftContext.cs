using Microsoft.EntityFrameworkCore;
using TFT.Core.Models;

namespace TFT.Infrastructure.Data;

public class TftContext : DbContext
{
    public TftContext(DbContextOptions<TftContext> options) : base(options)
    {
    }

    public DbSet<Champion> Champions => Set<Champion>();
    public DbSet<Trait> Traits => Set<Trait>();
    public DbSet<ChampionTrait> ChampionTraits => Set<ChampionTrait>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Augment> Augments => Set<Augment>();
    public DbSet<SetData> SetData => Set<SetData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Champion configuration
        modelBuilder.Entity<Champion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ApiName);
            entity.HasIndex(e => e.Cost);

            entity.HasOne(e => e.SetData)
                .WithMany(s => s.Champions)
                .HasForeignKey(e => e.SetDataId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Trait configuration
        modelBuilder.Entity<Trait>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ApiName);
            entity.HasIndex(e => e.Name);

            entity.HasOne(e => e.SetData)
                .WithMany(s => s.Traits)
                .HasForeignKey(e => e.SetDataId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ChampionTrait (Many-to-Many) configuration
        modelBuilder.Entity<ChampionTrait>(entity =>
        {
            entity.HasKey(ct => new { ct.ChampionId, ct.TraitId });

            entity.HasOne(ct => ct.Champion)
                .WithMany(c => c.ChampionTraits)
                .HasForeignKey(ct => ct.ChampionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ct => ct.Trait)
                .WithMany(t => t.ChampionTraits)
                .HasForeignKey(ct => ct.TraitId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Item configuration
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ApiName).IsUnique();
            entity.HasIndex(e => e.Name);
        });

        // Augment configuration
        modelBuilder.Entity<Augment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ApiName).IsUnique();
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Tier);
        });

        // SetData configuration
        modelBuilder.Entity<SetData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Name, e.Mutator }).IsUnique();
        });
    }
}
