using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKFinancePerformPreferenceSetting
{
	public interface IBKFinancePerformPreferenceSettingRepository : IBaseRepository<BKFinancePerformPreferenceSettings>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKFinancePerformPreferenceSettings MultiplePost(BKFinancePerformPreferenceSettings objBKCheckListType);
        BKFinancePerformPreferenceSettings MultipleUnPost(BKFinancePerformPreferenceSettings model);
        BKFinancePreformPreferenceCBS FinancePreformPreferenceCBSTempInsert(BKFinancePreformPreferenceCBS model);
        List<BKFinancePerformPreferenceSettings> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


    }
}
