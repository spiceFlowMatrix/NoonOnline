package com.ibl.apps.RoomDatabase.dao.quizManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.RoomDatabase.entity.QuizAnswerSelect;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface QuizAnswerDao {

    @Insert
    void insertAll(QuizMainObject... quizMainObjects);

    @Query("SELECT * FROM QuizMainObject WHERE userId = :userId AND newquizId = :newquizId")
    QuizMainObject getquizData(String userId, String newquizId);

    @Insert
    void insertAll(QuizAnswerSelect... quizAnswerSelects);

    @Query("SELECT * FROM QuizAnswerSelect WHERE nextID = :nextID")
    QuizAnswerSelect getItemAnswerSelect(String nextID);


    @Query("SELECT COUNT(*) FROM QuizAnswerSelect WHERE quizID = :quizID AND iscorrect = :iscorrect")
    int getCountTrueAnswerSelect(String quizID, String iscorrect);

    @Query("UPDATE QuizAnswerSelect SET selectedAnswerId = :selectedAnswerId  WHERE nextID = :nextID")
    void updateItemQuizAnswerSelect(String selectedAnswerId, String nextID);

    @Query("DELETE FROM QuizAnswerSelect")
    void deleteSelectedAnswer();

    @Query("SELECT COUNT(*) FROM QuizAnswerSelect WHERE selectedAnswerId = :selectedAnswerId AND questionID = :questionID AND iscorrect = :iscorrect")
    int getTrueAnswer(String selectedAnswerId, String questionID, String iscorrect);

}
