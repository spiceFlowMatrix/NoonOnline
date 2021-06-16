package com.ibl.apps.ParentControlManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.LocationData;
import com.ibl.apps.Model.parent.Chart;
import com.ibl.apps.Model.parent.CourseSpinnerData;
import com.ibl.apps.Model.parent.GPAProgress;
import com.ibl.apps.Model.parent.LastOnline;
import com.ibl.apps.Model.parent.MChart;
import com.ibl.apps.Model.parent.ParentGraphPoints;
import com.ibl.apps.Model.parent.ParentSpinnerModel;
import com.ibl.apps.Model.parent.ProgressReport;

import io.reactivex.Single;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface ParentControlApiService {
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

    @GET("ParentControl/ViewChildActivity")
    Single<ParentGraphPoints> getViewChildActivity(@Query("date") String date,
                                                   @Query("pagenumber") int pagenumber,
                                                   @Query("perpagerecord") int perpagerecord);
}
