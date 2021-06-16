package com.ibl.apps.QuizModule.entities;

import androidx.room.Embedded;
import androidx.room.Relation;

import java.util.List;

public class QuestionWithAnswerEntity {

    @Embedded
    public QuestionEntity questionEntity;

    @Relation(
            parentColumn = "id",
            entityColumn = "question_id"
    )
    public List<AnswerEntity> answers;

    public QuestionWithAnswerEntity() {
    }

    public QuestionEntity getQuestionEntity() {
        return questionEntity;
    }

    public void setQuestionEntity(QuestionEntity questionEntity) {
        this.questionEntity = questionEntity;
    }

    public List<AnswerEntity> getAnswers() {
        return answers;
    }

    public void setAnswers(List<AnswerEntity> answers) {
        this.answers = answers;
    }
}
