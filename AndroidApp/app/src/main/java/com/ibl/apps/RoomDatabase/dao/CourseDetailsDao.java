package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;

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
