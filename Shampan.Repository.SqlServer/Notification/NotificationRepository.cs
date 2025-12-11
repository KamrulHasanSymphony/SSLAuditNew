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
using Shampan.Core.Interfaces.Repository.Notification;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.Notification
{
    public class NotificationRepository : Repository, INotificationRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

        public NotificationRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

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

        public List<Notifications> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"select 
 Id
,Code
,Description
,AuditId
,TeamId
,AdvanceDate
,AdvanceAmount
,IsPost
,CreatedBy
,CreatedOn
,CreatedFrom
,LastUpdateBy
,LastUpdateOn
,LastUpdateFrom

from  Advances
where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<Notifications> vms = dtResult.ToList<Notifications>();
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
            List<Notifications> VMs = new List<Notifications>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
select 
count(Advances.Id)FilteredCount

from Advances  where 1=1 ";


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

        public List<Notifications> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();

        }

        public List<Notifications> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();

        }

        public List<Notifications> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Notifications> VMs = new List<Notifications>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


SELECT 

 N.Id
,N.AuditName
,isnull(N.AuditId,0) AuditId
,isnull(N.IsAudit,0) IsAudit
,N.AuditUrl
,isnull(N.IssueId,0) IssueId
,N.IssueName
,isnull(N.IsIssue,0) IsIssue
,N.IssueUrl
,isnull(N.IsAuditApprove,0) IsAuditApprove
,N.AuditApproveUrl
,isnull(N.IsFeedback,0) IsFeedback
,isnull(N.FeedbackHeading,'FeedbackHeading') FeedbackHeading
,N.FeedbackUrl
,isnull(N.IsBranchFeedBack,0) IsBranchFeedBack
,isnull(N.BranchFeedbackHeading,'BranchFeedbackHeading') BranchFeedbackHeading
,N.BranchFeedbackUrl

FROM Notification N
 

CROSS APPLY STRING_SPLIT(N.UserId, ',') s


WHERE 1=1
";


                
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"AND LTRIM(RTRIM(s.value)) = @UserId";

                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                // Add UserId parameter
                objComm.SelectCommand.Parameters.AddWithValue("@UserId", index.UserId);

                objComm.Fill(dt);

                var req = new Notifications();

                VMs.Add(req);

                VMs = dt.ToList<Notifications>();

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
                from Advances  where 1=1 ";


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

        public Notifications Insert(Notifications model)
        {
            try
            {
                string sqlText = "";

                if (model.NotificationStatus == "Audit")
                {
                    var command = CreateCommand(@" INSERT INTO Notification(

 AuditName
,AuditId
,IsAudit
,AuditUrl
,UserId
,CreatedBy
,CreatedOn

) VALUES (

 @AuditName
,@AuditId
,@IsAudit
,@AuditUrl
,@UserId
,@CreatedBy
,@CreatedOn

)SELECT SCOPE_IDENTITY()");

                    
                    command.Parameters.Add("@AuditName", SqlDbType.NVarChar).Value = model.Name;
                    command.Parameters.Add("@IsAudit", SqlDbType.Bit).Value = 1;
                    command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;                 
                    command.Parameters.Add("@AuditUrl", SqlDbType.NVarChar).Value = model.URL;
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = model.UserId;
                    command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
                    command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
                    model.Id = Convert.ToInt32(command.ExecuteScalar());

                }


                else if (model.NotificationStatus == "Issue")
                {


                    var command = CreateCommand(@" INSERT INTO Notification(

 IssueId
,AuditId
,IsIssue
,IssueName
,IssueUrl
,CreatedBy
,CreatedOn
,UserId

) VALUES (

 @IssueId
,@AuditId
,@IsIssue
,@IssueName
,@IssueUrl
,@CreatedBy
,@CreatedOn
,@UserId

)SELECT SCOPE_IDENTITY()");
                   
                    command.Parameters.Add("@IssueId", SqlDbType.Int).Value = model.CommonId;
                    command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                    command.Parameters.Add("@IsIssue", SqlDbType.Bit).Value = 1;                    
                    command.Parameters.Add("@IssueName", SqlDbType.NVarChar).Value = model.Name;                  
                    command.Parameters.Add("@IssueUrl", SqlDbType.NVarChar).Value = model.URL;
                    command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
                    command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = model.UserId;

                    model.Id = Convert.ToInt32(command.ExecuteScalar());

                }

                else if (model.NotificationStatus == "AuditApprove")
                {
                    var command = CreateCommand(@" INSERT INTO Notification(

 AuditId
,IsAuditApprove
,AuditApproveUrl
,AuditName
,CreatedBy
,CreatedOn
,UserId

) VALUES (

 @AuditId
,@IsAuditApprove
,@AuditApproveUrl
,@AuditName
,@CreatedBy
,@CreatedOn
,@UserId

)SELECT SCOPE_IDENTITY()");


                    command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                    command.Parameters.Add("@IsAuditApprove", SqlDbType.Int).Value = true;
                    //command.Parameters.Add("@AuditApproveUrl", SqlDbType.NVarChar).Value = model.AuditApproveUrl;
                    command.Parameters.Add("@AuditApproveUrl", SqlDbType.NVarChar).Value = model.URL;
                    //command.Parameters.Add("@AuditName", SqlDbType.NVarChar).Value = model.AuditName;
                    command.Parameters.Add("@AuditName", SqlDbType.NVarChar).Value = model.Name;
                    command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
                    command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = model.UserId;


                    model.Id = Convert.ToInt32(command.ExecuteScalar());

                }

                else if (model.NotificationStatus == "Feedback")
                {
                    var command = CreateCommand(@" INSERT INTO Notification(

 AuditId
,FeedBackId
,IsFeedback
,FeedbackHeading
,FeedbackUrl
,UserId
,CreatedBy
,CreatedOn

) VALUES (

 @AuditId
,@FeedBackId
,@IsFeedback
,@FeedbackHeading
,@FeedbackUrl
,@UserId
,@CreatedBy
,@CreatedOn

)SELECT SCOPE_IDENTITY()");


                    command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                    command.Parameters.Add("@FeedBackId", SqlDbType.Int).Value = model.CommonId;
                    command.Parameters.Add("@IsFeedback", SqlDbType.Int).Value = true;
                    command.Parameters.Add("@FeedbackHeading", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(model.Name.ToString()) ? (object)DBNull.Value : model.Name.ToString();
                    command.Parameters.Add("@FeedbackUrl", SqlDbType.NVarChar).Value = model.URL;
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = model.UserId;
                    command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
                    command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
                    model.Id = Convert.ToInt32(command.ExecuteScalar());

                }

                else if (model.NotificationStatus == "BranchFeedback")
                {
                    var command = CreateCommand(@" INSERT INTO Notification(

 AuditId
,BranchFeedbackId
,IsBranchFeedBack
,BranchFeedbackHeading
,BranchFeedbackUrl
,UserId
,CreatedBy
,CreatedOn

) VALUES (

 @AuditId
,@BranchFeedbackId
,@IsBranchFeedBack
,@BranchFeedbackHeading
,@BranchFeedbackUrl
,@UserId
,@CreatedBy
,@CreatedOn

)SELECT SCOPE_IDENTITY()");


                    command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.AuditId;
                    command.Parameters.Add("@BranchFeedbackId", SqlDbType.Int).Value = model.CommonId;
                    command.Parameters.Add("@IsBranchFeedBack", SqlDbType.Int).Value = true;
                    command.Parameters.Add("@BranchFeedbackHeading", SqlDbType.NVarChar).Value = model.Name;
                    command.Parameters.Add("@BranchFeedbackUrl", SqlDbType.NVarChar).Value = model.URL;
                    command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = model.UserId;
                    command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();
                    command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();
                    model.Id = Convert.ToInt32(command.ExecuteScalar());

                }

                return model;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Notifications MultiplePost(Notifications objAdvances)
        {
            throw new NotImplementedException();

        }

        public Notifications Update(Notifications model)
        {
            try
            {
                string sqlText = "";
                int count = 0;

                if (model.NotificationStatus == "Audit" && model.IsAudit == true)
                {
                    string query = @"  UPDATE Notification set
 
 IsAudit                      =@IsAudit  
,LastUpdateBy                 =@LastUpdateBy  
,LastUpdateOn                 =@LastUpdateOn                     
 where  AuditId = @AuditId ";


                    SqlCommand command = CreateCommand(query);

                    command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.Id;
                    command.Parameters.Add("@IsAudit", SqlDbType.NChar).Value = false;
                    command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                    command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();

                    int rowcount = command.ExecuteNonQuery();

                    if (rowcount <= 0)
                    {
                        throw new Exception(MessageModel.UpdateFail);
                    }
                }


                else if (model.IsIssue = true && model.NotificationStatus == "issueNotification")
                {
                    string query = @"  UPDATE Notification set
 
 IsIssue                      =@IsIssue  
,LastUpdateBy                 =@LastUpdateBy  
,LastUpdateOn                 =@LastUpdateOn                     
 where  IssueId = @IssueId ";


                    SqlCommand command = CreateCommand(query);

                
                    command.Parameters.Add("@IssueId", SqlDbType.Int).Value = model.Id;
                    command.Parameters.Add("@IsIssue", SqlDbType.NChar).Value = false;
                    command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                    command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();

                    int rowcount = command.ExecuteNonQuery();

                    if (rowcount <= 0)
                    {
                        throw new Exception(MessageModel.UpdateFail);
                    }
                }

                else if (model.IsAuditApprove = true && model.NotificationStatus == "AuditApproved")
                {
                    string query = @"  UPDATE Notification set
 
 IsAuditApprove                      =@IsAuditApprove  
,LastUpdateBy                        =@LastUpdateBy  
,LastUpdateOn                        =@LastUpdateOn                     
 where  AuditId = @AuditId ";


                    SqlCommand command = CreateCommand(query);

                    command.Parameters.Add("@AuditId", SqlDbType.Int).Value = model.Id;
                    command.Parameters.Add("@IsAuditApprove", SqlDbType.NChar).Value = false;
                    command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                    command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();

                    int rowcount = command.ExecuteNonQuery();

                    if (rowcount <= 0)
                    {
                        throw new Exception(MessageModel.UpdateFail);
                    }
                }

                
                else if (model.IsFeedback = true && model.NotificationStatus == "feedbackNotification")
                {
                    string query = @"  UPDATE Notification set
 
 IsFeedback                          =@IsFeedback  
,LastUpdateBy                        =@LastUpdateBy  
,LastUpdateOn                        =@LastUpdateOn                     
 where  FeedBackId = @FeedBackId ";


                    SqlCommand command = CreateCommand(query);

                 
                    command.Parameters.Add("@FeedBackId", SqlDbType.Int).Value = model.Id;
                    command.Parameters.Add("@IsFeedback", SqlDbType.NChar).Value = false;
                    command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                    command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();

                    int rowcount = command.ExecuteNonQuery();

                    if (rowcount <= 0)
                    {
                        throw new Exception(MessageModel.UpdateFail);
                    }
                }


                else if (model.IsBranchFeedBack = true && model.NotificationStatus == "branchFeedbackNotification")
                {
                    string query = @"  UPDATE Notification set
 
 IsBranchFeedBack                    =@IsBranchFeedBack  
,LastUpdateBy                        =@LastUpdateBy  
,LastUpdateOn                        =@LastUpdateOn                     
 where  BranchFeedbackId = @BranchFeedbackId ";


                    SqlCommand command = CreateCommand(query);
                  
                    command.Parameters.Add("@BranchFeedbackId", SqlDbType.Int).Value = model.Id;
                    command.Parameters.Add("@IsBranchFeedBack", SqlDbType.NChar).Value = false;
                    command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateBy.ToString();
                    command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : model.Audit.LastUpdateOn.ToString();

                    int rowcount = command.ExecuteNonQuery();

                    if (rowcount <= 0)
                    {
                        throw new Exception(MessageModel.UpdateFail);
                    }
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
