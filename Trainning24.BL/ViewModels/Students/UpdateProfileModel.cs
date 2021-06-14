using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Students
{
    public class UpdateProfileModel
    {
        public long userid { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string bio { get; set; }
        public string phonenumber { get; set; }
        //public string profile_pic_url { get; set; }
    }
}
