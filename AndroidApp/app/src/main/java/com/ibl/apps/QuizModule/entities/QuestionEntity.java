package com.ibl.apps.QuizModule.entities;

import androidx.room.ColumnInfo;
import androidx.room.Embedded;
import androidx.room.Entity;
import androidx.room.ForeignKey;
import androidx.room.PrimaryKey;

import java.util.List;

@Entity(tableName = "questions",
        foreignKeys = @ForeignKey(entity = QuizEntity.class,
                parentColumns = "id",
                childColumns = "quiz_id"
        )
)
public class QuestionEntity {

    @PrimaryKey
    @ColumnInfo(name = "id")
    private Long id;

    @ColumnInfo(name = "quiz_id", index = true)
    private Long quizId;

    @ColumnInfo(name = "question_type_id")
    private Integer questionTypeId;

    @ColumnInfo(name = "question_type")
    private Integer questionType;

    @ColumnInfo(name = "question_text")
    private String questionText;

    @ColumnInfo(name = "explanation")
    private String explanation;

    @ColumnInfo(name = "is_multi_answer")
    private Boolean isMultiAnswer;

    @ColumnInfo(name = "is_question_picked")
    private Boolean isQuestionPicked;

    public QuestionEntity() {
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Long getQuizId() {
        return quizId;
    }

    public void setQuizId(Long quizId) {
        this.quizId = quizId;
    }

    public Integer getQuestionTypeId() {
        return questionTypeId;
    }

    public void setQuestionTypeId(Integer questionTypeId) {
        this.questionTypeId = questionTypeId;
    }

    public Integer getQuestionType() {
        return questionType;
    }

    public void setQuestionType(Integer questionType) {
        this.questionType = questionType;
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

    public Boolean getMultiAnswer() {
        return isMultiAnswer;
    }

    public void setMultiAnswer(Boolean multiAnswer) {
        isMultiAnswer = multiAnswer;
    }

    public Boolean getQuestionPicked() {
        return isQuestionPicked;
    }

    public void setQuestionPicked(Boolean questionPicked) {
        isQuestionPicked = questionPicked;
    }
}
