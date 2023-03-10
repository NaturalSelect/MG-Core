using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MG_Core.Models
{
    public class Post
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name ="发送者")]
        public virtual ApplicationUser User { get; set; }

        [Display(Name ="标题")]
        [Required(ErrorMessage ="标题不能为空")]
        [MaxLength(15,ErrorMessage ="标题最多15个字符")]
        [MinLength(3,ErrorMessage ="标题至少包含3个字符")]
        public string Title { get; set; }

        [Display(Name ="内容")]
        [Required(ErrorMessage ="内容不能为空")]
        [MinLength(15,ErrorMessage ="内容至少包含15个字符")]
        public string Body { get; set; }

        [Required]
        [Display(Name ="发帖时间")]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }

        public virtual Block Block { get; set; }


        public virtual ICollection<Reply> Reply { get; set; }
        
    }
}
