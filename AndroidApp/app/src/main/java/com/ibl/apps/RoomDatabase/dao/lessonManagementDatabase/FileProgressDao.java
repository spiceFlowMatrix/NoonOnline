package com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.RoomDatabase.entity.FileProgress;

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
