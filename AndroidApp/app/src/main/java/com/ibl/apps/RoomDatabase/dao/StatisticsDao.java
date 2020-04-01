package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.Model.StatisticsObject;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface StatisticsDao {

    @Insert
    void insertAll(StatisticsObject... statisticsObjects);

    @Query("SELECT * FROM StatisticsObject WHERE userId=:userId")
    StatisticsObject getStatisticsObject(String userId);
}
