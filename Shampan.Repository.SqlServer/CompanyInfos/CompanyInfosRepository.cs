using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Company;
using Shampan.Core.Interfaces.Repository.CompanyInfos;
using Shampan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Repository.SqlServer.CompanyInfos
{
    public class CompanyInfosRepository : Repository, ICompanyinfosRepository
    {

        private readonly string _ldapServer = "yourdomain.com";
        private readonly string _ldapBaseDN = "DC=yourdomain,DC=com"; // AD Base DN
        private readonly string _adminUser = "erp"; // Admin account
        private readonly string _adminPassword = "123456"; // Admin password

        public CompanyInfosRepository(SqlConnection context, SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }

        //public string[] CheckADAuth(string UserName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null, Companyinfos companyInfo = null)
        //{
        //    string[] retResults = new string[3];
        //    retResults[0] = "Fail";
        //    retResults[1] = "Message";
        //    retResults[2] = "";

        //    try
        //    {

        //        string AdUrl = companyInfo.AdUrl;

        //        if (string.IsNullOrWhiteSpace(AdUrl))
        //        {
        //            retResults[1] = "AD Url not Exist";
        //            return retResults;
        //        }
        //        if (string.Equals(UserName, "admin", StringComparison.OrdinalIgnoreCase))
        //        {
        //            retResults[1] = "AD User not Exist";
        //            return retResults;
        //        }

        //        if (CheckUserinAD(AdUrl, UserName))
        //        {
        //            try
        //            {
        //                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(AdUrl)))
        //                {
        //                    connection.Credential = new NetworkCredential(_adminUser, _adminPassword, AdUrl);
        //                    connection.AuthType = AuthType.Negotiate;
        //                    connection.Bind();

        //                    string searchFilter = $"(sAMAccountName={UserName})";
        //                    var request = new SearchRequest(_ldapBaseDN, searchFilter, SearchScope.Subtree);
        //                    var response = (SearchResponse)connection.SendRequest(request);

        //                    if (response.Entries.Count > 0)
        //                    {
        //                        retResults[1] = "User found in AD";
        //                        return retResults;
        //                    }
        //                }
        //            }
        //            catch (LdapException ex)
        //            {

        //            }
        //        }
        //        else
        //        {
        //            retResults[1] = "User not found in AD";
        //            return retResults;
        //        }
        //    }
        //    catch (Exception e)
        //    {
                
        //        throw;
        //    }

        //    return retResults;
        //}

        //public bool CheckUserinAD(string domain, string username)
        //{
        //    try
        //    {
        //        using (var connection = new LdapConnection(new LdapDirectoryIdentifier(_ldapServer)))
        //        {
        //            connection.Credential = new NetworkCredential(_adminUser, _adminPassword, domain); // Provide an admin account
        //            connection.AuthType = AuthType.Negotiate;
        //            connection.Bind(); // Authenticate

        //            string searchFilter = $"(sAMAccountName={username})";
        //            var request = new SearchRequest("DC=yourdomain,DC=com", searchFilter, SearchScope.Subtree);

        //            var response = (SearchResponse)connection.SendRequest(request);

        //            return response.Entries.Count > 0; // User exists if entries found
        //        }
        //    }
        //    catch (LdapException)
        //    {
        //        return false;
        //    }


        //}

        public List<Companyinfos> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {

            try
            {
                string sqlText = @"select 
CompanyID
,CompanyName
,CompanyLegalName
,Address
,City
,ZipCode
,TelephoneNo
,FaxNo
,Email
,ContactPerson
,ContactPersonDesignation
,ContactPersonTelephone
,ContactPersonEmail
,TINNo
,BIN
,VatRegistrationNo
,Comments
,ActiveStatus
,IsAdCheck
,AdUrl
,CreatedBy
,CreatedOn
,LastModifiedBy
,LastModifiedOn







from   CompanyInfo
where 1=1";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlCommand objComm = CreateCommand(sqlText);

                objComm = ApplyParameters(objComm, conditionalFields, conditionalValue);

                SqlDataAdapter adapter = new SqlDataAdapter(objComm);
                DataTable dtResult = new DataTable();
                adapter.Fill(dtResult);

                List<Companyinfos> vms = dtResult.ToList<Companyinfos>();
                return vms;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Companyinfos> GetIndexData(IndexModel Index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            string sqlText = "";
            List<Companyinfos> VMs = new List<Companyinfos>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select CompanyInfo.CompanyID
               
                ,CompanyInfo.CompanyName
                ,CompanyInfo.Address
                 ,CompanyInfo.City
 
                ,CompanyInfo.TelephoneNo
               

                from CompanyInfo 
                 where 1=1 ";



                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue, false);

                // ToDo Escape Sql Injection
                sqlText += @"  order by  " + Index.OrderName + "  " + Index.orderDir;
                sqlText += @" OFFSET  " + Index.startRec + @" ROWS FETCH NEXT " + Index.pageSize + " ROWS ONLY";

                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);
                var req = new Companyinfos();

                VMs.Add(req);


                VMs = dt
                .ToList<Companyinfos>();

                return VMs;


            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {


            string sqlText = "";
            List<CompanyInfo> VMs = new List<CompanyInfo>();
            DataTable dt = new DataTable();

            try
            {
                sqlText = @"
                 select count(CompanyInfo.CompanyID)FilteredCount
                from CompanyInfo  where 1=1 ";


                sqlText = ApplyConditions(sqlText, conditionalFields, conditionalValue);


                SqlDataAdapter objComm = CreateAdapter(sqlText);

                objComm.SelectCommand = ApplyParameters(objComm.SelectCommand, conditionalFields, conditionalValue);

                objComm.Fill(dt);


                return Convert.ToInt32(dt.Rows[0][0]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }
    

        public Companyinfos Insert(Companyinfos model)
        {


                    try
                    {
                        // Check if there is already a company in the database
                        var countCommand = CreateCommand(@"SELECT COUNT(*) FROM CompanyInfo");
                        int count = (int)countCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            throw new Exception("Cannot insert another company.");
                        }

                        // Proceed with inserting the new company
                        string sqlText = "";
                        //  string[] retResults = { "Fail", "Fail", Id.ToString(), sqlText, "ex", "Insert" };

                        var command = CreateCommand(@" INSERT INTO CompanyInfo(





CompanyName
,CompanyLegalName
,Address
,City
,ZipCode
,TelephoneNo
,FaxNo
,Email
,ContactPerson
,ContactPersonDesignation
,ContactPersonTelephone
,ContactPersonEmail

,Comments
,ActiveStatus
,CreatedBy
,CreatedOn


) VALUES (


@CompanyName
,@CompanyLegalName
,@Address
,@City
,@ZipCode
,@TelephoneNo
,@FaxNo
,@Email
,@ContactPerson
,@ContactPersonDesignation
,@ContactPersonTelephone
,@ContactPersonEmail


,@Comments
,@ActiveStatus
,@CreatedBy
,@CreatedOn





)SELECT SCOPE_IDENTITY()");
                    command.Parameters.Add("@CompanyName", SqlDbType.NChar).Value = model.CompanyName;
                    command.Parameters.Add("@CompanyLegalName", SqlDbType.NChar).Value = model.CompanyLegalName;

                    command.Parameters.Add("@Address", SqlDbType.NChar).Value = model.Address;
                    command.Parameters.Add("@City", SqlDbType.NChar).Value = model.City;

                    command.Parameters.Add("@ZipCode", SqlDbType.NChar).Value = model.ZipCode;
                    command.Parameters.Add("@TelephoneNo", SqlDbType.NChar).Value = model.TelephoneNo;
                    command.Parameters.Add("@FaxNo", SqlDbType.NChar).Value = model.FaxNo;
                    //command.Parameters.Add("@CurrencyCode", SqlDbType.NChar).Value = model.CurrencyCode;
                    command.Parameters.Add("@Email", SqlDbType.NChar).Value = model.Email;
                    command.Parameters.Add("@ContactPerson", SqlDbType.NChar).Value = model.ContactPerson;
                    command.Parameters.Add("@ContactPersonDesignation", SqlDbType.NChar).Value = model.ContactPersonDesignation;
                    command.Parameters.Add("@ContactPersonEmail", SqlDbType.NChar).Value = model.ContactPersonEmail;
                    command.Parameters.Add("@Comments", SqlDbType.NChar).Value = model.Comments;
                    command.Parameters.Add("@ActiveStatus", SqlDbType.NChar).Value = model.ActiveStatus;
                    command.Parameters.Add("@ContactPersonTelephone", SqlDbType.NChar).Value = model.ContactPersonTelephone;





                    command.Parameters.Add("@CreatedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedBy.ToString()) ? (object)DBNull.Value : model.Audit.CreatedBy.ToString();

                    command.Parameters.Add("@CreatedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.CreatedOn.ToString()) ? (object)DBNull.Value : model.Audit.CreatedOn.ToString();



                    model.CompanyID = Convert.ToInt32(command.ExecuteScalar());


                    return model;

                }
            
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Companyinfos Update(Companyinfos model)
        {



            try
            {
                string sqlText = "";
                int count = 0;

                string query = @"  update CompanyInfo set 

 
CompanyName              =@CompanyName
,CompanyLegalName        =@CompanyLegalName 
,Address                 =@Address  
,City                    =@City  
,ZipCode                 =@ZipCode  
,TelephoneNo             =@TelephoneNo  
,FaxNo                   =@FaxNo  
,Email                   =@Email  
,ContactPerson           =@ContactPerson  
,ContactPersonDesignation=@ContactPersonDesignation   
,ContactPersonTelephone  =@ContactPersonTelephone  
,ContactPersonEmail      =@ContactPersonEmail 

,Comments                =@Comments  
,ActiveStatus            =@ActiveStatus  
,IsAdCheck               =@IsAdCheck  
,AdUrl                   =@AdUrl  
,LastModifiedBy               =@LastModifiedBy  
,LastModifiedOn               =@LastModifiedOn  

                       
where  CompanyID= @CompanyID ";


                SqlCommand command = CreateCommand(query);
                
                command.Parameters.Add("@CompanyName", SqlDbType.NChar).Value = model.CompanyName;
                command.Parameters.Add("@CompanyLegalName", SqlDbType.NChar).Value = model.CompanyLegalName;

                command.Parameters.Add("@Address", SqlDbType.NChar).Value = model.Address;
                command.Parameters.Add("@City", SqlDbType.NChar).Value = model.City;

                command.Parameters.Add("@ZipCode", SqlDbType.NChar).Value = model.ZipCode;
                command.Parameters.Add("@TelephoneNo", SqlDbType.NChar).Value = model.TelephoneNo;
                command.Parameters.Add("@FaxNo", SqlDbType.NChar).Value = model.FaxNo;
                //command.Parameters.Add("@CurrencyCode", SqlDbType.NChar).Value = model.CurrencyCode;
                command.Parameters.Add("@Email", SqlDbType.NChar).Value = model.Email;
                command.Parameters.Add("@ContactPerson", SqlDbType.NChar).Value = model.ContactPerson;
                command.Parameters.Add("@ContactPersonDesignation", SqlDbType.NChar).Value = model.ContactPersonDesignation;
                command.Parameters.Add("@ContactPersonEmail", SqlDbType.NChar).Value = model.ContactPersonEmail;
                command.Parameters.Add("@Comments", SqlDbType.NChar).Value = model.Comments;
                command.Parameters.Add("@ActiveStatus", SqlDbType.NChar).Value = model.ActiveStatus;
                command.Parameters.Add("@ContactPersonTelephone", SqlDbType.NChar).Value = model.ContactPersonTelephone;
                command.Parameters.Add("@IsAdCheck", SqlDbType.NChar).Value = model.IsAdCheck;
                command.Parameters.Add("@AdUrl", SqlDbType.NChar).Value = model.AdUrl;
               


                command.Parameters.Add("@LastModifiedBy", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastModifiedBy.ToString()) ? (object)DBNull.Value : model.Audit.LastModifiedBy.ToString();

                command.Parameters.Add("@LastModifiedOn", SqlDbType.NChar).Value = string.IsNullOrEmpty(model.Audit.LastModifiedOn.ToString()) ? (object)DBNull.Value : model.Audit.LastModifiedOn.ToString();

                command.Parameters.Add("@CompanyID", SqlDbType.Int).Value = model.CompanyID;


                int rowcount = command.ExecuteNonQuery();

                if (rowcount <= 0)
                {
                    throw new Exception(MessageModel.UpdateFail);
                }

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
