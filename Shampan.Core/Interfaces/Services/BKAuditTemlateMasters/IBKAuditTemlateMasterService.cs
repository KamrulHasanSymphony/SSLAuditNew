using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditTemlateMasters
{
	public interface IBKAuditTemlateMasterService : IBaseService<BKAuditTemlateMaster>
	{
        ResultModel<BKAuditTemlateMaster> MultiplePost(BKAuditTemlateMaster model);
		ResultModel<BKAuditTemlateMaster> MultipleUnPost(BKAuditTemlateMaster model);
        ResultModel<List<MappingData>> GetIndexMappingData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<CheckListItem>> GetIndexCheckListItemData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> GetIndexMappingDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int GetMappingCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<BKAuditTemplateDetails>> GetDetailsAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
