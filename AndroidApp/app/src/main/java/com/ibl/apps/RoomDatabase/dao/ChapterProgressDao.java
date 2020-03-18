package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;
import android.arch.persistence.room.Update;

import com.ibl.apps.RoomDatabase.entity.ChapterProgress;

import java.util.ArrayList;
import java.util.List;

import retrofit2.http.DELETE;

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
