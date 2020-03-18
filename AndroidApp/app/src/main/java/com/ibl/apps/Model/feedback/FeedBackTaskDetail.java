
package com.ibl.apps.Model.feedback;

import com.google.gson.annotations.SerializedName;

import java.util.List;

@SuppressWarnings("unused")
public class FeedBackTaskDetail {

    @SerializedName("data")
    private Data mData;
    @SerializedName("message")
    private String mMessage;
    @SerializedName("response_code")
    private Long mResponseCode;
    @SerializedName("status")
    private String mStatus;

    public Data getData() {
        return mData;
    }

    public void setData(Data data) {
        mData = data;
    }

    public String getMessage() {
        return mMessage;
    }

    public void setMessage(String message) {
        mMessage = message;
    }

    public Long getResponseCode() {
        return mResponseCode;
    }

    public void setResponseCode(Long responseCode) {
        mResponseCode = responseCode;
    }

    public String getStatus() {
        return mStatus;
    }

    public void setStatus(String status) {
        mStatus = status;
    }

    @SuppressWarnings("unused")
    public static class Data {

        @SerializedName("archiveddate")
        private Object mArchiveddate;
        @SerializedName("category")
        private Category mCategory;
        @SerializedName("complateddate")
        private Object mComplateddate;
        @SerializedName("description")
        private String mDescription;
        @SerializedName("files")
        private List<FileData> mFileData;
        @SerializedName("id")
        private Long mId;
        @SerializedName("startdate")
        private String mStartdate;
        @SerializedName("status")
        private Long mStatus;
        @SerializedName("submitteddate")
        private String mSubmitteddate;
        @SerializedName("title")
        private String mTitle;
        @SerializedName("user")
        private User mUser;

        @SerializedName("course")
        private CourseData course;

        @SerializedName("chapter")
        private ChaptersData chapter;

        @SerializedName("lesson")
        private LessonsData lesson;

        @SerializedName("time")
        private String time;

        public String getTime() {
            return time;
        }

        public void setTime(String time) {
            this.time = time;
        }

        public CourseData getCourse() {
            return course;
        }

        public void setCourse(CourseData course) {
            this.course = course;
        }

        public ChaptersData getChapter() {
            return chapter;
        }

        public void setChapter(ChaptersData chapter) {
            this.chapter = chapter;
        }

        public LessonsData getLesson() {
            return lesson;
        }

        public void setLesson(LessonsData lesson) {
            this.lesson = lesson;
        }

        public Object getArchiveddate() {
            return mArchiveddate;
        }

        public void setArchiveddate(Object archiveddate) {
            mArchiveddate = archiveddate;
        }

        public Category getCategory() {
            return mCategory;
        }

        public void setCategory(Category category) {
            mCategory = category;
        }

        public Object getComplateddate() {
            return mComplateddate;
        }

        public void setComplateddate(Object complateddate) {
            mComplateddate = complateddate;
        }

        public String getDescription() {
            return mDescription;
        }

        public void setDescription(String description) {
            mDescription = description;
        }

        public List<FileData> getFiles() {
            return mFileData;
        }

        public void setFiles(List<FileData> fileData) {
            mFileData = fileData;
        }

        public Long getId() {
            return mId;
        }

        public void setId(Long id) {
            mId = id;
        }

        public String getStartdate() {
            return mStartdate;
        }

        public void setStartdate(String startdate) {
            mStartdate = startdate;
        }

        public Long getStatus() {
            return mStatus;
        }

        public void setStatus(Long status) {
            mStatus = status;
        }

        public String getSubmitteddate() {
            return mSubmitteddate;
        }

        public void setSubmitteddate(String submitteddate) {
            mSubmitteddate = submitteddate;
        }

        public String getTitle() {
            return mTitle;
        }

        public void setTitle(String title) {
            mTitle = title;
        }

        public User getUser() {
            return mUser;
        }

        public void setUser(User user) {
            mUser = user;
        }

    }

    @Override
    public String toString() {
        return "FeedBackTaskDetail{" +
                "mData=" + mData +
                ", mMessage='" + mMessage + '\'' +
                ", mResponseCode=" + mResponseCode +
                ", mStatus='" + mStatus + '\'' +
                '}';
    }

    @SuppressWarnings("unused")
    public static class Category {

        @SerializedName("id")
        private Long mId;
        @SerializedName("name")
        private String mName;

        public Long getId() {
            return mId;
        }

        public void setId(Long id) {
            mId = id;
        }

        public String getName() {
            return mName;
        }

        public void setName(String name) {
            mName = name;
        }

    }

    public static class CourseData {
        @SerializedName("id")
        private Long mId;
        @SerializedName("name")
        private String mName;

        public Long getmId() {
            return mId;
        }

        public void setmId(Long mId) {
            this.mId = mId;
        }

        public String getmName() {
            return mName;
        }

        public void setmName(String mName) {
            this.mName = mName;
        }
    }

    public static class ChaptersData {
        @SerializedName("id")
        private Long mId;
        @SerializedName("name")
        private String mName;

        public Long getmId() {
            return mId;
        }

        public void setmId(Long mId) {
            this.mId = mId;
        }

        public String getmName() {
            return mName;
        }

        public void setmName(String mName) {
            this.mName = mName;
        }
    }

    public static class LessonsData {
        @SerializedName("id")
        private Long mId;
        @SerializedName("name")
        private String mName;

        public Long getmId() {
            return mId;
        }

        public void setmId(Long mId) {
            this.mId = mId;
        }

        public String getmName() {
            return mName;
        }

        public void setmName(String mName) {
            this.mName = mName;
        }
    }
}
