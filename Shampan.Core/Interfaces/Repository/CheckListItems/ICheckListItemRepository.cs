using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.CheckListItems
{
	public interface ICheckListItemRepository : IBaseRepository<CheckListItem>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        CheckListItem MultiplePost(CheckListItem objCheckListItem);
        CheckListItem MultipleUnPost(CheckListItem model);
		
	}
}
