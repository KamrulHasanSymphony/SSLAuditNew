using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class EmployeeInfoVM
    {
        public string? Id { get; set; }
        public string? EmpNameId { get; set; }


        [Display(Name = "Branch ID")]
        public int? BranchId { get; set; }

        [Display(Name = "Advance Entry Date")]
        public string? AdvanceEntryDate { get; set; }
        public string? Designation { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        [Display(Name = "Advance Amount")]
        public decimal? AdvanceAmount { get; set; }

        [Display(Name = "Payment Type")]
        public int? PaymentEnumTypeId { get; set; }

        [Display(Name = "Payment Date")]
        public string? PaymentDate { get; set; }

        [Display(Name = "Document No")]
        public string? DocumentNo { get; set; }

        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [Display(Name = "Bank Branch Name")]
        public string? BankBranchName { get; set; }

        [Display(Name = "Received By")]
        public int? ReceiveByEnumTypeId { get; set; }
        public decimal? TravelAllowance { get; set; }
        public string? EmployeeId { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Approved By")]
        public string? ApproveById { get; set; }
        public string? EnumType { get; set; }
        public string? DistributorCode { get; set; }

        [Display(Name = "Approval Date")]
        public string? ApproveDate { get; set; }

        [Display(Name = "IsPost")]
        public bool IsPost { get; set; }

        [Display(Name = "Posted By")]
        public string? PostedBy { get; set; }
        public string? Operation { get; set; }

        [Display(Name = "Posted Date")]
        public string? PostedDate { get; set; }
        public string? PostedOn { get; set; }

        [Display(Name = "Created By")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Created On")]
        public string? CreatedOn { get; set; }
        public string? CreatedFrom { get; set; }
        [Display(Name = "Last Modified By")]
        public string? LastModifiedBy { get; set; }

        [Display(Name = "Last Modified On")]
        public string? LastModifiedOn { get; set; }
        public string? LastUpdateFrom { get; set; }
        public string? Status { get; set; }
        public string? MiddleName { get; set; }
        public string? EmpName { get; set; }
        public string? EmConPhone { get; set; }
        public string? PersonalEmail { get; set; }
        public string? PresentPhone { get; set; }
        public string? PermanentPhone { get; set; }

    }
}
