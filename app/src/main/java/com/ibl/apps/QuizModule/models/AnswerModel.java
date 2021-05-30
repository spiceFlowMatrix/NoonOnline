package com.ibl.apps.QuizModule.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class AnswerModel {

    @SerializedName("id")
    @Expose
    private long id;

    @SerializedName("answer")
    @Expose
    private String answer;

    @SerializedName("extratext")
    @Expose
    private String extraText;

    @SerializedName("iscorrect")
    @Expose
    private boolean isCorrect;

    @SerializedName("questionid")
    @Expose
    private long questionId;

    public AnswerModel() {
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getAnswer() {
        return answer;
    }

    public void setAnswer(String answer) {
        this.answer = answer;
    }

    public String getExtraText() {
        return extraText;
    }

    public void setExtraText(String extraText) {
        this.extraText = extraText;
    }

    public boolean isCorrect() {
        return isCorrect;
    }

    public void setCorrect(boolean correct) {
        isCorrect = correct;
    }

    public long getQuestionId() {
        return questionId;
    }

    public void setQuestionId(long questionId) {
        this.questionId = questionId;
    }
}
