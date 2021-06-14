using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IChapter : IEntityBase
    {        
         string Name { get; set; }
         string Code { get; set; }
         long CourseId { get; set; }
    }
}
