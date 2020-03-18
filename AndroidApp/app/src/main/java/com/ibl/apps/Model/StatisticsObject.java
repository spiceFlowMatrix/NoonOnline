package com.ibl.apps.Model;

import android.arch.persistence.room.Embedded;
import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

/**
 * Created by iblinfotech on 27/10/18.
 */
@Entity
public class StatisticsObject {

    @PrimaryKey(autoGenerate = true)
    private int statisticsID;

    private String message;

    private String status;

    private String response_code;

    private String userId;

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

    public int getStatisticsID() {
        return statisticsID;
    }

    public void setStatisticsID(int statisticsID) {
        this.statisticsID = statisticsID;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    @Override
    public String toString() {
        return "StatisticsObject{" +
                "statisticsID=" + statisticsID +
                ", message='" + message + '\'' +
                ", status='" + status + '\'' +
                ", response_code='" + response_code + '\'' +
                ", userId='" + userId + '\'' +
                ", data=" + data +
                '}';
    }

    public static class Data {

        private String avrageassignmentscore;

        private String terminatedcourse;

        private String totalavailablescore;

        private String complatecourse;

        private String avragequizscore;

        private String totalcourse;

        private String unreadnotifications;

        public String getAvrageassignmentscore() {
            return avrageassignmentscore;
        }

        public void setAvrageassignmentscore(String avrageassignmentscore) {
            this.avrageassignmentscore = avrageassignmentscore;
        }

        public String getTerminatedcourse() {
            return terminatedcourse;
        }

        public void setTerminatedcourse(String terminatedcourse) {
            this.terminatedcourse = terminatedcourse;
        }

        public String getTotalavailablescore() {
            return totalavailablescore;
        }

        public void setTotalavailablescore(String totalavailablescore) {
            this.totalavailablescore = totalavailablescore;
        }

        public String getComplatecourse() {
            return complatecourse;
        }

        public void setComplatecourse(String complatecourse) {
            this.complatecourse = complatecourse;
        }

        public String getAvragequizscore() {
            return avragequizscore;
        }

        public void setAvragequizscore(String avragequizscore) {
            this.avragequizscore = avragequizscore;
        }

        public String getTotalcourse() {
            return totalcourse;
        }

        public void setTotalcourse(String totalcourse) {
            this.totalcourse = totalcourse;
        }

        public String getUnreadnotifications() {
            return unreadnotifications;
        }

        public void setUnreadnotifications(String unreadnotifications) {
            this.unreadnotifications = unreadnotifications;
        }

        @Override
        public String toString() {
            return "ClassPojo [avrageassignmentscore = " + avrageassignmentscore + ", terminatedcourse = " + terminatedcourse + ", totalavailablescore = " + totalavailablescore + ", complatecourse = " + complatecourse + ", avragequizscore = " + avragequizscore + ", totalcourse = " + totalcourse + ", unreadnotifications = " + unreadnotifications + "]";
        }
    }

}
