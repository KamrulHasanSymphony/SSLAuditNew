using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditOfficesPreferenceInfos
{
	public interface IBKAuditOfficesPreferenceInfoRepository : IBaseRepository<BKAuditOfficesPreferenceInfo>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditOfficesPreferenceInfo MultiplePost(BKAuditOfficesPreferenceInfo model);
        BKAuditOfficesPreferenceInfo MultipleUnPost(BKAuditOfficesPreferenceInfo model);
		
	}
}
