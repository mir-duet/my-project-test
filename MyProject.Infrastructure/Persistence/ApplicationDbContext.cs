using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities;
using MyProject.Domain.Common;
using System.Text.Json;

namespace MyProject.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var auditEntries = new List<AuditLog>();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                var now = DateTime.UtcNow;
                
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                var auditLog = new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = entry.Entity.Id,
                    Action = entry.State.ToString(),
                    Changes = SerializeChanges(entry),
                    PerformedBy = "System", // This should be replaced with actual user information
                    Timestamp = DateTime.UtcNow
                };

                auditEntries.Add(auditLog);
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        if (auditEntries.Any())
        {
            await AuditLogs.AddRangeAsync(auditEntries, cancellationToken);
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    private string SerializeChanges(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
    {
        var changes = new Dictionary<string, object?>();

        if (entry.State == EntityState.Added)
        {
            foreach (var property in entry.CurrentValues.Properties)
            {
                changes[property.Name] = entry.CurrentValues[property];
            }
        }
        else if (entry.State == EntityState.Modified)
        {
            foreach (var property in entry.Properties)
            {
                if (property.IsModified)
                {
                    changes[$"{property.Metadata.Name}_Old"] = property.OriginalValue;
                    changes[$"{property.Metadata.Name}_New"] = property.CurrentValue;
                }
            }
        }
        else if (entry.State == EntityState.Deleted)
        {
            foreach (var property in entry.OriginalValues.Properties)
            {
                changes[property.Name] = entry.OriginalValues[property];
            }
        }

        return JsonSerializer.Serialize(changes);
    }
}
