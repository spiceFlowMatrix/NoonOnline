package com.ibl.apps.Model;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.List;

/**
 * Created by iblinfotech on 20/09/18.
 */

@Entity
public class AssignmentObject {

    private String message;

    private String status;

    private String response_code;

    @ColumnInfo(name = "ListData")
    @TypeConverters(DataTypeConverter.class)
    private List<Data> data = null;

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

    public List<Data> getData() {
        return data;
    }

    public void setData(List<Data> data) {
        this.data = data;
    }

    @Override
    public String toString() {
        return "ClassPojo [message = " + message + ", status = " + status + ", response_code = " + response_code + ", data = " + data + "]";
    }

    public static class Data {

        private String id;

        private String status;

        private String description;

        private String name;

        private String code;

        private String submissioncount;

        private String chapterid;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getStatus() {
            return status;
        }

        public void setStatus(String status) {
            this.status = status;
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

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public String getSubmissioncount() {
            return submissioncount;
        }

        public void setSubmissioncount(String submissioncount) {
            this.submissioncount = submissioncount;
        }

        public String getChapterid() {
            return chapterid;
        }

        public void setChapterid(String chapterid) {
            this.chapterid = chapterid;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", status = " + status + ", description = " + description + ", name = " + name + ", code = " + code + ", submissioncount = " + submissioncount + ", chapterid = " + chapterid + "]";
        }
    }
}
