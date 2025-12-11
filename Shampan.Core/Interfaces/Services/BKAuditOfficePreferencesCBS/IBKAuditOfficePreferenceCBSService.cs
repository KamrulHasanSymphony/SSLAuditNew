using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditOfficePreferencesCBS
{
	public interface IBKAuditOfficePreferenceCBSService : IBaseService<BKAuditOfficePreferenceCBS>
	{
        ResultModel<BKAuditOfficePreferenceCBS> AuditOfficePreferenceCBSTempInsert(BKAuditOfficePreferenceCBS model);
        ResultModel<List<BKAuditOfficePreferenceCBS>> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


    }
}
