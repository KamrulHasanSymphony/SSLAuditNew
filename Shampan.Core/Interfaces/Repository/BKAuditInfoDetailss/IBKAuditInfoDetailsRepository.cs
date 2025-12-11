using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditInfoDetailss
{
	public interface IBKAuditInfoDetailsRepository : IBaseRepository<BKAuditInfoDetails>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditInfoDetails MultiplePost(BKAuditInfoDetails model);
        BKAuditInfoDetails MultipleUnPost(BKAuditInfoDetails model);
		
	}
}
