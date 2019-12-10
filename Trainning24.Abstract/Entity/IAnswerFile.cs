using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IAnswerFile
    {
        long AnswerId { get; set; }
        long FileId { get; set; }
    }
}
