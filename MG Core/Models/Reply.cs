using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace MG_Core.Models
{
    public class Reply
    {
        [Required]
        [Display(Name ="回复者")]
        public virtual ApplicationUser UserName { get; set; }

        [Required(ErrorMessage ="要回复的字符至少包含15字")]
        [MinLength(15,ErrorMessage ="回复至少包含15字")]
        [MaxLength(3000,ErrorMessage ="回复最大包含3000字节")]
        [Display(Name ="回复")]
        public string Body { get; set; }

        [Required]
        [Display(Name ="回复时间")]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

        [Key]
        public string Id { get; set; }

        public virtual Post Post { get; set; }
        
    }
}
