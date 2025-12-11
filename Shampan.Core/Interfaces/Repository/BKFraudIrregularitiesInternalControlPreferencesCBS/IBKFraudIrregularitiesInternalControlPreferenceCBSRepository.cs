using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKFraudIrregularitiesInternalControlPreferencesCBS
{
	public interface IBKFraudIrregularitiesInternalControlPreferenceCBSRepository : IBaseRepository<BKFraudIrregularitiesInternalControlPreferenceCBS>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
		
	}
}
