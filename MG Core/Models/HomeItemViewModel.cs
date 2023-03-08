using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MG_Core.Models
{
    public class HomeItemViewModel
    {
        [Display(Name ="简介")]
        [Required(ErrorMessage ="简介不能为空")]
        [StringLength(50,ErrorMessage ="简介最多包含50字至少包含5字",MinimumLength =5)]
        public string Introduction { get; set; }

        [Display(Name ="图片URL")]
        [Required(ErrorMessage ="图片URL必填")]
        public string ImgURL { get; set; }

        [Display(Name ="跳转URL")]
        [Required(ErrorMessage ="跳转URL不能为空")]
        public string JumpURL { get; set; }

        [Key]
        [Display(Name ="Id")]
        public string Id { get; set; }


    }
}
