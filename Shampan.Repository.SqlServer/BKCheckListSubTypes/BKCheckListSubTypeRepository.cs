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
using Shampan.Core.Interfaces.Repository.BKCheckListSubTypes;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.BKCheckListSubTypes
{
	public class BKCheckListSubTypeRepository : Repository, IBKCheckListSubTypeRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;       
        public BKCheckListSubTypeRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

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

        public List<BKCheckListSubType> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 
 Id
,Code
,BkCheckListTypesId
,Description
,Status

,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom


from  BKCheckListSubTypes

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<BKCheckListSubType> vms = dtResult.ToList<BKCheckListSubType>();
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
            List<BKCheckListSubType> VMs = new List<BKCheckListSubType>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(BKCheckListSubTypes.Id)FilteredCount
                from BKCheckListSubTypes  where 1=1 ";

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


        public List<BKCheckListSubType> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<BKCheckListSubType> VMs = new List<BKCheckListSubType>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
Select 
Id,
Code,
BkCheckListTypesId,
Description,
Status

from BKCheckListSubTypes 
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
                var req = new BKCheckListSubType();

                VMs.Add(req);


                VMs = dt.ToList<BKCheckListSubType>();

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
                from BKCheckListSubTypes  where 1=1 ";


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

		public BKCheckListSubType Insert(BKCheckListSubType model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKCheckListSubTypes(


 Code
,BkCheckListTypesId
,Description
,Status
,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (


 @Code
,@BkCheckListTypesId
,@Description
,@Status
,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");

				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;			
				command.Parameters.Add("@BkCheckListTypesId", SqlDbType.Int).Value = model.BkCheckListTypesId;
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

        public BKCheckListSubType MultiplePost(BKCheckListSubType objBKCheckListSubType)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update BKCheckListSubTypes set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom

    ,IsRejected=@IsRejected 

     where  Id= @Id ";

                foreach (string ID in objBKCheckListSubType.IDs)
                {
                    SqlCommand command = CreateCommand(query);
                    command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "Y";
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = ID;
                    command.Parameters.Add("@PostedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(objBKCheckListSubType.Audit.PostedBy.ToString()) ? (object)DBNull.Value : objBKCheckListSubType.Audit.PostedBy.ToString();
                    command.Parameters.Add("@PostedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(objBKCheckListSubType.Audit.PostedOn.ToString()) ? (object)DBNull.Value : objBKCheckListSubType.Audit.PostedOn.ToString();
                    command.Parameters.Add("@PostedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(objBKCheckListSubType.Audit.PostedFrom.ToString()) ? (object)DBNull.Value : objBKCheckListSubType.Audit.PostedFrom.ToString();


					command.Parameters.Add("@IsRejected", SqlDbType.Bit).Value = 0;


					rowcount = command.ExecuteNonQuery();
                }
                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }

                return objBKCheckListSubType;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public BKCheckListSubType MultipleUnPost(BKCheckListSubType vm)
		{

			try
			{
				string sqlText = "";

				int rowcount = 0;

				string query = @"  ";
				SqlCommand command = CreateCommand(query);
			
				foreach (string ID in vm.IDs)
				{
					if (vm.Operation == "unpost")
					{
						query = @"   update BKCheckListSubType set 

 IsPost=@Post                   
,ReasonOfUnPost=@ReasonOfUnPost
,LastUpdateBy=@LastUpdateBy
,LastUpdateOn=@LastUpdateOn
,LastUpdateFrom=@LastUpdateFrom

 where  Id= @Id ";
						command = CreateCommand(query);

						command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
						command.Parameters.Add("@Post", SqlDbType.NChar).Value = "N";
						command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedBy.ToString()) ? (object)DBNull.Value : vm.Audit.PostedBy.ToString();
						command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedOn.ToString()) ? (object)DBNull.Value : vm.Audit.PostedOn.ToString();
						command.Parameters.Add("@LastUpdateFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(vm.Audit.PostedFrom.ToString()) ? (object)DBNull.Value : vm.Audit.PostedFrom.ToString();


						rowcount = command.ExecuteNonQuery();

					}

					else if (vm.Operation == "reject")
					{

						query = @"update BKCheckListSubType set 

      IsPost=@IsPost  
     ,IsRejected=@IsRejected 
     ,RejectedBy=@RejectedBy  
     ,RejectedDate=@RejectedDate
     ,RejectedComments=@RejectedComments



     ,IsApprovedL1=@IsApprovedL1
     ,IsApprovedL2=@IsApprovedL2
     ,IsApprovedL3=@IsApprovedL3
     ,IsApprovedL4=@IsApprovedL4


      where  Id= @Id "
						;


						command = CreateCommand(query);

						command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
						command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "N";
					

						command.Parameters.Add("@IsApprovedL1", SqlDbType.NChar).Value = 0;
						command.Parameters.Add("@IsApprovedL2", SqlDbType.NChar).Value = 0;
						command.Parameters.Add("@IsApprovedL3", SqlDbType.NChar).Value = 0;
						command.Parameters.Add("@IsApprovedL4", SqlDbType.NChar).Value = 0;


						rowcount = command.ExecuteNonQuery();

					}


				}

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

		public BKCheckListSubType Update(BKCheckListSubType model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKCheckListSubTypes set

 Code                         =@Code  
,BkCheckListTypesId           =@BkCheckListTypesId  
,Description                  =@Description  
,Status                       =@Status  
,LastUpdateBy                 =@LastUpdateBy  
,LastUpdateOn                 =@LastUpdateOn  
,LastUpdateFrom               =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
				command.Parameters.Add("@Code", SqlDbType.NChar).Value = model.Code;
				command.Parameters.Add("@Description", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Description) ? (object)DBNull.Value : model.Description;				
				command.Parameters.Add("@BkCheckListTypesId", SqlDbType.Int).Value = model.BkCheckListTypesId;				
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
