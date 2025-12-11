using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKReguCompliancesPreferenceSetting;
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
	public class BKReguCompliancesPreferenceSettingController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKReguCompliancesPreferenceSettingsService _BKReguCompliancesPreferenceSettingsService;

        public BKReguCompliancesPreferenceSettingController(ApplicationDbContext applicationDb, ITeamsService teamsService,
		IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService,
        IBKReguCompliancesPreferenceSettingsService BKReguCompliancesPreferenceSettingsService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _BKReguCompliancesPreferenceSettingsService = BKReguCompliancesPreferenceSettingsService;
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

                ResultModel<List<BKReguCompliancesPreferenceSettings>> indexData =
                    _BKReguCompliancesPreferenceSettingsService.GetIndexData(index, conditionalFields, conditionalValue);

				ResultModel<int> indexDataCount =
                _BKReguCompliancesPreferenceSettingsService.GetIndexDataCount(index, conditionalFields, conditionalValue);

				int result = _BKReguCompliancesPreferenceSettingsService.GetCount(TableName.BKReguCompliancesPreferenceSettings, "Id", new[] { "BKReguCompliancesPreferenceSettings.createdBy", }, new[] { userName });

				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<BKReguCompliancesPreferenceSettings>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}

		}

        public ActionResult Create()
        {

            ModelState.Clear();
            BKReguCompliancesPreferenceSettings vm = new BKReguCompliancesPreferenceSettings();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        //public ActionResult CreateEdit(BKReguCompliancesPreferenceSettings master)
        public ActionResult CreateEdit(BranchProfile data)
        {
            ResultModel<BKReguCompliancesPreferenceSettings> result = new ResultModel<BKReguCompliancesPreferenceSettings>();
			BKReguCompliancesPreferenceSettings master = new BKReguCompliancesPreferenceSettings();
            try
            {

                //ResultModel<List<BKReguCompliancesPreferenceSettings>> checkData = _BKReguCompliancesPreferenceSettingsService.GetAll(new[] { "BranchID" }, new[] { data.BKReguCompliancesPreferenceSetting.BranchID.ToString() });
                //if (checkData.Data.Count() == 0)
                //{
                //    data.BKReguCompliancesPreferenceSetting.Operation = "add";
                //}
                //else
                //{
                //    data.BKReguCompliancesPreferenceSetting.Operation = "update";
                //    master.Id = checkData.Data.FirstOrDefault().Id;
                //}


                master.Id = data.BKReguCompliancesPreferenceSetting.Id;
                master.Code = data.BKReguCompliancesPreferenceSetting.Code;
                master.Edit = data.BKReguCompliancesPreferenceSetting.Edit;
                master.BKAuditOfficeTypeId = data.BKReguCompliancesPreferenceSetting.BKAuditOfficeTypeId;
                master.InternationTxnFlag = data.BKReguCompliancesPreferenceSetting.InternationTxnFlag;
                master.ForexFlag = data.BKReguCompliancesPreferenceSetting.ForexFlag;
                master.HighProfileClintsFlag = data.BKReguCompliancesPreferenceSetting.HighProfileClintsFlag;
                master.CorporateChientsFlag = data.BKReguCompliancesPreferenceSetting.CorporateChientsFlag;
                master.AmlFlag = data.BKReguCompliancesPreferenceSetting.AmlFlag;
                master.Status = data.BKReguCompliancesPreferenceSetting.Status;
                master.BranchID = data.BKReguCompliancesPreferenceSetting.BranchID;
                master.AuditYear = data.BKReguCompliancesPreferenceSetting.AuditYear;
                master.AuditFiscalYear = data.BKReguCompliancesPreferenceSetting.AuditFiscalYear;
                master.InfoReceiveDate = data.BKReguCompliancesPreferenceSetting.InfoReceiveDate;
                master.InfoReceiveId = data.BKReguCompliancesPreferenceSetting.InfoReceiveId;
                master.InfoReceiveFlag = data.BKReguCompliancesPreferenceSetting.InfoReceiveFlag;



                //if (master.Operation == "update")
                if (data.BKReguCompliancesPreferenceSetting.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _BKReguCompliancesPreferenceSettingsService.Update(master);

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
                    result = _BKReguCompliancesPreferenceSettingsService.Insert(master);

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
                ResultModel<List<BKReguCompliancesPreferenceSettings>> result =
                    _BKReguCompliancesPreferenceSettingsService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKReguCompliancesPreferenceSettings BKReguCompliancesPreferenceSettings = result.Data.FirstOrDefault();
                BKReguCompliancesPreferenceSettings.Operation = "update";
                BKReguCompliancesPreferenceSettings.Id = id;

                return View("CreateEdit", BKReguCompliancesPreferenceSettings);
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
