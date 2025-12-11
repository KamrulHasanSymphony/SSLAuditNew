using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKFinancePreformPreferencesCBS;
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Repository.SqlServer.BKCheckListTypes;
using Shampan.Services.BKAuditOfficePreferencesCBS;
using Shampan.Services.BKFinancePreformPreferencesCBS;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class BKFinancePreformPreferenceCBSController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKFinancePerformPreferenceSettingService _bkFinancePerformPreferenceSettingService;
        private readonly IBKFinancePreformPreferenceCBSService _bkFinancePreformPreferenceCBSService;

        public BKFinancePreformPreferenceCBSController(ApplicationDbContext applicationDb, ITeamsService teamsService,
		IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService, 
        IBKFinancePerformPreferenceSettingService bkFinancePerformPreferenceSettingService, IBKFinancePreformPreferenceCBSService bkFinancePreformPreferenceCBSService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _bkFinancePerformPreferenceSettingService = bkFinancePerformPreferenceSettingService;
            _bkFinancePreformPreferenceCBSService = bkFinancePreformPreferenceCBSService;
        }

        public async Task<IActionResult> Itegration()
        {
            BKFinancePreformPreferenceCBS vm = new BKFinancePreformPreferenceCBS();
            return View(vm);
        }
        public ActionResult AuditOfficePreferenceIntegration(BKFinancePreformPreferenceCBS vm)
        {
            int i = 0;
            ResultModel<BKFinancePreformPreferenceCBS> resultData = new ResultModel<BKFinancePreformPreferenceCBS>();

            BKFinancePreformPreferenceCBSRepo _repo = new BKFinancePreformPreferenceCBSRepo();
            List<BKFinancePreformPreferenceCBS> FinancePreformPreference = new List<BKFinancePreformPreferenceCBS>();

            ResultVM result = _repo.GetFinancePreformPreferenceData(vm);

            if (result.Status == "Success" && result.DataVM != null)
            {
                FinancePreformPreference = JsonConvert.DeserializeObject<List<BKFinancePreformPreferenceCBS>>(result.DataVM.ToString());
            }
            else
            {
                FinancePreformPreference = null;
            }

            if (FinancePreformPreference != null)
            {
                _bkFinancePerformPreferenceSettingService.Delete(0);

                foreach (BKFinancePreformPreferenceCBS data in FinancePreformPreference)
                {
                    resultData = _bkFinancePerformPreferenceSettingService.FinancePerformPreferenceTempInsert(data);
                    FinancePreformPreference[i].ImportId = resultData.Data.ImportId;
                    i++;
                }
            }

            return Ok(new { data = FinancePreformPreference, recordsTotal = 1000, recordsFiltered = FinancePreformPreference.Count() });

        }

        public ActionResult SaveFinancePreformPreferenceIntegration(BKFinancePreformPreferenceCBS master)
        {
            ResultModel<BKFinancePreformPreferenceCBS> result = new ResultModel<BKFinancePreformPreferenceCBS>();
            ResultModel<BKFinancePerformPreferenceSettings> resultData = new ResultModel<BKFinancePerformPreferenceSettings>();

            try
            {
                IndexModel index = new IndexModel();
                string[] conditionalFields = new[] { "" };
                string?[] conditionalValue = new[] { "" };
                index.IDs = master.IDs;
                ResultModel<List<BKFinancePerformPreferenceSettings>> indexData =_bkFinancePerformPreferenceSettingService.GetIndexDataTemp(index, conditionalFields, conditionalValue);


                foreach (BKFinancePerformPreferenceSettings data in indexData.Data)
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    BKFinancePerformPreferenceSettings masterData = new BKFinancePerformPreferenceSettings();
                    masterData.Audit.CreatedBy = user.UserName;
                    masterData.Audit.CreatedOn = DateTime.Now;
                    masterData.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    masterData.BranchID = data.BranchID;
                    masterData.BKAuditOfficeTypeId = 1;
                    masterData.BKAuditOfficeId = 1;
                    masterData.AuditYear = data.AuditYear;
                    masterData.AuditFiscalYear = data.AuditFiscalYear;
                    masterData.FundAvailableFlag = data.FundAvailableFlag;
                    masterData.MisManagementClinentsFlag = data.MisManagementClinentsFlag;
                    masterData.EfficienctyFlag = data.EfficienctyFlag;
                    masterData.NPLSLargeFlag = data.NPLSLargeFlag;
                    masterData.LargeTxnManageFlag = data.LargeTxnManageFlag;
                    masterData.HighValueAssetMangeFlag = data.HighValueAssetMangeFlag;
                    masterData.BudgetMgtFlag = data.BudgetMgtFlag;
                    masterData.Status = data.Status;
                    masterData.InfoReceiveId = data.ImportId.ToString();

                    resultData = _bkFinancePerformPreferenceSettingService.Insert(masterData);

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

                ResultModel<List<BKFinancePreformPreferenceCBS>> indexData =
                    _bkFinancePreformPreferenceCBSService.GetIndexData(index, conditionalFields, conditionalValue);

				ResultModel<int> indexDataCount =
                _bkFinancePreformPreferenceCBSService.GetIndexDataCount(index, conditionalFields, conditionalValue);

				int result = _bkFinancePreformPreferenceCBSService.GetCount(TableName.BKFinancePreformPreferenceCBS, "Id", new[] { "BKFinancePreformPreferenceCBS.createdBy", }, new[] { userName });

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
            BKFinancePreformPreferenceCBS vm = new BKFinancePreformPreferenceCBS();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(BKFinancePreformPreferenceCBS master)
        {
            ResultModel<BKFinancePreformPreferenceCBS> result = new ResultModel<BKFinancePreformPreferenceCBS>();

            try
            {

                if (master.Operation == "update")
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _bkFinancePreformPreferenceCBSService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _bkFinancePreformPreferenceCBSService.Insert(master);

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
                ResultModel<List<BKFinancePreformPreferenceCBS>> result =
                    _bkFinancePreformPreferenceCBSService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKFinancePreformPreferenceCBS BKFinancePreformPreferenceCBS = result.Data.FirstOrDefault();
                BKFinancePreformPreferenceCBS.Operation = "update";
                BKFinancePreformPreferenceCBS.Id = id;

                return View("CreateEdit", BKFinancePreformPreferenceCBS);
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
