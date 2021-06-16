using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class BundleCourse : EntityBase, IBundleCourse
    {        
        public long BundleId { get; set; }
        public long CourseId { get; set; }
    }
}
