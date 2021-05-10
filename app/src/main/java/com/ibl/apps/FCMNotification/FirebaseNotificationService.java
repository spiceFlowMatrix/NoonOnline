package com.ibl.apps.FCMNotification;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.os.Build;
import androidx.core.app.NotificationCompat;
import androidx.localbroadcastmanager.content.LocalBroadcastManager;
import android.util.Log;

import com.google.firebase.messaging.RemoteMessage;
import com.google.gson.Gson;
import com.ibl.apps.Model.NotiType3Object;
import com.ibl.apps.Model.NotiType4Object;
import com.ibl.apps.Model.NotiType6_7Object;
import com.ibl.apps.Model.NotiType8Object;
import com.ibl.apps.Model.NotiType9Object;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.noon.ChapterActivity;
import com.ibl.apps.noon.MainDashBoardActivity;
import com.ibl.apps.noon.R;

import org.json.JSONObject;

import java.util.Map;


public class FirebaseNotificationService extends com.google.firebase.messaging.FirebaseMessagingService {

    Context context;

    @Override
    public void onNewToken(String token) {
        Log.e(Const.LOG_NOON_TAG, "Refreshed token: " + token);

        PrefUtils.storerefreshedToken(getApplicationContext(), token);
        // If you want to send messages to this application instance or
        // manage this apps subscriptions on the server side, send the
        // Instance ID token to your app server.
        sendRegistrationToServer(token);
    }

    private void sendRegistrationToServer(String token) {
        // TODO: Implement this method to send token to your app server.
    }

    @Override
    public void onMessageReceived(RemoteMessage remoteMessage) {
        super.onMessageReceived(remoteMessage);
        context = getApplicationContext();
        Log.e(Const.LOG_NOON_TAG, "Message data payload: " + remoteMessage.getData());


        // Check if message contains a data payload.
        if (remoteMessage.getData().size() > 0) {
            try {
                Map<String, String> data = remoteMessage.getData();
                String alert = data.get(Const.noti_alert);
                String notificationText = '\u200f' + alert + '\u200f';
                String nPayload = data.get(Const.noti_n);
                JSONObject njsonObject = new JSONObject(nPayload);
                String notiType = njsonObject.get(Const.noti_type).toString();
/*
                if (remoteMessage.getData().size() == 0) {
                    String myjson = "{\"response_code\":0,\"message\":\"Assignment Detail\",\"status\":\"Success\",\"data\":[{\"id\":9,\"name\":\"lesson-1cc\",\"description\":\"sadasd\",\"code\":\"AS-1559885237645\",\"chapterid\":129,\"submissioncount\":0,\"status\":0}]}";



                    Gson gson = new Gson();
                    remoteMessage = gson.fromJson(myjson);
                }*/

                sendNotification(this, notificationText, notiType, nPayload);
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    private void handleNow() {
        Log.e(Const.LOG_NOON_TAG, "Short lived task is done.");
    }

    public static void sendNotification(Context context, String alert, String notiType, String nPayload) {

        Log.e(Const.LOG_NOON_TAG, "==Noti_PAYLOAD==" + nPayload);

        PendingIntent pendingIntent = null;
        Intent intent = new Intent(context, MainDashBoardActivity.class);

        if (notiType.equals("4")) {

            NotiType4Object notiType4Object = new Gson().fromJson(nPayload, NotiType4Object.class);
            intent = new Intent(context, ChapterActivity.class);
            intent.putExtra(Const.isDiscussions, true);
            intent.putExtra(Const.isNotification, true);
            intent.putExtra(Const.GradeID, notiType4Object.getDiscussion().getCourseid());
            intent.putExtra(Const.topicId, notiType4Object.getDiscussion().getId());
            intent.putExtra(Const.topicname, notiType4Object.getDiscussion().getTitle());
            intent.putExtra(Const.isprivate, notiType4Object.getDiscussion().getIsprivate());
            intent.putExtra(Const.CourseName, "");
            intent.putExtra(Const.ActivityFlag, "0");
            intent.putExtra(Const.LessonID, "");
            intent.putExtra(Const.QuizID, "");

        } else if (notiType.equals("6") || notiType.equals("7")) {

            NotiType6_7Object notiType6_7Object = new Gson().fromJson(nPayload, NotiType6_7Object.class);
            intent = new Intent(context, ChapterActivity.class);
            intent.putExtra(Const.isDiscussions, true);
            intent.putExtra(Const.isNotification, true);
            intent.putExtra(Const.GradeID, notiType6_7Object.getDiscussion().getCourseid());
            intent.putExtra(Const.topicId, notiType6_7Object.getDiscussion().getId());
            intent.putExtra(Const.topicname, notiType6_7Object.getDiscussion().getTitle());
            intent.putExtra(Const.isprivate, notiType6_7Object.getDiscussion().getIsprivate());
            intent.putExtra(Const.CourseName, "");
            intent.putExtra(Const.ActivityFlag, "0");
            intent.putExtra(Const.LessonID, "");
            intent.putExtra(Const.QuizID, "");

            Intent intent1 = new Intent(context.getString(R.string.brodcastTAG));
            intent1.putExtra(Const.topicId, notiType6_7Object.getDiscussion().getId());
            LocalBroadcastManager.getInstance(context).sendBroadcast(intent1);

        } else if (notiType.equals("1")) {

            intent = new Intent(context, MainDashBoardActivity.class);
            intent.putExtra(Const.isNotification, true);

        } else if (notiType.equals("3")) {

            NotiType3Object notiType3Object = new Gson().fromJson(nPayload, NotiType3Object.class);
            intent = new Intent(context, ChapterActivity.class);
            intent.putExtra(Const.isNotification, true);
            intent.putExtra(Const.GradeID, notiType3Object.getCourse().getId());
            intent.putExtra(Const.CourseName, notiType3Object.getCourse().getName());
            intent.putExtra(Const.ActivityFlag, "0");
            intent.putExtra(Const.LessonID, notiType3Object.getLessionId());
            intent.putExtra(Const.QuizID, "0");

        } else if (notiType.equals("8")) {

            NotiType8Object notiType8Object = new Gson().fromJson(nPayload, NotiType8Object.class);
            intent = new Intent(context, ChapterActivity.class);
            intent.putExtra(Const.isNotification, true);
            intent.putExtra(Const.GradeID, notiType8Object.getCourse().getId());
            intent.putExtra(Const.CourseName, notiType8Object.getCourse().getName());
            intent.putExtra(Const.ActivityFlag, "0");
            intent.putExtra(Const.LessonID, "0");
            intent.putExtra(Const.QuizID, "0");

        } else if (notiType.equals("9")) {

            NotiType9Object notiType9Object = new Gson().fromJson(nPayload, NotiType9Object.class);
            intent = new Intent(context, ChapterActivity.class);
            intent.putExtra(Const.isNotification, true);
            intent.putExtra(Const.GradeID, notiType9Object.getCourse().getId());
            intent.putExtra(Const.CourseName, notiType9Object.getCourse().getName());
            intent.putExtra(Const.ActivityFlag, "0");
            intent.putExtra(Const.LessonID, "0");
            intent.putExtra(Const.QuizID, notiType9Object.getQuizId());

        } else if (notiType.equals("11") || notiType.equals("12")) {

            NotiType6_7Object notiType6_7Object = new Gson().fromJson(nPayload, NotiType6_7Object.class);
            intent = new Intent(context, MainDashBoardActivity.class);
            intent.putExtra(Const.isDiscussions, true);
            intent.putExtra(Const.isNotification, true);
            intent.putExtra(Const.GradeID, notiType6_7Object.getDiscussion().getCourseid());
            intent.putExtra(Const.topicId, notiType6_7Object.getDiscussion().getId());
            intent.putExtra(Const.topicname, notiType6_7Object.getDiscussion().getTitle());
            intent.putExtra(Const.isprivate, notiType6_7Object.getDiscussion().getIsprivate());
            intent.putExtra(Const.CourseName, "");
            intent.putExtra(Const.ActivityFlag, "0");
            intent.putExtra(Const.LessonID, "");
            intent.putExtra(Const.QuizID, "");

            Intent intent1 = new Intent(context.getString(R.string.brodcastTAG));
            intent1.putExtra(Const.topicId, notiType6_7Object.getDiscussion().getId());
            LocalBroadcastManager.getInstance(context).sendBroadcast(intent1);
        }

        NotificationManager notifManager = null;

        final int NOTIFY_ID = 0; // ID of notification
        String id = context.getString(R.string.notifications_admin_channel_name);
        String title = context.getString(R.string.notifications_admin_channel_description);
        NotificationCompat.Builder builder;

        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
        pendingIntent = PendingIntent.getActivity(context, 0, intent, PendingIntent.FLAG_ONE_SHOT);

        if (notifManager == null) {
            notifManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);
        }
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            int importance = NotificationManager.IMPORTANCE_HIGH;
            NotificationChannel mChannel = notifManager.getNotificationChannel(id);
            if (mChannel == null) {
                mChannel = new NotificationChannel(id, title, importance);
                mChannel.enableVibration(true);
                mChannel.setVibrationPattern(new long[]{100, 200, 300, 400, 500, 400, 300, 200, 400});
                notifManager.createNotificationChannel(mChannel);
            }
            builder = new NotificationCompat.Builder(context, id);
            builder.setContentTitle(context.getString(R.string.app_name))   // required
                    .setSmallIcon(R.drawable.noon_logo)   // required
                    .setContentText(alert) // required
                    .setDefaults(Notification.DEFAULT_ALL)
                    .setAutoCancel(true)
                    .setContentIntent(pendingIntent)
                    .setVibrate(new long[]{100, 200, 300, 400, 500, 400, 300, 200, 400});
        } else {
            builder = new NotificationCompat.Builder(context, id);
            builder.setContentTitle(context.getString(R.string.app_name))   // required
                    .setSmallIcon(R.drawable.noon_logo)   // required
                    .setContentText(alert) // required
                    .setDefaults(Notification.DEFAULT_ALL)
                    .setAutoCancel(true)
                    .setContentIntent(pendingIntent)
                    .setVibrate(new long[]{100, 200, 300, 400, 500, 400, 300, 200, 400})
                    .setPriority(Notification.PRIORITY_HIGH);
        }
        Notification notification = builder.build();
        notifManager.notify(NOTIFY_ID, notification);
    }
}
