using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class UserRole : EntityBase, IUserRole
    {
        public long UserId { get; set; }
        public long RoleId { set; get; }
    }
}
