using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Package
{
    public class PackageCourseDTO
    {
        public long id { get; set; }
        public long package_id { get; set; }
        public long course_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }
}
