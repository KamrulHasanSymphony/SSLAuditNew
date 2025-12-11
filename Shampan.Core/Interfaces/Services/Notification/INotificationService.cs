using Shampan.Models;
using Shampan.Models.AuditModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Core.Interfaces.Services.Notification
{
	public interface INotificationService : IBaseService<Notifications>
	{

        ResultModel<Notifications> UpdateNotification(string id, string userName, string notificationType);

        ResultModel<Notifications> InsertNotification(string URL, string Name, int auditId,
            int commonId, string UserName, string notification,List<AuditUser> users);

        ResultModel<Notifications> InsertNotification(string URL, string Name, int auditId,
            int commonId, string UserName, string notification, List<TeamMembers> users);

    }
}
