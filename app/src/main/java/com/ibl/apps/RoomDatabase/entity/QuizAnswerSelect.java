package com.ibl.apps.RoomDatabase.entity;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;

/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Entity
public class QuizAnswerSelect {

    @PrimaryKey(autoGenerate = true)
    private int answerselectID;

    @ColumnInfo(name = "quizID")
    private String quizID;

    @ColumnInfo(name = "questionID")
    private String questionID;

    @ColumnInfo(name = "selectedAnswerId")
    private String selectedAnswerId;

    @ColumnInfo(name = "nextID")
    private String nextID;

    @ColumnInfo(name = "isStatus")
    private String isStatus;

    @ColumnInfo(name = "iscorrect")
    private String iscorrect;

    public String getIscorrect() {
        return iscorrect;
    }

    public void setIscorrect(String iscorrect) {
        this.iscorrect = iscorrect;
    }

    public int getAnswerselectID() {
        return answerselectID;
    }

    public void setAnswerselectID(int answerselectID) {
        this.answerselectID = answerselectID;
    }

    public String getQuizID() {
        return quizID;
    }

    public void setQuizID(String quizID) {
        this.quizID = quizID;
    }

    public String getQuestionID() {
        return questionID;
    }

    public void setQuestionID(String questionID) {
        this.questionID = questionID;
    }

    public String getSelectedAnswerId() {
        return selectedAnswerId;
    }

    public void setSelectedAnswerId(String selectedAnswerId) {
        this.selectedAnswerId = selectedAnswerId;
    }

    public String getNextID() {
        return nextID;
    }

    public void setNextID(String nextID) {
        this.nextID = nextID;
    }

    public String getIsStatus() {
        return isStatus;
    }

    public void setIsStatus(String isStatus) {
        this.isStatus = isStatus;
    }


    @Override
    public String toString() {
        return "QuizAnswerSelect{" +
                "answerselectID=" + answerselectID +
                ", quizID='" + quizID + '\'' +
                ", questionID='" + questionID + '\'' +
                ", selectedAnswerId='" + selectedAnswerId + '\'' +
                ", nextID='" + nextID + '\'' +
                ", isStatus='" + isStatus + '\'' +
                ", iscorrect='" + iscorrect + '\'' +
                '}';
    }
}
