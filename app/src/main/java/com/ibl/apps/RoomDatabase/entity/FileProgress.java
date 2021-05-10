package com.ibl.apps.RoomDatabase.entity;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;

@Entity
public class FileProgress {

    @PrimaryKey(autoGenerate = true)
    public int id ;

    @ColumnInfo(name = "LessonId")
    public String LessonId;

    @ColumnInfo(name = "FileId")
    public String FileId;

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

    public String getLessonId() {
        return LessonId;
    }

    public void setLessonId(String lessonId) {
        LessonId = lessonId;
    }

    public String getFileId() {
        return FileId;
    }

    public void setFileId(String fileId) {
        FileId = fileId;
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
