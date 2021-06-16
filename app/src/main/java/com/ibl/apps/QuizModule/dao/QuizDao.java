package com.ibl.apps.QuizModule.dao;

import androidx.room.Dao;
import androidx.room.Delete;
import androidx.room.Insert;
import androidx.room.OnConflictStrategy;
import androidx.room.Query;
import androidx.room.Transaction;
import androidx.room.Update;

import com.ibl.apps.QuizModule.entities.QuizEntity;
import com.ibl.apps.QuizModule.entities.QuizWithQuestionEntity;

import java.util.List;

@Dao
public interface QuizDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    long insert(QuizEntity quizEntity);

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    void insertAll(QuizEntity... quizEntities);

    @Query("SELECT * FROM quizzes WHERE id = :id")
    QuizEntity getById(long id);

    @Query("SELECT * FROM quizzes")
    List<QuizEntity> getAll();

    @Transaction
    @Query("SELECT * FROM quizzes WHERE id=:id")
    QuizWithQuestionEntity getQuizWithQuestionAndAnswer(long id);

    @Update
    void update(QuizEntity quizEntity);

    @Update
    void updateAll(List<QuizEntity> quizEntities);

    @Delete
    void delete(QuizEntity quizEntity);
}
