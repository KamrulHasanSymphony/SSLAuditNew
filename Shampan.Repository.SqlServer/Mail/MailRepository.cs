using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Shampan.Core.ExtentionMethod;
using Shampan.Core.Interfaces.Repository.Advance;
using Shampan.Core.Interfaces.Repository.Mail;
using Shampan.Core.Interfaces.Repository.Team;
using Shampan.Models;
using System.Xml.Linq;

namespace Shampan.Repository.SqlServer.Mail
{
    public class MailRepository : Repository, IMailRepository
    {
        private DbConfig _dbConfig;
        private SqlConnection context;
        private SqlTransaction transaction;
        public MailRepository(SqlConnection context, SqlTransaction transaction, DbConfig dbConfig)
        {
            this._context = context;
            this._transaction = transaction;
            this._dbConfig = dbConfig;

        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }
        public bool CheckExists(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }
        public bool CheckPostStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();

        }
        public bool CheckPushStatus(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }
        public string CodeGeneration(string CodeGroup, string CodeName)
        {
            try
            {
                string codeGeneration = GenerateCode(CodeGroup, CodeName);
                return codeGeneration;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Delete(string tableName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();

        }
        public List<AuditMail> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();

        }

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();

        }

        public List<AuditMail> GetIndexDataStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();

        }

        public List<AuditMail> GetIndexDataSelfStatus(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();

        }
        public List<AuditMail> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();

        }
        public int GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();

        }

        public string GetSettingsValue(string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public string GetSingleValeByID(string tableName, string ReturnFields, string[] conditionalFields, string[] conditionalValue)
        {
            throw new NotImplementedException();
        }

        public AuditMail Insert(AuditMail model)
        {
            try
            {

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public AuditMail MultiplePost(AuditMail objAdvances)
        {
            throw new NotImplementedException();
        }

        public AuditMail MultipleUnPost(AuditMail vm)
        {
            throw new NotImplementedException();

        }

        public AuditMail Update(AuditMail model)
        {
            throw new NotImplementedException();

        }
        public AuditMail SendEmail(AuditMail model)
        {
            try
            {

                #region AuditReport
                if (model.Status == "AuditReport")
                {
                    string subject = "Final Audit Report"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "Final Audit Report";
                    var bodyString = "<tr>" +
                                     "</tr>" +

                                  "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                                   "<div> &nbsp;</div> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We trust this message finds you well.  </span></font></div>" +
                                                     "<div> &nbsp;</div> " +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are pleased to inform you that the <b style='color:blue'> Final Audit Report </b> - <b>" + model.Name + "</b> has been concluded, requesting to click on the below link to view the report.</span></font></div>" +
                                                     "<div> &nbsp;</div> " +

                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +

                                    "<tr>" +
                                        "<td  style='height:0px;'> " +

                                                  "<div style = 'margin-left:10px;color:black;'> " +
                                                        "<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:0px 0px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                                  "</div> " +

                                        "</td>" +


                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Should you have any queries or need clarification, our team is available for discussion. </span></font></div>" +
                                                    "<div> &nbsp;</div> " +
                                                    "<div> &nbsp;</div> " +
                                                    "<div> &nbsp;</div> " +



                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                                    "<div> &nbsp;</div> " +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +


                                        "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +



                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;
                    sent = AuditReportSendMail(model.To, bodyString, message);

                    return model;
                }
                #endregion


                #region FollowUpAuditIssue
                else if (model.Status == "FollowUpAuditIssue")
                {
                    string subject = "FollowUpAuditIssue"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "FollowUpAuditIssue";

                    var bodyString = "<tr>" +

                                    "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The audit issue- <b>" + model.Name + "</b> has been submitted for your review </span></font></div>" +

                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +
                                    "<tr>" +
                                        "<td  style='height:50px;'> " +

                                                 "<div style = 'margin-left:10px;color:black;'> " +
                                                     "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                                  "</div> " +
                                        "</td>" +

                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:18px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +


                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +
                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +
                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +

                                        "<tr> " +

                                        "<td style = 'text-align:left;'>" +

                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +

                                               "</div> " +

                                       "</tr> " +

                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;

                    sent = FollowUpAuditIssueEamilSendMail(model.To, bodyString, message);

                    return model;
                }
                #endregion

                #region AuditIssue
                else if (model.Status == "AuditIssue")
                {
                    string subject = "AuditIssue"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "AuditIssue";

                    var bodyString = "<tr>" +

                                     "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +


                                                     "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                                     "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                                     "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are requesting you to provide your Feedback/Response of the audit observations <b>" + model.Name + "</b> </span></font></div>" +

                                                     "<div> &nbsp;</div> " +

                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +

                                    "<tr>" +
                                        "<td  style='height:50px;padding-left: 0px;'> " +

                                                  "<div style = 'margin-left:10px;color:black;'> " +
                                                         "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                                  "</div> " +

                                        "</td>" +


                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +


                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +

                                        "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:10px 0 20px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +


                                               "</div> " +
                                       "</tr> " +

                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;
                    sent = AuditIssueSendMail(model.To, bodyString, message);

                    return model;
                }
                #endregion

                #region TotalPendingIssuesReview
                else if (model.Status == "TotalPendingIssuesReview")
                {
                    string subject = "TotalPendingIssuesReviewEamil"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "TotalPendingIssuesReviewEamil";

                    var bodyString = "<tr>" +

                                    "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:10px 0 20px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The audit issue- <b>" + model.Name + "</b> has been submitted for your review </span></font></div>" +

                                                     "<div> &nbsp;</div> " +
                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +
                                    "<tr>" +

                                        "<td  style='height:50px;'> " +

                                                 "<div style = 'margin-left:10px;color:black;'> " +
                                                       "<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                                  "</div> " +

                                        "</td>" +

                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +


                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +

                                        "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +


                                               "</div> " +

                                       "</tr> " +

                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;
                    sent = TotalPendingIssuesReviewSendMail(model.To, bodyString, message);

                    return model;
                }
                #endregion

                #region PendingAuditApproval
                else if (model.Status == "PendingAuditApproval")
                {
                    string subject = "PendingAuditApproval"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "PendingAuditApproval";

                    var bodyString = "<tr>" +

                                    "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +


                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are seeking for your approval for the upcoming audit <b>" + model.Name + "</b> </span></font></div>" +

                                                     "<div> &nbsp;</div> " +
                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +
                                    "<tr>" +
                                        "<td  style='height:50px;'> " +

                                            "<div style = 'margin-left:10px;color:black;'> " +
                                                 "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                              "</div> " +

                                        "</td>" +

                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +


                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +



                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;

                    sent = PendingAuditApprovalSendMail(model.To, bodyString, message);

                    return model;
                }
                #endregion

                #region ConfirmationOfAuditApproval
                else if (model.Status == "ConfirmationOfAuditApproval")
                {
                    string subject = "ConfirmationOfAuditApproval"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "ConfirmationOfAuditApproval";

                    var bodyString = "<tr>" +

                                    "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +


                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" + "<div> &nbsp;</div> " +
                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" + "<div> &nbsp;</div> " +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The upcoming audit <b>" + model.Name + "</b> has been approved, requesting to initiate audit.  </span></font></div>" + "<div> &nbsp;</div> " +

                                                     "<div> &nbsp;</div> " +
                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +
                                    "<tr>" +
                                        "<td  style='height:50px;'> " +

                                            "<div style = 'margin-left:10px;color:black;'> " +
                                                 "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                              "</div> " +

                                        "</td>" +

                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +

                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +



                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                    "</tr> ";


                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.EmailAddress));
                    message.Subject = subject;

                    sent = SendAuditApprovalMailToTeam(model.EmailAddress, bodyString, message);

                    return model;
                }
                #endregion

                #region AuditBranchFeedbackUser
                else if (model.Status == "BranchFeedbackUserMail")
                {
                    string subject = "AuditBranchFeedbackUser"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "AuditBranchFeedbackUser";

                    var bodyString = "<tr>" +

                                    "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                                    "<div> &nbsp;</div> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We trust this message finds you well. </span></font></div>" +

                                                    "<div> &nbsp;</div> " +

                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are requesting you to provide your Feedback/Response on the audit observations (<b>" + model.Name + "</b>) </span></font></div>" +

                                                    "<div> &nbsp;</div> " +
                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +


                                    "<tr>" +
                                        "<td  style='height:50px;'> " +
                                                "<div style = 'margin-left:10px;color:black;'> " +
                                                        "<a href='" + url + "' style = 'text-decoration: none;margin:0px 0px 0px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                                       "<div> &nbsp;</div> " +

                                                "</div> " +
                                        "</td>" +
                                    "</tr> " +


                                     "<tr>" +
                                        "<td  style='height:50px;'> " +

                                                "<div style = 'margin-left:10px;color:black;'> " +


                                                        "<font size='2'><strong><span style='font-size:20pt;margin-top:5px;'>Process Flow – Response on Audit Issues</span></strong></font>" +

                                                        "<div> &nbsp;</div> " +

                                                "</div> " +
                                        "</td>" +
                                    "</tr> " +


                                    "<tr>" +

                                        "<td  style='height:50px;'> " +
                                                "<div style = 'margin-left:10px;color:black;'> " +


                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Email Link </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Auditee Response  </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Add Branch Feedback  </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Select Issue Heading  </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> View Audit Issue Preview </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'>Feedback Heading  </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Provide response heading (Optional)  </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Details – Provide response details  </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Provide supporting docs (if applicable)  </span></font>" +
                                                        "<font size = '2' ><span style='font-weight:bold;font-size:50px;' class=arrow>&rarr;</span></font>" +

                                                        "<font size = '2' ><span style='font-size:10pt;margin-top:15px;border-radius:5px;padding:20px;color:white;background-color: #114232;display: inline-block;'> Save – (Draft) -> Branch Feedback (To issue to Audit Team)  </span></font>" +

                                                "</div> " +
                                        "</td>" +
                                    "</tr> " +


                                    "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +

                                                    "<div> &nbsp;</div> " +

                                               "</div> " +
                                         "</td> " +




                                    "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +

                                        "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +


                                            "</div> " +
                                       "</tr> " +


                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +


                                            "</div> " +
                                       "</tr> " +


                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;
                    sent = AuditBranchFeedbackSendMail(model.To, bodyString, message, model.WebRoot);
                }
                #endregion

                else if (model.Status == "PendingAuditApproval2")
                {
                    string subject = "PendingAuditApproval"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "PendingAuditApproval";

                    var bodyString = "<tr>" +

                                    "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +


                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" + "<div> &nbsp;</div> " +
                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" + "<div> &nbsp;</div> " +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The upcoming audit <b>" + model.Name + "</b> has been approved, requesting to initiate audit.  </span></font></div>" + "<div> &nbsp;</div> " +

                                                     "<div> &nbsp;</div> " +
                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +
                                    "<tr>" +
                                        "<td  style='height:50px;'> " +

                                            "<div style = 'margin-left:10px;color:black;'> " +
                                                 "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                              "</div> " +

                                        "</td>" +

                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +


                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +



                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;
                    sent = SendAuditApprovalMailToTeam2(model.To, bodyString, message);
                    //return sent;
                }

                else if (model.Status == "PendingAuditApproval3")
                {
                    string subject = "PendingAuditApproval3"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "PendingAuditApproval3";

                    var bodyString = "<tr>" +

                                    "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +


                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>We are seeking for your approval for the upcoming audit <b>" + model.Name + "</b> </span></font></div>" +
                                                     "<div> &nbsp;</div> " +

                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +
                                    "<tr>" +
                                        "<td  style='height:50px;'> " +

                                                 "<div style = 'margin-left:10px;color:black;'> " +

                                                      "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +

                                                  "</div> " +


                                        "</td>" +

                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +


                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +



                                        "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                               //"<div> &nbsp;</div> " +

                                               "</div> " +
                                       "</tr> " +

                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;

                    sent = PendingAuditApprovalMail(model.To, bodyString, message);

                }

                #region IssuedeadLineLapsed
                else if (model.Status == "IssuedeadLineLapsed")
                {
                    string subject = "IssuedeadLineLapsed"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "IssuedeadLineLapsed";

                    var bodyString = "<tr>" +

                                    "</tr>" +

                                    "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +
                                            "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" +
                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The audit issue- <b>" + model.Name + "</b> has been submitted for your review </span></font></div>" +

                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +


                                    "<tr>" +
                                        "<td  style='height:50px;'> " +
                                                 "<div style = 'margin-left:10px;color:black;'> " +
                                                      "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                                  "</div> " +
                                        "</td>" +


                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +
                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +

                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +


                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +
                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +

                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +


                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>GDIC, IAC</span></font></div>" +
                                               "</div> " +
                                       "</tr> " +

                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;

                    sent = IssuedeadLineLapsedSendMail(model.To, bodyString, message);

                }
                #endregion

                #region SendFeedbackReviewedlMail
                else if (model.Status == "SendFeedbackReviewedlMail")
                {
                    string subject = "SendFeedbackReviewedlMail"; string mailBody; string url = model.URL; string userFullName = "Sir"; Attachment pdfLink;
                    var sent = false;
                    subject = "SendFeedbackReviewedlMail";

                    var bodyString = "<tr>" +

                                    "</tr>" +
                                  "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +


                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> Hello! </span></font></div>" + "<div> &nbsp;</div> " +
                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'> We hope this email finds you well. </span></font></div>" + "<div> &nbsp;</div> " +
                                                    "<div><font face = 'Calibri' size = '2'><span style = 'font-size:10pt'>The audit issue  -<b>" + model.Name + "</b> has been reviewed and change request has been provided.  </span></font></div>" + "<div> &nbsp;</div> " +

                                                     "<div> &nbsp;</div> " +
                                               "</div> " +
                                           "</td> " +
                                    "</tr> " +
                                    "<tr>" +
                                        "<td  style='height:50px;'> " +

                                            "<div style = 'margin-left:10px;color:black;'> " +
                                                 "<a href='" + url + "' style = 'text-decoration: none;margin:10px 0px 30px 0px;border-radius:4px;padding:3px 5px;border: 0;color:#fff;background-color:#005daa;'>Click here </a>" +
                                              "</div> " +

                                        "</td>" +

                                        "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:17px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Thank you for your attention.</span></font></div>" +

                                               "</div> " +
                                            "</td> " +
                                       "</tr> " +

                                       "<tr> " +

                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                    "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Regards</span></font></div>" +


                                               "</div> " +
                                            "</td> " +

                                       "</tr> " +



                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Internal Audit & Compliance</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                       "<tr> " +
                                        "<td style = 'text-align:left;'>" +
                                            "<div style = 'margin-left:10px;color:black;'> " +

                                                 "<div style='margin:0px 0 0px 0'><font size = '2' ><span style='font-size:10pt'>Green Delta Insurance Company Limited</span></font></div>" +
                                                 "<div> &nbsp;</div> " +

                                            "</div> " +
                                       "</tr> " +


                                    "</tr> ";

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.To));
                    message.Subject = subject;

                    sent = SendFeedbackReviewedlMailToTeam(model.To, bodyString, message);

                }
                #endregion

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SendFeedbackReviewedlMailToTeam(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
                mail.To.Add(To);
                mail.Subject = "Feedback Or Change Request of Audit Issue";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }

        }

        public static bool IssuedeadLineLapsedSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "Issue DeadLin Lapsed";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }

            }

        }

        public static bool PendingAuditApprovalMail(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "PendingAuditApproval";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }

            }

        }

        public static bool SendAuditApprovalMailToTeam2(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                //mail.From = new MailAddress("iac@green-delta.com");
                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
                mail.To.Add(To);
                mail.Subject = "Confirmation Of Audit Approval";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }

        }

        public static bool AuditReportSendMail(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");

                mail.To.Add(To);
                mail.Subject = "Final Audit Report";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }


        public static bool FollowUpAuditIssueEamilSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "FollowUp Audit Issue";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }

            }
        }
        public static bool AuditIssueSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {


                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
                mail.To.Add(To);
                mail.Subject = "AuditIssue";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }
        public static bool TotalPendingIssuesReviewSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";

            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com");
                mail.To.Add(To);
                mail.Subject = "Total Pending Issues Review";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }


        public static bool PendingAuditApprovalSendMail(string To, string body, MailMessage message)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
                mail.To.Add(To);
                mail.Subject = "Pending Audit Approval";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }

        }

        public static bool SendAuditApprovalMailToTeam(string To, string body, MailMessage message)
        {

            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);

            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");
                mail.To.Add(To);
                mail.Subject = "Confirmation Of Audit Approval";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }

            }

        }

        public static bool AuditBranchFeedbackSendMail(string To, string body, MailMessage message, string str)
        {
            var content = "<html>" +
                                "<head>" +
                                "</head>" +
                                "<body>" +
                                    "<table border='0' width='100%' style='margin:auto;padding:10px;background-color: #F3F3F3;border:1px solid #0C143B;'>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%'>" +
                                                    "<tr>" +
                                                        "<td style='text-align: center;'>" +
                                                            "<h1>" +
                                                                "<img src='cid:companylogo'  width='350px' height='80px'/> " +
                                                            "</h1>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' cellpadding='0' cellspacing='0' style='text-align:center;width:100%;background-color: #FFFF;'>" +
                                                body
                                                + "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td>" +
                                                "<table border='0' width='100%' style='border-radius: 5px;text-align: center;'>" +


                                                    "<tr>" +
                                                        "<td>" +
                                                            "<div style='margin-top: 20px;color:black;'>" +


                                                            "</div>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</body>" +
                                "</html>";


            AlternateView av1 = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            message.IsBodyHtml = true;

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress("iac@green-delta.com", "Internal Audit & Compliance");

                mail.To.Add(To);
                mail.Subject = "Audit BranchFeedback";
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("mail.green-delta.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("iac@green-delta.com", "9ty@u3{24E;%");
                    smtp.EnableSsl = true;
                    try
                    {
                        smtp.Send(mail);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false;
                    }
                }
            }
        }

    }
}
