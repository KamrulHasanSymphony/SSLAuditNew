using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditCompliances
{
	public interface IBKAuditComplianceRepository : IBaseRepository<BKAuditCompliance>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditCompliance MultiplePost(BKAuditCompliance objBKAuditCompliance);
        BKAuditCompliance MultipleUnPost(BKAuditCompliance model);
		
	}
}
