using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class Books : EntityBase
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }
        public string BookPublisher { get; set; }
        public string GradeId { get; set; }
        public string Description { get; set; }
        public long FileId { get; set; }
        public long coverimage { get; set; }
        public bool IsPublished { get; set; }
        public string PublishedTime { get; set; }
        public int? PublisherUserId { get; set; }
    }
}
