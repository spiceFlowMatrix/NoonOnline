package com.ibl.apps.RoomDatabase.entity;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;

@Entity
public class ChapterProgress {
    @PrimaryKey(autoGenerate = true)
    public int id;

    @ColumnInfo(name = "CourseId")
    public String CourseId;

    @ColumnInfo(name = "ChapterId")
    public String ChapterId;

    @ColumnInfo(name = "UserId")
    public String UserId;

    @ColumnInfo(name = "Progress")
    public String Progress;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getCourseId() {
        return CourseId;
    }

    public void setCourseId(String courseId) {
        CourseId = courseId;
    }

    public String getChapterId() {
        return ChapterId;
    }

    public void setChapterId(String chapterId) {
        ChapterId = chapterId;
    }

    public String getUserId() {
        return UserId;
    }

    public void setUserId(String userId) {
        UserId = userId;
    }

    public String getProgress() {
        return Progress;
    }

    public void setProgress(String progress) {
        Progress = progress;
    }
}
