package com.ibl.apps.Model;

import android.arch.persistence.room.ColumnInfo;
import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;
import android.arch.persistence.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.Arrays;
import java.util.List;

@Entity
public class CourseObject {

    @PrimaryKey(autoGenerate = true)
    private int CourseObjectID;

    private String totalcount;

    private String message;

    private String status;

    private String response_code;

    private String userId;

    @ColumnInfo(name = "ListData")
    @TypeConverters(DataTypeConverter.class)
    private List<Data> data = null;

    public String getTotalcount() {
        return totalcount;
    }

    public void setTotalcount(String totalcount) {
        this.totalcount = totalcount;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
    }

    public int getCourseObjectID() {
        return CourseObjectID;
    }

    public void setCourseObjectID(int courseObjectID) {
        CourseObjectID = courseObjectID;
    }

    public List<Data> getData() {
        return data;
    }

    public void setData(List<Data> data) {
        this.data = data;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    @Override
    public String toString() {
        return "CourseObject{" +
                "CourseObjectID=" + CourseObjectID +
                ", totalcount='" + totalcount + '\'' +
                ", message='" + message + '\'' +
                ", status='" + status + '\'' +
                ", response_code='" + response_code + '\'' +
                ", userId='" + userId + '\'' +
                ", data=" + data +
                '}';
    }

    public static class Data {

        private String id;

        private List<Courses> courses = null;

        private String name;

        public Data(String id, List<Courses> courses, String name) {
            this.id = id;
            this.courses = courses;
            this.name = name;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public List<Courses> getCourses() {
            return courses;
        }

        public void setCourses(List<Courses> courses) {
            this.courses = courses;
        }

        @Override
        public String toString() {
            return "FillesData{" +
                    "id='" + id + '\'' +
                    ", courses=" + courses +
                    ", name='" + name + '\'' +
                    '}';
        }
    }

    public class Courses {

        private String id;

        private String description;

        private String name;

        private String image;

        private String code;

        private int progressVal;

        private String startdate;

        private String enddate;

        public boolean isDeleted;

        @ColumnInfo(typeAffinity = ColumnInfo.BLOB)
        private byte[] courseImage;

        private Addtionalservice addtionalservice;

        public String getStartdate() {
            return startdate;
        }

        public void setStartdate(String startdate) {
            this.startdate = startdate;
        }

        public String getEnddate() {
            return enddate;
        }

        public void setEnddate(String enddate) {
            this.enddate = enddate;
        }

        public int getProgressVal() {
            return progressVal;
        }

        public void setProgressVal(int progressVal) {
            this.progressVal = progressVal;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getImage() {
            return image;
        }

        public void setImage(String image) {
            this.image = image;
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public byte[] getCourseImage() {
            return courseImage;
        }

        public void setCourseImage(byte[] courseImage) {
            this.courseImage = courseImage;
        }

        public boolean isDeleted() {
            return isDeleted;
        }

        public void setDeleted(boolean deleted) {
            isDeleted = deleted;
        }

        public Addtionalservice getAddtionalservice() {
            return addtionalservice;
        }

        public void setAddtionalservice(Addtionalservice addtionalservice) {
            this.addtionalservice = addtionalservice;
        }

        @Override
        public String toString() {
            return "Courses{" +
                    "id='" + id + '\'' +
                    ", description='" + description + '\'' +
                    ", name='" + name + '\'' +
                    ", image='" + image + '\'' +
                    ", code='" + code + '\'' +
                    ", progressVal=" + progressVal +
                    ", startdate='" + startdate + '\'' +
                    ", enddate='" + enddate + '\'' +
                    ", isDeleted=" + isDeleted +
                    ", courseImage=" + Arrays.toString(courseImage) +
                    '}';
        }
    }


    public static class Addtionalservice
    {
        private String library;

        private String assignment;

        private String discussion;

        public String getLibrary ()
        {
            return library;
        }

        public void setLibrary (String library)
        {
            this.library = library;
        }

        public String getAssignment ()
        {
            return assignment;
        }

        public void setAssignment (String assignment)
        {
            this.assignment = assignment;
        }

        public String getDiscussion ()
        {
            return discussion;
        }

        public void setDiscussion (String discussion)
        {
            this.discussion = discussion;
        }

        @Override
        public String toString()
        {
            return "ClassPojo [library = "+library+", assignment = "+assignment+", discussion = "+discussion+"]";
        }
    }


}