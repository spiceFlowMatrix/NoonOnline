package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;
import android.arch.persistence.room.Update;

import com.ibl.apps.RoomDatabase.entity.ChapterProgress;
import com.ibl.apps.RoomDatabase.entity.FileProgress;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;

import java.util.ArrayList;
import java.util.List;

@Dao
public interface FileProgressDao {
    @Insert
    void insertAll(FileProgress... fileProgresses);

    @Update
    void updateFileProgress(FileProgress... fileProgresses);

    @Query("SELECT * FROM FileProgress WHERE FileId = :fileId AND LessonId = :lessonId")
    FileProgress getFileProgress(String fileId, String lessonId);

    @Query("DELETE FROM FileProgress")
    void deleteAll();

//    @Query("SELECT * FROM FileProgress")
//    ArrayList<FileProgress> getAll();
}
