using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class SalesInvoice
    {
        public int? Id { get; set; }
        public string? SalesInvoiceNo { get; set; }
        public string? BranchCode { get; set; }
        public string? BranchName { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? InvoiceDateTime { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public decimal? NBRPrice { get; set; }
        public decimal? VATAmount { get; set; }
        public decimal? SubTotal { get; set; }
        public string? BranchId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
