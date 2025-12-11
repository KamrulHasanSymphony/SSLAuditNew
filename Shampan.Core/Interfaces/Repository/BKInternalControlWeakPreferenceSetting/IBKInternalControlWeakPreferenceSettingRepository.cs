using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKInternalControlWeakPreferenceSetting
{
	public interface IBKInternalControlWeakPreferenceSettingRepository : IBaseRepository<BKInternalControlWeakPreferenceSettings>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKInternalControlWeakPreferenceSettings MultiplePost(BKInternalControlWeakPreferenceSettings objBKCheckListType);
        BKInternalControlWeakPreferenceSettings MultipleUnPost(BKInternalControlWeakPreferenceSettings model);
		
	}
}
