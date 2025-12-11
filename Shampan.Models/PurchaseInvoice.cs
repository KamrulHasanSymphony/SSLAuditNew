using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class PurchaseInvoice
    {
        public int Id { get; set; }
        public string? PurchaseInvoiceNo { get; set; }
        public string? BranchCode { get; set; }
        public string? BranchName { get; set; }
        public string? VendorCode { get; set; }
        public string? VendorName { get; set; }
        public string? InvoiceDateTime { get; set; }
        public string? BENumber { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? SDAmount { get; set; }
        public decimal? VATAmount { get; set; }
        public string? BranchId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
