using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Users
{
    public class BasicUserDTO
    {
        public long id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string profilepicurl { get; set; }
    }
}
