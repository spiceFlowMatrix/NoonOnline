using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class AddtionalServices : EntityBase
    {
        public string Name { get; set; }
        public string Price { get; set; }
    }
}
