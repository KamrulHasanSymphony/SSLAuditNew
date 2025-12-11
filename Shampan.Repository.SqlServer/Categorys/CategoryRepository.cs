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
using Shampan.Core.Interfaces.Repository.Categorys;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.Categorys
{
	public class CategoryRepository : Repository, ICategoryRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;
        public CategoryRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
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

        public List<Category> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            try
            {
                string sqlText = @"select 

 Id
,Code
,CategoryName
,Status
,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom

from  BKAuditCategories

where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<Category> vms = dtResult.ToList<Category>();
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
            List<Category> VMs = new List<Category>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

                select count(Category.Id)FilteredCount
                from Category  where 1=1 ";

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

		public List<Category> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			string sqlText = "";
			List<Category> VMs = new List<Category>();
			DataTable dt = new DataTable();

			try
			{
				sqlText = $@"
declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

select @UserId=Id  from {AuthDbConfig.AuthDB}.dbo.AspNetUsers where UserName=@UserName

Create table #TempId(Id int)
select @A1=AdvanceApproval1,@A2=AdvanceApproval2,@A3=AdvanceApproval3,@A4=AdvanceApproval4  from UserRolls
where  IsAdvance=1 and UserId=@UserId


if(@A4=1)
begin
insert into  #TempId(Id)
select Id from Category where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1and IsApprovedL4=0
end
if(@A3=1)
begin
insert into  #TempId(Id)
 
select id from Category where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A2=1)
begin
insert into  #TempId(Id)
select id from Category where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A1=1)
begin
insert into  #TempId(Id)
select id from Category where IsRejected=0 and IsApprovedL1=0and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end


select 
                  Category.Id
                 ,Category.Code
                 ,Category.Description
                 ,Category.AuditId
                 ,Category.TeamId
                 ,Category.AdvanceDate
                 ,Category.AdvanceAmount
                 ,Category.IsPost
                 ,case 
				 when isnull(Category.IsRejected,0)=1 then 'Reject'
				 when isnull(Category.IsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(Category.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(Category.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(Category.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				 ,case when isnull(Category.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus
                 from Category 
  where 1=1 


and Category.id in( select id from #TempId)

";



				//sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
				sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

				// ToDo Escape Sql Injection
				sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
				sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                sqlText += @"  drop table #TempId  ";



                SqlDataAdapter objComm = CreateAdapter(sqlText);

				objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

				objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;


				objComm.Fill(dt);
				var req = new Category();

				VMs.Add(req);


				VMs = dt.ToList<Category>();

				return VMs;


			}
			catch (Exception e)
			{
				throw e;
			}
		}

        public List<Category> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Category> VMs = new List<Category>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = $@"
declare @A1 varchar(1);
declare @A2 varchar(1);
declare @A3 varchar(1);
declare @A4 varchar(1);
declare @UserId varchar(max);

select @UserId=Id  from {AuthDbConfig.AuthDB}.dbo.AspNetUsers where UserName=@UserName

Create table #TempId(Id int)
select @A1=AdvanceApproval1,@A2=AdvanceApproval1,@A3=AdvanceApproval1,@A4=AdvanceApproval1  from UserRolls
where  IsAdvance=1 and UserId=@UserId


if(@A4=1)
begin
insert into  #TempId(Id)
select Id from Category where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=1and IsApprovedL4=0
end
if(@A3=1)
begin
insert into  #TempId(Id)
 
select id from Category where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=1 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A2=1)
begin
insert into  #TempId(Id)
select id from Category where IsRejected=0 and IsApprovedL1=1and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end
if(@A1=1)
begin
insert into  #TempId(Id)
select id from Category where IsRejected=0 and IsApprovedL1=0and IsApprovedL2=0 and IsApprovedL3=0 and IsApprovedL4=0
end


select 
                  Category.Id
                 ,Category.Code
                 ,Category.Description
                 ,Category.AuditId
                 ,Category.TeamId
                 ,Category.AdvanceDate
                 ,Category.AdvanceAmount
                 ,Category.IsPost
                 ,case 
				 when isnull(Category.IsRejected,0)=1 then 'Reject'
				 when isnull(Category.IsApprovedL4,0)=1 then 'Approveed' 
				 when isnull(Category.IsApprovedL3,0)=1 then 'Waiting For Approval 4' 
				 when isnull(Category.IsApprovedL2,0)=1 then 'Waiting For Approval 3' 
				 when isnull(Category.IsApprovedL1,0)=1 then 'Waiting For Approval 2' 
				 else 'Waiting For Approval 1' 
				 end ApproveStatus
				 ,case when isnull(Category.IsAudited,0)=1 then 'Audited' else 'Not yet Audited' end AuditStatus
                 from Category 
  where 1=1 and Category.CreatedBy=@UserName


and Category.id in( select id from #TempId)

";



                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                sqlText += @"  drop table #TempId  ";



                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.SelectCommand.Parameters.Add("@UserName", SqlDbType.NChar).Value = index.UserName;


                objComm.Fill(dt);
                var req = new Category();

                VMs.Add(req);


                VMs = dt.ToList<Category>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public List<Category> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            string sqlText = "";
            List<Category> VMs = new List<Category>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

SELECT 
Id,
Code,
CategoryName,
Status,
CreatedBy,
CreatedOn,
LastUpdateBy,
LastUpdateBy,
LastUpdateOn

from BKAuditCategories 
where 1=1 

";



                
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new Category();

                VMs.Add(req);


                VMs = dt.ToList<Category>();

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
                from BKAuditCategories  where 1=1 ";


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

		public Category Insert(Category model)
		{
			try
			{
				string sqlText = "";

				var command = CreateCommand(@" INSERT INTO BKAuditCategories(


 Code
,CategoryName
,Status
,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (

 @Code
,@CategoryName
,@Status
,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");



				command.Parameters.Add("@Code", SqlDbType.NVarChar).Value = model.Code;			
				command.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = model.CategoryName;
				command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;

				command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
				command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
				command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : model.Audit.CreatedFrom.ToString();
				

                model.Id = Convert.ToInt32(command.ExecuteScalar());


				return model;

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public Category MultiplePost(Category objCategory)
        {
            try
            {
                string sqlText = "";

                int rowcount = 0;

                string query = @"  update Category set 

     IsPost=@IsPost                   
    ,PostedBy=@PostedBy
    ,PostedOn=@PostedOn
    ,PostedFrom=@PostedFrom


    ,IsRejected=@IsRejected 


     where  Id= @Id ";

                foreach (string ID in objCategory.IDs)
                {
                    SqlCommand command = CreateCommand(query);
                    command.Parameters.Add("@IsPost", SqlDbType.NChar).Value = "Y";
                    command.Parameters.Add("@Id", SqlDbType.BigInt).Value = ID;
                    command.Parameters.Add("@PostedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(objCategory.Audit.PostedBy.ToString()) ? (object)DBNull.Value : objCategory.Audit.PostedBy.ToString();
                    command.Parameters.Add("@PostedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(objCategory.Audit.PostedOn.ToString()) ? (object)DBNull.Value : objCategory.Audit.PostedOn.ToString();
                    command.Parameters.Add("@PostedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(objCategory.Audit.PostedFrom.ToString()) ? (object)DBNull.Value : objCategory.Audit.PostedFrom.ToString();


					command.Parameters.Add("@IsRejected", SqlDbType.Bit).Value = 0;


					rowcount = command.ExecuteNonQuery();
                }
                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }

                return objCategory;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public Category MultipleUnPost(Category vm)
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
						query = @"   update Category set 

 IsPost=@Post                   
,ReasonOfUnPost=@ReasonOfUnPost
,LastUpdateBy=@LastUpdateBy
,LastUpdateOn=@LastUpdateOn
,LastUpdateFrom=@LastUpdateFrom

 where  Id= @Id ";
						command = CreateCommand(query);

						command.Parameters.Add("@Id", SqlDbType.BigInt).Value = vm.Id;
						command.Parameters.Add("@Post", SqlDbType.NChar).Value = "N";
						
						rowcount = command.ExecuteNonQuery();

					}

					else if (vm.Operation == "reject")
					{

						query = @"update Category set 

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

		public Category Update(Category model)
		{
			try
			{
				string sqlText = "";
				int count = 0;

				string query = @"  update BKAuditCategories set


CategoryName                 =@CategoryName  
,Status                       =@Status  
,LastUpdateBy                 =@LastUpdateBy  
,LastUpdateOn                 =@LastUpdateOn  
,LastUpdateFrom               =@LastUpdateFrom 
                       
where  Id= @Id ";


				SqlCommand command = CreateCommand(query);

				command.Parameters.Add("@Id", SqlDbType.Int).Value = model.Id;
				command.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = model.CategoryName;
                command.Parameters.Add("@Status", SqlDbType.Bit).Value = model.Status;

                command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();
                command.Parameters.Add("@LastUpdateFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateFrom.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateFrom.ToString();

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
