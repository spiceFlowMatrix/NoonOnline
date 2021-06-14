using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class CourseGrade : EntityBase, ICourseGrade
    {
        public long CourseId { get; set; }
        public long Gradeid { get; set; }
    }
}
