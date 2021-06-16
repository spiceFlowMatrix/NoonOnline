using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class LessonFile : EntityBase, ILessonFile
    {
        public long LessionId { get; set; }
        public long FileId { get; set; }
    }
}
