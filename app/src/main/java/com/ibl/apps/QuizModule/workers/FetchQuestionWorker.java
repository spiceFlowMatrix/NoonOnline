package com.ibl.apps.QuizModule.workers;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Context;
import android.os.Build;
import android.util.Log;

import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.core.app.NotificationCompat;
import androidx.work.Data;
import androidx.work.ForegroundInfo;
import androidx.work.Worker;
import androidx.work.WorkerParameters;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.QuizManament.QuizApiService;
import com.ibl.apps.QuizModule.entities.AnswerEntity;
import com.ibl.apps.QuizModule.entities.QuestionEntity;
import com.ibl.apps.QuizModule.entities.QuizEntity;
import com.ibl.apps.QuizModule.models.AnswerModel;
import com.ibl.apps.QuizModule.models.QuestionModel;
import com.ibl.apps.QuizModule.models.QuizResponseModel;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.noon.R;

import java.io.IOException;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;
import java.util.concurrent.CountDownLatch;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class FetchQuestionWorker extends Worker {

    private static final String PROGRESS = "PROGRESS";

    public FetchQuestionWorker(@NonNull Context context, @NonNull WorkerParameters workerParams) {
        super(context, workerParams);
    }

    @NonNull
    @Override
    public Result doWork() {
        Log.e("IIIIII", "fetch question worker started");

        final Result[] result = {Result.failure()};
        Data taskData = getInputData();
        String quizId = taskData.getString("quiz_id");
        CountDownLatch countDownLatch = new CountDownLatch(1);

        setProgressAsync(new Data.Builder().putInt(PROGRESS, 0).build());
        setForegroundAsync(createForegroundInfo());

        QuizApiService apiService = ApiClient.getClient(getApplicationContext()).create(QuizApiService.class);
        apiService.fetchAllQuestionData(quizId).enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(@NonNull Call<ResponseBody> call, @NonNull Response<ResponseBody> response) {
                try {
                    setProgressAsync(new Data.Builder().putInt(PROGRESS, 20).build());
                    QuizResponseModel quizResponseModel = convertToQuizMainObject(response.body().string());
                    AppDatabase appDatabase = AppDatabase.getAppDatabase(getApplicationContext());
                    QuizEntity quizEntity = new QuizEntity();
                    quizEntity.setId(quizResponseModel.getQuizDataModel().getQuizId());
                    quizEntity.setPassMark((float) quizResponseModel.getQuizDataModel().getPassMark());
                    quizEntity.setQuizSummary(quizResponseModel.getQuizDataModel().getQuizSummaryResponse());
                    appDatabase.quizDao().insert(quizEntity);
                    setProgressAsync(new Data.Builder().putInt(PROGRESS, 30).build());

                    List<QuestionEntity> questionEntities = new ArrayList<>();
                    List<AnswerEntity> answerEntities = new ArrayList<>();
                    ArrayList<QuestionModel> quizQuestions = new ArrayList<>(quizResponseModel.getQuizDataModel().getQuestionModels());
                    for(QuestionModel questionModel : quizQuestions){
                        QuestionEntity questionEntity = new QuestionEntity();
                        questionEntity.setId(questionModel.getId());
                        questionEntity.setQuizId(quizResponseModel.getQuizDataModel().getQuizId());
                        questionEntity.setQuestionTypeId(questionModel.getQuestionTypeId());
                        questionEntity.setQuestionType(questionModel.getQuestionType());
                        questionEntity.setQuestionText(questionModel.getQuestionText());
                        questionEntity.setExplanation(questionModel.getExplanation());
                        questionEntity.setMultiAnswer(questionModel.isMultiAnswer());
                        questionEntity.setQuestionPicked(false);
                        questionEntities.add(questionEntity);

                        for(AnswerModel answerModel : questionModel.getAnswerModels()){
                            AnswerEntity answerEntity = new AnswerEntity();
                            answerEntity.setId(answerModel.getId());
                            answerEntity.setQuestionId(questionModel.getId());
                            answerEntity.setAnswer(answerModel.getAnswer());
                            answerEntity.setExtraText(answerModel.getExtraText());
                            answerEntity.setCorrect(answerModel.isCorrect());
                            answerEntities.add(answerEntity);
                        }
                    }
                    appDatabase.questionDao().insertAll(questionEntities);
                    setProgressAsync(new Data.Builder().putInt(PROGRESS, 40).build());

                    appDatabase.answerDao().insertAll(answerEntities);
                    setProgressAsync(new Data.Builder().putInt(PROGRESS, 50).build());

                    new DownloadTask(getApplicationContext(), quizId);
                    setProgressAsync(new Data.Builder().putInt(PROGRESS, 100).build());
                    result[0] = Result.success();
                } catch (NullPointerException | IOException e) {
                    Log.e("IIIIII", Objects.requireNonNull(e.getMessage()));
                    Log.e("IIIIII", Objects.requireNonNull(e.getLocalizedMessage()));
                    e.printStackTrace();
                    setProgressAsync(new Data.Builder().putInt(PROGRESS, 100).build());
                }
                countDownLatch.countDown();
            }

            @Override
            public void onFailure(@NonNull Call<ResponseBody> call, @NonNull Throwable throwable) {
                Log.e("IIIII", call.request().toString());
                Log.e("IIIIII", Objects.requireNonNull(throwable.getLocalizedMessage()));
                setProgressAsync(new Data.Builder().putInt(PROGRESS, 100).build());
                countDownLatch.countDown();
            }
        });

        try {
            countDownLatch.await();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        return result[0];
    }

    @NonNull
    private ForegroundInfo createForegroundInfo() {
        // Build a notification using bytesRead and contentLength

        Context context = getApplicationContext();
        String channelId = "quiz_notification";

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            createChannel();
        }

        Notification notification = new NotificationCompat.Builder(context, channelId)
                .setContentTitle("Fetching quiz data")
                .setTicker("Fetching quiz data")
                .setProgress(100, 0, true)
                .setSmallIcon(R.mipmap.ic_launcher)
                .setOngoing(true)
                // Add the cancel action to the notification which can
                // be used to cancel the worker
//                .addAction(android.R.drawable.ic_delete, cancel, intent)
                .build();

        return new ForegroundInfo(5253, notification);
    }

    @RequiresApi(Build.VERSION_CODES.O)
    private void createChannel() {
        // Create the NotificationChannel, but only on API 26+ because
        // the NotificationChannel class is new and not in the support library
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            CharSequence name = "Quiz Notification";
            String description = "You will get notification realted to Quiz data sync.";
            int importance = NotificationManager.IMPORTANCE_DEFAULT;
            NotificationChannel channel = new NotificationChannel("quiz_notification", name, importance);
            channel.setDescription(description);
            // Register the channel with the system; you can't change the importance
            // or other notification behaviors after this
            NotificationManager notificationManager = getApplicationContext().getSystemService(NotificationManager.class);
            notificationManager.createNotificationChannel(channel);
        }
    }

    public static QuizResponseModel convertToQuizMainObject(String jsonString) {
        try {
            Gson gson = new Gson();
            Type type = new TypeToken<QuizResponseModel>() {
            }.getType();
            return gson.fromJson(jsonString, type);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return new QuizResponseModel();
    }
}
