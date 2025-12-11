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
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Core.Interfaces.Repository.FiscalYear;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.FiscalYear
{
	public class FiscalYearRepository : Repository, IFiscalYearRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public FiscalYearRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public bool CheckFiscalYearStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            try
            {
                bool fiscalYear = false;
                string Post = "";

                DataTable dt = new DataTable();

                // ToDo sql injection
                string sqlText = "select Year  from " + tableName + " where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand command = CreateCommand(sqlText);

                command = ApplyParameters(command, conditionalFields, conditionalValue);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Post = dt.Rows[0]["Year"].ToString();
                    return true;
                }

                return fiscalYear;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<FiscalYearVM> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"

SELECT

 Id
,Year
,YearStart
,YearEnd
,YearLock
,Remarks
,CreatedBy
,CreatedOn
,CreatedFrom

FROM  FiscalYears

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<FiscalYearVM> vms = dtResult.ToList<FiscalYearVM>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
		}

        public List<FiscalYearDetailVM> GetAllDetail(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"

SELECT

 Id
,FiscalYearId
,Year
,MonthId
,MonthName
,MonthStart
,MonthEnd
,MonthLock
,Remarks

FROM  FiscalYearDetails

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<FiscalYearDetailVM> vms = dtResult.ToList<FiscalYearDetailVM>();
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
            List<FiscalYearVM> VMs = new List<FiscalYearVM>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(FiscalYearVMs.Id)FilteredCount
                from FiscalYearVMs  where 1=1 ";

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


        public List<FiscalYearVM> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<FiscalYearVM> VMs = new List<FiscalYearVM>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select

 Id
,Year
,YearStart
,YearEnd
,YearLock
,Remarks
,CreatedBy
,CreatedOn
,CreatedFrom

FROM FiscalYears 
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
                var req = new FiscalYearVM();

                VMs.Add(req);


                VMs = dt.ToList<FiscalYearVM>();

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
            List<FiscalYearVM> VMs = new List<FiscalYearVM>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM FiscalYearVMs  where 1=1 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                if (index.PId != "xx" && index.PId != "")
                {
                    string Pid = index.PId;
                    sqlText += " AND FiscalYearVMs.PId = @PID";
                }
                if (index.P_Level != "0" && index.PId != "")
                {
                    string P_Level = index.P_Level;
                    sqlText += " AND FiscalYearVMs.P_Level = @P_Level";
                }


                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                if (index.PId != "xx" && index.PId != "")
                {
                    string Pid = index.PId;
                    objComm.SelectCommand.Parameters.AddWithValue("@PID", Pid);
                }
                if (index.P_Level != "0" && index.PId != "")
                {
                    string P_Level = index.P_Level;
                    objComm.SelectCommand.Parameters.AddWithValue("@P_Level", P_Level);
                }

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

            try
            {
                string sqlText = @"select SettingValue from Settings where 1=1";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResutl = new DataTable();

                adapter.Fill(dtResutl);

                string settingValue = "2";

                if (dtResutl.Rows.Count > 0)
                {
                    DataRow row = dtResutl.Rows[0];
                    settingValue = row["SettingValue"].ToString();
                }

                return settingValue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

		public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public FiscalYearVM Insert(FiscalYearVM model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO FiscalYears(

 Year
,YearStart
,YearEnd
,YearLock
,Remarks
,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (

 @Year
,@YearStart
,@YearEnd
,@YearLock
,@Remarks
,@CreatedBy
,@CreatedOn
,@CreatedFrom



)SELECT SCOPE_IDENTITY()");

            
				command.Parameters.Add("@Year", SqlDbType.Int).Value = model.Year;			
				command.Parameters.Add("@YearStart", SqlDbType.DateTime).Value = model.YearStart;			
				command.Parameters.Add("@YearEnd", SqlDbType.DateTime).Value = model.YearEnd;			
				command.Parameters.Add("@YearLock", SqlDbType.Bit).Value = model.YearLock;			
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks;

                command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
                command.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
                command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();

                model.Id = Convert.ToInt32(command.ExecuteScalar());

				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public FiscalYearDetailVM InsertDetails(FiscalYearDetailVM model)
        {
            try
            {
                string sqlText = "";

                var command = CreateCommand(@" INSERT INTO FiscalYearDetails(

 FiscalYearId
,Year
,MonthId
,MonthName
,MonthStart
,MonthEnd
,MonthLock
,Remarks


) VALUES (

 @FiscalYearId
,@Year
,@MonthId
,@MonthName
,@MonthStart
,@MonthEnd
,@MonthLock
,@Remarks


)SELECT SCOPE_IDENTITY()");


                command.Parameters.Add("@FiscalYearId", SqlDbType.Int).Value = model.FiscalYearId;
                command.Parameters.Add("@Year", SqlDbType.Int).Value = model.Year;
                command.Parameters.Add("@MonthId", SqlDbType.Int).Value = model.MonthId;
                command.Parameters.Add("@MonthName", SqlDbType.NVarChar).Value = model.MonthName;
                command.Parameters.Add("@MonthStart", SqlDbType.NVarChar).Value = model.MonthStart;
                command.Parameters.Add("@MonthEnd", SqlDbType.NVarChar).Value = model.MonthEnd;
                command.Parameters.Add("@MonthLock", SqlDbType.Bit).Value = model.MonthLock;
                //command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = model.Remarks;
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks;



                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FiscalYearVM MultiplePost(FiscalYearVM objFiscalYearVM)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update FiscalYearVMs set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objFiscalYearVM;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public FiscalYearVM MultipleUnPost(FiscalYearVM vm)
		{

            return new FiscalYearVM();
		
		}

		public FiscalYearVM Update(FiscalYearVM model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update FiscalYears set

 
 Year                         =@Year  
,YearStart                    =@YearStart  
,YearEnd                      =@YearEnd  
,YearLock                     =@YearLock   
,Remarks                      =@Remarks   
,LastUpdateBy                 =@LastUpdateBy   
,LastUpdateOn                 =@LastUpdateOn   
,LastUpdateFrom               =@LastUpdateFrom   
                       
where  Id= @Id ";



                SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@Year", SqlDbType.Int).Value = model.Year;
                command.Parameters.Add("@YearStart", SqlDbType.DateTime).Value = model.YearStart;
                command.Parameters.Add("@YearEnd", SqlDbType.DateTime).Value = model.YearEnd;
                command.Parameters.Add("@YearLock", SqlDbType.Bit).Value = model.YearLock;
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks;


                command.Parameters.Add("@LastUpdateBy", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                command.Parameters.Add("@LastUpdateOn", SqlDbType.DateTime).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();
                command.Parameters.Add("@LastUpdateFrom", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateFrom.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateFrom.ToString();


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



        public FiscalYearDetailVM UpdateDetail(FiscalYearDetailVM model)
        {
            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update FiscalYearDetails set

 
 Year                    =@Year  
,MonthId                 =@MonthId  
,MonthName               =@MonthName  
,MonthStart              =@MonthStart   
,MonthEnd                =@MonthEnd   
,MonthLock               =@MonthLock   
,Remarks                 =@Remarks   
   
                       
where  Id= @Id ";



                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@Year", SqlDbType.Int).Value = model.Year;
                command.Parameters.Add("@MonthId", SqlDbType.Int).Value = model.MonthId;
                command.Parameters.Add("@MonthName", SqlDbType.NVarChar).Value = model.MonthName;
                command.Parameters.Add("@MonthStart", SqlDbType.NVarChar).Value = model.MonthStart;
                command.Parameters.Add("@MonthEnd", SqlDbType.NVarChar).Value = model.MonthEnd;
                command.Parameters.Add("@MonthLock", SqlDbType.Bit).Value = model.MonthLock;
                
                command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks;

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
