package com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase;

import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.FileDownloadStatus;
import com.ibl.apps.RoomDatabase.entity.FileProgress;
import com.ibl.apps.RoomDatabase.entity.LessonNewProgress;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.noon.NoonApplication;

import java.util.List;

public class LessonDatabaseRepository {
    private AppDatabase database;

    public LessonDatabaseRepository() {
        database = AppDatabase.getAppDatabase(NoonApplication.getContext());
    }

    public void insertLessonProgressData(LessonProgress... lessonProgresses) {
        database.lessonProgressDao().insertAll(lessonProgresses);
    }

    public LessonProgress getLessonProgressData() {
        return database.lessonProgressDao().getLessonProgress();
    }

    public int getItemGradeIdProgress(String gradeId, String lessonProgress, String userId) {
        return database.lessonProgressDao().getItemgradeIdProgress(gradeId, lessonProgress, userId);
    }

    public String getLessonStringProgress(String gradeId, String userId) {
        return database.lessonProgressDao().getStringProgress(gradeId, userId);
    }

    public LessonProgress getItemLessonProgressData(String lessonId, String fileId, String userId) {
        return database.lessonProgressDao().getItemLessonProgress(lessonId, fileId, userId);
    }

    public List<LessonProgress> getAllLessonProgressData(boolean isStatus, String userId) {
        return database.lessonProgressDao().getAllLessonProgress(isStatus, userId);
    }

    public List<LessonProgress> getAllLessonProgress1Data() {
        return database.lessonProgressDao().getAllLessonProgress1();
    }

    public LessonProgress getItemQuizIdProgress(String quizId, String userId) {
        return database.lessonProgressDao().getItemquizidProgress(quizId, userId);
    }

    public void updateItemLessonProgressData(String lessonId, String lessonProgress, boolean isStatus, String fileId, String userId) {
        database.lessonProgressDao().updateItemLessonProgress(lessonId, lessonProgress, isStatus, fileId, userId);
    }

    public void updateItemQuizIdProgress(String quizId, String lessonProgress, boolean isStatus, String userId) {
        database.lessonProgressDao().updateItemquizidProgress(quizId, lessonProgress, isStatus, userId);
    }

    public void updateLessonIdisStatus(String lessonId, boolean isStatus, String userId) {
        database.lessonProgressDao().updatelessonIdisStatus(lessonId, isStatus, userId);
    }

    public void updateQuizIdisStatus(String quizId, boolean isStatus, String userId) {
        database.lessonProgressDao().updatequizIdisStatus(quizId, isStatus, userId);
    }

    public LessonProgress getItemProgressData(String lessonProgressId, String userId) {
        return database.lessonProgressDao().getItemProgress(lessonProgressId, userId);
    }

    public void updateLessonUserIdWise(String lessonId, String lessonProgress, String gradeId, String userId, String totalRecords, String quizId, boolean isStatus, String fileId, String lessonProgressId) {
        database.lessonProgressDao().updateLessonIDWise(lessonId, lessonProgress, gradeId, userId, totalRecords, quizId, isStatus, fileId, lessonProgressId);
    }

    public void insertFileDownloadData(FileDownloadStatus... fileDownloadStatuses) {
        database.fileDownloadStatusDao().insertAll(fileDownloadStatuses);
    }

    public FileDownloadStatus getItemFileDownloadStatusData(String fileid) {
        return database.fileDownloadStatusDao().getItemFileDownloadStatus(fileid);
    }

    public void updateItemFileDownloadStatusData(String fileid, String downloadStatus, int progressval) {
        database.fileDownloadStatusDao().updateItemFileDownloadStatus(fileid, downloadStatus, progressval);
    }

    public void insertLessonNewProgress(LessonNewProgress... lessonNewProgresses) {
        database.lessonnewProgressDao().insertAll(lessonNewProgresses);
    }

    public LessonNewProgress getLessonProgressData(String lessonId, String chapterId) {
        return database.lessonnewProgressDao().getLessonProgress(lessonId, chapterId);
    }

    public void updateLessonProgressData(LessonNewProgress... lessonNewProgresses) {
        database.lessonnewProgressDao().updateLessonProgress(lessonNewProgresses);
    }

    public void insertFileProgressData(FileProgress... fileProgresses) {
        database.fileProgressDao().insertAll(fileProgresses);
    }

    public void updateFileProgressData(FileProgress... fileProgresses) {
        database.fileProgressDao().updateFileProgress(fileProgresses);
    }

    public FileProgress getFileProgressData(String fileId, String lessonId) {
        return database.fileProgressDao().getFileProgress(fileId, lessonId);
    }

    public void deleteAllData() {
        database.fileProgressDao().deleteAll();
    }
}
