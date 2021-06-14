using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.CourseGrade
{
    public class ResponseCourseGradeModel
    {
        public long id { get; set; }
        public long courseid { get; set; }
        public long gradeid { get; set; }
    }


    public class GradeDetail
    {                
        public long gradeid { get; set; }
        public string name { set; get; }
    }
}
