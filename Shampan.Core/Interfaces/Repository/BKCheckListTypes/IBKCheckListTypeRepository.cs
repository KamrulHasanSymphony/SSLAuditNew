using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKCheckListTypes
{
	public interface IBKCheckListTypeRepository : IBaseRepository<BKCheckListType>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKCheckListType MultiplePost(BKCheckListType objBKCheckListType);
        BKCheckListType MultipleUnPost(BKCheckListType model);
		
	}
}
