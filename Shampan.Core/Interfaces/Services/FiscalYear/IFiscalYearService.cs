using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.FiscalYear
{
	public interface IFiscalYearService : IBaseService<FiscalYearVM>
	{
        ResultModel<FiscalYearDetailVM> InsertDetails(FiscalYearDetailVM model);
        ResultModel<List<FiscalYearDetailVM>> GetAllDetail(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


    }
}
