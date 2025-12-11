using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKReguCompliancesPreferenceSetting
{
	public interface IBKReguCompliancesPreferenceSettingsService : IBaseService<BKReguCompliancesPreferenceSettings>
	{
        ResultModel<BKReguCompliancesPreferenceSettings> MultiplePost(BKFinancePerformPreferenceSettings model);
		ResultModel<BKReguCompliancesPreferenceSettings> MultipleUnPost(BKReguCompliancesPreferenceSettings model);
		
	}
}
