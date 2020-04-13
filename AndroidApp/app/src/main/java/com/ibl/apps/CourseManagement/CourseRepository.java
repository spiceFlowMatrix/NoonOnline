package com.ibl.apps.CourseManagement;

import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.IntervalObject;
import com.ibl.apps.Model.SearchObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.RoomDatabase.dao.LessonProgressDao;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.noon.NoonApplication;

import java.util.List;

import io.reactivex.Single;

public class CourseRepository implements CourseApiService, LessonProgressDao {
    private CourseApiService courseApiService;
    private LessonProgressDao lessonProgressDao;

    public CourseRepository() {
        courseApiService = ApiClient.getClient(NoonApplication.getContext()).create(CourseApiService.class);
        lessonProgressDao = AppDatabase.getAppDatabase(NoonApplication.getContext()).lessonProgressDao();
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

    @Override
    public void insertAll(LessonProgress... lessonProgresses) {
        lessonProgressDao.insertAll(lessonProgresses);
    }

    @Override
    public LessonProgress getLessonProgress() {
        return lessonProgressDao.getLessonProgress();
    }

    @Override
    public int  getItemgradeIdProgress(String gradeId, String lessonProgress, String userId) {
        return lessonProgressDao.getItemgradeIdProgress(gradeId, lessonProgress, userId);
    }

    @Override
    public String getStringProgress(String gradeId, String userId) {
        return lessonProgressDao.getStringProgress(gradeId, userId);
    }

    @Override
    public LessonProgress getItemLessonProgress(String lessonId, String fileId, String userId) {
        return lessonProgressDao.getItemLessonProgress(lessonId, fileId, userId);
    }

    @Override
    public List<LessonProgress> getAllLessonProgress(boolean isStatus, String userId) {
        return lessonProgressDao.getAllLessonProgress(isStatus, userId);
    }

    @Override
    public List<LessonProgress> getAllLessonProgress1() {
        return lessonProgressDao.getAllLessonProgress1();
    }

    @Override
    public LessonProgress getItemquizidProgress(String quizId, String userId) {
        return lessonProgressDao.getItemquizidProgress(quizId, userId);
    }

    @Override
    public void updateItemLessonProgress(String lessonId, String lessonProgress, boolean isStatus, String fileId, String userId) {
        lessonProgressDao.updateItemLessonProgress(lessonId, lessonProgress, isStatus, fileId, userId);
    }

    @Override
    public void updateItemquizidProgress(String quizId, String lessonProgress, boolean isStatus, String userId) {
        lessonProgressDao.updateItemquizidProgress(quizId, lessonProgress, isStatus, userId);
    }

    @Override
    public void updatelessonIdisStatus(String lessonId, boolean isStatus, String userId) {
        lessonProgressDao.updatelessonIdisStatus(lessonId, isStatus, userId);
    }

    @Override
    public void updatequizIdisStatus(String quizId, boolean isStatus, String userId) {
        lessonProgressDao.updatequizIdisStatus(quizId, isStatus, userId);
    }

    @Override
    public LessonProgress getItemProgress(String lessonProgressId, String userId) {
        return lessonProgressDao.getItemProgress(lessonProgressId, userId);
    }

    @Override
    public void updateLessonIDWise(String lessonId, String lessonProgress, String gradeId, String userId, String totalRecords, String quizId, boolean isStatus, String fileId, String lessonProgressId) {
        lessonProgressDao.updateLessonIDWise(lessonId, lessonProgress, gradeId, userId, totalRecords, quizId, isStatus, fileId, lessonProgressId);
    }

}
