using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings
{
	public interface IBKRiskAssessPerferenceSettingRepository : IBaseRepository<BKRiskAssessPerferenceSetting>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKRiskAssessPerferenceSetting MultiplePost(BKRiskAssessPerferenceSetting objBKCheckListType);
        BKRiskAssessPerferenceSetting MultipleUnPost(BKRiskAssessPerferenceSetting model);

        BKRiskAssessRegulationPreferenceCBS RiskAssessPreferenceTempInsert(BKRiskAssessRegulationPreferenceCBS model);
        List<BKRiskAssessPerferenceSetting> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


    }
}
