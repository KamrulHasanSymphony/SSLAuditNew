using Microsoft.AspNetCore.Identity;

namespace ShampanERP.Models;

public class ApplicationUser : IdentityUser
{

    public string SageUserName { get; set; }
    public int PFNo { get; set; }
    public string Designation { get; set; }
    public bool IsPushAllow { get; set; }
    public string ProfileName { get; set; }
    public string BranchName { get; set; }
    public bool IsArchive { get; set; }
    public string? EmployeeId { get; set; } //Removing UserIdentity BY ?
    public string? FullName { get; set; }
    public string? NormalizedName { get; set; }
    public string? NormalizedPassword { get; set; }

    

}