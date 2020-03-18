package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;
import android.arch.persistence.room.Update;

import com.ibl.apps.RoomDatabase.entity.ChapterProgress;
import com.ibl.apps.RoomDatabase.entity.QuizProgress;

import java.util.ArrayList;

@Dao
public interface QuizProgressDao {
    @Insert
    void insertAll(QuizProgress... quizProgresses);

    @Query("SELECT * FROM QuizProgress WHERE QuizId = :quizId AND ChapterId = :chapterId")
    QuizProgress getQuizProgress(String quizId, String chapterId);

    @Query("Delete from QuizProgress")
    void deleteAll();

    @Update
    void updateQuizProgress(QuizProgress... quizProgresses);

//    @Query("SELECT * FROM QuizProgress")
//    ArrayList<QuizProgress> getAll();
}
