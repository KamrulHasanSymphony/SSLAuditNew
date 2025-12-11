using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditInfoMasterApprovals
{
	public interface IBKAuditInfoMasterApprovalService : IBaseService<BKAuditInfoMasterApproval>
	{
        ResultModel<BKAuditInfoMasterApproval> MultiplePost(BKAuditInfoMasterApproval model);
		ResultModel<BKAuditInfoMasterApproval> MultipleUnPost(BKAuditInfoMasterApproval model);
		
	}
}
