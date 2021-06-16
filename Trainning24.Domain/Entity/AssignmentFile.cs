using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class AssignmentFile : EntityBase, IAssignmentFile
    {
        public long AssignmentId { get; set; }
        public long FileId { get; set; }
    }
}
