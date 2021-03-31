using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.Controllers.Account
{
    public class ChangePasswordInputModel
    {
        [Required]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
    }
}
