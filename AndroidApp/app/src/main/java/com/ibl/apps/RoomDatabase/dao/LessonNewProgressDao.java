package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;
import android.arch.persistence.room.Update;

import com.ibl.apps.RoomDatabase.entity.ChapterProgress;
import com.ibl.apps.RoomDatabase.entity.LessonNewProgress;

import java.util.ArrayList;

@Dao
public interface LessonNewProgressDao {
    @Insert
    void insertAll(LessonNewProgress... lessonNewProgresses);

    @Query("SELECT * FROM LessonNewProgress WHERE LessonId = :lessonId AND ChapterId = :chapterId")
    LessonNewProgress getLessonProgress(String lessonId, String chapterId);

//    @Query("Delete from LessonNewProgress")
//    void deleteAll();

    @Update
    void updateLessonProgress(LessonNewProgress... lessonNewProgresses);

//    @Query("SELECT * FROM LessonNewProgress")
//    ArrayList<LessonNewProgress> getAll();
}
