using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Files
{
    public class ResponseFilesModel
    {
        public long Id { get; set; }
        public string name { get; set; }
        public string filename { get; set; }
        public string description { get; set; }
        public string filetypename { get; set; }
        public string url { get; set; }
        public long filesize { get; set; }
        public long filetypeid { get; set; }
        public int totalpages { get; set; }
        public string duration { get; set; }
    }

    public class ResponseFilesModel1
    {
        public long Id { get; set; }
        public string name { get; set; }
        public long filetypeid { get; set; }
        public string filetypename { get; set; }
    }
}
