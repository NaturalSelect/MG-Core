using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MG_Core.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="您必须填写用户名")]
        [StringLength(maximumLength:20,MinimumLength =1,ErrorMessage ="用户名至多包含20个字符至少包含3个字符")]
        public string Name { get; set; }

        [Required(ErrorMessage ="您必须填写密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}
