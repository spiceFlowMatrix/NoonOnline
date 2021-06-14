using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class TaskFileFeedBack : EntityBase 
    {
        public long TaskId { get; set; }
        public long FileId { get; set; }
    }
}
