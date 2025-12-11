using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class Tours
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
		[Display(Name = "Name Of Officer")]
		public string Name { get; set; }
		public string Designation { get; set; }
        [Display(Name = "Purpose Of Visite")]
        public string VisitPurpose { get; set; }
		public string Transport { get; set; }
        [Display(Name = "Amount")]

        public decimal Amount { get; set; }
        [Display(Name = "Total Amount")]

        public decimal TotalAmount { get; set; }
        public string? Operation { get; set; }
		[Display(Name = "Audit Name")]

		public int AuditId { get; set; }
		public string EmpNameId { get; set; }
		public string EmpName { get; set; }
		[Display(Name = "Team Name")]

		public int TeamId { set; get; }
		public string TeamName { set; get; }
        [Display(Name = "Remarks")]

        public string? Description { set; get; }
        [Display(Name = "To Date")]
        public string ToDate { set; get; }
        public string Date { set; get; }
		[Display(Name = "From Date")]
		public string FromDate { set; get; }
        public string IsPost { set; get; }
        public string? ReasonOfUnPost { set; get; }
		[Display(Name = "Rejected Comments")]
		public string? RejectedComments { set; get; }
		[Display(Name = "Approved Comments")]
		public string? CommentsL1 { set; get; }
        public string? ApproveStatus { set; get; }
        public string BranchName { set; get; }
        public string AuditType { set; get; }
		public List<string> IDs { get; set; }	      
        public string Edit { get; set; } = "Audit";
        public int DayCount { get; set; }
        public string Word { get; set; }
        public int FiscalYearDetailId { get; set; }
        public int EarningTypeId { get; set; }
        [Display(Name = "Code")]
        public string DesignationId { get; set; }
        public string DepartmentId { get; set; }
        public string SalaryMonthId { get; set; }
        public decimal EarningAmount { get; set; }
        public string OTHrsSpecial { get; set; }
        public string EarningDate { get; set; }
        public string ProjectId { get; set; }
        public string EmployeeId { get; set; }
        public string SectionId { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchive { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedAt { get; set; }
        public string? CreatedFrom { get; set; }

        public Audit Audit;
		public Approval Approval;

        public Tours()
        {
            Audit = new Audit();
            Approval = new Approval();
           
        }


    }

}
