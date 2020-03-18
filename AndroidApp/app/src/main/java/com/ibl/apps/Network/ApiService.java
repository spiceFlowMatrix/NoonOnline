package com.ibl.apps.Network;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.ibl.apps.Model.APICourseObject;
import com.ibl.apps.Model.AddComment;
import com.ibl.apps.Model.AddDiscussionTopic;
import com.ibl.apps.Model.AllGradeObject;
import com.ibl.apps.Model.AssignmentDetailObject;
import com.ibl.apps.Model.AssignmentObject;
import com.ibl.apps.Model.AssignmentSubmission;
import com.ibl.apps.Model.CheckForgetKey;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.DiscssionsAllTopics;
import com.ibl.apps.Model.DiscussionsDetails;
import com.ibl.apps.Model.GetAllComment;
import com.ibl.apps.Model.GradeSpinner;
import com.ibl.apps.Model.IntervalObject;
import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.Model.LocationData;
import com.ibl.apps.Model.LoginObject;
import com.ibl.apps.Model.NotificationObject;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.SearchObject;
import com.ibl.apps.Model.SignedUrlObject;
import com.ibl.apps.Model.StatisticsObject;
import com.ibl.apps.Model.SyncRecords;
import com.ibl.apps.Model.SyncTimeTracking;
import com.ibl.apps.Model.TemsCondition;
import com.ibl.apps.Model.TopicLike;
import com.ibl.apps.Model.TrileSignupObject;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UploadTopicFile;
import com.ibl.apps.Model.UserObject;
import com.ibl.apps.Model.assignment.AssignmentData;
import com.ibl.apps.Model.assignment.CommentResponse;
import com.ibl.apps.Model.assignment.FileUploadResponse;
import com.ibl.apps.Model.assignment.StudentDetailData;
import com.ibl.apps.Model.feedback.ChapterData;
import com.ibl.apps.Model.feedback.CourseSpinner;
import com.ibl.apps.Model.feedback.FeebBackTask;
import com.ibl.apps.Model.feedback.FeedBack;
import com.ibl.apps.Model.feedback.FeedBackTaskDetail;
import com.ibl.apps.Model.feedback.LessonData;
import com.ibl.apps.Model.feedback.UploadFeedBackFile;
import com.ibl.apps.Model.parent.Chart;
import com.ibl.apps.Model.parent.CourseSpinnerData;
import com.ibl.apps.Model.parent.GPAProgress;
import com.ibl.apps.Model.parent.LastOnline;
import com.ibl.apps.Model.parent.MChart;
import com.ibl.apps.Model.parent.ParentGraphPoints;
import com.ibl.apps.Model.parent.ParentSpinnerModel;
import com.ibl.apps.Model.parent.ProgressReport;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;

import java.util.ArrayList;

import io.reactivex.Observable;
import io.reactivex.Single;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Part;
import retrofit2.http.Path;
import retrofit2.http.Query;

/**
 * Created by ravi on 20/02/18.
 */

public interface ApiService {

    // Login user
    @POST("Account")
    Single<LoginObject> loginUser(@Body JsonObject body);

    // Signup user
    @POST("Users/SignupTrialUser")
    Single<TrileSignupObject> SignupTrialUser(@Body JsonObject body);

    // Signup user
    @POST("Users/Signup")
    Single<LoginObject> signUpUser(@Body JsonObject body);

    // Change Password
    @PUT("Password")
    Single<UserObject> changePassword(@Body JsonObject body);

    // UserData
    @GET("Users/{id}")
    Single<UserObject> fetchUser(@Path("id") String userId);

    // Update Profile
    @PUT("Profile")
    Single<UserObject> updateProfile(@Body JsonObject body);

    // Check Username is exiest or not
    @GET("Profile")
    Observable<String> userExiest(@Query("Username") String Username);

    // Logout User
    @GET("Account")
    Single<UserObject> logout();

    // Upload Profile picture of specific login user
    @Multipart
    @PUT("Upload/UploadProfilePicture")
    Call<UploadImageObject> uploadImage(@Part MultipartBody.Part image);

    // UserCourseData
    @GET("UserCourse/GetCoursesByUserId/{id}")
    Single<APICourseObject> fetchcourse(@Path("id") String userId,
                                        @Query("pagenumber") String pagenumber,
                                        @Query("perpagerecord") String perpagerecord);

    // QuizData
    @GET("Quiz/Quizpriview/{id}")
    Single<QuizMainObject> fetchQuizData(@Path("id") String Id);


    @GET("Course/CoursePriview/{id}/{studentid}")
    Single<CoursePriviewObject> fetchCoursePriview(@Path("id") String Id, @Path("studentid") String studentid);

    // CoursePriviewGradeWise
    @GET("Course/CoursePriviewGradeWise")
    Single<CourseObject> fetchCourseList(@Query("pagenumber") int pagenumber,
                                         @Query("perpagerecord") int perpagerecord,
                                         @Query("search") String search,
                                         @Query("gradeid") int gradeid);

    // StudentLessonProgress
    @POST("ProgessSync/ProgessSyncAdd")
    Single<LessonProgress> ProgessSyncAdd(@Body JsonObject body);

    // CoursePriviewGradeWise
    @GET("Files/GetSignedUrl")
    Single<SignedUrlObject> fetchSignedUrl(@Query("fileid") String fileid,
                                           @Query("lessionid") String lessionid);

    // Get Statistic
    @GET("Users/GetStatistic")
    Single<StatisticsObject> StatisticUser();

    // Get Search FillesData
    @GET("Course/GetAllDetails")
    Single<SearchObject> SearchDetails(@Query("pagenumber") int pagenumber,
                                       @Query("perpagerecord") int perpagerecord,
                                       @Query("search") String search,
                                       @Query(value = "filter", encoded = true) String filter,
                                       @Query("bygrade") String bygrade);

    // Grade List
    @GET("Grade")
    Single<AllGradeObject> AllGradeList();

    // Get All App FillesData
    @GET("ProgessSync/GetSyncRecords")
    Single<SyncRecords> GetSyncRecords();

    // Assignments List
    @GET("Assignment/AssignmentByCourseId/{id}")
    Single<AssignmentObject> fetchAssignments(@Path("id") String Id);

    // Assignments List
    @GET("Assignment/{id}")
    Single<AssignmentDetailObject> fetchAssignmentsDetails(@Path("id") String Id);

    //Assignment Student list for Lesson
    @GET("Lesson/GetStudentDetails")
    Single<RestResponse<ArrayList<StudentDetailData>>> assignmentStudentDetail(@Query("assignmentid") String assignmentid);

    //Assignment Student list
    @GET("Assignment/GetStudentDetails")
    Single<RestResponse<ArrayList<StudentDetailData>>> assignmentStudentDetailChapter(@Query("assignmentid") String assignmentid);

    @POST("Lesson/ApprovedSubmission")
    Single<RestResponse<AssignmentData>> addTeaherTopicSubmission(@Body JsonObject body);

    @POST("Assignment/ApprovedSubmission")
    Single<RestResponse<AssignmentData>> addTeaherTopicSubmissionChapter(@Body JsonObject body);

    // AddDiscussionTopic
    @POST("Discussion/AddDiscussionTopic")
    Single<AddDiscussionTopic> AddDiscussionTopic(@Body JsonObject body);

    // DiscssionsAllTopics List
    @GET("Discussion/GetAllTopics")
    Single<DiscssionsAllTopics> GetAllTopics(@Query("pagenumber") String pagenumber,
                                             @Query("perpagerecord") String perpagerecord,
                                             @Query("courseid") String courseid,
                                             @Query("ispublic") boolean ispublic);

    // Discussion Details
    @GET("Discussion/GetAllTopicsById")
    Single<DiscussionsDetails> DiscussionsDetails(@Query("topicId") String topicId);

    // UpdateDiscussionTopic
    @POST("Discussion/UpdateDiscussionTopic")
    Single<AddDiscussionTopic> UpdateDiscussionTopic(@Body JsonObject body);

    // DeleteTopic
    @DELETE("Discussion/DeleteTopic")
    Single<UploadImageObject> DeleteTopic(@Query("topicId") String topicId);

    // AddTopic Comment
    @POST("Discussion/AddComment")
    Single<AddComment> AddComment(@Body JsonObject body);

    // AddTopic Comment for Assignment
    @POST("Lesson/AddAssignmentComment")
    Single<AssignmentSubmission> assignmentAddComment(@Body JsonObject body);

    @POST("Assignment/AddAssignmentComment")
    Single<AssignmentSubmission> assignmentAddCommentChapter(@Body JsonObject body);

    @POST("Assignment/AssginmentSubmission")
    Single<RestResponse<AssignmentData>> submitAssignment(@Body JsonObject body);

    @POST("Lesson/AssginmentSubmission")
    Single<RestResponse<AssignmentData>> submitAssignmentLession(@Body JsonObject body);

    // Discussion Details && SubmissionAssignm,ent
    @GET("Discussion/GetComments")
    Single<GetAllComment> GetComments(@Query("pagenumber") String pagenumber,
                                      @Query("perpagerecord") String perpagerecord,
                                      @Query("topicid") String topicid);

    @GET("Lesson/GetSubmissionDetails")
    Single<RestResponse<CommentResponse>> getAssignmentComments(@Query("pagenumber") String pagenumber,
                                                                @Query("perpagerecord") String perpagerecord,
                                                                @Query("assignmentid") String topicid,
                                                                @Query("studentid") String studentid);

    @GET("Assignment/GetSubmissionDetails")
    Single<RestResponse<CommentResponse>> getAssignmentCommentsChapter(@Query("pagenumber") String pagenumber,
                                                                       @Query("perpagerecord") String perpagerecord,
                                                                       @Query("assignmentid") String topicid,
                                                                       @Query("studentid") String studentid);

    @Multipart
    @POST("Discussion/AddTopicFile")
    Call<UploadTopicFile> UploadTopicFile(@Part MultipartBody.Part image,
                                          @Part("fileTypeId") RequestBody fileTypeId,
                                          @Part("duration") RequestBody duration,
                                          @Part("filesize") RequestBody filesize);

    @Multipart
    @POST("Files")
    Call<RestResponse<FileUploadResponse>> uploadAssignmentFile(@Part MultipartBody.Part file,
                                                                @Part("fileTypeId") RequestBody fileTypeId,
                                                                @Part("duration") RequestBody duration,
                                                                @Part("filesize") RequestBody filesize,
                                                                @Part("totalpages") RequestBody totalpages);

    @Multipart
    @POST("Discussion/AddCommentFile")
    Call<UploadTopicFile> UploadAddCommentFile(@Part MultipartBody.Part image,
                                               @Part("fileTypeId") RequestBody fileTypeId,
                                               @Part("duration") RequestBody duration,
                                               @Part("filesize") RequestBody filesize);

    // ForgotPassword
    @GET("Password/ForgotPassword/{email}")
    Single<UploadImageObject> ForgotPasswordEmail(@Path("email") String email);

    // ForgotPassword
    @GET("Password/CheckForgetKey/{passcode}")
    Single<CheckForgetKey> CheckForgetKey(@Path("passcode") String passcode);

    // Change Password
    @POST("Password/UpdatePassword")
    Single<UploadImageObject> UpdatePassword(@Body JsonObject body);


    // Notification List
    @GET("Notification/GetAllNotifications")
    Single<NotificationObject> fetchNotification(@Query("pagenumber") String pagenumber,
                                                 @Query("perpagerecord") String perpagerecord);

    // Book List
    @GET("Library/GetBooksApp")
    Single<LibraryObject> fetchBookList(@Query("search") String search);

    // Book List
    @GET("Library/GetBooksGradeWiseApp")
    Single<LibraryGradeObject> fetchBooksGradevise(@Query("search") String search);

    @GET("TimeInterval/GetIntervalPub")
    Single<IntervalObject> fetchinterval();

    @GET("Grade/GetGradeList")
    Single<GradeSpinner> getSpinnerData();

    @POST("ProgessSync/AppTimeTrack")
    Single<SyncTimeTracking> getSyncTimeTracking(@Body JsonArray body);

    @POST("ChapterProgress/ChapterProgressSync")
    Single<RestResponse> getChapterProgressSync(@Body JsonArray body);

    @POST("LessonProgress/LessonProgressSync")
    Single<RestResponse> getLessonProgressSync(@Body JsonArray body);

    @POST("QuizProgress/QuizProgressSync")
    Single<RestResponse> getQuizProgressSync(@Body JsonArray body);

    @POST("FileProgress/FileProgressSync")
    Single<RestResponse> getFileProgressSync(@Body JsonArray body);

    @POST("UserQuizResult/UserQuizResultSync")
    Single<RestResponse> getUserQuizResultSync(@Body JsonArray body);

    @GET("ParentControl/GetStudentList/{parentid}")
    Single<ParentSpinnerModel> getUserSpinnerData(@Path("parentid") int parentid);

    @POST("ParentControl/GetTimeActivity")
    Single<Chart> getOverAllChartData(@Body JsonObject jsonObject);

    @POST("ParentControl/GetStudentCourse")
    Single<CourseSpinnerData> getCourseData(@Body JsonObject jsonObject);

    @POST("ParentControl/GetQuizActivityPoint")
    Single<Chart> getQuizChartData(@Body JsonObject jsonObject);

    @POST("ParentControl/GetOverAllQuizProgress")
    Single<ProgressReport> getQuizPieChartData(@Body JsonObject jsonObject);

    @POST("ParentControl/GetQuizProgress")
    Single<ProgressReport> getQuizProgress(@Body JsonObject jsonObject);

    @POST("ParentControl/GetAssignmentActivityPoint")
    Single<Chart> getAssignmentChartData(@Body JsonObject jsonObject);

    @POST("ParentControl/GetOverAllAssignmentProgress")
    Single<ProgressReport> getAssignmentPieChartData(@Body JsonObject jsonObject);

    @POST("ParentControl/GetAssignmentProgress")
    Single<ProgressReport> getAssignmentProgress(@Body JsonObject jsonObject);

    @POST("ParentControl/GetAllProgress")
    Single<ProgressReport> getOverAllProgress(@Body JsonObject jsonObject);

    @POST("ParentControl/GetCurrentGPA")
    Single<GPAProgress> getCurrentGPA(@Body JsonObject jsonObject);

    @POST("ParentControl/GetCurrentPoint")
    Single<MChart> getCurrentPoints(@Body JsonObject jsonObject);

    @POST("ParentControl/GetLastOnline")
    Single<LastOnline> getLastOnline(@Body JsonObject jsonObject);

    @POST("ParentControl/GetUsersLocations")
    Single<LocationData> getUsersLocation(@Body JsonObject jsonObject);

    @POST("Discussion/DiscussionTopicLike")
    Single<TopicLike> getDiscussionTopicLike(@Body JsonObject jsonObject);

    @POST("Discussion/DiscussionCommentLike")
    Single<TopicLike> getDiscussionCommentLike(@Body JsonObject jsonObject);

    @GET("Discussion/SearchDiscussionTopic")
    Single<DiscssionsAllTopics> getSearchDiscussionTopic(@Query("keyword") String keyword, @Query("courseid") int courseid, @Query("IsPrivate") boolean IsPrivate);

    @GET("Users/GetTerms")
    Single<TemsCondition> getTerms();


    @Multipart
    @POST("Feedback/AddFeedbackTimeApp")
    Call<UploadFeedBackFile> addFeedbackTimeApp(@Part MultipartBody.Part file,
                                                @Part("Description") RequestBody Description,
                                                @Part("Time") RequestBody Time,
                                                @Part("duration") RequestBody duration,
                                                @Part("FeedbackId") RequestBody FeedbackId);

    @POST("TaskFeedBack/AddFeedback")
    Single<FeedBack> getAddFeedbackApp(@Body JsonObject jsonObject);

    @GET("UserCourse/GetAllCourseByUser")
    Single<CourseSpinner> getAllCourseByUser();

    @GET("Chapter/GetChapterByCourse/{id}")
    Single<ChapterData> getChapterByCourse(@Path("id") int courseid);

    @GET("Lesson/GetLessonByCourse/{id}")
    Single<LessonData> getLessonByCourse(@Path("id") int courseid);


    @GET("TaskFeedBack/GetInQueueListApp")
    Single<FeebBackTask> getInQueueListApp(@Query("pagenumber") int pagenumber,
                                           @Query("perpagerecord") int perpagerecord);

    @GET("TaskFeedBack/GetInProcessListApp")
    Single<FeebBackTask> getProcessListApp(@Query("pagenumber") int pagenumber,
                                           @Query("perpagerecord") int perpagerecord);

    @GET("TaskFeedBack/GetCompletedListApp")
    Single<FeebBackTask> getCompletedListApp(@Query("pagenumber") int pagenumber,
                                             @Query("perpagerecord") int perpagerecord);


    @GET("ParentControl/ViewChildActivity")
    Single<ParentGraphPoints> getViewChildActivity(@Query("date") String date,
                                                   @Query("pagenumber") int pagenumber,
                                                   @Query("perpagerecord") int perpagerecord);

    @GET("TaskFeedBack/GetTaskFeedBackDetailsById")
    Single<FeedBackTaskDetail> getTaskFeedBackDetailsById(@Query("Id") Long feedbackid);
}
