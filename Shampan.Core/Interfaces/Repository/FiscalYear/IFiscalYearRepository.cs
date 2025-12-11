using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.FiscalYear
{
	public interface IFiscalYearRepository : IBaseRepository<FiscalYearVM>
	{

        FiscalYearDetailVM InsertDetails(FiscalYearDetailVM model);
        List<FiscalYearDetailVM> GetAllDetail(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        FiscalYearDetailVM UpdateDetail(FiscalYearDetailVM model);
        bool CheckFiscalYearStatus(string tableName, string[] conditionalFields, string[] conditionalValue);


    }
}
