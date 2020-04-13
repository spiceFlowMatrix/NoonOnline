package com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase;

import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.noon.NoonApplication;

public class LessonDatabaseRepository {
    private AppDatabase database;

    public LessonDatabaseRepository() {
        database = AppDatabase.getAppDatabase(NoonApplication.getContext());
    }
}
