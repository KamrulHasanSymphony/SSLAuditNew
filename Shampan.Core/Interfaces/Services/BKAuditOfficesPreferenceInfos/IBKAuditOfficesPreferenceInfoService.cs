using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditOfficesPreferenceInfos
{
	public interface IBKAuditOfficesPreferenceInfoService : IBaseService<BKAuditOfficesPreferenceInfo>
	{
        ResultModel<BKAuditOfficesPreferenceInfo> MultiplePost(BKAuditOfficesPreferenceInfo model);
		ResultModel<BKAuditOfficesPreferenceInfo> MultipleUnPost(BKAuditOfficesPreferenceInfo model);
		
	}
}
