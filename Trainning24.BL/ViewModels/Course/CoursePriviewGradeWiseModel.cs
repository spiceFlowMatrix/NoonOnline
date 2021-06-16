using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Assignment;
using Trainning24.BL.ViewModels.Lesson;

namespace Trainning24.BL.ViewModels.Course
{
    public class CoursePriviewGradeWiseModel
    {
        public long id { get; set; }
        public string name { get; set; }

        public List<GradeWiseCoursePriviewModel> courses { get; set; }
    }


    public class GetAllDetailsModel
    {
        public List<CourseDetailModel> courses { get; set; }
        public List<LessonDetailModel> lessons { get; set; }
        public List<AssignmentDetails> assignment { get; set; }
    }
}
