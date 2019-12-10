using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Video //: EntityBase, IVideo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public long FileSize { get; set; }
    }

}
