using Newtonsoft.Json;
using Shampan.Core.Interfaces.Services.Advance;
using Shampan.Core.Interfaces.Services.HighLevelReports;
using Shampan.Core.Interfaces.Services.Team;
using Shampan.Core.Interfaces.Services.Tour;
using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace Shampan.Services.HighLevelReport
{
    public class HighLevelReportService : IHighLevelReportService
    {
        private IUnitOfWork _unitOfWork;

        public HighLevelReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public int Archive(string tableName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public ResultModel<Models.HighLevelReport> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<Models.HighLevelReport>> GetAll(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            throw new NotImplementedException();
        }

        public int GetCount(string tableName, string fieldName, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                int count =
                    context.Repositories.HighLevelReportRepository.GetCount(tableName,
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

        public ResultModel<List<Models.HighLevelReport>> GetIndexData(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                List< Models.HighLevelReport> records = context.Repositories.HighLevelReportRepository.GetIndexData(index, conditionalFields, conditionalValue);
                context.SaveChanges();

                return new ResultModel<List<Models.HighLevelReport>>()
                {
                    Status = Status.Success,
                    Message = MessageModel.DataLoaded,
                    Data = records
                };

            }
            catch (Exception e)
            {
                context.RollBack();

                return new ResultModel<List<Models.HighLevelReport>>()
                {
                    Status = Status.Fail,
                    Message = MessageModel.DataLoadedFailed,
                    Exception = e
                };
            }
        }

        public ResultModel<int> GetIndexDataCount(IndexModel index, string[] conditionalFields, string[] conditionalValue, PeramModel vm = null)
        {
            using IUnitOfWorkAdapter context = _unitOfWork.Create();
            try
            {
                int records = context.Repositories.HighLevelReportRepository.GetIndexDataCount(index, conditionalFields, conditionalValue);
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

        public ResultModel<Models.HighLevelReport> Insert(Models.HighLevelReport model)
        {
            throw new NotImplementedException();
        }

        public ResultModel<Models.HighLevelReport> Update(Models.HighLevelReport model)
        {
            throw new NotImplementedException();
        }
    }
}
