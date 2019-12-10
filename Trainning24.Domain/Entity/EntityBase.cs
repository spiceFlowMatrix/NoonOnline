using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class EntityBase : IEntityBase
    {
        public long Id { get; set; }

        public string CreationTime { get; set; }
        public int? CreatorUserId { get; set; }
        public string LastModificationTime { get; set; }
        public int? LastModifierUserId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? DeleterUserId { get; set; }
        public string DeletionTime { get; set; }
    }
}
