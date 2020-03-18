package com.ibl.apps.Model;

/**
 * Created by iblinfotech on 13/09/18.
 */

public class ReminderObject {

    private String reminderToken;

    private String accessToken;

    private String idToken;

    private String logoutDate;

    private int reminderCount;

    private int currantHour;

    public String getAccessToken() {
        return accessToken;
    }

    public void setAccessToken(String accessToken) {
        this.accessToken = accessToken;
    }

    public String getIdToken() {
        return idToken;
    }

    public void setIdToken(String idToken) {
        this.idToken = idToken;
    }

    public String getReminderToken() {
        return reminderToken;
    }

    public void setReminderToken(String reminderToken) {
        this.reminderToken = reminderToken;
    }

    public String getLogoutDate() {
        return logoutDate;
    }

    public void setLogoutDate(String logoutDate) {
        this.logoutDate = logoutDate;
    }

    public int getReminderCount() {
        return reminderCount;
    }

    public void setReminderCount(int reminderCount) {
        this.reminderCount = reminderCount;
    }

    public int getCurrantHour() {
        return currantHour;
    }

    public void setCurrantHour(int currantHour) {
        this.currantHour = currantHour;
    }

    @Override
    public String toString() {
        return "ReminderObject{" +
                "reminderToken='" + reminderToken + '\'' +
                ", logoutDate='" + logoutDate + '\'' +
                ", reminderCount=" + reminderCount +
                ", currantHour=" + currantHour +
                '}';
    }
}
