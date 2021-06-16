using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.BundleCourse
{
    public class ResponseStudentCourseModel
    {
        public long id { get; set; }
        public long userid { get; set; }
        public long courseid { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
    }


    public class ResponseCourseByStudent
    {
        public long id { get; set; }
        public long studentid { get; set; }
        public long courseid { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public long schoolid { get; set; }
        public long gradeid { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool istrial { get; set; }
    }

    public class ResponseStudentByCourse
    {
        public long id { get; set; }
        public long studentid { get; set; }
        public long courseid { get; set; }
        public string name { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
