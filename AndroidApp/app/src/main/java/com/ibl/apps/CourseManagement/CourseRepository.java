package com.ibl.apps.CourseManagement;

import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.IntervalObject;
import com.ibl.apps.Model.IntervalTableObject;
import com.ibl.apps.Model.SearchObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.RoomDatabase.dao.CourseDao;
import com.ibl.apps.RoomDatabase.dao.CourseDetailsDao;
import com.ibl.apps.RoomDatabase.dao.IntervalDao;
import com.ibl.apps.RoomDatabase.dao.LessonProgressDao;
import com.ibl.apps.RoomDatabase.dao.SyncTimeTrackingDao;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.CourseImageTable;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;
import com.ibl.apps.noon.NoonApplication;

import java.util.List;

import io.reactivex.Single;

public class CourseRepository implements CourseApiService, CourseDao, IntervalDao, LessonProgressDao, SyncTimeTrackingDao, CourseDetailsDao {
    private CourseApiService courseApiService;
    private CourseDao courseDao;
    private IntervalDao intervalDao;
    private LessonProgressDao lessonProgressDao;
    private SyncTimeTrackingDao syncTimeTrackingDao;
    private CourseDetailsDao courseDetailsDao;

    public CourseRepository() {
        courseApiService = ApiClient.getClient(NoonApplication.getContext()).create(CourseApiService.class);
        courseDao = AppDatabase.getAppDatabase(NoonApplication.getContext()).courseDao();
        intervalDao = AppDatabase.getAppDatabase(NoonApplication.getContext()).intervalDao();
        lessonProgressDao = AppDatabase.getAppDatabase(NoonApplication.getContext()).lessonProgressDao();
        syncTimeTrackingDao = AppDatabase.getAppDatabase(NoonApplication.getContext()).syncTimeTrackingDao();
        courseDetailsDao = AppDatabase.getAppDatabase(NoonApplication.getContext()).courseDetailsDao();
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
    public void insertAll(CourseObject... courseObjects) {
        courseDao.insertAll(courseObjects);
    }

    @Override
    public List<CourseObject> getAllCourse(String userId) {
        return courseDao.getAllCourse(userId);
    }

    @Override
    public CourseObject getAllCourseObject(String userId) {
        return courseDao.getAllCourseObject(userId);
    }

    @Override
    public void insertAll(CourseImageTable... courseImageTables) {
        courseDao.insertAll(courseImageTables);
    }

    @Override
    public byte[] getCourseImage(String userId, String gradeId) {
        return courseDao.getCourseImage(userId, gradeId);
    }

    @Override
    public List<CourseImageTable> getImage() {
        return courseDao.getImage();
    }

    @Override
    public void updateAll(List<CourseObject.Data> ListData, String userId) {
        courseDao.updateAll(ListData, userId);
    }

    @Override
    public void insertAll(IntervalTableObject... IntervalTableObject) {
        intervalDao.insertAll(IntervalTableObject);
    }

    @Override
    public IntervalTableObject getAllInterval() {
        return intervalDao.getAllInterval();
    }

    @Override
    public void deleteInterval() {
        intervalDao.deleteInterval();
    }

    @Override
    public void updateItem(String localinterval, int IntervalTableID) {
        intervalDao.updateItem(localinterval, IntervalTableID);
    }

    @Override
    public void updateINterval(String interval, int IntervalTableID) {
        intervalDao.updateINterval(interval, IntervalTableID);
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
    public int getItemgradeIdProgress(String gradeId, String lessonProgress, String userId) {
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

    @Override
    public void insertAll(SyncTimeTrackingObject... syncTimeTrackingObjects) {
        syncTimeTrackingDao.insertAll(syncTimeTrackingObjects);
    }

    @Override
    public SyncTimeTrackingObject getSyncTimeTracking() {
        return syncTimeTrackingDao.getSyncTimeTracking();
    }

    @Override
    public void updateSyncTimeTracking(SyncTimeTrackingObject... syncTimeTrackingObjects) {
        syncTimeTrackingDao.updateSyncTimeTracking(syncTimeTrackingObjects);
    }

    @Override
    public void deleteAll() {
        syncTimeTrackingDao.deleteAll();
    }

    @Override
    public SyncTimeTrackingObject getSyncTimeTrack(int userid) {
        return syncTimeTrackingDao.getSyncTimeTrack(userid);
    }

    @Override
    public void insertAll(CoursePriviewObject... coursePriviewObjects) {
        courseDetailsDao.insertAll(coursePriviewObjects);
    }

    @Override
    public CoursePriviewObject getAllCourseDetails(String GradeId, String userId) {
        return courseDetailsDao.getAllCourseDetails(GradeId, userId);
    }
}
