using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;
using Trainning24.Abstract.Infrastructure.IGeneric;

namespace Trainning24.Abstract.Infrastructure.IRepository
{
    public interface IUsersRepository : IGenericRepository<IUser>
    {
    }
}
