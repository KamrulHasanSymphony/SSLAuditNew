using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Repository.Tour
{
	public interface IToursRepository : IBaseRepository<Tours>
	{
		string CodeGeneration(string CodeGroup, string CodeName);

		Tours MultiplePost(Tours objAdvances);
		Tours MultipleUnPost(Tours model);
		List<Tours> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        List<Tours> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        List<Tours> GetAllForReports(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);
        SalesInvoice SaleInsert(SalesInvoice model);
        PurchaseInvoice PurchaseInsert(PurchaseInvoice model);
        
        List<SalesInvoice> GetSaleIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        int GetSaleIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        List<PurchaseInvoice> GetPurchasseIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

        int GetPurchaseIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
