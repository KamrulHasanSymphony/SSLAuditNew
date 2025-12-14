using Shampan.Core.Interfaces.Services.Settings;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private IUnitOfWork _unitOfWork;

        public SettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }
        public ResultModel<DbUpdateModel> DbUpdate(DbUpdateModel model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    #region Settings
                    SettingsModel vm = new SettingsModel();

                    context.Repositories.SettingsRepository.SettingsDataInsert(vm, "AuditPoint", "Demo", "Decimal", "5");
                    context.Repositories.SettingsRepository.SettingsDataInsert(vm, "AuditPoint", "MaximumMark", "Decimal", "7");
                    context.Repositories.SettingsRepository.SettingsDataInsert(vm, "AuditPoint", "AuditIssueLevel", "Decimal", "4");
                    context.Repositories.SettingsRepository.SettingsDataInsert(vm, "AuditPoint", "AuditMarkLevel", "Decimal", "3");
                    context.Repositories.SettingsRepository.SettingsDataInsert(vm, "Company", "Code", "String", "GDIC");
                    //context.Repositories.SettingsRepository.SettingsDataInsert(vm, "Branch", "ProfileName", "String", "-");
                    //context.Repositories.SettingsRepository.SettingsDataInsert(vm, "Journal", "SourceLedger", "String", "-");
                    //context.Repositories.SettingsRepository.SettingsDataInsert(vm, "Segments", "SegmentCode", "String", "-");

                    #endregion Settings


                    #region Table Add

                    #region TestHeader
                    String sqlText = " ";
                    sqlText = @"
    CREATE TABLE [dbo].[TestHeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[GLAccount] [nvarchar](50) NULL,
	[TransDate] [date] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
	[PostedBy] [nvarchar](50) NULL,
	[PostedOn] [datetime] NULL,
	[PostedFrom] [nvarchar](50) NULL,
	[PushedBy] [nvarchar](50) NULL,
	[PushedOn] [datetime] NULL,
	[PushedFrom] [nvarchar](50) NULL,
	[BranchId] [int] NULL,
	[CompanyId] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_TestHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";
                    context.Repositories.SettingsRepository.NewTableAdd("TestHeader", sqlText);

                    #endregion

                    #region TestDetails
                    sqlText = " ";

                    sqlText = @"
CREATE TABLE [dbo].[TestDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TestHeaderId] [int] NULL,
	[BankCode] [nvarchar](50) NULL,
	[Amount] [decimal](18, 4) NULL,
	[Quantity] [decimal](18, 4) NULL,
 CONSTRAINT [PK_TestDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


"
                        ;


                    context.Repositories.SettingsRepository.NewTableAdd("TestDetails", sqlText);



                    #endregion

                    #region TransportAllownaceDetails

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[TransportAllownaceDetails](
	[Id] [int] NOT NULL,
	[TransportAllowanceId] [int] NULL,
	[Date] [datetime] NULL,
	[Particulars] [nvarchar](MAX) NULL,
	[Amount] [decimal](18, 0) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_TransportAllownaceDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("TransportAllownaceDetails", sqlText);

                    #endregion TransportAllownaceDetails

                    #region TransportAllownaceOthers

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[TransportAllownaceOthers](
	[Id] [int] NOT NULL,
	[TransportAllowanceId] [int] NULL,
	[Date] [datetime] NULL,
	[Details] [nvarchar](MAX) NULL,
	[Amount] [decimal](18, 0) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_TransportAllownaceOthers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("TransportAllownaceOthers", sqlText);

                    #endregion TransportAllownaceOthers

                    #region TransportAllownaceLessAdvance

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[TransportAllownaceLessAdvance](
	[Id] [int] NOT NULL,
	[TransportAllowanceId] [int] NULL,
	[Date] [datetime] NULL,
	[Details] [nvarchar](MAX) NULL,
	[Amount] [decimal](18, 0) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_TransportAllownaceLessAdvance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("TransportAllownaceLessAdvance", sqlText);

                    #endregion TransportAllownaceOthers

                    #region AuditPoints

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[AuditPoints](
	[Id] [int] NOT NULL,
	[PId] [int] NULL,
	[AuditTypeId] [int] NULL,
	[AuditType] [nvarchar](500) NULL,
	[WeightPersent] [int] NULL,
	[P_Level] [int] NULL,


 CONSTRAINT [PK_AuditPoints] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("AuditPoints", sqlText);

                    #endregion AuditPoints



                    #region AuditApprover

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[AuditApprover](
	[Id] [int] NOT NULL,
	[UserId] [nvarchar](250) NULL,
    [ApproverSLNo] [int] NULL,
    [ModuleId] [nvarchar](250) NULL,
    [IsApproved] [bit] NULL,

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_AuditApprover] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("AuditApprover", sqlText);

                    #endregion AuditApprover



                    #region AuditComponent

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[AuditComponent](

	[Id] [int] NOT NULL,
	[BranchPlan] [int] NULL,
    [BranchOngoing] [int] NULL,
    [BranchCompleted] [int] NULL,
    [BranchRemaining] [int] NULL,
    [BranchCompletedOngoing] [int] NULL,
    [DepartmentPlan] [int] NULL,
    [DepartmentOngoing] [int] NULL,
    [DepartmentCompleted] [int] NULL,
    [DepartmentRemaining] [int] NULL,
    [DepartmentCompletedOngoing] [int] NULL,
    [SubsidiaryPlan] [int] NULL,
    [SubsidiaryOngoing] [int] NULL,
    [SubsidiaryCompleted] [int] NULL,
    [SubsidiaryRemaining] [int] NULL,
    [SubsidiaryCompletedOngoing] [int] NULL,
    [FollowUpPlan] [int] NULL,
    [FollowUpOngoing] [int] NULL,
    [FollowUpCompleted] [int] NULL,
    [FollowUpRemaining] [int] NULL,
    [FollowUpCompletedOngoing] [int] NULL,
    [UnderwritingPlan] [int] NULL,
    [UnderwritingOngoing] [int] NULL,
    [UnderwritingCompleted] [int] NULL,
    [UnderwritingRemaining] [int] NULL,
    [UnderwritingCompletedOngoing] [int] NULL,
    [PettyCasHOPlan] [int] NULL,
    [PettyCasHOOngoing] [int] NULL,
    [PettyCasHOCompleted] [int] NULL,
    [PettyCasHORemaining] [int] NULL,
    [PettyCasHOCompletedOngoing] [int] NULL,
    [PettyCashBranchPlan] [int] NULL,
    [PettyCashBranchOngoing] [int] NULL,
    [PettyCashBranchCompleted] [int] NULL,
    [PettyCashBranchRemaining] [int] NULL,
    [PettyCashBranchCompletedOngoing] [int] NULL,
    [MonthlyBCOReportingPlan] [int] NULL,
    [MonthlyBCOReportingOngoing] [int] NULL,
    [MonthlyBCOReportingCompleted] [int] NULL,
    [MonthlyBCOReportingRemaining] [int] NULL,
    [MonthlyBCOReportingCompletedRemaining] [int] NULL,

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_AuditComponent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("AuditComponent", sqlText);

                    #endregion AuditComponent



                    #region AuditOfficesPreferenceInfo

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[AuditOfficesPreferenceInfo](

	[Id] [int] NOT NULL,
	[AuditOfficeTypesId] [int] NULL,
    [AuditOfficeId] [int] NULL,
    [IsHistoricalPerformhistorical] [bit] NULL,
    [AuditYear] [int] NULL,
    [AuditFiscalYear] [nvarchar](250) NULL,
    [AuditPreferenceValue] [nvarchar](250) NULL,
    [EntreyDate] [datetime] NULL,
    [UpdateDate] [datetime] NULL,
    [Status] [bit] NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_AuditOfficesPreferenceInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("AuditOfficesPreferenceInfo", sqlText);


                    #endregion AuditOfficesPreferenceInfo




                    #region AuditPreferenceEvalutions

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[AuditPreferenceEvalutions](

	[Id] [int] NOT NULL,
	[PreferenceValue] [nvarchar](MAX) NULL,
	[FlagPercentFromCommonSettingSelectedValuesMin] [int] NULL,
    [FlagPercentFromCommonSettingSelectedValuesMax] [int] NULL,
    [FlagPercentFromRiskAssessSelectedValuesMin] [int] NULL,
    [FlagPercentFromRiskAssessSelectedValuesMax] [int] NULL,
    [FlagPercentFromReguCompliancesSelectedValuesMin] [int] NULL,
    [FlagPercentFromReguCompliancesSelectedValuesMax] [int] NULL,
    [FlagPercentFromFinancePreformSelectedValuesMin] [int] NULL,
    [FlagPercentFromFinancePreformSelectedValuesMax] [int] NULL,
    [FlagPercentFromFraudIrregularitiesSelectedValuesMin] [int] NULL,
    [FlagPercentFromFraudIrregularitiesSelectedValuesMax] [int] NULL,
    [FlagPercentPromInternalControlWeakSelectedValuesMin] [int] NULL,
    [FlagPercentFromInternalControlWeakSelectedValuesMax] [int] NULL,
    [EntryDate] [datetime] NULL,
    [UpdateDate] [datetime] NULL,
    [Status] [bit] NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_AuditPreferenceEvalutions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("AuditPreferenceEvalutions", sqlText);


                    #endregion AuditPreferenceEvalutions



                    #region AuditTemplateDetails

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[AuditTemplateDetails](

	[Id] [int] NOT NULL,
	[AuditId] [int] NULL,
    [BKAuditOfficeTypeId] [int] NULL,
    [BKAuditTemlateMasterId] [int] NULL,
    [BKAuditComplianceId] [int] NULL,
    [BKCheckListTypeId] [int] NULL,
    [BKCheckListSubTypeId] [int] NULL,
    [BKCheckListItemId] [int] NULL,
    [IsFieldType] [bit] NULL,
    [Status] [bit] NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_AuditTemplateDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("AuditTemplateDetails", sqlText);


                    #endregion AuditTemplateDetails


                    #region BKAuditCategories

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditCategories](

	[Id] [int] NOT NULL,
	[Code] [nvarchar](50) NULL,
    [CategoryName] [nvarchar](50) NULL,
    [Status] [bit] NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditCategories", sqlText);


                    #endregion BKAuditCategories



                    #region BKAuditCompliances

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditCompliances](

	[Id] [int] NOT NULL,
	[Code] [nvarchar](50) NULL,
    [BKAuidtCategoryId] [int] NULL,
    [BKAuditOfficeTypesId] [int] NULL,
    [Description] [nvarchar](MAX) NULL,
    [Date] [datetime] NULL,
    [Severity] [nvarchar](50) NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditCompliances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditCompliances", sqlText);


                    #endregion BKAuditCompliances


                    #region BKAuditInfoDetails

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditInfoDetails](

	[Id] [int] NOT NULL,
	[BKAuditOffeceTypeId] [int] NULL,
    [BKAuditInfoMasterId] [int] NULL,
    [BKChecklistItemId] [int] NULL,
    [BKCheckListSubItemId] [int] NULL,
    [IsFieldType] [bit] NULL,
    [Status] [bit] NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditInfoDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditInfoDetails", sqlText);


                    #endregion BKAuditInfoDetails



                    #region BKAuditInfoMaster

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditInfoMaster](

	[Id] [int] NOT NULL,
	[BKAuditOfficeTypeId] [int] NULL,
    [BKAuditTemplateId] [int] NULL,
    [Date] [datetime] NULL,
    [Status] [bit] NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditInfoMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditInfoMaster", sqlText);


                    #endregion BKAuditInfoMaster



                    #region BKAuditInfoMasterApproval

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditInfoMasterApproval](

	[Id] [int] NOT NULL,
    [Code] [nvarchar](50) NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BKAuditInfoMasterId] [int]  NULL,
    [Date] [datetime]  NULL,
    [AuditApprovalFlag] [bit]  NULL,
    [ApprovalAuthorityDesc] [nvarchar](MAX)  NULL,
    [LastApprovedBy] [nvarchar](50) NULL,
    [LastApprovedDate] [datetime]  NULL,
    [Status] [bit]  NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditInfoMasterApproval] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditInfoMasterApproval", sqlText);


                    #endregion BKAuditInfoMasterApproval



                    #region BKAuditOfficePreferenceCBSTemp

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditOfficePreferenceCBSTemp](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [AuditYear] [datetime] NULL,
    [AuditFiscalYear] [datetime] NULL,
    [HistoricalPerformFlg] [bit]  NULL,
    [LastYearAuditFindingsFlg] [bit]  NULL,
    [PreviousYearsExceptLastYearAuditFindingsFlg] [bit]  NULL,
    [TechCyberRiskFlg] [bit]  NULL,
    [OfficeSizeFlg] [bit]  NULL,
    [OfficeSignificanceFlg] [bit]  NULL,
    [StaffTurnoverFlg] [bit]  NULL,
    [StaffTrainingGapsFlg] [bit] NULL,
    [StrategicInitiativeFlg] [bit]  NULL,
    [OperationalCompFlg] [bit]  NULL,
    [EntryDate] [datetime]  NULL,
    [UpdateDate] [datetime]  NULL,
    [Status] [bit]  NULL,
    [ImportId] [int]  NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditOfficePreferenceCBSTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditOfficePreferenceCBSTemp", sqlText);


                    #endregion BKAuditOfficePreferenceCBSTemp





                    #region BKAuditOfficesPreferenceInfo

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditOfficesPreferenceInfo](

	[Id] [int] NOT NULL,
    [Code] [nvarchar](50) NULL,
    [BranchID] [int] NULL,
    [BKAuditOfficeTypeId] [int] NULL,
    [BKAuditOfficeId] [int]  NULL,
    [HistoricalPerformFlag] [bit] NULL,
    [AuditYear] [datetime] NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [AuditPerferenceValue] [nvarchar](50) NULL,
    [EntryDate] [datetime] NULL,
    [Status] [bit] NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditOfficesPreferenceInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditOfficesPreferenceInfo", sqlText);


                    #endregion BKAuditOfficesPreferenceInfo


                    #region BKAuditOfficeTypes

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditOfficeTypes](

	[Id] [int] NOT NULL,
    [Code] [nvarchar](50) NULL,
    [Name] [nvarchar](50) NULL,
    [Status] [bit] NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditOfficeTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditOfficeTypes", sqlText);


                    #endregion BKAuditOfficeTypes



                    #region BKAuditPreferenceEvaluations

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditPreferenceEvaluations](

	[Id] [int] NOT NULL,
    [Code] [nvarchar](50) NULL,
    [BkAuditOfficeId] [int]  NULL,
    [BranchID] [int]  NULL,
    [FlagPercentFromCommonSettingSelectedValuesMin] [decimal](18,2) NULL,
    [FlagPercentFromCommonSettingSelectedValuesMax] [decimal](18, 4) NULL,
    [FlagPercentFromRiskAssessSelectedValuesMin] [decimal](18, 4) NULL,
    [FlagPercentFromRiskAssessSelectedValuesMax] [decimal](18, 4) NULL,
    [FlagPercentFromReguCompliancesSelectedValuesMin] [decimal](18, 4) NULL,
    [FlagPercentFromReguComliancesSelectedValuesMax] [decimal](18, 4) NULL,
    [FlagPercentFromFinancePerformSelectedValuesMin] [decimal](18, 4) NULL,
    [FlagPercentFromFinancePerformSelectedValuesMax] [decimal](18, 4) NULL,
    [FlagPercentFromFraudIrrgularitiesSelectedValuesMin] [decimal](18, 4) NULL,
    [FlagPercentFromFraudIrregularitiesSelectedValuesMax] [decimal](18, 4) NULL,
    [FlagPercentFromInternalControlWeakSelectedValuesMin] [decimal](18, 4) NULL,
    [FlagPercentFromInternalControlWeakSelectedValuesMax] [decimal](18, 4) NULL,
    [EntryDate] [datetime]  NULL,
    [Status] [bit]  NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditPreferenceEvaluations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditPreferenceEvaluations", sqlText);


                    #endregion BKAuditPreferenceEvaluations



                    #region BKAuditTemlateMaster

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditTemlateMaster](

	[Id] [int] NOT NULL,
    [Code] [nvarchar](50) NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BKAuditCategoryId] [int]  NULL,
    [Description] [nvarchar](MAX) NULL,
    [Status] [bit]  NULL,
    

	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

 CONSTRAINT [PK_BKAuditTemlateMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditTemlateMaster", sqlText);


                    #endregion BKAuditTemlateMaster



                    #region BKAuditTemplateDetails

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKAuditTemplateDetails](

	[Id] [int] NOT NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BKAuditTemlateMasterId] [int]  NULL,
    [BKAuditComplianceId] [int]  NULL,
    [BKCheckListTypeId] [int]  NULL,
    [BKCheckListSubTypeId] [int]  NULL,
    [BKCheckListItemId] [int] NULL,
    [IsFieldType] [bit]  NULL,
    [Status] [bit] NULL,
    

 CONSTRAINT [PK_BKAuditTemplateDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKAuditTemplateDetails", sqlText);


                    #endregion BKAuditTemplateDetails



                    #region BKCheckListItems

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKCheckListItems](

	[Id] [int] NOT NULL,
     [Code] [nvarchar](250) NULL,
    [BkCheckListTypesId] [int]  NULL,
    [BKCheckListSubTypesId] [int]  NULL,
    [Description] [nvarchar](MAX)  NULL,
    [IsFieldType] [bit]  NULL,
    [Status] [bit] NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKCheckListItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKCheckListItems", sqlText);


                    #endregion BKCheckListItems





                    #region BKCheckListSubTypes

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKCheckListSubTypes](

	[Id] [int] NOT NULL,
    [Code] [nvarchar](250) NULL,
    [BkCheckListTypesId] [int]  NULL,
    [Description] [nvarchar](MAX)  NULL,
    [Status] [bit] NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKCheckListSubTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKCheckListSubTypes", sqlText);


                    #endregion BKCheckListSubTypes


                    #region BKCheckListTypes

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKCheckListTypes](

	[Id] [int] NOT NULL,
    [Code] [nvarchar](250) NULL,
    [BkAuditCompliancesId] [int]  NULL,
    [Description] [nvarchar](MAX)  NULL,
    [Status] [bit] NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKCheckListTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKCheckListTypes", sqlText);


                    #endregion BKCheckListTypes


                    #region BKCommonSelectionSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKCommonSelectionSettings](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [Code] [nvarchar](250) NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BKAuditOfficeId] [int]  NULL,
    [HitoricalPreformFlag] [bit]  NULL,
    [HistoricalPerformFlagDesc] [nvarchar](MAX)  NULL,
    [LastYearAuditFindingFlag] [bit]  NULL,
    [LastYearAuditFindingFlagDesc] [nvarchar](MAX)  NULL,
    [PreviousYearExceptLastYearAuditFindingFlag] [bit]  NULL,
    [PreviousYearExceptLastYearAuditFindingFlagDesc] [nvarchar](MAX)  NULL,
    [TechCyberRiskFlag] [bit]  NULL,
    [OfficeSizeFlag] [bit]  NULL,
    [OfficeSignificanceFlag] [bit]  NULL,
    [TechCyberRiskFlagDesc] [nvarchar](MAX)  NULL,
    [StaffTurnoverFlag] [bit]  NULL,
    [StaffTurnoverFlagDesc] [nvarchar](MAX)  NULL,
    [StaffTrainingGapsFlag] [bit]  NULL,
    [StaffTrainingGapsFlagDesc] [nvarchar](MAX)  NULL,
    [StrategicInitiativeFlagveFlag] [bit]  NULL,
    [StrategicInitiativeFlagDesc] [nvarchar](MAX)  NULL,
    [OperationalCompFlag] [bit]  NULL,
    [OperationalCompFlagDesc] [nvarchar](MAX)  NULL,
    [Status] [bit]  NULL,
    [EntryDate] [datetime]  NULL,
    [AuditYear] [datetime] NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoReceiveDate] [datetime]  NULL,
    [InfoReceiveId] [nvarchar](50) NULL,
    [InfoReceiveFlag] [bit]  NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKCommonSelectionSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKCommonSelectionSettings", sqlText);


                    #endregion BKCommonSelectionSettings




                    #region BKFinancePerformPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKFinancePerformPreferenceSettings](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [Code] [nvarchar](250) NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BkAuditOfficeId] [int]  NULL,
    [FundAvailableFlag] [bit] NULL,
    [MisManagementClinentsFlag] [bit]  NULL,
    [EfficienctyFlag] [bit]  NULL,
    [NPLSLargeFlag] [bit]  NULL,
    [LargeTxnManageFlag] [bit]  NULL,
    [HighValueAssetMangeFlag] [bit]  NULL,
    [BudgetMgtFlag] [bit]  NULL,
    [Status] [bit]  NULL,
    [AuditYear] [datetime] NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoReceiveDate] [datetime]  NULL,
    [InfoReceiveId] [nvarchar](50) NULL,
    [InfoReceiveFlag] [bit]  NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKFinancePerformPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKFinancePerformPreferenceSettings", sqlText);


                    #endregion BKFinancePerformPreferenceSettings


                    #region BKFinancePreformPreferenceCBSTemp

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKFinancePreformPreferenceCBSTemp](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [AuditYear] [datetime] NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [FinancialPerformFlg] [bit]  NULL,
    [FundAvailableFlg] [bit]  NULL,
    [MisManagementClientsFlg] [bit]  NULL,
    [EfficiencyFlg] [bit]  NULL,
    [NplsLargeFlg] [bit]  NULL,
    [LargeTxnManageFlg] [bit]  NULL,
    [HighValueAssetManageFlg] [bit]  NULL,
    [SecurityMeasuresStaffFlg] [bit]  NULL,
    [BudgetMgtFlg] [bit]  NULL,
    [SignificantLossesFlg] [bit]  NULL,
    [Status] [bit]  NULL,
    [ImportId] [int]  NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKFinancePreformPreferenceCBSTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKFinancePreformPreferenceCBSTemp", sqlText);


                    #endregion BKFinancePreformPreferenceCBSTemp




                    #region BKFraudIrregularitiesInternalControlPreferenceCBSTemp

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKFraudIrregularitiesInternalControlPreferenceCBSTemp](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [AuditYear] [datetime] NULL,
    [AuditFiscalYear] [datetime] NULL,
    [PreviouslyFraudIncidentFlg] [bit]  NULL,
    [EmpMisConductFlg] [bit]  NULL,
    [IrregularitiesFlg] [bit]  NULL,
    [InternalControlFlg] [bit]  NULL,
    [ProperDocumentationFlg] [bit]  NULL,
    [ProperReportingFlg] [bit]  NULL,
    [Status] [bit]  NULL,
    [ImportId] [int]  NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKFraudIrregularitiesInternalControlPreferenceCBSTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKFraudIrregularitiesInternalControlPreferenceCBSTemp", sqlText);


                    #endregion BKFraudIrregularitiesInternalControlPreferenceCBSTemp




                    #region BKFraudIrrgularitiesPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKFraudIrrgularitiesPreferenceSettings](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [Code] [nvarchar](50) NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BKAuditOfficeId] [int]  NULL,
    [PreviouslyFraudIncidentFlag] [bit]  NULL,
    [EmpMisConductFlag] [bit]  NULL,
    [Status] [bit]  NULL,
    [AuditYear] [datetime] NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoReceiveDate] [datetime]  NULL,
    [InfoReceiveId] [int]  NULL,
    [InfoReceiveFlag] [bit]  NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKFraudIrrgularitiesPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKFraudIrrgularitiesPreferenceSettings", sqlText);


                    #endregion BKFraudIrrgularitiesPreferenceSettings


                    #region BKInternalControlWeakPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKInternalControlWeakPreferenceSettings](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [Code] [nvarchar](50) NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BKAuditOfficeId] [int]  NULL,
    [InternalControlFlag] [bit]  NULL,
    [ProperDocumentationFlag] [bit]  NULL,
    [ProperReportingFlag] [bit]  NULL,
    [Status] [bit]  NULL,
    [AuditYear] [datetime]  NULL,
    [InfoReceiveDate] [datetime]  NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoReceiveId] [nvarchar](50) NULL,
    [InfoReceiveFlag] [bit] NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKInternalControlWeakPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKInternalControlWeakPreferenceSettings", sqlText);


                    #endregion BKInternalControlWeakPreferenceSettings






                    #region BKReguCompliancesPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKReguCompliancesPreferenceSettings](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [Code]  [nvarchar](50) NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BKAuditOfficeId] [int]  NULL,
    [InternationTxnFlag] [bit]  NULL,
    [ForexFlag] [bit]  NULL,
    [HighProfileClintsFlag] [bit]  NULL,
    [CorporateChientsFlag] [bit]  NULL,
    [AmlFlag] [bit]  NULL,
    [Status] [bit]  NULL,
    [AuditYear] [datetime]  NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoReceiveDate] [datetime]  NULL,
    [InfoReceiveId][nvarchar](50) NULL,
    [InfoReceiveFlag] [bit]  NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKReguCompliancesPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";



                    context.Repositories.SettingsRepository.NewTableAdd("BKReguCompliancesPreferenceSettings", sqlText);


                    #endregion BKReguCompliancesPreferenceSettings


                    #region BKRiskAssessPerferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKRiskAssessPerferenceSettings](

	[Id] [int] NOT NULL,
    [BranchID] [int] NULL,
    [Code] [nvarchar](50) NULL,
    [BKAuditOfficeTypeId] [int]  NULL,
    [BKAuditOfficeId] [int]  NULL,
    [Amount] [decimal](18,2)  NULL,
    [RiskLocFlag] [bit]  NULL,
    [EntryDate] [datetime]  NULL,
    [Status] [bit]  NULL,
    [AuditYear] [int]  NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoReceiveDate] [datetime]  NULL,
    [InfoReceiveId] [nvarchar](50) NULL,
    [InfoReceiveFlag] [bit]  NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKRiskAssessPerferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKRiskAssessPerferenceSettings", sqlText);


                    #endregion BKRiskAssessPerferenceSettings



                    #region BKRiskAssessRegulationPreferenceCBSTemp

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[BKRiskAssessRegulationPreferenceCBSTemp](

	[Id] [int] NOT NULL,
    [BranchID] [int]  NULL,
    [ImportId] [int]  NULL,
    [AuditYear] [datetime] NULL,
    [AuditFiscalYear] [datetime] NULL,
    [RiskTxnAmount] [decimal](18,2)  NULL,
    [CompFinProductsFlg] [bit]  NULL,
    [GeoLocRiskFlg] [bit]  NULL,
    [InternationTxnFlg] [bit]  NULL,
    [ForexFlg] [bit]  NULL,
    [HighProfileClientsFlg] [bit]  NULL,
    [CorporateClientsFlg] [bit]  NULL,
    [AmlFlg] [bit]  NULL,
    [KycGuidelinesFlg] [bit]  NULL,
    [Status] [bit] NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_BKRiskAssessRegulationPreferenceCBSTemp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("BKRiskAssessRegulationPreferenceCBSTemp", sqlText);


                    #endregion BKRiskAssessRegulationPreferenceCBSTemp




                    #region CommonSelectionSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[CommonSelectionSettings](

	[Id] [int] NOT NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [HistoricalPerformFlg] [bit]  NULL,
    [HistoricalPerformFlgDesc] [nvarchar](MAX) NULL,
    [LastYearAuditFindingsFlg] [bit]  NULL,
    [LastYearAuditFindingsFlgDesc] [nvarchar](MAX)  NULL,
    [PreviousYearsExceptLastYearAuditFindingsFlg] [bit]  NULL,
    [PreviousYearsExceptLastYearAuditFindingsFlgDesc] [nvarchar](MAX)  NULL,
    [TechCyberRiskFlg] [bit]  NULL,
    [OfficeSizeFlg] [bit]  NULL,
    [OfficeSignificanceFlg] [bit]  NULL,
    [TechCyberRiskFlgDesc] [nvarchar](MAX)  NULL,
    [StaffTurnoverFlg] [bit]  NULL,
    [StaffTurnoverFlgDesc] [nvarchar](MAX)  NULL,
    [StaffTrainingGapsFlg] [bit]  NULL,
    [StaffTrainingGapsFlgDesc] [nvarchar](MAX) NULL,
    [StrategicInitiativeFlg] [bit]  NULL,
    [StrategicInitiativeFlgDesc] [nvarchar](MAX)  NULL,
    [OperationalCompFlg] [bit]  NULL,
    [OperationalCompFlgDesc] [nvarchar](MAX)  NULL,
    [Fields1Flg] [bit]  NULL,
    [Fields1FlgDesc] [nvarchar](MAX)  NULL,
    [Fields2Flg] [bit]  NULL,
    [Fields2FlgDesc] [nvarchar](MAX)  NULL,
    [Fields3Flg] [bit]  NULL,
    [Fields3FlgDesc] [nvarchar](MAX)  NULL,
    [Fields4Flg] [bit]  NULL,
    [Fields4FlgDesc] [nvarchar](MAX)  NULL,
    [Fields5Flg] [bit]  NULL,
    [Fields5FlgDesc] [nvarchar](MAX)  NULL,
    [Fields6Flg] [bit]  NULL,
    [Fields6Flgdesc] [nvarchar](MAX)  NULL,
    [Fields7Flg] [bit]  NULL,
    [Fields7FlgDesc] [nvarchar](MAX)  NULL,
    [Fields8Flg] [bit]  NULL,
    [Fields8FlgDesc] [nvarchar](MAX)  NULL,
    [Fields9Flg] [bit] NULL,
    [Fields9FlgDesc] [nvarchar](MAX)  NULL,
    [Fields10Flg] [bit]  NULL,
    [Fields10FlgDesc] [nvarchar](MAX)  NULL,
    [Fields11Flg] [bit]  NULL,
    [Fields11FlgDesc] [nvarchar](MAX) NULL,
    [Fields12Flg] [bit]  NULL,
    [Fields12flgDesc] [nvarchar](MAX)  NULL,
    [Fields13Flg] [bit]  NULL,
    [Fields13FlgDesc] [nvarchar](MAX)  NULL,
    [Fields14Flg] [bit]  NULL,
    [Fields14FlgDesc] [nvarchar](MAX)  NULL,
    [Fields15Flg] [bit]  NULL,
    [Fields15Flg_desc] [nvarchar](MAX)  NULL,
    [Fields16Flg] [bit]  NULL,
    [Fields16FlgDesc] [nvarchar](MAX)  NULL,
    [Fields17Flg] [bit]  NULL,
    [Felds17FlgDesc] [nvarchar](MAX)  NULL,
    [Fields18Flg] [bit]  NULL,
    [Fields18FlgDesc] [nvarchar](MAX)  NULL,
    [Fields19Flg] [bit]  NULL,
    [Fields19FlgDesc] [nvarchar](MAX)  NULL,
    [Fields20Flg] [bit]  NULL,
    [Fields20FlgDesc] [nvarchar](MAX)  NULL,
    [EntryDate] [datetime]  NULL,
    [UpdateDate] [datetime]  NULL,
    [Status] [bit] NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_CommonSelectionSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("CommonSelectionSettings", sqlText);


                    #endregion CommonSelectionSettings



                    #region CommonSelectionSettingsAuditPreferences

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[CommonSelectionSettingsAuditPreferences](

	[Id] [int] NOT NULL,
    [AuditYear] [int]  NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoRecvDate] [datetime]  NULL,
    [InfoRecvById] [int]  NULL,
    [InfoRecvFlg] [bit]  NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [HistoricalPerformFlg] [bit] NULL,
    [HistoricalPerformFlgDesc] [nvarchar](MAX) NULL,
    [LastYearAuditFindingsFlg] [bit] NULL,
    [LastYearAuditFindingsFlgDesc] [nvarchar](MAX)  NULL,
    [PreviousYearsExceptLastYearAuditFindingsFlg] [bit]  NULL,
    [PreviousYearsExceptLastYearAuditFindingsFlgDesc] [nvarchar](MAX)  NULL,
    [TechCyberRiskFlg] [bit]  NULL,
    [OfficeSizeFlg] [bit]  NULL,
    [OfficeSignificanceFlg] [bit] NULL,
    [TechCyberRiskFlgDesc] [nvarchar](MAX) NULL,
    [StaffTurnoverFlg] [bit] NULL,
    [StaffTurnoverFlgDesc] [nvarchar](MAX) NULL,
    [StaffTrainingGapsFlg] [bit] NULL,
    [StaffTrainingGapsFlgDesc] [nvarchar](MAX) NULL,
    [StrategicInitiativeFlg] [bit] NULL,
    [StrategicInitiativeFlgDesc] [nvarchar](MAX)  NULL,
    [OperationalCompFlg] [bit] NULL,
    [OperationalCompFlgDesc] [nvarchar](MAX) NULL,
    [Fields1Flg] [bit] NULL,
    [Fields1FlgDesc] [nvarchar](MAX)  NULL,
    [Fields2Flg] [bit]  NULL,
    [Fields2FlgDesc] [nvarchar](MAX)  NULL,
    [Fields3Flg] [bit] NULL,
    [Fields3FlgDesc] [nvarchar](MAX)  NULL,
    [Fields4Flg] [bit] NULL,
    [Fields4FlgDesc] [nvarchar](MAX)  NULL,
    [Fields5Flg] [bit]  NULL,
    [Fields5FlgDesc] [nvarchar](MAX)  NULL,
    [Fields6Flg] [bit]  NULL,
    [Fields6FlgDesc] [nvarchar](MAX)  NULL,
    [Fields7Flg] [bit]  NULL,
    [Fields7FlgDesc] [nvarchar](MAX)  NULL,
    [Fields8Flg] [bit]  NULL,
    [Fields8FlgDesc] [nvarchar](MAX)  NULL,
    [Fields9Flg] [bit] NULL,
    [Fields9FlgDesc] [nvarchar](MAX)  NULL,
    [Fields10Flg] [bit]  NULL,
    [Fields10FlgDesc] [nvarchar](MAX)  NULL,
    [Fields11Flg] [bit]  NULL,
    [Fields11FlgDesc] [nvarchar](MAX)  NULL,
    [Fields12Flg] [bit]  NULL,
    [Fields12FlgDesc] [nvarchar](MAX)  NULL,
    [Fields13Flg] [bit]  NULL,
    [Fields13FlgDesc] [nvarchar](MAX)  NULL,
    [Fields14Flg] [bit]  NULL,
    [Fields14FlgDesc] [nvarchar](MAX) NULL,
    [Fields15Flg] [bit]  NULL,
    [Fields15FlgDesc] [nvarchar](MAX) NULL,
    [Fields16Flg] [bit]  NULL,
    [Fields16FlgDesc] [nvarchar](MAX) NULL,
    [Fields17Flg] [bit] NULL,
    [Fields17FlgDesc] [nvarchar](MAX) NULL,
    [Fields18Flg] [bit]  NULL,
    [Fields18FlgDesc] [nvarchar](MAX) NULL,
    [Fields19Flg] [bit]  NULL,
    [Fields19FlgDesc] [nvarchar](MAX) NULL,
    [Fields20Flg] [bit] NULL,
    [Fields20FlgDesc] [nvarchar](MAX)  NULL,
    [RecStatus] [bit] NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_CommonSelectionSettingsAuditPreferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("CommonSelectionSettingsAuditPreferences", sqlText);


                    #endregion CommonSelectionSettingsAuditPreferences




                    #region FinancePreformPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[FinancePreformPreferenceSettings](

	[Id] [int] NOT NULL,
    [AauditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [FinancialPerformFlg] [bit]  NULL,
    [FundAvailableFlg] [bit]  NULL,
    [MisManagementClientsFlg] [bit]  NULL,
    [EfficiencyFlg] [bit]  NULL,
    [NplsLargeFlg] [bit]  NULL,
    [LargeTxnManageFlg] [bit]  NULL,
    [HighValueAssetManageFlg] [bit]  NULL,
    [SecurityMeasuresStaffFlg] [bit]  NULL,
    [BudgetMgtFlg] [bit]  NULL,
    [SignificantLossesFlg] [bit]  NULL,
    [EntryDate] [datetime]  NULL,
    [UpdateDate] [datetime]  NULL,
    [Status] [bit] NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_FinancePreformPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("FinancePreformPreferenceSettings", sqlText);


                    #endregion FinancePreformPreferenceSettings



                    #region FinancePreformPreferenceSettingsAuditPreferences

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[FinancePreformPreferenceSettingsAuditPreferences](

	[Id] [int] NOT NULL,
    [AuditYear] [int]  NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoRecvDate] [datetime]  NULL,
    [InfoRecvById] [int]  NULL,
    [InfoRecvFlg] [bit]  NULL,
    [AauditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [FinancialPerformFlg] [bit]  NULL,
    [FundAvailableFlg] [bit]  NULL,
    [MisManagementClientsFlg] [bit]  NULL,
    [EfficiencyFlg] [bit]  NULL,
    [NplsLargeFlg] [bit]  NULL,
    [LargeTxnManageFlg] [bit]  NULL,
    [HighValueAssetManageFlg] [bit]  NULL,
    [SecurityMeasuresStaffFlg] [bit]  NULL,
    [BudgetMgtFlg] [bit]  NULL,
    [SignificantLossesFalg] [bit]  NULL,
    [RecStatus] [bit]  NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_FinancePreformPreferenceSettingsAuditPreferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("FinancePreformPreferenceSettingsAuditPreferences", sqlText);


                    #endregion FinancePreformPreferenceSettingsAuditPreferences



                    #region FraudIrregularitiesPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[FraudIrregularitiesPreferenceSettings](

	[Id] [int] NOT NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [PreviouslyFraudIncidentFlg] [bit]  NULL,
    [EmpMisConductFlg] [bit]  NULL,
    [IregularitiesFlg] [bit]  NULL,
    [EntryDate] [datetime]  NULL,
    [UpdateDate] [datetime]  NULL,
    [Status] [bit]  NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_FraudIrregularitiesPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("FraudIrregularitiesPreferenceSettings", sqlText);


                    #endregion FraudIrregularitiesPreferenceSettings



                    #region FraudIrregularitiesPreferenceSettingsAuditPreferences

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[FraudIrregularitiesPreferenceSettingsAuditPreferences](

	[Id] [int] NOT NULL,
    [AuditYear] [int] NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoRecvDate] [datetime]  NULL,
    [InfoRecvById] [nvarchar](50) NULL,
    [InfoRecvFlg] [bit] NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int] NULL,
    [PreviouslyFraudIncidentFlg] [bit]  NULL,
    [EmpMisConductFlg] [bit]  NULL,
    [IregularitiesFlag] [bit]  NULL,
    [RecStatus] [bit] NULL,


    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_FraudIrregularitiesPreferenceSettingsAuditPreferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("FraudIrregularitiesPreferenceSettingsAuditPreferences", sqlText);


                    #endregion FraudIrregularitiesPreferenceSettingsAuditPreferences




                    #region InternalControlWeakPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[InternalControlWeakPreferenceSettings](

	[Id] [int] NOT NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [InternalControlFlag] [bit]  NULL,
    [ProperDocumentationFlag] [bit]  NULL,
    [ProperReportingFlag] [bit]  NULL,
    [EntryDate] [datetime]  NULL,
    [UpdateDate] [datetime]  NULL,
    [Status] [bit]  NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_InternalControlWeakPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("InternalControlWeakPreferenceSettings", sqlText);


                    #endregion InternalControlWeakPreferenceSettings



                    #region InternalControlWeakPreferenceSettingsAuditPreferences

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[InternalControlWeakPreferenceSettingsAuditPreferences](

	[Id] [int] NOT NULL,
    [AuditYear] [int] NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoRecvDate] [datetime]  NULL,
    [InfoRecvById] [nvarchar](50) NULL,
    [InfoRecvFlag] [bit]  NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [InternalControlFlag] [bit]  NULL,
    [ProperDocumentationFlag] [bit]  NULL,
    [ProperReportingFlag] [bit]  NULL,
    [RecStatus] [bit]  NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_InternalControlWeakPreferenceSettingsAuditPreferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("InternalControlWeakPreferenceSettingsAuditPreferences", sqlText);


                    #endregion InternalControlWeakPreferenceSettingsAuditPreferences



                    #region ReguCompliancesPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[ReguCompliancesPreferenceSettings](

	[Id] [int] NOT NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [InternationTxnFlg] [bit]  NULL,
    [ForexFlg] [bit]  NULL,
    [HighProfileClientsFlg] [bit]  NULL,
    [CorporateClientsFlg] [bit]  NULL,
    [AmlFlg] [bit]  NULL,
    [KycGuidelinesFlg] [bit]  NULL,
    [EntryDate] [datetime]  NULL,
    [UpdateDate] [datetime]  NULL,
    [Status] [bit]  NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_ReguCompliancesPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("ReguCompliancesPreferenceSettings", sqlText);


                    #endregion ReguCompliancesPreferenceSettings




                    #region ReguCompliancesPreferenceSettingsAuditPreferences

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[ReguCompliancesPreferenceSettingsAuditPreferences](

	[Id] [int] NOT NULL,
    [AuditYear] [int]  NULL,
    [AuditRiscalYear] [nvarchar](50) NULL,
    [InfoRecvDate] [datetime] NULL,
    [InfoRecvyId] [nvarchar](50) NULL,
    [InfoRecvFlg] [bit]  NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [InternationTxnFlg] [bit]  NULL,
    [ForexFlg] [bit]  NULL,
    [HighProfileClientsFlg] [bit]  NULL,
    [CorporateClientsFlg] [bit]  NULL,
    [AmlFlg] [bit]  NULL,
    [KycGuidelinesFlg] [bit]  NULL,
    [RecStatus] [bit]  NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_ReguCompliancesPreferenceSettingsAuditPreferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("ReguCompliancesPreferenceSettingsAuditPreferences", sqlText);


                    #endregion ReguCompliancesPreferenceSettingsAuditPreferences




                    #region RiskAssessPreferenceSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[RiskAssessPreferenceSettings](

	[Id] [int] NOT NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int] NULL,
    [RiskTxnAmount] [decimal](18,2)  NULL,
    [CompFinProductsFlg] [bit]  NULL,
    [GeoLocRiskFlg] [bit]  NULL,
    [EntryDate] [datetime]  NULL,
    [UpdateDate] [datetime]  NULL,
    [Status] [bit] NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_RiskAssessPreferenceSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("RiskAssessPreferenceSettings", sqlText);


                    #endregion RiskAssessPreferenceSettings




                    #region RiskAssessPreferenceSettingsAuditPreferences

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[RiskAssessPreferenceSettingsAuditPreferences](

	[Id] [int] NOT NULL,
    [AauditYear] [int]  NULL,
    [AuditFiscalYear] [nvarchar](50) NULL,
    [InfoRecvDate] [datetime]  NULL,
    [InfoRecvById] [int]  NULL,
    [InfoRecvFlg] [bit]  NULL,
    [AuditOfficeTypesId] [int]  NULL,
    [AuditOfficeId] [int]  NULL,
    [RiskTxnAmount] [decimal](18,2) NULL,
    [CompFinProductsFlg] [bit] NULL,
    [GeoLocRiskFlg] [bit] NULL,
    [RecStatus] [bit] NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,
    

 CONSTRAINT [PK_RiskAssessPreferenceSettingsAuditPreferences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("RiskAssessPreferenceSettingsAuditPreferences", sqlText);


                    #endregion RiskAssessPreferenceSettingsAuditPreferences



                    #region A_AuditAreaPoints

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[A_AuditAreaPoints](

	[Id] [int] NOT NULL,
    [AuditId] [int]  NULL,
    [AuditPointId] [int]  NULL,
    [WeightPersent] [int]  NULL,
    [P_Mark] [decimal](18, 2) NULL,

    
 CONSTRAINT [PK_A_AuditAreaPoints] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("A_AuditAreaPoints", sqlText);


                    #endregion A_AuditAreaPoints


                    #region FiscalYears

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[FiscalYears](

	[Id] [int] NOT NULL,
    [Year] [int]  NULL,
    [YearStart] [datetime]  NULL,
    [YearEnd] [datetime]  NULL,
    [YearLock] [bit] NULL,  
    [Remarks] [nvarchar](500) NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

    
 CONSTRAINT [PK_FiscalYears] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("FiscalYears", sqlText);


                    #endregion FiscalYears




                    #region FiscalYearDetails

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[FiscalYearDetails](

	[Id] [int] NOT NULL,
    [FiscalYearId] [int]  NULL,
    [Year] [int]  NULL,
    [MonthId] [int]  NULL,
    [WeightPersent] [int]  NULL,
    [MonthName] [nvarchar](50) NULL,
    [MonthStart] [nvarchar](20) NULL,
    [MonthEnd] [nvarchar](20) NULL,
    [MonthLock] [bit] NULL,
    [Remarks] [nvarchar](500) NULL,

    
 CONSTRAINT [PK_FiscalYearDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("FiscalYearDetails", sqlText);


                    #endregion FiscalYearDetails




                    #region MonthlyData

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[MonthlyData](

	[Id] [int] NOT NULL,
    [FiscalYearId] [int]  NULL,
    [Year] [int]  NULL,
    [MonthId] [int]  NULL,
    [MonthStart] [nvarchar](20) NULL,
    [MonthEnd] [nvarchar](20) NULL,
    [MonthName] [nvarchar](50) NULL,
    [MonthLock]  [bit] NULL,
    [Remarks] [nvarchar](500) NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

    
 CONSTRAINT [PK_MonthlyData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("MonthlyData", sqlText);




                    #endregion MonthlyData




                    #region PurchaseInvoices

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[PurchaseInvoices](

	[Id] [int] NOT NULL,
    [PurchaseInvoiceNo] [nvarchar](20) NULL,
    [BranchCode] [nvarchar](120) NULL,
    [BranchName] [nvarchar](120) NULL,
    [VendorCode] [nvarchar](50) NULL,
    [VendorName] [nvarchar](MAX) NULL,
    [InvoiceDateTime] [datetime] NULL,
    [BENumber] [nvarchar](200) NULL,
    [ProductCode] [nvarchar](50) NULL,
    [ProductName] [nvarchar](MAX) NULL,
    [CostPrice] [decimal](29, 9) NULL,
    [SDAmount] [decimal](29, 9) NULL,
    [VATAmount] [decimal](29, 9) NULL,
    

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

    
 CONSTRAINT [PK_PurchaseInvoices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("PurchaseInvoices", sqlText);


                    #endregion PurchaseInvoices



                    #region SalesInvoices

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[SalesInvoices](

	[Id] [int] NOT NULL,
    [SalesInvoiceNo] [nvarchar](20) NULL,
    [BranchCode] [nvarchar](120) NULL,
    [BranchName] [nvarchar](120) NULL,
    [CustomerCode] [nvarchar](50) NULL,
    [CustomerName] [nvarchar](MAX) NULL,
    [InvoiceDateTime] [datetime] NULL,
    [ProductCode] [nvarchar](50) NULL,
    [ProductName] [nvarchar](MAX) NULL,
    [NBRPrice] [decimal](29, 9) NULL,
    [VATAmount] [decimal](29, 9) NULL,
    [SubTotal] [decimal](29, 9) NULL,
    

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

    
 CONSTRAINT [PK_SalesInvoices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("SalesInvoices", sqlText);


                    #endregion SalesInvoices


                    #region YearlyData

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[YearlyData](

	[Id] [int] NOT NULL,
	[Year] [int]  NULL,
    [YearStart] [datetime] NULL,
    [YearEnd] [datetime] NULL,
    [YearLock] [bit] NULL,
    [Remarks] [nvarchar](MAX) NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

    
 CONSTRAINT [PK_YearlyData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("YearlyData", sqlText);


                    #endregion YearlyData


                    #region DashboardSettings

                    sqlText = " ";
                    sqlText = @"

    CREATE TABLE [dbo].[DashboardSettings](

	[Id] [int] NOT NULL,
	[UserId] [varchar](450) NULL,
    [IsPieChart] [bit] NULL,
    [IsFinancialData] [bit] NULL,
    [IsPlannedEngagement] [bit] NULL,
    [IsUnplannedEngagement] [bit] NULL,
    [IsYearData] [bit] NULL,
    [IsBoxData] [bit] NULL,

    [CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedFrom] [nvarchar](50) NULL,
	[LastUpdateBy] [nvarchar](50) NULL,
	[LastUpdateOn] [datetime] NULL,
	[LastUpdateFrom] [nvarchar](50) NULL,

    
 CONSTRAINT [PK_DashboardSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

";


                    context.Repositories.SettingsRepository.NewTableAdd("DashboardSettings", sqlText);


                    #endregion DashboardSettings


                    #endregion Table




                    #region  AddField

                    context.Repositories.SettingsRepository.DBTableFieldAdd("Notification", "UserId", "varchar(MAX)", true);

                    context.Repositories.SettingsRepository.DBTableFieldAdd("Tours", "EmpName", "nvarchar(50)", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("Tours", "EmployeeId", "nvarchar(50)", true);

                    context.Repositories.SettingsRepository.DBTableFieldAdd("CompanyInfo", "IsAdCheck", "nchar(1)", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("CompanyInfo", "AdUrl", "varchar(50)", true);

                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_Audits", "AuditTemplateId", "int", true);

                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditIssues", "CheckListItemId", "int", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditIssues", "RiskType", "nvarchar(500)", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditIssues", "AuditAreaId", "int", true);

                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditAreas", "AuditTypeId", "int", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditAreas", "AuditPointId", "int", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditAreas", "PID", "int", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditAreas", "AuditPointName", "nvarchar(500)", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditAreas", "WeightPersent", "int", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditAreas", "P_Mark", "decimal(18, 2)", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("A_AuditAreas", "P_Level", "int", true);

                    context.Repositories.SettingsRepository.DBTableFieldAdd("TestHeader", "Amount", "decimal(18, 2)", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("TestDetails", "TestAmount", "int", true);
                    context.Repositories.SettingsRepository.DBTableFieldAdd("TestDetails", "RawQuantity", "decimal(18, 2)", true);

                    #endregion





                    #region FieldAlter/Update

                    context.Repositories.SettingsRepository.DBTableFieldAlter("TestDetails", "BankCode", "nvarchar(400)");
                    context.Repositories.SettingsRepository.DBTableFieldAlter("TestDetails", "BankCode", "nvarchar(500)");
                    context.Repositories.SettingsRepository.DBTableFieldAlter("TestDetails", "BankCode", "nvarchar(1000)");

                    #endregion


                    context.SaveChanges();

                    return new ResultModel<DbUpdateModel>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DbUpdateSuccess,
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<DbUpdateModel>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DbUpdateFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<SettingsModel> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<SettingsModel>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.SettingsRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<SettingsModel>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<SettingsModel>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<SettingsModel>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<string> GetSettingsValue(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    var record = context.Repositories.SettingsRepository.GetSettingsValue(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<string>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = record
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<string>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<SettingsModel> Insert(SettingsModel model)
        {

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<SettingsModel>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    string[] conditionField = { "SettingGroup", "SettingName" };
                    string[] conditionValue = { model.SettingGroup.Trim(), model.SettingName.Trim() };

                    bool exist = true;// context.Repositories.IPOReceiptsMasterRepository.CheckExists("Settings", conditionField, conditionValue);


                    if (!exist)
                    {

                        SettingsModel master = context.Repositories.SettingsRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<SettingsModel>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }


                        context.SaveChanges();

                        return new ResultModel<SettingsModel>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };


                    }
                    else
                    {
                        return new ResultModel<SettingsModel>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<SettingsModel>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

        }

        public ResultModel<SettingsModel> Update(SettingsModel model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {



                    SettingsModel master = context.Repositories.SettingsRepository.Update(model);

                    if (master.Id == 0)
                    {
                        return new ResultModel<SettingsModel>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DetailInsertFailed,
                            Data = master
                        };
                    }

                    context.SaveChanges();


                    return new ResultModel<SettingsModel>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<SettingsModel>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }


    }
}


