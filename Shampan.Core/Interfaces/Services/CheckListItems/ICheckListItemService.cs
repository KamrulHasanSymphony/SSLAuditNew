using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.CheckListItems
{
	public interface ICheckListItemService : IBaseService<CheckListItem>
	{
        ResultModel<CheckListItem> MultiplePost(CheckListItem model);
		ResultModel<CheckListItem> MultipleUnPost(CheckListItem model);
		
	}
}
