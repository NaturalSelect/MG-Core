using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MG_Core.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20,ErrorMessage ="用户名最多包含20个字符")]
        [MinLength(3,ErrorMessage ="用户名至少需要3个字符")]
        [Display(Name ="用户名")]
        public string Name { get; set; }

        [Required]
        [Display(Name ="名称")]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}必须包含至少 {2} 个字符，长度最多为 {1}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "重复密码必须与密码一致")]
        public string ConfirmPassword { get; set; }
    }
}
