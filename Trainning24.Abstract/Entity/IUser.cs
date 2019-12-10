using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IUser : IEntityBase
    {
        string Username { get; set; }
        string FullName { get; set; }
        string Password { get; set; }
        //long RoleId { get; set; }
        string Email { get; set; }
    }
}
