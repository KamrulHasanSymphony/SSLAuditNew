using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using Serilog;
using Serilog.Events;
using Shampan.Core.Interfaces.Repository.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Audit;
using Shampan.Core.Interfaces.Services.AuditFeedbackService;
using Shampan.Core.Interfaces.Services.AuditIssues;
using Shampan.Core.Interfaces.Services.AuditPoints;
using Shampan.Core.Interfaces.Services.BKAuditCompliances;
using Shampan.Core.Interfaces.Services.BKAuditInfoDetailss;
using Shampan.Core.Interfaces.Services.BKAuditInfoMasterApprovals;
using Shampan.Core.Interfaces.Services.BKAuditInfoMasters;
using Shampan.Core.Interfaces.Services.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.BKAuditOfficesPreferenceInfos;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKAuditPreferenceEvaluation;
using Shampan.Core.Interfaces.Services.BKAuditTemlateMasters;
using Shampan.Core.Interfaces.Services.BKCheckListSubTypes;
using Shampan.Core.Interfaces.Services.BKCommonSelectionSetting;
using Shampan.Core.Interfaces.Services.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKFinancePreformPreferencesCBS;
using Shampan.Core.Interfaces.Services.BKFraudIrregularitiesInternalControlPreferencesCBS;
using Shampan.Core.Interfaces.Services.BKFraudIrrgularitiesPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKInternalControlWeakPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKReguCompliancesPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.BKRiskAssessRegulationPreferencesCBS;
using Shampan.Core.Interfaces.Services.Branch;
using Shampan.Core.Interfaces.Services.Calender;
using Shampan.Core.Interfaces.Services.Categorys;
using Shampan.Core.Interfaces.Services.CheckListItems;
using Shampan.Core.Interfaces.Services.Circular;
using Shampan.Core.Interfaces.Services.CISReport;
using Shampan.Core.Interfaces.Services.Company;
using Shampan.Core.Interfaces.Services.Deshboard;
using Shampan.Core.Interfaces.Services.FiscalYear;
using Shampan.Core.Interfaces.Services.HighLevelReports;
using Shampan.Core.Interfaces.Services.Mail;
using Shampan.Core.Interfaces.Services.ModulePermissions;
using Shampan.Core.Interfaces.Services.Modules;
using Shampan.Core.Interfaces.Services.Node;
using Shampan.Core.Interfaces.Services.Notification;
using Shampan.Core.Interfaces.Services.Oragnogram;
using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.TeamMember;
using Shampan.Core.Interfaces.Services.Tour;
using Shampan.Core.Interfaces.Services.TransportAllownace;
using Shampan.Core.Interfaces.Services.TransportAllownaceDetails;
using Shampan.Core.Interfaces.Services.User;
using Shampan.Core.Interfaces.Services.UserRoll;
using Shampan.Core.Interfaces.Services.UsersPermission;
using Shampan.Models;
using Shampan.Services;
using Shampan.Services.Advance;
using Shampan.Services.Audit;
using Shampan.Services.AuditFeedbackService;
using Shampan.Services.AuditIssues;
using Shampan.Services.AuditPoints;
using Shampan.Services.BKAuditCompliances;
using Shampan.Services.BKAuditInfoDetailss;
using Shampan.Services.BKAuditInfoMasterApprovals;
using Shampan.Services.BKAuditInfoMasters;
using Shampan.Services.BKAuditOfficePreferencesCBS;
using Shampan.Services.BKAuditOfficesPreferenceInfos;
using Shampan.Services.BKAuditOfficeTypes;
using Shampan.Services.BKAuditPreferenceEvaluation;
using Shampan.Services.BKAuditTemlateMasters;
using Shampan.Services.BKCheckListSubTypes;
using Shampan.Services.BKCheckListTypes;
using Shampan.Services.BKCommonSelectionSetting;
using Shampan.Services.BKFinancePerformPreferenceSetting;
using Shampan.Services.BKFinancePreformPreferencesCBS;
using Shampan.Services.BKFraudIrrgularitiesPreferenceSetting;
using Shampan.Services.BKInternalControlWeakPreferenceSetting;
using Shampan.Services.BKReguCompliancesPreferenceSetting;
using Shampan.Services.BKRiskAssessPerferenceSettings;
using Shampan.Services.BKRiskAssessRegulationPreferencesCBS;
using Shampan.Services.Branch;
using Shampan.Services.Calender;
using Shampan.Services.Categorys;
using Shampan.Services.CheckListItems;
using Shampan.Services.Circular;
using Shampan.Services.CISReport;
using Shampan.Services.Company;
using Shampan.Services.CompanyInfo;
using Shampan.Services.Configuration;
using Shampan.Services.Deshboard;
using Shampan.Services.FiscalYear;
using Shampan.Services.HighLevelReport;
using Shampan.Services.Mail;
using Shampan.Services.ModulePermissions;
using Shampan.Services.Node;
using Shampan.Services.Notification;
using Shampan.Services.Oragnogram;
using Shampan.Services.Settings;
using Shampan.Services.Team;
using Shampan.Services.TeamMember;
using Shampan.Services.Tour;
using Shampan.Services.TransportAllownace;
using Shampan.Services.TransportAllownaceDetails;
using Shampan.Services.User;
using Shampan.Services.UserRoll;
using Shampan.Services.UsersPermission;
using Shampan.UnitOfWork.SqlServer;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Controllers;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UnitOfWork.Interfaces;

namespace ShampanERP.Configuration.ServiceRegistration
{
    public static class CustomServiceCollection
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services)
        {
            //For Auto Call

            //services.AddHostedService<TimerService>();
            //services.AddHostedService<RepeatingFunctionExecutionService>();
            //services.AddHostedService<DailyFunctionExecutionService>();
            //services.AddHostedService<ScheduledFunctionExecutionService>();

            //End

            //services.AddSingleton<HttpRequestHelper>();
            //HttpRequestHelper.Initialize(Configuration);


            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; 
            });


         
            //ForEveryActionCallSideManu+UserManu
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddMemoryCache();
			services.AddScoped<UserMenuActionFilter>();
			services.AddScoped<DeshboardActionFilter>();
			
            

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			//end of new change
			//// Configure Serilog for error logging
			//Log.Logger = new LoggerConfiguration()
			//	.WriteTo.File("errorlog.txt", rollingInterval: RollingInterval.Day)
			//	.CreateLogger();
			//services.AddLogging(loggingBuilder =>
			//{
			//	loggingBuilder.AddSerilog();
			//});

			Log.Logger = new LoggerConfiguration()
		   .MinimumLevel.Information()
		   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) 
		   .Enrich.FromLogContext()
		   .WriteTo.File(@"F:\errorlog.txt", rollingInterval: RollingInterval.Day)	   
		   .CreateLogger();


			services.AddScoped<IUnitOfWork, UnitOfWorkSqlServer>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICompanyInfosService, CompanyInfosService>();
            services.AddTransient<IUserRollsService, UserRollsService>();
			services.AddTransient<IUserBranchService, UserBranchService>();
            services.AddTransient<IBranchProfileService, BranchProfilesService>();
            services.AddTransient<ICompanyInfoService, CompanyInfoService>();
            services.AddScoped<IAuditMasterService, AuditMasterService>();          
            services.AddScoped<IAuditAreasService, AuditAreasService>();
            services.AddScoped<IAuditUserService, AuditUserService>();
            services.AddTransient<IFileService, FileService>();
            services.AddScoped<IAuditIssueService, AuditIssuesService>();
            services.AddScoped<IAuditIssueAttachmentsService, AuditIssueAttachmentsService>();
            services.AddScoped<ICircularAttachmentsService, CircularAttachmentsService>();
            services.AddTransient<IAuditFeedbackService, AuditFeedbackService>();
            services.AddTransient<IAuditFeedbackAttachmentsService, AuditFeedbackAttachmentsService>();
            services.AddTransient<IAuditBranchFeedbackAttachmentsService, AuditBranchFeedbackAttachmentsService>();
            services.AddTransient<IAuditIssueService, AuditIssuesService>();
            services.AddTransient<IAuditIssueAttachmentsService, AuditIssueAttachmentsService>();
            services.AddTransient<ITeamsService, TeamsService>();
            services.AddTransient<ITeamMembersService, TeamMembeService>();
            services.AddTransient<IAdvancesService, AdvancesService>();
            services.AddTransient<IToursService, ToursService>();
            services.AddTransient<ITransportAllownacesService, TransportAllownacesService>();
            services.AddTransient<IUserRollsService, UserRollsService>();
            services.AddTransient<ICircularsService, CircularsService>();
            services.AddTransient<ICalendersService, CalenderService>();
            services.AddTransient<IOragnogramService, OragnogramService>();
            services.AddTransient<IEmployeesHiAttachmentsService, EmployeesHierarchyAttachmentsService>();
            services.AddTransient<IAuditIssueUserService, AuditIssueUserService>();     
            services.AddTransient<IAuditBranchFeedbackService, AuditBranchFeedbackService>();
            services.AddTransient<IModuleService, ModuleService>();
            services.AddTransient<IModulePermissionService, ModulePermissionService>();
            services.AddTransient<IDeshboardService, DeshboardService>();
            services.AddTransient<IUsersPermissionService, UsersPermissionService>();
            services.AddTransient<ICISReportService, CISReportService>();
            services.AddTransient<IDateWisePolicyEditLogService, DateWisePolicyEditLogService>();
            services.AddTransient<IDateWisePolicyEditLogWithDetailsService, DateWisePolicyEditLogWithDetailsService>();
            services.AddTransient<ICollectionEditLogService, CollectionEditLogService>();
            services.AddTransient<IDocumentWiseEditLogService, DocumentWiseEditLogService>();
            services.AddTransient<INodeService, NodeService>();
            services.AddTransient<ITransportAllownaceDetailService, TransportAllownaceDetailService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IMailService, MailsService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICheckListItemService, CheckListItemService>();
            services.AddTransient<IBKAuditComplianceService, BKAuditComplianceService>();
            services.AddTransient<IBKAuditOfficeTypeService, BKAuditOfficeTypeService>();
            services.AddTransient<IBKCheckListTypeService, BKCheckListTypeService>();
            services.AddTransient<IBKCheckListSubTypeService, BKCheckListSubTypeService>();
            services.AddTransient<IBKAuditTemlateMasterService, BKAuditTemlateMasterService>();
            services.AddTransient<IBKRiskAssessPerferenceSettingService, BKRiskAssessPerferenceSettingService>();
            services.AddTransient<IBKFinancePerformPreferenceSettingService, BKFinancePerformPreferenceSettingService>();
            services.AddTransient<IBKFraudIrrgularitiesPreferenceSettingService, BKFraudIrrgularitiesPreferenceSettingsService>();
            services.AddTransient<IBKInternalControlWeakPreferenceSettingService, BKInternalControlWeakPreferenceSettingService>();
            services.AddTransient<IBKReguCompliancesPreferenceSettingsService, BKReguCompliancesPreferenceSettingService>();
            services.AddTransient<IBKAuditOfficesPreferenceInfoService, BKAuditOfficesPreferenceInfoService>();
            services.AddTransient<IBKAuditPreferenceEvaluationService, BKAuditPreferenceEvaluationsService>();
            services.AddTransient<IBKCommonSelectionSettingService, BKCommonSelectionSettingService>();
            services.AddTransient<IBKAuditInfoMasterService, BKAuditInfoMasterService>();
            services.AddTransient<IBKAuditInfoDetailsService, BKAuditInfoDetailsService>();
            services.AddTransient<IBKAuditInfoMasterApprovalService, BKAuditInfoMasterApprovalService>();

            services.AddTransient<IAuditPointService, AuditPointService>();

            services.AddTransient<IBKAuditOfficePreferenceCBSService, BKAuditOfficePreferenceCBSService>();
            services.AddTransient<IBKFinancePreformPreferenceCBSService, BKFinancePreformPreferenceCBSService>();
            services.AddTransient<IBKFraudIrregularitiesInternalControlPreferenceCBSService, BKFraudIrregularitiesInternalControlPreferenceCBSService>();
            services.AddTransient<IBKRiskAssessRegulationPreferenceCBSService, BKRiskAssessRegulationPreferenceCBSService>();
            services.AddTransient<IFiscalYearService, FiscalYearService>();
            services.AddTransient<IHighLevelReportService, HighLevelReportService>();

            services.AddScoped<DbConfig, DbConfig>();

            return services;

        }

        public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }


        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.Password.RequireDigit = false;
                    config.Password.RequireLowercase = false;
                    config.Password.RequireUppercase = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    }
                )
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
					options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
					//options.ExpireTimeSpan = TimeSpan.FromSeconds(10);
					
					options.AccessDeniedPath = PathString.FromUriComponent("/login");
                    options.LoginPath = PathString.FromUriComponent("/login");
                    options.LogoutPath = PathString.FromUriComponent("/home/logoutweb");



                });
                //.AddJwtBearer(options =>
                //{
                //    options.RequireHttpsMetadata = false;
                //    options.SaveToken = true;
                //    options.TokenValidationParameters = new TokenValidationParameters
                //    {
                //        ValidateIssuerSigningKey = true,
                //        ValidateIssuer = true,
                //        ValidateLifetime = true,
                //        ValidateAudience = true,
                //        ValidIssuer = configuration["JwtIssuer"],
                //        ValidAudience = configuration["JwtIssuer"],
                //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"])),
                //        ClockSkew = TimeSpan.Zero
                //    };
                //});

            //var multiSchemePolicy = new AuthorizationPolicyBuilder(
            //        CookieAuthenticationDefaults.AuthenticationScheme,
            //        JwtBearerDefaults.AuthenticationScheme)
            //    .RequireAuthenticatedUser()
            //    .Build();

            //services.AddAuthorization(options => options.DefaultPolicy = multiSchemePolicy);

            return services;
        }

        public static  void ConfigureServices(IServiceCollection services)
        {
            //New Add
            //services.AddDistributedMemoryCache();

            //Old
            services.AddSession(options =>
            {
				options.IdleTimeout = TimeSpan.FromMinutes(30); 
	             //options.IdleTimeout = TimeSpan.FromSeconds(10);

			});


        }
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {           
            app.UseSession();

        }

    }
}
