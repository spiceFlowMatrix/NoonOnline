using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.CourseDefination
{
    public class ResponseCourseDefination
    {
        public long Id { get; set; }
        public long GradeId { get; set; }
        public string GradeName { get; set; }
        public long CourseId { get; set; }
        public string CourseName { get; set; }
        public string Subject { get; set; }
        public string BasePrice { get; set; }
    }


    public class ResponseCourseDefination1
    {
        public long Id { get; set; }
        public long GradeId { get; set; }
        public long CourseId { get; set; }
        public string Subject { get; set; }
        public string BasePrice { get; set; }
    }
}
