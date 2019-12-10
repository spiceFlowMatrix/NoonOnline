using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class CourseItemProgressSync : EntityBase, ICourseItemProgressSync
    {
        public long Userid { get; set; }
        public long Lessonid { get; set; }
        public long Lessonprogress { get; set; }
        public long Quizid { get; set; }
        public long IsStatus { get; set; }
    }
}
