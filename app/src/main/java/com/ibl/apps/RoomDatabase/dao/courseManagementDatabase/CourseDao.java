package com.ibl.apps.RoomDatabase.dao.courseManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.RoomDatabase.entity.CourseImageTable;

import java.util.List;


/**
 * Created by lukegjpotter on 25/11/2017.
 */

//Here is DAO of course
@Dao
public interface CourseDao {

    @Insert
    void insertAll(CourseObject... courseObjects);

    @Query("SELECT * FROM CourseObject WHERE userId = :userId")
    List<CourseObject> getAllCourse(String userId);

    @Query("SELECT * FROM CourseObject WHERE userId = :userId")
    CourseObject getAllCourseObject(String userId);

    @Insert
    void insertAll(CourseImageTable... courseImageTables);

    @Query("SELECT courseImage FROM CourseImageTable WHERE userId = :userId AND gradeId = :gradeId")
    byte[] getCourseImage(String userId, String gradeId);

    @Query("SELECT * FROM CourseImageTable WHERE userId = :userId AND gradeId = :gradeId LIMIT 1")
    CourseImageTable getCourseImageByGradeUserId(String userId, String gradeId);

    @Query("SELECT * FROM CourseImageTable")
    List<CourseImageTable> getImage();

    @Query("UPDATE CourseObject SET ListData = :ListData  WHERE userId = :userId")
    void updateAll(List<CourseObject.Data> ListData, String userId);
}
