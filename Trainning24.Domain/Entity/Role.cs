using Trainning24.Abstract.Entity;
namespace Trainning24.Domain.Entity
{
    public class Role : EntityBase, IRole
    {        
        public string Name { get; set; }
        public string RoleKey { get; set; }
    }
}
