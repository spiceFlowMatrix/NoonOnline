using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class FileTypes : EntityBase, IFileTypes
    {
        public string Filetype { get; set; }
    }
}
