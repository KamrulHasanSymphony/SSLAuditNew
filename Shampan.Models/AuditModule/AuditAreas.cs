using System.ComponentModel.DataAnnotations;

namespace Shampan.Models.AuditModule;

public class AuditAreas
{

    public int Id { get; set; }
    public int AuditId { get; set; }
    public string PID { get; set; }
    //public int PID { get; set; }
    public int AuditPointId { get; set; }
    public int AuditTypeId { get; set; }
    public int WeightPersent { get; set; }
    public string MaximumMark { get; set; }
    public string AuditPointName { get; set; }
    public decimal P_Mark { get; set; }
    public int P_Level { get; set; }
    [Display(Name = "Audit Type")]
    public string AuditType { get; set; }
    [Display(Name = "Area Audit Type")]
    public string AreaAuditType { get; set; }

    [Display(Name = "Audit Area")]
    public string? AuditArea { get; set; }
    public string? AreaDetails { get; set; }
    public string Operation { get; set; } = "add";
    public string Edit { get; set; } = "";
}