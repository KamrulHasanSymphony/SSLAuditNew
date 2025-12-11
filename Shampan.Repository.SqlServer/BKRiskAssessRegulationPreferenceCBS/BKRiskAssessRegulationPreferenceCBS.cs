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
using Shampan.Core.Interfaces.Repository.BKRiskAssessRegulationPreferencesCBS;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKRiskAssessRegulationPreferencesCBS
{
	public class BKRiskAssessRegulationPreferenceCBSRepository : Repository, IBKRiskAssessRegulationPreferenceCBSRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKRiskAssessRegulationPreferenceCBSRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<BKRiskAssessRegulationPreferenceCBS> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,BranchID
,ISNULL(FORMAT(AuditYear, 'yyyy-MM-dd'), '199-01-01') AS AuditYear
,ISNULL(FORMAT(AuditFiscalYear, 'yyyy-MM-dd'), '1999-01-01') AS AuditFiscalYear
,RiskTxnAmount
,CompFinProductsFlg
,GeoLocRiskFlg
,InternationTxnFlg
,ForexFlg
,HighProfileClientsFlg
,CorporateClientsFlg
,AmlFlg
,KycGuidelinesFlg
,Status

FROM  BKRiskAssessRegulationPreferenceCBS

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKRiskAssessRegulationPreferenceCBS> vms = dtResult.ToList<BKRiskAssessRegulationPreferenceCBS>();
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
            List<BKRiskAssessRegulationPreferenceCBS> VMs = new List<BKRiskAssessRegulationPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKRiskAssessRegulationPreferenceCBS.Id)FilteredCount
                from BKRiskAssessRegulationPreferenceCBS  where 1=1 ";

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


        public List<BKRiskAssessRegulationPreferenceCBS> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKRiskAssessRegulationPreferenceCBS> VMs = new List<BKRiskAssessRegulationPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
AuditYear,
RiskTxnAmount,
Status

FROM BKRiskAssessRegulationPreferenceCBS 
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
                var req = new BKRiskAssessRegulationPreferenceCBS();

                VMs.Add(req);


                VMs = dt.ToList<BKRiskAssessRegulationPreferenceCBS>();

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
            List<BKRiskAssessRegulationPreferenceCBS> VMs = new List<BKRiskAssessRegulationPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKRiskAssessRegulationPreferenceCBS  where 1=1 ";


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

		public BKRiskAssessRegulationPreferenceCBS Insert(BKRiskAssessRegulationPreferenceCBS model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKRiskAssessRegulationPreferenceCBS(


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
,@Status


)SELECT SCOPE_IDENTITY()");



	
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;						
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.DateTime).Value = model.AuditFiscalYear;
                command.Parameters.Add("@RiskTxnAmount", SqlDbType.Decimal).Value = model.RiskTxnAmount;
                command.Parameters.Add("@CompFinProductsFlg", SqlDbType.Bit).Value = model.CompFinProductsFlg;
                command.Parameters.Add("@GeoLocRiskFlg", SqlDbType.Bit).Value = model.GeoLocRiskFlg;
                command.Parameters.Add("@InternationTxnFlg", SqlDbType.Bit).Value = model.InternationTxnFlg;
                command.Parameters.Add("@ForexFlg", SqlDbType.Bit).Value = model.ForexFlg;
                command.Parameters.Add("@HighProfileClientsFlg", SqlDbType.Bit).Value = model.HighProfileClientsFlg;
                command.Parameters.Add("@CorporateClientsFlg", SqlDbType.Bit).Value = model.CorporateClientsFlg;
                command.Parameters.Add("@AmlFlg", SqlDbType.Bit).Value = model.AmlFlg;
                command.Parameters.Add("@KycGuidelinesFlg", SqlDbType.Bit).Value = model.KycGuidelinesFlg;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public BKRiskAssessRegulationPreferenceCBS MultiplePost(BKRiskAssessRegulationPreferenceCBS objBKRiskAssessRegulationPreferenceCBS)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKRiskAssessRegulationPreferenceCBS set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKRiskAssessRegulationPreferenceCBS;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKRiskAssessRegulationPreferenceCBS MultipleUnPost(BKRiskAssessRegulationPreferenceCBS vm)
		{

            return new BKRiskAssessRegulationPreferenceCBS();
		
		}

		public BKRiskAssessRegulationPreferenceCBS Update(BKRiskAssessRegulationPreferenceCBS model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKRiskAssessRegulationPreferenceCBS set

 
 BranchID                =@BranchID  
,AuditYear                                        =@AuditYear  
,AuditFiscalYear                                  =@AuditFiscalYear  
,RiskTxnAmount                                   =@RiskTxnAmount  
,CompFinProductsFlg                              =@CompFinProductsFlg    
,GeoLocRiskFlg                                   =@GeoLocRiskFlg  
,InternationTxnFlg                               =@InternationTxnFlg  
,ForexFlg                                         =@ForexFlg  
,HighProfileClientsFlg                            =@HighProfileClientsFlg  
,CorporateClientsFlg                            =@CorporateClientsFlg  
,AmlFlg                                         =@AmlFlg  
,KycGuidelinesFlg                            =@KycGuidelinesFlg  
,Status                                       =@Status  

                     
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.DateTime).Value = model.AuditFiscalYear;
                command.Parameters.Add("@RiskTxnAmount", SqlDbType.Decimal).Value = model.RiskTxnAmount;
                command.Parameters.Add("@CompFinProductsFlg", SqlDbType.Bit).Value = model.CompFinProductsFlg;
                command.Parameters.Add("@GeoLocRiskFlg", SqlDbType.Bit).Value = model.GeoLocRiskFlg;
                command.Parameters.Add("@InternationTxnFlg", SqlDbType.Bit).Value = model.InternationTxnFlg;
                command.Parameters.Add("@ForexFlg", SqlDbType.Bit).Value = model.ForexFlg;
                command.Parameters.Add("@HighProfileClientsFlg", SqlDbType.Bit).Value = model.HighProfileClientsFlg;
                command.Parameters.Add("@CorporateClientsFlg", SqlDbType.Bit).Value = model.CorporateClientsFlg;
                command.Parameters.Add("@AmlFlg", SqlDbType.Bit).Value = model.AmlFlg;
                command.Parameters.Add("@KycGuidelinesFlg", SqlDbType.Bit).Value = model.KycGuidelinesFlg;
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
