using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditTemlateMasters;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKAuditTemlateMasters
{
    public class BKAuditTemlateMasterService : IBKAuditTemlateMasterService
    {
        private IUnitOfWork _unitOfWork;

        public BKAuditTemlateMasterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<BKAuditTemlateMaster> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<BKAuditTemlateMaster>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditTemlateMasterRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditTemlateMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditTemlateMaster>>()
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
                        context.Repositories.BKAuditTemlateMasterRepository.GetCount(tableName,
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

        public ResultModel<List<BKAuditTemplateDetails>> GetDetailsAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditTemlateMasterRepository.GetDetailsAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditTemplateDetails>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditTemplateDetails>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<CheckListItem>> GetIndexCheckListItemData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditTemlateMasterRepository.GetIndexCheckListItemData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<CheckListItem>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<CheckListItem>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<List<BKAuditTemlateMaster>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditTemlateMasterRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditTemlateMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditTemlateMaster>>()
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
                    var records = context.Repositories.BKAuditTemlateMasterRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<List<MappingData>> GetIndexMappingData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditTemlateMasterRepository.GetIndexMappingData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<MappingData>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<MappingData>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<int> GetIndexMappingDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {
                try
                {
                    var records = context.Repositories.BKAuditTemlateMasterRepository.GetIndexMappingDataCount(index, conditionalFields, conditionalValue);
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

        public int GetMappingCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    int count =
                        context.Repositories.BKAuditTemlateMasterRepository.GetMappingCount(tableName,
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

        public ResultModel<BKAuditTemlateMaster> Insert(BKAuditTemlateMaster model)
        {
            string CodeGroup = "ATM";
            string CodeName = "BKAuditTemlateMaster";

            BKAuditTemplateDetails BKAuditTemplateDetail = new BKAuditTemplateDetails();

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKAuditTemlateMaster>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.BKAuditTemlateMasterRepository.CodeGeneration(CodeGroup, CodeName);

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

                        BKAuditTemlateMaster master = context.Repositories.BKAuditTemlateMasterRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKAuditTemlateMaster>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }


                        foreach (var detail in model.BKAuditTemplateDetailsList)
                        {
                            detail.BKAuditTemlateMasterId = master.Id;
                            detail.BKAuditOfficeTypeId = master.BKAuditOfficeTypeId;
                            detail.BKAuditComplianceId = detail.BKAuditComplianceId;
                            detail.BKCheckListTypeId = detail.BKCheckListTypeId;
                            detail.BKCheckListSubTypeId = detail.BKCheckListSubTypeId;

                            //detail.Audit.CreatedBy = model.Audit.CreatedBy;
                            //detail.Audit.CreatedOn = model.Audit.CreatedOn;
                            //detail.Audit.CreatedFrom = model.Audit.CreatedFrom;

                            if (detail.CheckListItemList.Count() != 0)
                            {
                                foreach (var item in detail.CheckListItemList)
                                {
                                    detail.BKCheckListItemId = item.Id;
                                    detail.Status = item.Status;
                                    detail.IsFieldType = item.IsFieldType;

                                    if (item.Status)
                                    {
                                        BKAuditTemplateDetail = context.Repositories.BKAuditTemlateMasterRepository.InsertDetails(detail);

                                    }

                                }
                            }
                        

                            if (BKAuditTemplateDetail.Id == 0)
                            {
                                return new ResultModel<BKAuditTemlateMaster>()
                                {
                                    Status = Status.Fail,
                                    Message = MessageModel.DetailInsertFailed,
                                    Data = master
                                };
                            }

                        }

                        context.SaveChanges();

                        return new ResultModel<BKAuditTemlateMaster>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKAuditTemlateMaster>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditTemlateMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }

        }

        public ResultModel<BKAuditTemlateMaster> MultiplePost(BKAuditTemlateMaster model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.BKAuditTemlateMasterRepository.CheckPostStatus("BKAuditTemlateMaster", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<BKAuditTemlateMaster>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    BKAuditTemlateMaster master = context.Repositories.BKAuditTemlateMasterRepository.MultiplePost(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditTemlateMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditTemlateMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<BKAuditTemlateMaster> MultipleUnPost(BKAuditTemlateMaster model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    if (model.Operation == "unpost")
                    {
                        bool CheckUpPostStatus = false;
                        CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("BKAuditTemlateMaster", new[] { "Id" }, new[] { model.Id.ToString() });
                        if (CheckUpPostStatus)
                        {
                            return new ResultModel<BKAuditTemlateMaster>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.UpPostAlready,

                            };
                        }
                    }

                    BKAuditTemlateMaster master = context.Repositories.BKAuditTemlateMasterRepository.MultipleUnPost(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditTemlateMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UnPostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditTemlateMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UnPostFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<BKAuditTemlateMaster> Update(BKAuditTemlateMaster model)
        {
            using (var context = _unitOfWork.Create())
            {

                BKAuditTemplateDetails BKAuditTemplateDetail = new BKAuditTemplateDetails();


                try
                {

                    BKAuditTemlateMaster master = context.Repositories.BKAuditTemlateMasterRepository.Update(model);


                    if (master.Id > 0)
                    {
                        int recordCount = context.Repositories.BKAuditTemlateMasterRepository.DetailsDelete(
                            TableName.BKAuditTemplateDetails, 
                            new[] { "BKAuditTemlateMasterId" },new[] { master.Id.ToString() });


                        if (recordCount < 0)
                        {
                            return new ResultModel<BKAuditTemlateMaster>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.DeleteFail,
                            };
                        }



                        foreach (var detail in model.BKAuditTemplateDetailsList)
                        {
                            detail.BKAuditTemlateMasterId = master.Id;
                            detail.BKAuditOfficeTypeId = master.BKAuditOfficeTypeId;
                            detail.BKAuditComplianceId = detail.BKAuditComplianceId;
                            detail.BKCheckListTypeId = detail.BKCheckListTypeId;
                            detail.BKCheckListSubTypeId = detail.BKCheckListSubTypeId;

                            //detail.Audit.LastUpdateBy = model.Audit.LastUpdateBy;
                            //detail.Audit.LastUpdateOn = model.Audit.LastUpdateOn;
                            //detail.Audit.LastUpdateFrom = model.Audit.LastUpdateFrom;

                            if (detail.CheckListItemList.Count() != 0)
                            {
                                foreach (var item in detail.CheckListItemList)
                                {
                                    detail.BKCheckListItemId = item.Id;
                                    detail.Status = item.Status;
                                    detail.IsFieldType = item.IsFieldType;

                                    if (item.Status)
                                    {
                                        BKAuditTemplateDetail = context.Repositories.BKAuditTemlateMasterRepository.InsertDetails(detail);


                                        if (BKAuditTemplateDetail.Id == 0)
                                        {
                                            return new ResultModel<BKAuditTemlateMaster>()
                                            {
                                                Status = Status.Fail,
                                                Message = MessageModel.DetailInsertFailed,
                                                Data = master
                                            };
                                        }

                                    }

                                }
                            }


                           

                        }


                        //foreach (ICReceiptDetails detail in master.ICReceiptDetailsList)
                        //{
                        //    detail.ICReceiptId = master.Id;
                        //    detail.BranchId = 1;
                        //    detail.CompanyId = 1;
                        //    var returnDetail = context.Repositories.ICReceiptDetailsRepository.Insert(detail);
                        //    if (returnDetail.Id == 0)
                        //    {
                        //        return new ResultModel<ICReceipts>()
                        //        {
                        //            Status = Status.Fail,
                        //            Message = MessageModel.DetailInsertFailed,
                        //            Data = master
                        //        };
                        //    }
                        //}

                    }




                    context.SaveChanges();

                    return new ResultModel<BKAuditTemlateMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditTemlateMaster>()
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
