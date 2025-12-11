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
using Shampan.Core.Interfaces.Repository.BKAuditTemlateMasters;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKAuditTemlateMasters
{
	public class BKAuditTemlateMasterRepository : Repository, IBKAuditTemlateMasterRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKAuditTemlateMasterRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<BKAuditTemlateMaster> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,Code
,BKAuditOfficeTypeId
,BKAuditCategoryId
,Description
,Status

,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom


from  BKAuditTemlateMaster

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKAuditTemlateMaster> vms = dtResult.ToList<BKAuditTemlateMaster>();
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
            List<BKAuditTemlateMaster> VMs = new List<BKAuditTemlateMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKAuditTemlateMaster.Id)FilteredCount
                from BKAuditTemlateMaster  where 1=1 ";

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

        public List<BKAuditTemplateDetails> GetDetailsAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            //string SageDbName = _dbConfig.SageDbName;

            try
            {
                string sqlText = $@"


SELECT 

ATD.*

from  BKAuditTemplateDetails ATD

WHERE 1=1 

";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKAuditTemplateDetails> vms = dtResult.ToList<BKAuditTemplateDetails>();

                return vms;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CheckListItem> GetIndexCheckListItemData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<CheckListItem> VMs = new List<CheckListItem>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 

ISNULL(Id, 0) AS Id,
ISNULL(Description, '') AS Description,
ISNULL(IsFieldType, 0) AS IsFieldType,
ISNULL(Status, 0) AS Status,
ISNULL(BKCheckListSubTypesId, 0) AS BKCheckListSubTypesId

from BKCheckListItems 

where 1=1 

";



                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new CheckListItem();

                VMs.Add(req);


                VMs = dt.ToList<CheckListItem>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<BKAuditTemlateMaster> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKAuditTemlateMaster> VMs = new List<BKAuditTemlateMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
Code,
Description,
Status

from BKAuditTemlateMaster 
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
                var req = new BKAuditTemlateMaster();

                VMs.Add(req);


                VMs = dt.ToList<BKAuditTemlateMaster>();

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
            List<Teams> VMs = new List<Teams>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from BKAuditTemlateMaster  where 1=1 ";


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

        public List<MappingData> GetIndexMappingData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<MappingData> VMs = new List<MappingData>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


Select

--AOT.Id As BKAuditOfficeTypeId,
--AOT.Name As BKAuditOfficeTypeName,
ISNULL(AOT.Id, 0) AS BKAuditOfficeTypeId,
ISNULL(AOT.Name, '-') AS BKAuditOfficeTypeName,

--AC.Id As BKAuditComplianceId,
--AC.BKAuditOfficeTypesId AS BKAuditOfficeTypesId,
--AC.Description As BKAuditComplianceDes,
ISNULL(AC.Id, 0) AS BKAuditComplianceId,
ISNULL(AC.BKAuditOfficeTypesId, 0) AS BKAuditOfficeTypesId,
ISNULL(AC.Description, '-') AS BKAuditComplianceDes,



--CLT.Id As BKCheckListTypeId,
--CLT.BkAuditCompliancesId As BkAuditCompliancesId,
--CLT.Description As BKCheckListTypeDes,
ISNULL(CLT.Id, 0) AS BKCheckListTypeId,
ISNULL(CLT.BkAuditCompliancesId, 0) AS BkAuditCompliancesId,
ISNULL(CLT.Description, '-') AS BKCheckListTypeDes,


--CLST.Id As BKCheckListSubTypeId,
--CLST.BkCheckListTypesId As BkCheckListTypesId,
--CLST.Description As BKCheckListSubTypeDes
ISNULL(CLST.Id, 0) AS BKCheckListSubTypeId,
ISNULL(CLST.BkCheckListTypesId, 0) AS BkCheckListTypesId,
ISNULL(CLST.Description, '-') AS BKCheckListSubTypeDes


from BKAuditOfficeTypes AOT
left outer join BKAuditCompliances AC on AC.BKAuditOfficeTypesId = AOT.Id
left outer join BKCheckListTypes CLT on CLT.BkAuditCompliancesId = AC.Id
left outer join BKCheckListSubTypes CLST on CLST.BkCheckListTypesId = CLT.Id

where 1=1 

";



                
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new MappingData();

                VMs.Add(req);


                VMs = dt.ToList<MappingData>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetIndexMappingDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public int GetMappingCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public string GetSettingsValue(string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

		public BKAuditTemlateMaster Insert(BKAuditTemlateMaster model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKAuditTemlateMaster(


 Code
,BKAuditOfficeTypeId
,BKAuditCategoryId
,Description
,Status

,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@BKAuditOfficeTypeId
,@BKAuditCategoryId
,@Description
,@Status

,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");

				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;			
				command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;			
				command.Parameters.Add("@BKAuditCategoryId", SqlDbType.Int).Value = model.BKAuditCategoryId;			
				command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = model.Description;
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

        public BKAuditTemplateDetails InsertDetails(BKAuditTemplateDetails model)
        {
            try
            {
                string sqlText = "";

                var command = CreateCommand(@" INSERT INTO BKAuditTemplateDetails(

 BKAuditOfficeTypeId
,BKAuditTemlateMasterId
,BKAuditComplianceId
,BKCheckListTypeId
,BKCheckListSubTypeId
,BKCheckListItemId
,IsFieldType
,Status



) VALUES (


 @BKAuditOfficeTypeId
,@BKAuditTemlateMasterId
,@BKAuditComplianceId
,@BKCheckListTypeId
,@BKCheckListSubTypeId
,@BKCheckListItemId
,@IsFieldType
,@Status



)SELECT SCOPE_IDENTITY()");


                command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;
                command.Parameters.Add("@BKAuditTemlateMasterId", SqlDbType.Int).Value = model.BKAuditTemlateMasterId;
                command.Parameters.Add("@BKAuditComplianceId", SqlDbType.Int).Value = model.BKAuditComplianceId;
                command.Parameters.Add("@BKCheckListTypeId", SqlDbType.Int).Value = model.BKCheckListTypeId;
                command.Parameters.Add("@BKCheckListSubTypeId", SqlDbType.Int).Value = model.BKCheckListSubTypeId;
                command.Parameters.Add("@BKCheckListItemId", SqlDbType.Int).Value = model.BKCheckListItemId;
                command.Parameters.Add("@IsFieldType", SqlDbType.Bit).Value = model.IsFieldType;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;
               
                model.Id = Convert.ToInt32(command.ExecuteScalar());

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BKAuditTemlateMaster MultiplePost(BKAuditTemlateMaster objBKAuditTemlateMaster)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKAuditTemlateMaster set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

             

                return objBKAuditTemlateMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKAuditTemlateMaster MultipleUnPost(BKAuditTemlateMaster vm)
		{

			try
			{
				string sqlText = "";

				int rowcount = 0;

				string query = @"  ";
				SqlCommand command = CreateCommand(query);
			
				

				if (rowcount <= 0)
				{
					throw new Exception(MessageModel.PostFail);
				}
				return vm;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public BKAuditTemlateMaster Update(BKAuditTemlateMaster model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKAuditTemlateMaster set


 Code                                =@Code  
,BKAuditOfficeTypeId                 =@BKAuditOfficeTypeId   
,BKAuditCategoryId                   =@BKAuditCategoryId   
,Description                         =@Description   
,Status                              =@Status  

,LastUpdateBy                        =@LastUpdateBy  
,LastUpdateOn                        =@LastUpdateOn  
,LastUpdateFrom                      =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
				command.Parameters.Add("@BKAuditOfficeTypeId", SqlDbType.Int).Value = model.BKAuditOfficeTypeId;
				command.Parameters.Add("@BKAuditCategoryId", SqlDbType.Int).Value = model.BKAuditCategoryId;
				command.Parameters.Add("@Description", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Description) ? (object)DBNull.Value : model.Description;				
				command.Parameters.Add("@Status", SqlDbType.NChar).Value = model.Status;

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
