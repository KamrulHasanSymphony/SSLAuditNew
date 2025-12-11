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
using Shampan.Core.Interfaces.Repository.BKAuditPreferenceEvaluation;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKAuditPreferenceEvaluation
{
	public class BKAuditPreferenceEvaluationRepository : Repository, IBKAuditPreferenceEvaluationRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKAuditPreferenceEvaluationRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<BKAuditPreferenceEvaluations> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,Code
,BranchID
,BkAuditOfficeId as BKAuditOfficeId
,FlagPercentFromCommonSettingSelectedValuesMin
,FlagPercentFromCommonSettingSelectedValuesMax
,FlagPercentFromRiskAssessSelectedValuesMin
,FlagPercentFromRiskAssessSelectedValuesMax
,FlagPercentFromReguCompliancesSelectedValuesMin
,FlagPercentFromReguComliancesSelectedValuesMax
,FlagPercentFromFinancePerformSelectedValuesMin
,FlagPercentFromFinancePerformSelectedValuesMax
,FlagPercentFromFraudIrrgularitiesSelectedValuesMin
,FlagPercentFromFraudIrregularitiesSelectedValuesMax
,FlagPercentFromInternalControlWeakSelectedValuesMin
,FlagPercentFromInternalControlWeakSelectedValuesMax

,EntryDate
,Status
,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom


FROM  BKAuditPreferenceEvaluations

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKAuditPreferenceEvaluations> vms = dtResult.ToList<BKAuditPreferenceEvaluations>();
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
            List<BKAuditPreferenceEvaluations> VMs = new List<BKAuditPreferenceEvaluations>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKAuditPreferenceEvaluations.Id)FilteredCount
                from BKAuditPreferenceEvaluations  where 1=1 ";

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


        public List<BKAuditPreferenceEvaluations> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKAuditPreferenceEvaluations> VMs = new List<BKAuditPreferenceEvaluations>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
Code,
ISNULL(FlagPercentFromCommonSettingSelectedValuesMin,0) FlagPercentFromCommonSettingSelectedValuesMin,
ISNULL(FlagPercentFromCommonSettingSelectedValuesMax,0) FlagPercentFromCommonSettingSelectedValuesMax,
ISNULL(FlagPercentFromRiskAssessSelectedValuesMin,0) FlagPercentFromRiskAssessSelectedValuesMin,
ISNULL(FlagPercentFromRiskAssessSelectedValuesMax,0) FlagPercentFromRiskAssessSelectedValuesMax,
ISNULL(FlagPercentFromReguCompliancesSelectedValuesMin,0) FlagPercentFromReguCompliancesSelectedValuesMin,
ISNULL(FlagPercentFromReguComliancesSelectedValuesMax,0) FlagPercentFromReguComliancesSelectedValuesMax,
ISNULL(FlagPercentFromFinancePerformSelectedValuesMin,0) FlagPercentFromFinancePerformSelectedValuesMin,
ISNULL(FlagPercentFromFinancePerformSelectedValuesMax,0) FlagPercentFromFinancePerformSelectedValuesMax,
ISNULL(FlagPercentFromFraudIrrgularitiesSelectedValuesMin,0) FlagPercentFromFraudIrrgularitiesSelectedValuesMin,
ISNULL(FlagPercentFromFraudIrregularitiesSelectedValuesMax,0) FlagPercentFromFraudIrregularitiesSelectedValuesMax,
ISNULL(FlagPercentFromInternalControlWeakSelectedValuesMin,0) FlagPercentFromInternalControlWeakSelectedValuesMin,
ISNULL(FlagPercentFromInternalControlWeakSelectedValuesMax,0) FlagPercentFromInternalControlWeakSelectedValuesMax,
EntryDate,
Status

FROM BKAuditPreferenceEvaluations 
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
                var req = new BKAuditPreferenceEvaluations();

                VMs.Add(req);


                VMs = dt.ToList<BKAuditPreferenceEvaluations>();

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
            List<BKAuditPreferenceEvaluations> VMs = new List<BKAuditPreferenceEvaluations>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKAuditPreferenceEvaluations  where 1=1 ";


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

		public BKAuditPreferenceEvaluations Insert(BKAuditPreferenceEvaluations model)
		{
			try
			{
				string sqlText = "";


				var command = CreateCommand(@" INSERT INTO BKAuditPreferenceEvaluations(


 Code
,BkAuditOfficeId
,BranchID
,FlagPercentFromCommonSettingSelectedValuesMin
,FlagPercentFromCommonSettingSelectedValuesMax
,FlagPercentFromRiskAssessSelectedValuesMin
,FlagPercentFromRiskAssessSelectedValuesMax
,FlagPercentFromReguCompliancesSelectedValuesMin
,FlagPercentFromReguComliancesSelectedValuesMax
,FlagPercentFromFinancePerformSelectedValuesMin
,FlagPercentFromFinancePerformSelectedValuesMax
,FlagPercentFromFraudIrrgularitiesSelectedValuesMin
,FlagPercentFromFraudIrregularitiesSelectedValuesMax
,FlagPercentFromInternalControlWeakSelectedValuesMin
,FlagPercentFromInternalControlWeakSelectedValuesMax
,EntryDate
,Status
,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@BkAuditOfficeId
,@BranchID
,@FlagPercentFromCommonSettingSelectedValuesMin
,@FlagPercentFromCommonSettingSelectedValuesMax
,@FlagPercentFromRiskAssessSelectedValuesMin
,@FlagPercentFromRiskAssessSelectedValuesMax
,@FlagPercentFromReguCompliancesSelectedValuesMin
,@FlagPercentFromReguComliancesSelectedValuesMax

,@FlagPercentFromFinancePerformSelectedValuesMin
,@FlagPercentFromFinancePerformSelectedValuesMax
,@FlagPercentFromFraudIrrgularitiesSelectedValuesMin
,@FlagPercentFromFraudIrregularitiesSelectedValuesMax
,@FlagPercentFromInternalControlWeakSelectedValuesMin
,@FlagPercentFromInternalControlWeakSelectedValuesMax
,@EntryDate
,@Status
,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");



				command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = model.Code;			
				command.Parameters.Add("@BkAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;			
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;			
				command.Parameters.Add("@FlagPercentFromCommonSettingSelectedValuesMin", SqlDbType.Decimal).Value = model.FlagPercentFromCommonSettingSelectedValuesMin;			
				command.Parameters.Add("@FlagPercentFromCommonSettingSelectedValuesMax", SqlDbType.Decimal).Value = model.FlagPercentFromCommonSettingSelectedValuesMax;			
				command.Parameters.Add("@FlagPercentFromRiskAssessSelectedValuesMin", SqlDbType.Decimal).Value = model.FlagPercentFromRiskAssessSelectedValuesMin;			
				command.Parameters.Add("@FlagPercentFromRiskAssessSelectedValuesMax", SqlDbType.Decimal).Value = model.FlagPercentFromRiskAssessSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromReguCompliancesSelectedValuesMin", SqlDbType.Decimal).Value = model.FlagPercentFromReguCompliancesSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromReguComliancesSelectedValuesMax", SqlDbType.Decimal).Value = model.FlagPercentFromReguComliancesSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromFinancePerformSelectedValuesMin", SqlDbType.Decimal).Value = model.FlagPercentFromFinancePerformSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromFinancePerformSelectedValuesMax", SqlDbType.Decimal).Value = model.FlagPercentFromFinancePerformSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromFraudIrrgularitiesSelectedValuesMin", SqlDbType.Decimal).Value = model.FlagPercentFromFraudIrrgularitiesSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromFraudIrregularitiesSelectedValuesMax", SqlDbType.Decimal).Value = model.FlagPercentFromFraudIrregularitiesSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromInternalControlWeakSelectedValuesMin", SqlDbType.Decimal).Value = model.FlagPercentFromInternalControlWeakSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromInternalControlWeakSelectedValuesMax", SqlDbType.Decimal).Value = model.FlagPercentFromInternalControlWeakSelectedValuesMax;
                
				command.Parameters.Add("@EntryDate", SqlDbType.NVarChar).Value = model.EntryDate;
				command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;

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

        public BKAuditPreferenceEvaluations MultiplePost(BKAuditPreferenceEvaluations objBKAuditPreferenceEvaluations)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKAuditPreferenceEvaluations set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKAuditPreferenceEvaluations;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKAuditPreferenceEvaluations MultipleUnPost(BKAuditPreferenceEvaluations vm)
		{

            return new BKAuditPreferenceEvaluations();
		
		}

		public BKAuditPreferenceEvaluations Update(BKAuditPreferenceEvaluations model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKAuditPreferenceEvaluations set

  

 BkAuditOfficeId                                                        =@BkAuditOfficeId  
,FlagPercentFromCommonSettingSelectedValuesMin                          =@FlagPercentFromCommonSettingSelectedValuesMin  
,FlagPercentFromCommonSettingSelectedValuesMax                          =@FlagPercentFromCommonSettingSelectedValuesMax  
,FlagPercentFromRiskAssessSelectedValuesMin                          =@FlagPercentFromRiskAssessSelectedValuesMin  
,FlagPercentFromRiskAssessSelectedValuesMax                          =@FlagPercentFromRiskAssessSelectedValuesMax  
,FlagPercentFromReguCompliancesSelectedValuesMin                          =@FlagPercentFromReguCompliancesSelectedValuesMin  
,FlagPercentFromReguComliancesSelectedValuesMax                          =@FlagPercentFromReguComliancesSelectedValuesMax  
,FlagPercentFromFinancePerformSelectedValuesMin                          =@FlagPercentFromFinancePerformSelectedValuesMin  
,FlagPercentFromFinancePerformSelectedValuesMax                          =@FlagPercentFromFinancePerformSelectedValuesMax  
,FlagPercentFromFraudIrrgularitiesSelectedValuesMin                          =@FlagPercentFromFraudIrrgularitiesSelectedValuesMin  
,FlagPercentFromFraudIrregularitiesSelectedValuesMax                          =@FlagPercentFromFraudIrregularitiesSelectedValuesMax  
,FlagPercentFromInternalControlWeakSelectedValuesMin                          =@FlagPercentFromInternalControlWeakSelectedValuesMin  
,FlagPercentFromInternalControlWeakSelectedValuesMax                          =@FlagPercentFromInternalControlWeakSelectedValuesMax  



,EntryDate                          =@EntryDate  
,Status                             =@Status  
,LastUpdateBy                       =@LastUpdateBy  
,LastUpdateOn                       =@LastUpdateOn  
,LastUpdateFrom                     =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

				command.Parameters.Add("@BkAuditOfficeId", SqlDbType.Int).Value = model.BKAuditOfficeId;
				command.Parameters.Add("@FlagPercentFromCommonSettingSelectedValuesMin", SqlDbType.NChar).Value = model.FlagPercentFromCommonSettingSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromCommonSettingSelectedValuesMax", SqlDbType.NChar).Value = model.FlagPercentFromCommonSettingSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromRiskAssessSelectedValuesMin", SqlDbType.NChar).Value = model.FlagPercentFromRiskAssessSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromRiskAssessSelectedValuesMax", SqlDbType.NChar).Value = model.FlagPercentFromRiskAssessSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromReguCompliancesSelectedValuesMin", SqlDbType.NChar).Value = model.FlagPercentFromReguCompliancesSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromReguComliancesSelectedValuesMax", SqlDbType.NChar).Value = model.FlagPercentFromReguComliancesSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromFinancePerformSelectedValuesMin", SqlDbType.NChar).Value = model.FlagPercentFromFinancePerformSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromFinancePerformSelectedValuesMax", SqlDbType.NChar).Value = model.FlagPercentFromFinancePerformSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromFraudIrrgularitiesSelectedValuesMin", SqlDbType.NChar).Value = model.FlagPercentFromFraudIrrgularitiesSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromFraudIrregularitiesSelectedValuesMax", SqlDbType.NChar).Value = model.FlagPercentFromFraudIrregularitiesSelectedValuesMax;
				command.Parameters.Add("@FlagPercentFromInternalControlWeakSelectedValuesMin", SqlDbType.NChar).Value = model.FlagPercentFromInternalControlWeakSelectedValuesMin;
				command.Parameters.Add("@FlagPercentFromInternalControlWeakSelectedValuesMax", SqlDbType.NChar).Value = model.FlagPercentFromInternalControlWeakSelectedValuesMax;
                
				command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = model.EntryDate;		
				command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;
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
