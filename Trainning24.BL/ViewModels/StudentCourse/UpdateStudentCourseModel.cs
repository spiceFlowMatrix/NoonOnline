using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.BundleCourse
{
    public class UpdateStudentCourseModel
    {
        public long id { get; set; }
        public long userid { get; set; }
        public long courseid { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
    }
}
