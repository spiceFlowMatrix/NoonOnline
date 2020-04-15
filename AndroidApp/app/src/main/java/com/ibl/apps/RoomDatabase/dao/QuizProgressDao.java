package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.RoomDatabase.entity.QuizProgress;

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
