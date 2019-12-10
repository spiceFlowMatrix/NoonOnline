using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IGrade : IEntityBase
    {
        string Name { set; get; }
        string Description { set; get; }
    }
}
