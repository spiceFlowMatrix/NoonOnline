package com.ibl.apps.RoomDatabase.entity;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;

/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Entity
public class CourseImageTable {

    @PrimaryKey(autoGenerate = true)
    private int CourseImageId;

    private String gradeId;

    private String userId;

    @ColumnInfo(typeAffinity = ColumnInfo.BLOB)
    private byte[] courseImage;

    public int getCourseImageId() {
        return CourseImageId;
    }

    public void setCourseImageId(int courseImageId) {
        this.CourseImageId = courseImageId;
    }

    public String getGradeId() {
        return gradeId;
    }

    public void setGradeId(String gradeId) {
        this.gradeId = gradeId;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public byte[] getCourseImage() {
        return courseImage;
    }

    public void setCourseImage(byte[] courseImage) {
        this.courseImage = courseImage;
    }
}
