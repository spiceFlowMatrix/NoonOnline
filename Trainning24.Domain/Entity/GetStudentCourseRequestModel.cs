using Trainning24.Abstract.Entity;
namespace Trainning24.Domain.Entity
{
    public class GetBundleCourseRequestModel// : EntityBase, IGetBundleCourseRequestModel
    {
        public long StudentId { get; set; }
        public long CourseId { get; set; }
    }
}
