package com.ibl.apps.RoomDatabase.entity;

import androidx.room.Entity;
import androidx.room.PrimaryKey;

/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Entity
public class LessonProgress {

    @PrimaryKey(autoGenerate = true)
    private int lessonProgressId;

    private String lessonId;

    private String lessonProgress;

    private String gradeId;

    private String userId;

    private String totalRecords;

    private String quizId;

    private boolean isStatus;

    private String fileId;

    public String getFileId() {
        return fileId;
    }

    public void setFileId(String fileId) {
        this.fileId = fileId;
    }

    public String getLessonId() {
        return lessonId;
    }

    public void setLessonId(String lessonId) {
        this.lessonId = lessonId;
    }

    public String getLessonProgress() {
        return lessonProgress;
    }

    public void setLessonProgress(String lessonProgress) {
        this.lessonProgress = lessonProgress;
    }

    public String getGradeId() {
        return gradeId;
    }

    public void setGradeId(String gradeId) {
        this.gradeId = gradeId;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getTotalRecords() {
        return totalRecords;
    }

    public void setTotalRecords(String totalRecords) {
        this.totalRecords = totalRecords;
    }

    public String getQuizId() {
        return quizId;
    }

    public void setQuizId(String quizId) {
        this.quizId = quizId;
    }

    public boolean isStatus() {
        return isStatus;
    }

    public void setStatus(boolean status) {
        isStatus = status;
    }

    public int getLessonProgressId() {
        return lessonProgressId;
    }

    public void setLessonProgressId(int lessonProgressId) {
        this.lessonProgressId = lessonProgressId;
    }

    @Override
    public String toString() {
        return "ClassPojo [lessonId = " + lessonId + ", lessonProgressId = " + lessonProgressId + ", lessonProgress = " + lessonProgress + ", gradeId = " + gradeId + ", userId = " + userId + ", totalRecords = " + totalRecords + ", quizId = " + quizId + ", isStatus = " + isStatus + "]";
    }
}
