package com.ibl.apps.QuizModule.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class QuizResponseModel {

    @SerializedName("response_code")
    @Expose
    private int responseCode;

    @SerializedName("message")
    @Expose
    private String message;

    @SerializedName("status")
    @Expose
    private String status;

    @SerializedName("data")
    @Expose
    private QuizDataModel quizDataModel;

    public void QuizDataModel() {
    }

    public int getResponseCode() {
        return responseCode;
    }

    public void setResponseCode(int responseCode) {
        this.responseCode = responseCode;
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

    public QuizDataModel getQuizDataModel() {
        return quizDataModel;
    }

    public void setQuizDataModel(QuizDataModel quizDataModel) {
        this.quizDataModel = quizDataModel;
    }
}
