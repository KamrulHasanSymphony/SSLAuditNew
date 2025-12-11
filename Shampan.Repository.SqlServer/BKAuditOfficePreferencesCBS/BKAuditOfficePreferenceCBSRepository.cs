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
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKAuditOfficePreferencesCBS
{
	public class BKAuditOfficePreferenceCBSRepository : Repository, IBKAuditOfficePreferenceCBSRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKAuditOfficePreferenceCBSRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

        {
            this._context = context;
			this._transaction = transaction;
		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

        public BKAuditOfficePreferenceCBS AuditOfficePreferenceCBSTempInsert(BKAuditOfficePreferenceCBS model)
        {
            try
            {
                string sqlText = "";

                var command = CreateCommand(@" INSERT INTO BKAuditOfficePreferenceCBSTemp(




 BranchID
,AuditYear
,AuditFiscalYear
,HistoricalPerformFlg
,LastYearAuditFindingsFlg
,PreviousYearsExceptLastYearAuditFindingsFlg
,TechCyberRiskFlg
,OfficeSizeFlg
,OfficeSignificanceFlg
,StaffTurnoverFlg
,StaffTrainingGapsFlg
,StrategicInitiativeFlg
,OperationalCompFlg
,EntryDate
,UpdateDate
,ImportId
,Status

) VALUES (


 

@BranchID
,@AuditYear
,@AuditFiscalYear
,@HistoricalPerformFlg
,@LastYearAuditFindingsFlg
,@PreviousYearsExceptLastYearAuditFindingsFlg
,@TechCyberRiskFlg
,@OfficeSizeFlg
,@OfficeSignificanceFlg
,@StaffTurnoverFlg
,@StaffTrainingGapsFlg
,@StrategicInitiativeFlg
,@OperationalCompFlg
,@EntryDate
,@UpdateDate
,@ImportId
,@Status


)SELECT SCOPE_IDENTITY()");




                //command.Parameters.Add("@id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.NVarChar).Value = model.AuditFiscalYear;
                command.Parameters.Add("@HistoricalPerformFlg", SqlDbType.Bit).Value = model.HistoricalPerformFlg;
                command.Parameters.Add("@LastYearAuditFindingsFlg", SqlDbType.Bit).Value = model.LastYearAuditFindingsFlg;
                command.Parameters.Add("@PreviousYearsExceptLastYearAuditFindingsFlg", SqlDbType.Bit).Value = model.PreviousYearsExceptLastYearAuditFindingsFlg;
                command.Parameters.Add("@TechCyberRiskFlg", SqlDbType.Bit).Value = model.TechCyberRiskFlg;
                command.Parameters.Add("@OfficeSizeFlg", SqlDbType.Bit).Value = model.OfficeSizeFlg;
                command.Parameters.Add("@OfficeSignificanceFlg", SqlDbType.Bit).Value = model.OfficeSignificanceFlg;
                command.Parameters.Add("@StaffTurnoverFlg", SqlDbType.Bit).Value = model.StaffTurnoverFlg;
                command.Parameters.Add("@StaffTrainingGapsFlg", SqlDbType.Bit).Value = model.StaffTrainingGapsFlg;
                command.Parameters.Add("@StrategicInitiativeFlg", SqlDbType.Bit).Value = model.StrategicInitiativeFlg;
                command.Parameters.Add("@OperationalCompFlg", SqlDbType.Bit).Value = model.OperationalCompFlg;
                command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = model.EntryDate;
                command.Parameters.Add("@UpdateDate", SqlDbType.DateTime).Value = model.UpdateDate;
                command.Parameters.Add("@ImportId", SqlDbType.Int).Value = model.ImportId;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public List<BKAuditOfficePreferenceCBS> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 

 Id
,BranchID
,AuditYear
,AuditFiscalYear
,HistoricalPerformFlg
,LastYearAuditFindingsFlg
,PreviousYearsExceptLastYearAuditFindingsFlg
,TechCyberRiskFlg
,OfficeSizeFlg
,OfficeSignificanceFlg
,StaffTurnoverFlg
,StaffTrainingGapsFlg
,StrategicInitiativeFlg
,OperationalCompFlg
,EntryDate
,UpdateDate
,Status


FROM  BKAuditOfficePreferenceCBS

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();

                adapter.Fill(dtResult);

                List<BKAuditOfficePreferenceCBS> vms = dtResult.ToList<BKAuditOfficePreferenceCBS>();
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
            List<BKAuditOfficePreferenceCBS> VMs = new List<BKAuditOfficePreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKAuditOfficePreferenceCBS.Id)FilteredCount
                from BKAuditOfficePreferenceCBS  where 1=1 ";

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


        public List<BKAuditOfficePreferenceCBS> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKAuditOfficePreferenceCBS> VMs = new List<BKAuditOfficePreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
 Id
,BranchID
,AuditYear
,AuditFiscalYear
,HistoricalPerformFlg
,LastYearAuditFindingsFlg
,PreviousYearsExceptLastYearAuditFindingsFlg
,TechCyberRiskFlg
,OfficeSizeFlg
,OfficeSignificanceFlg
,StaffTurnoverFlg
,StaffTrainingGapsFlg
,StrategicInitiativeFlg
,OperationalCompFlg
,EntryDate
,UpdateDate
,Status

FROM BKAuditOfficePreferenceCBS 
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
                var req = new BKAuditOfficePreferenceCBS();

                VMs.Add(req);


                VMs = dt.ToList<BKAuditOfficePreferenceCBS>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public List<BKAuditOfficePreferenceCBS> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<BKAuditOfficePreferenceCBS> VMs = new List<BKAuditOfficePreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
 Id
,BranchID
,AuditYear
,AuditFiscalYear
,HistoricalPerformFlg
,LastYearAuditFindingsFlg
,PreviousYearsExceptLastYearAuditFindingsFlg
,TechCyberRiskFlg
,OfficeSizeFlg
,OfficeSignificanceFlg
,StaffTurnoverFlg
,StaffTrainingGapsFlg
,StrategicInitiativeFlg
,OperationalCompFlg
,EntryDate
,UpdateDate
,Status

FROM BKAuditOfficePreferenceCBSTemp 
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
                var req = new BKAuditOfficePreferenceCBS();

                VMs.Add(req);


                VMs = dt.ToList<BKAuditOfficePreferenceCBS>();

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
            List<BKAuditOfficePreferenceCBS> VMs = new List<BKAuditOfficePreferenceCBS>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM BKAuditOfficePreferenceCBS  where 1=1 ";


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

		public BKAuditOfficePreferenceCBS Insert(BKAuditOfficePreferenceCBS model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKAuditOfficePreferenceCBS(


 BranchID
,AuditYear
,AuditFiscalYear
,HistoricalPerformFlg
,LastYearAuditFindingsFlg
,PreviousYearsExceptLastYearAuditFindingsFlg
,TechCyberRiskFlg
,OfficeSizeFlg
,OfficeSignificanceFlg
,StaffTurnoverFlg
,StaffTrainingGapsFlg
,StrategicInitiativeFlg
,OperationalCompFlg
,EntryDate
,UpdateDate
,Status

) VALUES (


 @BranchID
,@AuditYear
,@AuditFiscalYear
,@HistoricalPerformFlg
,@LastYearAuditFindingsFlg
,@PreviousYearsExceptLastYearAuditFindingsFlg
,@TechCyberRiskFlg
,@OfficeSizeFlg
,@OfficeSignificanceFlg
,@StaffTurnoverFlg
,@StaffTrainingGapsFlg
,@StrategicInitiativeFlg
,@OperationalCompFlg
,@EntryDate
,@UpdateDate
,@Status


)SELECT SCOPE_IDENTITY()");



			
				command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
				command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
				command.Parameters.Add("@AuditFiscalYear", SqlDbType.NVarChar).Value = model.AuditFiscalYear;
				command.Parameters.Add("@HistoricalPerformFlg", SqlDbType.Bit).Value = model.HistoricalPerformFlg;
				command.Parameters.Add("@LastYearAuditFindingsFlg", SqlDbType.Bit).Value = model.LastYearAuditFindingsFlg;
				command.Parameters.Add("@PreviousYearsExceptLastYearAuditFindingsFlg", SqlDbType.Bit).Value = model.PreviousYearsExceptLastYearAuditFindingsFlg;
				command.Parameters.Add("@TechCyberRiskFlg", SqlDbType.Bit).Value = model.TechCyberRiskFlg;
				command.Parameters.Add("@OfficeSizeFlg", SqlDbType.Bit).Value = model.OfficeSizeFlg;
                command.Parameters.Add("@OfficeSignificanceFlg", SqlDbType.Bit).Value = model.OfficeSignificanceFlg;
                command.Parameters.Add("@StaffTurnoverFlg", SqlDbType.Bit).Value = model.StaffTurnoverFlg;
                command.Parameters.Add("@StaffTrainingGapsFlg", SqlDbType.Bit).Value = model.StaffTrainingGapsFlg;
                command.Parameters.Add("@StrategicInitiativeFlg", SqlDbType.Bit).Value = model.StrategicInitiativeFlg;
                command.Parameters.Add("@OperationalCompFlg", SqlDbType.Bit).Value = model.OperationalCompFlg;
                command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = model.EntryDate;
                command.Parameters.Add("@UpdateDate", SqlDbType.DateTime).Value = model.UpdateDate;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public BKAuditOfficePreferenceCBS MultiplePost(BKAuditOfficePreferenceCBS objBKAuditOfficePreferenceCBS)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKAuditOfficePreferenceCBSs set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objBKAuditOfficePreferenceCBS;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKAuditOfficePreferenceCBS MultipleUnPost(BKAuditOfficePreferenceCBS vm)
		{

            return new BKAuditOfficePreferenceCBS();
		
		}

		public BKAuditOfficePreferenceCBS Update(BKAuditOfficePreferenceCBS model)
		{
			try
			{
				string sqlText = "";
				int count = 0;


                string query = @"  update BKAuditOfficePreferenceCBS set

 
 BranchID                                         =@BranchID  
,AuditYear                                        =@AuditYear  
,AuditFiscalYear                                  =@AuditFiscalYear  
,HistoricalPerformFlg                             =@HistoricalPerformFlg  
,LastYearAuditFindingsFlg                         =@LastYearAuditFindingsFlg  
,PreviousYearsExceptLastYearAuditFindingsFlg      =@PreviousYearsExceptLastYearAuditFindingsFlg  
,TechCyberRiskFlg                                 =@TechCyberRiskFlg  
,OfficeSizeFlg                                    =@OfficeSizeFlg 
,OfficeSignificanceFlg                            =@OfficeSignificanceFlg 
,StaffTurnoverFlg                                 =@StaffTurnoverFlg 
,StaffTrainingGapsFlg                             =@StaffTrainingGapsFlg 
,StrategicInitiativeFlg                           =@StrategicInitiativeFlg 
,OperationalCompFlg                               =@OperationalCompFlg 
,EntryDate                                        =@EntryDate 
,UpdateDate                                       =@UpdateDate 
,Status                                           =@Status  


where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@BranchID", SqlDbType.Int).Value = model.BranchID;
                command.Parameters.Add("@AuditYear", SqlDbType.DateTime).Value = model.AuditYear;
                command.Parameters.Add("@AuditFiscalYear", SqlDbType.NVarChar).Value = model.AuditFiscalYear;
                command.Parameters.Add("@HistoricalPerformFlg", SqlDbType.Bit).Value = model.HistoricalPerformFlg;
                command.Parameters.Add("@LastYearAuditFindingsFlg", SqlDbType.Bit).Value = model.LastYearAuditFindingsFlg;
                command.Parameters.Add("@PreviousYearsExceptLastYearAuditFindingsFlg", SqlDbType.Bit).Value = model.PreviousYearsExceptLastYearAuditFindingsFlg;
                command.Parameters.Add("@TechCyberRiskFlg", SqlDbType.Bit).Value = model.TechCyberRiskFlg;
                command.Parameters.Add("@OfficeSizeFlg", SqlDbType.Bit).Value = model.OfficeSizeFlg;
                command.Parameters.Add("@OfficeSignificanceFlg", SqlDbType.Bit).Value = model.OfficeSignificanceFlg;
                command.Parameters.Add("@StaffTurnoverFlg", SqlDbType.Bit).Value = model.StaffTurnoverFlg;
                command.Parameters.Add("@StaffTrainingGapsFlg", SqlDbType.Bit).Value = model.StaffTrainingGapsFlg;
                command.Parameters.Add("@StrategicInitiativeFlg", SqlDbType.Bit).Value = model.StrategicInitiativeFlg;
                command.Parameters.Add("@OperationalCompFlg", SqlDbType.Bit).Value = model.OperationalCompFlg;
                command.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = model.EntryDate;
                command.Parameters.Add("@UpdateDate", SqlDbType.DateTime).Value = model.UpdateDate;
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
