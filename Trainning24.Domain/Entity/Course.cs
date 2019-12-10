using System.ComponentModel;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Course : EntityBase, ICourse
    {        
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }        
        public decimal? PassMark { get; set; }
        public bool istrial { get; set; }
    }
}
