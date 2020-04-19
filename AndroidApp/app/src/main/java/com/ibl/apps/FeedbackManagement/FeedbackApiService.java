package com.ibl.apps.FeedbackManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.assignment.FileUploadResponse;
import com.ibl.apps.Model.feedback.ChapterData;
import com.ibl.apps.Model.feedback.CourseSpinner;
import com.ibl.apps.Model.feedback.FeebBackTask;
import com.ibl.apps.Model.feedback.FeedBack;
import com.ibl.apps.Model.feedback.FeedBackTaskDetail;
import com.ibl.apps.Model.feedback.LessonData;

import io.reactivex.Single;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.Part;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface FeedbackApiService {
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

    @GET("TaskFeedBack/GetTaskFeedBackDetailsById")
    Single<FeedBackTaskDetail> getTaskFeedBackDetailsById(@Query("Id") Long feedbackid);

    @Multipart
    @POST("Files")
    Call<RestResponse<FileUploadResponse>> uploadAssignmentFile(@Part MultipartBody.Part file,
                                                                @Part("fileTypeId") RequestBody fileTypeId,
                                                                @Part("duration") RequestBody duration,
                                                                @Part("filesize") RequestBody filesize,
                                                                @Part("totalpages") RequestBody totalpages);
}
