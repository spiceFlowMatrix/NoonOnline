package com.ibl.apps.QuizModule.entities;

import androidx.room.Embedded;
import androidx.room.Relation;

import java.util.List;

public class QuizWithQuestionEntity {

    @Embedded
    public QuizEntity quiz;

    @Relation(
            parentColumn = "id",
            entityColumn = "quiz_id",
            entity = QuestionEntity.class
    )
    public List<QuestionWithAnswerEntity> questions;

    public QuizWithQuestionEntity() {
    }

    public QuizEntity getQuiz() {
        return quiz;
    }

    public void setQuiz(QuizEntity quiz) {
        this.quiz = quiz;
    }

    public List<QuestionWithAnswerEntity> getQuestions() {
        return questions;
    }

    public void setQuestions(List<QuestionWithAnswerEntity> questions) {
        this.questions = questions;
    }
}
