using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IFiles : IEntityBase
    {
        string Name { get; set; }
        string FileName { get; set; }
        string Description { get; set; }
        string Url { get; set; }
        long FileSize { get; set; }
        long FileTypeId { get; set; }
        int TotalPages { get; set; }
        string Duration { get; set; }
    }
}
