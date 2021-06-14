using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Files : EntityBase, IFiles
    {        
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public long FileSize { get; set; }
        public long FileTypeId { get; set; }
        public int TotalPages { get; set; }
        public string Duration { get; set; }
    }
}
