package com.ibl.apps.RoomDatabase.entity;

import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Entity
public class QuizUserResult {

    @PrimaryKey(autoGenerate = true)
    private int quizuserResultID;

    private String userId;

    private boolean isStatus;

    private String quizId;

    private String quizTime;

    private String yourScore;

    private String passingScore = "0";

    private String totalQuitions = "0";

    private String totalAnswers = "0";

    private String quizDate ;

    public String getQuizDate() {
        return quizDate;
    }

    public void setQuizDate(String quizDate) {
        this.quizDate = quizDate;
    }


    public int getQuizuserResultID() {
        return quizuserResultID;
    }

    public void setQuizuserResultID(int quizuserResultID) {
        this.quizuserResultID = quizuserResultID;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public boolean isStatus() {
        return isStatus;
    }

    public void setStatus(boolean status) {
        isStatus = status;
    }

    public String getQuizId() {
        return quizId;
    }

    public void setQuizId(String quizId) {
        this.quizId = quizId;
    }

    public String getQuizTime() {
        return quizTime;
    }

    public void setQuizTime(String quizTime) {
        this.quizTime = quizTime;
    }

    public String getYourScore() {
        return yourScore;
    }

    public void setYourScore(String yourScore) {
        this.yourScore = yourScore;
    }

    public String getPassingScore() {
        return passingScore ;
    }

    public void setPassingScore(String passingScore) {
        this.passingScore = passingScore;
    }

    public String getTotalQuitions() {
        return totalQuitions;
    }

    public void setTotalQuitions(String totalQuitions) {
        this.totalQuitions = totalQuitions;
    }

    public String getTotalAnswers() {
        return totalAnswers;
    }

    public void setTotalAnswers(String totalAnswers) {
        this.totalAnswers = totalAnswers;
    }

    @Override
    public String toString() {
        return "QuizUserResult{" +
                "quizuserResultID=" + quizuserResultID +
                ", userId='" + userId + '\'' +
                ", isStatus='" + isStatus + '\'' +
                ", quizId='" + quizId + '\'' +
                ", quizTime='" + quizTime + '\'' +
                ", yourScore='" + yourScore + '\'' +
                ", passingScore='" + passingScore + '\'' +
                '}';
    }
}
