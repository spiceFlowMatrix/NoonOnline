using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class PdfFile //: EntityBase, IPdfFile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }

}
