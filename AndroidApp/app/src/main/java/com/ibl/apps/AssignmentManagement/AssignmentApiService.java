package com.ibl.apps.AssignmentManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.AssignmentDetailObject;
import com.ibl.apps.Model.AssignmentObject;
import com.ibl.apps.Model.AssignmentSubmission;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.UploadTopicFile;
import com.ibl.apps.Model.assignment.AssignmentData;
import com.ibl.apps.Model.assignment.CommentResponse;
import com.ibl.apps.Model.assignment.FileUploadResponse;
import com.ibl.apps.Model.assignment.StudentDetailData;

import java.util.ArrayList;

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

public interface AssignmentApiService {
    // Assignments List
    @GET("Assignment/AssignmentByCourseId/{id}")
    Single<AssignmentObject> fetchAssignments(@Path("id") String Id);

    //Assignment Student list for Lesson
    @GET("Lesson/GetStudentDetails")
    Single<RestResponse<ArrayList<StudentDetailData>>> assignmentStudentDetail(@Query("assignmentid") String assignmentid);

    //Assignment Student list
    @GET("Assignment/GetStudentDetails")
    Single<RestResponse<ArrayList<StudentDetailData>>> assignmentStudentDetailChapter(@Query("assignmentid") String assignmentid);

    // AddTopic Comment for Assignment
    @POST("Lesson/AddAssignmentComment")
    Single<AssignmentSubmission> assignmentAddComment(@Body JsonObject body);

    @POST("Assignment/AddAssignmentComment")
    Single<AssignmentSubmission> assignmentAddCommentChapter(@Body JsonObject body);

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

    // Assignments List
    @GET("Assignment/{id}")
    Single<AssignmentDetailObject> fetchAssignmentsDetails(@Path("id") String Id);

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
                                                                @Part("totalpages") RequestBody totalpages);    @POST("Lesson/AssginmentSubmission")
    Single<RestResponse<AssignmentData>> submitAssignmentLession(@Body JsonObject body);

    @POST("Assignment/AssginmentSubmission")
    Single<RestResponse<AssignmentData>> submitAssignment(@Body JsonObject body);

    @POST("Lesson/ApprovedSubmission")
    Single<RestResponse<AssignmentData>> addTeaherTopicSubmission(@Body JsonObject body);

    @POST("Assignment/ApprovedSubmission")
    Single<RestResponse<AssignmentData>> addTeaherTopicSubmissionChapter(@Body JsonObject body);

}
