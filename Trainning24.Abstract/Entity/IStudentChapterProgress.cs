using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IStudentChapterProgress : IEntityBase
    {
        long ChapterId { get; set; }
        long ChapterStatus { get; set; }
        long StudentId { get; set; }
    }
}
