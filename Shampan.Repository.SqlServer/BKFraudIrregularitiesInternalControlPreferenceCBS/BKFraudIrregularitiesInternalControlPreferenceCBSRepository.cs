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
using Shampan.Core.Interfaces.Repository.BKFraudIrregularitiesInternalControlPreferencesCBS;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKFraudIrregularitiesInternalControlPreferencesCBS
{
	public class BKFraudIrregularitiesInternalControlPreferenceCBSRepository : Repository, IBKFraudIrregularitiesInternalControlPreferenceCBSRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKFraudIrregularitiesInternalControlPreferenceCBSRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<BKFraudIrregularitiesInternalControlPreferenceCBS> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,BranchID
,AuditYear
,AuditFiscalYear
,PreviouslyFraudIncidentFlg
,EmpMisConductFlg
,IrregularitiesFlg
,InternalControlFlg
,ProperDocumentationFlg
,ProperReportingFlg
,Status


FROM  BKFraudIrregularitiesInternalControlPreferenceCBS

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKFraudIrregularitiesInternalControlPreferenceCBS> vms = dtResult.ToList<BKFraudIrregularitiesInternalControlPreferenceCBS>();
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
            List<BKFraudIrregularitiesInternalControlPreferenceCBS> VMs = new List<BKFraudIrregularitiesInternalControlPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKFraudIrregularitiesInternalControlPreferenceCBS.Id)FilteredCount
                from BKFraudIrregularitiesInternalControlPreferenceCBS  where 1=1 ";

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


        public List<BKFraudIrregularitiesInternalControlPreferenceCBS> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKFraudIrregularitiesInternalControlPreferenceCBS> VMs = new List<BKFraudIrregularitiesInternalControlPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
AuditYear,
PreviouslyFraudIncidentFlg,
Status

FROM BKFraudIrregularitiesInternalControlPreferenceCBS 
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
                var req = new BKFraudIrregularitiesInternalControlPreferenceCBS();

                VMs.Add(req);


                VMs = dt.ToList<BKFraudIrregularitiesInternalControlPreferenceCBS>();

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
            List<BKFraudIrregularitiesInternalControlPreferenceCBS> VMs = new List<BKFraudIrregularitiesInternalControlPreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKFraudIrregularitiesInternalControlPreferenceCBS  where 1=1 ";


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

		public BKFraudIrregularitiesInternalControlPreferenceCBS Insert(BKFraudIrregularitiesInternalControlPreferenceCBS model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKFraudIrregularitiesInternalControlPreferenceCBS(



BranchID
,AuditYear
,AuditFiscalYear
,PreviouslyFraudIncidentFlg
,EmpMisConductFlg
,IrregularitiesFlg
,InternalControlFlg
,ProperDocumentationFlg
,ProperReportingFlg
,Status


) VALUES (



@BranchID
,@AuditYear
,@AuditFiscalYear
,@PreviouslyFraudIncidentFlg
,@EmpMisConductFlg
,@IrregularitiesFlg
,@InternalControlFlg
,@ProperDocumentationFlg
,@ProperReportingFlg
,@Status


)SELECT SCOPE_IDENTITY()");



	
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;			
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.DateTime).Value = model.AuditFiscalYear;
                command.Parameters.Add("@PreviouslyFraudIncidentFlg", SqlDbType.Bit).Value = model.PreviouslyFraudIncidentFlg;
                command.Parameters.Add("@EmpMisConductFlg", SqlDbType.Bit).Value = model.EmpMisConductFlg;
                command.Parameters.Add("@IrregularitiesFlg", SqlDbType.Bit).Value = model.IrregularitiesFlg;
                command.Parameters.Add("@InternalControlFlg", SqlDbType.Bit).Value = model.InternalControlFlg;
                command.Parameters.Add("@ProperDocumentationFlg", SqlDbType.Bit).Value = model.ProperDocumentationFlg;
                command.Parameters.Add("@ProperReportingFlg", SqlDbType.Bit).Value = model.ProperReportingFlg;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;


                model.Id = Convert.ToInt32(command.ExecuteScalar());

				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public BKFraudIrregularitiesInternalControlPreferenceCBS MultiplePost(BKFraudIrregularitiesInternalControlPreferenceCBS objBKFraudIrregularitiesInternalControlPreferenceCBS)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKFraudIrregularitiesInternalControlPreferenceCBS set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKFraudIrregularitiesInternalControlPreferenceCBS;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKFraudIrregularitiesInternalControlPreferenceCBS MultipleUnPost(BKFraudIrregularitiesInternalControlPreferenceCBS vm)
		{

            return new BKFraudIrregularitiesInternalControlPreferenceCBS();
		
		}

		public BKFraudIrregularitiesInternalControlPreferenceCBS Update(BKFraudIrregularitiesInternalControlPreferenceCBS model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKFraudIrregularitiesInternalControlPreferenceCBS set

 
 BranchID                =@BranchID  
,AuditYear                                         =@AuditYear  
,AuditFiscalYear                                   =@AuditFiscalYear  
,PreviouslyFraudIncidentFlg                         =@PreviouslyFraudIncidentFlg  
,EmpMisConductFlg                                   =@EmpMisConductFlg  
,IrregularitiesFlg                                  =@IrregularitiesFlg  
,InternalControlFlg                                 =@InternalControlFlg  
,ProperDocumentationFlg                             =@ProperDocumentationFlg  
,ProperReportingFlg                                 =@ProperReportingFlg  
,Status                                             =@Status  

                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;


                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.DateTime).Value = model.AuditFiscalYear;
                command.Parameters.Add("@PreviouslyFraudIncidentFlg", SqlDbType.Bit).Value = model.PreviouslyFraudIncidentFlg;
                command.Parameters.Add("@EmpMisConductFlg", SqlDbType.Bit).Value = model.EmpMisConductFlg;
                command.Parameters.Add("@IrregularitiesFlg", SqlDbType.Bit).Value = model.IrregularitiesFlg;
                command.Parameters.Add("@InternalControlFlg", SqlDbType.Bit).Value = model.InternalControlFlg;
                command.Parameters.Add("@ProperDocumentationFlg", SqlDbType.Bit).Value = model.ProperDocumentationFlg;
                command.Parameters.Add("@ProperReportingFlg", SqlDbType.Bit).Value = model.ProperReportingFlg;
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
