using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.Repository.User;
using Shampan.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Deshboard;
using Shampan.Models.AuditModule;
using System.Reflection;
using SixLabors.ImageSharp.ColorSpaces;

namespace Shampan.Repository.SqlServer.Deshboard
{
    public class DeshboardRepository : Repository, IDeshboardRepository
    {

        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;

        public DeshboardRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }
        public List<AuditIssueUser> AuditBranchUserGetAll()
        {
            try
            {
                string sqlText = $@"select 
                 
                 AIU.Id
                ,AuditId
                ,AuditIssueId
                ,UserId
                ,EmailAddress 
				,ASP.UserName
                
                from  AuditIssueUsers AIU
                Left outer join {AuthDbConfig.AuthDB}.[dbo].[AspNetUsers] ASP on ASP.Id = AIU.UserId
                where 1=1";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, null, null);
                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<AuditIssueUser> vms = dtResult.ToList<AuditIssueUser>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public List<TeamMembers> AuditTeamMemberGetAll(string userId)
        {
            try
            {
                string sqlText = $@"select 
                 
                 TM.Id
                ,TeamId
                ,UserId
				,ASP.UserName
                
                from  A_TeamMembers TM
                Left outer join {AuthDbConfig.AuthDB}.[dbo].[AspNetUsers] ASP on ASP.Id = TM.UserId
                where 1=1 AND TM.UserId = @UserId";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, null, null);
                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();

                adapter.SelectCommand.Parameters.AddWithValue("@UserId", userId);


                adapter.Fill(dtResult);

                List<TeamMembers> vms = dtResult.ToList<TeamMembers>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int NotificationsCount(IndexModel model)
        {
            try
            {

                DataTable dt = new DataTable();


                string sqlText = $@"
SELECT 
COUNT(N.Id)Id

FROM Notification N

CROSS APPLY STRING_SPLIT(N.UserId, ',') s

WHERE 1=1 AND

(IsAudit=1 OR IsIssue=1 OR IsAuditApprove=1 OR IsFeedback=1 OR IsBranchFeedBack=1)

";

                sqlText = ApplyConditions(sqlText, null, null);

                sqlText += @"AND LTRIM(RTRIM(s.value)) = @UserId";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@UserId", model.UserId);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int BeforeDeadLineIssue(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"



SELECT COUNT(DISTINCT t.AuditIssueId) AS Id
FROM (
    SELECT *,
           ROW_NUMBER() OVER (
               PARTITION BY AuditIssueId 
               ORDER BY CreatedOn DESC
           ) AS rn
    FROM A_AuditBranchFeedbacks
    WHERE 
        CreatedOn >= @BeginingYear 
        AND CreatedOn <= @EndYear
        
) AS t
LEFT OUTER JOIN A_Audits A ON t.AuditId = A.Id
WHERE t.rn = 1 
  AND A.StartDate >= @BeginingYear 
  AND A.StartDate <= @EndYear
  AND t.CreatedOn <= t.DeadLineDate  

";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckExists(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public string CodeGeneration(string codeGroup, string codeName)
        {
            try
            {
                string codeGeneration = GenerateCode(codeGroup, codeName);
                return codeGeneration;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int DeadLineForResponse(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

SELECT
Count(AI.Id)Id

from A_AuditIssues AI
left outer join A_Audits A on AI.AuditId=A.Id
left outer join Enums E on E.Id=AI.IssuePriority

WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks) and AI.createdby = @UserName
";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);
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

        public int FinalAuidtApproved(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"
SELECT 
Count(A_Audits.Id)Id

FROM A_Audits
WHERE 1=1 AND A_Audits.IssueIsApprovedL4=1                
";

                sqlText += "AND StartDate >= @BeginingYear AND StartDate <= @EndYear";
                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int FollowUpAuditIssues(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

--SELECT 
--COUNT(A.Id)Id
--FROM A_AuditIssues AI
--LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
--WHERE 1=1 AND AI.IssueStatus != '1046' AND	 AI.CreatedOn >= @BeginingYear AND AI.CreatedOn <= @EndYear AND 
--CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
--AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks) AND AI.Id IN (SELECT AuditIssueId from AuditIssueUsers WHERE IsMailSend=1)

SELECT COUNT(DISTINCT t.AuditIssueId) AS Id
FROM (
    SELECT *,
           ROW_NUMBER() OVER (
               PARTITION BY AuditIssueId 
               ORDER BY CreatedOn DESC
           ) AS rn
    FROM A_AuditBranchFeedbacks
	WHERE 
1=1 AND CreatedOn >= @BeginingYear AND CreatedOn <= @EndYear
AND CAST(ImplementationDate AS date) < CAST(CreatedOn AS date)
) AS t
LEFT OUTER JOIN A_Audits A ON t.AuditId = A.Id
WHERE t.rn = 1
AND A.StartDate >= @BeginingYear AND A.StartDate <= @EndYear


";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);


                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditMaster> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = $@"select 

u.Id,
A.UserName,
U.UserId,
U.BranchId,
B.BranchName

from {AuthDbConfig.AuthDB}.[dbo]. AspNetUsers A
left outer join UserBranchMap U on U.UserId=A.Id
left outer join BranchProfiles B on B.BranchId=U.BranchId

WHERE U.UserId IS NOT NULL";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResutl = new DataTable();
                adapter.Fill(dtResutl);

                List<AuditMaster> vms = dtResutl.ToList<AuditMaster>();
                return vms;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public UserBranch GetBranchName(string UserId)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            List<UserBranch> vms = new List<UserBranch>();

            try
            {

                sqlText = $@"

                 SELECT BranchName
                 from 
                 {AuthDbConfig.AuthDB}.[dbo].AspNetUsers 
                  WHERE Id =@Id

                ";


                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@Id", UserId);
                objComm.Fill(dt);

                vms = dt.ToList<UserBranch>();
                return vms.FirstOrDefault();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(UserBranchMap.UserId)FilteredCount
                from UserBranchMap  where 1=1 ";


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


        public List<AuditMaster> GetIndexData(IndexModel Index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = $@"Select
u.Id,
A.UserName,

U.UserId,
U.BranchId,
B.BranchName

from  {AuthDbConfig.AuthDB}.[dbo]. AspNetUsers A
left outer join UserBranchMap U on U.UserId=A.Id
left outer join BranchProfiles B on B.BranchId=U.BranchId
WHERE U.UserId IS NOT NULL";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + Index.OrderName + "  " + Index.orderDir;
                sqlText += @" OFFSET  " + Index.startRec + @" ROWS FETCH NEXT " + Index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new AuditMaster();

                VMs.Add(req);


                VMs = dt
                .ToList<AuditMaster>();

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
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from UserBranchMap  where 1=1 ";


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

        public PrePaymentMaster GetPrePayment()
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            List<PrePaymentMaster> VMs = new List<PrePaymentMaster>();
            try
            {
                sqlText = @"

    SELECT    
    FORMAT(COALESCE(SUM(0), 0), '0.00') AS PreviousAmount,
    FORMAT(COALESCE(count(Id), 0), '0.00') AS CorrectedAmount,
    --FORMAT(COALESCE(SUM(AdditionalPayment), 0), '0.00') AS AdditionalPayment 
    FORMAT(COALESCE(SUM(ABS(AdditionalPayment)), 0), '0.00') AS AdditionalPayment
    FROM 
    Financial WHERE 1=1
    ";

                sqlText += "AND CreatedOn >= @BeginingYear AND CreatedOn <= @EndYear";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                VMs = dt.ToList<PrePaymentMaster>();
                return VMs.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public List<AuditComponent> GetUnPlanAuditComponents()
        {
            string sqlText = "";
            List<AuditComponent> VMs = new List<AuditComponent>();
            DataTable dt = new DataTable();
            try
            {
                sqlText = @"



SELECT
    AT.AuditType AS [Name],
    COUNT(CASE WHEN a.IsPlaned = 0 THEN 1 ELSE NULL END) AS BranchPlan,
    COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) AS BranchOngoin,
    COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END) AS BranchCompleted,
    (COUNT(CASE WHEN a.IsPlaned = 0 THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)) AS BranchRemaining,
    (CASE
        WHEN (COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) + COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)) = 0 THEN 0
        ELSE (100 * COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END) / (COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) + COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)))
    END) AS [% (Completed + Ongoing)]
FROM   
    AuditTypes AS AT
LEFT JOIN  
    A_Audits AS a ON AT.Id = a.AuditTypeId
	  WHERE a.IsPlaned=0 AND StartDate >= @BeginingYear AND StartDate <= @EndYear
GROUP BY
    AT.AuditType


                ";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);

                VMs = dt.ToList<AuditComponent>();

                foreach (AuditComponent audit in VMs)
                {
                    audit.BranchCompletedOngoing = ((audit.BranchCompleted + audit.BranchOngoin) / audit.BranchPlan) * 100;
                }

                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public AuditMaster Insert(AuditMaster model)
        {
            try
            {
                string selectQuery = @"SELECT COUNT(*) FROM UserBranchMap WHERE UserId = @UserId AND BranchId = @BranchId";
                var selectCommand = CreateCommand(selectQuery);
                selectCommand.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;

                int count = Convert.ToInt32(selectCommand.ExecuteScalar());
                if (count > 0)
                {
                    throw new Exception("User ID has already been assigned to the branch");
                }

                var command = CreateCommand(@" INSERT INTO UserBranchMap(

 UserId
,BranchId
,CreatedBy
,CreatedOn
,CreatedFrom

) VALUES (

 @UserId
,@BranchId
,@CreatedBy
,@CreatedOn
,@CreatedFrom

)SELECT SCOPE_IDENTITY()");


                command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
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

        public int MissDeadLineIssues(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"


SELECT COUNT(DISTINCT t.AuditIssueId) AS Id
FROM (
    SELECT *,
           ROW_NUMBER() OVER (
               PARTITION BY AuditIssueId 
               ORDER BY CreatedOn DESC
           ) AS rn
    FROM A_AuditBranchFeedbacks
    WHERE 
        CreatedOn >= @BeginingYear
        AND CreatedOn <= @EndYear
        
) AS t
LEFT OUTER JOIN A_Audits A ON t.AuditId = A.Id
WHERE t.rn = 1 
  AND A.StartDate >= @BeginingYear 
  AND A.StartDate <= @EndYear
  AND t.CreatedOn >= t.DeadLineDate  


             ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<PrePaymentMaster> NonFinancialGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {

            try
            {
                string sqlText = @"select 
                Id
                ,Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,Format(FinalCorrectionDate , 'yyyy-MM-dd') FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks

 FROM  NonFinancial
 where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<PrePaymentMaster> vms = dtResult.ToList<PrePaymentMaster>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PrePaymentMaster> NonFinancialGetGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public List<PrePaymentMaster> NonFinancialGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<PrePaymentMaster> VMs = new List<PrePaymentMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 SELECT 
                 Id
                ,Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,Format(FinalCorrectionDate , 'yyyy-MM-dd') FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks
     
      FROM NonFinancial 

      where 1=1 
    
";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt
                    .ToList<PrePaymentMaster>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int NonFinancialGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Teams> VMs = new List<Teams>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
select 
count(Id)FilteredCount

FROM NonFinancial  where 1=1 ";

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

        public PrePaymentMaster NonFinancialInsert(PrePaymentMaster objMaster)
        {
            try
            {
                string sqlText = "";
                int count = 0;


                var command = CreateCommand(@" INSERT INTO NonFinancial(

                 Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks
                
                ) VALUES (

                 @Code
                ,@Auditor
                ,@EmployeeName
                ,@Details 
                ,@FinalCorrectionDate 
                ,@PreviousAmount
                ,@CorrectedAmount
                ,@AdditionalPayment
                ,@PaymentMemoReferenceNo
                ,@Department
                ,@Remarks
                
                )SELECT SCOPE_IDENTITY()");


                command.Parameters.Add("@Code", SqlDbType.VarChar).Value = objMaster.Code;
                command.Parameters.Add("@Auditor", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Auditor) ? (object)DBNull.Value : objMaster.Auditor;
                command.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.EmployeeName) ? (object)DBNull.Value : objMaster.EmployeeName;
                command.Parameters.Add("@Details", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Details) ? (object)DBNull.Value : objMaster.Details;
                command.Parameters.Add("@FinalCorrectionDate", SqlDbType.DateTime).Value = objMaster.FinalCorrectionDate;
                command.Parameters.Add("@PreviousAmount", SqlDbType.Decimal).Value = objMaster.PreviousAmount;
                command.Parameters.Add("@CorrectedAmount", SqlDbType.Decimal).Value = objMaster.CorrectedAmount;
                command.Parameters.Add("@AdditionalPayment", SqlDbType.Decimal).Value = objMaster.AdditionalPayment;
                command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.PaymentMemoReferenceNo) ? (object)DBNull.Value : objMaster.PaymentMemoReferenceNo;
                command.Parameters.Add("@Department", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Department) ? (object)DBNull.Value : objMaster.Department;
                command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Remarks) ? (object)DBNull.Value : objMaster.Remarks;

                objMaster.Id = Convert.ToInt32(command.ExecuteScalar());
                return objMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PrePaymentMaster NonFinancialUpdate(PrePaymentMaster objMaster)
        {
            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update NonFinancial set

 Auditor                        =@Auditor  
,EmployeeName                   =@EmployeeName  
,Details                        =@Details   
,FinalCorrectionDate            =@FinalCorrectionDate  
,PreviousAmount                 =@PreviousAmount  
,CorrectedAmount                =@CorrectedAmount  
,AdditionalPayment              =@AdditionalPayment  
,PaymentMemoReferenceNo         =@PaymentMemoReferenceNo  
,Department                     =@Department  
,Remarks                        =@Remarks  
                     
WHERE  Id= @Id ";


                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = objMaster.Id;
                var item = objMaster.NonFinancialDetails.FirstOrDefault();
                command.Parameters.Add("@Auditor", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Auditor) ? (object)DBNull.Value : item.Auditor;
                command.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.EmployeeName) ? (object)DBNull.Value : item.EmployeeName;
                command.Parameters.Add("@Details", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Details) ? (object)DBNull.Value : item.Details;
                command.Parameters.Add("@FinalCorrectionDate", SqlDbType.DateTime).Value = item.FinalCorrectionDate;
                command.Parameters.Add("@PreviousAmount", SqlDbType.Decimal).Value = item.PreviousAmount;
                command.Parameters.Add("@CorrectedAmount", SqlDbType.Decimal).Value = item.CorrectedAmount;
                command.Parameters.Add("@AdditionalPayment", SqlDbType.Decimal).Value = item.AdditionalPayment;
                command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.PaymentMemoReferenceNo) ? (object)DBNull.Value : item.PaymentMemoReferenceNo;
                command.Parameters.Add("@Department", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Department) ? (object)DBNull.Value : item.Department;
                command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Remarks) ? (object)DBNull.Value : item.Remarks;

                int rowcount = command.ExecuteNonQuery();

                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }
                return objMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int PendingAuditApproval(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = $@"

DECLARE @A1 VARCHAR(1);
DECLARE @A2 VARCHAR(1);
DECLARE @A3 VARCHAR(1);
DECLARE @A4 VARCHAR(1);
DECLARE @UserId VARCHAR(MAX);

SELECT @UserId = Id FROM {AuthDbConfig.AuthDB}.dbo.AspNetUsers WHERE UserName = @UserName;

CREATE TABLE #TempId(Id INT);

SELECT @A1 = AuditApproval1, @A2 = AuditApproval2, @A3 = AuditApproval3, @A4 = AuditApproval4  
FROM UserRolls
WHERE IsAudit = 1 AND UserId = @UserId;

IF (@A4 = 1)
BEGIN
    INSERT INTO #TempId(Id)
    SELECT Id FROM A_Audits WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 1 AND IsApprovedL3 = 1 AND IsApprovedL4 = 0;
END

IF (@A3 = 1)
BEGIN
    INSERT INTO #TempId(Id)
    SELECT Id FROM A_Audits WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 1 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END

IF (@A2 = 1)
BEGIN
    INSERT INTO #TempId(Id)
    SELECT Id FROM A_Audits WHERE IsRejected = 0 AND IsApprovedL1 = 1 AND IsApprovedL2 = 0 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END

IF (@A1 = 1)
BEGIN
    INSERT INTO #TempId(Id)
    SELECT Id FROM A_Audits WHERE IsRejected = 0 AND IsApprovedL1 = 0 AND IsApprovedL2 = 0 AND IsApprovedL3 = 0 AND IsApprovedL4 = 0;
END

SELECT COUNT(*) AS Id
FROM (
    SELECT 
        ad.Id,
        ad.[Code],
        ISNULL(ad.[Name], '') AS Name,
        FORMAT(ad.[StartDate], 'yyyy-MM-dd') AS StartDate,
        FORMAT(ad.[EndDate], 'yyyy-MM-dd') AS EndDate,
        CASE 
            WHEN ISNULL(ad.IsRejected, 0) = 1 THEN 'Reject'
            WHEN ISNULL(ad.IsApprovedL4, 0) = 1 THEN 'Approved' 
            WHEN ISNULL(ad.IsApprovedL3, 0) = 1 THEN 'Waiting For Approval 4' 
            WHEN ISNULL(ad.IsApprovedL2, 0) = 1 THEN 'Waiting For Approval 3' 
            WHEN ISNULL(ad.IsApprovedL1, 0) = 1 THEN 'Waiting For Approval 2' 
            ELSE 'Waiting For Approval 1' 
        END AS ApproveStatus,
        CASE 
            WHEN ISNULL(ad.IsAudited, 0) = 1 THEN 'Audited' 
            ELSE 'Not yet Audited' 
        END AS AuditStatus,
        ad.[IsPost]
    FROM A_Audits ad 
    WHERE ad.Id IN (SELECT Id FROM #TempId)
    AND ad.IsPost = 'Y' AND StartDate >= @BeginingYear AND StartDate <= @EndYear
) AS SubQuery;

DROP TABLE #TempId;
                 
                ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int PendingAuditResponse(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

                 SELECT 
                 COUNT(DISTINCT(Id)) Id
                 From A_AuditIssues
                 WHERE IsFeedbackForIssue = @IsFeedbackForIssue
 
                ";

                sqlText += "AND CreatedOn >= @BeginingYear AND CreatedOn <= @EndYear";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@IsFeedbackForIssue", 1);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int PendingForApproval(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

                 SELECT Count(A_Audits.Id)Id
                 FROM A_Audits
                 WHERE A_Audits.CreatedBy=@UserName and A_Audits.IsApprovedL4 != 1
                ";

                sqlText += "AND StartDate >= @BeginingYear AND StartDate <= @EndYear";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int PendingForAuditFeedback(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

             select 
             count(A.Id)Id

             from A_AuditIssues AI

             left outer join A_Audits A on AI.AuditId=A.Id
             left outer join Enums E on E.Id=AI.IssuePriority

             WHERE 1=1 and AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	         and AI.Id not in(select AuditIssueId from A_AuditBranchFeedbacks) 
                ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int PendingForIssueApproval(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = $@"

declare @A1 varchar(1);
DECLARE @A2 varchar(1);
DECLARE @A3 varchar(1);
DECLARE @A4 varchar(1);
DECLARE @UserId varchar(max);

SELECT @UserId=Id FROM {AuthDbConfig.AuthDB}.dbo.AspNetUsers WHERE UserName=@UserName

Create TABLE #TempId(Id int)
SELECT @A1=AuditIssueApproval1,@A2=AuditIssueApproval2,@A3=AuditIssueApproval3,@A4=AuditIssueApproval4 from UserRolls
WHERE IsAuditIssue=1 and UserId=@UserId


if(@A4=1)
begin
    INSERT into  #TempId(Id)
    SELECT Id from A_Audits WHERE IsApprovedL4=1 AND isnull(IssueIsRejected,0)=0 and IssueIsApprovedL1=1 AND IssueIsApprovedL2=1 AND IssueIsApprovedL3=1 AND IssueIsApprovedL4=0
end
if(@A3=1)
begin
    INSERT into  #TempId(Id)
    SELECT id from A_Audits WHERE IsApprovedL4=1 AND IssueIsRejected=0 AND IssueIsApprovedL1=1 AND IssueIsApprovedL2=1 AND IssueIsApprovedL3=0 AND IssueIsApprovedL4=0
end
if(@A2=1)
begin
    INSERT into  #TempId(Id)
    SELECT id from A_Audits WHERE IsApprovedL4=1 AND IssueIsRejected=0 AND IssueIsApprovedL1=1 AND IssueIsApprovedL2=0 AND IssueIsApprovedL3=0 AND IssueIsApprovedL4=0
end
if(@A1=1)
begin
    INSERT into  #TempId(Id)
    SELECT id from A_Audits WHERE IsApprovedL4=1 AND IssueIsRejected=0 AND IssueIsApprovedL1=0 AND IssueIsApprovedL2=0 AND IssueIsApprovedL3=0 AND IssueIsApprovedL4=0
end

SELECT COUNT(*) AS TotalCount FROM (
    SELECT 
        ad.Id,
        ad.[Code],
        ISNULL(ad.[Name], '') AS Name,
        FORMAT(ad.[StartDate], 'yyyy-MM-dd') AS StartDate,
        FORMAT(ad.[EndDate], 'yyyy-MM-dd') AS EndDate,
        CASE 
            WHEN ISNULL(ad.IssueIsRejected, 0) = 1 THEN 'Reject'
            WHEN ISNULL(ad.IssueIsApprovedL4, 0) = 1 THEN 'Approveed' 
            WHEN ISNULL(ad.IssueIsApprovedL3, 0) = 1 THEN 'Waiting For Approval 4' 
            WHEN ISNULL(ad.IssueIsApprovedL2, 0) = 1 THEN 'Waiting For Approval 3' 
            WHEN ISNULL(ad.IssueIsApprovedL1, 0) = 1 THEN 'Waiting For Approval 2' 
            ELSE 'Waiting For Approval 1' 
        END AS ApproveStatus,
        CASE 
            WHEN ISNULL(ad.IsAudited, 0) = 1 THEN 'Audited' 
            ELSE 'Not yet Audited' 
        END AS AuditStatus,
        ad.[IsPost]
    FROM 
        A_Audits ad 
    WHERE 
        IsApprovedL4 = 1 
        AND IsCompleteIssueBranchFeedback = 1
        AND ad.id IN (SELECT id FROM #TempId)
        AND ad.IsPost = 'Y' AND StartDate >= @BeginingYear AND StartDate <= @EndYear
    ORDER BY  
        Id ASC
    OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY
) AS subquery

DROP TABLE #TempId

             ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["TotalCount"]);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int PendingForReviewerFeedback(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            try
            {
                sqlText = $@"

             SELECT COUNT(A.Id)Id
             from A_AuditIssues a 

             left join AuditUsers AU on a.AuditId=au.AuditId
             left outer join {AuthDbConfig.AuthDB}.[dbo].[AspNetUsers] ANU on ANU.Id = AU.UserId

             left outer join A_Audits AA on A.AuditId=AA.Id

             where AA.IsCompleteIssueTeamFeedback=0 AND a.Id not  in (SELECT af.AuditIssueId from A_AuditFeedbacks af where af.CreatedBy=@UserName ) AND  
             ANU.UserName = @UserName AND a.CreatedOn >= @BeginingYear AND a.CreatedOn <= @EndYear AND  AU.TeamId in(0,null)

          ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public int PendingForReviewerFeedbackForTeam(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            try
            {
                sqlText = $@"

             SELECT 
Count(F.Id) Id
FROM A_AuditFeedbacks F
INNER JOIN A_Audits A on A.Id =  F.AuditId
WHERE  IsCompleteIssueTeamFeedback = 0
";

                sqlText += "AND F.CreatedOn >= @BeginingYear AND F.CreatedOn <= @EndYear";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PrePaymentMaster> PrePaymentGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"select 
                Id
                ,Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,Format(FinalCorrectionDate , 'yyyy-MM-dd') FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks

                from  Financial
                 where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);
                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);
                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<PrePaymentMaster> vms = dtResult.ToList<PrePaymentMaster>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<PrePaymentMaster> PrePaymentGetGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public List<PrePaymentMaster> PrePaymentGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<PrePaymentMaster> VMs = new List<PrePaymentMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                SELECT 
                 Id
                ,Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,Format(FinalCorrectionDate , 'yyyy-MM-dd') FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks
     
                FROM Financial 

      where 1=1 
    
 
";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.Fill(dt);
                VMs = dt
                    .ToList<PrePaymentMaster>();
                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int PrePaymentGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Teams> VMs = new List<Teams>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(Id)FilteredCount
                from Financial  where 1=1 ";


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

        public PrePaymentMaster PrePaymentInsert(PrePaymentMaster objMaster)
        {
            try
            {
                string sqlText = "";
                int count = 0;
                var command = CreateCommand(@" INSERT INTO Financial(
                 Code
                ,Auditor
                ,EmployeeName
                ,Details 
                ,FinalCorrectionDate
                ,PreviousAmount
                ,CorrectedAmount
                ,AdditionalPayment
                ,PaymentMemoReferenceNo
                ,Department
                ,Remarks
                ,CreatedOn
                ,CreatedBy
                
                ) VALUES (

                 @Code
                ,@Auditor
                ,@EmployeeName
                ,@Details 
                ,@FinalCorrectionDate 
                ,@PreviousAmount
                ,@CorrectedAmount
                ,@AdditionalPayment
                ,@PaymentMemoReferenceNo
                ,@Department
                ,@Remarks
                ,@CreatedOn
                ,@CreatedBy
                

                )SELECT SCOPE_IDENTITY()");

                command.Parameters.Add("@Code", SqlDbType.VarChar).Value = objMaster.Code;
                command.Parameters.Add("@Auditor", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Auditor) ? (object)DBNull.Value : objMaster.Auditor;
                command.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.EmployeeName) ? (object)DBNull.Value : objMaster.EmployeeName;
                command.Parameters.Add("@Details", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Details) ? (object)DBNull.Value : objMaster.Details;
                command.Parameters.Add("@FinalCorrectionDate", SqlDbType.DateTime).Value = objMaster.FinalCorrectionDate;
                command.Parameters.Add("@PreviousAmount", SqlDbType.Decimal).Value = objMaster.PreviousAmount;
                command.Parameters.Add("@CorrectedAmount", SqlDbType.Decimal).Value = objMaster.CorrectedAmount;
                command.Parameters.Add("@AdditionalPayment", SqlDbType.Decimal).Value = objMaster.AdditionalPayment;
                command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.PaymentMemoReferenceNo) ? (object)DBNull.Value : objMaster.PaymentMemoReferenceNo;
                command.Parameters.Add("@Department", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Department) ? (object)DBNull.Value : objMaster.Department;
                command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objMaster.Remarks) ? (object)DBNull.Value : objMaster.Remarks;
                command.Parameters.Add("@CreatedOn", SqlDbType.VarChar).Value = objMaster.Audit.CreatedOn;
                command.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = objMaster.Audit.CreatedBy;

                objMaster.Id = Convert.ToInt32(command.ExecuteScalar());
                return objMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PrepaymentReview PrepaymentReview()
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            List<PrepaymentReview> VMs = new List<PrepaymentReview>();
            try
            {
                sqlText = @"

    SELECT top(1)    
    Id,
    Value
    FROM PrepaymentReview
    WHERE 1=1 AND CreatedOn >= @BeginingYear AND CreatedOn <= @EndYear
    Order By Id Desc
    ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);


                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);


                objComm.Fill(dt);
                VMs = dt.ToList<PrepaymentReview>();
                return VMs.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PrepaymentReview PrepaymentReviewInsert(PrepaymentReview objMaster)
        {
            try
            {

                var command = CreateCommand(@" 
INSERT INTO PrepaymentReview(

 Value
,CreatedBy
,CreatedOn
,CreatedFrom

) VALUES (

 @Value
,@CreatedBy
,@CreatedOn
,@CreatedFrom

)SELECT SCOPE_IDENTITY()");

                command.Parameters.Add("@Value", SqlDbType.Decimal).Value = objMaster.Value;
                command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedBy.ToString();
                command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedOn.ToString();
                command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedFrom.ToString();
                objMaster.Id = Convert.ToInt32(command.ExecuteScalar());
                return objMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PrePaymentMaster PrePaymentUpdate(PrePaymentMaster objMaster)
        {
            try
            {
                string sqlText = "";
                int count = 0;
                string query = @"  update Financial set

 Auditor                        =@Auditor  
,EmployeeName                   =@EmployeeName  
,Details                        =@Details   
,FinalCorrectionDate            =@FinalCorrectionDate  
,PreviousAmount                 =@PreviousAmount  
,CorrectedAmount                =@CorrectedAmount  
,AdditionalPayment              =@AdditionalPayment  
,PaymentMemoReferenceNo         =@PaymentMemoReferenceNo  
,Department                     =@Department  
,Remarks                        =@Remarks                    
 where  Id= @Id ";


                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = objMaster.Id;
                var item = objMaster.PrePaymentDetails.FirstOrDefault();
                command.Parameters.Add("@Auditor", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Auditor) ? (object)DBNull.Value : item.Auditor;
                command.Parameters.Add("@EmployeeName", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.EmployeeName) ? (object)DBNull.Value : item.EmployeeName;
                command.Parameters.Add("@Details", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Details) ? (object)DBNull.Value : item.Details;
                command.Parameters.Add("@FinalCorrectionDate", SqlDbType.DateTime).Value = item.FinalCorrectionDate;
                command.Parameters.Add("@PreviousAmount", SqlDbType.Decimal).Value = item.PreviousAmount;
                command.Parameters.Add("@CorrectedAmount", SqlDbType.Decimal).Value = item.CorrectedAmount;
                command.Parameters.Add("@AdditionalPayment", SqlDbType.Decimal).Value = item.AdditionalPayment;
                command.Parameters.Add("@PaymentMemoReferenceNo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.PaymentMemoReferenceNo.Trim()) ? (object)DBNull.Value : item.PaymentMemoReferenceNo.Trim();
                command.Parameters.Add("@Department", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Department) ? (object)DBNull.Value : item.Department;
                command.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(item.Remarks) ? (object)DBNull.Value : item.Remarks;

                int rowcount = command.ExecuteNonQuery();
                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }
                return objMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalAdditionalPaymentCount()
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {

                sqlText = @"


SELECT 
SUM(ABS(AdditionalPayment)) AS AdditionalPayment
FROM Financial

";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["AdditionalPayment"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalAudit(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

                  SELECT Count(A_Audits.Id)Id 
                  FROM A_Audits 
                  where 1=1  

                ";

                sqlText += "AND StartDate >= @BeginingYear AND StartDate <= @EndYear";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);
                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalAuditApproved(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

                  SELECT Count(A_Audits.Id)Id 
                  FROM A_Audits
                  where A_Audits.ApprovedByL4=@UserName and A_Audits.IsApprovedL4=1
                  AND 1=1
                ";

                sqlText += "AND StartDate >= @BeginingYear AND StartDate <= @EndYear";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalAuditRejected(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

                  SELECT Count(A_Audits.Id)Id
                  FROM A_Audits
                  WHERE A_Audits.IsRejected = 1
                  AND 1=1

                ";

                sqlText += "AND StartDate >= @BeginingYear AND StartDate <= @EndYear";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditReports> TotalCompletedOngoingRemaing(string UserName)
        {
            string sqlText = "";
            List<AuditReports> VMs = new List<AuditReports>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


SELECT
 
    COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) AS Ongoing,
    COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END) AS Completed,
    (COUNT(CASE WHEN a.IsPlaned = 1 THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 1 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 1 ELSE NULL END)) AS Remaining

FROM 

      [AuditTypes] AS AT

LEFT JOIN

      [A_Audits] AS a ON AT.Id = a.AuditTypeId

WHERE 1=1 AND a.IsPlaned = 1 

                ";

                sqlText += "AND StartDate >= @BeginingYear AND StartDate <= @EndYear";


                //sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);
                // ToDo Escape Sql Injection
                //sqlText += @"  order by  " + index.OrderName.Replace("AuditName", "ad.Name") + "  " + index.orderDir;
                //sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);

                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);



                objComm.Fill(dt);

                VMs = dt.ToList<AuditReports>();


                var item = VMs.FirstOrDefault();
                decimal[] arr = new decimal[3];
                arr[0] = item.Completed;
                arr[1] = item.Ongoing;
                arr[2] = item.Remaining;
                decimal total = 0;
                foreach (var i in arr)
                {
                    total = total + i;
                }
                decimal compeleted = item.Completed;
                decimal ongoint = item.Ongoing;
                decimal remaining = item.Remaining;
                if (total != 0)
                {
                    item.Completed = Math.Round((compeleted / total) * 100, 0);
                    item.Ongoing = Math.Round((item.Ongoing / total) * 100, 0);
                    item.Remaining = Math.Round((item.Remaining / total) * 100, 0);
                }




                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalFollowUpAudit(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

                 --SELECT 
                 --COUNT(Distinct AI.AuditId)Id
                 --FROM A_AuditIssues AI
                 --LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
                 --WHERE 1=1 AND AI.CreatedOn >= @BeginingYear AND AI.CreatedOn <= @EndYear AND	
			     --CAST(AI.ImplementationDate AS DATE) <= CAST(GETDATE() AS DATE)
			     --AND AI.Id NOT IN(SELECT AuditIssueId FROM A_AuditBranchFeedbacks)
           


SELECT COUNT(DISTINCT t.AuditId) AS Id
FROM (
    SELECT *,
           ROW_NUMBER() OVER (
               PARTITION BY AuditIssueId 
               ORDER BY CreatedOn DESC
           ) AS rn
    FROM A_AuditBranchFeedbacks
WHERE 
1=1 AND CreatedOn >= @BeginingYear AND CreatedOn <= @EndYear
AND CAST(ImplementationDate AS date) < CAST(CreatedOn AS date)
) AS t
LEFT OUTER JOIN A_Audits A ON t.AuditId = A.Id
WHERE t.rn = 1
AND A.StartDate >= @BeginingYear AND A.StartDate <= @EndYear


                ";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalIssueRejected(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

                 SELECT Count(A_Audits.Id)Id
                 FROM A_Audits
                 where  A_Audits.IssueIsRejected = 1
                 AND 1=1

                ";

                sqlText += "AND StartDate >= @BeginingYear AND StartDate <= @EndYear";

                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalPendingIssueReview(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"
        SELECT 
        COUNT(AI.ID) Id
        FROM A_AuditIssues AI

        LEFT OUTER JOIN A_Audits A on AI.AuditId=A.Id
        LEFT OUTER JOIN Enums E on E.Id=AI.IssuePriority

        WHERE 1=1 and AI.CreatedBy=@UserName AND AI.CreatedOn >= @BeginingYear AND AI.CreatedOn <= @EndYear AND
        AI.DateOfSubmission < CAST(GETDATE() AS DATE)
	    AND AI.Id not in(select AuditIssueId FROM A_AuditBranchFeedbacks) AND AI.Id IN (SELECT AuditIssueId FROM AuditIssueUsers WHERE IsMailSend=1)

                ";
                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int TotalRisk(string UserName)
        {
            string sqlText = "";
            DataTable dt = new DataTable();
            DataTable authdt = new DataTable();
            try
            {
                sqlText = @"

               SELECT  Count(A_AuditIssues.Id)Id
               FROM A_AuditIssues 
               WHERE 1=1 AND CreatedOn >= @BeginingYear AND CreatedOn <= @EndYear AND risk IS NOT NULL 

               --and A_AuditIssues.Createdby=@UserName

                ";
                sqlText = ApplyConditions(sqlText, null, null);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                //objComm.SelectCommand.Parameters.AddWithValue("@UserName", UserName);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);

                objComm.Fill(dt);
                return Convert.ToInt32(dt.Rows[0]["Id"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AuditReports> UnPlan(string UserName)
        {
            string sqlText = "";
            List<AuditReports> VMs = new List<AuditReports>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"


SELECT
 
    COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 0 ELSE NULL END) AS UnPlanOngoing,
    COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 0 ELSE NULL END) AS UnPlanCompleted,
    (COUNT(CASE WHEN a.IsPlaned = 0 THEN 0 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Ongoing' THEN 0 ELSE NULL END) - COUNT(CASE WHEN a.AuditStatus = 'Completed' THEN 0 ELSE NULL END)) AS UnPlanRemaining
FROM   
      [AuditTypes] AS AT
LEFT JOIN    
      [A_Audits] AS a ON AT.Id = a.AuditTypeId
WHERE 1=1 AND a.IsPlaned = 0
      
                ";


                sqlText += "AND StartDate >= @BeginingYear AND StartDate <= @EndYear";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);
                objComm.SelectCommand.Parameters.AddWithValue("@BeginingYear", YearChange.BeginingYear);
                objComm.SelectCommand.Parameters.AddWithValue("@EndYear", YearChange.EndYear);


                objComm.Fill(dt);

                VMs = dt.ToList<AuditReports>();


                var item = VMs.FirstOrDefault();
                decimal[] arr = new decimal[3];
                arr[0] = item.UnPlanCompleted;
                arr[1] = item.UnPlanOngoing;
                arr[2] = item.UnPlanRemaining;
                decimal total = 0;
                foreach (var i in arr)
                {
                    total = total + i;
                }
                if (total != 0)
                {
                    item.UnPlanCompleted = Math.Round((item.UnPlanCompleted / total) * 100, 0);
                    item.UnPlanOngoing = Math.Round((item.UnPlanOngoing / total) * 100, 0);
                    item.UnPlanRemaining = Math.Round((item.UnPlanRemaining / total) * 100, 0);
                }




                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AuditMaster Update(AuditMaster model)
        {



            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update UserBranchMap set 

 UserId               =@UserId  
,BranchId              =@BranchId  
 
,LastUpdateBy               =@LastUpdateBy  
,LastUpdateOn               =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom   
                       
where  Id= @Id ";


                SqlCommand command = CreateCommand(query);
                command.Parameters.Add("@UserId", SqlDbType.NChar).Value = model.UserId;
                command.Parameters.Add("@Id", SqlDbType.NChar).Value = model.Id;

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


        public List<AuditMaster> GetTotalBranchWithAuditCount(AuditMaster model)
        {
            string sqlText = "";
            List<AuditMaster> VMs = new List<AuditMaster>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

    SELECT 
    --top(50)

    b.BranchName, 
    COUNT(DISTINCT a.Id) AS TotalAudits, 
    COUNT(ai.Id) AS TotalIssues

    FROM BranchProfiles b
    LEFT JOIN A_Audits a ON b.BranchID = a.BranchID
    LEFT JOIN A_AuditIssues ai ON a.Id = ai.AuditId

    GROUP BY b.BranchName
  
";


                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);


                objComm.Fill(dt);
                VMs = dt.ToList<AuditMaster>();

                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        public List<AuditIssue> GetIssueCategoryData(AuditIssue model)
        {
            string sqlText = "";
            List<AuditIssue> VMs = new List<AuditIssue>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

    SELECT 
    'InvestigationOrForensis' AS MeetingType, 
    COUNT(*) AS TotalCount
FROM 
    A_AuditIssues
WHERE 
    InvestigationOrForensis = 1

UNION

SELECT 
    'StratigicMeeting' AS MeetingType, 
    COUNT(*) AS TotalCount
FROM 
    A_AuditIssues
WHERE 
    StratigicMeeting = 1

UNION

SELECT 
    'ManagementReviewMeeting' AS MeetingType, 
    COUNT(*) AS TotalCount
FROM 
    A_AuditIssues
WHERE 
    ManagementReviewMeeting = 1

UNION

SELECT 
    'OtherMeeting' AS MeetingType, 
    COUNT(*) AS TotalCount
FROM 
    A_AuditIssues
WHERE 
    OtherMeeting = 1

UNION

SELECT 
    'Training' AS MeetingType, 
    COUNT(*) AS TotalCount
FROM 
    A_AuditIssues
WHERE 
    Training = 1

	UNION

SELECT 
    'Operational' AS Operational, 
    COUNT(*) AS TotalCount
FROM 
    A_AuditIssues
WHERE 
    Operational = 1

		UNION

SELECT 
    'Financial' AS Financial, 
    COUNT(*) AS TotalCount
FROM 
    A_AuditIssues
WHERE 
    Financial = 1


			UNION

SELECT 
    'Compliance' AS Compliance, 
    COUNT(*) AS Compliance
FROM 
    A_AuditIssues
WHERE 
    Compliance = 1
  
";


                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, null, null);


                objComm.Fill(dt);
                VMs = dt.ToList<AuditIssue>();

                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DeshboardSettings> DeshboardSettingGetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<DeshboardSettings> VMs = new List<DeshboardSettings>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = $@"

                 SELECT 
                 DS.Id
                ,UserId
                ,IsPieChart
                ,IsPlannedEngagement
                ,IsUnplannedEngagement 
                ,IsYearData
                ,IsBoxData
                ,UR.UserName
     
                FROM DashboardSettings DS
                LEFT OUTER JOIN  {AuthDbConfig.AuthDB}.dbo.AspNetUsers  UR on DS.UserId = UR.Id

      where 1=1 
    
";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt.ToList<DeshboardSettings>();

                return VMs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int DeshboardSettingGetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Teams> VMs = new List<Teams>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
select 
count(Id)FilteredCount

FROM DeshboardSetting  where 1=1 ";

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

        public DeshboardSettings DeshboardSettingInsert(DeshboardSettings objMaster)
        {
            try
            {
                string sqlText = "";

                var command = CreateCommand(@" INSERT INTO DashboardSettings(


 UserId
,IsPieChart
,IsFinancialData
,IsPlannedEngagement
,IsUnplannedEngagement
,IsYearData
,IsBoxData

,CreatedBy
,CreatedOn
,CreatedFrom


) VALUES (

 @UserId
,@IsPieChart
,@IsFinancialData
,@IsPlannedEngagement
,@IsUnplannedEngagement
,@IsYearData
,@IsBoxData

,@CreatedBy
,@CreatedOn
,@CreatedFrom


)SELECT SCOPE_IDENTITY()");


                command.Parameters.Add("@UserId", SqlDbType.NChar).Value = objMaster.UserId;
                command.Parameters.Add("@IsPieChart", SqlDbType.Bit).Value = objMaster.IsPieChart;
                command.Parameters.Add("@IsFinancialData", SqlDbType.Bit).Value = objMaster.IsFinancialData;
                command.Parameters.Add("@IsPlannedEngagement", SqlDbType.Bit).Value = objMaster.IsPlannedEngagement;
                command.Parameters.Add("@IsUnplannedEngagement", SqlDbType.Bit).Value = objMaster.IsUnplannedEngagement;
                command.Parameters.Add("@IsYearData", SqlDbType.Bit).Value = objMaster.IsYearData;
                command.Parameters.Add("@IsBoxData", SqlDbType.Bit).Value = objMaster.IsBoxData;
            
                command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedBy.ToString();
                command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedOn.ToString();
                command.Parameters.Add("@CreatedFrom", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.CreatedFrom.ToString()) ? (object)DBNull.Value : objMaster.Audit.CreatedFrom.ToString();

                objMaster.Id = Convert.ToInt32(command.ExecuteScalar());

                return objMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DeshboardSettings DeshboardSettingUpdate(DeshboardSettings objMaster)
        {
            try
            {
                string sqlText = "";
                int count = 0;
                string query = @"  update DashboardSettings set

 IsPieChart  =@IsPieChart
,IsFinancialData  =@IsFinancialData
,IsPlannedEngagement=@IsPlannedEngagement
,IsUnplannedEngagement=@IsUnplannedEngagement
,IsYearData=@IsYearData
,IsBoxData=@IsBoxData

,LastUpdateBy              =@LastUpdateBy  
,LastUpdateOn              =@LastUpdateOn  
,LastUpdateFrom            =@LastUpdateFrom 
                       
where  Id= @Id ";



                SqlCommand command = CreateCommand(query);

                command.Parameters.Add("@Id", SqlDbType.Int).Value = objMaster.Id;
                command.Parameters.Add("@IsPieChart", SqlDbType.Bit).Value = objMaster.IsPieChart;
                command.Parameters.Add("@IsFinancialData", SqlDbType.Bit).Value = objMaster.IsFinancialData;
                command.Parameters.Add("@IsPlannedEngagement", SqlDbType.Bit).Value = objMaster.IsPlannedEngagement;
                command.Parameters.Add("@IsUnplannedEngagement", SqlDbType.Bit).Value = objMaster.IsUnplannedEngagement;
                command.Parameters.Add("@IsYearData", SqlDbType.Bit).Value = objMaster.IsYearData;
                command.Parameters.Add("@IsBoxData", SqlDbType.Bit).Value = objMaster.IsBoxData;
                
                command.Parameters.Add("@LastUpdateBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.LastUpdateBy.ToString()) ? (object)DBNull.Value : objMaster.Audit.LastUpdateBy.ToString();
                command.Parameters.Add("@LastUpdateOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.LastUpdateOn.ToString()) ? (object)DBNull.Value : objMaster.Audit.LastUpdateOn.ToString();
                command.Parameters.Add("@LastUpdateFrom ", SqlDbType.NChar).Value = string.IsNullOrEmpty(objMaster.Audit.LastUpdateFrom.ToString()) ? (object)DBNull.Value : objMaster.Audit.LastUpdateFrom.ToString();

                int rowcount = command.ExecuteNonQuery();

                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }

                return objMaster;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DeshboardSettings> DeshboardSettingGetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            try
            {
                string sqlText = @"

                 SELECT 
                 Id
                ,UserId
                ,ISNULL(IsPieChart,'0')IsPieChart
                ,ISNULL(IsFinancialData,'0')IsFinancialData
                ,ISNULL(IsPlannedEngagement,'0')IsPlannedEngagement
                ,ISNULL(IsUnplannedEngagement,'0')IsUnplannedEngagement
                ,ISNULL(IsYearData,'0')IsYearData
                ,ISNULL(IsBoxData,'0')IsBoxData

                FROM  DashboardSettings

                where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);

                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();

                adapter.Fill(dtResult);

                List<DeshboardSettings> vms = dtResult.ToList<DeshboardSettings>();

                return vms;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
