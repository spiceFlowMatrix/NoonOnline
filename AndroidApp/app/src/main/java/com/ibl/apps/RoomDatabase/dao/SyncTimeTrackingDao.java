package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;
import android.arch.persistence.room.Update;

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
