using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKFraudIrrgularitiesPreferenceSetting
{
	public interface IBKFraudIrrgularitiesPreferenceSettingService : IBaseService<BKFraudIrrgularitiesPreferenceSettings>
	{

        ResultModel<BKFraudIrrgularitiesPreferenceSettings> MultiplePost(BKFraudIrrgularitiesPreferenceSettings model);
		ResultModel<BKFraudIrrgularitiesPreferenceSettings> MultipleUnPost(BKFraudIrrgularitiesPreferenceSettings model);
        ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS> FraudIrrgularitiesPreferenceTempInsert(BKFraudIrregularitiesInternalControlPreferenceCBS model);
        ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
