using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.Mail;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.Mail
{
    public class MailsService : IMailService
    {
        private IUnitOfWork _unitOfWork;

        public MailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<AuditMail> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<AuditMail>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.MailRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditMail>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditMail>>()
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
                        context.Repositories.MailRepository.GetCount(tableName,
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
        
        public ResultModel<List<AuditMail>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.MailRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditMail>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditMail>>()
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
                    var records = context.Repositories.MailRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<AuditMail> Insert(AuditMail model)
        {

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<AuditMail>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    AuditMail master = context.Repositories.MailRepository.Insert(model);

                    if (master.Id < 0)
                    {
                        return new ResultModel<AuditMail>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }


                    return new ResultModel<AuditMail>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.MailSuccess,
                        Data = master
                    };

                  
                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMail>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

        }

        public ResultModel<AuditMail> SendEmail(AuditMail model)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<AuditMail>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    AuditMail master = context.Repositories.MailRepository.SendEmail(model);

                    if (master.Id < 0)
                    {
                        return new ResultModel<AuditMail>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }

                    return new ResultModel<AuditMail>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.MailSuccess,
                        Data = master
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMail>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<AuditMail> Update(AuditMail model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.MailRepository.CheckPostStatus("AuditMail", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<AuditMail>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    AuditMail master = context.Repositories.MailRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<AuditMail>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditMail>()
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
