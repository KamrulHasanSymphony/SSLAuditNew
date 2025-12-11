using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        public string? Operation { get; set; }
        [Display(Name = "Category Name")]
        public string? CategoryName { set; get; }
        public string? created_at { set; get; }
        public string? updated_at { set; get; }
        public bool Status { set; get; }
        public List<string> IDs { get; set; }
        public string Edit { get; set; }

        public Audit Audit;

        public Category()
        {
            Audit = new Audit();

        }
    }
}
