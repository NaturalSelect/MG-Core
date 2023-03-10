using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MG_Core.Models
{
    public class UserRoleViewModel
    {
        [Required(ErrorMessage ="用户名不能为空")]
        [Display(Name ="用户名")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Role Id不能为空")]
        [Display(Name ="Role Id")]
        public string RoleId { get; set; }
        public string UserId { get; set; }
    }
}
