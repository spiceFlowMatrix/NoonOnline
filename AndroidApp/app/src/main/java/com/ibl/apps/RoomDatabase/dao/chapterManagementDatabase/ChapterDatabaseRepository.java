package com.ibl.apps.RoomDatabase.dao.chapterManagementDatabase;

import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.ChapterProgress;
import com.ibl.apps.noon.NoonApplication;

public class ChapterDatabaseRepository {
    private AppDatabase database;

    public ChapterDatabaseRepository() {
        database = AppDatabase.getAppDatabase(NoonApplication.getContext());
    }

    public void insertChapterProgressData(ChapterProgress... chapterProgresses) {
        database.chapterProgressDao().insertAll(chapterProgresses);
    }

    public ChapterProgress getChapterProgressData(String courseId, String chapterId) {
        return database.chapterProgressDao().getChapterProgress(courseId, chapterId);
    }

    public void updateChapterProgressData(ChapterProgress... chapterProgresses) {
        database.chapterProgressDao().updateChapterProgress(chapterProgresses);
    }

    public void deleteAllChapterProgress() {
        database.chapterProgressDao().deleteAll();
    }
}
