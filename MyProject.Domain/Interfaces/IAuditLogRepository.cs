using MyProject.Domain.Entities;

namespace MyProject.Domain.Interfaces;

public interface IAuditLogRepository : IRepository<AuditLog>
{
    Task<IEnumerable<AuditLog>> GetLogsByEntityAsync(string entityName, int entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
