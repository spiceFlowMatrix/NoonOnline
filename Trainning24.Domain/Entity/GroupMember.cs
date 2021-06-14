using Trainning24.Abstract.Entity;
namespace Trainning24.Domain.Entity
{
    public class GroupMember //: EntityBase, IGroupMember
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long UserId { get; set; }
    }
}
