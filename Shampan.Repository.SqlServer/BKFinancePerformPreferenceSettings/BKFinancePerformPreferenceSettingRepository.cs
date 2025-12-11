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
using Shampan.Core.Interfaces.Repository.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKFinancePerformPreferenceSetting
{
	public class BKFinancePerformPreferenceSettingRepository : Repository, IBKFinancePerformPreferenceSettingRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKFinancePerformPreferenceSettingRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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
            try
            {
                // ToDo sql injection
                string sqlText = " delete   " + tableName + "  where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                int totalRecords = Convert.ToInt32(command.ExecuteNonQuery());

                return totalRecords;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public BKFinancePreformPreferenceCBS FinancePreformPreferenceCBSTempInsert(BKFinancePreformPreferenceCBS model)
        {
            try
            {
                string sqlText = "";

                var command = CreateCommand(@" INSERT INTO BKFinancePreformPreferenceCBSTemp(



 BranchID
,ImportId
,AuditYear
,AuditFiscalYear
,FinancialPerformFlg
,FundAvailableFlg
,MisManagementClientsFlg
,EfficiencyFlg
,NplsLargeFlg
,LargeTxnManageFlg
,HighValueAssetManageFlg
,SecurityMeasuresStaffFlg
,BudgetMgtFlg
,SignificantLossesFlg
,Status


) VALUES (


 @BranchID
,@ImportId
,@AuditYear
,@AuditFiscalYear
,@FinancialPerformFlg
,@FundAvailableFlg
,@MisManagementClientsFlg
,@EfficiencyFlg
,@NplsLargeFlg
,@LargeTxnManageFlg
,@HighValueAssetManageFlg
,@SecurityMeasuresStaffFlg
,@BudgetMgtFlg
,@SignificantLossesFlg
,@Status


)SELECT SCOPE_IDENTITY()");




                //command.Parameters.Add("@id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@ImportId", SqlDbType.Int).Value = model.ImportId;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.NVarChar).Value = model.AuditFiscalYear;
                command.Parameters.Add("@FinancialPerformFlg", SqlDbType.Bit).Value = model.FinancialPerformFlg;
                command.Parameters.Add("@FundAvailableFlg", SqlDbType.Bit).Value = model.FundAvailableFlg;
                command.Parameters.Add("@MisManagementClientsFlg", SqlDbType.Bit).Value = model.MisManagementClientsFlg;
                command.Parameters.Add("@EfficiencyFlg", SqlDbType.Bit).Value = model.EfficiencyFlg;
                command.Parameters.Add("@NplsLargeFlg", SqlDbType.Bit).Value = model.NplsLargeFlg;
                command.Parameters.Add("@LargeTxnManageFlg", SqlDbType.Bit).Value = model.LargeTxnManageFlg;
                command.Parameters.Add("@HighValueAssetManageFlg", SqlDbType.Bit).Value = model.HighValueAssetManageFlg;
                command.Parameters.Add("@SecurityMeasuresStaffFlg", SqlDbType.Bit).Value = model.SecurityMeasuresStaffFlg;
                command.Parameters.Add("@BudgetMgtFlg", SqlDbType.Bit).Value = model.BudgetMgtFlg;
                command.Parameters.Add("@SignificantLossesFlg", SqlDbType.Bit).Value = model.SignificantLossesFlg;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BKFinancePerformPreferenceSettings> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,BranchID
,Code
,BKAuditOfficeTypeId
,BkAuditOfficeId
,FundAvailableFlag
,MisManagementClinentsFlag
,EfficienctyFlag
,NPLSLargeFlag
,LargeTxnManageFlag
,HighValueAssetMangeFlag
,BudgetMgtFlag
,Status
,ISNULL(FORMAT(AuditYear, 'yyyy-MM-dd'), '199-01-01') AS AuditYear

--,ISNULL(FORMAT(AuditFiscalYear, 'yyyy-MM-dd'), '1999-01-01') AS AuditFiscalYear
--,ISNULL(FORMAT(InfoReceiveDate, 'yyyy-MM-dd'), '1999-01-01') AS InfoReceiveDate
--,ISNULL(InfoReceiveId, '') AS InfoReceiveId
--,ISNULL(InfoReceiveFlag, 0) AS InfoReceiveFlag

,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom


FROM  BKFinancePerformPreferenceSettings

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKFinancePerformPreferenceSettings> vms = dtResult.ToList<BKFinancePerformPreferenceSettings>();
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
            List<BKFinancePerformPreferenceSettings> VMs = new List<BKFinancePerformPreferenceSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKFinancePerformPreferenceSettings.Id)FilteredCount
                from BKFinancePerformPreferenceSettings  where 1=1 ";

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


        public List<BKFinancePerformPreferenceSettings> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKFinancePerformPreferenceSettings> VMs = new List<BKFinancePerformPreferenceSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
Code,
FundAvailableFlag,
MisManagementClinentsFlag,
EfficienctyFlag,
Status

FROM BKFinancePerformPreferenceSettings 
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
                var req = new BKFinancePerformPreferenceSettings();

                VMs.Add(req);


                VMs = dt.ToList<BKFinancePerformPreferenceSettings>();

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
            List<BKFinancePerformPreferenceSettings> VMs = new List<BKFinancePerformPreferenceSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKFinancePerformPreferenceSettings  where 1=1 ";


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

        public List<BKFinancePerformPreferenceSettings> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<BKFinancePerformPreferenceSettings> VMs = new List<BKFinancePerformPreferenceSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
 Id
,BranchID
,ImportId
,AuditYear
,AuditFiscalYear
,FinancialPerformFlg
,FundAvailableFlg
,MisManagementClientsFlg
,EfficiencyFlg
,NplsLargeFlg
,LargeTxnManageFlg
,HighValueAssetManageFlg
,SecurityMeasuresStaffFlg
,BudgetMgtFlg
,SignificantLossesFlg
,Status

FROM BKFinancePreformPreferenceCBSTemp 
where 1=1 

";


                if (index.IDs != null && index.IDs.Count() > 0)
                {
                    string ids = string.Join(",", index.IDs);
                    sqlText += $" AND ImportId IN ({ids}) ";
                }
                else
                {
                    //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                    sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                    // ToDo Escape Sql Injection
                    sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                    sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                }

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new BKFinancePerformPreferenceSettings();

                VMs.Add(req);


                VMs = dt.ToList<BKFinancePerformPreferenceSettings>();

                return VMs;


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

		public BKFinancePerformPreferenceSettings Insert(BKFinancePerformPreferenceSettings model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKFinancePerformPreferenceSettings(


 Code
,BranchID
,BKAuditOfficeTypeId
,BKAuditOfficeId
,FundAvailableFlag
,MisManagementClinentsFlag
,EfficienctyFlag
,NPLSLargeFlag
,LargeTxnManageFlag
,HighValueAssetMangeFlag
,BudgetMgtFlag
,Status
,AuditYear
,InfoReceiveId

,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@BranchID
,@BKAuditOfficeTypeId
,@BKAuditOfficeId
,@FundAvailableFlag
,@MisManagementClinentsFlag
,@EfficienctyFlag
,@NPLSLargeFlag
,@LargeTxnManageFlag
,@HighValueAssetMangeFlag
,@BudgetMgtFlag
,@Status
,@AuditYear
,@InfoReceiveId



,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");



				command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = model.Code;			
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;			
				command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;
				command.Parameters.Add("@BKAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;
				command.Parameters.Add("@FundAvailableFlag", SqlDbType.Bit).Value = model.FundAvailableFlag;
				command.Parameters.Add("@MisManagementClinentsFlag", SqlDbType.Bit).Value = model.MisManagementClinentsFlag;
				command.Parameters.Add("@EfficienctyFlag", SqlDbType.Bit).Value = model.EfficienctyFlag;
				command.Parameters.Add("@NPLSLargeFlag", SqlDbType.Bit).Value = model.NPLSLargeFlag;
				command.Parameters.Add("@LargeTxnManageFlag", SqlDbType.Bit).Value = model.LargeTxnManageFlag;
				command.Parameters.Add("@HighValueAssetMangeFlag", SqlDbType.Bit).Value = model.HighValueAssetMangeFlag;
				command.Parameters.Add("@BudgetMgtFlag", SqlDbType.Bit).Value = model.BudgetMgtFlag;
				command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@InfoReceiveId", SqlDbType.NVarChar).Value = model.InfoReceiveId;

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

        public BKFinancePerformPreferenceSettings MultiplePost(BKFinancePerformPreferenceSettings objBKFinancePerformPreferenceSettings)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKFinancePerformPreferenceSettingss set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKFinancePerformPreferenceSettings;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKFinancePerformPreferenceSettings MultipleUnPost(BKFinancePerformPreferenceSettings vm)
		{

            return new BKFinancePerformPreferenceSettings();
		
		}

		public BKFinancePerformPreferenceSettings Update(BKFinancePerformPreferenceSettings model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKFinancePerformPreferenceSettings set
 
 BKAuditOfficeTypeId               =@BKAuditOfficeTypeId  
,BkAuditOfficeId                   =@BkAuditOfficeId  
,FundAvailableFlag                 =@FundAvailableFlag  
,MisManagementClinentsFlag         =@MisManagementClinentsFlag  
,EfficienctyFlag                   =@EfficienctyFlag  
,NPLSLargeFlag                     =@NPLSLargeFlag  
,LargeTxnManageFlag                =@LargeTxnManageFlag  
,HighValueAssetMangeFlag           =@HighValueAssetMangeFlag  
,BudgetMgtFlag                     =@BudgetMgtFlag  
,Status                            =@Status  
,AuditYear                                        =@AuditYear  

--,AuditFiscalYear                                  =@AuditFiscalYear  
--,InfoReceiveDate                                  =@InfoReceiveDate  
--,InfoReceiveId                                    =@InfoReceiveId  
--,InfoReceiveFlag                                  =@InfoReceiveFlag  

,LastUpdateBy                 =@LastUpdateBy  
,LastUpdateOn                 =@LastUpdateOn  
,LastUpdateFrom               =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;	
				command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;		
				command.Parameters.Add("@BkAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;		
				command.Parameters.Add("@FundAvailableFlag", SqlDbType.Bit).Value = model.FundAvailableFlag;		
				command.Parameters.Add("@MisManagementClinentsFlag", SqlDbType.Bit).Value = model.MisManagementClinentsFlag;		
				command.Parameters.Add("@EfficienctyFlag", SqlDbType.Bit).Value = model.EfficienctyFlag;		
				command.Parameters.Add("@NPLSLargeFlag", SqlDbType.Bit).Value = model.NPLSLargeFlag;		
				command.Parameters.Add("@LargeTxnManageFlag", SqlDbType.Bit).Value = model.LargeTxnManageFlag;		
				command.Parameters.Add("@HighValueAssetMangeFlag", SqlDbType.Bit).Value = model.HighValueAssetMangeFlag;		
				command.Parameters.Add("@BudgetMgtFlag", SqlDbType.Bit).Value = model.BudgetMgtFlag;		
				command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;
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
