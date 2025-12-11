using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Policy;

namespace ShampanERP.Controllers
{
    [ServiceFilter(typeof(UserMenuActionFilter))]
    public class LoginController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly ICompanyInfoService _companyInfoService;
        private readonly IMemoryCache _memoryCache;
        private readonly ICompanyInfosService _companyInfosService;
        private DbConfig _dbConfig;
        private readonly ISettingsService _settingsService;
        private readonly IDeshboardService _deshboardService;

        public LoginController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           IConfiguration configuration,
           ApplicationDbContext context,
           IMemoryCache memoryCache,
           ICompanyInfosService companyInfosService, 
           ICompanyInfoService companyInfoService, 
           DbConfig dbConfig,
           ISettingsService settingsService,
            IDeshboardService deshboardService


            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _companyInfoService = companyInfoService;
            _memoryCache = memoryCache;
            _companyInfosService = companyInfosService;
            this._dbConfig = dbConfig;
            _settingsService = settingsService;
            _deshboardService = deshboardService;
        }

        public IActionResult Index(string returnurl)
        {
           
            List<CompanyInfo> companyInfos = new List<CompanyInfo>();
            LoginResource loginModel = new LoginResource();

            var resultModel = _companyInfoService.GetAll(null, null);

          

            if (resultModel.Status == Status.Success)
            {
                companyInfos = resultModel.Data;
            }

            loginModel.CompanyInfos = companyInfos;
            loginModel.returnUrl = returnurl;
            AuthDbConfig.AuthDB = AuthDbName();
     

            return View(loginModel);
        }

        public async Task<IActionResult> CreateClaims()
        {
            string SageDbName = _dbConfig.SageDbName;

            string userName = "erp";

            var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);

            IdentityResult? result = await _userManager.AddClaimAsync(user, new Claim(ClaimNames.SageDatabase, SageDbName));

            return RedirectToAction("Index", "Home");

        }


        public string AuthDbName()
        {
            string connectionString = _configuration.GetConnectionString("AuthContext");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;
            return databaseName;
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginResource model, string returnurl)
        {

            var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            var url = location.AbsoluteUri;
            List<CompanyInfo> companyInfos = new List<CompanyInfo>();
            var resultModel = _companyInfoService.GetAll(null, null);
            string[] retResults = new string[3];

      

            CompanyInfo companyData = _companyInfoService.GetAll(null, null).Data.FirstOrDefault();
            if (!string.IsNullOrEmpty(companyData.IsAdCheck) && companyData.IsAdCheck == "Y")
            {
                //retResults = _companyInfoService.CheckADAuth(model.UserName, null, null, null, companyData);
            }

            if (resultModel.Status == Status.Success)
            {
                companyInfos = resultModel.Data;
            }

            model.CompanyInfos = companyInfos;


            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Message", "Wrong username or password");
                return View(model);
            }

            var user = _userManager.Users.SingleOrDefault(x => x.UserName == model.UserName);


            if (user.IsArchive == true)
            {
                ModelState.AddModelError("Message", "This user is not active, Please active the user first.");
                return View(model);
            }

            if (user == null)
            {
                ModelState.AddModelError("Message", "Wrong username or password");
                return View(model);
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var dbClaim = userClaims.FirstOrDefault(x => x.Type == ClaimNames.Database);

            if (dbClaim == null)
            {
                ModelState.AddModelError("Message", "Wrong username or password");
                return View(model);
            }

            if (dbClaim.Value != model.CompanyName)
            {
                ModelState.AddModelError("Message", "Wrong username or password");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.UserData, user.Id),
            };


            if (roles.Any())
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            claims.AddRange(userClaims);


            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IssuedUtc = DateTimeOffset.Now,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


            ViewData["Username"] = model.UserName;

            bool b = User.Identity.IsAuthenticated;
            string name = User.Identity.Name;

            int currentYear = DateTime.Now.Year;
            YearChange.Year = currentYear.ToString();
            YearChange.BeginingYear = YearChange.Year + "-01-01";
            YearChange.EndYear = YearChange.Year + "-12-30";

            if (returnurl == null)
            {
                return RedirectToAction("Index", "Home");
            }

            else
            {
                return Redirect(returnurl);
            }
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            try
            {

                ResultModel<string> companyCode = _settingsService.GetSettingsValue(new[] { "SettingGroup", "SettingName" }, new[] { "Company", "Code" });
                CommonChanges.CompanyCode = companyCode.Data.ToString();

                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
                _signInManager.SignOutAsync();
                HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}











