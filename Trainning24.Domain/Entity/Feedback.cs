using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Domain.Entity
{
    public class Feedback : EntityBase
    {
        public long Contactid { get; set; }
        public long CategoryId { get; set; }
        public long GradeId { get; set; }
        public long CourseId { get; set; }
        public long LessonId { get; set; }
        public long ChapterId { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public long QaUser { get; set; }
        public long Coordinator { get; set; }
    }
}
