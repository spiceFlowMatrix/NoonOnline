using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trainning24.Domain.Migrations
{
    public partial class _22042020_initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddtionalServices",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddtionalServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgentCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    CategoryName = table.Column<string>(nullable: true),
                    Commission = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnswerFile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    AnswerId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTimeTracks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    ServiceProvider = table.Column<string>(nullable: true),
                    School = table.Column<string>(nullable: true),
                    SubjectsTaken = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    HardwarePlatform = table.Column<string>(nullable: true),
                    OperatingSystem = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Isp = table.Column<string>(nullable: true),
                    ActivityTime = table.Column<string>(nullable: true),
                    Outtime = table.Column<string>(nullable: true),
                    NetworkSpeed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTimeTracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    ChapterId = table.Column<long>(nullable: false),
                    ItemOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentFile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentStudent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<long>(nullable: false),
                    StudentId = table.Column<long>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentStudent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSubmissionFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    SubmissionId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmissionFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<long>(nullable: false),
                    Score = table.Column<long>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    IsSubmission = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    BookPublisher = table.Column<string>(nullable: true),
                    GradeId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FileId = table.Column<long>(nullable: false),
                    coverimage = table.Column<long>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    PublishedTime = table.Column<string>(nullable: true),
                    PublisherUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buckets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    BucketName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buckets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bundle",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BundleCourse",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    BundleId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BundleCourse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CourseId = table.Column<long>(nullable: false),
                    QuizId = table.Column<long>(nullable: true),
                    ItemOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChapterProgresses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    CourseId = table.Column<long>(nullable: false),
                    ChapterId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Progress = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "chapterQuiz",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    QuizId = table.Column<long>(nullable: false),
                    ChapterId = table.Column<long>(nullable: false),
                    ItemOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chapterQuiz", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    PassMark = table.Column<decimal>(nullable: true),
                    istrial = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseDefination",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    GradeId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    BasePrice = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDefination", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseGrade",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    CourseId = table.Column<long>(nullable: false),
                    Gradeid = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseGrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseItemProgressSync",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Userid = table.Column<long>(nullable: false),
                    Lessonid = table.Column<long>(nullable: false),
                    Lessonprogress = table.Column<long>(nullable: false),
                    Quizid = table.Column<long>(nullable: false),
                    IsStatus = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItemProgressSync", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    currency = table.Column<string>(nullable: true),
                    abbreviation = table.Column<string>(nullable: true),
                    symbol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DefaultValues",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    timeout = table.Column<int>(nullable: false),
                    reminder = table.Column<int>(nullable: false),
                    intervals = table.Column<int>(nullable: false),
                    istimeouton = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deposit",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    DepositDate = table.Column<string>(nullable: true),
                    DepositAmount = table.Column<decimal>(nullable: false),
                    SalesAgentId = table.Column<long>(nullable: false),
                    DocumentIds = table.Column<string>(nullable: true),
                    IsRevoke = table.Column<bool>(nullable: false),
                    IsConfirm = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    PackageName = table.Column<string>(nullable: true),
                    OffTotalPrice = table.Column<int>(nullable: false),
                    OffSubscriptions = table.Column<int>(nullable: false),
                    FreeSubscriptions = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionCommentFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CommentId = table.Column<long>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileTypeId = table.Column<long>(nullable: false),
                    TotalPages = table.Column<int>(nullable: false),
                    Duration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionCommentFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionCommentLikes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    CommentId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Like = table.Column<bool>(nullable: false),
                    DisLike = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionCommentLikes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionComments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    TopicId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TopicId = table.Column<long>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileTypeId = table.Column<long>(nullable: false),
                    TotalPages = table.Column<int>(nullable: false),
                    Duration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionTopic",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    CourseId = table.Column<long>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionTopic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscussionTopicLikes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    TopicId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Like = table.Column<bool>(nullable: false),
                    DisLike = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionTopicLikes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    DocumentUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ERPAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    AccountCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeebackTime",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    FeedbackId = table.Column<long>(nullable: false),
                    Time = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeebackTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Contactid = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false),
                    GradeId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    LessonId = table.Column<long>(nullable: false),
                    ChapterId = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    QaUser = table.Column<long>(nullable: false),
                    Coordinator = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackActivity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    FeedbackId = table.Column<long>(nullable: false),
                    FeedbackTaskId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Type = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackFile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    FeedtimeId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackStaff",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    FeedBackId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Type = table.Column<long>(nullable: false),
                    IsManager = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackStaff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackTask",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    FeedbackId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FileLink = table.Column<string>(nullable: true),
                    Type = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackTaskStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    FeedbackId = table.Column<long>(nullable: false),
                    Status = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackTaskStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackTaskStatusOption",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackTaskStatusOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileProgresses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    LessonId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Progress = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    FileSize = table.Column<long>(nullable: false),
                    FileTypeId = table.Column<long>(nullable: false),
                    TotalPages = table.Column<int>(nullable: false),
                    Duration = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Filetype = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndividualDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    StudentCode = table.Column<string>(nullable: true),
                    StudentName = table.Column<string>(nullable: true),
                    SchoolName = table.Column<string>(nullable: true),
                    FatherNumber = table.Column<string>(nullable: true),
                    IdCardNumber = table.Column<string>(nullable: true),
                    PermanentAddress = table.Column<string>(nullable: true),
                    Village = table.Column<string>(nullable: true),
                    ProvinceId = table.Column<string>(nullable: true),
                    CityId = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    CountryId = table.Column<string>(nullable: true),
                    RefferedBy = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PassportNumber = table.Column<string>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    SexId = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<string>(nullable: true),
                    PlaceOfBirth = table.Column<string>(nullable: true),
                    CurrentAddress = table.Column<string>(nullable: true),
                    MaritalStatusId = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    StudentTazkira = table.Column<string>(nullable: true),
                    ParentTazrika = table.Column<string>(nullable: true),
                    PreviousMarksheets = table.Column<string>(nullable: true),
                    GradeId = table.Column<long>(nullable: true),
                    SoicalMediaLinked = table.Column<string>(nullable: true),
                    SocialMediaAccount = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lesson",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ChapterId = table.Column<long>(nullable: true),
                    ItemOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonAssignmentFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssignmentFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    LessonId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonAssignmentSubmissionFiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    SubmissionId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssignmentSubmissionFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonAssignmentSubmissions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    AssignmentId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<long>(nullable: false),
                    Score = table.Column<long>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    IsSubmission = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssignmentSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonFile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    LessionId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonProgresses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    ChapterId = table.Column<long>(nullable: false),
                    LessonId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Progress = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogObjects",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    TypeId = table.Column<long>(nullable: false),
                    EntityKey = table.Column<long>(nullable: false),
                    ActionUserId = table.Column<long>(nullable: false),
                    TimeStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogObjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogObjectTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    EntityType = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    TestColumn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagementInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    noon_background = table.Column<string>(nullable: true),
                    sales_partner_eng = table.Column<string>(nullable: true),
                    sales_partner_dari = table.Column<string>(nullable: true),
                    school_receipt_notes = table.Column<string>(nullable: true),
                    individual_receipt_notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MetaDataDetail",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    MetadataId = table.Column<long>(nullable: false),
                    DetailId = table.Column<long>(nullable: false),
                    DetailTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaDataDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    NotifiedUserId = table.Column<long>(nullable: false),
                    LogObjectId = table.Column<long>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageCourses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    PackageId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgessSync",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    GradeId = table.Column<long>(nullable: true),
                    IsStatus = table.Column<bool>(nullable: false),
                    LessonProgressId = table.Column<long>(nullable: true),
                    LessonId = table.Column<long>(nullable: true),
                    LessonProgress = table.Column<decimal>(nullable: true),
                    QuizId = table.Column<long>(nullable: true),
                    TotalRecords = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgessSync", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgressStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchagePdf",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    PurchageId = table.Column<string>(nullable: true),
                    PdfFile = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchagePdf", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchageUpload",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    PurchageId = table.Column<string>(nullable: true),
                    PdfFile = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchageUpload", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionFile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    QuestionId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Code = table.Column<string>(maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    NumQuestions = table.Column<int>(nullable: false),
                    PassMark = table.Column<decimal>(nullable: false),
                    TimeOut = table.Column<int>(nullable: false),
                    ItemOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizProgresses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    ChapterId = table.Column<long>(nullable: false),
                    QuizId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Progress = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizProgresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizSummary",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    StudentId = table.Column<long>(nullable: false),
                    QuizId = table.Column<long>(nullable: false),
                    QSummary = table.Column<int>(nullable: false),
                    Attempts = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizSummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizTimerSync",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    isStatus = table.Column<bool>(nullable: false),
                    passingScore = table.Column<string>(nullable: true),
                    quizId = table.Column<long>(nullable: false),
                    quizTime = table.Column<string>(nullable: true),
                    userId = table.Column<long>(nullable: false),
                    yourScore = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizTimerSync", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RoleKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesAgent",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    AgentName = table.Column<string>(nullable: true),
                    AgentCategoryId = table.Column<long>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PartnerBackgroud = table.Column<string>(nullable: true),
                    FocalPoint = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    PhysicalAddress = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    AgreedMonthlyDeposit = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesAgent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesTax",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Tax = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesTax", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    RegisterNumber = table.Column<string>(nullable: true),
                    SchoolTypeId = table.Column<string>(nullable: true),
                    SchoolName = table.Column<string>(nullable: true),
                    SchoolAddress = table.Column<string>(nullable: true),
                    SectionTypeId = table.Column<string>(nullable: true),
                    NumberOfTeacherMale = table.Column<int>(nullable: false),
                    NumberOfTeacherFemale = table.Column<int>(nullable: false),
                    NumberOfStudentMale = table.Column<int>(nullable: false),
                    NumberOfStudentFemale = table.Column<int>(nullable: false),
                    NumberOfStaffMale = table.Column<int>(nullable: false),
                    NumberOfStaffFemale = table.Column<int>(nullable: false),
                    PowerAddressId = table.Column<string>(nullable: true),
                    InternetAccessId = table.Column<string>(nullable: true),
                    BuildingOwnershipId = table.Column<string>(nullable: true),
                    Computers = table.Column<string>(nullable: true),
                    Monitors = table.Column<string>(nullable: true),
                    Routers = table.Column<string>(nullable: true),
                    Dongles = table.Column<string>(nullable: true),
                    SchoolLicense = table.Column<string>(nullable: true),
                    RegisterationPaper = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentChapterProgress",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    ChapterId = table.Column<long>(nullable: false),
                    ChapterStatus = table.Column<long>(nullable: false),
                    StudentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentChapterProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourseProgress",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    CourseId = table.Column<long>(nullable: false),
                    CourseStatus = table.Column<long>(nullable: false),
                    CourseProgress = table.Column<decimal>(nullable: false),
                    StudentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentLessonProgress",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    LessonId = table.Column<long>(nullable: false),
                    LessonStatus = table.Column<long>(nullable: false),
                    StudentId = table.Column<long>(nullable: false),
                    LessonProgress = table.Column<decimal>(nullable: false),
                    Duration = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentLessonProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentProgress",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    CourseId = table.Column<long>(nullable: false),
                    ChapterId = table.Column<long>(nullable: false),
                    LessonId = table.Column<long>(nullable: false),
                    CourseStatus = table.Column<long>(nullable: false),
                    ChapterStatus = table.Column<long>(nullable: false),
                    LessonStatus = table.Column<long>(nullable: false),
                    CourseProgress = table.Column<decimal>(nullable: false),
                    StudentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudParents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    StudentId = table.Column<long>(nullable: false),
                    ParentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudParents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionMetadata",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    CourseId = table.Column<string>(nullable: true),
                    DiscountPackageId = table.Column<long>(nullable: false),
                    SalesAgentId = table.Column<long>(nullable: false),
                    EnrollmentFromDate = table.Column<string>(nullable: true),
                    EnrollmentToDate = table.Column<string>(nullable: true),
                    SubscriptionTypeId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    PurchageId = table.Column<string>(nullable: true),
                    NoOfMonths = table.Column<int>(nullable: false),
                    PackageId = table.Column<long>(nullable: false),
                    ServiceId = table.Column<string>(nullable: true),
                    Tax = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionMetadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    SubscriptionMetadataId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskActivityCategoryFeedBacks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskActivityCategoryFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskActivityFeedBacks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    TaskId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskActivityFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskCategoryFeedBacks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCategoryFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskFeedBacks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CategoryId = table.Column<long>(nullable: false),
                    GradeId = table.Column<long>(nullable: true),
                    CourseId = table.Column<long>(nullable: true),
                    ChapterId = table.Column<long>(nullable: true),
                    LessonId = table.Column<long>(nullable: true),
                    StartDate = table.Column<string>(nullable: true),
                    ComplatedDate = table.Column<string>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Device = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    AppVersion = table.Column<string>(nullable: true),
                    OperatingSystem = table.Column<string>(nullable: true),
                    ArchivedDate = table.Column<string>(nullable: true),
                    Assign = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskFileFeedBacks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    TaskId = table.Column<long>(nullable: false),
                    FileId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFileFeedBacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TermsAndConditions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Terms = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAndConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeIntervals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Interval = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeIntervals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCourse",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    StartDate = table.Column<string>(nullable: true),
                    EndDate = table.Column<string>(nullable: true),
                    IsExpire = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    CourseId = table.Column<long>(nullable: true),
                    AssignmentId = table.Column<long>(nullable: true),
                    LessionId = table.Column<long>(nullable: true),
                    DiscussionId = table.Column<long>(nullable: true),
                    CommentId = table.Column<long>(nullable: true),
                    ChapterId = table.Column<long>(nullable: true),
                    QuizId = table.Column<long>(nullable: true),
                    FileId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserQuizResults",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    QuizId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    TotalQuestion = table.Column<long>(nullable: false),
                    AnsweredQuestion = table.Column<long>(nullable: false),
                    PerformDate = table.Column<string>(nullable: true),
                    PassingScore = table.Column<long>(nullable: false),
                    Score = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    ProfilePicUrl = table.Column<string>(nullable: true),
                    IsBlocked = table.Column<bool>(nullable: false),
                    is_skippable = table.Column<bool>(nullable: false),
                    timeout = table.Column<int>(nullable: false),
                    reminder = table.Column<int>(nullable: false),
                    istimeouton = table.Column<bool>(nullable: false),
                    intervals = table.Column<int>(nullable: false),
                    phonenumber = table.Column<string>(nullable: true),
                    AuthId = table.Column<string>(nullable: true),
                    isfirstlogin = table.Column<bool>(nullable: false),
                    isforgot = table.Column<bool>(nullable: false),
                    forgotkey = table.Column<string>(nullable: true),
                    is_discussion_authorized = table.Column<bool>(nullable: false),
                    is_library_authorized = table.Column<bool>(nullable: false),
                    is_assignment_authorized = table.Column<bool>(nullable: false),
                    istrial = table.Column<bool>(nullable: false),
                    isallowmap = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    DeviceToken = table.Column<string>(nullable: true),
                    DeviceType = table.Column<string>(maxLength: 10, nullable: true),
                    Version = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    QuestionTypeId = table.Column<long>(nullable: false),
                    QuestionText = table.Column<string>(maxLength: 255, nullable: false),
                    Explanation = table.Column<string>(maxLength: 255, nullable: true),
                    IsMultiAnswer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_QuestionType_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(maxLength: 255, nullable: false),
                    ExtraText = table.Column<string>(maxLength: 255, nullable: true),
                    IsCorrect = table.Column<bool>(nullable: false),
                    QuestionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<int>(nullable: true),
                    LastModificationTime = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeleterUserId = table.Column<int>(nullable: true),
                    DeletionTime = table.Column<string>(nullable: true),
                    QuizId = table.Column<long>(nullable: false),
                    QuestionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestion_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuizQuestion_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AddtionalServices",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name", "Price" },
                values: new object[,]
                {
                    { 1L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Homework", "0" },
                    { 2L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Discussion", "0" },
                    { 3L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Library", "0" },
                    { 4L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Parental Control", "0" }
                });

            migrationBuilder.InsertData(
                table: "Buckets",
                columns: new[] { "Id", "BucketName", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[,]
                {
                    { 11L, "t24-secure-files", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 9L, "t24-primary-pdf-storage", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 8L, "t24-primary-image-storage", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 7L, "t24-primary-audio-storage", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 6L, "t24-app-builds", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 10L, "t24-primary-video-storage", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 4L, "edg-sales-primary-storage", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 3L, "edg-primary-profile-image-storage", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 2L, "edg-primary-course-image-storage", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 1L, "core-api-sql-migrations", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 5L, "eu.artifacts.training24-197210.appspot.com", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "DefaultValues",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "intervals", "istimeouton", "reminder", "timeout" },
                values: new object[] { 1L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, 3, true, 5, 15 });

            migrationBuilder.InsertData(
                table: "ERPAccounts",
                columns: new[] { "Id", "AccountCode", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Type" },
                values: new object[,]
                {
                    { 5L, "", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, 5 },
                    { 4L, "", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, 4 },
                    { 3L, "", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, 3 },
                    { 1L, "", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, 1 },
                    { 2L, "", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "FeedBackTaskStatusOption",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name" },
                values: new object[,]
                {
                    { 1L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "UnApproved" },
                    { 2L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Approved" },
                    { 3L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Completed" }
                });

            migrationBuilder.InsertData(
                table: "FileTypes",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "Filetype", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[,]
                {
                    { 6L, "22-04-2020 06:46:25 PM", 1, null, null, "Xlsx", false, null, null },
                    { 8L, "22-04-2020 06:46:25 PM", 1, null, null, "PPT", false, null, null },
                    { 7L, "22-04-2020 06:46:25 PM", 1, null, null, "Docs", false, null, null },
                    { 5L, "22-04-2020 06:46:25 PM", 1, null, null, "Audio", false, null, null },
                    { 4L, "22-04-2020 06:46:25 PM", 1, null, null, "Zip", false, null, null },
                    { 3L, "22-04-2020 06:46:25 PM", 1, null, null, "Image", false, null, null },
                    { 2L, "22-04-2020 06:46:25 PM", 1, null, null, "Video", false, null, null },
                    { 1L, "22-04-2020 06:46:25 PM", 1, null, null, "PDF", false, null, null }
                });

            migrationBuilder.InsertData(
                table: "LogObjectTypes",
                columns: new[] { "Id", "Action", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "EntityType", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[,]
                {
                    { 28L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Comment", false, null, null },
                    { 29L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Comment", false, null, null },
                    { 30L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Comment", false, null, null },
                    { 31L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "DiscussionFile", false, null, null },
                    { 32L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "DiscussionFile", false, null, null },
                    { 33L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "DiscussionFile", false, null, null },
                    { 34L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "CommentFile", false, null, null },
                    { 35L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "CommentFile", false, null, null },
                    { 36L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "CommentFile", false, null, null },
                    { 37L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "AddtionalServices", false, null, null },
                    { 38L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "AddtionalServices", false, null, null },
                    { 39L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "AddtionalServices", false, null, null },
                    { 40L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Package", false, null, null },
                    { 41L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Package", false, null, null },
                    { 42L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Package", false, null, null },
                    { 43L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "PackageCourse", false, null, null },
                    { 44L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "PackageCourse", false, null, null },
                    { 45L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "PackageCourse", false, null, null },
                    { 46L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "BookAdd", false, null, null },
                    { 47L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "BookUpdate", false, null, null },
                    { 48L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "BookDelete", false, null, null },
                    { 49L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "BookPublish", false, null, null },
                    { 26L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Discussion", false, null, null },
                    { 25L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Discussion", false, null, null },
                    { 27L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Discussion", false, null, null },
                    { 23L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Notification", false, null, null },
                    { 24L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Notification", false, null, null },
                    { 1L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Course", false, null, null },
                    { 2L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Course", false, null, null },
                    { 3L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Course", false, null, null },
                    { 4L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "User", false, null, null },
                    { 5L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "User", false, null, null },
                    { 7L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Chapter", false, null, null },
                    { 8L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Chapter", false, null, null },
                    { 9L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Chapter", false, null, null },
                    { 10L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Lesson", false, null, null },
                    { 11L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Lesson", false, null, null },
                    { 6L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "User", false, null, null },
                    { 13L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Quiz", false, null, null },
                    { 14L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Quiz", false, null, null },
                    { 15L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Quiz", false, null, null },
                    { 16L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Question", false, null, null },
                    { 17L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "Question", false, null, null },
                    { 18L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Question", false, null, null },
                    { 19L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "File", false, null, null },
                    { 20L, "Update", "22-04-2020 06:46:25 PM", 1, null, null, "File", false, null, null },
                    { 21L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "File", false, null, null },
                    { 22L, "Create", "22-04-2020 06:46:25 PM", 1, null, null, "Notification", false, null, null },
                    { 12L, "Delete", "22-04-2020 06:46:25 PM", 1, null, null, "Lesson", false, null, null }
                });

            migrationBuilder.InsertData(
                table: "QuestionType",
                columns: new[] { "Id", "Code", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[,]
                {
                    { 4L, "abc", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 3L, "nonmcq", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 1L, "MCQ", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null },
                    { 2L, "SA", "22-04-2020 06:46:25 PM", 1, null, null, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name", "RoleKey" },
                values: new object[,]
                {
                    { 11L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Quality assurance", "quality_assurance" },
                    { 17L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Sales Agent", "sales_agent" },
                    { 16L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Graphics staff", "graphics_staff" },
                    { 18L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Parent", "parent" },
                    { 15L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Editing staff", "editing_staff" },
                    { 14L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Filming staff", "filming_staff" },
                    { 13L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Sales admin", "sales_admin" },
                    { 12L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Feedback edge team", "feedback_edge_team" },
                    { 10L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Graphics team leader", "graphics_team_leader" },
                    { 3L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Teacher", "teacher" },
                    { 8L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Edit team leader", "edit_team_leader" },
                    { 7L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Coordinator", "coordinator" },
                    { 6L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "AAF manager", "aaf_manager" },
                    { 5L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Customer", "customer" },
                    { 4L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Student", "student" },
                    { 2L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Analyst", "analyst" },
                    { 1L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Administrator", "admin" },
                    { 9L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, "Shooting team leader", "shooting_team_leader" }
                });

            migrationBuilder.InsertData(
                table: "SalesTax",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Tax" },
                values: new object[] { 1L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, 4 });

            migrationBuilder.InsertData(
                table: "TaskActivityCategoryFeedBacks",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name" },
                values: new object[,]
                {
                    { 4L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "Archived" },
                    { 3L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "Completed" },
                    { 1L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "In Queue" },
                    { 2L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "In Progress" }
                });

            migrationBuilder.InsertData(
                table: "TaskCategoryFeedBacks",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Name" },
                values: new object[,]
                {
                    { 1L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "Something Isn't Working in the App" },
                    { 2L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "Video Issue" },
                    { 3L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "Text Issue" },
                    { 4L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "Lesson Assignment Issue" },
                    { 5L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "Chapter Assignment Issue" },
                    { 6L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "Quiz Issue" },
                    { 7L, "22-04-2020 01:16:25 PM", 1, null, null, false, null, null, "General Feedback" }
                });

            migrationBuilder.InsertData(
                table: "TimeIntervals",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "Interval", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[] { 1L, "22-04-2020 06:46:25 PM", 1, null, null, 5, false, null, null });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId", "RoleId", "UserId" },
                values: new object[] { 1L, "22-04-2020 06:46:25 PM", 1, null, null, false, null, null, 1L, 1L });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthId", "Bio", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "Email", "FullName", "IsBlocked", "IsDeleted", "LastModificationTime", "LastModifierUserId", "Password", "ProfilePicUrl", "Username", "forgotkey", "intervals", "is_assignment_authorized", "is_discussion_authorized", "is_library_authorized", "is_skippable", "isallowmap", "isfirstlogin", "isforgot", "istimeouton", "istrial", "phonenumber", "reminder", "timeout" },
                values: new object[] { 1L, "auth0|5c08a3afe9f0262e9378a9b2", "string", "22-04-2020 06:46:25 PM", 1, null, null, "adminuser@email.com", "Admin", false, false, null, null, "F925916E2754E5E03F75DD58A5733251", "https://www.googleapis.com/download/storage/v1/b/edg-primary-profile-image-storage/o/3_d279c045-9325-4cbb-bd31-16a40f70a080.png?generation=1540626401907278&alt=media", "Admin", null, 0, false, false, false, true, false, false, false, false, false, null, 0, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionTypeId",
                table: "Question",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_QuestionId",
                table: "QuestionAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestion_QuestionId",
                table: "QuizQuestion",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestion_QuizId",
                table: "QuizQuestion",
                column: "QuizId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddtionalServices");

            migrationBuilder.DropTable(
                name: "AgentCategory");

            migrationBuilder.DropTable(
                name: "AnswerFile");

            migrationBuilder.DropTable(
                name: "AppTimeTracks");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "AssignmentFile");

            migrationBuilder.DropTable(
                name: "AssignmentStudent");

            migrationBuilder.DropTable(
                name: "AssignmentSubmissionFiles");

            migrationBuilder.DropTable(
                name: "AssignmentSubmissions");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Buckets");

            migrationBuilder.DropTable(
                name: "Bundle");

            migrationBuilder.DropTable(
                name: "BundleCourse");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "ChapterProgresses");

            migrationBuilder.DropTable(
                name: "chapterQuiz");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "CourseDefination");

            migrationBuilder.DropTable(
                name: "CourseGrade");

            migrationBuilder.DropTable(
                name: "CourseItemProgressSync");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "DefaultValues");

            migrationBuilder.DropTable(
                name: "Deposit");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "DiscussionCommentFiles");

            migrationBuilder.DropTable(
                name: "DiscussionCommentLikes");

            migrationBuilder.DropTable(
                name: "DiscussionComments");

            migrationBuilder.DropTable(
                name: "DiscussionFiles");

            migrationBuilder.DropTable(
                name: "DiscussionTopic");

            migrationBuilder.DropTable(
                name: "DiscussionTopicLikes");

            migrationBuilder.DropTable(
                name: "DocumentDetails");

            migrationBuilder.DropTable(
                name: "ERPAccounts");

            migrationBuilder.DropTable(
                name: "FeebackTime");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "FeedBackActivity");

            migrationBuilder.DropTable(
                name: "FeedBackCategory");

            migrationBuilder.DropTable(
                name: "FeedbackFile");

            migrationBuilder.DropTable(
                name: "FeedBackStaff");

            migrationBuilder.DropTable(
                name: "FeedBackTask");

            migrationBuilder.DropTable(
                name: "FeedBackTaskStatus");

            migrationBuilder.DropTable(
                name: "FeedBackTaskStatusOption");

            migrationBuilder.DropTable(
                name: "FileProgresses");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "FileTypes");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "IndividualDetails");

            migrationBuilder.DropTable(
                name: "Lesson");

            migrationBuilder.DropTable(
                name: "LessonAssignmentFiles");

            migrationBuilder.DropTable(
                name: "LessonAssignments");

            migrationBuilder.DropTable(
                name: "LessonAssignmentSubmissionFiles");

            migrationBuilder.DropTable(
                name: "LessonAssignmentSubmissions");

            migrationBuilder.DropTable(
                name: "LessonFile");

            migrationBuilder.DropTable(
                name: "LessonProgresses");

            migrationBuilder.DropTable(
                name: "LogObjects");

            migrationBuilder.DropTable(
                name: "LogObjectTypes");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ManagementInfo");

            migrationBuilder.DropTable(
                name: "MetaDataDetail");

            migrationBuilder.DropTable(
                name: "NotificationLogs");

            migrationBuilder.DropTable(
                name: "PackageCourses");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "ProgessSync");

            migrationBuilder.DropTable(
                name: "ProgressStatus");

            migrationBuilder.DropTable(
                name: "PurchagePdf");

            migrationBuilder.DropTable(
                name: "PurchageUpload");

            migrationBuilder.DropTable(
                name: "QuestionAnswer");

            migrationBuilder.DropTable(
                name: "QuestionFile");

            migrationBuilder.DropTable(
                name: "QuizProgresses");

            migrationBuilder.DropTable(
                name: "QuizQuestion");

            migrationBuilder.DropTable(
                name: "QuizSummary");

            migrationBuilder.DropTable(
                name: "QuizTimerSync");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "SalesAgent");

            migrationBuilder.DropTable(
                name: "SalesTax");

            migrationBuilder.DropTable(
                name: "School");

            migrationBuilder.DropTable(
                name: "SchoolDetails");

            migrationBuilder.DropTable(
                name: "StudentChapterProgress");

            migrationBuilder.DropTable(
                name: "StudentCourseProgress");

            migrationBuilder.DropTable(
                name: "StudentLessonProgress");

            migrationBuilder.DropTable(
                name: "StudentProgress");

            migrationBuilder.DropTable(
                name: "StudParents");

            migrationBuilder.DropTable(
                name: "SubscriptionMetadata");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "TaskActivityCategoryFeedBacks");

            migrationBuilder.DropTable(
                name: "TaskActivityFeedBacks");

            migrationBuilder.DropTable(
                name: "TaskCategoryFeedBacks");

            migrationBuilder.DropTable(
                name: "TaskFeedBacks");

            migrationBuilder.DropTable(
                name: "TaskFileFeedBacks");

            migrationBuilder.DropTable(
                name: "TermsAndConditions");

            migrationBuilder.DropTable(
                name: "TimeIntervals");

            migrationBuilder.DropTable(
                name: "UserCourse");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "UserQuizResults");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "QuestionType");
        }
    }
}
