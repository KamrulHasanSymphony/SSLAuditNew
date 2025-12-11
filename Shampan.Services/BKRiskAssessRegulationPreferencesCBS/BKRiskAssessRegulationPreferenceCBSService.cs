using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Repository.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficePreferencesCBS;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKRiskAssessRegulationPreferencesCBS;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKRiskAssessRegulationPreferencesCBS
{
	public class BKRiskAssessRegulationPreferenceCBSService : IBKRiskAssessRegulationPreferenceCBSService
    {
		private IUnitOfWork _unitOfWork;

		public BKRiskAssessRegulationPreferenceCBSService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKRiskAssessRegulationPreferenceCBS> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<BKRiskAssessRegulationPreferenceCBS>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {
                    var records = context.Repositories.BKRiskAssessRegulationPreferenceCBSRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKRiskAssessRegulationPreferenceCBS>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKRiskAssessRegulationPreferenceCBS>>()
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
                        context.Repositories.BKRiskAssessRegulationPreferenceCBSRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKRiskAssessRegulationPreferenceCBS>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {
                    var records = context.Repositories.BKRiskAssessRegulationPreferenceCBSRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKRiskAssessRegulationPreferenceCBS>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKRiskAssessRegulationPreferenceCBS>>()
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
                    var records = context.Repositories.BKRiskAssessRegulationPreferenceCBSRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<BKRiskAssessRegulationPreferenceCBS> Insert(BKRiskAssessRegulationPreferenceCBS model)
		{
            string CodeGroup = "RAPS";
            string CodeName = "BKRiskAssessRegulationPreferenceCBS";

            using (var context = _unitOfWork.CreateCBS())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKRiskAssessRegulationPreferenceCBS>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = "NA";

                    if (Code != "" || Code != null)
                    {
                        //model.Code = "";

						BKRiskAssessRegulationPreferenceCBS master = context.Repositories.BKRiskAssessRegulationPreferenceCBSRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKRiskAssessRegulationPreferenceCBS>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKRiskAssessRegulationPreferenceCBS>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKRiskAssessRegulationPreferenceCBS>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKRiskAssessRegulationPreferenceCBS>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        

		

		public ResultModel<BKRiskAssessRegulationPreferenceCBS> Update(BKRiskAssessRegulationPreferenceCBS model)
		{
            using (var context = _unitOfWork.CreateCBS())
            {

                try
                {

					BKRiskAssessRegulationPreferenceCBS master = context.Repositories.BKRiskAssessRegulationPreferenceCBSRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKRiskAssessRegulationPreferenceCBS>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKRiskAssessRegulationPreferenceCBS>()
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
