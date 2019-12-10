using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Users
{
    public class ChangePasswordModel
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "EamilIsRequired")]
        //public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "New Password Required")]
        public string new_password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Old Password Required")]
        public string old_password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password Required")]
        public string confirm_password { get; set; }
    }
}
