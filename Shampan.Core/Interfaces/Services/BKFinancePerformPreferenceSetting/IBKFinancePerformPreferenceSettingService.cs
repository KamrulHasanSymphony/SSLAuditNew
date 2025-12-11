using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKFinancePerformPreferenceSetting
{
	public interface IBKFinancePerformPreferenceSettingService : IBaseService<BKFinancePerformPreferenceSettings>
	{
        ResultModel<BKFinancePerformPreferenceSettings> MultiplePost(BKFinancePerformPreferenceSettings model);
		ResultModel<BKFinancePerformPreferenceSettings> MultipleUnPost(BKFinancePerformPreferenceSettings model);

        ResultModel<BKFinancePreformPreferenceCBS> FinancePerformPreferenceTempInsert(BKFinancePreformPreferenceCBS model);
        ResultModel<List<BKFinancePerformPreferenceSettings>> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);



    }
}
