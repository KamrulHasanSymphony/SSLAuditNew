using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKCheckListSubTypes
{
	public interface IBKCheckListSubTypeRepository : IBaseRepository<BKCheckListSubType>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKCheckListSubType MultiplePost(BKCheckListSubType objBKCheckListSubType);
        BKCheckListSubType MultipleUnPost(BKCheckListSubType model);
		
	}
}
