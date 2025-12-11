using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.AuditPoints
{
	public interface IAuditPointRepository : IBaseRepository<AuditPoint>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
		
	}
}
