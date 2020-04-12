package com.ibl.apps.DiscussionManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.AddComment;
import com.ibl.apps.Model.AddDiscussionTopic;
import com.ibl.apps.Model.DiscssionsAllTopics;
import com.ibl.apps.Model.DiscussionsDetails;
import com.ibl.apps.Model.GetAllComment;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.TopicLike;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UploadTopicFile;
import com.ibl.apps.Model.assignment.FileUploadResponse;

import io.reactivex.Single;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.Part;
import retrofit2.http.Query;

public interface DiscussionApiService {
    // DiscssionsAllTopics List
    @GET("Discussion/GetAllTopics")
    Single<DiscssionsAllTopics> GetAllTopics(@Query("pagenumber") String pagenumber,
                                             @Query("perpagerecord") String perpagerecord,
                                             @Query("courseid") String courseid,
                                             @Query("ispublic") boolean ispublic);

    @GET("Discussion/SearchDiscussionTopic")
    Single<DiscssionsAllTopics> getSearchDiscussionTopic(@Query("keyword") String keyword, @Query("courseid") int courseid, @Query("IsPrivate") boolean IsPrivate);

    @POST("Discussion/DiscussionTopicLike")
    Single<TopicLike> getDiscussionTopicLike(@Body JsonObject jsonObject);

    // AddTopic Comment
    @POST("Discussion/AddComment")
    Single<AddComment> AddComment(@Body JsonObject body);

    // Discussion Details
    @GET("Discussion/GetAllTopicsById")
    Single<DiscussionsDetails> DiscussionsDetails(@Query("topicId") String topicId);

    // Discussion Details && SubmissionAssignm,ent
    @GET("Discussion/GetComments")
    Single<GetAllComment> GetComments(@Query("pagenumber") String pagenumber,
                                      @Query("perpagerecord") String perpagerecord,
                                      @Query("topicid") String topicid);

    // UpdateDiscussionTopic
    @POST("Discussion/UpdateDiscussionTopic")
    Single<AddDiscussionTopic> UpdateDiscussionTopic(@Body JsonObject body);

    // DeleteTopic
    @DELETE("Discussion/DeleteTopic")
    Single<UploadImageObject> DeleteTopic(@Query("topicId") String topicId);

    @Multipart
    @POST("Discussion/AddTopicFile")
    Call<UploadTopicFile> UploadTopicFile(@Part MultipartBody.Part image,
                                          @Part("fileTypeId") RequestBody fileTypeId,
                                          @Part("duration") RequestBody duration,
                                          @Part("filesize") RequestBody filesize);

    @Multipart
    @POST("Discussion/AddCommentFile")
    Call<UploadTopicFile> UploadAddCommentFile(@Part MultipartBody.Part image,
                                               @Part("fileTypeId") RequestBody fileTypeId,
                                               @Part("duration") RequestBody duration,
                                               @Part("filesize") RequestBody filesize);

    // AddDiscussionTopic
    @POST("Discussion/AddDiscussionTopic")
    Single<AddDiscussionTopic> AddDiscussionTopic(@Body JsonObject body);

    @Multipart
    @POST("Files")
    Call<RestResponse<FileUploadResponse>> uploadAssignmentFile(@Part MultipartBody.Part file,
                                                                @Part("fileTypeId") RequestBody fileTypeId,
                                                                @Part("duration") RequestBody duration,
                                                                @Part("filesize") RequestBody filesize,
                                                                @Part("totalpages") RequestBody totalpages);

    @POST("Discussion/DiscussionCommentLike")
    Single<TopicLike> getDiscussionCommentLike(@Body JsonObject jsonObject);

}
