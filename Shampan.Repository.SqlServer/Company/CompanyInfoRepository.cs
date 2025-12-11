using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Company;
using Shampan.Models;

using System.Net;
using System.DirectoryServices;


//using System.DirectoryServices.AccountManagement;

namespace Shampan.Repository.SqlServer.Company
{
    public class CompanyInfoRepository : Repository, ICompanyInfoRepository
    {
        private readonly string _ldapServer = "yourdomain.com";
        private readonly string _ldapBaseDN = "DC=yourdomain,DC=com"; // AD Base DN

        private readonly string _adminUser = "erp"; // Admin account
        private readonly string _adminPassword = "123456"; // Admin password

        public CompanyInfoRepository(SqlConnection context, SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }

        public List<CompanyInfo> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<CompanyInfo> VMs = new List<CompanyInfo>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT  [Id]
      ,[CompanyId]
      ,[CompanyName]
      ,[CompanyDataBase]
      ,[SerialNo]
      ,[IsAdCheck]
      ,[AdUrl]
    FROM [CompanyInfo]

where 1=1 
";
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                VMs = dt.ToList<CompanyInfo>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<CompanyInfo> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public CompanyInfo Insert(CompanyInfo model)
        {
            throw new NotImplementedException();
        }

        public CompanyInfo Update(CompanyInfo model)
        {
            throw new NotImplementedException();
        }

        public List<BranchProfile> GetBranches(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            string sqlText = "";
            List<BranchProfile> branchProfiles = new List<BranchProfile>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
SELECT bp.[BranchID]
      ,bp.[BranchCode]
      ,bp.[BranchName]
      ,bp.[BranchLegalName]
      ,bp.[Address]
      ,bp.[City]
      ,bp.[ZipCode]
      ,bp.[TelephoneNo]
      ,bp.[FaxNo]
      ,bp.[Email]
      ,bp.[ContactPerson]
      ,bp.[ContactPersonDesignation]
      ,bp.[ContactPersonTelephone]
      ,bp.[ContactPersonEmail]
      
      
      ,bp.[TINNo]
      ,bp.[Comments]
      ,bp.[ActiveStatus]
      ,bp.[CreatedBy]
      ,bp.[CreatedOn]
      ,bp.[LastModifiedBy]
      ,bp.[LastModifiedOn]
      ,isnull(bp.[IsArchive],0)IsArchive
    
     
      ,bp.[Id]
  
      
      ,bp.[IsWCF]
      
      ,bp.[IsCentral]
  FROM [BranchProfiles] bp left outer join UserBranchMap um on bp.BranchID = um.BranchId

where 1=1 


";
                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, true);

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);

                branchProfiles = dt.ToList<BranchProfile>();

                return branchProfiles;


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public string[] CheckADAuth(string UserName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null, CompanyInfo companyInfo = null)
        //{
        //    throw new NotImplementedException();
        //}

        public string[] CheckADAuth(string UserName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null, CompanyInfo companyInfo = null)
        {

            string[] retResults = new string[3];
            retResults[0] = "Fail";
            retResults[1] = "Message";
            retResults[2] = "";

            try
            {

                string AdUrl = companyInfo.AdUrl;

                if (string.IsNullOrWhiteSpace(AdUrl))
                {
                    retResults[1] = "AD Url not Exist";
                    return retResults;
                }
                if (string.Equals(UserName, "admin", StringComparison.OrdinalIgnoreCase))
                {
                    retResults[1] = "AD User not Exist";
                    return retResults;
                }

                if (CheckUserinAD(AdUrl, UserName))
                {
                    //PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, AdUrl);
                    //UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, UserName);

                    //if (userPrincipal != null)
                    //{
                    //    System.DirectoryServices.DirectoryEntry dirEntry = userPrincipal.GetUnderlyingObject() as System.DirectoryServices.DirectoryEntry;
                    //    bool status = IsAccountDisabled(dirEntry);
                    //    if (status)
                    //    {
                    //        retResults[1] = "User has been disabled";
                    //        return retResults;
                    //    }
                    //}

                }
            }
            catch (Exception e)
            {

                throw;
            }

            return retResults;

        }

        public bool CheckUserinAD(string domain, string username)
        {
            try
            {
                //string ldapPath = $"LDAP://{domain}";
                string ldapPath = domain;

                using (System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(ldapPath))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = $"(&(objectClass=user)(sAMAccountName={username}))";
                        searcher.PropertiesToLoad.Add("sAMAccountName");

                        SearchResult result = searcher.FindOne();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking user in AD: {ex.Message}");
                return false;
            }
        }



        //public bool IsAccountDisabled(System.DirectoryServices.DirectoryEntry user)
        //{
        //    const string uac = "userAccountControl";
        //    if (user.NativeGuid == null) return false;

        //    if (user.Properties[uac] != null && user.Properties[uac].Value != null)
        //    {
        //        UserFlags userFlags = (UserFlags)user.Properties[uac].Value;
        //        return userFlags.Contains(UserFlags.AccountDisabled);
        //    }

        //    return false;
        //}


    }
}
