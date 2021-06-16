package com.ibl.apps.QuizModule.entities;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.ForeignKey;
import androidx.room.PrimaryKey;

@Entity(tableName = "answers",
        foreignKeys = @ForeignKey(entity = QuestionEntity.class,
            parentColumns = "id",
            childColumns = "question_id"
        )
)
public class AnswerEntity {

    @PrimaryKey
    @ColumnInfo(name = "id")
    private Long id;

    @ColumnInfo(name = "question_id", index = true)
    private Long questionId;

    @ColumnInfo(name = "answer")
    private String answer;

    @ColumnInfo(name = "extra_text")
    private String extraText;

    @ColumnInfo(name = "is_correct")
    private Boolean isCorrect;

    public AnswerEntity() {
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Long getQuestionId() {
        return questionId;
    }

    public void setQuestionId(Long questionId) {
        this.questionId = questionId;
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

    public Boolean getCorrect() {
        return isCorrect;
    }

    public void setCorrect(Boolean correct) {
        isCorrect = correct;
    }
}
