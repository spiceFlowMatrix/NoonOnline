using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class ChapterProgress : EntityBase
    {
        public long CourseId { get; set; }
        public long ChapterId { get; set; }
        public long UserId { get; set; }
        public long Progress { get; set; }
    }
}
