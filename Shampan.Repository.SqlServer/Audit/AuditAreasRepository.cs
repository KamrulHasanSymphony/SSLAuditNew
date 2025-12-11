using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Audit;
using Shampan.Models;
using Shampan.Models.AuditModule;

namespace Shampan.Repository.SqlServer.Audit
{
    public class AuditAreasRepository: Repository, IAuditAreasRepository
    {

        private DbConfig _dbConfig;
        public AuditAreasRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }



        public List<AuditAreas> GDIC_GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditAreas> VMs = new List<AuditAreas>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT  
       [Id]
      ,[AuditId]
      ,[AuditArea]
      ,[AreaDetails]
      FROM [A_AuditAreas]

where 1=1 
and AuditId = @AuditId

";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditAreas>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GDIC_GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";

            DataTable dt = new DataTable();
            try
            {
                sqlText = @"
select
count(ad.ID) FilteredCount
from  A_AuditAreas ad 
where 1=1 and AuditId = @AuditId
";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);
                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditAreas> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"

SELECT 

       AA.[Id]
      ,AA.[AuditId]
      ,AA.[AuditTypeId]
      ,AA.[AuditPointId]
      ,AA.[WeightPersent]
      ,Isnull(AA.P_Mark,0)P_Mark
      ,AA.[P_Level]
      ,AP.[AuditType]
      ,AA.[AuditArea]
      ,AA.[AreaDetails]



  FROM [A_AuditAreas] AA

  Left outer join AuditPoints AP on AP.Id = AA.AuditPointId

WHERE 1=1 
";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<AuditAreas> vms = dtResult.ToList<AuditAreas>();
                return vms;

            }
            catch (Exception)
            {

                throw;
            };
        }

        public List<AuditAreas> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<AuditAreas> VMs = new List<AuditAreas>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT  
       AA.[Id]
      ,AA.[AuditId]
      ,AA.[WeightPersent]
      ,Isnull(AA.P_Mark,0)P_Mark
      ,Isnull(AA.P_Level,0)P_Level
      ,ISNULL(AP.[AuditType],'')AuditType
      

      FROM [A_AuditAreas] AA

      Left outer join AuditPoints AP on AP.Id = AA.AuditPointId

where 1=1 
and AA.AuditId = @AuditId

";


                if(index.AuditMarkLevel != "0")
                {
                    sqlText += " AND AA.P_Level=@P_Level";
                }

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);
                if (index.AuditMarkLevel != "0")
                {
                    objComm.SelectCommand.Parameters.AddWithValue("@P_Level", index.AuditMarkLevel);

                }

                objComm.Fill(dt);

                VMs = dt
                    .ToList<AuditAreas>();
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

            DataTable dt = new DataTable();
            try
            {
                sqlText = @"
select
count(ad.ID) FilteredCount
from  A_AuditAreas ad 
where 1=1 and AuditId = @AuditId
";


                if (index.AuditMarkLevel != "0")
                {
                    sqlText += " AND P_Level=@P_Level";
                }

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.SelectCommand.Parameters.AddWithValue("@AuditId", index.AuditId);

                if (index.AuditMarkLevel != "0")
                {
                    objComm.SelectCommand.Parameters.AddWithValue("@P_Level", index.AuditMarkLevel);
                }


                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0][0]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditAreas Insert(AuditAreas model)
        {
            try
            {
                string sqlText = "";
                int Id = 0;

                sqlText = @"
insert into A_AuditAreas(

  [AuditId]
 ,[AuditArea]
 ,[AreaDetails]

 ,[PID]
 ,[AuditPointId]
 ,[AuditTypeId]
 ,[WeightPersent]
 ,[P_Level]
 ,[AuditPointName]

)
values( 

  @AuditId
 ,@AuditArea
 ,@AreaDetails

 ,@PID
 ,@AuditPointId
 ,@AuditTypeId
 ,@WeightPersent
 ,@P_Level
 ,@AuditPointName

     
)     SELECT SCOPE_IDENTITY() ";


                var command = CreateCommand(sqlText);

                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@AuditArea", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.AuditArea) ? (object)DBNull.Value : model.AuditArea;               
                command.Parameters.Add("@AreaDetails", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.AreaDetails) ? (object)DBNull.Value : model.AreaDetails;               
                command.Parameters.Add("@PID", SqlDbType.Int).Value = string.IsNullOrWhiteSpace(model.PID?.ToString()) ? 0 : model.PID;
                command.Parameters.Add("@AuditPointId", SqlDbType.Int).Value = model.AuditPointId;
                command.Parameters.Add("@AuditTypeId", SqlDbType.Int).Value = model.AuditTypeId;
                command.Parameters.Add("@WeightPersent", SqlDbType.Int).Value = model.WeightPersent;
                command.Parameters.Add("@P_Level", SqlDbType.Int).Value = model.P_Level;
                command.Parameters.Add("@AuditPointName", SqlDbType.NVarChar).Value = model.AuditPointName;

                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public AuditAreas Update(AuditAreas model)
        {
            try
            {

                string sql = "";

                sql = @"

Update [A_AuditAreas]
set

AuditId=@AuditId
,P_Mark=@P_Mark
,AuditArea=@AuditArea
,AreaDetails=@AreaDetails

where Id=@Id
";


                var command = CreateCommand(sql);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
                command.Parameters.Add("@P_Mark", SqlDbType.Int).Value = model.P_Mark;
                command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                command.Parameters.Add("@AuditArea", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.AuditArea) ? (object)DBNull.Value : model.AuditArea;
                command.Parameters.Add("@AreaDetails", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.AreaDetails) ? (object)DBNull.Value : model.AreaDetails;
                

                int rowcount = Convert.ToInt32(command.ExecuteNonQuery());

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
