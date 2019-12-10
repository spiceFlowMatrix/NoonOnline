using System;
using System.ComponentModel.DataAnnotations;

namespace Trainning24.Abstract.Entity
{
    public interface IEntityBase
    {
        [Key]
        long Id { get; set; }

        string CreationTime { get; set; }
        int? CreatorUserId { get; set; }
        string LastModificationTime { get; set; }
        int? LastModifierUserId { get; set; }
        bool? IsDeleted { get; set; }
        int? DeleterUserId { get; set; }
        string DeletionTime { get; set; }
    }
}
