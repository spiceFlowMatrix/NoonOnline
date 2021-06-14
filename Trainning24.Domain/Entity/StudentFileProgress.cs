using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class StudentFileProgress : EntityBase, IStudentFileProgress
    {
        public long FileId { get; set; }
        public long StudentId { get; set; }
        public decimal FileProgress { get; set; }
    }
}
