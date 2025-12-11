using Newtonsoft.Json;
using Shampan.Models;
using Shampan.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Services.UserProfil
{
    public class UserProfileRepo
    {
        public ResultVM GetEmployeeCodeData(CommonVM model)
        {
            try
            {
                //HttpRequestHelper httpRequestHelper = new HttpRequestHelper();
                HttpRequestHelperHRM httpRequestHelper = new HttpRequestHelperHRM();
                AuthModel authModel = httpRequestHelper.GetAuthentication(new CredentialModel { UserName = "erp", Password = "123456" });
                #region Invoke API
                var data = httpRequestHelper.PostData("api/HRM/GetEmployeeCodeData", authModel, JsonConvert.SerializeObject(model));
                ResultVM result = JsonConvert.DeserializeObject<ResultVM>(data);
                #endregion                

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ResultVM GetUserProfileData(CommonVM model)
        {
            try
            {
                //HttpRequestHelper httpRequestHelper = new HttpRequestHelper();
                HttpRequestHelperHRM httpRequestHelper = new HttpRequestHelperHRM();
                AuthModel authModel = httpRequestHelper.GetAuthentication(new CredentialModel { UserName = "erp", Password = "123456" });
                #region Invoke API
                var data = httpRequestHelper.PostData("api/HRM/GetUserProfileData", authModel, JsonConvert.SerializeObject(model));
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
