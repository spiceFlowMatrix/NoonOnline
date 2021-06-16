using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class LogObjectTypes : EntityBase
    {
        public string EntityType { get; set; }
        public string Action { get; set; }
    }
}
