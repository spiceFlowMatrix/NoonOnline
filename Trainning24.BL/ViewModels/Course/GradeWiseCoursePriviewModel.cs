using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Course
{
    public class GradeWiseCoursePriviewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
    }
}
