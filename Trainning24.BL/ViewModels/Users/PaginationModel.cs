using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Users
{
    public class PaginationModel
    {
        public int pagenumber { get; set; }
        public int perpagerecord { get; set; }
        public int roleid { get; set; }
        public string search { get; set; }
    }


    public class BundleCoursePaginationModel
    {
        public int pagenumber { get; set; }
        public int perpagerecord { get; set; }
        public long bundleid { get; set; }
    }

    public class StudentCoursePaginationModel
    {
        public int pagenumber { get; set; }
        public int perpagerecord { get; set; }
        public long courseid { get; set; }
    }

    public class CourseStudentPaginationModel
    {
        public int pagenumber { get; set; }
        public int perpagerecord { get; set; }
        public long studentid { get; set; }
    }

    public class PaginationModel2
    {
        public int pagenumber { get; set; }
        public int perpagerecord { get; set; }
        public string[] roleid { get; set; }
        public string search { get; set; }
    }

    public class FeedbackPaginationModel
    {
        public int pagenumber { get; set; }
        public int perpagerecord { get; set; }
        public long userid { get; set; }
        public long categoryid { get; set; }
        public string begin { get; set; }
        public string end { get; set; }
        public string startdate { get; set; }
        public string completeddate { get; set; }
        public string archiveddate { get; set; }
    }

}
