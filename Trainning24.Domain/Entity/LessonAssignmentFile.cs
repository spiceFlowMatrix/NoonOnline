using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class LessonAssignmentFile : EntityBase
    {
        public long AssignmentId { get; set; }
        public long FileId { get; set; }
    }
}
