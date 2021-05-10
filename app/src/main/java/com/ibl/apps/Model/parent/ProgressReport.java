
package com.ibl.apps.Model.parent;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;


@SuppressWarnings("unused")
public class ProgressReport {

    @Expose
    private Data data;
    @Expose
    private String message;
    @SerializedName("response_code")
    private Long responseCode;
    @Expose
    private String status;

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
        this.data = data;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public Long getResponseCode() {
        return responseCode;
    }

    public void setResponseCode(Long responseCode) {
        this.responseCode = responseCode;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public static class Data {

        @Expose
        private float overallscore = 0;
        @Expose
        private String score = " ";
        @Expose
        private String totaltries = " ";

        public float getOverallscore() {
            return overallscore;
        }

        public void setOverallscore(float overallscore) {
            this.overallscore = overallscore;
        }

        public String getScore() {
            return score;
        }

        public void setScore(String score) {
            this.score = score;
        }

        public String getTotaltries() {
            return totaltries;
        }

        public void setTotaltries(String totaltries) {
            this.totaltries = totaltries;
        }

    }
}
