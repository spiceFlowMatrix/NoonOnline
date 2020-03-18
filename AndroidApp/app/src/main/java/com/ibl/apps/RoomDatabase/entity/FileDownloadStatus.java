package com.ibl.apps.RoomDatabase.entity;

import android.arch.persistence.room.ColumnInfo;
import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Entity
public class FileDownloadStatus {

    @PrimaryKey(autoGenerate = true)
    private int fileDownloadid;

    @ColumnInfo(name = "lessonid")
    private String lessonid;

    @ColumnInfo(name = "fileid")
    private String fileid;

    @ColumnInfo(name = "downloadID")
    private int downloadID;

    @ColumnInfo(name = "downloadStatus")
    private String downloadStatus;

    @ColumnInfo(name = "progressval")
    private String progressval;

    @ColumnInfo(name = "isStatus")
    private String isStatus;

    public String getProgressval() {
        return progressval;
    }

    public void setProgressval(String progressval) {
        this.progressval = progressval;
    }

    public int getDownloadID() {
        return downloadID;
    }

    public void setDownloadID(int downloadID) {
        this.downloadID = downloadID;
    }

    public int getFileDownloadid() {
        return fileDownloadid;
    }

    public void setFileDownloadid(int fileDownloadid) {
        this.fileDownloadid = fileDownloadid;
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

    public String getDownloadStatus() {
        return downloadStatus;
    }

    public void setDownloadStatus(String downloadStatus) {
        this.downloadStatus = downloadStatus;
    }

    public String getIsStatus() {
        return isStatus;
    }

    public void setIsStatus(String isStatus) {
        this.isStatus = isStatus;
    }

    @Override
    public String toString() {
        return "FileDownloadStatus{" +
                "fileDownloadid=" + fileDownloadid +
                ", lessonid='" + lessonid + '\'' +
                ", fileid='" + fileid + '\'' +
                ", downloadID=" + downloadID +
                ", downloadStatus='" + downloadStatus + '\'' +
                ", progressval='" + progressval + '\'' +
                ", isStatus='" + isStatus + '\'' +
                '}';
    }
}
