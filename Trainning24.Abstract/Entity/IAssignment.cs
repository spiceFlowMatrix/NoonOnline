using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IAssignment
    {
        string Name { get; set; }
        string Description { get; set; }
        string Code { get; set; }
        long ChapterId { get; set; }
    }
}
