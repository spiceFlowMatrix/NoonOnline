package com.ibl.apps.QuizModule.dao;

import androidx.room.Dao;
import androidx.room.Delete;
import androidx.room.Insert;
import androidx.room.OnConflictStrategy;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.QuizModule.entities.AnswerEntity;
import com.ibl.apps.QuizModule.entities.QuestionEntity;

import java.util.List;

@Dao
public interface AnswerDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    long insert(AnswerEntity answerEntity);

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    void insertMultiple(AnswerEntity... answerEntities);

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    void insertAll(List<AnswerEntity> answerEntities);

    @Query("SELECT * FROM answers WHERE question_id = :questionId")
    List<AnswerEntity> getAllAnswerByQuestion(long questionId);

    @Update
    void update(AnswerEntity answerEntity);

    @Update
    void updateAll(List<AnswerEntity> answerEntities);

    @Delete
    void delete(AnswerEntity answerEntity);

    @Query("DELETE FROM answers WHERE question_id = :questionId")
    void deleteByQuestionId(long questionId);
}
