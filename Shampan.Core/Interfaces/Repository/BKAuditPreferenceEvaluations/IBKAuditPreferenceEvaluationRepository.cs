using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.BKAuditPreferenceEvaluation
{
	public interface IBKAuditPreferenceEvaluationRepository : IBaseRepository<BKAuditPreferenceEvaluations>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        BKAuditPreferenceEvaluations MultiplePost(BKAuditPreferenceEvaluations model);
        BKAuditPreferenceEvaluations MultipleUnPost(BKAuditPreferenceEvaluations model);
		
	}
}
