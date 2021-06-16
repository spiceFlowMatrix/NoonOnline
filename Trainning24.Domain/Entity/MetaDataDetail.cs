using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class MetaDataDetail : EntityBase
    {
        public long MetadataId { get; set; }
        public long DetailId { get; set; }
        public int DetailTypeId { get; set; }
    }
}
