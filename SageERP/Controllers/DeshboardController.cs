using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Models.AuditModule;
using Shampan.Services.Deshboard;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;
using System.Collections.Generic;

namespace SSLAudit.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [Authorize]
    public class DeshboardController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly IAdvancesService _advancesService;
        private readonly IDeshboardService _deshboardService;

        public DeshboardController(ApplicationDbContext applicationDb, 
            ITeamsService teamsService, IAdvancesService advancesService,
            IDeshboardService deshboardService)
        {

            _applicationDb = applicationDb;
            _advancesService = advancesService;
            _deshboardService = deshboardService;

        }

        public IActionResult Index()
        {
            var data = _deshboardService.TotalAdditionalPaymentCount();
            var premaymentReview = _deshboardService.PrepaymentReview();
            int sum = data.Data;
            PrePaymentMaster vm = new PrePaymentMaster();
            vm.AdditionalPayment = sum;

            //string formatted = Convert.ToInt32(premaymentReview.Value).ToString("N0");
            //premaymentReview.Value = formatted;

            if (premaymentReview != null)
            {
                vm.PrepaymentReview = premaymentReview;
            }
            return View(vm);
        }
        public ActionResult Create()
        {

            ModelState.Clear();
            PrePaymentMaster vm = new PrePaymentMaster();
            vm.Operation = "add";
            return View("CreateEdit", vm);


        }
        public ActionResult CreateEdit(PrePaymentMaster master)
        {
            ResultModel<PrePaymentMaster> result = new ResultModel<PrePaymentMaster>();
            try
            {

                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _deshboardService.PrePaymentUpdate(master);

                    return Ok(result);
                }
                else
                {

                    foreach (var item in master.PrePaymentDetails)
                    {
                        string userName = User.Identity.Name;

                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                        item.Audit.CreatedBy = user.UserName;
                        item.Audit.CreatedOn = DateTime.Now;
                        item.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                        result = _deshboardService.PrePaymentInsert(item);
                    }

                    result.Data.Operation = "add";


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
                ResultModel<List<PrePaymentMaster>> result =
                    _deshboardService.PrePaymentGetAll(new[] { "Id" }, new[] { id.ToString() });

                PrePaymentMaster pre = result.Data.FirstOrDefault();
                pre.Operation = "update";


                pre.Id = id;

                return View("CreateEdit", pre);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }
        public ActionResult PrepaymentReviewed(PrepaymentReview master)
        {
            ResultModel<PrepaymentReview> result = new ResultModel<PrepaymentReview>();
            try
            {

                string userName = User.Identity.Name;

                ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

                master.Audit.CreatedBy = user.UserName;
                master.Audit.CreatedOn = DateTime.Now;
                master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                result = _deshboardService.PrepaymentReviewInsert(master);

                return Ok(result);

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

                ResultModel<List<PrePaymentMaster>> indexData =
                    _deshboardService.PrePaymentGetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _deshboardService.PrePaymentGetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _deshboardService.PrePaymentGetCount(TableName.Financial, "Id", new[] { "Financial.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }

        #region NonFinacila

        public IActionResult NonFinacilaIndex()
        {
            return View();
        }
        public ActionResult NonFinacilaCreate()
        {

            ModelState.Clear();
            PrePaymentMaster vm = new PrePaymentMaster();
            vm.Operation = "add";
            return View("NonFinacilaCreateEdit", vm);

        }
        public ActionResult NonFinacilaCreateEdit(PrePaymentMaster master)
        {
            ResultModel<PrePaymentMaster> result = new ResultModel<PrePaymentMaster>();
            try
            {

                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _deshboardService.NonFinancialUpdate(master);

                    return Ok(result);
                }
                else
                {

                    foreach (var item in master.NonFinancialDetails)
                    {
                        string userName = User.Identity.Name;

                        ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                        master.Audit.CreatedBy = user.UserName;
                        master.Audit.CreatedOn = DateTime.Now;
                        master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                        result = _deshboardService.NonFinancialInsert(item);
                    }

                    result.Data.Operation = "add";


                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);
        }
        public ActionResult NonFinacilaEdit(int id)
        {
            try
            {
                ResultModel<List<PrePaymentMaster>> result =
                    _deshboardService.NonFinancialGetAll(new[] { "Id" }, new[] { id.ToString() });

                PrePaymentMaster pre = result.Data.FirstOrDefault();
                pre.Operation = "update";


                pre.Id = id;

                return View("NonFinacilaCreateEdit", pre);
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }
        public IActionResult _indexNonFinacila()
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

                ResultModel<List<PrePaymentMaster>> indexData =
                    _deshboardService.NonFinancialGetIndexData(index, conditionalFields, conditionalValue);



                ResultModel<int> indexDataCount =
                _deshboardService.NonFinancialGetIndexDataCount(index, conditionalFields, conditionalValue);


                int result = _deshboardService.NonFinancialGetCount(TableName.NonFinancial, "Id", new[] { "NonFinancial.createdBy", }, new[] { userName });


                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<Teams>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }


        }


        #endregion

        #region DeshboardSetting
        public IActionResult DeshboardSettingIndex()
        {
            return View();
        }
        public IActionResult _deshboardsettingIndex()
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

                string userNameVal = Request.Form["userName"].ToString();
                string isAuditVal = Request.Form["isAudit"].ToString();
                string isTour1Val = Request.Form["isTour"].ToString();
                string isAdvanceVal = Request.Form["isAdvance"].ToString();
                string isTaVal = Request.Form["isTa"].ToString();
                string isTourCompletionReportVal = Request.Form["isTourCompletionReport"].ToString();


                string isAudit = CheckTrueFalse(isAuditVal);
                string isTour = CheckTrueFalse(isTour1Val);
                string isAdvance = CheckTrueFalse(isAdvanceVal);
                string isTa = CheckTrueFalse(isTaVal);
                string isTourCompletionReport = CheckTrueFalse(isTourCompletionReportVal);

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
                            "UserName like",
                            "IsAudit like",
                            "IsTour like",
                            "IsAdvance like",
                            "IsTa like",
                            "IsTourCompletionReport like"
                };


                string?[] conditionalValue = new[] { userNameVal, isAudit, isTour, isAdvance, isTa, isTourCompletionReport };

                ResultModel<List<DeshboardSettings>> indexData =
                    _deshboardService.DeshboardSettingGetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _deshboardService.DeshboardSettingGetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _deshboardService.DeshboardSettingGetCount(TableName.DeskboardSetting, "Id", new[] { "DeskboardSetting.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<UserRolls>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

        }

        public ActionResult DeshboardSettingCreate()
        {

            ModelState.Clear();
            DeshboardSettings vm = new DeshboardSettings();
            vm.Operation = "add";
            return View("DeshboardSettingCreateEdit", vm);

        }
        public ActionResult DeshboardSettingCreateEdit(DeshboardSettings master)
        {
            ResultModel<DeshboardSettings> result = new ResultModel<DeshboardSettings>();
            try
            {

                if (master.Operation == "update")
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.LastUpdateBy = user.UserName;
                    master.Audit.LastUpdateOn = DateTime.Now;
                    master.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _deshboardService.DeshboardSettingUpdate(master);

                    return Ok(result);
                }
                else
                {

                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    master.Audit.CreatedBy = user.UserName;
                    master.Audit.CreatedOn = DateTime.Now;
                    master.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _deshboardService.DeshboardSettingInsert(master);

                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                ex.LogAsync(ControllerContext.HttpContext);
            }

            return Ok(result);

        }

        public ActionResult DeshboardSettingEdit(int id)
        {
            try
            {

                ResultModel<List<DeshboardSettings>> result = _deshboardService.DeshboardSettingGetAll(new[] { "Id" }, new[] { id.ToString() });

                DeshboardSettings pre = result.Data.FirstOrDefault();
                pre.Operation = "update";
                pre.Id = id;

                return View("DeshboardSettingCreateEdit", pre);

            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return RedirectToAction("Index");
            }
        }

        #endregion

        [HttpGet]
        public ActionResult GetTotalBranchWithAuditCount()
        {

            List<AuditMaster> listData = _deshboardService.GetTotalBranchWithAuditCount(new AuditMaster()).Data;
            return Ok(listData);
        }

        [HttpGet]
        public ActionResult GetIssueCategoryData()
        {

            List<AuditIssue> listData = _deshboardService.GetIssueCategoryData(new AuditIssue()).Data;
            return Ok(listData);
        }

        public string CheckTrueFalse(string check)
        {
            if (check == "" || check == "Select")
            {
                return "";
            }
            else
            {
                if (check == "True")
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
        }

    }
}
