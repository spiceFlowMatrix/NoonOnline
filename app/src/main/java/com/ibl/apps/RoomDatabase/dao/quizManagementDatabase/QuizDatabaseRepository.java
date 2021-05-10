package com.ibl.apps.RoomDatabase.dao.quizManagementDatabase;

import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.QuizAnswerSelect;
import com.ibl.apps.RoomDatabase.entity.QuizProgress;
import com.ibl.apps.RoomDatabase.entity.QuizUserResult;
import com.ibl.apps.noon.NoonApplication;

import java.util.List;

public class QuizDatabaseRepository {
    private AppDatabase database;

    public QuizDatabaseRepository() {
        database = AppDatabase.getAppDatabase(NoonApplication.getContext());
    }

    public void insertQuizAnswerData(QuizMainObject... quizMainObjects) {
        database.quizAnswerDao().insertAll(quizMainObjects);
    }

    public QuizMainObject getQuizByUserId(String userId, String newquizId) {
        return database.quizAnswerDao().getquizData(userId, newquizId);
    }

    public void insertAllQuizAnswerSelect(QuizAnswerSelect... quizAnswerSelects) {
        database.quizAnswerDao().insertAll(quizAnswerSelects);
    }

    public QuizAnswerSelect getItemAnswerSelect(String nextID) {
        return database.quizAnswerDao().getItemAnswerSelect(nextID);
    }

    public int getCountTrueAnswerSelect(String quizID, String iscorrect) {
        return database.quizAnswerDao().getCountTrueAnswerSelect(quizID, iscorrect);
    }

    public void updateItemQuizAnswerSelect(String selectedAnswerId, String nextID) {
        database.quizAnswerDao().updateItemQuizAnswerSelect(selectedAnswerId, nextID);
    }

    public void deleteSelectedAnswer() {
        database.quizAnswerDao().deleteSelectedAnswer();
    }

    public int getTrueAnswer(String selectedAnswerId, String questionID, String iscorrect) {
        return database.quizAnswerDao().getTrueAnswer(selectedAnswerId, questionID, iscorrect);
    }

    public void insertAllQuizProgress(QuizProgress... quizProgresses) {
        database.quizProgressDao().insertAll(quizProgresses);
    }

    public QuizProgress getQuizProgress(String quizId, String chapterId) {
        return database.quizProgressDao().getQuizProgress(quizId, chapterId);
    }

    public void deleteAll() {
        database.quizProgressDao().deleteAll();
    }

    public void updateQuizProgress(QuizProgress... quizProgresses) {
        database.quizProgressDao().updateQuizProgress(quizProgresses);
    }

    public void insertAllQuizUserResult(QuizUserResult... quizUserResults) {
        database.quizUserResultDao().insertAll(quizUserResults);
    }

    public void updateQuizUserResult(String userId, String quizId, String quizTime, String yourScore, String passingScore, String totalQuesions, String totalAnswers, String date) {
        database.quizUserResultDao().updateQuizUserResult(userId, quizId, quizTime, yourScore, passingScore, totalQuesions, totalAnswers, date);
    }

    public List<QuizUserResult> getAllQuizuserResult(boolean isStatus, String userId) {
        return database.quizUserResultDao().getAllQuizuserResult(isStatus, userId);
    }

    public QuizUserResult getQuizuserResult(String userId, String quizId) {
        return database.quizUserResultDao().getQuizuserResult(userId, quizId);
    }

    public void updatelQuizUserResultStatus(boolean isStatus, String quizId, String userId) {
        database.quizUserResultDao().updatelQuizUserResultStatus(isStatus, quizId, userId);
    }
}
