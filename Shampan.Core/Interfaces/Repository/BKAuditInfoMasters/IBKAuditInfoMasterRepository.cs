using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditInfoMasters
{
	public interface IBKAuditInfoMasterRepository : IBaseRepository<BKAuditInfoMaster>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditInfoMaster MultiplePost(BKAuditInfoMaster model);
        BKAuditInfoMaster MultipleUnPost(BKAuditInfoMaster model);
		
	}
}
