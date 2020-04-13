package com.ibl.apps.LessonManagement;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class LessonRepository implements LessonApiService {
    private LessonApiService lessonApiService;

    public LessonRepository() {
        lessonApiService = ApiClient.getClient(NoonApplication.getContext()).create(LessonApiService.class);
    }

    @Override
    public Single<CoursePriviewObject> fetchCoursePriview(String Id, String studentid) {
        return lessonApiService.fetchCoursePriview(Id,studentid);
    }

    @Override
    public Single<RestResponse> getLessonProgressSync(JsonArray body) {
        return lessonApiService.getLessonProgressSync(body);
    }

    @Override
    public Single<RestResponse> getChapterProgressSync(JsonArray body) {
        return lessonApiService.getChapterProgressSync(body);
    }

    @Override
    public Single<RestResponse> getFileProgressSync(JsonArray body) {
        return lessonApiService.getFileProgressSync(body);
    }

    @Override
    public Single<LessonProgress> ProgessSyncAdd(JsonObject body) {
        return lessonApiService.ProgessSyncAdd(body);
    }
}
