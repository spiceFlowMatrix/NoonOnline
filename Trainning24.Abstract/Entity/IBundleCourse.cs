using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IBundleCourse : IEntityBase
    {
        long BundleId { get; set; }
        long CourseId { get; set; }
    }
}
