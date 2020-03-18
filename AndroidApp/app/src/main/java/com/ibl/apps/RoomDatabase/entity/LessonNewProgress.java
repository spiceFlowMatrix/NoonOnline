package com.ibl.apps.RoomDatabase.entity;

import android.arch.persistence.room.ColumnInfo;
import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

@Entity
public class LessonNewProgress {
    @PrimaryKey(autoGenerate = true)
    public int id ;

    @ColumnInfo(name = "ChapterId")
    public String ChapterId;

    @ColumnInfo(name = "LessonId")
    public String LessonId;

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

    public String getChapterId() {
        return ChapterId;
    }

    public void setChapterId(String chapterId) {
        ChapterId = chapterId;
    }

    public String getLessonId() {
        return LessonId;
    }

    public void setLessonId(String lessonId) {
        LessonId = lessonId;
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
