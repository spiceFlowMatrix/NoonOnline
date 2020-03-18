package com.ibl.apps.RoomDatabase.entity;

import android.arch.persistence.room.ColumnInfo;
import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Entity
public class LessonSelectedStatus {

    @PrimaryKey(autoGenerate = true)
    private int lessonSelectid;

    @ColumnInfo(name = "lessonid")
    private String lessonid;

    @ColumnInfo(name = "fileid")
    private String fileid;

    @ColumnInfo(name = "chapterid")
    private String chapterId;

    @ColumnInfo(name = "position")
    private String position;


    public int getLessonSelectid() {
        return lessonSelectid;
    }

    public void setLessonSelectid(int lessonSelectid) {
        this.lessonSelectid = lessonSelectid;
    }

    public String getLessonid() {
        return lessonid;
    }

    public void setLessonid(String lessonid) {
        this.lessonid = lessonid;
    }

    public String getFileid() {
        return fileid;
    }

    public void setFileid(String fileid) {
        this.fileid = fileid;
    }

    public String getChapterId() {
        return chapterId;
    }

    public void setChapterId(String chapterId) {
        this.chapterId = chapterId;
    }

    public String getPosition() {
        return position;
    }

    public void setPosition(String position) {
        this.position = position;
    }

    @Override
    public String toString() {
        return "LessonSelectedStatus{" +
                "lessonSelectid=" + lessonSelectid +
                ", lessonid='" + lessonid + '\'' +
                ", fileid='" + fileid + '\'' +
                ", chapterId='" + chapterId + '\'' +
                ", position='" + position + '\'' +
                '}';
    }
}
