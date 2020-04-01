package com.ibl.apps.util;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.text.TextUtils;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonArray;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.ReminderObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.NoonApplication;

/**
 * Created by ravi on 20/02/18.
 */

public class PrefUtils {

    public PrefUtils() {

    }

    private static SharedPreferences getSharedPreferences(Context context) {
        return context.getSharedPreferences(Const.PREF_NAME, Context.MODE_PRIVATE);
    }

    public static void clearSharedPreferences(Context context) {
        SharedPreferences preferences = context.getSharedPreferences(Const.PREF_NAME, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = preferences.edit();
        editor.clear();
        editor.commit();
    }

    public static JsonArray convertToJsonArray(Object arrayList) {
        Gson gson = new GsonBuilder().create();
        JsonArray myCustomArray = gson.toJsonTree(arrayList).getAsJsonArray();
        return myCustomArray;
    }

    public static void storeReminderTokenKey(Context context, ReminderObject reminderObject) {
        SharedPreferences.Editor editor = getSharedPreferences(context).edit();
        Gson gson = new Gson();
        String json = gson.toJson(reminderObject);
        editor.putString(Const.PREF_REMINDER_DATA, json);
        editor.commit();
    }

    public static ReminderObject getReminderTokenKey(Context context) {
        Gson gson = new Gson();
        String json = getSharedPreferences(context).getString(Const.PREF_REMINDER_DATA, null);
        ReminderObject reminderObject = gson.fromJson(json, ReminderObject.class);
        return reminderObject;
    }

    public static void storeAuthid(Context context, String userid) {
        SharedPreferences.Editor editor = getSharedPreferences(context).edit();
        editor.putString(Const.PREF_AUTH_ID, userid);
        // Log.e("AUTH0", "storeAuthid: " + userid);
        editor.commit();
    }

    public static String getAuthid(Context context) {
        // Log.e("AUTH0", "getAuthid: " + getSharedPreferences(context).getString(Const.PREF_AUTH_ID, null));
        return getSharedPreferences(context).getString(Const.PREF_AUTH_ID, null);
    }

    public static void storerefreshedToken(Context context, String refreshedToken) {
        SharedPreferences.Editor editor = getSharedPreferences(context).edit();
        editor.putString(Const.PREF_refreshedToken, refreshedToken);
        editor.commit();
    }

    public static String getrefreshedToken(Context context) {
        return getSharedPreferences(context).getString(Const.PREF_refreshedToken, null);
    }


    public static void setDownloadAlertFirstTime(Context context, Boolean isFirst) {
        SharedPreferences.Editor editor = getSharedPreferences(context).edit();
        editor.putBoolean(Const.DOWNLOAD_ALERT, isFirst);
        editor.commit();
    }

    public static Boolean isDownloadAlertFirstTime(Context context) {
        return getSharedPreferences(context).getBoolean(Const.DOWNLOAD_ALERT, true);
    }

    public static class MyAsyncTask extends AsyncTask<Void, Void, UserDetails> {

        public AsyncResponse delegate = null;

        public interface AsyncResponse {
            UserDetails getLocalUserDetails(UserDetails userDetails);
        }

        public MyAsyncTask(AsyncResponse delegate) {
            this.delegate = delegate;
        }

        @Override
        protected UserDetails doInBackground(Void... voids) {
            UserDetails userDetails = null;
            try {

                String authid = PrefUtils.getAuthid(NoonApplication.getContext());
                if (!TextUtils.isEmpty(authid)) {
                    AuthTokenObject authTokenObject = AppDatabase.getAppDatabase(NoonApplication.getContext()).authTokenDao().getauthTokenData(authid);

                    if (authTokenObject != null) {
                        String sub = "";
                        if (authTokenObject.getSub() != null) {
                            sub = authTokenObject.getSub();
                            userDetails = AppDatabase.getAppDatabase(NoonApplication.getContext()).userDetailDao().getUserDetials(sub);
                            //Log.e(Const.LOG_NOON_TAG, "===userDetails==" + userDetails);
                        }
                    }
                }

            } catch (Exception e) {
                e.printStackTrace();
            }
            return userDetails;
        }

        @Override
        protected void onPostExecute(UserDetails result) {
            delegate.getLocalUserDetails(result);
        }
    }

}



