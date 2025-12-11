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
using Shampan.Core.Interfaces.Repository.BKReguCompliancesPreferenceSetting;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKFinancePerformPreferenceSetting
{
	public class BKReguCompliancesPreferenceSettingsRepository : Repository, IBKReguCompliancesPreferenceSettingRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKReguCompliancesPreferenceSettingsRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<BKReguCompliancesPreferenceSettings> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,Code
,BranchID
,BKAuditOfficeTypeId
,BKAuditOfficeId
,InternationTxnFlag
,ForexFlag
,HighProfileClintsFlag
,CorporateChientsFlag
,AmlFlag
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


FROM  BKReguCompliancesPreferenceSettings

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKReguCompliancesPreferenceSettings> vms = dtResult.ToList<BKReguCompliancesPreferenceSettings>();
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
            List<BKReguCompliancesPreferenceSettings> VMs = new List<BKReguCompliancesPreferenceSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKReguCompliancesPreferenceSettings.Id)FilteredCount
                from BKReguCompliancesPreferenceSettings  where 1=1 ";

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


        public List<BKReguCompliancesPreferenceSettings> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKReguCompliancesPreferenceSettings> VMs = new List<BKReguCompliancesPreferenceSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
Code,
InternationTxnFlag,
ForexFlag,
HighProfileClintsFlag,
CorporateChientsFlag,
AmlFlag,
Status

FROM BKReguCompliancesPreferenceSettings 
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
                var req = new BKReguCompliancesPreferenceSettings();

                VMs.Add(req);


                VMs = dt.ToList<BKReguCompliancesPreferenceSettings>();

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
            List<BKReguCompliancesPreferenceSettings> VMs = new List<BKReguCompliancesPreferenceSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKReguCompliancesPreferenceSettings  where 1=1 ";


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

		public BKReguCompliancesPreferenceSettings Insert(BKReguCompliancesPreferenceSettings model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKReguCompliancesPreferenceSettings(


 Code
,BranchID
,BKAuditOfficeTypeId
,BKAuditOfficeId
,InternationTxnFlag
,ForexFlag
,HighProfileClintsFlag
,CorporateChientsFlag
,AmlFlag
,Status
,AuditYear

--,AuditFiscalYear
--,InfoReceiveDate
--,InfoReceiveId
--,InfoReceiveFlag

,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@BranchID
,@BKAuditOfficeTypeId
,@BKAuditOfficeId
,@InternationTxnFlag
,@ForexFlag
,@HighProfileClintsFlag
,@CorporateChientsFlag
,@AmlFlag
,@Status
,@AuditYear

--,@AuditFiscalYear
--,@InfoReceiveDate
--,@InfoReceiveId
--,@InfoReceiveFlag

,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");



				command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = model.Code;			
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;			
				command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;
				command.Parameters.Add("@BKAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;	
				command.Parameters.Add("@InternationTxnFlag", SqlDbType.Bit).Value = model.InternationTxnFlag;
				command.Parameters.Add("@ForexFlag", SqlDbType.Bit).Value = model.ForexFlag;
				command.Parameters.Add("@HighProfileClintsFlag", SqlDbType.Bit).Value = model.HighProfileClintsFlag;
				command.Parameters.Add("@CorporateChientsFlag", SqlDbType.Bit).Value = model.CorporateChientsFlag;
				command.Parameters.Add("@AmlFlag", SqlDbType.Bit).Value = model.AmlFlag;
				command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;
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

        public BKReguCompliancesPreferenceSettings MultiplePost(BKReguCompliancesPreferenceSettings objBKReguCompliancesPreferenceSettings)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKReguCompliancesPreferenceSettingss set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKReguCompliancesPreferenceSettings;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKReguCompliancesPreferenceSettings MultipleUnPost(BKReguCompliancesPreferenceSettings vm)
		{

            return new BKReguCompliancesPreferenceSettings();
		
		}

		public BKReguCompliancesPreferenceSettings Update(BKReguCompliancesPreferenceSettings model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKReguCompliancesPreferenceSettings set

 BKAuditOfficeTypeId               =@BKAuditOfficeTypeId  
,BKAuditOfficeId                   =@BKAuditOfficeId  
,InternationTxnFlag                =@InternationTxnFlag  
,ForexFlag                         =@ForexFlag  
,HighProfileClintsFlag             =@HighProfileClintsFlag  
,CorporateChientsFlag              =@CorporateChientsFlag  
,AmlFlag                           =@AmlFlag  
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
				command.Parameters.Add("@BKAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;
				command.Parameters.Add("@InternationTxnFlag", SqlDbType.Bit).Value = model.InternationTxnFlag;
				command.Parameters.Add("@ForexFlag", SqlDbType.Bit).Value = model.ForexFlag;
				command.Parameters.Add("@HighProfileClintsFlag", SqlDbType.Bit).Value = model.HighProfileClintsFlag;
				command.Parameters.Add("@CorporateChientsFlag", SqlDbType.Bit).Value = model.CorporateChientsFlag;
				command.Parameters.Add("@AmlFlag", SqlDbType.Bit).Value = model.AmlFlag;
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
