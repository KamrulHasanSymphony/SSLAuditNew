using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditOfficeTypes
{
	public interface IBKAuditOfficeTypeRepository : IBaseRepository<BKAuditOfficeType>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditOfficeType MultiplePost(BKAuditOfficeType objBKAuditOfficeType);
        BKAuditOfficeType MultipleUnPost(BKAuditOfficeType model);
		
	}
}
