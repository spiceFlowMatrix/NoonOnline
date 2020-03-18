package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;

import com.ibl.apps.RoomDatabase.entity.GradeProgress;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;

import java.util.List;


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
