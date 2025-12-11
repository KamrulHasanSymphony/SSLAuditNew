using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKInternalControlWeakPreferenceSetting;
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
	public class BKInternalControlWeakPreferenceSettingController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKInternalControlWeakPreferenceSettingService _BKInternalControlWeakPreferenceSettingService;

        public BKInternalControlWeakPreferenceSettingController(ApplicationDbContext applicationDb, ITeamsService teamsService,
		IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService,
        IBKInternalControlWeakPreferenceSettingService BKInternalControlWeakPreferenceSettingService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _BKInternalControlWeakPreferenceSettingService = BKInternalControlWeakPreferenceSettingService;
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

                ResultModel<List<BKInternalControlWeakPreferenceSettings>> indexData =
                    _BKInternalControlWeakPreferenceSettingService.GetIndexData(index, conditionalFields, conditionalValue);

				ResultModel<int> indexDataCount =
                _BKInternalControlWeakPreferenceSettingService.GetIndexDataCount(index, conditionalFields, conditionalValue);

				int result = _BKInternalControlWeakPreferenceSettingService.GetCount(TableName.BKRiskAssessPerferenceSettings, "Id", new[] { "BKRiskAssessPerferenceSettings.createdBy", }, new[] { userName });

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
            BKInternalControlWeakPreferenceSettings vm = new BKInternalControlWeakPreferenceSettings();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        //public ActionResult CreateEdit(BKInternalControlWeakPreferenceSettings master)
        public ActionResult CreateEdit(BranchProfile data)
        {
            ResultModel<BKInternalControlWeakPreferenceSettings> result = new ResultModel<BKInternalControlWeakPreferenceSettings>();
			BKInternalControlWeakPreferenceSettings master = new BKInternalControlWeakPreferenceSettings();

            try
            {
                //ResultModel<List<BKInternalControlWeakPreferenceSettings>> checkData = _BKInternalControlWeakPreferenceSettingService.GetAll(new[] { "BranchID" }, new[] { data.BKInternalControlWeakPreferenceSetting.BranchID.ToString() });
                //if (checkData.Data.Count() == 0)
                //{
                //    data.BKInternalControlWeakPreferenceSetting.Operation = "add";
                //}
                //else
                //{
                //    data.BKInternalControlWeakPreferenceSetting.Operation = "update";
                //    master.Id = checkData.Data.FirstOrDefault().Id;
                //}

                master.Id = data.BKInternalControlWeakPreferenceSetting.Id;
                master.Code = data.BKInternalControlWeakPreferenceSetting.Code;
                master.Edit = data.BKInternalControlWeakPreferenceSetting.Edit;
                master.BKAuditOfficeTypeId = data.BKInternalControlWeakPreferenceSetting.BKAuditOfficeTypeId;
                master.InternalControlFlag = data.BKInternalControlWeakPreferenceSetting.InternalControlFlag;
                master.ProperDocumentationFlag = data.BKInternalControlWeakPreferenceSetting.ProperDocumentationFlag;
                master.ProperReportingFlag = data.BKInternalControlWeakPreferenceSetting.ProperReportingFlag;
                master.Status = data.BKInternalControlWeakPreferenceSetting.Status;
                master.BranchID = data.BKInternalControlWeakPreferenceSetting.BranchID;


                master.AuditYear = data.BKInternalControlWeakPreferenceSetting.AuditYear;
                master.AuditFiscalYear = data.BKInternalControlWeakPreferenceSetting.AuditFiscalYear;
                master.InfoReceiveDate = data.BKInternalControlWeakPreferenceSetting.InfoReceiveDate;
                master.InfoReceiveId = data.BKInternalControlWeakPreferenceSetting.InfoReceiveId;
                master.InfoReceiveFlag = data.BKInternalControlWeakPreferenceSetting.InfoReceiveFlag;


                //if (master.Operation == "update")
                if (data.BKInternalControlWeakPreferenceSetting.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _BKInternalControlWeakPreferenceSettingService.Update(master);

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
                    result = _BKInternalControlWeakPreferenceSettingService.Insert(master);

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
                ResultModel<List<BKInternalControlWeakPreferenceSettings>> result =
                    _BKInternalControlWeakPreferenceSettingService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKInternalControlWeakPreferenceSettings BKInternalControlWeakPreferenceSettings = result.Data.FirstOrDefault();
                BKInternalControlWeakPreferenceSettings.Operation = "update";
                BKInternalControlWeakPreferenceSettings.Id = id;

                return View("CreateEdit", BKInternalControlWeakPreferenceSettings);
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
