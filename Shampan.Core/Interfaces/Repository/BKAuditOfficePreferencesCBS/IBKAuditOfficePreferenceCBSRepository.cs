using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditOfficePreferencesCBS
{
	public interface IBKAuditOfficePreferenceCBSRepository : IBaseRepository<BKAuditOfficePreferenceCBS>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditOfficePreferenceCBS AuditOfficePreferenceCBSTempInsert(BKAuditOfficePreferenceCBS model);
        List<BKAuditOfficePreferenceCBS> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


    }
}
