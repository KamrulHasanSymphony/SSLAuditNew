using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Models;
using Shampan.Models.AuditModule;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using SSLAudit.Models;
using StackExchange.Exceptional;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;


namespace ShampanERP.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    [ServiceFilter(typeof(DeshboardActionFilter))]
    public class HomeController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly ApplicationDbContext _context;
		private readonly ICompanyInfoService _companyInfoService;
		private readonly IUserRollsService _userRollsService;
		private readonly IAuditMasterService _auditMasterService;
		private readonly IMemoryCache _memoryCache;
        private readonly ISettingsService _settingsService;
        private readonly IDeshboardService _deshboardService;
        private readonly ApplicationDbContext _applicationDb;

        public HomeController(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<IdentityRole> roleManager,
			IConfiguration configuration,
			ApplicationDbContext context,
			ICompanyInfoService companyInfoService,
			IUserRollsService userRollsService,
			IAuditMasterService auditMasterService,
			IMemoryCache memoryCache,
            ISettingsService settingsService,
            IDeshboardService deshboardService,
            ApplicationDbContext applicationDb
        )
		{

			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_context = context;
			_companyInfoService = companyInfoService;
			_userRollsService = userRollsService;
			_auditMasterService = auditMasterService;
			_memoryCache = memoryCache;
            _settingsService = settingsService;
            _deshboardService = deshboardService;
            _applicationDb = applicationDb;

        }

        [Authorize]
		public IActionResult Index(string IsShow,string year)
		{
			DeshboardSettings ds = new DeshboardSettings();
            ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });
            List<BranchProfile> branchProfiles = new List<BranchProfile>();
			Claim? currentBranchClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimNames.CurrentBranch);
            CommonChanges.CompanyCode = companyCode.Data.ToString();
            ViewBag.CompanyCode = companyCode.Data.ToUpper();


            #region DashboardSetting

            var currentUrl = HttpContext.Request.GetDisplayUrl();
            string[] parts = new string[] { "", "" };
            parts = currentUrl.TrimStart('/').Split('/');
            string urlhttp = parts[0];
            string HostUrl = parts[2];
            var baseUrl = $"{urlhttp}://{HostUrl}";
            ViewBag.BaseUrl = baseUrl;


            string userName = User.Identity.Name;
            ApplicationUser? user = _applicationDb.Users.FirstOrDefault(model => model.UserName == userName);

            ResultModel<List<DeshboardSettings>> DashboardSetting = _deshboardService.DeshboardSettingGetAll(new[] { "UserId" }, new[] { user.Id.ToString() });

			if(DashboardSetting.Data == null || DashboardSetting.Data.Count() == 0)
			{
                ds = new DeshboardSettings();
            }
            else
            {
				ds = DashboardSetting.Data.FirstOrDefault();
            }
            ViewData["DashboardSetting"] = ds;

            #endregion


            if (!string.IsNullOrEmpty(year))
			{
                YearChange.Year = year;
                YearChange.BeginingYear = YearChange.Year + "-01-01";
                YearChange.EndYear = YearChange.Year + "-12-30";
				branchProfiles.Add(new BranchProfile() { Year = year });
			}
			
            if (IsShow == "showpopup")
			{
				if (currentBranchClaim is null)
				{
					var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
					if (userIdClaim is null) return RedirectToAction("Index", "Login");
					var resultModel = _companyInfoService.GetBranches(new[] { "userid" }, new[] { userIdClaim.Value });
					if (resultModel.Status != Status.Success) return RedirectToAction("Index", "Login");
					branchProfiles = resultModel.Data;
				}
				return View(branchProfiles);
			}
			else
			{
                return View(branchProfiles);
            }
        }

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AssignBranch(BranchProfile branch)
		{
			try
			{
				if (branch.BranchID == 0) RedirectToAction("Index");

				var identity = new ClaimsIdentity(User.Identity);
				identity.AddClaim(new Claim(ClaimNames.CurrentBranch, branch.BranchID.ToString()));
				identity.AddClaim(new Claim(ClaimNames.CurrentBranchName, branch.BranchName.ToString().Trim()));
				var principal = new ClaimsPrincipal(identity);

				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					principal,
					new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

				var cacheKey = $"{User.Identity.Name}_BranchClaim";
				var branchClaim = new BranchClaim
				{
					BranchId = branch.BranchID,
					BranchName = branch.BranchName.Trim()
				};

                string userName = User.Identity.Name;
                ResultModel<List<UserBranch?>> rolls = _auditMasterService.GetUserIdbyUserName(userName);
                UserBranch ur = rolls.Data.FirstOrDefault();
				ur.BranchName = branch.BranchName.Trim();

                ResultModel<UserBranch>  model = _auditMasterService.UpdateBranchName(ur);

                _memoryCache.Set(cacheKey, branchClaim, TimeSpan.FromHours(1));

			}
			catch (Exception e)
			{
				await e.LogAsync(ControllerContext.HttpContext);
			}
			return RedirectToAction("Index");
		}

		[Authorize]
		public async Task<IActionResult> ChangeBranch()
		{
			try
			{
				var identity = User.Identity as ClaimsIdentity;
				Claim currentBranchClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimNames.CurrentBranch);
				Claim currentBranchNameClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimNames.CurrentBranchName);

				if (currentBranchClaim is null) RedirectToAction("Index");

				identity?.RemoveClaim(currentBranchClaim);
				identity?.RemoveClaim(currentBranchNameClaim);

				var principal = new ClaimsPrincipal(identity);

				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					principal,
					new AuthenticationProperties() { IssuedUtc = DateTimeOffset.Now, });

			}
			catch (Exception e)
			{
				await e.LogAsync(ControllerContext.HttpContext);
			}
			return RedirectToAction("Index",new { IsShow = "showpopup" });
		}


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<ActionResult> LoginWeb()
		{
			List<CompanyInfo> companyInfos = new List<CompanyInfo>();
			LoginModel loginModel = new LoginModel();

			var resultModel = _companyInfoService.GetAll(null, null);

			if (resultModel.Success)
			{
				companyInfos = resultModel.Data;
			}

			loginModel.CompanyInfos = companyInfos;

			return View("Login", loginModel);
		}

		[HttpPost]
		public async Task<ActionResult> LoginWeb(LoginResource model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("UserName", "Invalid username or password.");
				return View("index", model);
			}

			var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

			if (!result.Succeeded) return BadRequest("Wrong username or password");

			var user = _userManager.Users.SingleOrDefault(x => x.UserName == model.UserName);

			if (user == null) return BadRequest("Wrong username or password");

			IList<string> roles = await _userManager.GetRolesAsync(user);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
			};

			if (roles.Any())
				claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var claimsIdentity = new ClaimsIdentity(
				claims, CookieAuthenticationDefaults.AuthenticationScheme);

			var authProperties = new AuthenticationProperties
			{
				
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
				IssuedUtc = DateTimeOffset.Now,			
				RedirectUri = "/"
				
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);

			ViewData["Username"] = model.UserName;

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<ActionResult> RegistrationWeb()
		{
			return View("RegistrationWeb");
		}

		[HttpPost]
		public async Task<ActionResult> RegistrationWeb(LoginResource model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("RegistrationWeb", "Home");
			}

			if (ModelState.IsValid)
			{
				var _user = new ApplicationUser { UserName = model.UserName };
				var _result = await _userManager.CreateAsync(_user, model.Password);

				if (_result.Succeeded)
				{
					await _signInManager.SignInAsync(_user, false);

				}
				else
				{
					foreach (var error in _result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
					return RedirectToAction("RegistrationWeb", "Home");
				}

				var authProperties = new AuthenticationProperties
				{					
				};

			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<ActionResult> LogOutWeb()
		{
			await HttpContext.SignOutAsync(
				CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Index", "Login");

		}

		[HttpGet]
		public ActionResult LeftSideBar()
		{		
			try
			{			
			}
			catch (Exception ex)
			{
			}
			return PartialView("_leftSideBar", "");
		}

		public List<UserManuInfo> GetSideBar()
		{
			string userName = User.Identity.Name;

			List<UserManuInfo> manu = _userRollsService.GetUserManu(userName);

			return manu;
		}
	
		public async Task Exceptions()
		{
			await ExceptionalMiddleware.HandleRequestAsync(HttpContext).ConfigureAwait(false);
		}

		public ActionResult GetEvents()
		{
			List<object> birthdayList = new List<object>
			{
				new
				{
					title = "Birthday",
					description = "Birthday",
					start = DateTime.Parse("2023-07-01"),
					end = DateTime.Parse("2023-07-01"),
					color = "blue",
					allDay = true
				},
				new
				{
					title = "Another Birthday",
					description = "Another Birthday",
					start = DateTime.Parse("2023-08-15"),
					end = DateTime.Parse("2023-08-15"),
					color = "red",
					allDay = true
				},
                
            };

			return Ok(birthdayList);
		}


		public ActionResult GetEmployees()
		{
			List<object> chartData = new List<object>();

			string query = "SELECT EmployeeId, Name, Designation, isnull(ReportingManager,'')ReportingManager ";
			query += " FROM EmployeesHierarchy";

			string constr = "Data Source=192.168.15.100,1419\\MSSQLSERVER2019;Initial Catalog=AjaxSamples;User id=sa;Password=S123456_;";
			using (SqlConnection con = new SqlConnection(constr))
			{
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					con.Open();

					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);

						foreach (DataRow row in dt.Rows)
						{
							chartData.Add(new object[]
							{
								row["EmployeeId"], row["Name"], row["Designation"], row["ReportingManager"]
							});
						}
					}

					con.Close();
				}
			}

			return Ok(chartData);

		}

        [HttpPost]
        public IActionResult UpdateAuditComponents(string selectedValue)
        {            
            var updatedAuditComponents = _userRollsService.GetAuditComponents();
            
            HttpContext.Items["AuditComponentList"] = null;
		
			return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult ChangeYear(string year)
        {

			YearChange.Year = year;
			YearChange.BeginingYear = YearChange.Year + "-01-01";
			YearChange.EndYear = YearChange.Year + "-12-30";
            return RedirectToAction("Index");

        }

    }
}