package com.ibl.apps.RoomDatabase.dao.quizManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.RoomDatabase.entity.QuizUserResult;

import java.util.List;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface QuizUserResultDao {

    @Insert
    void insertAll(QuizUserResult... quizUserResults);

    @Query("UPDATE QuizUserResult SET quizTime = :quizTime, yourScore = :yourScore, passingScore = :passingScore ,totalQuitions = :totalQuesions , totalAnswers = :totalAnswers , quizDate = :date WHERE quizId = :quizId AND userId = :userId")
    void updateQuizUserResult(String userId, String quizId, String quizTime, String yourScore, String passingScore, String totalQuesions, String totalAnswers, String date);

    @Query("SELECT * FROM QuizUserResult WHERE isStatus = :isStatus AND userId=:userId")
    List<QuizUserResult> getAllQuizuserResult(boolean isStatus, String userId);

    @Query("SELECT * FROM QuizUserResult WHERE quizId = :quizId AND userId = :userId")
    QuizUserResult getQuizuserResult(String userId, String quizId);

    @Query("UPDATE QuizUserResult SET isStatus = :isStatus  WHERE quizId = :quizId AND userId = :userId")
    void updatelQuizUserResultStatus(boolean isStatus, String quizId, String userId);
}
