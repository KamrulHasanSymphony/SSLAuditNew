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
using Shampan.Core.Interfaces.Repository.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKRiskAssessPerferenceSettings
{
	public class BKRiskAssessPerferenceSettingRepository : Repository, IBKRiskAssessPerferenceSettingRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKRiskAssessPerferenceSettingRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<BKRiskAssessPerferenceSetting> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,Code
,BranchID
,BKAuditOfficeTypeId
,BKAuditOfficeId
,Amount
,RiskLocFlag
,EntryDate
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


FROM  BKRiskAssessPerferenceSettings

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKRiskAssessPerferenceSetting> vms = dtResult.ToList<BKRiskAssessPerferenceSetting>();
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
            List<BKRiskAssessPerferenceSetting> VMs = new List<BKRiskAssessPerferenceSetting>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKRiskAssessPerferenceSettings.Id)FilteredCount
                from BKRiskAssessPerferenceSettings  where 1=1 ";

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


        public List<BKRiskAssessPerferenceSetting> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKRiskAssessPerferenceSetting> VMs = new List<BKRiskAssessPerferenceSetting>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
Code,
Amount,
EntryDate,
Status

FROM BKRiskAssessPerferenceSettings 
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
                var req = new BKRiskAssessPerferenceSetting();

                VMs.Add(req);


                VMs = dt.ToList<BKRiskAssessPerferenceSetting>();

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
            List<BKRiskAssessPerferenceSetting> VMs = new List<BKRiskAssessPerferenceSetting>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKRiskAssessPerferenceSettings  where 1=1 ";


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

        public List<BKRiskAssessPerferenceSetting> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<BKRiskAssessPerferenceSetting> VMs = new List<BKRiskAssessPerferenceSetting>();
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
,RiskTxnAmount Amount
,CompFinProductsFlg
,GeoLocRiskFlg
,InternationTxnFlg
,ForexFlg
,HighProfileClientsFlg
,CorporateClientsFlg
,AmlFlg
,KycGuidelinesFlg
,Status

FROM BKRiskAssessRegulationPreferenceCBSTemp 
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
                var req = new BKRiskAssessPerferenceSetting();

                VMs.Add(req);


                VMs = dt.ToList<BKRiskAssessPerferenceSetting>();

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

		public BKRiskAssessPerferenceSetting Insert(BKRiskAssessPerferenceSetting model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKRiskAssessPerferenceSettings(


 Code
,BranchID
,BKAuditOfficeTypeId
,BKAuditOfficeId
,Amount
,RiskLocFlag
,EntryDate
,Status
,AuditYear
--,AuditFiscalYear
,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@BranchID
,@BKAuditOfficeTypeId
,@BKAuditOfficeId
,@Amount
,@RiskLocFlag
,@EntryDate
,@Status
,@AuditYear
--,@AuditFiscalYear
,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");



				command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = model.Code;			
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;			
				command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;
				command.Parameters.Add("@BKAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;
				command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = model.Amount;
				command.Parameters.Add("@RiskLocFlag", SqlDbType.Bit).Value = model.RiskLocFlag;
				command.Parameters.Add("@EntryDate", SqlDbType.NVarChar).Value = model.EntryDate;
				command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                //command.Parameters.Add("@AuditFiscalYear", SqlDbType.NVarChar).Value = model.AuditFiscalYear;

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

        public BKRiskAssessPerferenceSetting MultiplePost(BKRiskAssessPerferenceSetting objBKRiskAssessPerferenceSetting)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKRiskAssessPerferenceSettings set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKRiskAssessPerferenceSetting;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKRiskAssessPerferenceSetting MultipleUnPost(BKRiskAssessPerferenceSetting vm)
		{

            return new BKRiskAssessPerferenceSetting();
		
		}

        public BKRiskAssessRegulationPreferenceCBS RiskAssessPreferenceTempInsert(BKRiskAssessRegulationPreferenceCBS model)
        {
            try
            {
                string sqlText = "";

                var command = CreateCommand(@" INSERT INTO BKRiskAssessRegulationPreferenceCBSTemp(


 BranchID
,AuditYear
,AuditFiscalYear
,RiskTxnAmount
,CompFinProductsFlg
,GeoLocRiskFlg
,InternationTxnFlg
,ForexFlg
,HighProfileClientsFlg
,CorporateClientsFlg
,AmlFlg
,KycGuidelinesFlg
,ImportId
,Status

) VALUES (

 @BranchID
,@AuditYear
,@AuditFiscalYear
,@RiskTxnAmount
,@CompFinProductsFlg
,@GeoLocRiskFlg
,@InternationTxnFlg
,@ForexFlg
,@HighProfileClientsFlg
,@CorporateClientsFlg
,@AmlFlg
,@KycGuidelinesFlg
,@ImportId
,@Status


)SELECT SCOPE_IDENTITY()");


                //command.Parameters.Add("@id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@ImportId", SqlDbType.Int).Value = model.ImportId;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.NVarChar).Value = model.AuditFiscalYear;
                command.Parameters.Add("@RiskTxnAmount", SqlDbType.Decimal).Value = model.RiskTxnAmount;
                command.Parameters.Add("@CompFinProductsFlg", SqlDbType.Decimal).Value = model.CompFinProductsFlg;
                command.Parameters.Add("@GeoLocRiskFlg", SqlDbType.Decimal).Value = model.GeoLocRiskFlg;
                command.Parameters.Add("@InternationTxnFlg", SqlDbType.Decimal).Value = model.InternationTxnFlg;
                command.Parameters.Add("@ForexFlg", SqlDbType.Decimal).Value = model.ForexFlg;
                command.Parameters.Add("@HighProfileClientsFlg", SqlDbType.Decimal).Value = model.HighProfileClientsFlg;
                command.Parameters.Add("@CorporateClientsFlg", SqlDbType.Decimal).Value = model.CorporateClientsFlg;
                command.Parameters.Add("@AmlFlg", SqlDbType.Decimal).Value = model.AmlFlg;
                command.Parameters.Add("@KycGuidelinesFlg", SqlDbType.Decimal).Value = model.KycGuidelinesFlg;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BKRiskAssessPerferenceSetting Update(BKRiskAssessPerferenceSetting model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKRiskAssessPerferenceSettings set

 
 BKAuditOfficeTypeId                =@BKAuditOfficeTypeId  
,BKAuditOfficeId                    =@BKAuditOfficeId  
,Amount                             =@Amount  
,RiskLocFlag                        =@RiskLocFlag  
,EntryDate                          =@EntryDate  
,Status                             =@Status  
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
				command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = model.Amount;		
				command.Parameters.Add("@RiskLocFlag", SqlDbType.Bit).Value = model.RiskLocFlag;		
				command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = model.EntryDate;		
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
