package com.ibl.apps.LessonManament;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;

import io.reactivex.Single;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface LessonApiService {
    @GET("Course/CoursePriview/{id}/{studentid}")
    Single<CoursePriviewObject> fetchCoursePriview(@Path("id") String Id, @Path("studentid") String studentid);

    @POST("LessonProgress/LessonProgressSync")
    Single<RestResponse> getLessonProgressSync(@Body JsonArray body);

    @POST("ChapterProgress/ChapterProgressSync")
    Single<RestResponse> getChapterProgressSync(@Body JsonArray body);

    @POST("FileProgress/FileProgressSync")
    Single<RestResponse> getFileProgressSync(@Body JsonArray body);

    // StudentLessonProgress
    @POST("ProgessSync/ProgessSyncAdd")
    Single<LessonProgress> ProgessSyncAdd(@Body JsonObject body);
}
