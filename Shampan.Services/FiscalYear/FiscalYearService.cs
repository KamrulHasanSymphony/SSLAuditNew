using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.FiscalYear;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using Shampan.Models.AuditModule;
using UnitOfWork.Interfaces;

namespace Shampan.Services.FiscalYear
{
    public class FiscalYearService : IFiscalYearService
    {
        private IUnitOfWork _unitOfWork;

        public FiscalYearService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<FiscalYearVM> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<FiscalYearVM>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.FiscalYearRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<FiscalYearVM>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<FiscalYearVM>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }


        }

        public ResultModel<List<FiscalYearDetailVM>> GetAllDetail(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                   
                    var records = context.Repositories.FiscalYearRepository.GetAllDetail(conditionalFields, conditionalValue);

                    context.SaveChanges();

                    return new ResultModel<List<FiscalYearDetailVM>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<FiscalYearDetailVM>>()
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
                        context.Repositories.FiscalYearRepository.GetCount(tableName,
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




        public ResultModel<List<FiscalYearVM>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.FiscalYearRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<FiscalYearVM>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<FiscalYearVM>>()
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
                    var records = context.Repositories.FiscalYearRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<string> GetSettingsValue(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    var record = context.Repositories.FiscalYearRepository.GetSettingsValue(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<string>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = record
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<string>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<FiscalYearVM> Insert(FiscalYearVM model)
        {


            using (var context = _unitOfWork.Create())
            {
                try
                {


                    bool CheckFiscalYearStatus = false;
                    CheckFiscalYearStatus = context.Repositories.FiscalYearRepository.CheckFiscalYearStatus("FiscalYears", new[] { "Year" }, new[] { model.Year.ToString() });
                    if (CheckFiscalYearStatus)
                    {
                        return new ResultModel<FiscalYearVM>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.FiscalYearStatusAlready,

                        };
                    }


                    if (model == null)
                    {
                        return new ResultModel<FiscalYearVM>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    FiscalYearVM master = context.Repositories.FiscalYearRepository.Insert(model);


                    if (master.Id <= 0)
                    {
                        return new ResultModel<FiscalYearVM>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }


                    foreach(var detail in model.fiscalYearDetails)
                    {
                        detail.FiscalYearId = master.Id;
                        detail.Year = master.Year;

                        FiscalYearDetailVM masterDetails = context.Repositories.FiscalYearRepository.InsertDetails(detail);
                       
                    }


                    context.SaveChanges();



                    return new ResultModel<FiscalYearVM>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };


                }


                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<FiscalYearVM>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }


            }

        }

        public ResultModel<FiscalYearDetailVM> InsertDetails(FiscalYearDetailVM model)
        {
            throw new NotImplementedException();
        }

        public ResultModel<FiscalYearVM> Update(FiscalYearVM model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {

                    FiscalYearVM master = context.Repositories.FiscalYearRepository.Update(model);


                    if (master.Id <= 0)
                    {
                        return new ResultModel<FiscalYearVM>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }


                    foreach (var detail in model.fiscalYearDetails)
                    {
                        detail.FiscalYearId = master.Id;
                        detail.Year = master.Year;

                        FiscalYearDetailVM masterDetails = context.Repositories.FiscalYearRepository.UpdateDetail(detail);

                    }


                    context.SaveChanges();

                    return new ResultModel<FiscalYearVM>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<FiscalYearVM>()
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
