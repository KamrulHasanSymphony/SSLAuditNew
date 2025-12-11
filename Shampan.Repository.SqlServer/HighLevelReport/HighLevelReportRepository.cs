using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.HighLevelReports;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Core.Interfaces.Repository.Tour;
using Shampan.Models;

namespace Shampan.Repository.SqlServer.HighLevelReport
{
	public class HighLevelReportRepository : Repository, IHighLevelReportRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;
        public HighLevelReportRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)

        {
            this._context = context;
            this._transaction = transaction;
        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
		{
			throw new NotImplementedException();
		}

        public List<Models.HighLevelReport> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public List<Models.HighLevelReport> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<Models.HighLevelReport> VMs = new List<Models.HighLevelReport>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"

    SELECT 
    
    AI.Id,
    AI.AuditId,
    AI.IssueName,
    AI.IsPost,
    AI.Risk,
    AI.IssueDetails,
    ISNULL(AI.InvestigationOrForensis ,'0') AS InvestigationOrForensis,
    ISNULL(AI.StratigicMeeting ,'0') AS StratigicMeeting,
    ISNULL(AI.ManagementReviewMeeting ,'0') AS ManagementReviewMeeting,
    ISNULL(AI.OtherMeeting ,'0') AS OtherMeeting,
    ISNULL(AI.Training ,'0') AS Training,
    ISNULL(AI.Operational ,'0') AS Operational,
    ISNULL(AI.Financial ,'0') AS Financial,
    ISNULL(AI.Compliance ,'0') AS Compliance,
    FORMAT(AI.DateOfSubmission, 'yyyy-MM-dd') AS DateOfSubmission,
    A.Code,
    A.Name,
    A.AuditStatus,
    Enums.EnumValue AS IssuePriority,
    anm.EnumValue AS IssueStatus,
    AF.Heading FeedbackHeading, 
    AF.IssueDetails FeedbackDetails,  
	BF.Heading BranchFeedBackHeading,
	Bf.IssueDetails BranchFeedBackDetails

FROM A_AuditIssues AI
LEFT OUTER JOIN A_Audits A ON A.Id = AI.AuditId
LEFT OUTER JOIN Enums ON AI.IssuePriority = Enums.Id
LEFT OUTER JOIN Enums anm ON AI.IssueStatus = anm.Id

OUTER APPLY (
    SELECT TOP 1 
        Heading,  
        IssueDetails  
    FROM A_AuditFeedbacks AF
    WHERE AF.AuditIssueId = AI.Id  
    ORDER BY AF.Id DESC 
) AS AF

OUTER APPLY (
    SELECT TOP 1 
        Heading,  
        IssueDetails    
    FROM A_AuditBranchFeedbacks BF
    WHERE BF.AuditIssueId = AI.Id  
    ORDER BY BF.Id DESC 
) AS BF

WHERE 1 = 1
";



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + index.OrderName + "  " + index.orderDir;
                sqlText += @" OFFSET  " + index.startRec + @" ROWS FETCH NEXT " + index.pageSize + " ROWS ONLY";
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);


                objComm.Fill(dt);
                VMs = dt.ToList<Models.HighLevelReport>();

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
                 count(AI.Id)FilteredCount
                 FROM A_AuditIssues AI
                 
                 left outer join A_Audits A on A.Id = AI.AuditId

                 where 1=1 ";

                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);
                SqlDataAdapter objComm = CreateAdapter(sqlText);
                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);
                objComm.Fill(dt);

                return Convert.ToInt32(dt.Rows[0]["FilteredCount"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Models.HighLevelReport Insert(Models.HighLevelReport model)
        {
            throw new NotImplementedException();
        }

        public Models.HighLevelReport Update(Models.HighLevelReport model)
        {
            throw new NotImplementedException();
        }
    }
}
