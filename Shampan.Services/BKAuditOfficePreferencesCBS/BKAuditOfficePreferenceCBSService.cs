using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Repository.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKAuditOfficePreferencesCBS
{
    public class BKAuditOfficePreferenceCBSService : IBKAuditOfficePreferenceCBSService
    {
        private IUnitOfWork _unitOfWork;

        public BKAuditOfficePreferenceCBSService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<BKAuditOfficePreferenceCBS> AuditOfficePreferenceCBSTempInsert(BKAuditOfficePreferenceCBS model)
        {
            string CodeGroup = "RAPS";
            string CodeName = "BKAuditOfficePreferenceCBS";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKAuditOfficePreferenceCBS>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    //int delete = context.Repositories.BKAuditOfficePreferenceCBSRepository.Delete("BKAuditOfficePreferenceCBSTemp",null,null);

                    BKAuditOfficePreferenceCBS master = context.Repositories.BKAuditOfficePreferenceCBSRepository.AuditOfficePreferenceCBSTempInsert(model);

                    if (master.Id <= 0)
                    {
                        return new ResultModel<BKAuditOfficePreferenceCBS>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }

                    context.SaveChanges();

                    return new ResultModel<BKAuditOfficePreferenceCBS>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };





                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditOfficePreferenceCBS>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<BKAuditOfficePreferenceCBS> Delete(int id)
        {


            using (var context = _unitOfWork.Create())
            {
                try
                {
                  

                     int delete = context.Repositories.BKAuditOfficePreferenceCBSRepository.Delete("BKAuditOfficePreferenceCBSTemp", null, null);
                     context.SaveChanges();

                    return new ResultModel<BKAuditOfficePreferenceCBS>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        //Exception = e
                    };



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditOfficePreferenceCBS>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

        }

        public ResultModel<List<BKAuditOfficePreferenceCBS>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {
                    var records = context.Repositories.BKAuditOfficePreferenceCBSRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditOfficePreferenceCBS>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditOfficePreferenceCBS>>()
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
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {
                    int count =
                        context.Repositories.BKAuditOfficePreferenceCBSRepository.GetCount(tableName,
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




        public ResultModel<List<BKAuditOfficePreferenceCBS>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {
                    var records = context.Repositories.BKAuditOfficePreferenceCBSRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditOfficePreferenceCBS>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditOfficePreferenceCBS>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }


        }


        public ResultModel<List<BKAuditOfficePreferenceCBS>> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditOfficePreferenceCBSRepository.GetIndexDataTemp(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditOfficePreferenceCBS>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditOfficePreferenceCBS>>()
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
            using (var context = _unitOfWork.CreateCBS())
            {
                try
                {
                    var records = context.Repositories.BKAuditOfficePreferenceCBSRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<BKAuditOfficePreferenceCBS> Insert(BKAuditOfficePreferenceCBS model)
        {
            string CodeGroup = "RAPS";
            string CodeName = "BKAuditOfficePreferenceCBS";

            using (var context = _unitOfWork.CreateCBS())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKAuditOfficePreferenceCBS>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = "NA";

                    if (Code != "" || Code != null)
                    {
                        //model.Code = "";

                        BKAuditOfficePreferenceCBS master = context.Repositories.BKAuditOfficePreferenceCBSRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKAuditOfficePreferenceCBS>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKAuditOfficePreferenceCBS>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKAuditOfficePreferenceCBS>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditOfficePreferenceCBS>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

        }





        public ResultModel<BKAuditOfficePreferenceCBS> Update(BKAuditOfficePreferenceCBS model)
        {
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {

                    BKAuditOfficePreferenceCBS master = context.Repositories.BKAuditOfficePreferenceCBSRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditOfficePreferenceCBS>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditOfficePreferenceCBS>()
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
