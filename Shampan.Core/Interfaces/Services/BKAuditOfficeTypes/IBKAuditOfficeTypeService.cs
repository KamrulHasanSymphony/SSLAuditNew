using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditOfficeTypes
{
	public interface IBKAuditOfficeTypeService : IBaseService<BKAuditOfficeType>
	{
        ResultModel<BKAuditOfficeType> MultiplePost(BKAuditOfficeType model);
		ResultModel<BKAuditOfficeType> MultipleUnPost(BKAuditOfficeType model);
		
	}
}
