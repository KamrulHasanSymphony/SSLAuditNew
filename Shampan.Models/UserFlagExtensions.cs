using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shampan.Models
{
    public static class UserFlagExtensions
    {
        public static bool Contains(this UserFlags haystack, UserFlags needle)
        {
            return (haystack & needle) == needle;
        }
    }
}
