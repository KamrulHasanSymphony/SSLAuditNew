using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditInfoDetailss
{
	public interface IBKAuditInfoDetailsService : IBaseService<BKAuditInfoDetails>
	{
        ResultModel<BKAuditInfoDetails> MultiplePost(BKAuditInfoDetails model);
		ResultModel<BKAuditInfoDetails> MultipleUnPost(BKAuditInfoDetails model);
		
	}
}
