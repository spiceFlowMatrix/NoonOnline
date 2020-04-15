package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.Model.LibraryGradeObject;

import java.util.List;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface LibraryGradeDao {

    @Insert
    void insertAll(LibraryGradeObject... libraryGradeObjects);

    @Query("SELECT * FROM LibraryGradeObject WHERE userId = :userId")
    List<LibraryGradeObject> getAlllibraryGradeBook(String userId);

    @Query("SELECT * FROM LibraryGradeObject WHERE userId = :userId")
    LibraryGradeObject getAllLibraryGradeObject(String userId);

    @Query("SELECT BookImage FROM BookImageTable WHERE userId = :userId AND bookId = :bookId")
    byte[] getBookImage(String userId, String bookId);

    @Query("UPDATE LibraryGradeObject SET ListData = :ListData  WHERE userId = :userId")
    void updateGradeBookAll(List<LibraryGradeObject.Data> ListData, String userId);

}
