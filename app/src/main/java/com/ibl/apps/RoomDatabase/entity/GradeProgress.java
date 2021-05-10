package com.ibl.apps.RoomDatabase.entity;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;

/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Entity
public class GradeProgress {

    @PrimaryKey(autoGenerate = true)
    private int gradeProgressid;

    @ColumnInfo(name = "userid")
    private String userid;

    @ColumnInfo(name = "gradeid")
    private String gradeid;

    @ColumnInfo(name = "gradeprogress")
    private String gradeprogress;

    @ColumnInfo(name = "isStatus")
    private String isStatus;

    public int getGradeProgressid() {
        return gradeProgressid;
    }

    public void setGradeProgressid(int gradeProgressid) {
        this.gradeProgressid = gradeProgressid;
    }

    public String getUserid() {
        return userid;
    }

    public void setUserid(String userid) {
        this.userid = userid;
    }

    public String getGradeid() {
        return gradeid;
    }

    public void setGradeid(String gradeid) {
        this.gradeid = gradeid;
    }

    public String getGradeprogress() {
        return gradeprogress;
    }

    public void setGradeprogress(String gradeprogress) {
        this.gradeprogress = gradeprogress;
    }

    public String getIsStatus() {
        return isStatus;
    }

    public void setIsStatus(String isStatus) {
        this.isStatus = isStatus;
    }


    @Override
    public String toString() {
        return "GradeProgress{" +
                "gradeProgressid=" + gradeProgressid +
                ", userid='" + userid + '\'' +
                ", gradeid='" + gradeid + '\'' +
                ", gradeprogress='" + gradeprogress + '\'' +
                ", isStatus='" + isStatus + '\'' +
                '}';
    }
}
