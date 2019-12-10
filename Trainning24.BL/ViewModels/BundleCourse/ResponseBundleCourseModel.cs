using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.BundleCourse
{
    public class ResponseBundleCourseModel
    {
        public long id { get; set; }
        public long bundleid { get; set; }
        public long courseid { get; set; }
    }

    public class ResponseCourseByBundle
    {
        public long id { get; set; }
        public long bundleid { get; set; }
        public long courseid { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }

    public class ResponseCourseByGrade
    {
        public long id { get; set; }
        public long gradeid { get; set; }
        public long courseid { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }
}
