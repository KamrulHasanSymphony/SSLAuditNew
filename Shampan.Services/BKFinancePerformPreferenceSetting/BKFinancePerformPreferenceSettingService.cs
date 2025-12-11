using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKFinancePerformPreferenceSetting;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKFinancePerformPreferenceSetting
{
	public class BKFinancePerformPreferenceSettingService : IBKFinancePerformPreferenceSettingService
    {
		private IUnitOfWork _unitOfWork;

		public BKFinancePerformPreferenceSettingService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKFinancePerformPreferenceSettings> Delete(int id)
		{
            using (var context = _unitOfWork.Create())
            {
                try
                {


                    int delete = context.Repositories.BKFinancePerformPreferenceSettingRepository.Delete("BKFinancePreformPreferenceCBSTemp", null, null);
                    context.SaveChanges();

                    return new ResultModel<BKFinancePerformPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        //Exception = e
                    };



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFinancePerformPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<BKFinancePreformPreferenceCBS> FinancePerformPreferenceTempInsert(BKFinancePreformPreferenceCBS model)
        {
            string CodeGroup = "RAPS";
            string CodeName = "BKAuditOfficePreferenceCBS";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKFinancePreformPreferenceCBS>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }

                    BKFinancePreformPreferenceCBS master = context.Repositories.BKFinancePerformPreferenceSettingRepository.FinancePreformPreferenceCBSTempInsert(model);

                    if (master.Id <= 0)
                    {
                        return new ResultModel<BKFinancePreformPreferenceCBS>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.MasterInsertFailed,
                            Data = master
                        };
                    }

                    context.SaveChanges();

                    return new ResultModel<BKFinancePreformPreferenceCBS>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.InsertSuccess,
                        Data = master
                    };





                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFinancePreformPreferenceCBS>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

        public ResultModel<List<BKFinancePerformPreferenceSettings>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKFinancePerformPreferenceSettingRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKFinancePerformPreferenceSettings>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKFinancePerformPreferenceSettings>>()
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
                        context.Repositories.BKFinancePerformPreferenceSettingRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKFinancePerformPreferenceSettings>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKFinancePerformPreferenceSettingRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKFinancePerformPreferenceSettings>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKFinancePerformPreferenceSettings>>()
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
                    var records = context.Repositories.BKFinancePerformPreferenceSettingRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<List<BKFinancePerformPreferenceSettings>> GetIndexDataTemp(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKFinancePerformPreferenceSettingRepository.GetIndexDataTemp(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKFinancePerformPreferenceSettings>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKFinancePerformPreferenceSettings>>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.DataLoadedFailed,
                        Exception = e
                    };
                }

            }
        }

        public ResultModel<BKFinancePerformPreferenceSettings> Insert(BKFinancePerformPreferenceSettings model)
		{
            string CodeGroup = "FPPS";
            string CodeName = "BKFinancePerformPreferenceSettings";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKFinancePerformPreferenceSettings>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.BKFinancePerformPreferenceSettingRepository.CodeGeneration(CodeGroup, CodeName);

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

						BKFinancePerformPreferenceSettings master = context.Repositories.BKFinancePerformPreferenceSettingRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKFinancePerformPreferenceSettings>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKFinancePerformPreferenceSettings>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKFinancePerformPreferenceSettings>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFinancePerformPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        public ResultModel<BKFinancePerformPreferenceSettings> MultiplePost(BKFinancePerformPreferenceSettings model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.BKFinancePerformPreferenceSettingRepository.CheckPostStatus("BKFinancePerformPreferenceSettings", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<BKFinancePerformPreferenceSettings>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    BKFinancePerformPreferenceSettings master = context.Repositories.BKFinancePerformPreferenceSettingRepository.MultiplePost(model);
                    context.SaveChanges();

                    return new ResultModel<BKFinancePerformPreferenceSettings>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFinancePerformPreferenceSettings>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<BKFinancePerformPreferenceSettings> MultipleUnPost(BKFinancePerformPreferenceSettings model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					if (model.Operation == "unpost")
					{
						bool CheckUpPostStatus = false;
						CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("BKFinancePerformPreferenceSettings", new[] { "Id" }, new[] { model.Id.ToString() });
						if (CheckUpPostStatus)
						{
							return new ResultModel<BKFinancePerformPreferenceSettings>()
							{
								Status = Status.Fail,
								Message = MessageModel.UpPostAlready,

							};
						}
					}

					BKFinancePerformPreferenceSettings master = context.Repositories.BKFinancePerformPreferenceSettingRepository.MultipleUnPost(model);
					context.SaveChanges();

					return new ResultModel<BKFinancePerformPreferenceSettings>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<BKFinancePerformPreferenceSettings>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<BKFinancePerformPreferenceSettings> Update(BKFinancePerformPreferenceSettings model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					BKFinancePerformPreferenceSettings master = context.Repositories.BKFinancePerformPreferenceSettingRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKFinancePerformPreferenceSettings>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKFinancePerformPreferenceSettings>()
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
