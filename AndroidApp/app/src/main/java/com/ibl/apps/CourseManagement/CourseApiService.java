package com.ibl.apps.CourseManagement;


import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.IntervalObject;
import com.ibl.apps.Model.NotificationCourseType1;
import com.ibl.apps.Model.SearchObject;

import io.reactivex.Single;
import retrofit2.http.GET;
import retrofit2.http.Query;

//Here is API endpoints
public interface CourseApiService {
    // CoursePriviewGradeWise
    @GET("Course/CoursePriviewGradeWise")
    Single<CourseObject> fetchCourseList(@Query("pagenumber") int pagenumber,
                                         @Query("perpagerecord") int perpagerecord,
                                         @Query("search") String search,
                                         @Query("gradeid") int gradeid);

    @GET("TimeInterval/GetIntervalPub")
    Single<IntervalObject> fetchinterval();

    // Get Search FillesData
    @GET("Course/GetAllDetails")
    Single<SearchObject> SearchDetails(@Query("pagenumber") int pagenumber,
                                       @Query("perpagerecord") int perpagerecord,
                                       @Query("search") String search,
                                       @Query(value = "filter", encoded = true) String filter,
                                       @Query("bygrade") String bygrade);

    @GET("Course/GetCourseStatusIsExpire")
    Single<NotificationCourseType1> GetCourseStatusIsExpire();
}
