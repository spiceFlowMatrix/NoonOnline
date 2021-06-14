using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface ICourseItemProgressSync
    {
        long Userid { get; set; }
        long Lessonid { get; set; }
        long Lessonprogress { get; set; }
        long Quizid { get; set; }
    }
}
