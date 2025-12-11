using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shampan.Models
{
    public class AuditMail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Status { get; set; }
        public string WebRoot { get; set; }
        public string EmailAddress { get; set; }
       
    }
}
