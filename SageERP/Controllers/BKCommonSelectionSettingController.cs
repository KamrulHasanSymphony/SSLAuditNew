using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKCommonSelectionSetting;
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
	public class BKCommonSelectionSettingController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKCommonSelectionSettingService _bkCommonSelectionSettingService;

        public BKCommonSelectionSettingController(
	    ApplicationDbContext applicationDb, 
	    ITeamsService teamsService,
		IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService,
        IBKCommonSelectionSettingService bkCommonSelectionSettingService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _bkCommonSelectionSettingService = bkCommonSelectionSettingService;
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

                ResultModel<List<BKCommonSelectionSettings>> indexData =
                    _bkCommonSelectionSettingService.GetIndexData(index, conditionalFields, conditionalValue);

				ResultModel<int> indexDataCount =
                _bkCommonSelectionSettingService.GetIndexDataCount(index, conditionalFields, conditionalValue);

				int result = _bkCommonSelectionSettingService.GetCount(TableName.BKCommonSelectionSettings, "Id", new[] { "BKCommonSelectionSettings.createdBy", }, new[] { userName });

				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<BKCommonSelectionSettings>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}

		}

        public ActionResult Create()
        {

            ModelState.Clear();
            BKCommonSelectionSettings vm = new BKCommonSelectionSettings();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        //public ActionResult CreateEdit(BKCommonSelectionSettings master)
        public ActionResult CreateEdit(BranchProfile data)
        {
            ResultModel<BKCommonSelectionSettings> result = new ResultModel<BKCommonSelectionSettings>();
			BKCommonSelectionSettings master = new BKCommonSelectionSettings();

            try
            {

                //ResultModel<List<BKCommonSelectionSettings>> checkData = _bkCommonSelectionSettingService.GetAll(new[] { "BranchID" }, new[] { data.BKCommonSelectionSetting.BranchID.ToString() });
                //if (checkData.Data.Count() == 0)
                //{
                //    data.BKCommonSelectionSetting.Operation = "add";
                //}
                //else
                //{
                //    data.BKCommonSelectionSetting.Operation = "update";
                //    master.Id = checkData.Data.FirstOrDefault().Id;
                //}

                master.Id = data.BKCommonSelectionSetting.Id;
                master.Code = data.BKCommonSelectionSetting.Code;
                master.BKAuditOfficeTypeId = data.BKCommonSelectionSetting.BKAuditOfficeTypeId;
                master.HitoricalPreformFlag = data.BKCommonSelectionSetting.HitoricalPreformFlag;
                master.HistoricalPerformFlagDesc = data.BKCommonSelectionSetting.HistoricalPerformFlagDesc;
                master.LastYearAuditFindingFlag = data.BKCommonSelectionSetting.LastYearAuditFindingFlag;
                master.LastYearAuditFindingFlagDesc = data.BKCommonSelectionSetting.LastYearAuditFindingFlagDesc;
                master.PreviousYearExceptLastYearAuditFindingFlag = data.BKCommonSelectionSetting.PreviousYearExceptLastYearAuditFindingFlag;
                master.PreviousYearExceptLastYearAuditFindingFlagDesc = data.BKCommonSelectionSetting.PreviousYearExceptLastYearAuditFindingFlagDesc;
                master.TechCyberRiskFlag = data.BKCommonSelectionSetting.TechCyberRiskFlag;
                master.OfficeSizeFlag = data.BKCommonSelectionSetting.OfficeSizeFlag;
                master.OfficeSignificanceFlag = data.BKCommonSelectionSetting.OfficeSignificanceFlag;
                master.TechCyberRiskFlagDesc = data.BKCommonSelectionSetting.TechCyberRiskFlagDesc;
                master.StaffTurnoverFlag = data.BKCommonSelectionSetting.StaffTurnoverFlag;
                master.StaffTurnoverFlagDesc = data.BKCommonSelectionSetting.StaffTurnoverFlagDesc;
                master.StaffTrainingGapsFlag = data.BKCommonSelectionSetting.StaffTrainingGapsFlag;

                master.StaffTrainingGapsFlagDesc = data.BKCommonSelectionSetting.StaffTrainingGapsFlagDesc;
                master.StrategicInitiativeFlagveFlag = data.BKCommonSelectionSetting.StrategicInitiativeFlagveFlag;
                master.StrategicInitiativeFlagDesc = data.BKCommonSelectionSetting.StrategicInitiativeFlagDesc;
                master.OperationalCompFlag = data.BKCommonSelectionSetting.OperationalCompFlag;
                master.OperationalCompFlagDesc = data.BKCommonSelectionSetting.OperationalCompFlagDesc;
                master.Status = data.BKCommonSelectionSetting.Status;
                master.EntryDate = data.BKCommonSelectionSetting.EntryDate;

                master.Fields1Flag = data.BKCommonSelectionSetting.Fields1Flag;
                master.Fields1FlagDesc = data.BKCommonSelectionSetting.Fields1FlagDesc;

                master.Fields2Flag = data.BKCommonSelectionSetting.Fields2Flag;
                master.Fields2FlagDesc = data.BKCommonSelectionSetting.Fields2FlagDesc;

                master.Fields3Flag = data.BKCommonSelectionSetting.Fields3Flag;
                master.Fields3FlagDesc = data.BKCommonSelectionSetting.Fields3FlagDesc;

                master.Fields4Flag = data.BKCommonSelectionSetting.Fields4Flag;
                master.Fields4FlagDesc = data.BKCommonSelectionSetting.Fields4FlagDesc;

                master.Fields5Flag = data.BKCommonSelectionSetting.Fields5Flag;
                master.Fields5FlagDesc = data.BKCommonSelectionSetting.Fields5FlagDesc;

                master.Fields6Flag = data.BKCommonSelectionSetting.Fields6Flag;
                master.Fields6FlagDesc = data.BKCommonSelectionSetting.Fields6FlagDesc;

                master.Fields7Flag = data.BKCommonSelectionSetting.Fields7Flag;
                master.Fields7FlagDesc = data.BKCommonSelectionSetting.Fields7FlagDesc;

                master.Fields8Flag = data.BKCommonSelectionSetting.Fields8Flag;
                master.Fields8FlagDesc = data.BKCommonSelectionSetting.Fields8FlagDesc;

                master.Fields9Flag = data.BKCommonSelectionSetting.Fields9Flag;
                master.Fields9FlagDesc = data.BKCommonSelectionSetting.Fields9FlagDesc;

                master.Fields10Flag = data.BKCommonSelectionSetting.Fields10Flag;
                master.Fields10FlagDesc = data.BKCommonSelectionSetting.Fields10FlagDesc;

                master.Fields11Flag = data.BKCommonSelectionSetting.Fields11Flag;
                master.Fields11FlagDesc = data.BKCommonSelectionSetting.Fields11FlagDesc;

                master.Fields1Flag = data.BKCommonSelectionSetting.Fields1Flag;
                master.Fields1FlagDesc = data.BKCommonSelectionSetting.Fields1FlagDesc;

                master.Fields12Flag = data.BKCommonSelectionSetting.Fields12Flag;
                master.Fields12FlagDesc = data.BKCommonSelectionSetting.Fields12FlagDesc;

                master.Fields13Flag = data.BKCommonSelectionSetting.Fields13Flag;
                master.Fields13FlagDesc = data.BKCommonSelectionSetting.Fields13FlagDesc;

                master.Fields1Flag = data.BKCommonSelectionSetting.Fields1Flag;
                master.Fields1FlagDesc = data.BKCommonSelectionSetting.Fields1FlagDesc;

                master.Fields14Flag = data.BKCommonSelectionSetting.Fields14Flag;
                master.Fields14FlagDesc = data.BKCommonSelectionSetting.Fields14FlagDesc;

                master.Fields15Flag = data.BKCommonSelectionSetting.Fields15Flag;
                master.Fields15FlagDesc = data.BKCommonSelectionSetting.Fields15FlagDesc;

                master.Fields16Flag = data.BKCommonSelectionSetting.Fields16Flag;
                master.Fields16FlagDesc = data.BKCommonSelectionSetting.Fields16FlagDesc;

                master.Fields17Flag = data.BKCommonSelectionSetting.Fields17Flag;
                master.Fields17FlagDesc = data.BKCommonSelectionSetting.Fields17FlagDesc;

                master.Fields18Flag = data.BKCommonSelectionSetting.Fields18Flag;
                master.Fields18FlagDesc = data.BKCommonSelectionSetting.Fields18FlagDesc;

                master.Fields19Flag = data.BKCommonSelectionSetting.Fields19Flag;
                master.Fields19FlagDesc = data.BKCommonSelectionSetting.Fields19FlagDesc;

                master.Fields20Flag = data.BKCommonSelectionSetting.Fields20Flag;
                master.Fields20FlagDesc = data.BKCommonSelectionSetting.Fields20FlagDesc;


                master.AuditYear = data.BKCommonSelectionSetting.AuditYear;
                master.AuditFiscalYear = data.BKCommonSelectionSetting.AuditFiscalYear;
                master.InfoReceiveDate = data.BKCommonSelectionSetting.InfoReceiveDate;
                master.InfoReceiveId = data.BKCommonSelectionSetting.InfoReceiveId;
                master.InfoReceiveFlag = data.BKCommonSelectionSetting.InfoReceiveFlag;


                master.BranchID = data.BKCommonSelectionSetting.BranchID;




                //if (master.Operation == "update")
                if (data.BKCommonSelectionSetting.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _bkCommonSelectionSettingService.Update(master);

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
                    result = _bkCommonSelectionSettingService.Insert(master);

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
                ResultModel<List<BKCommonSelectionSettings>> result =
                    _bkCommonSelectionSettingService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKCommonSelectionSettings BKCommonSelectionSettings = result.Data.FirstOrDefault();
                BKCommonSelectionSettings.Operation = "update";
                BKCommonSelectionSettings.Id = id;

                return View("CreateEdit", BKCommonSelectionSettings);
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
