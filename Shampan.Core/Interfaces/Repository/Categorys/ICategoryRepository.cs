using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Categorys
{
	public interface ICategoryRepository : IBaseRepository<Category>
	{
		string CodeGeneration(string CodeGroup, string CodeName);
        Category MultiplePost(Category objAdvances);
        Category MultipleUnPost(Category model);
		
	}
}
