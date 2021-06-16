using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.CourseGrade;
using Trainning24.BL.ViewModels.Grade;

namespace Trainning24.BL.ViewModels.Course
{
    public class ResponseCourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public long gradeid { get; set; }
        public string gradename { get; set; }
        public bool istrial { get; set; }
        //public List<ResponseGradeModel> gradeDetails { get; set; }
    }


    public class AllResponseCourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class CourseDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Code { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<ResponseGradeModel> GradeDetails { get; set; }

    }
}
