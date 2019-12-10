using Trainning24.Abstract.Entity;
namespace Trainning24.Domain.Entity
{
    public class GroupCourse //: EntityBase, IGroupCourse
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long CourseId { get; set; }
    }
}
