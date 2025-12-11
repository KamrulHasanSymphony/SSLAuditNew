using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKRiskAssessRegulationPreferencesCBS
{
	public interface IBKRiskAssessRegulationPreferenceCBSRepository : IBaseRepository<BKRiskAssessRegulationPreferenceCBS>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
		
	}
}
