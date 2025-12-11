using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Categorys
{
	public interface ICategoryService : IBaseService<Category>
	{
        ResultModel<Category> MultiplePost(Category model);
		ResultModel<Category> MultipleUnPost(Category model);
		
	}
}
