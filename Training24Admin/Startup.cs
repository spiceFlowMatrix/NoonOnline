using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Training24Admin.Model;
using Training24Admin.Security.Bearer.Helpers;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Erp;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Language;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;
using Trainning24.Repository.EF.Generics;
using WebAPIApplication;

namespace Training24Admin
{
    public class Startup
    {
        private FilesBusiness FilesBusiness;
        private bool includeControllerXmlComments;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string dbconnection = "";
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Production")
            {
                dbconnection = Environment.GetEnvironmentVariable("ASPNET_DB_CONNECTIONSTRING");
            }
            else
            {
                dbconnection = Configuration.GetConnectionString("DefaultConnection");
            }

            //EmailSettings emailSettings1 = new EmailSettings();
            //emailSettings1.PrimaryDomain = Environment.GetEnvironmentVariable("PRIMARY_DOMAIN");
            //emailSettings1.PrimaryPort = Convert.ToInt32(Environment.GetEnvironmentVariable("PRIMARY_PORT"));
            //emailSettings1.UsernameEmail = Environment.GetEnvironmentVariable("USERNAME_EMAIL");
            //emailSettings1.UsernamePassword = Environment.GetEnvironmentVariable("USERNAME_PASSWORD");

            services.AddDbContext<Training24Context>(optionsAction =>
                optionsAction.UseMySql(dbconnection));
            services.Configure<FilesSettings>(Configuration.GetSection("Buckets"));
            services.Configure<EmailSettings>(emailSettings =>
            {
                emailSettings.PrimaryDomain = Environment.GetEnvironmentVariable("PRIMARY_DOMAIN");
                emailSettings.PrimaryPort = Convert.ToInt32(Environment.GetEnvironmentVariable("PRIMARY_PORT"));
                emailSettings.UsernameEmail = Environment.GetEnvironmentVariable("USERNAME_EMAIL");
                emailSettings.UsernamePassword = Environment.GetEnvironmentVariable("USERNAME_PASSWORD");
            });
            services.Configure<ErpSettings>(erpSettings =>
            {
                erpSettings.Url = Environment.GetEnvironmentVariable("ERP_URL");
                erpSettings.Un = Environment.GetEnvironmentVariable("ERP_UN");
                erpSettings.Pw = Environment.GetEnvironmentVariable("ERP_PW");
            });
            services.Configure<Language>(language =>
            {
                language.lan = Environment.GetEnvironmentVariable("LANGUAGE");
            });
            services.AddScoped<ProfileBusiness>();
            services.AddScoped<PasswordBusiness>();
            services.AddScoped<UsersBusiness>();
            services.AddScoped<EFUsersRepository>();
            services.AddScoped<User>();
            services.AddScoped<EFGenericRepository<User>>();

            services.AddScoped<BundleBusiness>();
            services.AddScoped<EFBundleRepository>();
            services.AddScoped<Bundle>();
            services.AddScoped<EFGenericRepository<Bundle>>();

            services.AddScoped<DepositBusiness>();
            services.AddScoped<EFDeposit>();
            services.AddScoped<Deposit>();
            services.AddScoped<EFGenericRepository<Deposit>>();
            services.AddScoped<EFGenericRepository<UserRole>>();


            services.AddScoped<FeedBackTaskBusiness>();
            services.AddScoped<EFFeedBackTask>();
            services.AddScoped<FeedBackTask>();
            services.AddScoped<EFGenericRepository<FeedBackTask>>();

            services.AddScoped<EFFeedBackTaskStatus>();
            services.AddScoped<FeedBackTaskStatus>();
            services.AddScoped<EFGenericRepository<FeedBackTaskStatus>>();


            services.AddScoped<EFFeedBackTaskStatusOption>();
            services.AddScoped<FeedBackTaskStatusOption>();
            services.AddScoped<EFGenericRepository<FeedBackTaskStatusOption>>();

            services.AddScoped<LogsBusiness>();
            services.AddScoped<EFLogsRepository>();
            services.AddScoped<Logs>();
            services.AddScoped<EFGenericRepository<Logs>>();

            services.AddScoped<FeedbackStaffBusiness>();
            services.AddScoped<EFFeedbackStaffRepository>();
            services.AddScoped<FeedBackStaff>();
            services.AddScoped<EFGenericRepository<FeedBackStaff>>();

            services.AddScoped<FeedBackActivityBusiness>();
            services.AddScoped<EFFeedBackActivity>();
            services.AddScoped<FeedBackActivity>();
            services.AddScoped<EFGenericRepository<FeedBackActivity>>();

            services.AddScoped<ProgressBusiness>();
            services.AddScoped<EFStudentLessonProgressRepository>();
            services.AddScoped<EFStudentCourseProgressRepository>();
            services.AddScoped<EFStudentChapterProgressRepository>();
            services.AddScoped<EFStudentProgressRepository>();
            services.AddScoped<StudentLessonProgress>();
            services.AddScoped<StudentCourseProgress>();
            services.AddScoped<StudentChapterProgress>();
            services.AddScoped<StudentProgress>();
            services.AddScoped<EFGenericRepository<StudentLessonProgress>>();
            services.AddScoped<EFGenericRepository<StudentCourseProgress>>();
            services.AddScoped<EFGenericRepository<StudentChapterProgress>>();
            services.AddScoped<EFGenericRepository<StudentProgress>>();

            services.AddScoped<CourseBusiness>();
            services.AddScoped<EFCourseRepository>();
            services.AddScoped<Course>();
            services.AddScoped<EFGenericRepository<Course>>();

            services.AddScoped<DefaultValuesBusiness>();
            services.AddScoped<EFDefaultValues>();
            services.AddScoped<DefaultValues>();
            services.AddScoped<EFGenericRepository<DefaultValues>>();

            services.AddScoped<RoleBusiness>();
            services.AddScoped<EFRolesRepository>();
            services.AddScoped<Role>();
            services.AddScoped<EFGenericRepository<Role>>();

            services.AddScoped<GradeBusiness>();
            services.AddScoped<EFGradeRepository>();
            services.AddScoped<Grade>();
            services.AddScoped<EFGenericRepository<Grade>>();

            services.AddScoped<SchoolBusiness>();
            services.AddScoped<EFSchoolRepository>();
            services.AddScoped<School>();
            services.AddScoped<EFGenericRepository<School>>();

            services.AddScoped<CourseGradeBusiness>();
            services.AddScoped<EFCourseGradeRepository>();
            services.AddScoped<CourseGrade>();
            services.AddScoped<EFGenericRepository<CourseGrade>>();

            services.AddScoped<RoleBusiness>();
            services.AddScoped<EFRolesRepository>();
            services.AddScoped<Role>();
            services.AddScoped<EFGenericRepository<Role>>();

            services.AddScoped<QuizBusiness>();
            services.AddScoped<EFQuizRepository>();
            services.AddScoped<Quiz>();
            services.AddScoped<QuizSummary>();
            services.AddScoped<EFGenericRepository<Quiz>>();
            services.AddScoped<EFGenericRepository<QuizSummary>>();

            services.AddScoped<QuizQuestionBusiness>();
            services.AddScoped<EFQuizQuestionRepository>();
            services.AddScoped<QuizQuestion>();
            services.AddScoped<EFGenericRepository<QuizQuestion>>();

            services.AddScoped<CourseItemProgressSyncBusiness>();
            services.AddScoped<EFCourseItemProgressSync>();
            services.AddScoped<CourseItemProgressSync>();
            services.AddScoped<EFGenericRepository<CourseItemProgressSync>>();

            services.AddScoped<QuestionTypeBusiness>();
            services.AddScoped<EFQuestionTypeRepository>();
            services.AddScoped<QuestionType>();
            services.AddScoped<EFGenericRepository<QuestionType>>();

            services.AddScoped<ChapterBusiness>();
            services.AddScoped<EFChapterRepository>();
            services.AddScoped<Chapter>();
            services.AddScoped<EFGenericRepository<Chapter>>();

            services.AddScoped<BundleBusiness>();
            services.AddScoped<EFBundleCourseCourseRepository>();
            services.AddScoped<BundleCourse>();
            services.AddScoped<EFGenericRepository<BundleCourse>>();

            services.AddScoped<QuestionBusiness>();
            services.AddScoped<EFQuestionRepository>();
            services.AddScoped<Question>();
            services.AddScoped<EFGenericRepository<Question>>();

            services.AddScoped<QuestionAnswerBusiness>();
            services.AddScoped<EFQuestionAnswerRepository>();
            services.AddScoped<QuestionAnswer>();
            services.AddScoped<EFGenericRepository<QuestionAnswer>>();
            services.AddScoped<EFGenericRepository<QuestionFile>>();
            services.AddScoped<EFGenericRepository<AnswerFile>>();

            services.AddScoped<FilesBusiness>();
            services.AddScoped<EFFilesRepository>();
            services.AddScoped<Files>();
            services.AddScoped<EFGenericRepository<Files>>();
            services.AddScoped<EFGenericRepository<FileTypes>>();

            services.AddScoped<StudentCourseBusiness>();
            services.AddScoped<EFStudentCourseRepository>();
            services.AddScoped<UserCourse>();
            services.AddScoped<EFGenericRepository<UserCourse>>();

            services.AddScoped<LessonFile>();
            services.AddScoped<EFGenericRepository<LessonFile>>();
            services.AddScoped<LessonBusiness>();
            services.AddScoped<EFLessonRepository>();
            services.AddScoped<Lesson>();
            services.AddScoped<EFGenericRepository<Lesson>>();

            services.AddScoped<AssignmentFile>();
            services.AddScoped<EFGenericRepository<AssignmentFile>>();
            services.AddScoped<AssignmentBusiness>();
            services.AddScoped<EFAssignmentRepository>();
            services.AddScoped<Assignment>();
            services.AddScoped<EFGenericRepository<Assignment>>();

            services.AddScoped<EFQuizQuestionRepository>();
            services.AddScoped<EFGenericRepository<QuizQuestion>>();

            services.AddScoped<UserSessionsBusiness>();
            services.AddScoped<EFUserSessionsRepository>();
            services.AddScoped<UserSessions>();
            services.AddScoped<EFGenericRepository<UserSessions>>();

            services.AddScoped<AssignmentBusiness>();
            services.AddScoped<EFAssignmentStudent>();
            services.AddScoped<AssignmentStudent>();
            services.AddScoped<EFGenericRepository<AssignmentStudent>>();

            services.AddScoped<FeedbackBusiness>();
            services.AddScoped<EFContact>();
            services.AddScoped<EFFeedbackTime>();
            services.AddScoped<EFFeedbackFile>();
            services.AddScoped<EFFeedback>();
            services.AddScoped<EFFeedBackCategory>();
            services.AddScoped<Trainning24.Domain.Entity.Contact>();
            services.AddScoped<Feedback>();
            services.AddScoped<FeedBackCategory>();
            services.AddScoped<FeebackTime>();
            services.AddScoped<FeedbackFile>();
            services.AddScoped<EFGenericRepository<Trainning24.Domain.Entity.Contact>>();
            services.AddScoped<EFGenericRepository<Feedback>>();
            services.AddScoped<EFGenericRepository<FeedBackCategory>>();
            services.AddScoped<EFGenericRepository<FeebackTime>>();
            services.AddScoped<EFGenericRepository<FeedbackFile>>();

            services.AddScoped<SalesAgentBusiness>();
            services.AddScoped<EFSalesAgentBusinessRepository>();
            services.AddScoped<SalesAgent>();
            services.AddScoped<EFGenericRepository<SalesAgent>>();
            services.AddScoped<EFGenericRepository<Currency>>();

            services.AddScoped<DiscountBusiness>();
            services.AddScoped<EFDiscountBusinessRepository>();
            services.AddScoped<Discount>();
            services.AddScoped<EFGenericRepository<Discount>>();

            services.AddScoped<CourseDefinationBusiness>();
            services.AddScoped<EFCourseDefinationRepository>();
            services.AddScoped<CourseDefination>();
            services.AddScoped<EFGenericRepository<CourseDefination>>();

            services.AddScoped<AgentCategoryBusiness>();
            services.AddScoped<EFAgentCategoryBusinessRepository>();
            services.AddScoped<AgentCategory>();
            services.AddScoped<EFGenericRepository<AgentCategory>>();

            services.AddScoped<SubscriptionMetadataBusiness>();
            services.AddScoped<EFSubscriptionMetadata>();
            services.AddScoped<SubscriptionMetadata>();
            services.AddScoped<EFGenericRepository<SubscriptionMetadata>>();

            services.AddScoped<SubscriptionsBusiness>();
            services.AddScoped<EFSubscriptions>();
            services.AddScoped<Subscriptions>();
            services.AddScoped<EFGenericRepository<Subscriptions>>();

            services.AddScoped<IndividualDetailsBusiness>();
            services.AddScoped<EFIndividualDetails>();
            services.AddScoped<IndividualDetails>();
            services.AddScoped<PurchagePdf>();
            services.AddScoped<PurchageUpload>();
            services.AddScoped<SchoolDetails>();
            services.AddScoped<DocumentDetails>();
            services.AddScoped<MetaDataDetail>();
            services.AddScoped<EFGenericRepository<IndividualDetails>>();
            services.AddScoped<EFGenericRepository<PurchagePdf>>();
            services.AddScoped<EFGenericRepository<PurchageUpload>>();
            services.AddScoped<EFGenericRepository<DocumentDetails>>();
            services.AddScoped<EFGenericRepository<SchoolDetails>>();
            services.AddScoped<EFGenericRepository<MetaDataDetail>>();

            services.AddScoped<ProgessSyncBusiness>();
            services.AddScoped<EFProgressSync>();
            services.AddScoped<EFQuizTimerSync>();
            services.AddScoped<ProgessSync>();
            services.AddScoped<QuizTimerSync>();
            services.AddScoped<EFGenericRepository<ProgessSync>>();
            services.AddScoped<EFGenericRepository<QuizTimerSync>>();

            services.AddScoped<ChapterQuizBusiness>();
            services.AddScoped<EFChapterQuizRepository>();
            services.AddScoped<ChapterQuiz>();
            services.AddScoped<EFGenericRepository<ChapterQuiz>>();

            services.AddScoped<ManagementInfoBusiness>();
            services.AddScoped<EFManagementInfoRepository>();
            services.AddScoped<ManagementInfo>();
            services.AddScoped<EFGenericRepository<ManagementInfo>>();

            services.AddScoped<DiscussionTopicBusiness>();
            services.AddScoped<EFDiscussionTopicRepository>();
            services.AddScoped<DiscussionTopic>();
            services.AddScoped<EFGenericRepository<DiscussionTopic>>();

            services.AddScoped<DiscussionCommentsBusiness>();
            services.AddScoped<EFDiscussionCommentsRepository>();
            services.AddScoped<DiscussionComments>();
            services.AddScoped<EFGenericRepository<DiscussionComments>>();

            services.AddScoped<DiscussionFilesBusiness>();
            services.AddScoped<EFDiscussionFilesRepository>();
            services.AddScoped<DiscussionFiles>();
            services.AddScoped<EFGenericRepository<DiscussionFiles>>();

            services.AddScoped<DiscussionCommentFilesBusiness>();
            services.AddScoped<EFDiscussionCommentFilesRepository>();
            services.AddScoped<DiscussionCommentFiles>();
            services.AddScoped<EFGenericRepository<DiscussionCommentFiles>>();

            services.AddScoped<UserNotificationBusiness>();
            services.AddScoped<EFUserNotificationRepository>();
            services.AddScoped<UserNotifications>();
            services.AddScoped<EFGenericRepository<UserNotifications>>();
            services.AddScoped<NotificationBusiness>();
            services.AddScoped<NotificationThreadsBusiness>();

            services.AddScoped<LogObjectBusiness>();
            services.AddScoped<EFLogObjectRepository>();
            services.AddScoped<LogObject>();
            services.AddScoped<EFGenericRepository<LogObject>>();

            services.AddScoped<NotificationLogBusiness>();
            services.AddScoped<EFNotificationLogRepository>();
            services.AddScoped<NotificationLog>();
            services.AddScoped<EFGenericRepository<NotificationLog>>();

            services.AddScoped<AddtionalServicesBusiness>();
            services.AddScoped<EFAddtionalServicesRepository>();
            services.AddScoped<AddtionalServices>();
            services.AddScoped<EFGenericRepository<AddtionalServices>>();

            services.AddScoped<PackageBusiness>();
            services.AddScoped<EFPackageRepository>();
            services.AddScoped<Package>();
            services.AddScoped<EFGenericRepository<Package>>();

            services.AddScoped<PackageCourseBusiness>();
            services.AddScoped<EFPackageCourseRepository>();
            services.AddScoped<PackageCourse>();
            services.AddScoped<EFGenericRepository<PackageCourse>>();

            services.AddScoped<LibraryBusiness>();
            services.AddScoped<EFLibraryRepository>();
            services.AddScoped<Books>();
            services.AddScoped<EFGenericRepository<Books>>();

            services.AddScoped<TimeIntervalBusiness>();
            services.AddScoped<EFTimeIntervalRepository>();
            services.AddScoped<TimeInterval>();
            services.AddScoped<EFGenericRepository<TimeInterval>>();

            services.AddScoped<SalesTaxBusiness>();
            services.AddScoped<EFSalesTaxRepository>();
            services.AddScoped<SalesTax>();
            services.AddScoped<EFGenericRepository<SalesTax>>();

            services.AddScoped<ErpAccountsBusiness>();
            services.AddScoped<EFErpAccountsRepository>();
            services.AddScoped<ERPAccounts>();
            services.AddScoped<EFGenericRepository<ERPAccounts>>();

            services.AddScoped<LessonAssignmentFile>();
            services.AddScoped<EFGenericRepository<LessonAssignmentFile>>();
            services.AddScoped<LessonAssignmentBusiness>();
            services.AddScoped<EFLessonAssignmentRepository>();
            services.AddScoped<LessonAssignment>();
            services.AddScoped<EFGenericRepository<LessonAssignment>>();

            services.AddScoped<AssignmentSubmissionFile>();
            services.AddScoped<EFGenericRepository<AssignmentSubmissionFile>>();
            services.AddScoped<AssignmentSubmissionBusinees>();
            services.AddScoped<EFAssignmentSubmissionRepository>();
            services.AddScoped<AssignmentSubmission>();
            services.AddScoped<EFGenericRepository<AssignmentSubmission>>();

            services.AddScoped<LessonAssignmentSubmissionFile>();
            services.AddScoped<EFGenericRepository<LessonAssignmentSubmissionFile>>();
            services.AddScoped<LessonAssignmentSubmissionBusinees>();
            services.AddScoped<EFLessonAssignmentSubmissionRepository>();
            services.AddScoped<LessonAssignmentSubmission>();
            services.AddScoped<EFGenericRepository<LessonAssignmentSubmission>>();

            services.AddScoped<AppTimeTrackBusiness>();
            services.AddScoped<EFAppTimeTrackSync>();
            services.AddScoped<AppTimeTrack>();
            services.AddScoped<EFGenericRepository<AppTimeTrack>>();

            services.AddScoped<ChapterProgressBusiness>();
            services.AddScoped<EFChapterProgressSync>();
            services.AddScoped<ChapterProgress>();
            services.AddScoped<EFGenericRepository<ChapterProgress>>();

            services.AddScoped<LessonProgressBusiness>();
            services.AddScoped<EFLessonProgressSync>();
            services.AddScoped<LessonProgress>();
            services.AddScoped<EFGenericRepository<LessonProgress>>();

            services.AddScoped<QuizProgressBusiness>();
            services.AddScoped<EFQuizProgressSync>();
            services.AddScoped<QuizProgress>();
            services.AddScoped<EFGenericRepository<QuizProgress>>();

            services.AddScoped<FileProgressBusiness>();
            services.AddScoped<EFFileProgressSync>();
            services.AddScoped<FileProgress>();
            services.AddScoped<EFGenericRepository<FileProgress>>();

            services.AddScoped<UserQuizResultBusiness>();
            services.AddScoped<EFUserQuizResultSync>();
            services.AddScoped<UserQuizResult>();
            services.AddScoped<EFGenericRepository<UserQuizResult>>();

            services.AddScoped<StudParentBusiness>();
            services.AddScoped<EFStudParentRepository>();
            services.AddScoped<StudParent>();
            services.AddScoped<EFGenericRepository<StudParent>>();

            services.AddScoped<DiscussionTopicLikeBusiness>();
            services.AddScoped<EFDiscussionTopicLikeRepository>();
            services.AddScoped<DiscussionTopicLikes>();
            services.AddScoped<EFGenericRepository<DiscussionTopicLikes>>();

            services.AddScoped<DiscussionCommentLikeBusiness>();
            services.AddScoped<EFDiscussionCommentLikeRepository>();
            services.AddScoped<DiscussionCommentLikes>();
            services.AddScoped<EFGenericRepository<DiscussionCommentLikes>>();

            services.AddScoped<TermsAndCondtionBusiness>();
            services.AddScoped<EFTermsAndConditionRepo>();
            services.AddScoped<TermsAndConditions>();
            services.AddScoped<EFGenericRepository<TermsAndConditions>>();

            services.AddScoped<TaskFeedbackBusiness>();
            services.AddScoped<EFTaskFeedBack>();
            services.AddScoped<TaskFeedBack>();
            services.AddScoped<EFGenericRepository<TaskFeedBack>>();

            services.AddScoped<EFTaskFileFeedback>();
            services.AddScoped<TaskFileFeedBack>();
            services.AddScoped<EFGenericRepository<TaskFileFeedBack>>();

            services.AddScoped<EFTaskActivityFeedBack>();
            services.AddScoped<TaskActivityFeedBack>();
            services.AddScoped<EFGenericRepository<TaskActivityFeedBack>>();

            services.AddScoped<EFTaskFeedBackCategory>();
            services.AddScoped<TaskCategoryFeedBack>();
            services.AddScoped<EFGenericRepository<TaskCategoryFeedBack>>();

            services.AddAutoMapper();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                  builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });



            //// this is used for live 
            string domainname = Environment.GetEnvironmentVariable("DOMAINNAME_ENVIRONMENT");
            //string domainname = "edgsolutions.eu.auth0.com";

            //// this is used for live 
            string domain = $"https://" + domainname + "/";
            string audience = $"https://" + domainname + "/api/v2/";


            //// this is used for local 
            //string domain = $"https://{Configuration["Auth0:Domain"]}/";
            //string audience = $"https://{Configuration["Auth0:Domain"]}/api/v2/";
            //// ===== Add Jwt Authentication ========
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = audience;
                options.RequireHttpsMetadata = false;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
            });

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddApiVersioning(o => o.ApiVersionReader = new HeaderApiVersionReader("api-version"));


            //var contextforpdf = new CustomAssemblyLoadContext();
            //contextforpdf.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            // Register the Swagger generator, defining one or more Swagger documents
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "Training24 Admin", Version = "v1" });
            //});

#if DEBUG
            // swagger configuration
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Noon Online Education v1", Version = "v1" });
                    // swagger tags config
                    c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
                    c.TagActionsBy(api => api.GroupName);

                    //// Swagger 2.+ support
                    //var security = new Dictionary<string, IEnumerable<string>>
                    //{
                    //    {"Bearer", new string[] { }},
                    //};

                    //c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    //{
                    //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    //    Name = "Authorization",
                    //    In = "header",
                    //    Type = "apiKey"
                    //});
                    //c.AddSecurityRequirement(security);


                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments = true);
                });
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory
            , FilesBusiness FilesBusiness
            )
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine);

            //Console.WriteLine(environment);

            string name = env.EnvironmentName = environment;

            UpdateDatabase(app); //2011

            this.FilesBusiness = FilesBusiness;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            //app.UseHttpsRedirection();
            //app.UseSwagger();

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Training24 Admin V1");
            //});

#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Noon Online Education API v1");
                c.DocumentTitle = "Noon Online Education";
                c.DocExpansion(DocExpansion.None);
                c.EnableFilter();
            });
#endif

            app.UseCors("AllowAnyOrigin");
            app.UseMvc();

            #region Front end config

            //Sales
            app.Map(new PathString("/web/sales"), client =>
            {
                string defaultSalesPath = env.IsDevelopment() ? "Sales-noon" : @"Sales-noon/dist";
                StaticFileOptions defaultSalesDist = new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), defaultSalesPath)
                        )
                };
                client.UseSpaStaticFiles(defaultSalesDist);

                client.UseSpa(spa =>
                {
                    spa.Options.StartupTimeout = new TimeSpan(0, 5, 0);
                    spa.Options.SourcePath = "Sales-noon";

                    if (env.IsDevelopment())
                    {
                        // it will use package.json & will search for start command to run
                        // spa.Options.DefaultPageStaticFileOptions = defaultSalesDist;
                        spa.UseAngularCliServer(npmScript: "start");
                    }
                    else
                    {
                        //spa.UseAngularCliServer(npmScript: "start");
                        spa.Options.DefaultPageStaticFileOptions = defaultSalesDist;
                    }
                });
            });

            //Admin
            app.Map(new PathString("/web/manage"), client =>
             {
                 string defaultAdminPath = env.IsDevelopment() ? "Admin-noon" : @"Admin-noon/dist";

                 // Each map gets its own physical path for it to map the static files to. 
                 StaticFileOptions defaultAdminDist = new StaticFileOptions()
                 {
                     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), defaultAdminPath))
                 };

                 // Each map its own static files otherwise it will only ever serve index.html no matter the filename 
                 client.UseSpaStaticFiles(defaultAdminDist);
                 client.UseSpa(spa =>
                 {
                     spa.Options.StartupTimeout = new TimeSpan(0, 5, 0);
                     spa.Options.SourcePath = "Admin-noon";
                     if (env.IsDevelopment())
                     {
                         // it will use package.json & will search for start command to run
                         // spa.Options.DefaultPageStaticFileOptions = defaultAdminDist;
                         spa.UseAngularCliServer(npmScript: "start");
                     }
                     else
                     {
                         //spa.UseAngularCliServer(npmScript: "start");
                         spa.Options.DefaultPageStaticFileOptions = defaultAdminDist;
                     }
                 });
             });

            ////Feedback
            app.Map(new PathString("/web/feedback"), feedback =>
                {
                    string defaultFeedbackPath = env.IsDevelopment() ? "Feedback-noon" : @"Feedback-noon/dist";

                    StaticFileOptions defaultFeedbackDist = new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), defaultFeedbackPath)
                        )
                    };
                    feedback.UseSpaStaticFiles(defaultFeedbackDist);

                    feedback.UseSpa(spa =>
                    {
                        spa.Options.StartupTimeout = new TimeSpan(0, 5, 0);
                        spa.Options.SourcePath = "Feedback-noon";

                        if (env.IsDevelopment())
                        {
                            // it will use package.json & will search for start command to run
                            // spa.Options.DefaultPageStaticFileOptions = defaultFeedbackDist;
                            spa.UseAngularCliServer(npmScript: "start");
                        }
                        else
                        {
                            //spa.UseAngularCliServer(npmScript: "start");
                            spa.Options.DefaultPageStaticFileOptions = defaultFeedbackDist;
                        }
                    });
                });
            #endregion
        }

        //2011
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<Training24Context>())
                {
                    context.Database.Migrate();
                }
            }
        }

    }
}
