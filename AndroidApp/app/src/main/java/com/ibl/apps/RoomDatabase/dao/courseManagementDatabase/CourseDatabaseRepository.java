package com.ibl.apps.RoomDatabase.dao.courseManagementDatabase;

import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.IntervalTableObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.CourseImageTable;
import com.ibl.apps.RoomDatabase.entity.GradeProgress;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;
import com.ibl.apps.noon.NoonApplication;

import java.util.List;

public class CourseDatabaseRepository {
    private AppDatabase database;

    public CourseDatabaseRepository() {
        database = AppDatabase.getAppDatabase(NoonApplication.getContext());
    }

    public void insertCourseObjectData(CourseObject... courseObjects) {
        database.courseDao().insertAll(courseObjects);
    }

    public List<CourseObject> getAllCourseByUserId(String userId) {
        return database.courseDao().getAllCourse(userId);
    }

    public CourseObject getAllCourseObject(String userId) {
        return database.courseDao().getAllCourseObject(userId);
    }

    public void insertCourseImageTable(CourseImageTable... courseImageTables) {
        database.courseDao().insertAll(courseImageTables);
    }

    public byte[] getCourseImage(String userId, String gradeId) {
        return database.courseDao().getCourseImage(userId, gradeId);
    }

    public List<CourseImageTable> getImage() {
        return database.courseDao().getImage();
    }

    public void updateAll(List<CourseObject.Data> ListData, String userId) {
        database.courseDao().updateAll(ListData, userId);
    }

    public void insertCoursePreviewObjectData(CoursePriviewObject... coursePriviewObjects) {
        database.courseDetailsDao().insertAll(coursePriviewObjects);
    }

    public CoursePriviewObject getAllCourseDetailsById(String GradeId, String userId) {
        return database.courseDetailsDao().getAllCourseDetails(GradeId, userId);
    }

    public void insertIntervalTableObject(IntervalTableObject... IntervalTableObject) {
        database.intervalDao().insertAll(IntervalTableObject);
    }

    public IntervalTableObject getAllInterval() {
        return database.intervalDao().getAllInterval();
    }

    public void deleteInterval() {
        database.intervalDao().deleteInterval();
    }

    public void updateItemIntervalId(String localinterval, int IntervalTableID) {
        database.intervalDao().updateItem(localinterval, IntervalTableID);
    }

    public void updateIntervalById(String interval, int IntervalTableID) {
        database.intervalDao().updateINterval(interval, IntervalTableID);
    }

    public void insertSyncTimeTrackingData(SyncTimeTrackingObject... syncTimeTrackingObjects) {
        database.syncTimeTrackingDao().insertAll(syncTimeTrackingObjects);
    }

    public SyncTimeTrackingObject getSyncTimeTracking() {
        return database.syncTimeTrackingDao().getSyncTimeTracking();
    }

    public void updateSyncTimeTracking(SyncTimeTrackingObject... syncTimeTrackingObjects) {
        database.syncTimeTrackingDao().updateSyncTimeTracking(syncTimeTrackingObjects);
    }

    public void deleteAll() {
        database.syncTimeTrackingDao().deleteAll();
    }

    public SyncTimeTrackingObject getSyncTimeTrackById(int userid) {
        return database.syncTimeTrackingDao().getSyncTimeTrack(userid);
    }

    public void insertGradeProgress(GradeProgress... gradeProgresses) {
        database.gradeProgressDao().insertAll(gradeProgresses);
    }

    public GradeProgress getItemGradeProgress(String gradeid) {
        return database.gradeProgressDao().getItemGradeProgress(gradeid);
    }

    public void updateItemGradeProgress(String gradeid, String gradeprogress) {
        database.gradeProgressDao().updateItemGradeProgress(gradeid, gradeprogress);
    }
}
