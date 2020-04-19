package com.ibl.apps.RoomDatabase.dao.libraryManagementDatabase;

import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.BookImageTable;
import com.ibl.apps.noon.NoonApplication;

import java.util.List;

public class LibraryDatabaseRepository {
    private AppDatabase database;

    public LibraryDatabaseRepository() {
        database = AppDatabase.getAppDatabase(NoonApplication.getContext());
    }

    public void insertLibraryData(LibraryObject... libraryObjects) {
        database.libraryDao().insertAll(libraryObjects);
    }

    public List<LibraryObject> getLibraryBookByUserId(String userId) {
        return database.libraryDao().getAlllibraryBook(userId);
    }

    public LibraryObject getAllLibraryObjectByUserId(String userId) {
        return database.libraryDao().getAllLibraryObject(userId);
    }

    public void insertBookImageTable(BookImageTable... bookImageTables) {
        database.libraryDao().insertAll(bookImageTables);
    }

    public byte[] getLibraryBookImage(String userId, String bookId) {
        return database.libraryDao().getBookImage(userId, bookId);
    }

    public void updateAll(List<LibraryObject.Data> ListData, String userId) {
        database.libraryDao().updateAll(ListData, userId);
    }

    public void insertLibraryGradeData(LibraryGradeObject... libraryGradeObjects) {
        database.libraryGradeDao().insertAll(libraryGradeObjects);
    }

    public List<LibraryGradeObject> getAllLibraryGradeBook(String userId) {
        return database.libraryGradeDao().getAlllibraryGradeBook(userId);
    }

    public LibraryGradeObject getAllLibraryGradeByUserId(String userId) {
        return database.libraryGradeDao().getAllLibraryGradeObject(userId);
    }

    public byte[] getLibraryGradeBookImage(String userId, String bookId) {
        return database.libraryGradeDao().getBookImage(userId, bookId);
    }


    public void updateLibraryGradeBook(List<LibraryGradeObject.Data> ListData, String userId) {
        database.libraryGradeDao().updateGradeBookAll(ListData, userId);
    }


}
