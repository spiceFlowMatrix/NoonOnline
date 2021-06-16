using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class FileProgress : EntityBase
    {
        public long LessonId { get; set; }
        public long FileId { get; set; }
        public long UserId { get; set; }
        public long Progress { get; set; }
    }
}
