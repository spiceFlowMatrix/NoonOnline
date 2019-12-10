using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Users
{
    public class ForgotPasswordModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string AuthId { get; set; }
        public bool isforgot { get; set; }
        public string forgotkey { get; set; }
    }
}
