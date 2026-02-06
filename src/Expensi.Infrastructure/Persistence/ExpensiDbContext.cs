using Expensi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensi.Infrastructure.Persistence;

public class ExpensiDbContext : DbContext
{
    public ExpensiDbContext(DbContextOptions<ExpensiDbContext> options) : base(options) { }

    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Remitter> Remitters => Set<Remitter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(builder =>
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            builder.Property(e => e.ReferenceDate).IsRequired();

            builder.HasOne(e => e.CreatedByUser)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.PasswordHash).IsRequired();

            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        });


        modelBuilder.Entity<Remitter>(builder =>
        {
            builder.HasKey(e => e.Id);
        });
    }
}
