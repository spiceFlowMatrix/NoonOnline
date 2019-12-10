using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class CourseDefination : EntityBase
    {
        public long GradeId { get; set; }
        public long CourseId { get; set; }
        public string Subject { get; set; }
        public string BasePrice { get; set; }
    }
}
