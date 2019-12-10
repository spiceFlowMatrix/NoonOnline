using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class QuestionFile : EntityBase, IQuestionFile
    {
        public long QuestionId { get; set; }
        public long FileId { get; set; }
    }
}
