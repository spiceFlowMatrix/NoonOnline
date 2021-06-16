package com.ibl.apps.Service.TimeOut;

import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

import java.util.Calendar;
import java.util.Date;

public class SyncEventReceiver extends BroadcastReceiver {

    public static void setupAlarm(Context context) {
        try {
            AlarmManager alarmManager = (AlarmManager) context.getSystemService(Context.ALARM_SERVICE);
            Intent intent = new Intent(context, SyncEventReceiver.class);
            PendingIntent alarmIntent = PendingIntent.getBroadcast(context, 0, intent, PendingIntent.FLAG_UPDATE_CURRENT);
            Calendar calendar = Calendar.getInstance();
            calendar.setTimeInMillis(System.currentTimeMillis());
            calendar.add(Calendar.MINUTE, 1000); // first time
            long frequency = 1000 * 60 * 5; // in ms

            alarmManager.setRepeating(AlarmManager.RTC_WAKEUP,
                    getTriggerAt(new Date()),
                    frequency,
                    alarmIntent);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


    private static long getTriggerAt(Date now) {
        Calendar calendar = Calendar.getInstance();
        calendar.setTime(now);
        return calendar.getTimeInMillis();
    }

    @Override
    public void onReceive(Context context, Intent intent) {
        try {
            SyncIntentService.start(context);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
