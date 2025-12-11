using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Core.Interfaces.Services.Audit;

public interface IAuditAreasService : IBaseService<AuditAreas>
{

    ResultModel<List<AuditAreas>> GDIC_GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    ResultModel<int> GDIC_GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
    int GDIC_GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

}