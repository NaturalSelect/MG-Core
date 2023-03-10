using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MG_Core.Models
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string E_mail {get;set;}
        public string Role { get; set; }
        public string Lock {
            get
            {
                return Userlock;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    Userlock = "NULL";
                }
                else
                {
                    Userlock = value;
                }
            }
        }
        private string Userlock;
        public string DisplayName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Id { get; set; }
    }
}
