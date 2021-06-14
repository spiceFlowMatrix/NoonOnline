using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class CourseComplete //: EntityBase, ICourseComplete
    {
        Course Course { get; set; }
        Chapter[] Chapters { get; set; }
    }
}
