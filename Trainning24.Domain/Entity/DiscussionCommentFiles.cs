using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class DiscussionCommentFiles : EntityBase
    {
        public string Name { get; set; }
        public long? CommentId { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public long FileSize { get; set; }
        public long FileTypeId { get; set; }
        public int TotalPages { get; set; }
        public string Duration { get; set; }
    }
}
