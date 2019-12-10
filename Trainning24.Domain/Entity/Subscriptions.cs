using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class Subscriptions : EntityBase
    {
        public string UserId { get; set; }
        public long SubscriptionMetadataId { get; set; }        
    }
}
