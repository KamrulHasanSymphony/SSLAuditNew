using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditOfficeTypes
{
	public interface IBKCheckListTypeService : IBaseService<BKCheckListType>
	{
        ResultModel<BKCheckListType> MultiplePost(BKCheckListType model);
		ResultModel<BKCheckListType> MultipleUnPost(BKCheckListType model);
		
	}
}
