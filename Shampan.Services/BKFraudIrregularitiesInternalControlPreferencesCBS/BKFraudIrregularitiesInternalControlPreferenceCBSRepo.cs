using Newtonsoft.Json;
using Shampan.Models;
using Shampan.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Services.BKFraudIrregularitiesInternalControlPreferencesCBS
{
    public class BKFraudIrregularitiesInternalControlPreferenceCBSRepo
    {

        public ResultVM GetFraudIrregularitiesInternalControlPreferenceData(BKFraudIrregularitiesInternalControlPreferenceCBS model)
        {
            try
            {

                HttpRequestHelperCBS httpRequestHelper = new HttpRequestHelperCBS();
                AuthModel authModel = httpRequestHelper.GetAuthentication(new CredentialModel { UserName = "erp", Password = "123456" });

                #region Invoke API
                var data = httpRequestHelper.PostData("api/AuditCBS/GetFraudIrregularitiesInternalControlPreferenceData", authModel, JsonConvert.SerializeObject(model));
                ResultVM result = JsonConvert.DeserializeObject<ResultVM>(data);
                #endregion

                return result;

            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}

