using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.StudentProgress
{
    public class UpdateStudentProgressModel
    {
        public long id { get; set; }
        public long courseid { get; set; }
        public long chapterid { get; set; }
        public long lessonid { get; set; }
        public long coursestatus { get; set; }
        public long chapterstatus { get; set; }
        public long lessonstatus { get; set; }
        public decimal courseprogress { get; set; }
        public long studentid { get; set; }
    }
}
