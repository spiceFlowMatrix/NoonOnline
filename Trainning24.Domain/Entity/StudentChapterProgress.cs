using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class StudentChapterProgress : EntityBase, IStudentChapterProgress
    {
        public long ChapterId { get; set; }
        public long ChapterStatus { get; set; }
        public long StudentId { get; set; }
    }
}
