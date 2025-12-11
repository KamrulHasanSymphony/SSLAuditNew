using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKFraudIrrgularitiesPreferenceSetting
{
	public interface IBKFraudIrrgularitiesPreferenceSettingRepository : IBaseRepository<BKFraudIrrgularitiesPreferenceSettings>
	{

		string CodeGeneration(string CodeGroup, string CodeName);
        BKFraudIrrgularitiesPreferenceSettings MultiplePost(BKFraudIrrgularitiesPreferenceSettings objBKCheckListType);
        BKFraudIrrgularitiesPreferenceSettings MultipleUnPost(BKFraudIrrgularitiesPreferenceSettings model);
        BKFraudIrregularitiesInternalControlPreferenceCBS FraudIrrgularitiesPreferenceTempInsert(BKFraudIrregularitiesInternalControlPreferenceCBS model);
        List<BKFraudIrrgularitiesPreferenceSettings> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
