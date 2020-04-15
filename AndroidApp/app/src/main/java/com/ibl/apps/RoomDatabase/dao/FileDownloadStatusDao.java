package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.RoomDatabase.entity.FileDownloadStatus;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface FileDownloadStatusDao {

    @Insert
    void insertAll(FileDownloadStatus... fileDownloadStatuses);

    @Query("SELECT * FROM FileDownloadStatus WHERE fileid = :fileid")
    FileDownloadStatus getItemFileDownloadStatus(String fileid);

    @Query("UPDATE FileDownloadStatus SET downloadStatus = :downloadStatus, progressval = :progressval  WHERE fileid = :fileid")
    void updateItemFileDownloadStatus(String fileid, String downloadStatus, int progressval);


}
