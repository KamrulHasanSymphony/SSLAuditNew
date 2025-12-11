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
using Shampan.Core.Interfaces.Repository.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Repository.BKFinancePreformPreferencesCBS;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKFinancePreformPreferencesCBS
{
	public class BKFinancePreformPreferenceCBSRepository : Repository, IBKFinancePreformPreferenceCBSRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKFinancePreformPreferenceCBSRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<BKFinancePreformPreferenceCBS> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,BranchID
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


FROM  BKFinancePreformPreferenceCBS

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKFinancePreformPreferenceCBS> vms = dtResult.ToList<BKFinancePreformPreferenceCBS>();
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
            List<BKFinancePreformPreferenceCBS> VMs = new List<BKFinancePreformPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKFinancePreformPreferenceCBS.Id)FilteredCount
                from BKFinancePreformPreferenceCBS  where 1=1 ";

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


        public List<BKFinancePreformPreferenceCBS> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKFinancePreformPreferenceCBS> VMs = new List<BKFinancePreformPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
AuditYear,
FinancialPerformFlg,
Status

FROM BKFinancePreformPreferenceCBS 
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
                var req = new BKFinancePreformPreferenceCBS();

                VMs.Add(req);


                VMs = dt.ToList<BKFinancePreformPreferenceCBS>();

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
            List<BKFinancePreformPreferenceCBS> VMs = new List<BKFinancePreformPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKFinancePreformPreferenceCBS  where 1=1 ";


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

		public BKFinancePreformPreferenceCBS Insert(BKFinancePreformPreferenceCBS model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKFinancePreformPreferenceCBS(


 BranchID
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



	
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;			
						
				command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;			
				command.Parameters.Add("@AuditFiscalYear", SqlDbType.DateTime).Value = model.AuditFiscalYear;			
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

        public BKFinancePreformPreferenceCBS MultiplePost(BKFinancePreformPreferenceCBS objBKFinancePreformPreferenceCBS)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKFinancePreformPreferenceCBSs set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKFinancePreformPreferenceCBS;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKFinancePreformPreferenceCBS MultipleUnPost(BKFinancePreformPreferenceCBS vm)
		{

            return new BKFinancePreformPreferenceCBS();
		
		}

		public BKFinancePreformPreferenceCBS Update(BKFinancePreformPreferenceCBS model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKFinancePreformPreferenceCBS set

 
 BranchID                              =@BranchID  
,AuditYear                                =@AuditYear  
,AuditFiscalYear                           =@AuditFiscalYear  
,FinancialPerformFlg                        =@FinancialPerformFlg  
,FundAvailableFlg                          =@FundAvailableFlg  
,MisManagementClientsFlg                   =@MisManagementClientsFlg  
,EfficiencyFlg                             =@EfficiencyFlg  
,NplsLargeFlg                             =@NplsLargeFlg  
,LargeTxnManageFlg                        =@LargeTxnManageFlg  
,HighValueAssetManageFlg                        =@HighValueAssetManageFlg  
,SecurityMeasuresStaffFlg                        =@SecurityMeasuresStaffFlg  
,BudgetMgtFlg                                  =@BudgetMgtFlg  
,SignificantLossesFlg                        =@SignificantLossesFlg  
,Status                                   =@Status  


                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.DateTime).Value = model.AuditFiscalYear;
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
