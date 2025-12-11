using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Tour
{
	public interface IToursService : IBaseService<Tours>
	{
		ResultModel<Tours> MultiplePost(Tours model);
		ResultModel<Tours> MultipleUnPost(Tours model);
		ResultModel<List<Tours>> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<List<Tours>> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        ResultModel<List<Tours>> GetAllForReports(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        ResultModel<SalesInvoice> SaleInsert(SalesInvoice model);
        ResultModel<PurchaseInvoice> PurchaseInsert(PurchaseInvoice model);
        ResultModel<List<SalesInvoice>> GetSaleIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> GetSaleIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


        ResultModel<List<PurchaseInvoice>> GetPurchaseIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        ResultModel<int> GetPurchaseIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);


    }
}
