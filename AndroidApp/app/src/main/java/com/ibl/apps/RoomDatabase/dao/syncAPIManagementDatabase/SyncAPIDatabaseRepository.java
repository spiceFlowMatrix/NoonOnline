package com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase;

import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.noon.NoonApplication;

import java.util.List;

public class SyncAPIDatabaseRepository {
    private AppDatabase database;

    public SyncAPIDatabaseRepository() {
        database = AppDatabase.getAppDatabase(NoonApplication.getContext());
    }

    public void insertSyncData(SyncAPITable... syncAPITables) {
        database.syncDataDao().insertAll(syncAPITables);
    }

    public List<SyncAPITable> getSyncUserById(int userid) {
        return database.syncDataDao().getSyncUserById(userid);
    }

    public void deleteAll() {
        database.syncDataDao().deleteAll();
    }

    public int deleteById(int userid) {
        return database.syncDataDao().deleteById(userid);
    }

}
