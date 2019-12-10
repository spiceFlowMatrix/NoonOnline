using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Grade;

namespace Trainning24.BL.ViewModels.Library
{
    public class BooksViewModelDTO
    {
        public long id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string subject { get; set; }
        public string bookpublisher { get; set; }
        public string gradeid { get; set; }
        public List<ResponseGradeModel> grades { get; set; }
        public string description { get; set; }
        public long fileid { get; set; }
        public ResponseFilesModel file { get; set; }
        public bool IsPublished { get; set; }
        public long coverimage { get; set; }
        public string coverurl { get; set; }
    }

    public class BooksAppDTO
    {
        public long id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string subject { get; set; }
        public string bookcoverimage { get; set; }
        public string bookpublisher { get; set; }
        //public string gradeid { get; set; }
        //public List<ResponseGradeModel> grades { get; set; }
        public string description { get; set; }
        //public long fileid { get; set; }
        public ResponseFilesModel file { get; set; }
        public bool IsPublished { get; set; }
    }

    public class BooksGradeWiseDTO
    {
        public long id { get; set; }
        public string gradename { get; set; }
        public List<BooksAppDTO> books { get; set; }
    }

}
