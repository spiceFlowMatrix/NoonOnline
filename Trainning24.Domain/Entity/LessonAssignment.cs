using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class LessonAssignment : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public long LessonId { get; set; }
    }
}
