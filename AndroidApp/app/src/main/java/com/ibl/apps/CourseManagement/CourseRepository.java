package com.ibl.apps.CourseManagement;

import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.IntervalObject;
import com.ibl.apps.Model.SearchObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class CourseRepository implements CourseApiService {
    private CourseApiService courseApiService;

    public CourseRepository() {
        courseApiService = ApiClient.getClient(NoonApplication.getContext()).create(CourseApiService.class);
    }

    @Override
    public Single<CourseObject> fetchCourseList(int pagenumber, int perpagerecord, String search, int gradeid) {
        return courseApiService.fetchCourseList(pagenumber, perpagerecord, search, gradeid);
    }

    @Override
    public Single<IntervalObject> fetchinterval() {
        return courseApiService.fetchinterval();
    }

    @Override
    public Single<SearchObject> SearchDetails(int pagenumber, int perpagerecord, String search, String filter, String bygrade) {
        return courseApiService.SearchDetails(pagenumber, perpagerecord, search, filter, bygrade);
    }
}
