using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Repository
{
    public interface ICommonRepository : IcommonBaseRepository<CommonDropDown>
    {
        List<CommonDropDown>? GetAuditType(bool isPlanned = true);
        List<CommonDropDown>? CheckListItem(int auditId=0);
        List<CommonDropDown>? AuditAreaType(int auditIssueLevel = 0,string level="0");
        List<CommonDropDown>? AuditPointType(int pId = 0);
        List<CommonDropDown>? AuditTemplate();
        List<CommonDropDown>? BkCheckListTypes();
        List<CommonDropDown>? BkCheckListSubTypes();
        List<CommonDropDown>? BKAuidtCategorys();
        List<CommonDropDown>? BKAuditOfficeTypes();
        List<CommonDropDown>? BKAuditOffice();
        List<CommonDropDown>? BkAuditCompliances();
        List<CommonDropDown>? GetTeams();
        List<CommonDropDown>? GetAreaAuditTypes();
        List<CommonDropDown>? GetReportStatus();
        List<CommonDropDown>? GetCircularType();
        List<CommonDropDown>? GetAuditStatus();
        List<CommonDropDown>? GetAuditName();
        List<CommonDropDown>? GetIssuePriority();
        List<CommonDropDown>? FieldType();
        List<CommonDropDown>? GetRiskType();
        List<CommonDropDown>? GetIssueStatus();
        List<CommonDropDown>? GetStatus();
        List<CommonDropDown>? GetAuditTypes();
        List<CommonDropDown>? GetIssues(string auditId);
        List<CommonDropDown>? GetBranchFeedbackIssues(string auditId,string userName, AuditMaster master);


    }
}
