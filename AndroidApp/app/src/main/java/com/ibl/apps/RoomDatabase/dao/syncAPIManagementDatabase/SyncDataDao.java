package com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.RoomDatabase.entity.SyncAPITable;

import java.util.List;

@Dao
public interface SyncDataDao {
    @Query("SELECT * FROM SyncAPITable WHERE userid=:userid")
    List<SyncAPITable> getSyncUserById(int userid);

    @Insert
    void insertAll(SyncAPITable... syncAPITables);

    @Query("DELETE FROM SyncAPITable")
    void deleteAll();

    @Query("DELETE FROM SyncAPITable WHERE userid=:userid")
    int deleteById(int userid);
}
