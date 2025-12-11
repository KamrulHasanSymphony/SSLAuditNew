using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Repository.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKFraudIrregularitiesInternalControlPreferencesCBS;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKAuditOfficePreferencesCBS
{
	public class BKFraudIrregularitiesInternalControlPreferenceCBSService : IBKFraudIrregularitiesInternalControlPreferenceCBSService
    {
		private IUnitOfWork _unitOfWork;

		public BKFraudIrregularitiesInternalControlPreferenceCBSService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<BKFraudIrregularitiesInternalControlPreferenceCBS>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {
                    var records = context.Repositories.BKFraudIrregularitiesInternalControlPreferenceCBSRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKFraudIrregularitiesInternalControlPreferenceCBS>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKFraudIrregularitiesInternalControlPreferenceCBS>>()
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
                        context.Repositories.BKFraudIrregularitiesInternalControlPreferenceCBSRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKFraudIrregularitiesInternalControlPreferenceCBS>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {
                    var records = context.Repositories.BKFraudIrregularitiesInternalControlPreferenceCBSRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKFraudIrregularitiesInternalControlPreferenceCBS>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKFraudIrregularitiesInternalControlPreferenceCBS>>()
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
                    var records = context.Repositories.BKFraudIrregularitiesInternalControlPreferenceCBSRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS> Insert(BKFraudIrregularitiesInternalControlPreferenceCBS model)
		{
            string CodeGroup = "RAPS";
            string CodeName = "BKFraudIrregularitiesInternalControlPreferenceCBS";

            using (var context = _unitOfWork.CreateCBS())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = "NA";

                    if (Code != "" || Code != null)
                    {
                        //model.Code = "";

						BKFraudIrregularitiesInternalControlPreferenceCBS master = context.Repositories.BKFraudIrregularitiesInternalControlPreferenceCBSRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        

		

		public ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS> Update(BKFraudIrregularitiesInternalControlPreferenceCBS model)
		{
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {

					BKFraudIrregularitiesInternalControlPreferenceCBS master = context.Repositories.BKFraudIrregularitiesInternalControlPreferenceCBSRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS>()
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
