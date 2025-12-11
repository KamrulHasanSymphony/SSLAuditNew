//using Repository.Interfaces;
//using Repository.SqlServer;
//using System.Data.SqlClient;

using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.Repository;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.Audit;
using Shampan.Core.Interfaces.Repository.AuditFeedbackRepo;
using Shampan.Core.Interfaces.Repository.AuditIssues;
using Shampan.Core.Interfaces.Repository.AuditPoints;
using Shampan.Core.Interfaces.Repository.BKAuditCompliances;
using Shampan.Core.Interfaces.Repository.BKAuditInfoDetailss;
using Shampan.Core.Interfaces.Repository.BKAuditInfoMasterApprovals;
using Shampan.Core.Interfaces.Repository.BKAuditInfoMasters;
using Shampan.Core.Interfaces.Repository.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Repository.BKAuditOfficesPreferenceInfos;
using Shampan.Core.Interfaces.Repository.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Repository.BKAuditPreferenceEvaluation;
using Shampan.Core.Interfaces.Repository.BKAuditTemlateMasters;
using Shampan.Core.Interfaces.Repository.BKCheckListSubTypes;
using Shampan.Core.Interfaces.Repository.BKCheckListTypes;
using Shampan.Core.Interfaces.Repository.BKCommonSelectionSetting;
using Shampan.Core.Interfaces.Repository.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Repository.BKFinancePreformPreferencesCBS;
using Shampan.Core.Interfaces.Repository.BKFraudIrregularitiesInternalControlPreferencesCBS;
using Shampan.Core.Interfaces.Repository.BKFraudIrrgularitiesPreferenceSetting;
using Shampan.Core.Interfaces.Repository.BKInternalControlWeakPreferenceSetting;
using Shampan.Core.Interfaces.Repository.BKReguCompliancesPreferenceSetting;
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Repository.BKRiskAssessRegulationPreferencesCBS;
using Shampan.Core.Interfaces.Repository.Branch;
using Shampan.Core.Interfaces.Repository.Calender;
using Shampan.Core.Interfaces.Repository.Categorys;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Core.Interfaces.Repository.Circular;
using Shampan.Core.Interfaces.Repository.CISReport;
using Shampan.Core.Interfaces.Repository.Company;
using Shampan.Core.Interfaces.Repository.CompanyInfos;
using Shampan.Core.Interfaces.Repository.Deshboard;
using Shampan.Core.Interfaces.Repository.FiscalYear;
using Shampan.Core.Interfaces.Repository.HighLevelReports;
using Shampan.Core.Interfaces.Repository.Mail;
using Shampan.Core.Interfaces.Repository.ModulePermissions;
using Shampan.Core.Interfaces.Repository.Modules;
using Shampan.Core.Interfaces.Repository.Node;
using Shampan.Core.Interfaces.Repository.Notification;
using Shampan.Core.Interfaces.Repository.Oragnogram;
using Shampan.Core.Interfaces.Repository.Settings;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Core.Interfaces.Repository.TeamMember;
using Shampan.Core.Interfaces.Repository.Tour;
using Shampan.Core.Interfaces.Repository.TransportAllownace;
using Shampan.Core.Interfaces.Repository.TransportAllownaceDetails;
using Shampan.Core.Interfaces.Repository.User;
using Shampan.Core.Interfaces.Repository.UserRoll;
using Shampan.Core.Interfaces.Repository.UsersPermission;
using Shampan.Core.Interfaces.UnitOfWork;
using Shampan.Models;
using Shampan.Repository.SqlServer;
using Shampan.Repository.SqlServer.Advance;
using Shampan.Repository.SqlServer.Audit;
using Shampan.Repository.SqlServer.AuditFeedbackRepo;
using Shampan.Repository.SqlServer.AuditIssues;
using Shampan.Repository.SqlServer.AuditPoints;
using Shampan.Repository.SqlServer.BKAuditCompliances;
using Shampan.Repository.SqlServer.BKAuditInfoDetailss;
using Shampan.Repository.SqlServer.BKAuditInfoMasterApprovalApprovals;
using Shampan.Repository.SqlServer.BKAuditInfoMasters;
using Shampan.Repository.SqlServer.BKAuditOfficePreferencesCBS;
using Shampan.Repository.SqlServer.BKAuditOfficesPreferenceInfos;
using Shampan.Repository.SqlServer.BKAuditOfficeTypes;
using Shampan.Repository.SqlServer.BKAuditPreferenceEvaluation;
using Shampan.Repository.SqlServer.BKAuditTemlateMasters;
using Shampan.Repository.SqlServer.BKCheckListSubTypes;
using Shampan.Repository.SqlServer.BKCheckListTypes;
using Shampan.Repository.SqlServer.BKCommonSelectionSetting;
using Shampan.Repository.SqlServer.BKFinancePerformPreferenceSetting;
using Shampan.Repository.SqlServer.BKFinancePreformPreferencesCBS;
using Shampan.Repository.SqlServer.BKFraudIrregularitiesInternalControlPreferencesCBS;
using Shampan.Repository.SqlServer.BKFraudIrrgularitiesPreferenceSetting;
using Shampan.Repository.SqlServer.BKInternalControlWeakPreferenceSetting;
using Shampan.Repository.SqlServer.BKRiskAssessPerferenceSettings;
using Shampan.Repository.SqlServer.BKRiskAssessRegulationPreferencesCBS;
using Shampan.Repository.SqlServer.Branch;
using Shampan.Repository.SqlServer.Calender;
using Shampan.Repository.SqlServer.Categorys;
using Shampan.Repository.SqlServer.CheckListItems;
using Shampan.Repository.SqlServer.Circular;
using Shampan.Repository.SqlServer.CISReport;
using Shampan.Repository.SqlServer.Company;
using Shampan.Repository.SqlServer.CompanyInfos;
using Shampan.Repository.SqlServer.Deshboard;
using Shampan.Repository.SqlServer.FiscalYear;
using Shampan.Repository.SqlServer.HighLevelReport;
using Shampan.Repository.SqlServer.Mail;
using Shampan.Repository.SqlServer.ModulePermissions;
using Shampan.Repository.SqlServer.Modules;
using Shampan.Repository.SqlServer.Node;
using Shampan.Repository.SqlServer.Notification;
using Shampan.Repository.SqlServer.Oragnogram;
using Shampan.Repository.SqlServer.Settings;
using Shampan.Repository.SqlServer.Team;
using Shampan.Repository.SqlServer.TeamMember;
using Shampan.Repository.SqlServer.Tour;
using Shampan.Repository.SqlServer.TransportAllownace;
using Shampan.Repository.SqlServer.TransportAllownaceDetails;
using Shampan.Repository.SqlServer.User;
using Shampan.Repository.SqlServer.UserRoll;
using Shampan.Repository.SqlServer.UsersPermission;

namespace Shampan.UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServerRepository : IUnitOfWorkRepository
    {
        //public IProductRepository ProductRepository { get; }
        //public IClientRepository ClientRepository { get; }
        //public IInvoiceRepository InvoiceRepository { get; }
        //public IInvoiceDetailRepository InvoiceDetailRepository { get; }

        public UnitOfWorkSqlServerRepository(SqlConnection context, SqlTransaction transaction, DbConfig dBConfig)
        {

            AuditUserRepository = new AuditUserRepository(context, transaction, dBConfig);
            AuditIssueUserRepository = new AuditIssueUserRepository(context, transaction, dBConfig);
            CommonRepository = new CommonRepository(context, transaction);
            SettingsRepository = new SettingsRepository(context, transaction, dBConfig);
            CompanyInfoRepository = new CompanyInfoRepository(context, transaction);
            BranchProfileRepository = new BranchProfileRepository(context, transaction);
            CompanyInfosRepository = new CompanyInfosRepository(context, transaction);
            UserBranchRepository = new UserBranchRepository(context, transaction, dBConfig);
            AuditMasterRepository = new AuditMasterRepository(context, transaction, dBConfig);
            AuditAreasRepository = new AuditAreasRepository(context, transaction, dBConfig);

            //SSLAudit
            TeamsRepository = new TeamsRepository(context, transaction);
            TeamMembersRepository = new TeamMembersRepository(context, transaction, dBConfig);
            AdvancesRepository = new AdvancesRepository(context, transaction);
            ToursRepository = new ToursRepository(context, transaction);
            TransportAllownacesRepository = new TransportAllownacesRepository(context, transaction);
            UserRollsRepository = new UserRollsRepository(context, transaction);
            CircularsRepository = new CircularsRepository(context, transaction);
			CalendersRepository = new CalenderRepository(context, transaction);
			OragnogramRepository = new OragnogramRepository(context, transaction);
            AuditIssueRepository = new AuditIssueRepository(context, transaction, dBConfig);
            AuditIssueAttachmentsRepository = new AuditIssueAttachmentsRepository(context, transaction, dBConfig);
			CircularAttachmentsRepository = new CircularAttachmentsRepository(context, transaction, dBConfig);
			EmployeesHiAttachmentsRepository = new EmployeesHierarchyAttachmentsRepository(context, transaction, dBConfig);
            AuditFeedbackRepository = new AuditFeedbackRepository(context, transaction, dBConfig);
            AuditFeedbackAttachmentsRepository = new AuditFeedbackAttachmentsRepository(context, transaction, dBConfig);
            AuditBranchFeedbackRepository = new AuditBranchFeedbackRepository(context, transaction, dBConfig);
            AuditBranchFeedbackRepository = new AuditBranchFeedbackRepository(context, transaction, dBConfig);
            AuditBranchFeedbackAttachmentsRepository = new AuditBranchFeedbackAttachmentsRepository(context, transaction, dBConfig);
			ModuleRepository = new ModuleRepository(context, transaction);
			ModulePermissionRepository = new ModulePermissionRepository(context, transaction);
            DeshboardRepository = new DeshboardRepository(context, transaction, dBConfig);
			UsersPermissionRepository = new UsersPermissionRepository(context, transaction, dBConfig);
            CISReportRepository = new CISReportRepository(context, transaction, dBConfig);
            DateWisePolicyEditLogRepository = new DateWisePolicyEditLogRepository(context, transaction, dBConfig);
			DateWisePolicyEditLogWithDetailsRepository = new DateWisePolicyEditLogWithDetailsRepository(context, transaction, dBConfig);
			CollectionEditLogRepository = new CollectionEditLogRepository(context, transaction, dBConfig);
			DocumentWiseEditLogRepository = new DocumentWiseEditLogRepository(context, transaction, dBConfig);
            NodeRepository = new NodeRepository(context, transaction, dBConfig);
            TransportAllownaceDetailRepository = new TransportAllownaceDetailRepository(context, transaction, dBConfig);
            NotificationRepository = new NotificationRepository(context, transaction, dBConfig);
            MailRepository = new MailRepository(context, transaction, dBConfig);

            CategoryRepository = new CategoryRepository(context, transaction, dBConfig);
            CheckListItemRepository = new CheckListItemRepository(context, transaction, dBConfig);
            BKAuditComplianceRepository = new BKAuditComplianceRepository(context, transaction, dBConfig);
            BKAuditOfficeTypeRepository = new BKAuditOfficeTypeRepository(context, transaction, dBConfig);
            BKCheckListTypeRepository = new BKCheckListTypeRepository(context, transaction, dBConfig);
            BKCheckListSubTypeRepository = new BKCheckListSubTypeRepository(context, transaction, dBConfig);
            BKAuditTemlateMasterRepository = new BKAuditTemlateMasterRepository(context, transaction, dBConfig);
            BKRiskAssessPerferenceSettingRepository = new BKRiskAssessPerferenceSettingRepository(context, transaction, dBConfig);

            BKReguCompliancesPreferenceSettingRepository = new BKReguCompliancesPreferenceSettingsRepository(context, transaction, dBConfig);
            BKFinancePerformPreferenceSettingRepository = new BKFinancePerformPreferenceSettingRepository(context, transaction, dBConfig);
            BKFraudIrrgularitiesPreferenceSettingRepository = new BKFraudIrrgularitiesPreferenceSettingsRepository(context, transaction, dBConfig);
            BKInternalControlWeakPreferenceSettingRepository = new BKInternalControlWeakPreferenceSettingsRepository(context, transaction, dBConfig);

            BKCommonSelectionSettingRepository = new BKCommonSelectionSettingRepository(context, transaction, dBConfig);
            BKAuditPreferenceEvaluationRepository = new BKAuditPreferenceEvaluationRepository(context, transaction, dBConfig);
            BKAuditOfficesPreferenceInfoRepository = new BKAuditOfficesPreferenceInfoRepository(context, transaction, dBConfig);
            BKAuditInfoMasterRepository = new BKAuditInfoMasterRepository(context, transaction, dBConfig);
            BKAuditInfoDetailsRepository = new BKAuditInfoDetailsRepository(context, transaction, dBConfig);
            BKAuditInfoMasterApprovalRepository = new BKAuditInfoMasterApprovalRepository(context, transaction, dBConfig);

            AuditPointRepository = new AuditPointRepository(context, transaction, dBConfig);

            BKAuditOfficePreferenceCBSRepository = new BKAuditOfficePreferenceCBSRepository(context, transaction, dBConfig);
            BKFinancePreformPreferenceCBSRepository = new BKFinancePreformPreferenceCBSRepository(context, transaction, dBConfig);
            BKFraudIrregularitiesInternalControlPreferenceCBSRepository = new BKFraudIrregularitiesInternalControlPreferenceCBSRepository(context, transaction, dBConfig);
            BKRiskAssessRegulationPreferenceCBSRepository = new BKRiskAssessRegulationPreferenceCBSRepository(context, transaction, dBConfig);

            FiscalYearRepository = new FiscalYearRepository(context, transaction, dBConfig);

            HighLevelReportRepository = new HighLevelReportRepository(context, transaction, dBConfig);
           
        }
        public ICompanyInfoRepository CompanyInfoRepository { get; }
        public ICommonRepository CommonRepository { get; }


        public IAuditFeedbackRepository AuditFeedbackRepository { get; }
        public IAuditBranchFeedbackRepository AuditBranchFeedbackRepository { get; }
        public IAuditFeedbackAttachmentsRepository AuditFeedbackAttachmentsRepository { get; }
        public IAuditUserRepository AuditUserRepository { get; }
        public IAuditIssueUserRepository AuditIssueUserRepository { get; }
        public IUserBranchRepository UserBranchRepository { get; }
        public IAuditMasterRepository AuditMasterRepository { get; }
        public IAuditIssueAttachmentsRepository AuditIssueAttachmentsRepository { get; }
        public IAuditIssueRepository AuditIssueRepository { get; }
        public IAuditAreasRepository AuditAreasRepository { get; }

        public IBranchProfileRepository BranchProfileRepository { get; }

        public ICompanyinfosRepository CompanyInfosRepository { get; }

        public ISettingsRepository SettingsRepository { get; }

        public ITeamsRepository TeamsRepository { get; }

        public ITeamMembersRepository TeamMembersRepository { get; }

        public IAdvancesRepository AdvancesRepository { get; }

        public IToursRepository ToursRepository { get; }

        public ITransportAllownacesRepository TransportAllownacesRepository { get; }

        public IUserRollsRepository UserRollsRepository { get; }

        public ICircularsRepository CircularsRepository { get; }

		public ICalendersRepository CalendersRepository { get; }

		public IOragnogramRepository OragnogramRepository { get; }

		public ICircularAttachmentsRepository CircularAttachmentsRepository { get; }

		public IEmployeesHiAttachmentsRepository EmployeesHiAttachmentsRepository { get; }

        public IAuditBranchFeedbackAttachmentsRepository AuditBranchFeedbackAttachmentsRepository { get; }

		public IModuleRepository ModuleRepository { get; }

		public IModulePermissionRepository ModulePermissionRepository { get; }

        public IDeshboardRepository DeshboardRepository { get; }

		public IUsersPermissionRepository UsersPermissionRepository { get; }

        public ICISReportRepository CISReportRepository { get; }

        public IDateWisePolicyEditLogRepository DateWisePolicyEditLogRepository { get; }

		public IDateWisePolicyEditLogWithDetailsRepository DateWisePolicyEditLogWithDetailsRepository { get; }

		public ICollectionEditLogRepository CollectionEditLogRepository { get; }

		public IDocumentWiseEditLogRepository DocumentWiseEditLogRepository { get; }

        public INodeRepository NodeRepository { get; }

        public ITransportAllownaceDetailRepository TransportAllownaceDetailRepository { get; }

        public INotificationRepository NotificationRepository { get; }

        public IMailRepository MailRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public ICheckListItemRepository CheckListItemRepository { get; }

        public IBKAuditComplianceRepository BKAuditComplianceRepository { get; }

        public IBKAuditOfficeTypeRepository BKAuditOfficeTypeRepository { get; }

        public IBKCheckListTypeRepository BKCheckListTypeRepository { get; }

        public IBKCheckListSubTypeRepository BKCheckListSubTypeRepository { get; }

        public IBKAuditTemlateMasterRepository BKAuditTemlateMasterRepository { get; }

        public IBKRiskAssessPerferenceSettingRepository BKRiskAssessPerferenceSettingRepository { get; }

        public IBKReguCompliancesPreferenceSettingRepository BKReguCompliancesPreferenceSettingRepository { get; }

        public IBKFinancePerformPreferenceSettingRepository BKFinancePerformPreferenceSettingRepository { get; }

        public IBKFraudIrrgularitiesPreferenceSettingRepository BKFraudIrrgularitiesPreferenceSettingRepository { get; }

        public IBKInternalControlWeakPreferenceSettingRepository BKInternalControlWeakPreferenceSettingRepository { get; }

        public IBKCommonSelectionSettingRepository BKCommonSelectionSettingRepository { get; }

        public IBKAuditPreferenceEvaluationRepository BKAuditPreferenceEvaluationRepository { get; }

        public IBKAuditOfficesPreferenceInfoRepository BKAuditOfficesPreferenceInfoRepository { get; }

        public IBKAuditInfoMasterRepository BKAuditInfoMasterRepository { get; }

        public IBKAuditInfoDetailsRepository BKAuditInfoDetailsRepository { get; }

        public IBKAuditInfoMasterApprovalRepository BKAuditInfoMasterApprovalRepository { get; }

        public IAuditPointRepository AuditPointRepository { get; }

        public IBKAuditOfficePreferenceCBSRepository BKAuditOfficePreferenceCBSRepository { get; }

        public IBKFinancePreformPreferenceCBSRepository BKFinancePreformPreferenceCBSRepository { get; }

        public IBKFraudIrregularitiesInternalControlPreferenceCBSRepository BKFraudIrregularitiesInternalControlPreferenceCBSRepository { get; }

        public IBKRiskAssessRegulationPreferenceCBSRepository BKRiskAssessRegulationPreferenceCBSRepository { get; }

        public IFiscalYearRepository FiscalYearRepository { get; }

        public IHighLevelReportRepository HighLevelReportRepository { get; }
    }
}
