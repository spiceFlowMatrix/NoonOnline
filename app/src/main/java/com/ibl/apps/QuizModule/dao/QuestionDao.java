package com.ibl.apps.QuizModule.dao;

import androidx.room.Dao;
import androidx.room.Delete;
import androidx.room.Insert;
import androidx.room.OnConflictStrategy;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.QuizModule.entities.QuestionEntity;

import java.util.ArrayList;
import java.util.List;

@Dao
public interface QuestionDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    long insert(QuestionEntity questionEntity);

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    void insertMultiple(QuestionEntity... questionEntities);

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    void insertAll(List<QuestionEntity> questionEntities);

    @Query("SELECT * FROM questions WHERE quiz_id = :quizId")
    List<QuestionEntity> getAllQuestionByQuiz(long quizId);

    @Query("SELECT * FROM questions WHERE quiz_id = :quizId AND is_question_picked = 0")
    List<QuestionEntity> getUnpickedQuestionsByQuiz(long quizId);

    @Query("SELECT * FROM questions WHERE quiz_id = :quizId AND is_question_picked = 1")
    List<QuestionEntity> getPickedQuestionsByQuiz(long quizId);

    @Update
    void update(QuestionEntity questionEntity);

    @Update
    void updateAll(List<QuestionEntity> questionEntities);

    @Delete
    void delete(QuestionEntity questionEntity);

    @Query("DELETE FROM questions WHERE quiz_id = :quizId")
    void deleteByQuizId(long quizId);
}
