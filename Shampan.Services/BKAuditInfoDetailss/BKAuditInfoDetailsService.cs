using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.BKAuditInfoDetailss;
using Shampan.Core.Interfaces.Services.BKAuditOfficeTypes;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Models;
using UnitOfWork.Interfaces;

namespace Shampan.Services.BKAuditInfoDetailss
{
	public class BKAuditInfoDetailsService : IBKAuditInfoDetailsService
    {
		private IUnitOfWork _unitOfWork;

		public BKAuditInfoDetailsService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
			throw new NotImplementedException();
		}

		public ResultModel<BKAuditInfoDetails> Delete(int id)
		{
			throw new NotImplementedException();
		}

		public ResultModel<List<BKAuditInfoDetails>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditInfoDetailsRepository.GetAll(conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditInfoDetails>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditInfoDetails>>()
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
                        context.Repositories.BKAuditInfoDetailsRepository.GetCount(tableName,
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
		

        

        public ResultModel<List<BKAuditInfoDetails>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    var records = context.Repositories.BKAuditInfoDetailsRepository.GetIndexData(index, conditionalFields, conditionalValue);
                    context.SaveChanges();

                    return new ResultModel<List<BKAuditInfoDetails>>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.DataLoaded,
                        Data = records
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<List<BKAuditInfoDetails>>()
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
                    var records = context.Repositories.BKAuditInfoDetailsRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

		public ResultModel<BKAuditInfoDetails> Insert(BKAuditInfoDetails model)
		{
            string CodeGroup = "RAPS";
            string CodeName = "BKAuditInfoDetails";

            using (var context = _unitOfWork.Create())
            {
                try
                {
                    if (model == null)
                    {
                        return new ResultModel<BKAuditInfoDetails>()
                        {
                            Status = Status.Warning,
                            Message = MessageModel.NotFoundForSave,
                        };
                    }


                    string Code = context.Repositories.BKAuditInfoDetailsRepository.CodeGeneration(CodeGroup, CodeName);

                    if (Code != "" || Code != null)
                    {
                        model.Code = Code;

						BKAuditInfoDetails master = context.Repositories.BKAuditInfoDetailsRepository.Insert(model);

                        if (master.Id <= 0)
                        {
                            return new ResultModel<BKAuditInfoDetails>()
                            {
                                Status = Status.Fail,
                                Message = MessageModel.MasterInsertFailed,
                                Data = master
                            };
                        }

                        context.SaveChanges();


                        return new ResultModel<BKAuditInfoDetails>()
                        {
                            Status = Status.Success,
                            Message = MessageModel.InsertSuccess,
                            Data = master
                        };

                    }
                    else
                    {
                        return new ResultModel<BKAuditInfoDetails>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.DataLoadedFailed,

                        };
                    }



                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditInfoDetails>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.InsertFail,
                        Exception = e
                    };
                }
            }
          
        }

        public ResultModel<BKAuditInfoDetails> MultiplePost(BKAuditInfoDetails model)
        {
            using (var context = _unitOfWork.Create())
            {

                try
                {
                    bool CheckPostStatus = false;
                    CheckPostStatus = context.Repositories.BKAuditInfoDetailsRepository.CheckPostStatus("BKAuditInfoDetails", new[] { "Id" }, new[] { model.Id.ToString() });
                    if (CheckPostStatus)
                    {
                        return new ResultModel<BKAuditInfoDetails>()
                        {
                            Status = Status.Fail,
                            Message = MessageModel.PostAlready,

                        };
                    }

                    BKAuditInfoDetails master = context.Repositories.BKAuditInfoDetailsRepository.MultiplePost(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditInfoDetails>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.PostSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditInfoDetails>()
                    {
                        Status = Status.Fail,
                        Message = MessageModel.UpdateFail,
                        Exception = e
                    };
                }
            }
        }

		public ResultModel<BKAuditInfoDetails> MultipleUnPost(BKAuditInfoDetails model)
		{
			using (var context = _unitOfWork.Create())
			{

				try
				{

					if (model.Operation == "unpost")
					{
						bool CheckUpPostStatus = false;
						CheckUpPostStatus = context.Repositories.AuditMasterRepository.CheckUnPostStatus("BKAuditInfoDetails", new[] { "Id" }, new[] { model.Id.ToString() });
						if (CheckUpPostStatus)
						{
							return new ResultModel<BKAuditInfoDetails>()
							{
								Status = Status.Fail,
								Message = MessageModel.UpPostAlready,

							};
						}
					}

					BKAuditInfoDetails master = context.Repositories.BKAuditInfoDetailsRepository.MultipleUnPost(model);
					context.SaveChanges();

					return new ResultModel<BKAuditInfoDetails>()
					{
						Status = Status.Success,
						Message = MessageModel.UnPostSuccess,
						Data = model
					};

				}
				catch (Exception e)
				{
					context.RollBack();

					return new ResultModel<BKAuditInfoDetails>()
					{
						Status = Status.Fail,
						Message = MessageModel.UnPostFail,
						Exception = e
					};
				}
			}
		}

		public ResultModel<BKAuditInfoDetails> Update(BKAuditInfoDetails model)
		{
            using (var context = _unitOfWork.Create())
            {

                try
                {

					BKAuditInfoDetails master = context.Repositories.BKAuditInfoDetailsRepository.Update(model);
                    context.SaveChanges();

                    return new ResultModel<BKAuditInfoDetails>()
                    {
                        Status = Status.Success,
                        Message = MessageModel.UpdateSuccess,
                        Data = model
                    };

                }
                catch (Exception e)
                {
                    context.RollBack();

                    return new ResultModel<BKAuditInfoDetails>()
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
