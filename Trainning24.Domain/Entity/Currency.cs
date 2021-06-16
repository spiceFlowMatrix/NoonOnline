using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class Currency : EntityBase
    {
        public string currency { get; set; }
        public string abbreviation { get; set; }
        public string symbol { get; set; }
    }
}
