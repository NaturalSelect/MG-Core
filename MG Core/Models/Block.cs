using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MG_Core.Models
{
    public class Block
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessage ="板块名称不能为空")]
        [Display(Name = "板块名称:")]
        public string Name { get; set; }

        [Display(Name ="版主")]
        [Required]
        public virtual ApplicationUser PowerUser { get; set; }

        public string ImgPath { get; set; }
		[Display(Name="简介")]
		public string Introduction {get; set;}

        public virtual ICollection<Post> Post {get; set; }

    }
}
