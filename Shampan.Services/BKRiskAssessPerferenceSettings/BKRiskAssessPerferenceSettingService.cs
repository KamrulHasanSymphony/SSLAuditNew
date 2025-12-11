using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKRiskAssessPerferenceSettings;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKRiskAssessPerferenceSettings
{
	public class BKRiskAssessPerferenceSettingService : IBKRiskAssessPerferenceSettingService
    {
		private IUnitOfWork _unitOfWork;

		public BKRiskAssessPerferenceSettingService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKRiskAssessPerferenceSetting> Delete(int id)
		{
            using (var context = _unitOfWork.Create())
            {
                try
                {

                    int delete = context.Repositories.BKRiskAssessPerferenceSettingRepository.Delete("BKRiskAssessRegulationPreferenceCBSTemp", null, null);
                    context.SaveChanges();

                    return new ResultModel<BKRiskAssessPerferenceSetting>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        //Exception = e
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKRiskAssessPerferenceSetting>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<List<BKRiskAssessPerferenceSetting>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKRiskAssessPerferenceSettingRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKRiskAssessPerferenceSetting>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKRiskAssessPerferenceSetting>>()
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
                        context.Repositories.BKRiskAssessPerferenceSettingRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKRiskAssessPerferenceSetting>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKRiskAssessPerferenceSettingRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKRiskAssessPerferenceSetting>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKRiskAssessPerferenceSetting>>()
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
                    var records = context.Repositories.BKRiskAssessPerferenceSettingRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<List<BKRiskAssessPerferenceSetting>> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKRiskAssessPerferenceSettingRepository.GetIndexDataTemp(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKRiskAssessPerferenceSetting>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKRiskAssessPerferenceSetting>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<BKRiskAssessPerferenceSetting> Insert(BKRiskAssessPerferenceSetting model)
		{
            string CodeGroup = "RAPS";
            string CodeName = "BKRiskAssessPerferenceSetting";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKRiskAssessPerferenceSetting>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.BKRiskAssessPerferenceSettingRepository.CodeGeneration(CodeGroup, CodeName);

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

						BKRiskAssessPerferenceSetting master = context.Repositories.BKRiskAssessPerferenceSettingRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKRiskAssessPerferenceSetting>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKRiskAssessPerferenceSetting>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKRiskAssessPerferenceSetting>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKRiskAssessPerferenceSetting>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        public ResultModel<BKRiskAssessPerferenceSetting> MultiplePost(BKRiskAssessPerferenceSetting model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.BKRiskAssessPerferenceSettingRepository.CheckPostStatus("BKRiskAssessPerferenceSetting", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<BKRiskAssessPerferenceSetting>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    BKRiskAssessPerferenceSetting master = context.Repositories.BKRiskAssessPerferenceSettingRepository.MultiplePost(model);
                    context.SaveChanges();

                    return new ResultModel<BKRiskAssessPerferenceSetting>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKRiskAssessPerferenceSetting>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<BKRiskAssessPerferenceSetting> MultipleUnPost(BKRiskAssessPerferenceSetting model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					if (model.Operation == "unpost")
					{
						bool CheckUpPostStatus = false;
						CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("BKRiskAssessPerferenceSetting", new[] { "Id" }, new[] { model.Id.ToString() });
						if (CheckUpPostStatus)
						{
							return new ResultModel<BKRiskAssessPerferenceSetting>()
							{
								Status = Status.Fail,
								Message = MessageModel.UpPostAlready,

							};
						}
					}

					BKRiskAssessPerferenceSetting master = context.Repositories.BKRiskAssessPerferenceSettingRepository.MultipleUnPost(model);
					context.SaveChanges();

					return new ResultModel<BKRiskAssessPerferenceSetting>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<BKRiskAssessPerferenceSetting>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}

        public ResultModel<BKRiskAssessRegulationPreferenceCBS> RiskAssessPreferenceTempInsert(BKRiskAssessRegulationPreferenceCBS model)
        {

            string CodeGroup = "RAPS";
            string CodeName = "BKAuditOfficePreferenceCBS";

            using (var context = _unitOfWork.Create())
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

                    BKRiskAssessRegulationPreferenceCBS master = context.Repositories.BKRiskAssessPerferenceSettingRepository.RiskAssessPreferenceTempInsert(model);

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

        public ResultModel<BKRiskAssessPerferenceSetting> Update(BKRiskAssessPerferenceSetting model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					BKRiskAssessPerferenceSetting master = context.Repositories.BKRiskAssessPerferenceSettingRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKRiskAssessPerferenceSetting>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKRiskAssessPerferenceSetting>()
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
