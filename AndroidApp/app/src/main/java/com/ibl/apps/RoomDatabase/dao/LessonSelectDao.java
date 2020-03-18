package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;

import com.ibl.apps.RoomDatabase.entity.LessonSelectedStatus;

import java.util.List;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface LessonSelectDao {

    @Insert
    void insertAll(LessonSelectedStatus... lessonSelectedStatus);

    @Query("SELECT * FROM LessonSelectedStatus")
    List<LessonSelectedStatus> getSelectedLesson();

    @Query("UPDATE LessonSelectedStatus SET lessonid = :lessonid,fileid = :fileid, chapterid = :chapterid, position = :position")
    void updateLesonSeleted(String lessonid, String fileid, String chapterid, String position);
}
