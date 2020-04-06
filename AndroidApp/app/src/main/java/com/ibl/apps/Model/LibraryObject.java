package com.ibl.apps.Model;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.List;

@Entity
public class LibraryObject {

    @PrimaryKey(autoGenerate = true)
    private int libraryObjectID;

    private String response_code;

    private String message;

    private String status;

    private String userId;

    @ColumnInfo(name = "ListData")
    @TypeConverters(DataTypeConverter.class)
    private List<Data> data;

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

    public int getLibraryObjectID() {
        return libraryObjectID;
    }

    public void setLibraryObjectID(int libraryObjectID) {
        this.libraryObjectID = libraryObjectID;
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

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    @Override
    public String toString() {
        return "ClassPojo [response_code = " + response_code + ", data = " + data + ", message = " + message + ", status = " + status + "]";
    }

    public static class Data {

        private String bookpublisher;

        private File file;

        private String author;

        private String subject;

        private String isPublished;

        private String bookcoverimage;

        private String description;

        private String id;

        private String title;

        @ColumnInfo(typeAffinity = ColumnInfo.BLOB)
        private byte[] bookcoverImageBitmap;

        public String getBookpublisher() {
            return bookpublisher;
        }

        public void setBookpublisher(String bookpublisher) {
            this.bookpublisher = bookpublisher;
        }

        public File getFile() {
            return file;
        }

        public void setFile(File file) {
            this.file = file;
        }

        public String getAuthor() {
            return author;
        }

        public void setAuthor(String author) {
            this.author = author;
        }

        public String getSubject() {
            return subject;
        }

        public void setSubject(String subject) {
            this.subject = subject;
        }

        public String getIsPublished() {
            return isPublished;
        }

        public void setIsPublished(String isPublished) {
            this.isPublished = isPublished;
        }

        public String getBookcoverimage() {
            return bookcoverimage;
        }

        public void setBookcoverimage(String bookcoverimage) {
            this.bookcoverimage = bookcoverimage;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getTitle() {
            return title;
        }

        public void setTitle(String title) {
            this.title = title;
        }

        public byte[] getBookcoverImageBitmap() {
            return bookcoverImageBitmap;
        }

        public void setBookcoverImageBitmap(byte[] bookcoverImageBitmap) {
            this.bookcoverImageBitmap = bookcoverImageBitmap;
        }

        @Override
        public String toString() {
            return "ClassPojo [bookpublisher = " + bookpublisher + ", author = " + author + ", subject = " + subject + ", isPublished = " + isPublished + ", bookcoverimage = " + bookcoverimage + ", description = " + description + ", id = " + id + ", title = " + title + "]";
        }
    }

    public class File {

        private String duration;

        private String filename;

        private String name;

        private String description;

        private String filetypeid;

        private String id;

        private String filesize;

        private String totalpages;

        private String filetypename;

        private String url;

        public String getDuration() {
            return duration;
        }

        public void setDuration(String duration) {
            this.duration = duration;
        }

        public String getFilename() {
            return filename;
        }

        public void setFilename(String filename) {
            this.filename = filename;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String getFiletypeid() {
            return filetypeid;
        }

        public void setFiletypeid(String filetypeid) {
            this.filetypeid = filetypeid;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getFilesize() {
            return filesize;
        }

        public void setFilesize(String filesize) {
            this.filesize = filesize;
        }

        public String getTotalpages() {
            return totalpages;
        }

        public void setTotalpages(String totalpages) {
            this.totalpages = totalpages;
        }

        public String getFiletypename() {
            return filetypename;
        }

        public void setFiletypename(String filetypename) {
            this.filetypename = filetypename;
        }

        public String getUrl() {
            return url;
        }

        public void setUrl(String url) {
            this.url = url;
        }

        @Override
        public String toString() {
            return "ClassPojo [duration = " + duration + ", filename = " + filename + ", name = " + name + ", description = " + description + ", filetypeid = " + filetypeid + ", id = " + id + ", filesize = " + filesize + ", totalpages = " + totalpages + ", filetypename = " + filetypename + ", url = " + url + "]";
        }
    }


}

