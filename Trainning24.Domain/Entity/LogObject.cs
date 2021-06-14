using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class LogObject : EntityBase
    {
        public long TypeId { get; set; }
        public long EntityKey { get; set; }
        public long ActionUserId { get; set; }
        public string TimeStamp { get; set; }
    }
}
