using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Repository.Audit;

public interface IAuditAreasRepository : IBaseRepository<AuditAreas>
{
    List<AuditAreas> GDIC_GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    int GDIC_GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    
}