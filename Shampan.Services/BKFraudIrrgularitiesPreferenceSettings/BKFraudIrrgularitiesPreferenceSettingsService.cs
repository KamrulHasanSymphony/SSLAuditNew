using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKFraudIrrgularitiesPreferenceSetting;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKFraudIrrgularitiesPreferenceSetting
{
	public class BKFraudIrrgularitiesPreferenceSettingsService : IBKFraudIrrgularitiesPreferenceSettingService
    {
		private IUnitOfWork _unitOfWork;

		public BKFraudIrrgularitiesPreferenceSettingsService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKFraudIrrgularitiesPreferenceSettings> Delete(int id)
		{

            using (var context = _unitOfWork.Create())
            {
                try
                {


                    int delete = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.Delete("BKFraudIrregularitiesInternalControlPreferenceCBSTemp", null, null);
                    context.SaveChanges();

                    return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        //Exception = e
                    };



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<BKFraudIrregularitiesInternalControlPreferenceCBS> FraudIrrgularitiesPreferenceTempInsert(BKFraudIrregularitiesInternalControlPreferenceCBS model)
        {
            string CodeGroup = "RAPS";
            string CodeName = "BKAuditOfficePreferenceCBS";

            using (var context = _unitOfWork.Create())
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

                    BKFraudIrregularitiesInternalControlPreferenceCBS master = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.FraudIrrgularitiesPreferenceTempInsert(model);

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

        public ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>>()
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
                        context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>>()
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
                    var records = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.GetIndexDataTemp(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKFraudIrrgularitiesPreferenceSettings>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<BKFraudIrrgularitiesPreferenceSettings> Insert(BKFraudIrrgularitiesPreferenceSettings model)
		{
            string CodeGroup = "FIPS";
            string CodeName = "BKFraudIrrgularitiesPreferenceSettings";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.CodeGeneration(CodeGroup, CodeName);

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

						BKFraudIrrgularitiesPreferenceSettings master = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        public ResultModel<BKFraudIrrgularitiesPreferenceSettings> MultiplePost(BKFraudIrrgularitiesPreferenceSettings model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.CheckPostStatus("BKFraudIrrgularitiesPreferenceSettings", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    BKFraudIrrgularitiesPreferenceSettings master = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.MultiplePost(model);
                    context.SaveChanges();

                    return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<BKFraudIrrgularitiesPreferenceSettings> MultipleUnPost(BKFraudIrrgularitiesPreferenceSettings model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					if (model.Operation == "unpost")
					{
						bool CheckUpPostStatus = false;
						CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("BKFraudIrrgularitiesPreferenceSettings", new[] { "Id" }, new[] { model.Id.ToString() });
						if (CheckUpPostStatus)
						{
							return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
							{
								Status = Status.Fail,
								Message = MessageModel.UpPostAlready,

							};
						}
					}

					BKFraudIrrgularitiesPreferenceSettings master = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.MultipleUnPost(model);
					context.SaveChanges();

					return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<BKFraudIrrgularitiesPreferenceSettings> Update(BKFraudIrrgularitiesPreferenceSettings model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					BKFraudIrrgularitiesPreferenceSettings master = context.Repositories.BKFraudIrrgularitiesPreferenceSettingRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFraudIrrgularitiesPreferenceSettings>()
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
