using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class TaskFeedBack : EntityBase 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public long? GradeId { get; set; }
        public long? CourseId { get; set; }
        public long? ChapterId { get; set; }
        public long? LessonId { get; set; }
        public string StartDate { get; set; }
        public string ComplatedDate { get; set; }
        public  string Time { get; set; }
        public int Status { get; set; }
        public string Device { get; set; }
        public string Version { get; set; }
        public string AppVersion { get; set; }
        public string OperatingSystem { get; set; }
        public string ArchivedDate { get; set; }
        public long Assign { get; set; }
    }
}
