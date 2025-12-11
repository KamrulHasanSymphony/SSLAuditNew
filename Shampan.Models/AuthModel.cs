using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class AuthModel
    {
        public string token { get; set; }

        public string Token_type { get; set; }

        public string Expires_in { get; set; }

        public AspNetUsersVM userInfo { get; set; }
    }
}
