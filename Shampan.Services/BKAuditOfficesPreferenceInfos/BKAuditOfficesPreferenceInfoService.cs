using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditOfficesPreferenceInfos;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKAuditOfficesPreferenceInfos
{
	public class BKAuditOfficesPreferenceInfoService : IBKAuditOfficesPreferenceInfoService
    {
		private IUnitOfWork _unitOfWork;

		public BKAuditOfficesPreferenceInfoService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKAuditOfficesPreferenceInfo> Delete(int id)
		{
            using (var context = _unitOfWork.Create())
            {
                try
                {


                    int delete = context.Repositories.BKAuditOfficesPreferenceInfoRepository.Delete("BKAuditOfficePreferenceCBSTemp", null, null);
                    context.SaveChanges();

                    return new ResultModel<BKAuditOfficesPreferenceInfo>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        //Exception = e
                    };



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditOfficesPreferenceInfo>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<List<BKAuditOfficesPreferenceInfo>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditOfficesPreferenceInfoRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditOfficesPreferenceInfo>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditOfficesPreferenceInfo>>()
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
                        context.Repositories.BKAuditOfficesPreferenceInfoRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKAuditOfficesPreferenceInfo>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditOfficesPreferenceInfoRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditOfficesPreferenceInfo>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditOfficesPreferenceInfo>>()
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
                    var records = context.Repositories.BKAuditOfficesPreferenceInfoRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<BKAuditOfficesPreferenceInfo> Insert(BKAuditOfficesPreferenceInfo model)
		{
            string CodeGroup = "AOPI";
            string CodeName = "BKAuditOfficesPreferenceInfo";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKAuditOfficesPreferenceInfo>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.BKAuditOfficesPreferenceInfoRepository.CodeGeneration(CodeGroup, CodeName);

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

						BKAuditOfficesPreferenceInfo master = context.Repositories.BKAuditOfficesPreferenceInfoRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKAuditOfficesPreferenceInfo>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKAuditOfficesPreferenceInfo>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKAuditOfficesPreferenceInfo>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditOfficesPreferenceInfo>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        public ResultModel<BKAuditOfficesPreferenceInfo> MultiplePost(BKAuditOfficesPreferenceInfo model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.BKAuditOfficesPreferenceInfoRepository.CheckPostStatus("BKAuditOfficesPreferenceInfo", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<BKAuditOfficesPreferenceInfo>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    BKAuditOfficesPreferenceInfo master = context.Repositories.BKAuditOfficesPreferenceInfoRepository.MultiplePost(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditOfficesPreferenceInfo>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditOfficesPreferenceInfo>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<BKAuditOfficesPreferenceInfo> MultipleUnPost(BKAuditOfficesPreferenceInfo model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					if (model.Operation == "unpost")
					{
						bool CheckUpPostStatus = false;
						CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("BKAuditOfficesPreferenceInfo", new[] { "Id" }, new[] { model.Id.ToString() });
						if (CheckUpPostStatus)
						{
							return new ResultModel<BKAuditOfficesPreferenceInfo>()
							{
								Status = Status.Fail,
								Message = MessageModel.UpPostAlready,

							};
						}
					}

					BKAuditOfficesPreferenceInfo master = context.Repositories.BKAuditOfficesPreferenceInfoRepository.MultipleUnPost(model);
					context.SaveChanges();

					return new ResultModel<BKAuditOfficesPreferenceInfo>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<BKAuditOfficesPreferenceInfo>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<BKAuditOfficesPreferenceInfo> Update(BKAuditOfficesPreferenceInfo model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					BKAuditOfficesPreferenceInfo master = context.Repositories.BKAuditOfficesPreferenceInfoRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditOfficesPreferenceInfo>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditOfficesPreferenceInfo>()
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
