using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Library
{
    public class BooksDTO
    {
        public long id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string subject { get; set; }
        public string bookpublisher { get; set; }
        public List<int> grades { get; set; }
        public string description { get; set; }
        public long fileid { get; set; }
        public long coverimage { get; set; }
    }
}
