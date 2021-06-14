using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.StudentCourseProgress
{
    public class AddStudentCourseProgressModel
    {
        public long courseid { get; set; }
        public long coursestatus { get; set; }
        public decimal courseprogress { get; set; }
        public long studentid { get; set; }
    }
}
