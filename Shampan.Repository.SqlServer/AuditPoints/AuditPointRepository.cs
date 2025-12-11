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
using Shampan.Core.Interfaces.Repository.AuditPoints;
using Shampan.Core.Interfaces.Repository.CheckListItems;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.AuditPoints
{
	public class AuditPointRepository : Repository, IAuditPointRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public AuditPointRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<AuditPoint> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,PId
,ISNULL(AuditTypeId,0) AuditTypeId
,WeightPersent
,P_Level
,ISNULL(AuditType,'')AuditType

FROM  AuditPoints

where 1=1 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);
                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();

                adapter.Fill(dtResult);

                List<AuditPoint> vms = dtResult.ToList<AuditPoint>();
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
            List<AuditPoint> VMs = new List<AuditPoint>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(AuditPoints.Id)FilteredCount
                from AuditPoints  where 1=1 ";

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


        public List<AuditPoint> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<AuditPoint> VMs = new List<AuditPoint>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
PId,
AuditType,
WeightPersent,
--P_Mark,
P_Level

FROM AuditPoints 
where 1=1 

";



                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);


                if (index.PId != "xx" && index.PId != "")
                {
                    string Pid = index.PId;
                    sqlText += " AND AuditPoints.PId = @PID";
                }
                if (index.P_Level != "0" && index.PId != "")
                {
                    string P_Level = index.P_Level;
                    sqlText += " AND AuditPoints.P_Level = @P_Level";
                }

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

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
                var req = new AuditPoint();

                VMs.Add(req);


                VMs = dt.ToList<AuditPoint>();

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
            List<AuditPoint> VMs = new List<AuditPoint>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                FROM AuditPoints  where 1=1 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                if (index.PId != "xx" && index.PId != "")
                {
                    string Pid = index.PId;
                    sqlText += " AND AuditPoints.PId = @PID";
                }
                if (index.P_Level != "0" && index.PId != "")
                {
                    string P_Level = index.P_Level;
                    sqlText += " AND AuditPoints.P_Level = @P_Level";
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

		public AuditPoint Insert(AuditPoint model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO AuditPoints(

 PId
,AuditTypeId
,AuditType
,WeightPersent
,P_Level


) VALUES (

 @PId
,@AuditTypeId
,@AuditType
,@WeightPersent
,@P_Level


)SELECT SCOPE_IDENTITY()");

            
				command.Parameters.Add("@PId", SqlDbType.Int).Value = model.PId;			
				command.Parameters.Add("@AuditTypeId", SqlDbType.Int).Value = model.AuditTypeId;			
				command.Parameters.Add("@AuditType", SqlDbType.NVarChar).Value = model.AuditType;			
				command.Parameters.Add("@WeightPersent", SqlDbType.Int).Value = model.WeightPersent;						
				command.Parameters.Add("@P_Level", SqlDbType.Int).Value = model.P_Level;

				//command.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
				//command.Parameters.Add("@CreatedOn", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
				//command.Parameters.Add("@CreatedFrom", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();

                model.Id = Convert.ToInt32(command.ExecuteScalar());

				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public AuditPoint MultiplePost(AuditPoint objAuditPoint)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update AuditPoints set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                

                return objAuditPoint;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public AuditPoint MultipleUnPost(AuditPoint vm)
		{

            return new AuditPoint();
		
		}

		public AuditPoint Update(AuditPoint model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update AuditPoints set

 
 PId                          =@PId  
,AuditTypeId                    =@AuditTypeId  
,AuditType                    =@AuditType  
,WeightPersent                =@WeightPersent  
--,P_Mark                       =@P_Mark  
,P_Level                      =@P_Level   
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;

                command.Parameters.Add("@PId", SqlDbType.Int).Value = model.PId;
                command.Parameters.Add("@AuditTypeId", SqlDbType.NVarChar).Value = model.AuditTypeId;
                command.Parameters.Add("@AuditType", SqlDbType.NVarChar).Value = model.AuditType;
                command.Parameters.Add("@WeightPersent", SqlDbType.Int).Value = model.WeightPersent;
                //command.Parameters.Add("@P_Mark", SqlDbType.Int).Value =  model.P_Mark;
                command.Parameters.Add("@P_Level", SqlDbType.Int).Value = model.P_Level;
              
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
