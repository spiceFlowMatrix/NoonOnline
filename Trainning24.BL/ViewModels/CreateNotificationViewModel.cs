using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels.Users;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.Quiz;

namespace Trainning24.BL.ViewModels
{
    public class CreateNotificationViewModel
    {
        public long Id { get; set; }
        public string Tag { get; set; }
        public int Type { get; set; }
        public long UserId { get; set; }
        public bool IsRead { get; set; }
        public long? CourseId { get; set; }
        public long? AssignmentId { get; set; }
        public long? LessionId { get; set; }
        public long? DiscussionId { get; set; }
        public long? CommentId { get; set; }
        public long? ChapterId {get;set;}
        public long? QuizId { get; set; }
        public long? FileId { get; set; }
        public string DateCreated { get; set; }
        public long CreatorUserId { get; set; }
        public UserDetails User { get; set; }
        public CourseDto Course { get; set; }
        public ChapterDTO Chapter { get; set; }
        public DiscussionTopicDto discussion { get; set; }
        public DiscussionCommentDto comments { get; set; }
        public LessonDTO lesson { get; set; }
        public QuizDTO quiz { get; set; }
        public List<DiscussionCommentFileViewModel> files { get; set; }
    }
}
