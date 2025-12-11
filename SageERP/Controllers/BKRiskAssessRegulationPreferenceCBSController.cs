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
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.BKRiskAssessRegulationPreferencesCBS;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Repository.SqlServer.BKCheckListTypes;
using Shampan.Services.BKFinancePreformPreferencesCBS;
using Shampan.Services.BKRiskAssessRegulationPreferencesCBS;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class BKRiskAssessRegulationPreferenceCBSController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKRiskAssessRegulationPreferenceCBSService _bkRiskAssessRegulationPreferenceCBSService;
        private readonly IBKRiskAssessPerferenceSettingService _bkRiskAssessPerferenceSettingService;

        public BKRiskAssessRegulationPreferenceCBSController(ApplicationDbContext applicationDb, ITeamsService teamsService,
		IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService,
        IBKRiskAssessRegulationPreferenceCBSService bkRiskAssessRegulationPreferenceCBSService, IBKRiskAssessPerferenceSettingService bkRiskAssessPerferenceSettingService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _bkRiskAssessRegulationPreferenceCBSService = bkRiskAssessRegulationPreferenceCBSService;
            _bkRiskAssessPerferenceSettingService = bkRiskAssessPerferenceSettingService;
        }


        public async Task<IActionResult> Itegration()
        {
            BKRiskAssessRegulationPreferenceCBS vm = new BKRiskAssessRegulationPreferenceCBS();
            return View(vm);
        }
        public ActionResult RiskAssessRegulationIntegration(BKRiskAssessRegulationPreferenceCBS vm)
        {
            int i = 0;
            ResultModel<BKRiskAssessRegulationPreferenceCBS> resultData = new ResultModel<BKRiskAssessRegulationPreferenceCBS>();

            BKRiskAssessRegulationPreferencesCBSRepo _repo = new BKRiskAssessRegulationPreferencesCBSRepo();
            List<BKRiskAssessRegulationPreferenceCBS> RiskAssessRegulationPreference = new List<BKRiskAssessRegulationPreferenceCBS>();

            ResultVM result = _repo.GetRiskAssessRegulationPreferenceData(vm);

            if (result.Status == "Success" && result.DataVM != null)
            {
                RiskAssessRegulationPreference = JsonConvert.DeserializeObject<List<BKRiskAssessRegulationPreferenceCBS>>(result.DataVM.ToString());
            }
            else
            {
                RiskAssessRegulationPreference = null;
            }

            if (RiskAssessRegulationPreference != null)
            {
                _bkRiskAssessPerferenceSettingService.Delete(0);

                foreach (BKRiskAssessRegulationPreferenceCBS data in RiskAssessRegulationPreference)
                {
                    resultData = _bkRiskAssessPerferenceSettingService.RiskAssessPreferenceTempInsert(data);
                    RiskAssessRegulationPreference[i].ImportId = resultData.Data.ImportId;
                    i++;
                }
            }

            return Ok(new { data = RiskAssessRegulationPreference, recordsTotal = 1000, recordsFiltered = RiskAssessRegulationPreference.Count() });

        }

        public ActionResult SaveRiskAssessRegulationPreferenceIntegration(BKRiskAssessRegulationPreferenceCBS master)
        {
            ResultModel<BKRiskAssessRegulationPreferenceCBS> result = new ResultModel<BKRiskAssessRegulationPreferenceCBS>();
            ResultModel<BKRiskAssessPerferenceSetting> resultData = new ResultModel<BKRiskAssessPerferenceSetting>();

            try
            {
                IndexModel index = new IndexModel();
                string[] conditionalFields = new[] { "" };
                string?[] conditionalValue = new[] { "" };
                index.IDs = master.IDs;
                ResultModel<List<BKRiskAssessPerferenceSetting>> indexData = _bkRiskAssessPerferenceSettingService.GetIndexDataTemp(index, conditionalFields, conditionalValue);


                foreach (BKRiskAssessPerferenceSetting data in indexData.Data)
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    BKRiskAssessPerferenceSetting masterData = new BKRiskAssessPerferenceSetting();
                    masterData.Audit.CreatedBy = user.UserName;
                    masterData.Audit.CreatedOn = DateTime.Now;
                    masterData.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    masterData.BranchID = data.BranchID;
                    masterData.BKAuditOfficeTypeId = 1;
                    masterData.BKAuditOfficeId = 1;
                    masterData.AuditYear = data.AuditYear;
                    masterData.AuditFiscalYear = data.AuditFiscalYear;
                    masterData.Amount = data.Amount;
                    masterData.RiskLocFlag = data.RiskLocFlag;
                    masterData.EntryDate = data.AuditYear;
                    masterData.Status = data.Status;

                    resultData = _bkRiskAssessPerferenceSettingService.Insert(masterData);

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

                ResultModel<List<BKRiskAssessRegulationPreferenceCBS>> indexData =
                    _bkRiskAssessRegulationPreferenceCBSService.GetIndexData(index, conditionalFields, conditionalValue);

				ResultModel<int> indexDataCount =
                _bkRiskAssessRegulationPreferenceCBSService.GetIndexDataCount(index, conditionalFields, conditionalValue);

				int result = _bkRiskAssessRegulationPreferenceCBSService.GetCount(TableName.BKRiskAssessRegulationPreferenceCBS, "Id", new[] { "BKRiskAssessRegulationPreferenceCBS.createdBy", }, new[] { userName });

				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<BKRiskAssessRegulationPreferenceCBS>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}

		}

        public ActionResult Create()
        {

            ModelState.Clear();
            BKRiskAssessRegulationPreferenceCBS vm = new BKRiskAssessRegulationPreferenceCBS();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(BKRiskAssessRegulationPreferenceCBS master)
        {
            ResultModel<BKRiskAssessRegulationPreferenceCBS> result = new ResultModel<BKRiskAssessRegulationPreferenceCBS>();

            try
            {
                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _bkRiskAssessRegulationPreferenceCBSService.Update(master);

                    return Ok(result);

                }
                else
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _bkRiskAssessRegulationPreferenceCBSService.Insert(master);

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
                ResultModel<List<BKRiskAssessRegulationPreferenceCBS>> result =
                    _bkRiskAssessRegulationPreferenceCBSService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKRiskAssessRegulationPreferenceCBS BKRiskAssessRegulationPreferenceCBS = result.Data.FirstOrDefault();
                BKRiskAssessRegulationPreferenceCBS.Operation = "update";
                BKRiskAssessRegulationPreferenceCBS.Id = id;

                return View("CreateEdit", BKRiskAssessRegulationPreferenceCBS);
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
