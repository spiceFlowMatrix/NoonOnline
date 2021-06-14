using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Chapter : EntityBase, IChapter
    {        
        public string Name { get; set; }
        public string Code { get; set; }
        public long CourseId { get; set; }
        public long? QuizId { get; set; }
        public int ItemOrder { get; set; }
    }
}
