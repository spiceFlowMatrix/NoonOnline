package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.RoomDatabase.entity.NetworkStatus;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface NetworkstatusDao {

    @Insert
    void insertAll(NetworkStatus... networkStatuses);

    @Query("SELECT * FROM NetworkStatus")
    NetworkStatus getnetworkStatus();

    @Query("UPDATE NetworkStatus SET networkStatus = :networkStatus WHERE networkstatusid = :networkstatusid")
    void updateNetworkStatus(boolean networkStatus, int networkstatusid);
}
