using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface ICourse : IEntityBase
    {
        string Name { get; set; }
        string Code { get; set; }
        string Description { get; set; }
        string Image { get; set; }
        decimal? PassMark { get; set; }
        //long TeacherId { get; set; }
    }
}
