using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IAssignmentStudent : IEntityBase
    {
        long AssignmentId { get; set; }
        long StudentId { get; set; }
        //long TeacherId { get; set; }
    }
}
