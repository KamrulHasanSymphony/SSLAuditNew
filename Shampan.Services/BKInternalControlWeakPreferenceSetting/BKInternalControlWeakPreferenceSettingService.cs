using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Services.BKInternalControlWeakPreferenceSetting;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKInternalControlWeakPreferenceSetting
{
	public class BKInternalControlWeakPreferenceSettingService : IBKInternalControlWeakPreferenceSettingService
    {
		private IUnitOfWork _unitOfWork;

		public BKInternalControlWeakPreferenceSettingService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKInternalControlWeakPreferenceSettings> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<BKInternalControlWeakPreferenceSettings>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKInternalControlWeakPreferenceSettings>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKInternalControlWeakPreferenceSettings>>()
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
                        context.Repositories.BKInternalControlWeakPreferenceSettingRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKInternalControlWeakPreferenceSettings>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKInternalControlWeakPreferenceSettings>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKInternalControlWeakPreferenceSettings>>()
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
                    var records = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<BKInternalControlWeakPreferenceSettings> Insert(BKInternalControlWeakPreferenceSettings model)
		{
            string CodeGroup = "ICWPS";
            string CodeName = "BKInternalControlWeakPreferenceSettings";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.CodeGeneration(CodeGroup, CodeName);

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

						BKInternalControlWeakPreferenceSettings master = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        public ResultModel<BKInternalControlWeakPreferenceSettings> MultiplePost(BKInternalControlWeakPreferenceSettings model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.CheckPostStatus("BKInternalControlWeakPreferenceSettings", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    BKInternalControlWeakPreferenceSettings master = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.MultiplePost(model);
                    context.SaveChanges();

                    return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<BKInternalControlWeakPreferenceSettings> MultipleUnPost(BKInternalControlWeakPreferenceSettings model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					if (model.Operation == "unpost")
					{
						bool CheckUpPostStatus = false;
						CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("BKInternalControlWeakPreferenceSettings", new[] { "Id" }, new[] { model.Id.ToString() });
						if (CheckUpPostStatus)
						{
							return new ResultModel<BKInternalControlWeakPreferenceSettings>()
							{
								Status = Status.Fail,
								Message = MessageModel.UpPostAlready,

							};
						}
					}

					BKInternalControlWeakPreferenceSettings master = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.MultipleUnPost(model);
					context.SaveChanges();

					return new ResultModel<BKInternalControlWeakPreferenceSettings>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<BKInternalControlWeakPreferenceSettings>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<BKInternalControlWeakPreferenceSettings> Update(BKInternalControlWeakPreferenceSettings model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					BKInternalControlWeakPreferenceSettings master = context.Repositories.BKInternalControlWeakPreferenceSettingRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKInternalControlWeakPreferenceSettings>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKInternalControlWeakPreferenceSettings>()
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
