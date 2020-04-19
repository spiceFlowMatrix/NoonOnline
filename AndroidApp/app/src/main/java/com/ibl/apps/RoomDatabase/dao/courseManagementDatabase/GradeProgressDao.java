package com.ibl.apps.RoomDatabase.dao.courseManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.RoomDatabase.entity.GradeProgress;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface GradeProgressDao {

    @Insert
    void insertAll(GradeProgress... gradeProgresses);

    @Query("SELECT * FROM GradeProgress WHERE gradeid = :gradeid")
    GradeProgress getItemGradeProgress(String gradeid);

    @Query("UPDATE GradeProgress SET gradeprogress = :gradeprogress  WHERE gradeid = :gradeid")
    void updateItemGradeProgress(String gradeid, String gradeprogress);

}
