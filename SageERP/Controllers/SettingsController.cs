using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using StackExchange.Exceptional;

namespace SageERP.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]

	public class SettingsController : Controller
    {

        private readonly ApplicationDbContext _applicationDb;
        private readonly ISettingsService _settingsService;
        public SettingsController(ApplicationDbContext applicationDb,ISettingsService settingService)
        {
            _applicationDb = applicationDb;
            _settingsService = settingService;

        }

        public ActionResult Index()
        {
            SettingsModel master = new SettingsModel();

            string userName = User.Identity.Name;
            ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

            ResultModel<SettingsModel> result = new ResultModel<SettingsModel>();



            try
            {
                SettingsModel model = new SettingsModel()
                {
                    SettingGroup = "SageERPReportAPIURL",
                    SettingName = "SageERPReportAPIURL",
                    SettingType = "String",
                    SettingValue = "http://localhost:11909/api/Sage/CreateGLEntry"

                };

                

                model.Audit.CreatedBy = user.UserName;
                model.Audit.CreatedAt = DateTime.Now;
                model.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                result = _settingsService.Insert(model);
                
                ResultModel<List<SettingsModel>> SettinsAllData = _settingsService.GetAll(new[] { "" }, new[] { "" },null);


                List<SettingsModel>  vm = SettinsAllData.Data;

                return View(vm);

            }

            catch (Exception ex)
            {
                return View();
            }


        }

        public ActionResult Edit(SettingsModel model)
        {
                try
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    model.Audit.LastUpdateAt = DateTime.Now;
                    model.Audit.LastUpdateBy = userName;
                    model.Audit.LastUpdateFrom = "erp";

                    ResultModel<SettingsModel> Result = _settingsService.Update(model);


                    if (Result.Status == Status.Success)
                    {
                        List<SettingsModel> vms = new List<SettingsModel>();
                        return RedirectToAction("Index");
                    }
                    else
                    {     
                    
                        List<SettingsModel> vms = new List<SettingsModel>();
                        return RedirectToAction("Index");

                    }
                }

                catch (Exception e)
                {

                    List<SettingsModel> vms = new List<SettingsModel>();
                    return RedirectToAction("Index");


                }
            
        }


        public ActionResult DbUpdate(DbUpdateModel model)
        {
            try
            {
                var result = _settingsService.DbUpdate(model);               
                return RedirectToAction("Index", "Home");
            }

            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
            }
            return RedirectToAction("Index", "Home");

        }

    }
}
