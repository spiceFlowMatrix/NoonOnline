using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IBundle : IEntityBase
    {
        string Name { get; set; }
        //long CourseId { get; set; }
    }
}
