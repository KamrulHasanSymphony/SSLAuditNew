using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Core;
using Shampan.Core.Interfaces.Services.BKAuditOfficesPreferenceInfos;
using Shampan.Core.Interfaces.Services.BKAuditPreferenceEvaluation;
using Shampan.Core.Interfaces.Services.BKCommonSelectionSetting;
using Shampan.Core.Interfaces.Services.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKFraudIrrgularitiesPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKInternalControlWeakPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKReguCompliancesPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Models;
using Shampan.Services.BKRiskAssessPerferenceSettings;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using StackExchange.Exceptional;

namespace SageERP.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class BranchProfileController : Controller
    {

        private readonly ApplicationDbContext _applicationDb;
        private readonly IBranchProfileService _branchProfileService;
        private readonly IBKAuditPreferenceEvaluationService _bkAuditPreferenceEvaluationService;
        private readonly IBKAuditOfficesPreferenceInfoService _bkAuditOfficesPreferenceInfoService;
        private readonly IBKFinancePerformPreferenceSettingService _BKFinancePerformPreferenceSettingService;
        private readonly IBKFraudIrrgularitiesPreferenceSettingService _BKFraudIrrgularitiesPreferenceSettingService;
        private readonly IBKInternalControlWeakPreferenceSettingService _BKInternalControlWeakPreferenceSettingService;
        private readonly IBKReguCompliancesPreferenceSettingsService _BKReguCompliancesPreferenceSettingsService;
        private readonly IBKRiskAssessPerferenceSettingService _BKRiskAssessPerferenceSettingService;
        private readonly IBKCommonSelectionSettingService _bkCommonSelectionSettingService;
        private readonly ISettingsService _settingsService;







        public BranchProfileController(ApplicationDbContext applicationDb,
            IBranchProfileService branchProfileService,
        IBKAuditPreferenceEvaluationService bkAuditPreferenceEvaluationService, 
        IBKAuditOfficesPreferenceInfoService bkAuditOfficesPreferenceInfoService
      , IBKFinancePerformPreferenceSettingService BKFinancePerformPreferenceSettingService,
        IBKFraudIrrgularitiesPreferenceSettingService BKFraudIrrgularitiesPreferenceSettingService,
        IBKInternalControlWeakPreferenceSettingService BKInternalControlWeakPreferenceSettingService, 
        IBKReguCompliancesPreferenceSettingsService BKReguCompliancesPreferenceSettingsService,
        IBKRiskAssessPerferenceSettingService BKRiskAssessPerferenceSettingService, 
        IBKCommonSelectionSettingService BKCommonSelectionSettingService, ISettingsService settingsService

            )
        {

            _applicationDb = applicationDb;
            _branchProfileService = branchProfileService;
            _bkAuditPreferenceEvaluationService = bkAuditPreferenceEvaluationService;
            _bkAuditOfficesPreferenceInfoService = bkAuditOfficesPreferenceInfoService;
            _BKFinancePerformPreferenceSettingService = BKFinancePerformPreferenceSettingService;
            _BKFraudIrrgularitiesPreferenceSettingService = BKFraudIrrgularitiesPreferenceSettingService;
            _BKInternalControlWeakPreferenceSettingService = BKInternalControlWeakPreferenceSettingService;
            _BKReguCompliancesPreferenceSettingsService = BKReguCompliancesPreferenceSettingsService;
            _BKRiskAssessPerferenceSettingService = BKRiskAssessPerferenceSettingService;
            _bkCommonSelectionSettingService = BKCommonSelectionSettingService;
            _settingsService = settingsService;


        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });


            ModelState.Clear();
            BranchProfile vm = new BranchProfile();
            vm.Operation = "add";
            ViewBag.CompanyCode = companyCode.Data.ToUpper();


            if (companyCode.Data.ToUpper() == "BRAC")
            {


                vm.BKAuditPreferenceEvaluation.Operation = "add";
                vm.BKAuditPreferenceEvaluation.OperationStatus = "evaluation";

                vm.BKAuditOfficesPreferenceInfo.Operation = "add";
                vm.BKAuditOfficesPreferenceInfo.OperationStatus = "OfficePreference";

                vm.BKFinancePerformPreferenceSetting.Operation = "add";
                vm.BKFinancePerformPreferenceSetting.OperationStatus = "FinancePerForm";

                vm.BKFraudIrrgularitiesPreferenceSetting.Operation = "add";
                vm.BKFraudIrrgularitiesPreferenceSetting.OperationStatus = "FraudIrrgularities";

                vm.BKInternalControlWeakPreferenceSetting.Operation = "add";
                vm.BKInternalControlWeakPreferenceSetting.OperationStatus = "InternalControl";

                vm.BKReguCompliancesPreferenceSetting.Operation = "add";
                vm.BKReguCompliancesPreferenceSetting.OperationStatus = "ReguCompliances";

                vm.BKRiskAssessPerferenceSetting.Operation = "add";
                vm.BKRiskAssessPerferenceSetting.OperationStatus = "RiskAssessPerference";

                vm.BKCommonSelectionSetting.Operation = "add";
                vm.BKCommonSelectionSetting.OperationStatus = "CommonSelection";

            }

            return View("CreateEdit", vm);

        }
        public ActionResult CreateEdit(BranchProfile master)
        {
            ResultModel<BranchProfile> result = new ResultModel<BranchProfile>();
            try
            {

                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _branchProfileService.Update(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _branchProfileService.Insert(master);

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
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

                string draw = Request.Form["draw"].ToString();
                var startRec = Request.Form["start"].FirstOrDefault();
                var pageSize = Request.Form["length"].FirstOrDefault();
                var orderName = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][Name]"].FirstOrDefault();
                var orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
                index.SearchValue = Request.Form["search[value]"].FirstOrDefault();
                index.OrderName = "BranchID";
                index.orderDir = orderDir;
                index.startRec = Convert.ToInt32(startRec);
                index.pageSize = Convert.ToInt32(pageSize);


                index.createdBy = userName;


                string[] conditionalFields = new[]
                          {


                            "BranchCode like",
                            "BranchName like",

                            "Address like",
                            "TelephoneNo like",

                             "BranchCode like",
                            "BranchName like",

                            "Address like",
                            "TelephoneNo like"

                          };

                string?[] conditionalValue = new[] { search, search, search, search, users, branch, Address, TelephoneNo };
                ResultModel<List<BranchProfile>> indexData =
                    _branchProfileService.GetIndexData(index,
                        conditionalFields, conditionalValue);


                ResultModel<int> indexDataCount =
                    _branchProfileService.GetIndexDataCount(index,
                       conditionalFields, conditionalValue);

                int result = _branchProfileService.GetCount(TableName.BranchProfile, "BranchID", new[]
                        {
                            "BranchProfile.createdBy",
                        }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<BranchProfile>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }


        public ActionResult Edit(int id, int? commonId = 0, string? status = null)
        {
            try
            {
                ResultModel<List<BranchProfile>> result = _branchProfileService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });


                BranchProfile branchProfile = result.Data.FirstOrDefault();
                if (branchProfile != null)
                {
                    branchProfile.Operation = "update";
                    branchProfile.BranchID = id;
                }

                ViewBag.CompanyCode = companyCode.Data.ToUpper();

                if (companyCode.Data.ToUpper() == "BRAC")
                {

                    //BKAuditPreferenceEvaluation
                    branchProfile.BKAuditPreferenceEvaluation.BranchID = id;

                    if (status == "evaluation")
                    {
                        if (commonId != 0)
                        {

                            //ResultModel<List<BKAuditPreferenceEvaluations>> evaluationData = _bkAuditPreferenceEvaluationService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                            ResultModel<List<BKAuditPreferenceEvaluations>> evaluationData = _bkAuditPreferenceEvaluationService.GetAll(new[] { "Id" }, new[] { commonId.ToString() });
                            BKAuditPreferenceEvaluations BKAuditPreferenceEvaluations = evaluationData.Data.FirstOrDefault();

                            if (BKAuditPreferenceEvaluations != null)
                            {
                                branchProfile.BKAuditPreferenceEvaluation.Code = BKAuditPreferenceEvaluations.Code;
                                branchProfile.BKAuditPreferenceEvaluation.Operation = "update";
                                branchProfile.BKAuditPreferenceEvaluation.Id = BKAuditPreferenceEvaluations.Id;
                                branchProfile.BKAuditPreferenceEvaluation.Edit = BKAuditPreferenceEvaluations.Edit;
                                branchProfile.BKAuditPreferenceEvaluation.EntryDate = BKAuditPreferenceEvaluations.EntryDate;
                                branchProfile.BKAuditPreferenceEvaluation.OperationStatus = status;
                                branchProfile.BKAuditPreferenceEvaluation.Status = BKAuditPreferenceEvaluations.Status;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromCommonSettingSelectedValuesMin = BKAuditPreferenceEvaluations.FlagPercentFromCommonSettingSelectedValuesMin;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromCommonSettingSelectedValuesMax = BKAuditPreferenceEvaluations.FlagPercentFromCommonSettingSelectedValuesMax;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromRiskAssessSelectedValuesMin = BKAuditPreferenceEvaluations.FlagPercentFromRiskAssessSelectedValuesMin;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromRiskAssessSelectedValuesMax = BKAuditPreferenceEvaluations.FlagPercentFromRiskAssessSelectedValuesMax;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromReguCompliancesSelectedValuesMin = BKAuditPreferenceEvaluations.FlagPercentFromReguCompliancesSelectedValuesMin;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromReguComliancesSelectedValuesMax = BKAuditPreferenceEvaluations.FlagPercentFromReguComliancesSelectedValuesMax;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromFinancePerformSelectedValuesMin = BKAuditPreferenceEvaluations.FlagPercentFromFinancePerformSelectedValuesMin;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromFinancePerformSelectedValuesMax = BKAuditPreferenceEvaluations.FlagPercentFromFinancePerformSelectedValuesMax;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromFraudIrrgularitiesSelectedValuesMin = BKAuditPreferenceEvaluations.FlagPercentFromFraudIrrgularitiesSelectedValuesMin;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromFraudIrregularitiesSelectedValuesMax = BKAuditPreferenceEvaluations.FlagPercentFromFraudIrregularitiesSelectedValuesMax;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromInternalControlWeakSelectedValuesMin = BKAuditPreferenceEvaluations.FlagPercentFromInternalControlWeakSelectedValuesMin;
                                branchProfile.BKAuditPreferenceEvaluation.FlagPercentFromInternalControlWeakSelectedValuesMax = BKAuditPreferenceEvaluations.FlagPercentFromInternalControlWeakSelectedValuesMax;
                                branchProfile.BKAuditPreferenceEvaluation.BKAuditOfficeId = BKAuditPreferenceEvaluations.BKAuditOfficeId;
                                branchProfile.BKAuditPreferenceEvaluation.BranchID = BKAuditPreferenceEvaluations.BranchID;
                            }
                        }
                        else
                        {
                            branchProfile.BKAuditPreferenceEvaluation.Operation = "add";
                            branchProfile.BKAuditPreferenceEvaluation.OperationStatus = status;

                        }
                    }

                    //End

                    //BKAuditOfficesPreferenceInfo
                    branchProfile.BKAuditOfficesPreferenceInfo.BranchID = id;

                    if (status == "OfficePreference")
                    {
                        if (commonId != 0)
                        {
                            //ResultModel<List<BKAuditOfficesPreferenceInfo>> BKAuditOfficesPreferenceInfo = _bkAuditOfficesPreferenceInfoService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                            ResultModel<List<BKAuditOfficesPreferenceInfo>> BKAuditOfficesPreferenceInfo = _bkAuditOfficesPreferenceInfoService.GetAll(new[] { "Id" }, new[] { commonId.ToString() });
                            BKAuditOfficesPreferenceInfo PreferenceInfo = BKAuditOfficesPreferenceInfo.Data.FirstOrDefault();
                            if (PreferenceInfo != null)
                            {
                                branchProfile.BKAuditOfficesPreferenceInfo.Operation = "update";
                                branchProfile.BKAuditOfficesPreferenceInfo.Id = PreferenceInfo.Id;
                                branchProfile.BKAuditOfficesPreferenceInfo.Code = PreferenceInfo.Code;
                                branchProfile.BKAuditOfficesPreferenceInfo.Edit = PreferenceInfo.Edit;
                                branchProfile.BKAuditOfficesPreferenceInfo.OperationStatus = status;
                                branchProfile.BKAuditOfficesPreferenceInfo.BKAuditOfficeTypeId = PreferenceInfo.BKAuditOfficeTypeId;
                                branchProfile.BKAuditOfficesPreferenceInfo.HistoricalPerformFlag = PreferenceInfo.HistoricalPerformFlag;
                                branchProfile.BKAuditOfficesPreferenceInfo.AuditYear = PreferenceInfo.AuditYear;
                                branchProfile.BKAuditOfficesPreferenceInfo.AuditFiscalYear = PreferenceInfo.AuditFiscalYear;
                                branchProfile.BKAuditOfficesPreferenceInfo.AuditPerferenceValue = PreferenceInfo.AuditPerferenceValue;
                                branchProfile.BKAuditOfficesPreferenceInfo.EntryDate = PreferenceInfo.EntryDate;
                                branchProfile.BKAuditOfficesPreferenceInfo.Status = PreferenceInfo.Status;

                            }
                        }
                        else
                        {
                            branchProfile.BKAuditOfficesPreferenceInfo.Operation = "add";
                            branchProfile.BKAuditOfficesPreferenceInfo.OperationStatus = status;

                        }
                    }

                    //End

                    //BKFinancePerformPreferenceSetting
                    branchProfile.BKFinancePerformPreferenceSetting.BranchID = id;
                    if (status == "FinancePerForm")
                    {
                        if (commonId != 0)
                        {
                            //ResultModel<List<BKFinancePerformPreferenceSettings>> BKFinancePerformPreferenceSetting = _BKFinancePerformPreferenceSettingService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                            ResultModel<List<BKFinancePerformPreferenceSettings>> BKFinancePerformPreferenceSetting = _BKFinancePerformPreferenceSettingService.GetAll(new[] { "Id" }, new[] { commonId.ToString() });
                            BKFinancePerformPreferenceSettings FinancePerformPreferenceSettingData = BKFinancePerformPreferenceSetting.Data.FirstOrDefault();

                            if (FinancePerformPreferenceSettingData != null)
                            {
                                branchProfile.BKFinancePerformPreferenceSetting.Operation = "update";
                                branchProfile.BKFinancePerformPreferenceSetting.Id = FinancePerformPreferenceSettingData.Id;
                                branchProfile.BKFinancePerformPreferenceSetting.BKAuditOfficeTypeId = FinancePerformPreferenceSettingData.BKAuditOfficeTypeId;
                                branchProfile.BKFinancePerformPreferenceSetting.FundAvailableFlag = FinancePerformPreferenceSettingData.FundAvailableFlag;
                                branchProfile.BKFinancePerformPreferenceSetting.MisManagementClinentsFlag = FinancePerformPreferenceSettingData.MisManagementClinentsFlag;
                                branchProfile.BKFinancePerformPreferenceSetting.EfficienctyFlag = FinancePerformPreferenceSettingData.EfficienctyFlag;
                                branchProfile.BKFinancePerformPreferenceSetting.NPLSLargeFlag = FinancePerformPreferenceSettingData.NPLSLargeFlag;
                                branchProfile.BKFinancePerformPreferenceSetting.LargeTxnManageFlag = FinancePerformPreferenceSettingData.LargeTxnManageFlag;
                                branchProfile.BKFinancePerformPreferenceSetting.HighValueAssetMangeFlag = FinancePerformPreferenceSettingData.HighValueAssetMangeFlag;
                                branchProfile.BKFinancePerformPreferenceSetting.BudgetMgtFlag = FinancePerformPreferenceSettingData.BudgetMgtFlag;
                                branchProfile.BKFinancePerformPreferenceSetting.Status = FinancePerformPreferenceSettingData.Status;
                                branchProfile.BKFinancePerformPreferenceSetting.OperationStatus = status;


                                branchProfile.BKFinancePerformPreferenceSetting.AuditYear = FinancePerformPreferenceSettingData.AuditYear;
                                branchProfile.BKFinancePerformPreferenceSetting.AuditFiscalYear = FinancePerformPreferenceSettingData.AuditFiscalYear;
                                branchProfile.BKFinancePerformPreferenceSetting.InfoReceiveDate = FinancePerformPreferenceSettingData.InfoReceiveDate;
                                branchProfile.BKFinancePerformPreferenceSetting.InfoReceiveId = FinancePerformPreferenceSettingData.InfoReceiveId;
                                branchProfile.BKFinancePerformPreferenceSetting.InfoReceiveFlag = FinancePerformPreferenceSettingData.InfoReceiveFlag;
                            }
                        }
                        else
                        {
                            branchProfile.BKFinancePerformPreferenceSetting.Operation = "add";
                            branchProfile.BKFinancePerformPreferenceSetting.OperationStatus = status;

                        }
                    }

                    //End

                    //BKFraudIrrgularitiesPreferenceSettings

                    branchProfile.BKFraudIrrgularitiesPreferenceSetting.BranchID = id;

                    if (status == "FraudIrrgularities")
                    {

                        if (commonId != 0)
                        {
                            //ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>> BKFraudIrrgularitiesPreferenceSetting = _BKFraudIrrgularitiesPreferenceSettingService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                            ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>> BKFraudIrrgularitiesPreferenceSetting = _BKFraudIrrgularitiesPreferenceSettingService.GetAll(new[] { "Id" }, new[] { commonId.ToString() });
                            BKFraudIrrgularitiesPreferenceSettings BKFraudIrrgularitiesPreferenceSettingData = BKFraudIrrgularitiesPreferenceSetting.Data.FirstOrDefault();

                            if (BKFraudIrrgularitiesPreferenceSettingData != null)
                            {
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.Id = BKFraudIrrgularitiesPreferenceSettingData.Id;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.Edit = BKFraudIrrgularitiesPreferenceSettingData.Edit;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.BKAuditOfficeTypeId = BKFraudIrrgularitiesPreferenceSettingData.BKAuditOfficeTypeId;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.PreviouslyFraudIncidentFlag = BKFraudIrrgularitiesPreferenceSettingData.PreviouslyFraudIncidentFlag;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.EmpMisConductFlag = BKFraudIrrgularitiesPreferenceSettingData.EmpMisConductFlag;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.Status = BKFraudIrrgularitiesPreferenceSettingData.Status;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.OperationStatus = status;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.Operation = "update";
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.AuditYear = BKFraudIrrgularitiesPreferenceSettingData.AuditYear;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.AuditFiscalYear = BKFraudIrrgularitiesPreferenceSettingData.AuditFiscalYear;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.InfoReceiveDate = BKFraudIrrgularitiesPreferenceSettingData.InfoReceiveDate;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.InfoReceiveId = BKFraudIrrgularitiesPreferenceSettingData.InfoReceiveId;
                                branchProfile.BKFraudIrrgularitiesPreferenceSetting.InfoReceiveFlag = BKFraudIrrgularitiesPreferenceSettingData.InfoReceiveFlag;

                            }
                        }
                        else
                        {
                            branchProfile.BKFraudIrrgularitiesPreferenceSetting.Operation = "add";
                            branchProfile.BKFraudIrrgularitiesPreferenceSetting.OperationStatus = status;

                        }
                    }

                    //End

                    //BKInternalControlWeakPreferenceSettings

                    branchProfile.BKInternalControlWeakPreferenceSetting.BranchID = id;

                    if (status == "InternalControl")
                    {
                        if (commonId != 0)
                        {
                            //ResultModel<List<BKInternalControlWeakPreferenceSettings>> BKInternalControlWeakPreferenceSettings = _BKInternalControlWeakPreferenceSettingService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                            ResultModel<List<BKInternalControlWeakPreferenceSettings>> BKInternalControlWeakPreferenceSettings = _BKInternalControlWeakPreferenceSettingService.GetAll(new[] { "Id" }, new[] { commonId.ToString() });
                            BKInternalControlWeakPreferenceSettings BKInternalControlWeakPreferenceSettingData = BKInternalControlWeakPreferenceSettings.Data.FirstOrDefault();

                            if (BKInternalControlWeakPreferenceSettingData != null)
                            {

                                branchProfile.BKInternalControlWeakPreferenceSetting.Id = BKInternalControlWeakPreferenceSettingData.Id;
                                branchProfile.BKInternalControlWeakPreferenceSetting.Code = BKInternalControlWeakPreferenceSettingData.Code;
                                branchProfile.BKInternalControlWeakPreferenceSetting.Edit = BKInternalControlWeakPreferenceSettingData.Edit;
                                branchProfile.BKInternalControlWeakPreferenceSetting.BKAuditOfficeTypeId = BKInternalControlWeakPreferenceSettingData.BKAuditOfficeTypeId;
                                branchProfile.BKInternalControlWeakPreferenceSetting.InternalControlFlag = BKInternalControlWeakPreferenceSettingData.InternalControlFlag;
                                branchProfile.BKInternalControlWeakPreferenceSetting.ProperDocumentationFlag = BKInternalControlWeakPreferenceSettingData.ProperDocumentationFlag;
                                branchProfile.BKInternalControlWeakPreferenceSetting.ProperReportingFlag = BKInternalControlWeakPreferenceSettingData.ProperReportingFlag;
                                branchProfile.BKInternalControlWeakPreferenceSetting.Status = BKInternalControlWeakPreferenceSettingData.Status;
                                branchProfile.BKInternalControlWeakPreferenceSetting.OperationStatus = status;
                                branchProfile.BKInternalControlWeakPreferenceSetting.Operation = "update";
                                branchProfile.BKInternalControlWeakPreferenceSetting.AuditYear = BKInternalControlWeakPreferenceSettingData.AuditYear;
                                branchProfile.BKInternalControlWeakPreferenceSetting.AuditFiscalYear = BKInternalControlWeakPreferenceSettingData.AuditFiscalYear;
                                branchProfile.BKInternalControlWeakPreferenceSetting.InfoReceiveDate = BKInternalControlWeakPreferenceSettingData.InfoReceiveDate;
                                branchProfile.BKInternalControlWeakPreferenceSetting.InfoReceiveId = BKInternalControlWeakPreferenceSettingData.InfoReceiveId;
                                branchProfile.BKInternalControlWeakPreferenceSetting.InfoReceiveFlag = BKInternalControlWeakPreferenceSettingData.InfoReceiveFlag;


                            }

                        }

                        else
                        {
                            branchProfile.BKInternalControlWeakPreferenceSetting.Operation = "add";
                            branchProfile.BKInternalControlWeakPreferenceSetting.OperationStatus = status;

                        }
                    }
                    //End

                    //BKReguCompliancesPreferenceSettings

                    branchProfile.BKReguCompliancesPreferenceSetting.BranchID = id;
                    if (status == "ReguCompliances")
                    {
                        if (commonId != 0)
                        {

                            ResultModel<List<BKReguCompliancesPreferenceSettings>> BKReguCompliancesPreferenceSettings = _BKReguCompliancesPreferenceSettingsService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                            BKReguCompliancesPreferenceSettings BKReguCompliancesPreferenceSettingsdata = BKReguCompliancesPreferenceSettings.Data.FirstOrDefault();

                            if (BKReguCompliancesPreferenceSettingsdata != null)
                            {
                                branchProfile.BKReguCompliancesPreferenceSetting.Id = BKReguCompliancesPreferenceSettingsdata.Id;
                                branchProfile.BKReguCompliancesPreferenceSetting.Code = BKReguCompliancesPreferenceSettingsdata.Code;
                                branchProfile.BKReguCompliancesPreferenceSetting.Edit = BKReguCompliancesPreferenceSettingsdata.Edit;
                                branchProfile.BKReguCompliancesPreferenceSetting.BKAuditOfficeTypeId = BKReguCompliancesPreferenceSettingsdata.BKAuditOfficeTypeId;
                                branchProfile.BKReguCompliancesPreferenceSetting.InternationTxnFlag = BKReguCompliancesPreferenceSettingsdata.InternationTxnFlag;
                                branchProfile.BKReguCompliancesPreferenceSetting.ForexFlag = BKReguCompliancesPreferenceSettingsdata.ForexFlag;
                                branchProfile.BKReguCompliancesPreferenceSetting.HighProfileClintsFlag = BKReguCompliancesPreferenceSettingsdata.HighProfileClintsFlag;
                                branchProfile.BKReguCompliancesPreferenceSetting.CorporateChientsFlag = BKReguCompliancesPreferenceSettingsdata.CorporateChientsFlag;
                                branchProfile.BKReguCompliancesPreferenceSetting.AmlFlag = BKReguCompliancesPreferenceSettingsdata.AmlFlag;
                                branchProfile.BKReguCompliancesPreferenceSetting.Status = BKReguCompliancesPreferenceSettingsdata.Status;
                                branchProfile.BKReguCompliancesPreferenceSetting.OperationStatus = status;
                                branchProfile.BKReguCompliancesPreferenceSetting.Operation = "update";
                                branchProfile.BKReguCompliancesPreferenceSetting.AuditYear = BKReguCompliancesPreferenceSettingsdata.AuditYear;
                                branchProfile.BKReguCompliancesPreferenceSetting.AuditFiscalYear = BKReguCompliancesPreferenceSettingsdata.AuditFiscalYear;
                                branchProfile.BKReguCompliancesPreferenceSetting.InfoReceiveDate = BKReguCompliancesPreferenceSettingsdata.InfoReceiveDate;
                                branchProfile.BKReguCompliancesPreferenceSetting.InfoReceiveId = BKReguCompliancesPreferenceSettingsdata.InfoReceiveId;
                                branchProfile.BKReguCompliancesPreferenceSetting.InfoReceiveFlag = BKReguCompliancesPreferenceSettingsdata.InfoReceiveFlag;

                            }
                        }

                        else
                        {
                            branchProfile.BKReguCompliancesPreferenceSetting.Operation = "add";
                            branchProfile.BKReguCompliancesPreferenceSetting.OperationStatus = status;

                        }

                    }

                    //End


                    //BKRiskAssessPerferenceSetting
                    branchProfile.BKRiskAssessPerferenceSetting.BranchID = id;

                    if (status == "RiskAssessPerference")
                    {
                        if (commonId != 0)
                        {
                            //ResultModel<List<BKRiskAssessPerferenceSetting>> BKRiskAssessPerferenceSetting = _BKRiskAssessPerferenceSettingService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                            ResultModel<List<BKRiskAssessPerferenceSetting>> BKRiskAssessPerferenceSetting = _BKRiskAssessPerferenceSettingService.GetAll(new[] { "Id" }, new[] { commonId.ToString() });
                            BKRiskAssessPerferenceSetting BKRiskAssessPerferenceSettingdata = BKRiskAssessPerferenceSetting.Data.FirstOrDefault();

                            if (BKRiskAssessPerferenceSettingdata != null)
                            {
                                branchProfile.BKRiskAssessPerferenceSetting.Id = BKRiskAssessPerferenceSettingdata.Id;
                                branchProfile.BKRiskAssessPerferenceSetting.Code = BKRiskAssessPerferenceSettingdata.Code;
                                branchProfile.BKRiskAssessPerferenceSetting.Edit = BKRiskAssessPerferenceSettingdata.Edit;
                                branchProfile.BKRiskAssessPerferenceSetting.BKAuditOfficeTypeId = BKRiskAssessPerferenceSettingdata.BKAuditOfficeTypeId;
                                branchProfile.BKRiskAssessPerferenceSetting.Amount = BKRiskAssessPerferenceSettingdata.Amount;
                                branchProfile.BKRiskAssessPerferenceSetting.RiskLocFlag = BKRiskAssessPerferenceSettingdata.RiskLocFlag;
                                branchProfile.BKRiskAssessPerferenceSetting.EntryDate = BKRiskAssessPerferenceSettingdata.EntryDate;
                                branchProfile.BKRiskAssessPerferenceSetting.Status = BKRiskAssessPerferenceSettingdata.Status;
                                branchProfile.BKRiskAssessPerferenceSetting.OperationStatus = status;
                                branchProfile.BKRiskAssessPerferenceSetting.Operation = "update";
                                branchProfile.BKRiskAssessPerferenceSetting.AuditYear = BKRiskAssessPerferenceSettingdata.AuditYear;
                                branchProfile.BKRiskAssessPerferenceSetting.AuditFiscalYear = BKRiskAssessPerferenceSettingdata.AuditFiscalYear;
                                branchProfile.BKRiskAssessPerferenceSetting.InfoReceiveDate = BKRiskAssessPerferenceSettingdata.InfoReceiveDate;
                                branchProfile.BKRiskAssessPerferenceSetting.InfoReceiveId = BKRiskAssessPerferenceSettingdata.InfoReceiveId;
                                branchProfile.BKRiskAssessPerferenceSetting.InfoReceiveFlag = BKRiskAssessPerferenceSettingdata.InfoReceiveFlag;
                            }
                        }
                        else
                        {
                            branchProfile.BKRiskAssessPerferenceSetting.Operation = "add";
                            branchProfile.BKRiskAssessPerferenceSetting.OperationStatus = status;

                        }
                    }

                    //End

                    //BKCommonSelectionSettings

                    branchProfile.BKCommonSelectionSetting.BranchID = id;
                    if (status == "CommonSelection")
                    {

                        if (commonId != 0)
                        {
                            //ResultModel<List<BKCommonSelectionSettings>> BKCommonSelectionSettings = _bkCommonSelectionSettingService.GetAll(new[] { "BranchID" }, new[] { id.ToString() });
                            ResultModel<List<BKCommonSelectionSettings>> BKCommonSelectionSettings = _bkCommonSelectionSettingService.GetAll(new[] { "Id" }, new[] { commonId.ToString() });
                            BKCommonSelectionSettings BKCommonSelectionSettingsdata = BKCommonSelectionSettings.Data.FirstOrDefault();

                            if (BKCommonSelectionSettingsdata != null)
                            {
                                branchProfile.BKCommonSelectionSetting.Id = BKCommonSelectionSettingsdata.Id;
                                branchProfile.BKCommonSelectionSetting.Code = BKCommonSelectionSettingsdata.Code;
                                branchProfile.BKCommonSelectionSetting.BKAuditOfficeTypeId = BKCommonSelectionSettingsdata.BKAuditOfficeTypeId;
                                branchProfile.BKCommonSelectionSetting.HitoricalPreformFlag = BKCommonSelectionSettingsdata.HitoricalPreformFlag;
                                branchProfile.BKCommonSelectionSetting.HistoricalPerformFlagDesc = BKCommonSelectionSettingsdata.HistoricalPerformFlagDesc;
                                branchProfile.BKCommonSelectionSetting.LastYearAuditFindingFlag = BKCommonSelectionSettingsdata.LastYearAuditFindingFlag;
                                branchProfile.BKCommonSelectionSetting.LastYearAuditFindingFlagDesc = BKCommonSelectionSettingsdata.LastYearAuditFindingFlagDesc;
                                branchProfile.BKCommonSelectionSetting.PreviousYearExceptLastYearAuditFindingFlag = BKCommonSelectionSettingsdata.PreviousYearExceptLastYearAuditFindingFlag;
                                branchProfile.BKCommonSelectionSetting.PreviousYearExceptLastYearAuditFindingFlagDesc = BKCommonSelectionSettingsdata.PreviousYearExceptLastYearAuditFindingFlagDesc;
                                branchProfile.BKCommonSelectionSetting.TechCyberRiskFlag = BKCommonSelectionSettingsdata.TechCyberRiskFlag;
                                branchProfile.BKCommonSelectionSetting.OfficeSizeFlag = BKCommonSelectionSettingsdata.OfficeSizeFlag;
                                branchProfile.BKCommonSelectionSetting.OfficeSignificanceFlag = BKCommonSelectionSettingsdata.OfficeSignificanceFlag;
                                branchProfile.BKCommonSelectionSetting.TechCyberRiskFlagDesc = BKCommonSelectionSettingsdata.TechCyberRiskFlagDesc;
                                branchProfile.BKCommonSelectionSetting.StaffTurnoverFlag = BKCommonSelectionSettingsdata.StaffTurnoverFlag;
                                branchProfile.BKCommonSelectionSetting.StaffTurnoverFlagDesc = BKCommonSelectionSettingsdata.StaffTurnoverFlagDesc;
                                branchProfile.BKCommonSelectionSetting.StaffTrainingGapsFlag = BKCommonSelectionSettingsdata.StaffTrainingGapsFlag;

                                branchProfile.BKCommonSelectionSetting.OperationStatus = status;
                                branchProfile.BKCommonSelectionSetting.Operation = "update";

                                branchProfile.BKCommonSelectionSetting.StaffTrainingGapsFlagDesc = BKCommonSelectionSettingsdata.StaffTrainingGapsFlagDesc;
                                branchProfile.BKCommonSelectionSetting.StrategicInitiativeFlagveFlag = BKCommonSelectionSettingsdata.StrategicInitiativeFlagveFlag;
                                branchProfile.BKCommonSelectionSetting.StrategicInitiativeFlagDesc = BKCommonSelectionSettingsdata.StrategicInitiativeFlagDesc;
                                branchProfile.BKCommonSelectionSetting.OperationalCompFlag = BKCommonSelectionSettingsdata.OperationalCompFlag;
                                branchProfile.BKCommonSelectionSetting.OperationalCompFlagDesc = BKCommonSelectionSettingsdata.OperationalCompFlagDesc;
                                branchProfile.BKCommonSelectionSetting.Status = BKCommonSelectionSettingsdata.Status;
                                branchProfile.BKCommonSelectionSetting.EntryDate = BKCommonSelectionSettingsdata.EntryDate;
                                branchProfile.BKCommonSelectionSetting.Fields1Flag = BKCommonSelectionSettingsdata.Fields1Flag;
                                branchProfile.BKCommonSelectionSetting.Fields1FlagDesc = BKCommonSelectionSettingsdata.Fields1FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields2Flag = BKCommonSelectionSettingsdata.Fields2Flag;
                                branchProfile.BKCommonSelectionSetting.Fields2FlagDesc = BKCommonSelectionSettingsdata.Fields2FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields3Flag = BKCommonSelectionSettingsdata.Fields3Flag;
                                branchProfile.BKCommonSelectionSetting.Fields3FlagDesc = BKCommonSelectionSettingsdata.Fields3FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields4Flag = BKCommonSelectionSettingsdata.Fields4Flag;
                                branchProfile.BKCommonSelectionSetting.Fields4FlagDesc = BKCommonSelectionSettingsdata.Fields4FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields5Flag = BKCommonSelectionSettingsdata.Fields5Flag;
                                branchProfile.BKCommonSelectionSetting.Fields5FlagDesc = BKCommonSelectionSettingsdata.Fields5FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields6Flag = BKCommonSelectionSettingsdata.Fields6Flag;
                                branchProfile.BKCommonSelectionSetting.Fields6FlagDesc = BKCommonSelectionSettingsdata.Fields6FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields7Flag = BKCommonSelectionSettingsdata.Fields7Flag;
                                branchProfile.BKCommonSelectionSetting.Fields7FlagDesc = BKCommonSelectionSettingsdata.Fields7FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields8Flag = BKCommonSelectionSettingsdata.Fields8Flag;
                                branchProfile.BKCommonSelectionSetting.Fields8FlagDesc = BKCommonSelectionSettingsdata.Fields8FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields9Flag = BKCommonSelectionSettingsdata.Fields9Flag;
                                branchProfile.BKCommonSelectionSetting.Fields9FlagDesc = BKCommonSelectionSettingsdata.Fields9FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields10Flag = BKCommonSelectionSettingsdata.Fields10Flag;
                                branchProfile.BKCommonSelectionSetting.Fields10FlagDesc = BKCommonSelectionSettingsdata.Fields10FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields11Flag = BKCommonSelectionSettingsdata.Fields11Flag;
                                branchProfile.BKCommonSelectionSetting.Fields11FlagDesc = BKCommonSelectionSettingsdata.Fields11FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields1Flag = BKCommonSelectionSettingsdata.Fields1Flag;
                                branchProfile.BKCommonSelectionSetting.Fields1FlagDesc = BKCommonSelectionSettingsdata.Fields1FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields12Flag = BKCommonSelectionSettingsdata.Fields12Flag;
                                branchProfile.BKCommonSelectionSetting.Fields12FlagDesc = BKCommonSelectionSettingsdata.Fields12FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields13Flag = BKCommonSelectionSettingsdata.Fields13Flag;
                                branchProfile.BKCommonSelectionSetting.Fields13FlagDesc = BKCommonSelectionSettingsdata.Fields13FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields1Flag = BKCommonSelectionSettingsdata.Fields1Flag;
                                branchProfile.BKCommonSelectionSetting.Fields1FlagDesc = BKCommonSelectionSettingsdata.Fields1FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields14Flag = BKCommonSelectionSettingsdata.Fields14Flag;
                                branchProfile.BKCommonSelectionSetting.Fields14FlagDesc = BKCommonSelectionSettingsdata.Fields14FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields15Flag = BKCommonSelectionSettingsdata.Fields15Flag;
                                branchProfile.BKCommonSelectionSetting.Fields15FlagDesc = BKCommonSelectionSettingsdata.Fields15FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields16Flag = BKCommonSelectionSettingsdata.Fields16Flag;
                                branchProfile.BKCommonSelectionSetting.Fields16FlagDesc = BKCommonSelectionSettingsdata.Fields16FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields17Flag = BKCommonSelectionSettingsdata.Fields17Flag;
                                branchProfile.BKCommonSelectionSetting.Fields17FlagDesc = BKCommonSelectionSettingsdata.Fields17FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields18Flag = BKCommonSelectionSettingsdata.Fields18Flag;
                                branchProfile.BKCommonSelectionSetting.Fields18FlagDesc = BKCommonSelectionSettingsdata.Fields18FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields19Flag = BKCommonSelectionSettingsdata.Fields19Flag;
                                branchProfile.BKCommonSelectionSetting.Fields19FlagDesc = BKCommonSelectionSettingsdata.Fields19FlagDesc;
                                branchProfile.BKCommonSelectionSetting.Fields20Flag = BKCommonSelectionSettingsdata.Fields20Flag;
                                branchProfile.BKCommonSelectionSetting.Fields20FlagDesc = BKCommonSelectionSettingsdata.Fields20FlagDesc;

                                branchProfile.BKCommonSelectionSetting.AuditYear = BKCommonSelectionSettingsdata.AuditYear;
                                branchProfile.BKCommonSelectionSetting.AuditFiscalYear = BKCommonSelectionSettingsdata.AuditFiscalYear;
                                branchProfile.BKCommonSelectionSetting.InfoReceiveDate = BKCommonSelectionSettingsdata.InfoReceiveDate;
                                branchProfile.BKCommonSelectionSetting.InfoReceiveId = BKCommonSelectionSettingsdata.InfoReceiveId;
                                branchProfile.BKCommonSelectionSetting.InfoReceiveFlag = BKCommonSelectionSettingsdata.InfoReceiveFlag;

                            }
                        }

                        else
                        {
                            branchProfile.BKCommonSelectionSetting.Operation = "add";
                            branchProfile.BKCommonSelectionSetting.OperationStatus = status;

                        }
                    }
                    //End


                }

                return View("CreateEdit", branchProfile);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }


        public ActionResult BranchProfileDelete(BranchProfile master)
        {
            ResultModel<BranchProfile> result = new ResultModel<BranchProfile>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                result = _branchProfileService.BranchProfileUpdate(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }
        public ActionResult BranchProfileActivate(BranchProfile master)
        {
            ResultModel<BranchProfile> result = new ResultModel<BranchProfile>();
            try
            {
                string userName = User.Identity.Name;
                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                master.Audit.LastUpdateBy = user.UserName;
                master.Audit.LastUpdateOn = DateTime.Now;
                master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                master.ActiveStatus = true;
                result = _branchProfileService.BranchProfileUpdate(master);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }
            return Ok(result);
        }



    }
}
