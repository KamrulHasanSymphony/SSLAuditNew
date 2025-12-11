using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKCheckListSubTypes
{
	public interface IBKCheckListSubTypeService : IBaseService<BKCheckListSubType>
	{
        ResultModel<BKCheckListSubType> MultiplePost(BKCheckListSubType model);
		ResultModel<BKCheckListSubType> MultipleUnPost(BKCheckListSubType model);
		
	}
}
