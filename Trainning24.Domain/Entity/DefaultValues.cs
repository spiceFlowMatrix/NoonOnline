using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class DefaultValues : EntityBase
    {
        public int timeout { get; set; }
        public int reminder { get; set; }
        public int intervals { get; set; }
        public bool istimeouton { get; set; }
    }
}
