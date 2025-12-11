using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficesPreferenceInfos;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Repository.SqlServer.BKCheckListTypes;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class BKAuditOfficesPreferenceInfoController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKAuditOfficesPreferenceInfoService _bkAuditOfficesPreferenceInfoService;

        public BKAuditOfficesPreferenceInfoController(
        ApplicationDbContext applicationDb,
        ITeamsService teamsService,
        IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService,
        IBKAuditOfficesPreferenceInfoService bkAuditOfficesPreferenceInfoService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _bkAuditOfficesPreferenceInfoService = bkAuditOfficesPreferenceInfoService;
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
                string branchid = Request.Form["branchid"].ToString();


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

                            "BranchID",
                            "Code like",
                            "AdvanceAmount like",
                            "Description like",
                            "IsPost like"
                };

                string?[] conditionalValue = new[] {branchid, code, advanceAmount, description, post };

                ResultModel<List<BKAuditOfficesPreferenceInfo>> indexData =
                    _bkAuditOfficesPreferenceInfoService.GetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _bkAuditOfficesPreferenceInfoService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _bkAuditOfficesPreferenceInfoService.GetCount(TableName.BKRiskAssessPerferenceSettings, "Id", new[] { "BKRiskAssessPerferenceSettings.createdBy", }, new[] { userName });

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
            BKAuditOfficesPreferenceInfo vm = new BKAuditOfficesPreferenceInfo();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(BranchProfile data)
        {
            ResultModel<BKAuditOfficesPreferenceInfo> result = new ResultModel<BKAuditOfficesPreferenceInfo>();
            BKAuditOfficesPreferenceInfo master = new BKAuditOfficesPreferenceInfo();

            try
            {
                master.Id = data.BKAuditOfficesPreferenceInfo.Id;
                master.Code = data.BKAuditOfficesPreferenceInfo.Code;
                master.Edit = data.BKAuditOfficesPreferenceInfo.Edit;
                master.EntryDate = data.BKAuditOfficesPreferenceInfo.EntryDate;
                master.Status = data.BKAuditOfficesPreferenceInfo.Status;                
                master.BKAuditOfficeTypeId = data.BKAuditOfficesPreferenceInfo.BKAuditOfficeTypeId;
                master.BKAuditOfficeId = data.BKAuditOfficesPreferenceInfo.BKAuditOfficeId;
                master.HistoricalPerformFlag = data.BKAuditOfficesPreferenceInfo.HistoricalPerformFlag;
                master.BranchID = data.BKAuditOfficesPreferenceInfo.BranchID;
                master.AuditYear = data.BKAuditOfficesPreferenceInfo.AuditYear;
                master.AuditFiscalYear = data.BKAuditOfficesPreferenceInfo.AuditFiscalYear;
                master.AuditPerferenceValue = data.BKAuditOfficesPreferenceInfo.AuditPerferenceValue;
                master.EntryDate = data.BKAuditOfficesPreferenceInfo.EntryDate;

                if (data.BKAuditOfficesPreferenceInfo.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _bkAuditOfficesPreferenceInfoService.Update(master);

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
                    result = _bkAuditOfficesPreferenceInfoService.Insert(master);

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
                ResultModel<List<BKAuditOfficesPreferenceInfo>> result =
                    _bkAuditOfficesPreferenceInfoService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKAuditOfficesPreferenceInfo BKRiskAssessPerferenceSetting = result.Data.FirstOrDefault();
                BKRiskAssessPerferenceSetting.Operation = "update";
                BKRiskAssessPerferenceSetting.Id = id;

                return View("CreateEdit", BKRiskAssessPerferenceSetting);
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
