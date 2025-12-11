using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKCommonSelectionSetting
{
	public interface IBKCommonSelectionSettingRepository : IBaseRepository<BKCommonSelectionSettings>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKCommonSelectionSettings MultiplePost(BKCommonSelectionSettings model);
        BKCommonSelectionSettings MultipleUnPost(BKCommonSelectionSettings model);
		
	}
}
