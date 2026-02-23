using Microsoft.EntityFrameworkCore;
using QuickCash.Api.Models;

namespace QuickCash.Api.Data;

public class QuickCashDbContext : DbContext
{
    public QuickCashDbContext(DbContextOptions<QuickCashDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CATEGORY RULES
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name)
                .IsRequired();

            entity.Property(c => c.NormalizedName)
                .IsRequired();

            // Enforce case-insensitive uniqueness via NormalizedName.
            entity.HasIndex(c => c.NormalizedName)
                .IsUnique();
        });

        // TRANSACTION RULES
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            // Relationship: Transaction -> Category (many transactions per category)
            entity.HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
