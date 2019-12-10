using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;



namespace Trainning24.Domain.Entity
{
    public class UpdateStudentCourseProgress //: EntityBase, IUpdateBundleCourseProgress
    {
        public long ID { get; set; }

        public StudentCourseProgress objStudentCourseProgress { get; set; }
    }
}
