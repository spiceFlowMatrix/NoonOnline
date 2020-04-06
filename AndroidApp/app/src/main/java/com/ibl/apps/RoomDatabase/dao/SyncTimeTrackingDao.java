package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;

@Dao
public interface SyncTimeTrackingDao {

    @Insert
    void insertAll(SyncTimeTrackingObject... syncTimeTrackingObjects);

    @Query("SELECT * FROM SyncTimeTrackingObject")
    SyncTimeTrackingObject getSyncTimeTracking();

    @Update
    void updateSyncTimeTracking(SyncTimeTrackingObject... syncTimeTrackingObjects);

    @Query("DELETE FROM SyncTimeTrackingObject")
    void deleteAll();

    @Query("SELECT * FROM SyncTimeTrackingObject WHERE userid=:userid")
    SyncTimeTrackingObject getSyncTimeTrack(int userid);
}
