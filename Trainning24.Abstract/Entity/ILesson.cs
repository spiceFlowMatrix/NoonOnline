using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface ILesson : IEntityBase
    {
        string Name { get; set; }
        string Code { get; set; }
        string Description { get; set; }
        //long? FileId { get; set; }
        //long? QuizId { get; set; }
        long? ChapterId { get; set; }
    }
}
