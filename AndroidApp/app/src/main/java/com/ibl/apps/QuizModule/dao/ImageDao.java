package com.ibl.apps.QuizModule.dao;

import androidx.room.Dao;
import androidx.room.Delete;
import androidx.room.Insert;
import androidx.room.OnConflictStrategy;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.QuizModule.entities.AnswerEntity;
import com.ibl.apps.QuizModule.entities.ImageEntity;

import java.util.List;

@Dao
public interface ImageDao {

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    long insert(ImageEntity imageEntity);

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    void insertMultiple(ImageEntity... imageEntities);

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    void insertAll(List<ImageEntity> imageEntities);

    @Query("SELECT * FROM images WHERE question_id = :questionId")
    List<AnswerEntity> getAllImagesOfQuestion(long questionId);

    @Update
    void update(ImageEntity imageEntity);

    @Update
    void updateAll(List<ImageEntity> imageEntities);

    @Delete
    void delete(ImageEntity imageEntity);

    @Query("DELETE FROM images WHERE question_id = :questionId")
    void deleteByQuestionId(long questionId);
}
