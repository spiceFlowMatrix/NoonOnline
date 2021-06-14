using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class StudParent : EntityBase
    {
        public long StudentId { get; set; }
        public long ParentId { get; set; }
    }
}
