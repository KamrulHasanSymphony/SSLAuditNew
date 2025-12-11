using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditInfoDetailss;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.BKAuditInfoMasters;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKAuditInfoMasters
{
	public class BKAuditInfoMasterService : IBKAuditInfoMasterService
    {
		private IUnitOfWork _unitOfWork;

		public BKAuditInfoMasterService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKAuditInfoMaster> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<BKAuditInfoMaster>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditInfoMasterRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditInfoMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditInfoMaster>>()
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
                        context.Repositories.BKAuditInfoMasterRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKAuditInfoMaster>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditInfoMasterRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditInfoMaster>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditInfoMaster>>()
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
                    var records = context.Repositories.BKAuditInfoMasterRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<BKAuditInfoMaster> Insert(BKAuditInfoMaster model)
		{
            string CodeGroup = "RAPS";
            string CodeName = "BKAuditInfoMaster";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKAuditInfoMaster>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.BKAuditInfoMasterRepository.CodeGeneration(CodeGroup, CodeName);

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

						BKAuditInfoMaster master = context.Repositories.BKAuditInfoMasterRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKAuditInfoMaster>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKAuditInfoMaster>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKAuditInfoMaster>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditInfoMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        public ResultModel<BKAuditInfoMaster> MultiplePost(BKAuditInfoMaster model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.BKAuditInfoMasterRepository.CheckPostStatus("BKAuditInfoMaster", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<BKAuditInfoMaster>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    BKAuditInfoMaster master = context.Repositories.BKAuditInfoMasterRepository.MultiplePost(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditInfoMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditInfoMaster>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<BKAuditInfoMaster> MultipleUnPost(BKAuditInfoMaster model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					if (model.Operation == "unpost")
					{
						bool CheckUpPostStatus = false;
						CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("BKAuditInfoMaster", new[] { "Id" }, new[] { model.Id.ToString() });
						if (CheckUpPostStatus)
						{
							return new ResultModel<BKAuditInfoMaster>()
							{
								Status = Status.Fail,
								Message = MessageModel.UpPostAlready,

							};
						}
					}

					BKAuditInfoMaster master = context.Repositories.BKAuditInfoMasterRepository.MultipleUnPost(model);
					context.SaveChanges();

					return new ResultModel<BKAuditInfoMaster>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<BKAuditInfoMaster>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<BKAuditInfoMaster> Update(BKAuditInfoMaster model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					BKAuditInfoMaster master = context.Repositories.BKAuditInfoMasterRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditInfoMaster>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditInfoMaster>()
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
