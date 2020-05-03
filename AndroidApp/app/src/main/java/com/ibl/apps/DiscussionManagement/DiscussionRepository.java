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
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;

public class DiscussionRepository implements DiscussionApiService {
    private DiscussionApiService discussionApiService;

    public DiscussionRepository() {
        discussionApiService = ApiClient.getClient(NoonApplication.getContext()).create(DiscussionApiService.class);
    }

    @Override
    public Single<DiscssionsAllTopics> GetAllTopics(String pagenumber, String perpagerecord, String courseid, boolean ispublic) {
        return discussionApiService.GetAllTopics(pagenumber, perpagerecord, courseid, ispublic);
    }

    @Override
    public Single<DiscssionsAllTopics> getSearchDiscussionTopic(String keyword, int courseid, boolean IsPrivate) {
        return discussionApiService.getSearchDiscussionTopic(keyword, courseid, IsPrivate);
    }

    @Override
    public Single<TopicLike> getDiscussionTopicLike(JsonObject jsonObject) {
        return discussionApiService.getDiscussionTopicLike(jsonObject);
    }

    @Override
    public Single<AddComment> AddComment(JsonObject body) {
        return discussionApiService.AddComment(body);
    }

    @Override
    public Single<DiscussionsDetails> DiscussionsDetails(String topicId) {
        return discussionApiService.DiscussionsDetails(topicId);
    }

    @Override
    public Single<GetAllComment> GetComments(String pagenumber, String perpagerecord, String topicid) {
        return discussionApiService.GetComments(pagenumber, perpagerecord, topicid);
    }

    @Override
    public Single<AddDiscussionTopic> UpdateDiscussionTopic(JsonObject body) {
        return discussionApiService.UpdateDiscussionTopic(body);
    }

    @Override
    public Single<UploadImageObject> DeleteTopic(String topicId) {
        return discussionApiService.DeleteTopic(topicId);
    }

    @Override
    public Call<UploadTopicFile> UploadTopicFile(MultipartBody.Part image, RequestBody fileTypeId, RequestBody duration, RequestBody filesize) {
        return discussionApiService.UploadTopicFile(image, fileTypeId, duration, filesize);
    }

    @Override
    public Call<UploadTopicFile> UploadAddCommentFile(MultipartBody.Part image, RequestBody fileTypeId, RequestBody duration, RequestBody filesize) {
        return discussionApiService.UploadAddCommentFile(image, fileTypeId, duration, filesize);
    }

    @Override
    public Single<AddDiscussionTopic> AddDiscussionTopic(JsonObject body) {
        return discussionApiService.AddDiscussionTopic(body);
    }

    @Override
    public Call<RestResponse<FileUploadResponse>> uploadAssignmentFile(MultipartBody.Part file, RequestBody fileTypeId, RequestBody duration, RequestBody filesize, RequestBody totalpages) {
        return discussionApiService.uploadAssignmentFile(file, fileTypeId, duration, filesize, totalpages);
    }

    @Override
    public Single<TopicLike> getDiscussionCommentLike(JsonObject jsonObject) {
        return discussionApiService.getDiscussionCommentLike(jsonObject);
    }
}
