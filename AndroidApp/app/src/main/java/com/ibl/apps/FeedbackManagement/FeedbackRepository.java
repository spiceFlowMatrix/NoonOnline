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
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;

public class FeedbackRepository implements FeedbackApiService {
    private FeedbackApiService feedbackApiService;

    public FeedbackRepository() {
        feedbackApiService = ApiClient.getClient(NoonApplication.getContext()).create(FeedbackApiService.class);
    }


    @Override
    public Single<FeedBack> getAddFeedbackApp(JsonObject jsonObject) {
        return feedbackApiService.getAddFeedbackApp(jsonObject);
    }

    @Override
    public Single<CourseSpinner> getAllCourseByUser() {
        return feedbackApiService.getAllCourseByUser();
    }

    @Override
    public Single<ChapterData> getChapterByCourse(int courseid) {
        return feedbackApiService.getChapterByCourse(courseid);
    }

    @Override
    public Single<LessonData> getLessonByCourse(int courseid) {
        return feedbackApiService.getLessonByCourse(courseid);
    }

    @Override
    public Single<FeebBackTask> getInQueueListApp(int pagenumber, int perpagerecord) {
        return feedbackApiService.getInQueueListApp(pagenumber, perpagerecord);
    }

    @Override
    public Single<FeebBackTask> getProcessListApp(int pagenumber, int perpagerecord) {
        return feedbackApiService.getProcessListApp(pagenumber, perpagerecord);
    }

    @Override
    public Single<FeebBackTask> getCompletedListApp(int pagenumber, int perpagerecord) {
        return feedbackApiService.getCompletedListApp(pagenumber, perpagerecord);
    }

    @Override
    public Single<FeedBackTaskDetail> getTaskFeedBackDetailsById(Long feedbackid) {
        return feedbackApiService.getTaskFeedBackDetailsById(feedbackid);
    }

    @Override
    public Call<RestResponse<FileUploadResponse>> uploadAssignmentFile(MultipartBody.Part file, RequestBody fileTypeId, RequestBody duration, RequestBody filesize, RequestBody totalpages) {
        return feedbackApiService.uploadAssignmentFile(file,fileTypeId,duration,filesize,totalpages);
    }
}
