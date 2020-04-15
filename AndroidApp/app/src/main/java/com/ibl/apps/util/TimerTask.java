package com.ibl.apps.util;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Handler;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.TimeZone;
import java.util.Timer;

import static android.content.Context.MODE_PRIVATE;

public class TimerTask {
    private Context context;

    public TimerTask(Context context) {
        this.context = context;
    }

    java.util.TimerTask mTimerTask;

    public void doTimerTask() {
        Handler handler = new Handler();
        Timer t = new Timer();
        mTimerTask = new java.util.TimerTask() {
            public void run() {
                handler.post(() -> {
                    SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH);
                    sdf.setTimeZone(TimeZone.getTimeZone("UTC"));
                    String gmtTime = sdf.format(new Date());

                    SharedPreferences sharedPreferences = context.getSharedPreferences("spendtime", MODE_PRIVATE);
                    SharedPreferences.Editor editor = sharedPreferences.edit();

                    SharedPreferences intervalsharedPreferences = context.getSharedPreferences("interval", MODE_PRIVATE);
                    SharedPreferences.Editor intervaleditor = intervalsharedPreferences.edit();

                    if (intervaleditor != null) {
                        intervaleditor.clear();
                        intervaleditor.putBoolean("iscall", true);
                        intervaleditor.apply();

                    }

                    if (editor != null) {
                        editor.clear();
                        editor.putString("totaltime", gmtTime);
//                        Log.e("TotalTime", "===run===" + gmtTime);
                        editor.apply();
                    }
                });
            }
        }

        ;


        // public void schedule (TimerTask task, long delay, long period)
        t.schedule(mTimerTask, 1000, 1000);  //
    }

    public void stopTimerTask() {
        if (mTimerTask != null) {
            mTimerTask.cancel();

            SharedPreferences sharedPreferences = context.getSharedPreferences("interval", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferences.edit();
            editor.putBoolean("iscall", false);
            editor.apply();
        }
    }
}