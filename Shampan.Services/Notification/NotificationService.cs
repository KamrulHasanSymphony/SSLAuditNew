using Microsoft.Data.SqlClient;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Notification;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private IUnitOfWork _unitOfWork;
        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }
        public ResultModel<Notifications> Delete(int id)
        {
            throw new NotImplementedException();
        }
        public ResultModel<List<Notifications>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    var records = context.Repositories.NotificationRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Notifications>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Notifications>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }
        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    int count =
                        context.Repositories.NotificationRepository.GetCount(tableName,
                            "Id", null, null);
                    context.SaveChanges();
                    return count;
                }
                catch (Exception e)
                {
                    context.RollBack();

                    return 0;
                }

            }
        }

        public ResultModel<List<Notifications>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    index.OrderName = "Id";
                    index.orderDir = "Desc";
                    var records = context.Repositories.NotificationRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<Notifications>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<Notifications>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    var records = context.Repositories.NotificationRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<int>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<int>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<Notifications> Insert(Notifications model)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<Notifications>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }
                  

                    Notifications master = context.Repositories.NotificationRepository.Insert(model);

                    if (master.Id <= 0)
                    {
                        return new ResultModel<Notifications>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }

                    context.SaveChanges();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<Notifications> InsertNotification(string URL, string Name, int auditId, int commonId, string UserName, string notification,List<AuditUser> teamUsers)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {

                    string userIds = string.Join(",", teamUsers.Select(u => u.UserId));

                    Notifications model = new Notifications();

                    model.URL = URL;
                    model.Name = Name;
                    model.AuditId = auditId;
                    model.CommonId = commonId;
                    model.Audit.CreatedBy = UserName;
                    model.Audit.CreatedOn = DateTime.Now;
                    model.NotificationStatus = notification;
                    model.UserId = userIds;

                    if (model == null)
                    {
                        return new ResultModel<Notifications>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    Notifications master = context.Repositories.NotificationRepository.Insert(model);

                    if (master.Id <= 0)
                    {
                        return new ResultModel<Notifications>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }

                    context.SaveChanges();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<Notifications> InsertNotification(string URL, string Name, int auditId, int commonId, string UserName, string notification, List<TeamMembers> users)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {

                    string userIds = string.Join(",", users.Select(u => u.UserId));

                    Notifications model = new Notifications();

                    model.URL = URL;
                    model.Name = Name;
                    model.AuditId = auditId;
                    model.CommonId = commonId;
                    model.Audit.CreatedBy = UserName;
                    model.Audit.CreatedOn = DateTime.Now;
                    model.NotificationStatus = notification;
                    model.UserId = userIds;

                    if (model == null)
                    {
                        return new ResultModel<Notifications>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    Notifications master = context.Repositories.NotificationRepository.Insert(model);

                    if (master.Id <= 0)
                    {
                        return new ResultModel<Notifications>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }

                    context.SaveChanges();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<Notifications> Update(Notifications model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    if (model.NotificationStatus == "Audit")
                    {
                        model.IsAudit = true;
                    }
                    else if(model.NotificationStatus == "AuditApprove")
                    {
                        model.IsAuditApprove = true;
                    }
                    else if(model.NotificationStatus == "issueNotification")
                    {
                        model.IsIssue = true;
                    }
                    else if(model.NotificationStatus == "feedbackNotification")
                    {
                        model.IsFeedback = true;
                    }
                    else if (model.NotificationStatus == "branchFeedbackNotification")
                    {
                        model.IsBranchFeedBack = true;
                    }

                    Notifications master = context.Repositories.NotificationRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }

        }

        public ResultModel<Notifications> UpdateNotification(string id, string userName, string notificationType) 
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    Notifications model = new Notifications();
                    model.Id = Convert.ToInt32(id);
                    model.Audit.LastUpdateOn = DateTime.Now;
                    model.Audit.LastUpdateBy = userName;
                    model.NotificationStatus = notificationType;

                    if (model.NotificationStatus == "Audit")
                    {
                        model.IsAudit = true;
                    }
                    else if (model.NotificationStatus == "AuditApproved")
                    {
                        model.IsAuditApprove = true;
                    }
                    else if (model.NotificationStatus == "issueNotification")
                    {
                        model.IsIssue = true;
                    }
                    else if (model.NotificationStatus == "feedbackNotification")
                    {
                        model.IsFeedback = true;
                    }
                    else if (model.NotificationStatus == "branchFeedbackNotification")
                    {
                        model.IsBranchFeedBack = true;
                    }

                    Notifications master = context.Repositories.NotificationRepository.Update(model);

                    context.SaveChanges();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<Notifications>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }


    }
}
