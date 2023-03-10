using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MG_Core.Models
{
    public class BlockViewModel
    {
        [Required(ErrorMessage ="名称不能为空")]
        [MaxLength(10,ErrorMessage ="名称最大为10字符")]
        [MinLength(3,ErrorMessage ="请至少包含3个字符")]
        public string Name{get; set;}

        [Required(ErrorMessage ="必须有权限用户")]
        [Display(Name ="权限用户Id")]
        public string PowerUserId{get; set;}

        [FileExtensions(Extensions = "jpg")]
        [Display(Name ="头像")]
        public IFormFile Img { get; set; }

        [Display(Name = "简介")]
        [Required(ErrorMessage = "简介不能为空")]
        [StringLength(300,ErrorMessage ="简介最多包涵300字至少包含20字",MinimumLength =20)]
        public string Introduction { get; set; }
    }
}
