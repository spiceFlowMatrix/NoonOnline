using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface ICourseGrade : IEntityBase
    {
        long CourseId { get; set; }
        long Gradeid { get; set; }
    }
}
