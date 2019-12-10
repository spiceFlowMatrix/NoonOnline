using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IUserRole : IEntityBase
    {
        long UserId { get; set; }
        long RoleId { get; set; }
    }
}
