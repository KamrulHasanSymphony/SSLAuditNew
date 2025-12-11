using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKInternalControlWeakPreferenceSetting
{
	public interface IBKInternalControlWeakPreferenceSettingService : IBaseService<BKInternalControlWeakPreferenceSettings>
	{
        ResultModel<BKInternalControlWeakPreferenceSettings> MultiplePost(BKInternalControlWeakPreferenceSettings model);
		ResultModel<BKInternalControlWeakPreferenceSettings> MultipleUnPost(BKInternalControlWeakPreferenceSettings model);
		
	}
}
