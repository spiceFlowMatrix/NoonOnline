package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;

import com.ibl.apps.RoomDatabase.entity.LessonProgress;

import java.util.List;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface LessonProgressDao {

    @Insert()
    void insertAll(LessonProgress... lessonProgresses);

    @Query("SELECT * FROM LessonProgress")
    LessonProgress getLessonProgress();

    @Query("SELECT COUNT(*) FROM LessonProgress WHERE gradeId = :gradeId AND lessonProgress = :lessonProgress AND userId=:userId")
    int getItemgradeIdProgress(String gradeId, String lessonProgress, String userId);

    @Query("SELECT totalRecords FROM LessonProgress WHERE gradeId = :gradeId AND userId=:userId")
    String getStringProgress(String gradeId, String userId);

    @Query("SELECT * FROM LessonProgress WHERE lessonId = :lessonId AND fileId = :fileId AND userId=:userId")
    LessonProgress getItemLessonProgress(String lessonId, String fileId, String userId);

    @Query("SELECT * FROM LessonProgress WHERE isStatus = :isStatus AND userId=:userId")
    List<LessonProgress> getAllLessonProgress(boolean isStatus, String userId);

    @Query("SELECT * FROM LessonProgress")
    List<LessonProgress> getAllLessonProgress1();

    @Query("SELECT * FROM LessonProgress WHERE quizId = :quizId AND userId=:userId")
    LessonProgress getItemquizidProgress(String quizId, String userId);

    @Query("UPDATE LessonProgress SET lessonProgress = :lessonProgress,isStatus = :isStatus WHERE lessonId = :lessonId AND fileId = :fileId AND userId=:userId")
    void updateItemLessonProgress(String lessonId, String lessonProgress, boolean isStatus, String fileId, String userId);

    @Query("UPDATE LessonProgress SET lessonProgress = :lessonProgress,isStatus = :isStatus WHERE quizId = :quizId AND userId=:userId")
    void updateItemquizidProgress(String quizId, String lessonProgress, boolean isStatus, String userId);

    @Query("UPDATE LessonProgress SET isStatus = :isStatus  WHERE lessonId = :lessonId AND userId=:userId")
    void updatelessonIdisStatus(String lessonId, boolean isStatus, String userId);

    @Query("UPDATE LessonProgress SET isStatus = :isStatus  WHERE quizId = :quizId AND userId=:userId")
    void updatequizIdisStatus(String quizId, boolean isStatus, String userId);

    @Query("SELECT * FROM LessonProgress WHERE lessonProgressId = :lessonProgressId AND userId=:userId")
    LessonProgress getItemProgress(String lessonProgressId, String userId);

    @Query("UPDATE LessonProgress SET lessonId = :lessonId," +
            "lessonProgress =:lessonProgress," +
            "gradeId =:gradeId," +
            "userId =:userId," +
            "totalRecords =:totalRecords," +
            "quizId =:quizId," +
            "isStatus =:isStatus," +
            "fileId=:fileId " +
            "WHERE lessonProgressId = :lessonProgressId AND userId=:userId")
    void updateLessonIDWise(String lessonId, String lessonProgress, String gradeId, String userId, String totalRecords, String quizId, boolean isStatus, String fileId, String lessonProgressId);

}
