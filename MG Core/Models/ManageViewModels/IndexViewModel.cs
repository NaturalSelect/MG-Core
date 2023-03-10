using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MG_Core.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        [Display(Name ="名称")]
        public string Name { get; set; }

        public string ImgPath { get; set; }
    }
}
