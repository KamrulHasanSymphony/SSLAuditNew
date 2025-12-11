using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Repository.SqlServer.BKCheckListTypes;
using Shampan.Services.BKFinancePerformPreferenceSetting;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class BKFinancePerformPreferenceSettingController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKFinancePerformPreferenceSettingService _BKFinancePerformPreferenceSettingService;

        public BKFinancePerformPreferenceSettingController(ApplicationDbContext applicationDb, ITeamsService teamsService,
        IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService, IBKFinancePerformPreferenceSettingService BKFinancePerformPreferenceSettingService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _BKFinancePerformPreferenceSettingService = BKFinancePerformPreferenceSettingService;
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

                string branchid = Request.Form["branchid"].ToString();


                index.createdBy = userName;

                string[] conditionalFields = new[]
                {
                            "BranchID",
                            "Code like",
                            "AdvanceAmount like",
                            "Description like",
                            "IsPost like"
                };

                string?[] conditionalValue = new[] {branchid, code, advanceAmount, description, post };

                ResultModel<List<BKFinancePerformPreferenceSettings>> indexData =
                    _BKFinancePerformPreferenceSettingService.GetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _BKFinancePerformPreferenceSettingService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _BKFinancePerformPreferenceSettingService.GetCount(TableName.BKFinancePerformPreferenceSettings, "Id", new[] { "BKFinancePerformPreferenceSettings.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
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
            BKFinancePerformPreferenceSettings vm = new BKFinancePerformPreferenceSettings();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(BranchProfile data)
        {
            ResultModel<BKFinancePerformPreferenceSettings> result = new ResultModel<BKFinancePerformPreferenceSettings>();
            BKFinancePerformPreferenceSettings master =new BKFinancePerformPreferenceSettings();

            try
            {

                //ResultModel<List<BKFinancePerformPreferenceSettings>> checkData = _BKFinancePerformPreferenceSettingService.GetAll(new[] { "BranchID" }, new[] { data.BKFinancePerformPreferenceSetting.BranchID.ToString() });
                //if (checkData.Data.Count() == 0)
                //{
                //    data.BKFinancePerformPreferenceSetting.Operation = "add";
                //}
                //else
                //{
                //    data.BKFinancePerformPreferenceSetting.Operation = "update";
                //    master.Id = checkData.Data.FirstOrDefault().Id;
                //}


                master.Id = data.BKFinancePerformPreferenceSetting.Id;
                master.BKAuditOfficeTypeId = data.BKFinancePerformPreferenceSetting.BKAuditOfficeTypeId;
                master.FundAvailableFlag = data.BKFinancePerformPreferenceSetting.FundAvailableFlag;
                master.MisManagementClinentsFlag = data.BKFinancePerformPreferenceSetting.MisManagementClinentsFlag;
                master.EfficienctyFlag = data.BKFinancePerformPreferenceSetting.EfficienctyFlag;
                master.NPLSLargeFlag = data.BKFinancePerformPreferenceSetting.NPLSLargeFlag;
                master.LargeTxnManageFlag = data.BKFinancePerformPreferenceSetting.LargeTxnManageFlag;
                master.HighValueAssetMangeFlag = data.BKFinancePerformPreferenceSetting.HighValueAssetMangeFlag;
                master.BudgetMgtFlag = data.BKFinancePerformPreferenceSetting.BudgetMgtFlag;
                master.Status = data.BKFinancePerformPreferenceSetting.Status;
                master.BranchID = data.BKFinancePerformPreferenceSetting.BranchID;
                master.AuditYear = data.BKFinancePerformPreferenceSetting.AuditYear;
                master.AuditFiscalYear = data.BKFinancePerformPreferenceSetting.AuditFiscalYear;
                master.InfoReceiveDate = data.BKFinancePerformPreferenceSetting.InfoReceiveDate;
                master.InfoReceiveId = data.BKFinancePerformPreferenceSetting.InfoReceiveId;
                master.InfoReceiveFlag = data.BKFinancePerformPreferenceSetting.InfoReceiveFlag;

                //if (master.Operation == "update")
                if (data.BKFinancePerformPreferenceSetting.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _BKFinancePerformPreferenceSettingService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    master.Operation = "add";
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _BKFinancePerformPreferenceSettingService.Insert(master);

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
                ResultModel<List<BKFinancePerformPreferenceSettings>> result =
                    _BKFinancePerformPreferenceSettingService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKFinancePerformPreferenceSettings BKFinancePerformPreferenceSettings = result.Data.FirstOrDefault();
                BKFinancePerformPreferenceSettings.Operation = "update";
                BKFinancePerformPreferenceSettings.Id = id;

                return View("CreateEdit", BKFinancePerformPreferenceSettings);
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
