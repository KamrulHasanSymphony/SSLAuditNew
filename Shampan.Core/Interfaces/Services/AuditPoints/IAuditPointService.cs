using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.AuditPoints
{
    public interface IAuditPointService : IBaseService<AuditPoint>
    {
        ResultModel<string> GetSettingsValue(string[] conditionalFields, string[] conditionalValue, PeramModel vm = null);

    }
}
