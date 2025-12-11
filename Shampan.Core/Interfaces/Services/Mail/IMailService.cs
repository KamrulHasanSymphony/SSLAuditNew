using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shampan.Models;

namespace Shampan.Core.Interfaces.Services.Mail
{
	public interface IMailService : IBaseService<AuditMail>
	{

       ResultModel<AuditMail> SendEmail(AuditMail model);

    }
}
