using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.BKAuditPreferenceEvaluation
{
	public interface IBKAuditPreferenceEvaluationService : IBaseService<BKAuditPreferenceEvaluations>
	{
        ResultModel<BKAuditPreferenceEvaluations> MultiplePost(BKAuditPreferenceEvaluations model);
		ResultModel<BKAuditPreferenceEvaluations> MultipleUnPost(BKAuditPreferenceEvaluations model);
		
	}
}
