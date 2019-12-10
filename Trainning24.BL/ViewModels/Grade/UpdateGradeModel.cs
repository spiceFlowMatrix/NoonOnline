using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Grade
{
    public class UpdateGradeModel
    {
        public long id { get; set; }
        public string name { set; get; }
        public string description { set; get; }
        public long schoolid { get; set; }
    }
}
