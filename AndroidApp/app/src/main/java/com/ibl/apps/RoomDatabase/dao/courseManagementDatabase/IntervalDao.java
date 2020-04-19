package com.ibl.apps.RoomDatabase.dao.courseManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.Model.IntervalTableObject;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface IntervalDao {

    @Insert
    void insertAll(IntervalTableObject... IntervalTableObject);

    @Query("SELECT * FROM IntervalTableObject")
    IntervalTableObject getAllInterval();

    @Query("DELETE FROM IntervalTableObject")
    void deleteInterval();

    @Query("UPDATE IntervalTableObject SET localinterval = :localinterval WHERE IntervalTableID = :IntervalTableID")
    void updateItem(String localinterval, int IntervalTableID);

    @Query("UPDATE IntervalTableObject SET interval = :interval WHERE IntervalTableID = :IntervalTableID")
    void updateINterval(String interval, int IntervalTableID);
}
