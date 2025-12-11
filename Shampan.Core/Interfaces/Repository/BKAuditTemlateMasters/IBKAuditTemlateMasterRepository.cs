using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditTemlateMasters
{
	public interface IBKAuditTemlateMasterRepository : IBaseRepository<BKAuditTemlateMaster>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditTemlateMaster MultiplePost(BKAuditTemlateMaster objCheckListItem);
        BKAuditTemlateMaster MultipleUnPost(BKAuditTemlateMaster model);
        List<MappingData> GetIndexMappingData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<CheckListItem> GetIndexCheckListItemData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int GetIndexMappingDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        int GetMappingCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue);
        BKAuditTemplateDetails InsertDetails(BKAuditTemplateDetails model);
        int DetailsDelete(string tableName, string[] conditionalFields, string[] conditionalValue);
        List<BKAuditTemplateDetails> GetDetailsAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);



    }
}
