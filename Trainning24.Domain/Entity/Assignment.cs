using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Assignment : EntityBase, IAssignment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public long ChapterId { get; set; }
        public int ItemOrder { get; set; }
    }
}
