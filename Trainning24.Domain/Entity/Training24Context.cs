using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Trainning24.Domain.Entity
{
    public class Training24Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Deposit> Deposit { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Bundle> Bundle { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Chapter> Chapter { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<QuestionType> QuestionType { get; set; }
        public DbSet<BundleCourse> BundleCourse { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<FileTypes> FileTypes { get; set; }
        public DbSet<QuizQuestion> QuizQuestion { get; set; }
        public DbSet<UserCourse> UserCourse { get; set; }
        public DbSet<LessonFile> LessonFile { get; set; }
        public DbSet<UserSessions> UserSessions { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<CourseGrade> CourseGrade { get; set; }
        public DbSet<Assignment> Assignment { get; set; }
        public DbSet<AssignmentFile> AssignmentFile { get; set; }
        public DbSet<AssignmentStudent> AssignmentStudent { get; set; }
        public DbSet<QuestionFile> QuestionFile { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<AnswerFile> AnswerFile { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<FeedBackCategory> FeedBackCategory { get; set; }
        public DbSet<FeebackTime> FeebackTime { get; set; }
        public DbSet<FeedbackFile> FeedbackFile { get; set; }
        public DbSet<StudentCourseProgress> StudentCourseProgress { get; set; }
        public DbSet<StudentChapterProgress> StudentChapterProgress { get; set; }
        public DbSet<StudentLessonProgress> StudentLessonProgress { get; set; }
        public DbSet<StudentProgress> StudentProgress { get; set; }
        public DbSet<ProgressStatus> ProgressStatus { get; set; }
        public DbSet<SalesAgent> SalesAgent { get; set; }
        public DbSet<CourseDefination> CourseDefination { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<AgentCategory> AgentCategory { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<SubscriptionMetadata> SubscriptionMetadata { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<QuizSummary> QuizSummary { get; set; }
        public DbSet<IndividualDetails> IndividualDetails { get; set; }
        public DbSet<DocumentDetails> DocumentDetails { get; set; }
        public DbSet<SchoolDetails> SchoolDetails { get; set; }
        public DbSet<MetaDataDetail> MetaDataDetail { get; set; }
        public DbSet<CourseItemProgressSync> CourseItemProgressSync { get; set; }
        public DbSet<DefaultValues> DefaultValues { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<FeedBackStaff> FeedBackStaff { get; set; }
        public DbSet<FeedBackTask> FeedBackTask { get; set; }
        public DbSet<FeedBackActivity> FeedBackActivity { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<ProgessSync> ProgessSync { get; set; }
        public DbSet<QuizTimerSync> QuizTimerSync { get; set; }
        public DbSet<ChapterQuiz> chapterQuiz { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Buckets> Buckets { get; set; }
        public DbSet<FeedBackTaskStatus> FeedBackTaskStatus { get; set; }
        public DbSet<FeedBackTaskStatusOption> FeedBackTaskStatusOption { get; set; }
        public DbSet<PurchagePdf> PurchagePdf { get; set; }
        public DbSet<PurchageUpload> PurchageUpload { get; set; }
        public DbSet<ManagementInfo> ManagementInfo { get; set; }
        public DbSet<DiscussionTopic> DiscussionTopic { get; set; }
        public DbSet<DiscussionFiles> DiscussionFiles { get; set; }
        public DbSet<DiscussionComments> DiscussionComments { get; set; }
        public DbSet<DiscussionCommentFiles> DiscussionCommentFiles { get; set; }
        public DbSet<UserNotifications> UserNotifications { get; set; }
        public DbSet<LogObjectTypes> LogObjectTypes { get; set; }
        public DbSet<LogObject> LogObjects { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<AddtionalServices> AddtionalServices { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageCourse> PackageCourses { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<TimeInterval> TimeIntervals { get; set; }
        public DbSet<SalesTax> SalesTax { get; set; }
        public DbSet<ERPAccounts> ERPAccounts { get; set; }
        public DbSet<LessonAssignment> LessonAssignments { get; set; }
        public DbSet<LessonAssignmentFile> LessonAssignmentFiles { get; set; }
        public DbSet<AssignmentSubmissionFile> AssignmentSubmissionFiles { get; set; }
        public DbSet<LessonAssignmentSubmission> LessonAssignmentSubmissions { get; set; }
        public DbSet<LessonAssignmentSubmissionFile> LessonAssignmentSubmissionFiles { get; set; }
        public DbSet<AppTimeTrack> AppTimeTracks { get; set; }
        public DbSet<ChapterProgress> ChapterProgresses { get; set; }
        public DbSet<LessonProgress> LessonProgresses { get; set; }
        public DbSet<QuizProgress> QuizProgresses { get; set; }
        public DbSet<FileProgress> FileProgresses { get; set; }
        public DbSet<UserQuizResult> UserQuizResults { get; set; }
        public DbSet<StudParent> StudParents { get; set; }
        public DbSet<DiscussionTopicLikes> DiscussionTopicLikes { get; set; }
        public DbSet<DiscussionCommentLikes> DiscussionCommentLikes { get; set; }
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; }
        //Feedback new module
        public DbSet<TaskFeedBack> TaskFeedBacks { get; set; }
        public DbSet<TaskCategoryFeedBack> TaskCategoryFeedBacks { get; set; }
        public DbSet<TaskFileFeedBack> TaskFileFeedBacks { get; set; }
        public DbSet<TaskActivityFeedBack> TaskActivityFeedBacks { get; set; }
        public DbSet<TaskActivityCategoryFeedBack> TaskActivityCategoryFeedBacks { get; set; }

        public Training24Context(DbContextOptions<Training24Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Administrator", RoleKey = "admin" },
                new Role { Id = 2, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Analyst", RoleKey = "analyst" },
                new Role { Id = 3, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Teacher", RoleKey = "teacher" },
                new Role { Id = 4, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Student", RoleKey = "student" },
                new Role { Id = 5, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Customer", RoleKey = "customer" },
                new Role { Id = 6, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "AAF manager", RoleKey = "aaf_manager" },
                new Role { Id = 7, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Coordinator", RoleKey = "coordinator" },
                new Role { Id = 8, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Edit team leader", RoleKey = "edit_team_leader" },
                new Role { Id = 9, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Shooting team leader", RoleKey = "shooting_team_leader" },
                new Role { Id = 10, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Graphics team leader", RoleKey = "graphics_team_leader" },
                new Role { Id = 11, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Quality assurance", RoleKey = "quality_assurance" },
                new Role { Id = 12, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Feedback edge team", RoleKey = "feedback_edge_team" },
                new Role { Id = 13, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Sales admin", RoleKey = "sales_admin" },
                new Role { Id = 14, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Filming staff", RoleKey = "filming_staff" },
                new Role { Id = 15, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Editing staff", RoleKey = "editing_staff" },
                new Role { Id = 16, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Graphics staff", RoleKey = "graphics_staff" },
                new Role { Id = 17, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Sales Agent", RoleKey = "sales_agent" },
                new Role { Id = 18, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Parent", RoleKey = "parent" }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, RoleId = 1, UserId = 1 }
            );

            modelBuilder.Entity<Buckets>().HasData(
                new Buckets { Id = 1, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "core-api-sql-migrations" },
                new Buckets { Id = 2, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "edg-primary-course-image-storage" },
                new Buckets { Id = 3, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "edg-primary-profile-image-storage" },
                new Buckets { Id = 4, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "edg-sales-primary-storage" },
                new Buckets { Id = 5, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "eu.artifacts.training24-197210.appspot.com" },
                new Buckets { Id = 6, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "t24-app-builds" },
                new Buckets { Id = 7, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "t24-primary-audio-storage" },
                new Buckets { Id = 8, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "t24-primary-image-storage" },
                new Buckets { Id = 9, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "t24-primary-pdf-storage" },
                new Buckets { Id = 10, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "t24-primary-video-storage" },
                new Buckets { Id = 11, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, BucketName = "t24-secure-files" }
            );


            modelBuilder.Entity<FeedBackTaskStatusOption>().HasData(
                new FeedBackTaskStatusOption { Id = 1, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "UnApproved" },
                new FeedBackTaskStatusOption { Id = 2, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Approved" },
                new FeedBackTaskStatusOption { Id = 3, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Name = "Completed" }
            );

            modelBuilder.Entity<FileTypes>().HasData(
                new FileTypes { Id = 1, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Filetype = "PDF" },
                new FileTypes { Id = 2, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Filetype = "Video" },
                new FileTypes { Id = 3, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Filetype = "Image" },
                new FileTypes { Id = 4, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Filetype = "Zip" },
                new FileTypes { Id = 5, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Filetype = "Audio" },
                new FileTypes { Id = 6, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Filetype = "Xlsx" },
                new FileTypes { Id = 7, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Filetype = "Docs" },
                new FileTypes { Id = 8, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Filetype = "PPT" }
            );

            modelBuilder.Entity<QuestionType>().HasData(
                new QuestionType { Id = 1, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Code = "MCQ" },
                new QuestionType { Id = 2, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Code = "SA" },
                new QuestionType { Id = 3, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Code = "nonmcq" },
                new QuestionType { Id = 4, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, Code = "abc" }
            );

            modelBuilder.Entity<DefaultValues>().HasData(
                new DefaultValues { Id = 1, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null, intervals = 3, istimeouton = true, reminder = 5, timeout = 15 }
            );


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    CreationTime = DateTime.Now.ToString(),
                    CreatorUserId = 1,
                    LastModificationTime = null,
                    DeleterUserId = null,
                    DeletionTime = null,
                    IsDeleted = false,
                    LastModifierUserId = null,
                    Username = "Admin",
                    FullName = "Admin",
                    Password = "F925916E2754E5E03F75DD58A5733251",
                    Email = "adminuser@email.com",
                    Bio = "string",
                    ProfilePicUrl = "https://www.googleapis.com/download/storage/v1/b/edg-primary-profile-image-storage/o/3_d279c045-9325-4cbb-bd31-16a40f70a080.png?generation=1540626401907278&alt=media",
                    IsBlocked = false,
                    is_skippable = true,
                    istimeouton = false,
                    reminder = 0,
                    timeout = 0,
                    intervals = 0,
                    phonenumber = null,
                    AuthId = "auth0|5c08a3afe9f0262e9378a9b2",
                    isfirstlogin = false

                }
            );

            modelBuilder.Entity<LogObjectTypes>().HasData(
                new LogObjectTypes { Id = 1, EntityType = "Course", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 2, EntityType = "Course", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 3, EntityType = "Course", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 4, EntityType = "User", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 5, EntityType = "User", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 6, EntityType = "User", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 7, EntityType = "Chapter", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 8, EntityType = "Chapter", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 9, EntityType = "Chapter", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 10, EntityType = "Lesson", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 11, EntityType = "Lesson", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 12, EntityType = "Lesson", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 13, EntityType = "Quiz", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 14, EntityType = "Quiz", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 15, EntityType = "Quiz", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 16, EntityType = "Question", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 17, EntityType = "Question", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 18, EntityType = "Question", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 19, EntityType = "File", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 20, EntityType = "File", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 21, EntityType = "File", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 22, EntityType = "Notification", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 23, EntityType = "Notification", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 24, EntityType = "Notification", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 25, EntityType = "Discussion", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 26, EntityType = "Discussion", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 27, EntityType = "Discussion", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 28, EntityType = "Comment", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 29, EntityType = "Comment", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 30, EntityType = "Comment", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 31, EntityType = "DiscussionFile", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 32, EntityType = "DiscussionFile", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 33, EntityType = "DiscussionFile", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 34, EntityType = "CommentFile", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 35, EntityType = "CommentFile", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 36, EntityType = "CommentFile", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 37, EntityType = "AddtionalServices", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 38, EntityType = "AddtionalServices", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 39, EntityType = "AddtionalServices", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 40, EntityType = "Package", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 41, EntityType = "Package", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 42, EntityType = "Package", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 43, EntityType = "PackageCourse", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 44, EntityType = "PackageCourse", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 45, EntityType = "PackageCourse", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 46, EntityType = "BookAdd", Action = "Create", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 47, EntityType = "BookUpdate", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 48, EntityType = "BookDelete", Action = "Delete", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new LogObjectTypes { Id = 49, EntityType = "BookPublish", Action = "Update", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null }
                );

            modelBuilder.Entity<AddtionalServices>().HasData(
                new AddtionalServices { Id = 1, Name = "Homework", Price = "0", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new AddtionalServices { Id = 2, Name = "Discussion", Price = "0", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new AddtionalServices { Id = 3, Name = "Library", Price = "0", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new AddtionalServices { Id = 4, Name = "Parental Control", Price = "0", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null }
                );

            modelBuilder.Entity<TimeInterval>().HasData(
                new TimeInterval { Id = 1, Interval = 5, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null }
                );

            modelBuilder.Entity<SalesTax>().HasData(
                new SalesTax { Id = 1, Tax = 4, CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null }
                );

            modelBuilder.Entity<ERPAccounts>().HasData(
                new ERPAccounts { Id = 1, Type = 1, AccountCode = "", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new ERPAccounts { Id = 2, Type = 2, AccountCode = "", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new ERPAccounts { Id = 3, Type = 3, AccountCode = "", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new ERPAccounts { Id = 4, Type = 4, AccountCode = "", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null },
                new ERPAccounts { Id = 5, Type = 5, AccountCode = "", CreationTime = DateTime.Now.ToString(), CreatorUserId = 1, LastModificationTime = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, LastModifierUserId = null }
                );

            modelBuilder.Entity<TaskCategoryFeedBack>().HasData(
                new TaskCategoryFeedBack { Id = 1, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "Something Isn't Working in the App" },
                new TaskCategoryFeedBack { Id = 2, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "Video Issue" },
                new TaskCategoryFeedBack { Id = 3, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "Text Issue" },
                new TaskCategoryFeedBack { Id = 4, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "Lesson Assignment Issue" },
                new TaskCategoryFeedBack { Id = 5, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "Chapter Assignment Issue" },
                new TaskCategoryFeedBack { Id = 6, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "Quiz Issue" },
                new TaskCategoryFeedBack { Id = 7, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "General Feedback" }
                );

            modelBuilder.Entity<TaskActivityCategoryFeedBack>().HasData(
                new TaskActivityCategoryFeedBack { Id = 1, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "In Queue" },
                new TaskActivityCategoryFeedBack { Id = 2, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "In Progress" },
                new TaskActivityCategoryFeedBack { Id = 3, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "Completed" },
                new TaskActivityCategoryFeedBack { Id = 4, CreationTime = DateTime.UtcNow.ToString(), CreatorUserId = 1, LastModificationTime = null, LastModifierUserId = null, DeleterUserId = null, DeletionTime = null, IsDeleted = false, Name = "Archived" }
                );
        }
    }
}
