package com.ibl.apps.QuizModule;

import androidx.appcompat.app.AppCompatActivity;
import androidx.work.Constraints;
import androidx.work.Data;
import androidx.work.NetworkType;
import androidx.work.OneTimeWorkRequest;
import androidx.work.WorkManager;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.ibl.apps.QuizModule.workers.FetchQuestionWorker;
import com.ibl.apps.noon.R;

public class QuizTestActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_quiz_test);

        Button btnQuestionDownloads = findViewById(R.id.btn_question_downloads);
        Button btnFileDownloads = findViewById(R.id.btn_file_downloads);

        btnQuestionDownloads.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Constraints constraints = new Constraints.Builder()
                        .setRequiredNetworkType(NetworkType.CONNECTED)
                        .build();

                Data inputData = new Data.Builder()
                        .putString("quiz_id", "2940")
                        .build();

                OneTimeWorkRequest workRequest = new OneTimeWorkRequest.Builder(FetchQuestionWorker.class)
                        .setConstraints(constraints)
                        .setInputData(inputData)
                        .build();

                WorkManager.getInstance(QuizTestActivity.this).enqueue(workRequest);
            }
        });

        btnFileDownloads.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
//                Constraints constraints = new Constraints.Builder()
//                        .setRequiredNetworkType(NetworkType.CONNECTED)
//                        .build();

//                OneTimeWorkRequest workRequest = new OneTimeWorkRequest.Builder(FetchAssetsWorker.class)
//                        .setConstraints(constraints)
//                        .build();
//
//                WorkManager.getInstance(QuizTestActivity.this).enqueue(workRequest);
            }
        });
    }
}