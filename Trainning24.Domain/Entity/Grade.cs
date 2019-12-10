using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Grade : EntityBase, IGrade
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public long SchoolId { get; set; }
    }
}