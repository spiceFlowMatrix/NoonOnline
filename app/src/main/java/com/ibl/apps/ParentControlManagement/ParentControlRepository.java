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
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class ParentControlRepository implements ParentControlApiService {
    private ParentControlApiService parentControlApiService;

    public ParentControlRepository() {
        parentControlApiService = ApiClient.getClient(NoonApplication.getContext()).create(ParentControlApiService.class);
    }

    @Override
    public Single<ParentSpinnerModel> getUserSpinnerData(int parentid) {
        return parentControlApiService.getUserSpinnerData(parentid);
    }

    @Override
    public Single<Chart> getOverAllChartData(JsonObject jsonObject) {
        return parentControlApiService.getOverAllChartData(jsonObject);
    }

    @Override
    public Single<CourseSpinnerData> getCourseData(JsonObject jsonObject) {
        return parentControlApiService.getCourseData(jsonObject);
    }

    @Override
    public Single<Chart> getQuizChartData(JsonObject jsonObject) {
        return parentControlApiService.getQuizChartData(jsonObject);
    }

    @Override
    public Single<ProgressReport> getQuizPieChartData(JsonObject jsonObject) {
        return parentControlApiService.getQuizPieChartData(jsonObject);
    }

    @Override
    public Single<ProgressReport> getQuizProgress(JsonObject jsonObject) {
        return parentControlApiService.getQuizProgress(jsonObject);
    }

    @Override
    public Single<Chart> getAssignmentChartData(JsonObject jsonObject) {
        return parentControlApiService.getAssignmentChartData(jsonObject);
    }

    @Override
    public Single<ProgressReport> getAssignmentPieChartData(JsonObject jsonObject) {
        return parentControlApiService.getAssignmentPieChartData(jsonObject);
    }

    @Override
    public Single<ProgressReport> getAssignmentProgress(JsonObject jsonObject) {
        return parentControlApiService.getAssignmentProgress(jsonObject);
    }

    @Override
    public Single<ProgressReport> getOverAllProgress(JsonObject jsonObject) {
        return parentControlApiService.getOverAllProgress(jsonObject);
    }

    @Override
    public Single<GPAProgress> getCurrentGPA(JsonObject jsonObject) {
        return parentControlApiService.getCurrentGPA(jsonObject);
    }

    @Override
    public Single<MChart> getCurrentPoints(JsonObject jsonObject) {
        return parentControlApiService.getCurrentPoints(jsonObject);
    }

    @Override
    public Single<LastOnline> getLastOnline(JsonObject jsonObject) {
        return parentControlApiService.getLastOnline(jsonObject);
    }

    @Override
    public Single<LocationData> getUsersLocation(JsonObject jsonObject) {
        return parentControlApiService.getUsersLocation(jsonObject);
    }

    @Override
    public Single<ParentGraphPoints> getViewChildActivity(String date, int pagenumber, int perpagerecord) {
        return parentControlApiService.getViewChildActivity(date, pagenumber, perpagerecord);
    }
}
