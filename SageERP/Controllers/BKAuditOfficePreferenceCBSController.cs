using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.BKAuditOfficesPreferenceInfos;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.Tour;
using Shampan.Models;
using Shampan.Repository.SqlServer.BKCheckListTypes;
using Shampan.Services.BKAuditOfficePreferencesCBS;
using Shampan.Services.Tour;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class BKAuditOfficePreferenceCBSController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKAuditOfficePreferenceCBSService _bkAuditOfficePreferenceCBSService;
        private readonly IBKAuditOfficesPreferenceInfoService _bkAuditOfficesPreferenceInfoService;

        public BKAuditOfficePreferenceCBSController(ApplicationDbContext applicationDb, ITeamsService teamsService,
        IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService, 
        IBKAuditOfficePreferenceCBSService bkAuditOfficePreferenceCBSService, IBKAuditOfficesPreferenceInfoService bkAuditOfficesPreferenceInfoService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _bkAuditOfficePreferenceCBSService = bkAuditOfficePreferenceCBSService;
            _bkAuditOfficesPreferenceInfoService = bkAuditOfficesPreferenceInfoService;
        }

        public async Task<IActionResult> Itegration()
        {
            BKAuditOfficePreferenceCBS vm = new BKAuditOfficePreferenceCBS();
            return View(vm);
        }
        public ActionResult AuditOfficePreferenceIntegration(BKAuditOfficePreferenceCBS vm)
        {
            int i = 0;
            ResultModel<BKAuditOfficePreferenceCBS> resultData = new ResultModel<BKAuditOfficePreferenceCBS>();

            BKAuditOfficePreferencesCBSRepo _repo = new BKAuditOfficePreferencesCBSRepo();
            List<BKAuditOfficePreferenceCBS> AuditOfficePreference = new List<BKAuditOfficePreferenceCBS>();


            ResultVM result = _repo.GetAuditOfficePreferenceData(vm);

            if (result.Status == "Success" && result.DataVM != null)
            {
                AuditOfficePreference = JsonConvert.DeserializeObject<List<BKAuditOfficePreferenceCBS>>(result.DataVM.ToString());
            }
            else
            {
                AuditOfficePreference = null;
            }

            if (AuditOfficePreference != null)
            {
                _bkAuditOfficesPreferenceInfoService.Delete(0);

                foreach (BKAuditOfficePreferenceCBS data in AuditOfficePreference)
                {
                    resultData = _bkAuditOfficePreferenceCBSService.AuditOfficePreferenceCBSTempInsert(data);
                    AuditOfficePreference[i].ImportId = resultData.Data.ImportId;
                    i++;
                }
            }

            return Ok(new { data = AuditOfficePreference, recordsTotal = 1000, recordsFiltered = AuditOfficePreference.Count() });

        }

        public ActionResult SaveAuditOfficePreferenceIntegration(BKAuditOfficePreferenceCBS master)
        {
            ResultModel<BKAuditOfficePreferenceCBS> result = new ResultModel<BKAuditOfficePreferenceCBS>();
            ResultModel<BKAuditOfficesPreferenceInfo> resultData = new ResultModel<BKAuditOfficesPreferenceInfo>();

            try
            {
                IndexModel index = new IndexModel();
                string[] conditionalFields = new[] { "" };
                string?[] conditionalValue = new[] { "" };
                index.IDs = master.IDs;

                ResultModel<List<BKAuditOfficePreferenceCBS>> indexData =
                    _bkAuditOfficePreferenceCBSService.GetIndexDataTemp(index, conditionalFields, conditionalValue);


                foreach(BKAuditOfficePreferenceCBS data in indexData.Data)
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    BKAuditOfficesPreferenceInfo masterData = new BKAuditOfficesPreferenceInfo();
                    masterData.Audit.CreatedBy = user.UserName;
                    masterData.Audit.CreatedOn = DateTime.Now;
                    masterData.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    masterData.BranchID = data.BranchID;
                    masterData.BKAuditOfficeTypeId = 1;
                    masterData.BKAuditOfficeId = 1;
                    masterData.HistoricalPerformFlag = data.HistoricalPerformFlg;
                    masterData.AuditYear = data.AuditYear;
                    masterData.AuditFiscalYear = data.AuditFiscalYear;
                    masterData.AuditPerferenceValue = "0";
                    masterData.EntryDate = data.EntryDate;
                    masterData.Status = data.Status;
                   
                    resultData = _bkAuditOfficesPreferenceInfoService.Insert(masterData);

                }

                return Ok(resultData);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult _index()
        {
            try
            {
                IndexModel index = new IndexModel();
                string userName = User.Identity.Name;
                ApplicationUser user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                var search = Request.Form["search[value]"].FirstOrDefault();
                string users = Request.Form["BranchCode"].ToString();
                string branch = Request.Form["BranchName"].ToString();
                string Address = Request.Form["Address"].ToString();
                string TelephoneNo = Request.Form["TelephoneNo"].ToString();
                string code = Request.Form["code"].ToString();
                string advanceAmount = Request.Form["advanceAmount"].ToString();
                string description = Request.Form["description"].ToString();
                string post = Request.Form["ispost"].ToString();

                if (post == "Select")
                {
                    post = "";

                }

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "Id";
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);

                index.createdBy = userName;

                string[] conditionalFields = new[]
                {
                            "Code like",
                            "AdvanceAmount like",
                            "Description like",
                            "IsPost like"
                };

                string?[] conditionalValue = new[] { code, advanceAmount, description, post };

                ResultModel<List<BKAuditOfficePreferenceCBS>> indexData =
                    _bkAuditOfficePreferenceCBSService.GetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _bkAuditOfficePreferenceCBSService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _bkAuditOfficePreferenceCBSService.GetCount(TableName.BKRiskAssessPerferenceSettings, "Id", new[] { "BKRiskAssessPerferenceSettings.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

        }

        public ActionResult Create()
        {

            ModelState.Clear();
            BKAuditOfficePreferenceCBS vm = new BKAuditOfficePreferenceCBS();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(BKAuditOfficePreferenceCBS master)
        {
            ResultModel<BKAuditOfficePreferenceCBS> result = new ResultModel<BKAuditOfficePreferenceCBS>();

            try
            {

                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _bkAuditOfficePreferenceCBSService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _bkAuditOfficePreferenceCBSService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<BKAuditOfficePreferenceCBS>> result =
                    _bkAuditOfficePreferenceCBSService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKAuditOfficePreferenceCBS BKAuditOfficePreferenceCBS = result.Data.FirstOrDefault();
                BKAuditOfficePreferenceCBS.Operation = "update";
                BKAuditOfficePreferenceCBS.Id = id;

                return View("CreateEdit", BKAuditOfficePreferenceCBS);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        public ActionResult MultiplePost(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

            try
            {

                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _advancesService.MultiplePost(master);

                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }

        public ActionResult MultipleUnPost(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

            try
            {

                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    master.Operation = "unpost";


                    result = _advancesService.MultipleUnPost(master);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }


        public ActionResult MultipleReject(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

            try
            {
                foreach (string ID in master.IDs)
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Approval.IsRejected = true;
                    master.Approval.RejectedBy = user.UserName;
                    master.Approval.RejectedDate = DateTime.Now;
                    master.Operation = "reject";

                    result = _advancesService.MultipleUnPost(master);

                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }


        public ActionResult MultipleApproved(Advances master)
        {
            ResultModel<Advances> result = new ResultModel<Advances>();

            try
            {
                foreach (string ID in master.IDs)
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.PostedBy = user.UserName;
                    master.Audit.PostedOn = DateTime.Now;
                    master.Audit.PostedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    master.Operation = "approved";

                    result = _advancesService.MultipleUnPost(master);
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok("");
        }


    }
}
