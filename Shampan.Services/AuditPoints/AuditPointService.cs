using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.AuditPoints;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.AuditPoints
{
	public class AuditPointService : IAuditPointService
    {
		private IUnitOfWork _unitOfWork;

		public AuditPointService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<AuditPoint> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<AuditPoint>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.AuditPointRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditPoint>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditPoint>>()
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
                        context.Repositories.AuditPointRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<AuditPoint>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.AuditPointRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<AuditPoint>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<AuditPoint>>()
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
                    var records = context.Repositories.AuditPointRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

                    var record = context.Repositories.AuditPointRepository.GetSettingsValue(conditionalFields, conditionalValue);
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

        public ResultModel<AuditPoint> Insert(AuditPoint model)
		{
            string CodeGroup = "RAPS";
            string CodeName = "AuditPoint";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<AuditPoint>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    //string Code = context.Repositories.AuditPointRepository.CodeGeneration(CodeGroup, CodeName);
                    string Code = "NA";

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

						AuditPoint master = context.Repositories.AuditPointRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<AuditPoint>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<AuditPoint>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<AuditPoint>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditPoint>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

		public ResultModel<AuditPoint> Update(AuditPoint model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					AuditPoint master = context.Repositories.AuditPointRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<AuditPoint>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<AuditPoint>()
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
