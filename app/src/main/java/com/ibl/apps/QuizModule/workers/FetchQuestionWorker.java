package com.ibl.apps.QuizModule.workers;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.content.Context;
import android.os.Build;
import android.os.Environment;
import android.util.Log;
import android.widget.Toast;

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

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;
import java.util.concurrent.CountDownLatch;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class FetchQuestionWorker extends Worker {

    private String quizId = "";
    private Data outputData;

    private final String TAG = "IIIII";
    private static final String PROGRESS = "PROGRESS";

    public FetchQuestionWorker(@NonNull Context context, @NonNull WorkerParameters workerParams) {
        super(context, workerParams);
    }

    @NonNull
    @Override
    public Result doWork() {
        Data taskData = getInputData();
        quizId = taskData.getString("quiz_id");
        outputData = new Data.Builder()
                .putString("quiz_id", quizId)
                .build();

        final Result[] result = {Result.failure(outputData)};

        CountDownLatch countDownLatch = new CountDownLatch(1);

        setProgress(0);
        setForegroundAsync(createForegroundInfo());

        QuizApiService apiService = ApiClient.getClient(getApplicationContext()).create(QuizApiService.class);
        apiService.fetchAllQuestionData(quizId).enqueue(new Callback<ResponseBody>() {
            @Override
            public void onResponse(@NonNull Call<ResponseBody> call, @NonNull Response<ResponseBody> response) {
                try {
                    setProgress(20);
                    QuizResponseModel quizResponseModel = convertToQuizMainObject(response.body().string());
                    AppDatabase appDatabase = AppDatabase.getAppDatabase(getApplicationContext());
                    QuizEntity quizEntity = new QuizEntity();
                    quizEntity.setId(quizResponseModel.getQuizDataModel().getQuizId());
                    quizEntity.setPassMark((float) quizResponseModel.getQuizDataModel().getPassMark());
                    quizEntity.setQuizSummary(quizResponseModel.getQuizDataModel().getQuizSummaryResponse());
                    appDatabase.quizDao().insert(quizEntity);

                    List<QuestionEntity> questionEntities = new ArrayList<>();
                    List<AnswerEntity> answerEntities = new ArrayList<>();
                    ArrayList<QuestionModel> quizQuestions = new ArrayList<>(quizResponseModel.getQuizDataModel().getQuestionModels());
                    for (QuestionModel questionModel : quizQuestions) {
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

                        for (AnswerModel answerModel : questionModel.getAnswerModels()) {
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

                    appDatabase.answerDao().insertAll(answerEntities);
                    setProgress(50);

                    apiService.downloadQuizFiles(quizId)
                            .enqueue(new Callback<ResponseBody>() {
                                @Override
                                public void onResponse(Call<ResponseBody> call, Response<ResponseBody> response) {
                                    if (response.isSuccessful()) {
                                        try {
                                            File outputFile = null;
                                            String path = null;

                                            File dir = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES), "noon");

                                            if (!dir.exists()) {
                                                dir.mkdirs();
                                                Log.e(TAG, "Directory Created.");
                                                Log.e("path >>", "" + dir.getAbsolutePath());
                                            }
                                            path = dir.getAbsolutePath();

                                            //Create Output file in Main File
                                            outputFile = new File(dir, quizId + ".zip");

                                            //Create New File if not present
                                            if (!outputFile.exists()) {
                                                outputFile.createNewFile();
                                            }

                                            FileOutputStream fos = new FileOutputStream(outputFile);//Get OutputStream for NewFile Location

                                            InputStream is = response.body().byteStream();//Get InputStream for connection

                                            byte[] buffer = new byte[1024];//Set buffer type
                                            int len1 = 0;//init length
                                            while ((len1 = is.read(buffer)) != -1) {
                                                fos.write(buffer, 0, len1);//Write new file
                                            }

                                            //Close all connection after doing task
                                            fos.close();
                                            is.close();

                                            Log.e(TAG, "unZip folder method called");
                                            Log.e(TAG, outputFile.getAbsolutePath());
                                            Log.e(TAG, path);

                                            unzipFolder(outputFile.getAbsolutePath(), path);

                                            setProgress(100);
                                            result[0] = Result.success(outputData);
                                            Toast.makeText(getApplicationContext(), getApplicationContext().getString(R.string.quiz_downloaded), Toast.LENGTH_SHORT)
                                                    .show();
                                            countDownLatch.countDown();
                                        } catch (IOException ioException) {
                                            Log.e(TAG, Objects.requireNonNull(ioException.getMessage()));
                                            setProgress(100);
                                            countDownLatch.countDown();
                                        }
                                    } else {
                                        try {
                                            if (response.errorBody() != null) {
                                                Log.e(TAG, String.valueOf(response.code()));
                                                Log.e(TAG, response.errorBody().string());
                                            }
                                            setProgress(100);
                                            countDownLatch.countDown();
                                        } catch (IOException e) {
                                            e.printStackTrace();
                                            setProgress(100);
                                            countDownLatch.countDown();
                                        }
                                    }
                                }

                                @Override
                                public void onFailure(@NonNull Call<ResponseBody> call, @NonNull Throwable throwable) {
                                    Log.e(TAG, Objects.requireNonNull(throwable.getMessage()));
                                    setProgress(100);
                                    countDownLatch.countDown();
                                }
                            });
                } catch (NullPointerException | IOException e) {
                    Log.e("IIIIII", Objects.requireNonNull(e.getMessage()));
                    Log.e("IIIIII", Objects.requireNonNull(e.getLocalizedMessage()));
                    e.printStackTrace();
                    setProgress(100);
                    countDownLatch.countDown();
                }
            }

            @Override
            public void onFailure(@NonNull Call<ResponseBody> call, @NonNull Throwable throwable) {
                Log.e("IIIII", call.request().toString());
                Log.e("IIIIII", Objects.requireNonNull(throwable.getLocalizedMessage()));
                setProgress(100);
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

    private void unzipFolder(String zipFile, String location) throws IOException {
        try {
            File z1 = new File(zipFile);
            File f = new File(location);
            if (!f.isDirectory()) {
                f.mkdirs();
            }
            ZipInputStream zin = new ZipInputStream(new FileInputStream(zipFile));
            try {
                ZipEntry ze = null;
                while ((ze = zin.getNextEntry()) != null) {
                    String path = location + File.separator + ze.getName();

                    if (ze.isDirectory()) {
                        File unzipFile = new File(path);
                        if (!unzipFile.isDirectory()) {
                            unzipFile.mkdirs();
                        }
                    } else {
                        FileOutputStream fout = new FileOutputStream(path, false);
                        try {
                            for (int c = zin.read(); c != -1; c = zin.read()) {
                                fout.write(c);
                            }
                            zin.closeEntry();
                        } finally {
                            fout.close();
                        }
                    }
                    deleteRecursive(z1);
                }
            } finally {
                zin.close();
            }
        } catch (Exception e) {
            e.printStackTrace();
            Log.e(TAG, "Unzip exception", e);
        }
    }

    private void deleteRecursive(File fileOrDirectory) {
        if (fileOrDirectory.isDirectory()) {
            for (File child : fileOrDirectory.listFiles()) {
                deleteRecursive(child);
                Log.e("deleteRecursive >>", "Zip file deleted.");
            }
        }
        fileOrDirectory.delete();
    }

    private QuizResponseModel convertToQuizMainObject(String jsonString) {
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

    private void setProgress(int progress) {
        setProgressAsync(
                new Data.Builder()
                        .putString("quiz_id", quizId)
                        .putInt(PROGRESS, progress).build()
        );
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
                .setContentTitle(getApplicationContext().getString(R.string.quiz_download_notification))
                .setTicker(getApplicationContext().getString(R.string.quiz_download_notification))
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
}
