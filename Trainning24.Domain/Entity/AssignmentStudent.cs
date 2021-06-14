using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class AssignmentStudent : EntityBase, IAssignmentStudent
    {
        public long AssignmentId { get; set; }
        public long StudentId { get; set; }
        public bool IsApproved { get; set; }
        //public long TeacherId { get; set; }
    }
}
