using Microsoft.EntityFrameworkCore;
using Rosa.Core.Models;

namespace Rosa.Data;

public class RosaDbContext(DbContextOptions<RosaDbContext> options) : DbContext(options)
{
    public DbSet<CertificateRequest> Request { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CertificateRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EmployeeName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Reason)
                .HasMaxLength(500);

        });
    }
}