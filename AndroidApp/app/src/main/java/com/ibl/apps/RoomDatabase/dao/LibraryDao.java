package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.RoomDatabase.entity.BookImageTable;

import java.util.List;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface LibraryDao {

    @Insert
    void insertAll(LibraryObject... libraryObjects);

    @Query("SELECT * FROM LibraryObject WHERE userId = :userId")
    List<LibraryObject> getAlllibraryBook(String userId);

    @Query("SELECT * FROM LibraryObject WHERE userId = :userId")
    LibraryObject getAllLibraryObject(String userId);

    @Insert
    void insertAll(BookImageTable... bookImageTables);

    @Query("SELECT BookImage FROM BookImageTable WHERE userId = :userId AND bookId = :bookId")
    byte[] getBookImage(String userId, String bookId);

    @Query("UPDATE LibraryObject SET ListData = :ListData  WHERE userId = :userId")
    void updateAll(List<LibraryObject.Data> ListData, String userId);

}
