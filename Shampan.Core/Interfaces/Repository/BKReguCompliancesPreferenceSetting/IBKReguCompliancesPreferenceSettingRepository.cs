using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKReguCompliancesPreferenceSetting
{
	public interface IBKReguCompliancesPreferenceSettingRepository : IBaseRepository<BKReguCompliancesPreferenceSettings>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKReguCompliancesPreferenceSettings MultiplePost(BKReguCompliancesPreferenceSettings model);
        BKReguCompliancesPreferenceSettings MultipleUnPost(BKReguCompliancesPreferenceSettings model);
		
	}
}
