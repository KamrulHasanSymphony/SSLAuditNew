using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditCompliances
{
	public interface IBKAuditComplianceService : IBaseService<BKAuditCompliance>
	{
        ResultModel<BKAuditCompliance> MultiplePost(BKAuditCompliance model);
		ResultModel<BKAuditCompliance> MultipleUnPost(BKAuditCompliance model);
		
	}
}
