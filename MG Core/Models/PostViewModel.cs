using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MG_Core.Models
{
    public class PostViewModel
    {
        [Required(ErrorMessage ="标题不能为空")]
        [MaxLength(15,ErrorMessage ="标题不能大于10字")]
        [MinLength(3,ErrorMessage ="标题至少包含3字")]
        [Display(Name ="标题")]
        public string Tittle { get; set; }

        [Required(ErrorMessage ="主体不能为空")]
        [MinLength(15,ErrorMessage ="主体至少包含15字")]
        [Display(Name ="主体")]
        public string Body { get; set; }
    }
}
