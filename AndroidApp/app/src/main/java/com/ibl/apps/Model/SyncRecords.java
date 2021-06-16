package com.ibl.apps.Model;

import androidx.room.Embedded;

import java.util.List;

/**
 * Created by iblinfotech on 16/11/18.
 */

public class SyncRecords {

    private String message;

    private String status;

    private String response_code;

    @Embedded
    private Data data;

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

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
        this.data = data;
    }

    @Override
    public String toString() {
        return "ClassPojo [message = " + message + ", status = " + status + ", response_code = " + response_code + ", data = " + data + "]";
    }

    public static class Data {

        private List<Timerdata> timerdata;

        private List<Progressdata> progressdata;

        public List<Timerdata> getTimerdata() {
            return timerdata;
        }

        public void setTimerdata(List<Timerdata> timerdata) {
            this.timerdata = timerdata;
        }

        public List<Progressdata> getProgressdata() {
            return progressdata;
        }

        public void setProgressdata(List<Progressdata> progressdata) {
            this.progressdata = progressdata;
        }

        @Override
        public String toString() {
            return "ClassPojo [timerdata = " + timerdata + ", progressdata = " + progressdata + "]";
        }
    }

    public class Timerdata {

        private String userId;

        private String quizTime;

        private String passingScore;

        private String yourScore;

        private String quizId;

        private String isStatus;

        public String getUserId() {
            return userId;
        }

        public void setUserId(String userId) {
            this.userId = userId;
        }

        public String getQuizTime() {
            return quizTime;
        }

        public void setQuizTime(String quizTime) {
            this.quizTime = quizTime;
        }

        public String getPassingScore() {
            return passingScore;
        }

        public void setPassingScore(String passingScore) {
            this.passingScore = passingScore;
        }

        public String getYourScore() {
            return yourScore;
        }

        public void setYourScore(String yourScore) {
            this.yourScore = yourScore;
        }

        public String getQuizId() {
            return quizId;
        }

        public void setQuizId(String quizId) {
            this.quizId = quizId;
        }

        public String getIsStatus() {
            return isStatus;
        }

        public void setIsStatus(String isStatus) {
            this.isStatus = isStatus;
        }

        @Override
        public String toString() {
            return "ClassPojo [userId = " + userId + ", quizTime = " + quizTime + ", passingScore = " + passingScore + ", yourScore = " + yourScore + ", quizId = " + quizId + ", isStatus = " + isStatus + "]";
        }
    }

    public class Progressdata {

        private String fileId;

        private String lessonId;

        private String lessonProgressId;

        private String lessonProgress;

        private String gradeId;

        private String userId;

        private String totalRecords;

        private String quizId;

        private String isStatus;

        public String getFileId() {
            return fileId;
        }

        public void setFileId(String fileId) {
            this.fileId = fileId;
        }

        public String getLessonId() {
            return lessonId;
        }

        public void setLessonId(String lessonId) {
            this.lessonId = lessonId;
        }

        public String getLessonProgressId() {
            return lessonProgressId;
        }

        public void setLessonProgressId(String lessonProgressId) {
            this.lessonProgressId = lessonProgressId;
        }

        public String getLessonProgress() {
            return lessonProgress;
        }

        public void setLessonProgress(String lessonProgress) {
            this.lessonProgress = lessonProgress;
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

        public String getTotalRecords() {
            return totalRecords;
        }

        public void setTotalRecords(String totalRecords) {
            this.totalRecords = totalRecords;
        }

        public String getQuizId() {
            return quizId;
        }

        public void setQuizId(String quizId) {
            this.quizId = quizId;
        }

        public String getIsStatus() {
            return isStatus;
        }

        public void setIsStatus(String isStatus) {
            this.isStatus = isStatus;
        }

        @Override
        public String toString() {
            return "ClassPojo [fileId = " + fileId + ", lessonId = " + lessonId + ", lessonProgressId = " + lessonProgressId + ", lessonProgress = " + lessonProgress + ", gradeId = " + gradeId + ", userId = " + userId + ", totalRecords = " + totalRecords + ", quizId = " + quizId + ", isStatus = " + isStatus + "]";
        }
    }
}
