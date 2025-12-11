using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings
{
	public interface IBKRiskAssessPerferenceSettingService : IBaseService<BKRiskAssessPerferenceSetting>
	{
        ResultModel<BKRiskAssessPerferenceSetting> MultiplePost(BKRiskAssessPerferenceSetting model);
		ResultModel<BKRiskAssessPerferenceSetting> MultipleUnPost(BKRiskAssessPerferenceSetting model);

        ResultModel<BKRiskAssessRegulationPreferenceCBS> RiskAssessPreferenceTempInsert(BKRiskAssessRegulationPreferenceCBS model);
        ResultModel<List<BKRiskAssessPerferenceSetting>> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


    }
}
