package com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.RoomDatabase.entity.LessonNewProgress;

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
