package com.ibl.apps.Service;

import android.content.Context;
import android.content.Intent;
import android.support.annotation.NonNull;
import android.support.v4.app.JobIntentService;
import android.util.Log;

import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.CourseImageTable;

import java.io.BufferedInputStream;
import java.io.InputStream;
import java.net.URL;
import java.net.URLConnection;

import cz.msebera.android.httpclient.util.ByteArrayBuffer;
import cz.msebera.android.httpclient.util.TextUtils;

/**
 * Created by TangChao on 2018/2/11.
 */

public class CourseImageManager extends JobIntentService {

    public static CourseObject apiCourseObject;
    public static String userId;
    private static final String ACTION_START = "ACTION_START";
    static final int JOB_ID = 1001;

    public static void start(Context context, CourseObject CourseObject, String newuserId) {
        apiCourseObject = CourseObject;
        userId = newuserId;
        Intent starter = new Intent(context, CourseImageManager.class);
        CourseImageManager.enqueueWork(context, starter);
    }

    public static void enqueueWork(Context context, Intent intent) {
        intent.setAction(ACTION_START);
        enqueueWork(context, CourseImageManager.class, JOB_ID, intent);
    }

    @Override
    protected void onHandleWork(@NonNull Intent intent) {
        Context context = getApplicationContext();
        try {
            String action = intent.getAction();
            if (ACTION_START.equals(action)) {

                if (apiCourseObject != null && apiCourseObject.getData() != null && apiCourseObject.getData().size() != 0) {
                    for (int j = 0; j < apiCourseObject.getData().size(); j++) {
                        for (int k = 0; k < apiCourseObject.getData().get(j).getCourses().size(); k++) {

                            if (!TextUtils.isEmpty(apiCourseObject.getData().get(j).getCourses().get(k).getImage())) {
                                byte[] bitmapImage = getByteArrayImage(apiCourseObject.getData().get(j).getCourses().get(k).getImage());
                                if (bitmapImage != null) {
                                    //Log.e(Const.LOG_NOON_TAG, "=====BITMAPIMAGE==00=" + compressedArray);
                                    CourseImageTable courseImageTable = new CourseImageTable();
                                    courseImageTable.setGradeId(apiCourseObject.getData().get(j).getCourses().get(k).getId());
                                    courseImageTable.setUserId(userId);
                                    courseImageTable.setCourseImage(bitmapImage);
                                    AppDatabase.getAppDatabase(context).courseDao().insertAll(courseImageTable);
                                }
                            }
                        }
                    }
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private static byte[] getByteArrayImage(String url) {
        try {
            URL imageUrl = new URL(url);
            URLConnection ucon = imageUrl.openConnection();

            InputStream is = ucon.getInputStream();
            BufferedInputStream bis = new BufferedInputStream(is);

            ByteArrayBuffer baf = new ByteArrayBuffer(50000);
            int current = 0;
            while ((current = bis.read()) != -1) {
                baf.append((byte) current);
            }

            return baf.toByteArray();

        } catch (Exception e) {
            Log.e("ImageManager", "Error: " + e.toString());
        }
        return null;
    }
}
