package com.ibl.apps.QuizModule.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class QuestionModel {

    @SerializedName("id")
    @Expose
    private long id;

    @SerializedName("questiontypeid")
    @Expose
    private int questionTypeId;

    @SerializedName("questiontext")
    @Expose
    private String questionText;

    @SerializedName("explanation")
    @Expose
    private String explanation;

    @SerializedName("ismultianswer")
    @Expose
    private boolean isMultiAnswer;

    @SerializedName("questiontype")
    @Expose
    private int questionType;

    @SerializedName("answers")
    @Expose
    private List<AnswerModel> answerModels;

    @SerializedName("images")
    @Expose
    private ImageModel[] imageModels;

    public QuestionModel() {
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public int getQuestionTypeId() {
        return questionTypeId;
    }

    public void setQuestionTypeId(int questionTypeId) {
        this.questionTypeId = questionTypeId;
    }

    public String getQuestionText() {
        return questionText;
    }

    public void setQuestionText(String questionText) {
        this.questionText = questionText;
    }

    public String getExplanation() {
        return explanation;
    }

    public void setExplanation(String explanation) {
        this.explanation = explanation;
    }

    public boolean isMultiAnswer() {
        return isMultiAnswer;
    }

    public void setMultiAnswer(boolean multiAnswer) {
        isMultiAnswer = multiAnswer;
    }

    public int getQuestionType() {
        return questionType;
    }

    public void setQuestionType(int questionType) {
        this.questionType = questionType;
    }

    public List<AnswerModel> getAnswerModels() {
        return answerModels;
    }

    public void setAnswerModels(List<AnswerModel> answerModels) {
        this.answerModels = answerModels;
    }

    public ImageModel[] getImageModels() {
        return imageModels;
    }

    public void setImageModels(ImageModel[] imageModels) {
        this.imageModels = imageModels;
    }
}
