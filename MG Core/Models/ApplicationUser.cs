using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MG_Core.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Display(Name ="头像:")]
        public string ImgPath { get; set; }

        [Display(Name ="名称:")]
        public string DisplayName { get; set; }

        public virtual ICollection<Post> Post { get; set; }

        public virtual ICollection<Reply> Reply { get; set; }

        public virtual Block Block { get; set; }

    }
}
