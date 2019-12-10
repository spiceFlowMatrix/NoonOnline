using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class PackageCourse : EntityBase
    {
        public long PackageId { get; set; }
        public long CourseId { get; set; }
    }
}
