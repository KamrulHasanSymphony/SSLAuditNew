using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.BKCommonSelectionSetting;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKCommonSelectionSetting
{
	public class BKCommonSelectionSettingRepository : Repository, IBKCommonSelectionSettingRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKCommonSelectionSettingRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

        {
            this._context = context;
			this._transaction = transaction;
		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public bool CheckExists(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public bool CheckPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
            try
            {
                bool ÌsPost = false;
                string Post = "";

                DataTable dt = new DataTable();

                // ToDo sql injection
                string sqlText = "select IsPost  from " + tableName + " where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Post = dt.Rows[0]["IsPost"].ToString();
                    return (Post == "Y");
                }


                return ÌsPost;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public bool CheckPushStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public string CodeGeneration(string CodeGroup, string CodeName)
		{
			try
			{

				string codeGeneration = GenerateCode(CodeGroup, CodeName);
				return codeGeneration;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public int Delete(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
            throw new NotImplementedException();

        }

        public List<BKCommonSelectionSettings> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
    string sqlText = @"select 

    Id,
    BranchID,
    Code,
    BKAuditOfficeTypeId,
    BKAuditOfficeId,
    ISNULL(HitoricalPreformFlag, 0) AS HitoricalPreformFlag,
    ISNULL(HistoricalPerformFlagDesc, '') AS HistoricalPerformFlagDesc,
    ISNULL(LastYearAuditFindingFlag, 0) AS LastYearAuditFindingFlag,
    ISNULL(LastYearAuditFindingFlagDesc, '') AS LastYearAuditFindingFlagDesc,
    ISNULL(PreviousYearExceptLastYearAuditFindingFlag, 0) AS PreviousYearExceptLastYearAuditFindingFlag,
    ISNULL(PreviousYearExceptLastYearAuditFindingFlagDesc, '') AS PreviousYearExceptLastYearAuditFindingFlagDesc,
    ISNULL(TechCyberRiskFlag, 0) AS TechCyberRiskFlag,
    ISNULL(OfficeSizeFlag, 0) AS OfficeSizeFlag,
    ISNULL(OfficeSignificanceFlag, 0) AS OfficeSignificanceFlag,
    ISNULL(TechCyberRiskFlagDesc, '') AS TechCyberRiskFlagDesc,
    ISNULL(StaffTurnoverFlag, 0) AS StaffTurnoverFlag,
    ISNULL(StaffTurnoverFlagDesc, '') AS StaffTurnoverFlagDesc,
    ISNULL(StaffTrainingGapsFlag, 0) AS StaffTrainingGapsFlag,
    ISNULL(StaffTrainingGapsFlagDesc, '') AS StaffTrainingGapsFlagDesc,
    ISNULL(StrategicInitiativeFlagveFlag, 0) AS StrategicInitiativeFlagveFlag,
    ISNULL(StrategicInitiativeFlagDesc, '') AS StrategicInitiativeFlagDesc,
    ISNULL(OperationalCompFlag, 0) AS OperationalCompFlag,
    ISNULL(OperationalCompFlagDesc, '') AS OperationalCompFlagDesc,
    ISNULL(Status, 0) AS Status,
    ISNULL(EntryDate, GETDATE()) AS EntryDate, 
    ISNULL(FORMAT(AuditYear, 'yyyy-MM-dd'), '199-01-01') AS AuditYear

    --ISNULL(FORMAT(AuditFiscalYear, 'yyyy-MM-dd'), '1999-01-01') AS AuditFiscalYear,
    --ISNULL(FORMAT(InfoReceiveDate, 'yyyy-MM-dd'), '1999-01-01') AS InfoReceiveDate,
    --ISNULL(InfoReceiveId, '') AS InfoReceiveId,
    --ISNULL(InfoReceiveFlag, 0) AS InfoReceiveFlag


    FROM BKCommonSelectionSettings 

    where 1=1 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKCommonSelectionSettings> vms = dtResult.ToList<BKCommonSelectionSettings>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
		}

		public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue)
		{
            string sqlText = "";
            List<BKCommonSelectionSettings> VMs = new List<BKCommonSelectionSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKCommonSelectionSettings.Id)FilteredCount
                from BKCommonSelectionSettings  where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);

            }
            catch (Exception e)
            {
                throw e;
            }

        }


        public List<BKCommonSelectionSettings> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKCommonSelectionSettings> VMs = new List<BKCommonSelectionSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 

    Id,
    Code,
    BKAuditOfficeTypeId,
    BKAuditOfficeId,
    ISNULL(HitoricalPreformFlag, 0) AS HitoricalPreformFlag,
    ISNULL(HistoricalPerformFlagDesc, '') AS HistoricalPerformFlagDesc,
    ISNULL(LastYearAuditFindingFlag, 0) AS LastYearAuditFindingFlag,
    ISNULL(LastYearAuditFindingFlagDesc, '') AS LastYearAuditFindingFlagDesc,
    ISNULL(PreviousYearExceptLastYearAuditFindingFlag, 0) AS PreviousYearExceptLastYearAuditFindingFlag,
    ISNULL(PreviousYearExceptLastYearAuditFindingFlagDesc, '') AS PreviousYearExceptLastYearAuditFindingFlagDesc,
    ISNULL(TechCyberRiskFlag, 0) AS TechCyberRiskFlag,
    ISNULL(OfficeSizeFlag, 0) AS OfficeSizeFlag,
    ISNULL(OfficeSignificanceFlag, 0) AS OfficeSignificanceFlag,
    ISNULL(TechCyberRiskFlagDesc, '') AS TechCyberRiskFlagDesc,
    ISNULL(StaffTurnoverFlag, 0) AS StaffTurnoverFlag,
    ISNULL(StaffTurnoverFlagDesc, '') AS StaffTurnoverFlagDesc,
    ISNULL(StaffTrainingGapsFlag, 0) AS StaffTrainingGapsFlag,
    ISNULL(StaffTrainingGapsFlagDesc, '') AS StaffTrainingGapsFlagDesc,
    ISNULL(StrategicInitiativeFlagveFlag, 0) AS StrategicInitiativeFlagveFlag,
    ISNULL(StrategicInitiativeFlagDesc, '') AS StrategicInitiativeFlagDesc,
    ISNULL(OperationalCompFlag, 0) AS OperationalCompFlag,
    ISNULL(OperationalCompFlagDesc, '') AS OperationalCompFlagDesc,
    ISNULL(Status, 0) AS Status,
    ISNULL(EntryDate, GETDATE()) AS EntryDate

    
    FROM BKCommonSelectionSettings 
    where 1=1 

";



                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new BKCommonSelectionSettings();

                VMs.Add(req);


                VMs = dt.ToList<BKCommonSelectionSettings>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKCommonSelectionSettings> VMs = new List<BKCommonSelectionSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKCommonSelectionSettings  where 1=1 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);


                return Convert.ToInt32(dt.Rows[0][0]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

		public string GetSettingsValue(string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public BKCommonSelectionSettings Insert(BKCommonSelectionSettings model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKCommonSelectionSettings(


    Code,
    BranchID,
    BKAuditOfficeTypeId,
    BKAuditOfficeId,
    HitoricalPreformFlag,
    HistoricalPerformFlagDesc,
    LastYearAuditFindingFlag,
    LastYearAuditFindingFlagDesc,
    PreviousYearExceptLastYearAuditFindingFlag,
    PreviousYearExceptLastYearAuditFindingFlagDesc,
    TechCyberRiskFlag,
    OfficeSizeFlag,
    OfficeSignificanceFlag,
    TechCyberRiskFlagDesc,
    StaffTurnoverFlag,
    StaffTurnoverFlagDesc,
    StaffTrainingGapsFlag,
    StaffTrainingGapsFlagDesc,
    StrategicInitiativeFlagveFlag,
    StrategicInitiativeFlagDesc,
    OperationalCompFlag,
    OperationalCompFlagDesc,
    Status,
    EntryDate,
    AuditYear,

    --AuditFiscalYear,
    --InfoReceiveDate,
    --InfoReceiveId,
    --InfoReceiveFlag,

    CreatedBy,
    CreatedOn,
    CreatedFrom

) VALUES (


    @Code,
    @BranchID,
    @BKAuditOfficeTypeId,
    @BKAuditOfficeId,
    @HitoricalPreformFlag,
    @HistoricalPerformFlagDesc,
    @LastYearAuditFindingFlag,
    @LastYearAuditFindingFlagDesc,
    @PreviousYearExceptLastYearAuditFindingFlag,
    @PreviousYearExceptLastYearAuditFindingFlagDesc,
    @TechCyberRiskFlag,
    @OfficeSizeFlag,
    @OfficeSignificanceFlag,
    @TechCyberRiskFlagDesc,
    @StaffTurnoverFlag,
    @StaffTurnoverFlagDesc,
    @StaffTrainingGapsFlag,
    @StaffTrainingGapsFlagDesc,
    @StrategicInitiativeFlagveFlag,
    @StrategicInitiativeFlagDesc,
    @OperationalCompFlag,
    @OperationalCompFlagDesc,
    @Status,
    @EntryDate,
    @AuditYear,

    --@AuditFiscalYear,
    --@InfoReceiveDate,
    --@InfoReceiveId,
    --@InfoReceiveFlag,

    @CreatedBy,
    @CreatedOn,
    @CreatedFrom


)SELECT SCOPE_IDENTITY()");


				command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = model.Code;			
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;			
				command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;
				command.Parameters.Add("@BKAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;
				command.Parameters.Add("@HitoricalPreformFlag", SqlDbType.Bit).Value = model.HitoricalPreformFlag;
				command.Parameters.Add("@HistoricalPerformFlagDesc", SqlDbType.NVarChar).Value = model.HistoricalPerformFlagDesc;
				command.Parameters.Add("@LastYearAuditFindingFlag", SqlDbType.Bit).Value = model.LastYearAuditFindingFlag;
				command.Parameters.Add("@LastYearAuditFindingFlagDesc", SqlDbType.NVarChar).Value = model.LastYearAuditFindingFlagDesc;
				command.Parameters.Add("@PreviousYearExceptLastYearAuditFindingFlag", SqlDbType.Bit).Value = model.PreviousYearExceptLastYearAuditFindingFlag;
				command.Parameters.Add("@PreviousYearExceptLastYearAuditFindingFlagDesc", SqlDbType.NVarChar).Value = model.PreviousYearExceptLastYearAuditFindingFlagDesc;
				command.Parameters.Add("@TechCyberRiskFlag", SqlDbType.Bit).Value = model.TechCyberRiskFlag;
				command.Parameters.Add("@OfficeSizeFlag", SqlDbType.Bit).Value = model.OfficeSizeFlag;
				command.Parameters.Add("@OfficeSignificanceFlag", SqlDbType.Bit).Value = model.OfficeSignificanceFlag;
				command.Parameters.Add("@TechCyberRiskFlagDesc", SqlDbType.NVarChar).Value = model.TechCyberRiskFlagDesc;
				command.Parameters.Add("@StaffTurnoverFlag", SqlDbType.Bit).Value = model.StaffTurnoverFlag;
				command.Parameters.Add("@StaffTurnoverFlagDesc", SqlDbType.NVarChar).Value = model.StaffTurnoverFlagDesc;
				command.Parameters.Add("@StaffTrainingGapsFlag", SqlDbType.Bit).Value = model.StaffTrainingGapsFlag;
				command.Parameters.Add("@StaffTrainingGapsFlagDesc", SqlDbType.NVarChar).Value = model.StaffTrainingGapsFlagDesc;
				command.Parameters.Add("@StrategicInitiativeFlagveFlag", SqlDbType.Bit).Value = model.StrategicInitiativeFlagveFlag;
				command.Parameters.Add("@StrategicInitiativeFlagDesc", SqlDbType.NVarChar).Value = model.StrategicInitiativeFlagDesc;
				command.Parameters.Add("@OperationalCompFlag", SqlDbType.Bit).Value = model.OperationalCompFlag;
				command.Parameters.Add("@OperationalCompFlagDesc", SqlDbType.NVarChar).Value = model.OperationalCompFlagDesc;
				command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;
				command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = model.EntryDate;             
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;

                //command.Parameters.Add("@AuditFiscalYear", SqlDbType.DateTime).Value = model.AuditFiscalYear;
                //command.Parameters.Add("@InfoReceiveDate", SqlDbType.DateTime).Value = model.InfoReceiveDate;
                //command.Parameters.Add("@InfoReceiveId", SqlDbType.NVarChar).Value = model.InfoReceiveId;
                //command.Parameters.Add("@InfoReceiveFlag", SqlDbType.Bit).Value = model.InfoReceiveFlag;



               
                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
				command.Parameters.Add("@CreatedOn", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
				command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();

                model.Id = Convert.ToInt32(command.ExecuteScalar());

				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public BKCommonSelectionSettings MultiplePost(BKCommonSelectionSettings objBKCommonSelectionSettings)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKCommonSelectionSettings set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKCommonSelectionSettings;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKCommonSelectionSettings MultipleUnPost(BKCommonSelectionSettings vm)
		{

            return new BKCommonSelectionSettings();
		
		}

		public BKCommonSelectionSettings Update(BKCommonSelectionSettings model)
		{
			try
			{
				string sqlText = "";
				int count = 0;


				string query = @"  update BKCommonSelectionSettings set
 
 BKAuditOfficeTypeId                               =@BKAuditOfficeTypeId  
,BKAuditOfficeId                                   =@BKAuditOfficeId  
,HitoricalPreformFlag                              =@HitoricalPreformFlag  
,HistoricalPerformFlagDesc                         =@HistoricalPerformFlagDesc  
,LastYearAuditFindingFlag                          =@LastYearAuditFindingFlag  
,LastYearAuditFindingFlagDesc                      =@LastYearAuditFindingFlagDesc  
,PreviousYearExceptLastYearAuditFindingFlag        =@PreviousYearExceptLastYearAuditFindingFlag  
,PreviousYearExceptLastYearAuditFindingFlagDesc    =@PreviousYearExceptLastYearAuditFindingFlagDesc  
,TechCyberRiskFlag                                 =@TechCyberRiskFlag  
,OfficeSizeFlag                                    =@OfficeSizeFlag  
,OfficeSignificanceFlag                            =@OfficeSignificanceFlag  
,TechCyberRiskFlagDesc                             =@TechCyberRiskFlagDesc  
,StaffTurnoverFlag                                 =@StaffTurnoverFlag  
,StaffTurnoverFlagDesc                             =@StaffTurnoverFlagDesc  
,StaffTrainingGapsFlag                             =@StaffTrainingGapsFlag  
,StaffTrainingGapsFlagDesc                         =@StaffTrainingGapsFlagDesc  
,StrategicInitiativeFlagveFlag                     =@StrategicInitiativeFlagveFlag  
,StrategicInitiativeFlagDesc                       =@StrategicInitiativeFlagDesc  
,OperationalCompFlag                               =@OperationalCompFlag  
,OperationalCompFlagDesc                           =@OperationalCompFlagDesc  
,Status                                            =@Status  
,EntryDate                                         =@EntryDate  
,AuditYear                                         =@AuditYear 


--,AuditFiscalYear                                   =@AuditFiscalYear  
--,InfoReceiveDate                                   =@InfoReceiveDate  
--,InfoReceiveId                                     =@InfoReceiveId  
--,InfoReceiveFlag                                   =@InfoReceiveFlag  

,LastUpdateBy                       =@LastUpdateBy  
,LastUpdateOn                       =@LastUpdateOn  
,LastUpdateFrom                     =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;
                command.Parameters.Add("@BKAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;
                command.Parameters.Add("@HitoricalPreformFlag", SqlDbType.Bit).Value = model.HitoricalPreformFlag;
                command.Parameters.Add("@HistoricalPerformFlagDesc", SqlDbType.NVarChar).Value = model.HistoricalPerformFlagDesc;
                command.Parameters.Add("@LastYearAuditFindingFlag", SqlDbType.Bit).Value = model.LastYearAuditFindingFlag;
                command.Parameters.Add("@LastYearAuditFindingFlagDesc", SqlDbType.NVarChar).Value = model.LastYearAuditFindingFlagDesc;
                command.Parameters.Add("@PreviousYearExceptLastYearAuditFindingFlag", SqlDbType.Bit).Value = model.PreviousYearExceptLastYearAuditFindingFlag;
                command.Parameters.Add("@PreviousYearExceptLastYearAuditFindingFlagDesc", SqlDbType.NVarChar).Value = model.PreviousYearExceptLastYearAuditFindingFlagDesc;
                command.Parameters.Add("@TechCyberRiskFlag", SqlDbType.Bit).Value = model.TechCyberRiskFlag;
                command.Parameters.Add("@OfficeSizeFlag", SqlDbType.Bit).Value = model.OfficeSizeFlag;
                command.Parameters.Add("@OfficeSignificanceFlag", SqlDbType.Bit).Value = model.OfficeSignificanceFlag;
                command.Parameters.Add("@TechCyberRiskFlagDesc", SqlDbType.NVarChar).Value = model.TechCyberRiskFlagDesc;
                command.Parameters.Add("@StaffTurnoverFlag", SqlDbType.Bit).Value = model.StaffTurnoverFlag;
                command.Parameters.Add("@StaffTurnoverFlagDesc", SqlDbType.NVarChar).Value = model.StaffTurnoverFlagDesc;
                command.Parameters.Add("@StaffTrainingGapsFlag", SqlDbType.Bit).Value = model.StaffTrainingGapsFlag;
                command.Parameters.Add("@StaffTrainingGapsFlagDesc", SqlDbType.NVarChar).Value = model.StaffTrainingGapsFlagDesc;
                command.Parameters.Add("@StrategicInitiativeFlagveFlag", SqlDbType.Bit).Value = model.StrategicInitiativeFlagveFlag;
                command.Parameters.Add("@StrategicInitiativeFlagDesc", SqlDbType.NVarChar).Value = model.StrategicInitiativeFlagDesc;
                command.Parameters.Add("@OperationalCompFlag", SqlDbType.Bit).Value = model.OperationalCompFlag;
                command.Parameters.Add("@OperationalCompFlagDesc", SqlDbType.NVarChar).Value = model.OperationalCompFlagDesc;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;
                command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = model.EntryDate;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;


                //command.Parameters.Add("@AuditFiscalYear", SqlDbType.DateTime).Value = model.AuditFiscalYear;
                //command.Parameters.Add("@InfoReceiveDate", SqlDbType.DateTime).Value = model.InfoReceiveDate;
                //command.Parameters.Add("@InfoReceiveId", SqlDbType.NVarChar).Value = model.InfoReceiveId;
                //command.Parameters.Add("@InfoReceiveFlag", SqlDbType.Bit).Value = model.InfoReceiveFlag;


                command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
				command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();
				command.Parameters.Add("@LastUpdateFrom ", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateFrom.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateFrom.ToString();

				int rowcount = command.ExecuteNonQuery();

				if (rowcount <= 0)
				{
					throw new Exception(MessageModel.UpdateFail);
				}

				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
