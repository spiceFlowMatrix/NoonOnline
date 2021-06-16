package com.ibl.apps.QuizModule.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class QuizDataModel {

    @SerializedName("quizid")
    @Expose
    private long quizId;

    @SerializedName("passmark")
    @Expose
    private double passMark;

    @SerializedName("quizSummaryResponse")
    @Expose
    private String quizSummaryResponse;

    @SerializedName("questions")
    @Expose
    private List<QuestionModel> questionModels;

    public QuizDataModel() {
    }

    public long getQuizId() {
        return quizId;
    }

    public void setQuizId(long quizId) {
        this.quizId = quizId;
    }

    public double getPassMark() {
        return passMark;
    }

    public void setPassMark(double passMark) {
        this.passMark = passMark;
    }

    public String getQuizSummaryResponse() {
        return quizSummaryResponse;
    }

    public void setQuizSummaryResponse(String quizSummaryResponse) {
        this.quizSummaryResponse = quizSummaryResponse;
    }

    public List<QuestionModel> getQuestionModels() {
        return questionModels;
    }

    public void setQuestionModels(List<QuestionModel> questionModels) {
        this.questionModels = questionModels;
    }
}
