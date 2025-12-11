using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditInfoMasters
{
	public interface IBKAuditInfoMasterService : IBaseService<BKAuditInfoMaster>
	{
        ResultModel<BKAuditInfoMaster> MultiplePost(BKAuditInfoMaster model);
		ResultModel<BKAuditInfoMaster> MultipleUnPost(BKAuditInfoMaster model);
		
	}
}
