using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKCommonSelectionSetting
{
	public interface IBKCommonSelectionSettingService : IBaseService<BKCommonSelectionSettings>
	{
        ResultModel<BKCommonSelectionSettings> MultiplePost(BKCommonSelectionSettings model);
		ResultModel<BKCommonSelectionSettings> MultipleUnPost(BKCommonSelectionSettings model);
		
	}
}
