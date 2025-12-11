using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKFraudIrregularitiesInternalControlPreferencesCBS;
using Shampan.Core.Interfaces.Services.BKFraudIrrgularitiesPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Repository.SqlServer.BKCheckListTypes;
using Shampan.Services.BKFinancePreformPreferencesCBS;
using Shampan.Services.BKFraudIrregularitiesInternalControlPreferencesCBS;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]
	public class BKFraudIrregularitiesInternalControlPreferenceCBSController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKFraudIrregularitiesInternalControlPreferenceCBSService _bkFraudIrregularitiesInternalControlPreferenceCBSService;
        private readonly IBKFraudIrrgularitiesPreferenceSettingService _bkFraudIrrgularitiesPreferenceSettingService;

        public BKFraudIrregularitiesInternalControlPreferenceCBSController(ApplicationDbContext applicationDb, ITeamsService teamsService,
		IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService, 
        IBKFraudIrregularitiesInternalControlPreferenceCBSService bkFraudIrregularitiesInternalControlPreferenceCBSService
            , IBKFraudIrrgularitiesPreferenceSettingService bkFraudIrrgularitiesPreferenceSettingService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _bkFraudIrregularitiesInternalControlPreferenceCBSService = bkFraudIrregularitiesInternalControlPreferenceCBSService;
            _bkFraudIrrgularitiesPreferenceSettingService = bkFraudIrrgularitiesPreferenceSettingService;
        }

        public async Task<IActionResult> Itegration()
        {
            BKFraudIrregularitiesInternalControlPreferenceCBS vm = new BKFraudIrregularitiesInternalControlPreferenceCBS();
            return View(vm);
        }
        public ActionResult FraudIrregularitiesInternalControlPreferenceIntegration(BKFraudIrregularitiesInternalControlPreferenceCBS vm)
        {
            int i = 0;
            ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS> resultData = new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>();

            BKFraudIrregularitiesInternalControlPreferenceCBSRepo _repo = new BKFraudIrregularitiesInternalControlPreferenceCBSRepo();
            List<BKFraudIrregularitiesInternalControlPreferenceCBS> FraudIrregularitiesInternalControlPreference = new List<BKFraudIrregularitiesInternalControlPreferenceCBS>();

            ResultVM result = _repo.GetFraudIrregularitiesInternalControlPreferenceData(vm);

            if (result.Status == "Success" && result.DataVM != null)
            {
                FraudIrregularitiesInternalControlPreference = JsonConvert.DeserializeObject<List<BKFraudIrregularitiesInternalControlPreferenceCBS>>(result.DataVM.ToString());
            }
            else
            {
                FraudIrregularitiesInternalControlPreference = null;
            }

            if (FraudIrregularitiesInternalControlPreference != null)
            {
                _bkFraudIrrgularitiesPreferenceSettingService.Delete(0);

                foreach (BKFraudIrregularitiesInternalControlPreferenceCBS data in FraudIrregularitiesInternalControlPreference)
                {
                    resultData = _bkFraudIrrgularitiesPreferenceSettingService.FraudIrrgularitiesPreferenceTempInsert(data);
                    FraudIrregularitiesInternalControlPreference[i].ImportId = resultData.Data.ImportId;
                    i++;
                }
            }

            return Ok(new { data = FraudIrregularitiesInternalControlPreference, recordsTotal = 1000, recordsFiltered = FraudIrregularitiesInternalControlPreference.Count() });

        }

        public ActionResult SaveFraudIrregularitiesInternalControlPreferenceIntegration(BKFraudIrregularitiesInternalControlPreferenceCBS master)
        {
            ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS> result = new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>();
            ResultModel<BKFraudIrrgularitiesPreferenceSettings> resultData = new ResultModel<BKFraudIrrgularitiesPreferenceSettings>();

            try
            {
                IndexModel index = new IndexModel();
                string[] conditionalFields = new[] { "" };
                string?[] conditionalValue = new[] { "" };
                index.IDs = master.IDs;
                ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>> indexData = _bkFraudIrrgularitiesPreferenceSettingService.GetIndexDataTemp(index, conditionalFields, conditionalValue);


                foreach (BKFraudIrrgularitiesPreferenceSettings data in indexData.Data)
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    BKFraudIrrgularitiesPreferenceSettings masterData = new BKFraudIrrgularitiesPreferenceSettings();
                    masterData.Audit.CreatedBy = user.UserName;
                    masterData.Audit.CreatedOn = DateTime.Now;
                    masterData.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    masterData.BranchID = data.BranchID;
                    masterData.BKAuditOfficeTypeId = 1;
                    masterData.BKAuditOfficeId = 1;
                    masterData.PreviouslyFraudIncidentFlag = data.PreviouslyFraudIncidentFlag;
                    masterData.EmpMisConductFlag = data.EmpMisConductFlag;
                    masterData.Status = data.Status;
                    masterData.InfoReceiveId = data.ImportId.ToString();
                    masterData.AuditFiscalYear = data.AuditFiscalYear;
                    masterData.AuditYear = data.AuditYear;
                    masterData.AuditFiscalYear = data.AuditFiscalYear;

                    resultData = _bkFraudIrrgularitiesPreferenceSettingService.Insert(masterData);

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

                ResultModel<List<BKFraudIrregularitiesInternalControlPreferenceCBS>> indexData =
                    _bkFraudIrregularitiesInternalControlPreferenceCBSService.GetIndexData(index, conditionalFields, conditionalValue);

				ResultModel<int> indexDataCount =
                _bkFraudIrregularitiesInternalControlPreferenceCBSService.GetIndexDataCount(index, conditionalFields, conditionalValue);

				int result = _bkFraudIrregularitiesInternalControlPreferenceCBSService.GetCount(TableName.BKFraudIrregularitiesInternalControlPreferenceCBS, "Id", new[] { "BKFraudIrregularitiesInternalControlPreferenceCBS.createdBy", }, new[] { userName });

				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<BKFraudIrregularitiesInternalControlPreferenceCBS>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}

		}

        public ActionResult Create()
        {

            ModelState.Clear();
            BKFraudIrregularitiesInternalControlPreferenceCBS vm = new BKFraudIrregularitiesInternalControlPreferenceCBS();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }

        public ActionResult CreateEdit(BKFraudIrregularitiesInternalControlPreferenceCBS master)
        {
            ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS> result = new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>();

            try
            {


                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _bkFraudIrregularitiesInternalControlPreferenceCBSService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _bkFraudIrregularitiesInternalControlPreferenceCBSService.Insert(master);

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
                ResultModel<List<BKFraudIrregularitiesInternalControlPreferenceCBS>> result =
                    _bkFraudIrregularitiesInternalControlPreferenceCBSService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKFraudIrregularitiesInternalControlPreferenceCBS BKFraudIrregularitiesInternalControlPreferenceCBS = result.Data.FirstOrDefault();
                BKFraudIrregularitiesInternalControlPreferenceCBS.Operation = "update";
                BKFraudIrregularitiesInternalControlPreferenceCBS.Id = id;

                return View("CreateEdit", BKFraudIrregularitiesInternalControlPreferenceCBS);
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
