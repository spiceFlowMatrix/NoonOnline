using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IStudentFileProgress : IEntityBase
    {
        long FileId { get; set; }        
        long StudentId { get; set; }
        decimal FileProgress { get; set; }
    }
}
