using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SageERP.ExtensionMethods;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.FiscalYear;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using ShampanERP.Models;
using ShampanERP.Persistence;
using StackExchange.Exceptional;

namespace SSLAudit.Controllers
{
	[ServiceFilter(typeof(UserMenuActionFilter))]
	[Authorize]

    public class FiscalYearController : Controller
    {
        //private readonly ApplicationDbContext _applicationDb;
        public string YearEnd { get; set; }
        private readonly ApplicationDbContext _applicationDb;
        private readonly IFiscalYearService _fiscalYearService;

        public FiscalYearController(ApplicationDbContext applicationDb, IFiscalYearService fiscalYearService)
        {
            _applicationDb = applicationDb;
            _fiscalYearService = fiscalYearService;
        }


        // GET: Audit/FiscalYears
        public ActionResult Index()
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

                ResultModel<List<FiscalYearVM>> indexData =
                    _fiscalYearService.GetIndexData(index, conditionalFields, conditionalValue);

                ResultModel<int> indexDataCount =
                _fiscalYearService.GetIndexDataCount(index, conditionalFields, conditionalValue);

                int result = _fiscalYearService.GetCount(TableName.FiscalYears, "Id", new[] { "FiscalYears.createdBy", }, new[] { userName });

                return Ok(new { data = indexData.Data, draw, recordsTotal = result, recordsFiltered = indexDataCount.Data });
            }
            catch (Exception e)
            {
                e.LogAsync(ControllerContext.HttpContext);
                return Ok(new { Data = new List<FiscalYearVM>(), draw = "", recordsTotal = 0, recordsFiltered = 0 });
            }

        }

        public ActionResult Create()
        {
            //FiscalYearsRepo _fiscalRepo = new FiscalYearsRepo();
            //CompanyProfileRepo _repo = new CompanyProfileRepo();
            CommonVM param = new CommonVM();
            FiscalYearVM vm = new FiscalYearVM();
            //CompanyProfileVM companyVm = new CompanyProfileVM();
            List<FiscalYearVM> fiscalYearLists = new List<FiscalYearVM>();
            string yearStartDate = "";
            int year;

            //var companyId = Session["CompanyId"];
            //param.Id = companyId.ToString()
            //ResultVM companyData = _repo.List(param);
            //if (companyData.Status == "Success" && companyData.DataVM != null)
            //{
            //    companyVm = JsonConvert.DeserializeObject<List<CompanyProfileVM>>(companyData.DataVM.ToString()).FirstOrDefault();
            //    yearStartDate = companyVm.FYearStart;
            //    year = DateTime.ParseExact(yearStartDate, "yyyy-MM-dd", null).Year;
            //    vm.YearStart = yearStartDate;
            //    vm.Year = year;
            //}
            //else
            //{
            //    vm = null;
            //}
            vm.YearStart = "2025/07/01";

            List<FiscalYearDetailVM> detailVMs = new List<FiscalYearDetailVM>();
            FiscalYearDetailVM dvm;
            
            vm.fiscalYearDetails = detailVMs;
            vm = DesignFiscalYear(vm);
            vm.Operation = "add";
            return View("Create", vm);

        }
        private FiscalYearVM DesignFiscalYear(FiscalYearVM model)
        {
            try
            {
                // Attempt to parse the input date string using the correct format
                DateTime start_date;
                bool parsed = DateTime.TryParseExact(model.YearStart, new[] { "yyyy/MM/dd", "yyyy-MM-dd" }, null, System.Globalization.DateTimeStyles.None, out start_date);

                if (!parsed)
                {
                    //throw new FormatException($"Invalid date format for YearStart: {model.YearStart}");
                }

                model.YearEnd = start_date.AddYears(1).AddDays(-1).ToString("yyyy/MM/dd");
                model.Year = start_date.AddYears(1).AddDays(-1).Year;

                var fvms = Enumerable.Range(0, 12)
                                     .Select(i => new FiscalYearDetailVM
                                     {
                                         MonthName = start_date.AddMonths(i).ToString("MMM-yy"),
                                         MonthStart = start_date.AddMonths(i).ToString("yyyy/MM/dd"),
                                         MonthEnd = start_date.AddMonths(i + 1).AddDays(-1).ToString("yyyy/MM/dd"),
                                         MonthId = Convert.ToInt32(start_date.AddMonths(i).ToString("yyyyMM"))
                                     })
                                     .ToList();

                model.fiscalYearDetails = fvms;
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult FiscalYearSet(FiscalYearVM model)
        {
            try
            {

                DateTime start_date = DateTime.ParseExact(model.YearStart, "yyyy-MM-dd", null);
                model.YearEnd = start_date.AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                model.Year = start_date.AddYears(1).AddDays(-1).Year;

                var fvms = Enumerable.Range(0, 12)
                                     .Select(i => new FiscalYearDetailVM
                                     {
                                         MonthName = start_date.AddMonths(i).ToString("MMM-yy"),
                                         MonthStart = start_date.AddMonths(i).ToString("dd-MMM-yyyy"),
                                         MonthEnd = start_date.AddMonths(i + 1).AddDays(-1).ToString("dd-MMM-yyyy"),
                                         MonthId = Convert.ToInt32(start_date.AddMonths(i).ToString("yyyyMM"))
                                     })
                                     .ToList();

                model.fiscalYearDetails = fvms;
                model.Operation = "add";
                return PartialView("_period", model.fiscalYearDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult CreateEdit(FiscalYearVM model)
        {
            ResultModel<FiscalYearVM> result = new ResultModel<FiscalYearVM>();
            ResultVM resultVM = new ResultVM { Status = "Fail", Message = "Error", ExMessage = null, Id = "0", DataVM = null };
            //_repo = new FiscalYearsRepo();


            try
            {
                if (model.Operation.ToLower() == "add")
                {

                    string userName = User.Identity.Name;

                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    model.Audit.CreatedBy = user.UserName;
                    model.Audit.CreatedOn = DateTime.Now;
                    model.Audit.CreatedFrom = HttpContext.Connection.RemoteIpAddress.ToString();
                    result = _fiscalYearService.Insert(model);

                    return Ok(result);

                    

                }
                else if (model.Operation.ToLower() == "update")
                {
                    string userName = User.Identity.Name;
                    ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);
                    model.Audit.LastUpdateBy = user.UserName;
                    model.Audit.LastUpdateOn = DateTime.Now;
                    model.Audit.LastUpdateFrom = HttpContext.Connection.RemoteIpAddress.ToString();

                    result = _fiscalYearService.Update(model);

                    return Ok(result);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {

                return View("Create", model);
            }

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                ResultModel<List<FiscalYearDetailVM>> FiscalYearDetail = new ResultModel<List<FiscalYearDetailVM>>();
                CommonVM param = new CommonVM();
                param.Id = id.ToString();
               

                ResultModel<List<FiscalYearVM>> result = _fiscalYearService.GetAll(new[] { "Id" }, new[] { id.ToString() });
                FiscalYearVM vm = result.Data.FirstOrDefault();
               

                if(vm != null)
                {
                     FiscalYearDetail = _fiscalYearService.GetAllDetail(new[] { "FiscalYearId" }, new[] { id.ToString() });
                }
                vm.fiscalYearDetails = FiscalYearDetail.Data;
                vm.Operation = "update";
                vm.YearPeriod = vm.Year;


                return View("CreateEdit", vm);
            }
            catch (Exception e)
            {

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(FiscalYearVM vm)
        {
            ResultModel<FiscalYearVM> result = new ResultModel<FiscalYearVM>();

            try
            {
               // _repo = new FiscalYearsRepo();
                CommonVM param = new CommonVM();
                param.IDs = vm.IDs;
                //param.ModifyBy = Session["UserId"].ToString();
                //param.ModifyFrom = Ordinary.GetLocalIpAddress();
                //ResultVM resultData = _repo.Delete(param);

                //Session["result"] = resultData.Status + "~" + resultData.Message;

                result = new ResultModel<FiscalYearVM>()
                {
                    Success = true,
                    Status = Status.Success,
                    //Message = resultData.Message,
                    Data = null
                };

                return Json(result);
            }
            catch (Exception e)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                return RedirectToAction("Index");
            }
        }

        


    }
}
