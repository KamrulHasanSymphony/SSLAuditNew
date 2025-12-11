//using Repository.Interfaces;

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

namespace Shampan.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        ICommonRepository CommonRepository { get; }


        IAuditFeedbackRepository AuditFeedbackRepository { get; }
        IAuditBranchFeedbackRepository AuditBranchFeedbackRepository { get; }
        IAuditFeedbackAttachmentsRepository AuditFeedbackAttachmentsRepository { get; }
        IAuditUserRepository AuditUserRepository { get; }

        IAuditIssueUserRepository AuditIssueUserRepository { get; }
        //change
        ISettingsRepository SettingsRepository { get; }
        ICompanyInfoRepository CompanyInfoRepository { get; }
        IBranchProfileRepository BranchProfileRepository { get; }
        ICompanyinfosRepository CompanyInfosRepository { get; }
        IUserBranchRepository UserBranchRepository { get; }
        ITeamsRepository TeamsRepository { get; }
        ITeamMembersRepository TeamMembersRepository { get; }
        IAdvancesRepository AdvancesRepository { get; }
        IToursRepository ToursRepository { get; }
        ITransportAllownacesRepository TransportAllownacesRepository { get; }
		IUserRollsRepository UserRollsRepository { get; }

        IAuditAreasRepository AuditAreasRepository { get; }
        IAuditMasterRepository AuditMasterRepository { get; }
        ICircularsRepository CircularsRepository { get; }
		ICalendersRepository CalendersRepository { get; }
		IOragnogramRepository OragnogramRepository { get; }


        IAuditIssueAttachmentsRepository AuditIssueAttachmentsRepository { get; }
        IAuditIssueRepository AuditIssueRepository { get; }
		ICircularAttachmentsRepository CircularAttachmentsRepository { get; }
		IEmployeesHiAttachmentsRepository EmployeesHiAttachmentsRepository { get; }

        IAuditBranchFeedbackAttachmentsRepository AuditBranchFeedbackAttachmentsRepository { get; }
		IModuleRepository ModuleRepository { get; }
		IModulePermissionRepository ModulePermissionRepository { get; }
        IDeshboardRepository DeshboardRepository { get; }
		IUsersPermissionRepository UsersPermissionRepository { get; }
        ICISReportRepository CISReportRepository { get; }
        IDateWisePolicyEditLogRepository DateWisePolicyEditLogRepository { get; }
		IDateWisePolicyEditLogWithDetailsRepository DateWisePolicyEditLogWithDetailsRepository { get; }
		ICollectionEditLogRepository CollectionEditLogRepository { get; }
		IDocumentWiseEditLogRepository DocumentWiseEditLogRepository { get; }
        INodeRepository NodeRepository { get; }
        ITransportAllownaceDetailRepository TransportAllownaceDetailRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IMailRepository MailRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICheckListItemRepository CheckListItemRepository { get; }
        IBKAuditComplianceRepository BKAuditComplianceRepository { get; }
        IBKAuditOfficeTypeRepository BKAuditOfficeTypeRepository { get; }
        IBKCheckListTypeRepository BKCheckListTypeRepository { get; }
        IBKCheckListSubTypeRepository BKCheckListSubTypeRepository { get; }
        IBKAuditTemlateMasterRepository BKAuditTemlateMasterRepository { get; }
        IBKRiskAssessPerferenceSettingRepository BKRiskAssessPerferenceSettingRepository { get; }
        IBKReguCompliancesPreferenceSettingRepository BKReguCompliancesPreferenceSettingRepository { get; }
        IBKFinancePerformPreferenceSettingRepository BKFinancePerformPreferenceSettingRepository { get; }
        IBKFraudIrrgularitiesPreferenceSettingRepository BKFraudIrrgularitiesPreferenceSettingRepository { get; }
        IBKInternalControlWeakPreferenceSettingRepository BKInternalControlWeakPreferenceSettingRepository { get; }
        IBKCommonSelectionSettingRepository BKCommonSelectionSettingRepository { get; }
        IBKAuditPreferenceEvaluationRepository BKAuditPreferenceEvaluationRepository { get; }
        IBKAuditOfficesPreferenceInfoRepository BKAuditOfficesPreferenceInfoRepository { get; }
        IBKAuditInfoMasterRepository BKAuditInfoMasterRepository { get; }
        IBKAuditInfoDetailsRepository BKAuditInfoDetailsRepository { get; }
        IBKAuditInfoMasterApprovalRepository BKAuditInfoMasterApprovalRepository { get; }
        IAuditPointRepository AuditPointRepository { get; }
        IBKAuditOfficePreferenceCBSRepository BKAuditOfficePreferenceCBSRepository { get; }
        IBKFinancePreformPreferenceCBSRepository BKFinancePreformPreferenceCBSRepository { get; }
        IBKFraudIrregularitiesInternalControlPreferenceCBSRepository BKFraudIrregularitiesInternalControlPreferenceCBSRepository { get; }
        IBKRiskAssessRegulationPreferenceCBSRepository BKRiskAssessRegulationPreferenceCBSRepository { get; }
        IFiscalYearRepository FiscalYearRepository { get; }
        IHighLevelReportRepository HighLevelReportRepository { get; }

    }
}
