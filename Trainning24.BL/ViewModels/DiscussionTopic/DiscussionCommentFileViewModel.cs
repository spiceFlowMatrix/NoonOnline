using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.DiscussionTopic
{
    public class DiscussionCommentFileViewModel
    {
        public long Id { get; set; }
        public long? CommentId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public long FileSize { get; set; }
        public long FileTypeId { get; set; }
        public int TotalPages { get; set; }
        public string Duration { get; set; }
        public string SignedUrl { get; set; }
    }
}
