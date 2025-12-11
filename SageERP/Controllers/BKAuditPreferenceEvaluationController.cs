using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis.Operations;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Repository.BKAuditPreferenceEvaluation;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKAuditPreferenceEvaluation;
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
	public class BKAuditPreferenceEvaluationController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IBKCheckListTypeService _BKCheckListTypeService;
        private readonly IBKAuditPreferenceEvaluationService _bkAuditPreferenceEvaluationService;

        public BKAuditPreferenceEvaluationController(
	    ApplicationDbContext applicationDb, 
	    ITeamsService teamsService,
		IAdvancesService advancesService, IBKCheckListTypeService BKCheckListTypeService,
        IBKAuditPreferenceEvaluationService bkAuditPreferenceEvaluationService

            )
        {
            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _BKCheckListTypeService = BKCheckListTypeService;
            _bkAuditPreferenceEvaluationService = bkAuditPreferenceEvaluationService;
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
                            "Code like"				
				};

				string?[] conditionalValue = new[] {branchid, search };

                ResultModel<List<BKAuditPreferenceEvaluations>> indexData =
                    _bkAuditPreferenceEvaluationService.GetIndexData(index, conditionalFields, conditionalValue);

				ResultModel<int> indexDataCount =
                _bkAuditPreferenceEvaluationService.GetIndexDataCount(index, conditionalFields, conditionalValue);

				int result = _bkAuditPreferenceEvaluationService.GetCount(TableName.BKAuditPreferenceEvaluations, "Id", new[] { "BKRiskAssessPerferenceSettings.createdBy", }, new[] { userName });

				return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
			}
			catch (Exception e)
			{
				e.LogAsync(ControllerContext.HttpContext);
				return Ok(new { Data = new List<BKAuditPreferenceEvaluations>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
			}

		}

        public ActionResult Create()
        {

            ModelState.Clear();
            BKAuditPreferenceEvaluations vm = new BKAuditPreferenceEvaluations();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(BranchProfile data)
        {
            ResultModel<BKAuditPreferenceEvaluations> result = new ResultModel<BKAuditPreferenceEvaluations>();
			BKAuditPreferenceEvaluations master = new BKAuditPreferenceEvaluations();

            try
            {

    //            ResultModel<List<BKAuditPreferenceEvaluations>> checkData = _bkAuditPreferenceEvaluationService.GetAll(new[] { "BranchID" }, new[] { data.BKAuditPreferenceEvaluation.BranchID.ToString() });              
				//if(checkData.Data.Count() == 0)
				//{
				//	data.BKAuditPreferenceEvaluation.Operation = "add";

    //            }
				//else
				//{
    //                data.BKAuditPreferenceEvaluation.Operation = "update";
    //                master.Id = checkData.Data.FirstOrDefault().Id;
    //            }
                


                master.Id = data.BKAuditPreferenceEvaluation.Id;
				master.Code = data.BKAuditPreferenceEvaluation.Code;
				master.Edit = data.BKAuditPreferenceEvaluation.Edit;
				master.EntryDate = data.BKAuditPreferenceEvaluation.EntryDate;
				master.Status = data.BKAuditPreferenceEvaluation.Status;
				master.FlagPercentFromCommonSettingSelectedValuesMin = data.BKAuditPreferenceEvaluation.FlagPercentFromCommonSettingSelectedValuesMin;
				master.FlagPercentFromCommonSettingSelectedValuesMax = data.BKAuditPreferenceEvaluation.FlagPercentFromCommonSettingSelectedValuesMax;
				master.FlagPercentFromRiskAssessSelectedValuesMin = data.BKAuditPreferenceEvaluation.FlagPercentFromRiskAssessSelectedValuesMin;
				master.FlagPercentFromRiskAssessSelectedValuesMax = data.BKAuditPreferenceEvaluation.FlagPercentFromRiskAssessSelectedValuesMax;
				master.FlagPercentFromReguCompliancesSelectedValuesMin = data.BKAuditPreferenceEvaluation.FlagPercentFromReguCompliancesSelectedValuesMin;
				master.FlagPercentFromReguComliancesSelectedValuesMax = data.BKAuditPreferenceEvaluation.FlagPercentFromReguComliancesSelectedValuesMax;
				master.FlagPercentFromFinancePerformSelectedValuesMin = data.BKAuditPreferenceEvaluation.FlagPercentFromFinancePerformSelectedValuesMin;
				master.FlagPercentFromFinancePerformSelectedValuesMax = data.BKAuditPreferenceEvaluation.FlagPercentFromFinancePerformSelectedValuesMax;
				master.FlagPercentFromFraudIrrgularitiesSelectedValuesMin = data.BKAuditPreferenceEvaluation.FlagPercentFromFraudIrrgularitiesSelectedValuesMin;
				master.FlagPercentFromFraudIrregularitiesSelectedValuesMax = data.BKAuditPreferenceEvaluation.FlagPercentFromFraudIrregularitiesSelectedValuesMax;
				master.FlagPercentFromInternalControlWeakSelectedValuesMin = data.BKAuditPreferenceEvaluation.FlagPercentFromInternalControlWeakSelectedValuesMin;
				master.FlagPercentFromInternalControlWeakSelectedValuesMax = data.BKAuditPreferenceEvaluation.FlagPercentFromInternalControlWeakSelectedValuesMax;
				master.BKAuditOfficeId = data.BKAuditPreferenceEvaluation.BKAuditOfficeId;
				master.BranchID = data.BKAuditPreferenceEvaluation.BranchID;
			

                //if (master.Operation == "update")
                if (data.BKAuditPreferenceEvaluation.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _bkAuditPreferenceEvaluationService.Update(master);

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
                    result = _bkAuditPreferenceEvaluationService.Insert(master);

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
                ResultModel<List<BKAuditPreferenceEvaluations>> result =
                    _bkAuditPreferenceEvaluationService.GetAll(new[] { "Id" }, new[] { id.ToString() });

                BKAuditPreferenceEvaluations BKAuditPreferenceEvaluations = result.Data.FirstOrDefault();
                BKAuditPreferenceEvaluations.Operation = "update";
                BKAuditPreferenceEvaluations.Id = id;

                return View("CreateEdit", BKAuditPreferenceEvaluations);
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
