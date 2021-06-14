using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Lesson : EntityBase, ILesson
    {        
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long? ChapterId { get; set; }
        public int ItemOrder { get; set; }
    }
}
