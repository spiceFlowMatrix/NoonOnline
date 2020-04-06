package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.Model.CoursePriviewObject;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface CourseDetailsDao {

    @Insert
    void insertAll(CoursePriviewObject... coursePriviewObjects);

    @Query("SELECT * FROM CoursePriviewObject WHERE id =:GradeId AND userId=:userId")
    CoursePriviewObject getAllCourseDetails(String GradeId, String userId);
}
