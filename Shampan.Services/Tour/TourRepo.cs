using Newtonsoft.Json;
using Shampan.Models;
using Shampan.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Services.Tour
{
    public class TourRepo
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

        public ResultVM GetTravelAmountData(CommonVM model)
        {
            try
            {
                //HttpRequestHelper httpRequestHelper = new HttpRequestHelper();
                HttpRequestHelperHRM httpRequestHelper = new HttpRequestHelperHRM();
                AuthModel authModel = httpRequestHelper.GetAuthentication(new CredentialModel { UserName = "erp", Password = "123456" });
                #region Invoke API
                var data = httpRequestHelper.PostData("api/HRM/GetTravelAmountData", authModel, JsonConvert.SerializeObject(model));
                ResultVM result = JsonConvert.DeserializeObject<ResultVM>(data);
                #endregion                

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public ResultVM AuditTourInsert(AuditTourVM model)
        {
            try
            {
                HttpRequestHelper httpRequestHelper = new HttpRequestHelper();
                AuthModel authModel = httpRequestHelper.GetAuthentication(new CredentialModel { UserName = "erp", Password = "123456" });
                #region Invoke API
                var data = httpRequestHelper.PostData("api/Audit/AuditTourInsert", authModel, JsonConvert.SerializeObject(model));
                ResultVM result = JsonConvert.DeserializeObject<ResultVM>(data);
                #endregion                

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public ResultVM GetSaleData(SalesInvoice model)
        {
            try
            {

                HttpRequestHelperVAT httpRequestHelper = new HttpRequestHelperVAT();
                AuthModel authModel = httpRequestHelper.GetAuthentication(new CredentialModel { UserName = "erp", Password = "123456" });
                #region Invoke API
                var data = httpRequestHelper.PostData("api/BranchAccounting/GetSaleData", authModel, JsonConvert.SerializeObject(model));
                ResultVM result = JsonConvert.DeserializeObject<ResultVM>(data);
                #endregion                

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ResultVM GetPurchaseData(PurchaseInvoice model)
        {
            try
            {

                HttpRequestHelperVAT httpRequestHelper = new HttpRequestHelperVAT();
                AuthModel authModel = httpRequestHelper.GetAuthentication(new CredentialModel { UserName = "erp", Password = "123456" });
                #region Invoke API
                var data = httpRequestHelper.PostData("api/BranchAccounting/GetPurchaseData", authModel, JsonConvert.SerializeObject(model));
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
