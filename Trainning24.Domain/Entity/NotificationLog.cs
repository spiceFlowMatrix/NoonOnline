using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class NotificationLog : EntityBase
    {
        public long NotifiedUserId { get; set; }
        public long LogObjectId { get; set; }
        public bool IsRead { get; set; }
    }
}
