package com.ibl.apps.RoomDatabase.dao.chapterManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.RoomDatabase.entity.ChapterProgress;

@Dao
public interface ChapterProgressDao {
    @Insert
    void insertAll(ChapterProgress... chapterProgresses);

    @Query("SELECT * FROM ChapterProgress WHERE CourseId = :courseId AND ChapterId = :chapterId")
    ChapterProgress getChapterProgress(String courseId, String chapterId);

    @Update
    void updateChapterProgress(ChapterProgress... chapterProgresses);

    @Query("DELETE FROM ChapterProgress")
    void deleteAll();
//    @Query("SELECT * FROM ChapterProgress")
//    ArrayList<ChapterProgress> getAll();
}
