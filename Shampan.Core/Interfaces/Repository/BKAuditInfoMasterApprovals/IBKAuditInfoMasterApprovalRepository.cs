using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditInfoMasterApprovals
{
	public interface IBKAuditInfoMasterApprovalRepository : IBaseRepository<BKAuditInfoMasterApproval>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditInfoMasterApproval MultiplePost(BKAuditInfoMasterApproval model);
        BKAuditInfoMasterApproval MultipleUnPost(BKAuditInfoMasterApproval model);
		
	}
}
