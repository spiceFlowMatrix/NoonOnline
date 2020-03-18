package com.ibl.apps.Utils;

import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.res.Resources;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.TaskStackBuilder;

import com.ibl.apps.noon.LoginActivity;
import com.ibl.apps.noon.MainDashBoardActivity;
import com.ibl.apps.noon.R;

/**
 * Created by ravi on 20/02/18.
 */

public class NotificationUtils {

    public static void showReminderNotification(Context context) {

        //Log.e(Const.LOG_NOON_TAG, "==== NOTIFICATION=====0000===");
        PendingIntent pi = PendingIntent.getActivity(context, 0, new Intent(context, MainDashBoardActivity.class), 0);
        NotificationManager notificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);

        int notificationId = 1;
        String channelId = "channel-01";
        String channelName = "Channel Name";
        int importance = NotificationManager.IMPORTANCE_HIGH;

        if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.O) {
            NotificationChannel mChannel = new NotificationChannel(
                    channelId, channelName, importance);
            notificationManager.createNotificationChannel(mChannel);
        }
        Resources resources = context.getResources();
        NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(context, channelId)
                .setSmallIcon(R.drawable.noon_logo)
                .setContentTitle(resources.getString(R.string.app_name))
                .setContentText(resources.getString(R.string.error_reminder_login));

        TaskStackBuilder stackBuilder = TaskStackBuilder.create(context);
        stackBuilder.addNextIntent(new Intent(context, LoginActivity.class));
        PendingIntent resultPendingIntent = stackBuilder.getPendingIntent(
                0,
                PendingIntent.FLAG_UPDATE_CURRENT
        );
        mBuilder.setContentIntent(resultPendingIntent);

        notificationManager.notify(notificationId, mBuilder.build());

    }
}
