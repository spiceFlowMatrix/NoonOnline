package com.ibl.apps.Service;

import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.util.Log;

import androidx.annotation.NonNull;
import androidx.core.app.JobIntentService;

import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.RoomDatabase.dao.libraryManagementDatabase.LibraryDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.BookImageTable;
import com.ibl.apps.noon.R;

import java.io.BufferedInputStream;
import java.io.ByteArrayOutputStream;
import java.io.InputStream;
import java.net.URL;
import java.net.URLConnection;

import cz.msebera.android.httpclient.util.ByteArrayBuffer;
import cz.msebera.android.httpclient.util.TextUtils;

/**
 * Created by TangChao on 2018/2/11.
 */

public class BookImageManager extends JobIntentService {

    public static LibraryObject libraryObject;
    public static String userId;
    private static final String ACTION_START = "ACTION_START";
    static final int JOB_ID = 1002;

    public static void start(Context context, LibraryObject CourseObject, String newuserId) {
        libraryObject = CourseObject;
        userId = newuserId;
        Intent starter = new Intent(context, BookImageManager.class);
        BookImageManager.enqueueWork(context, starter);

    }

    public static void enqueueWork(Context context, Intent intent) {
        intent.setAction(ACTION_START);
        enqueueWork(context, BookImageManager.class, JOB_ID, intent);
    }

    @Override
    protected void onHandleWork(@NonNull Intent intent) {
        Context context = getApplicationContext();
        try {
            String action = intent.getAction();
            if (ACTION_START.equals(action)) {

                if (libraryObject != null && libraryObject.getData() != null && libraryObject.getData().size() != 0) {
                    for (int j = 0; j < libraryObject.getData().size(); j++) {

                        if (!TextUtils.isEmpty(libraryObject.getData().get(j).getBookcoverimage())) {
                            byte[] bitmapImage = getByteArrayImage(libraryObject.getData().get(j).getBookcoverimage());

                            //Log.e(Const.LOG_NOON_TAG, "=====BITMAPIMAGE===" + bitmapImage);
                            LibraryDatabaseRepository libraryDatabaseRepository = new LibraryDatabaseRepository();
                            if (bitmapImage != null) {
                                BookImageTable bookImageTable = new BookImageTable();
                                bookImageTable.setBookId(libraryObject.getData().get(j).getId());
                                bookImageTable.setUserId(userId);
                                bookImageTable.setBookImage(bitmapImage);
                                if (libraryDatabaseRepository.getLibraryBookUserId(userId, bookImageTable.getBookId()) == null) {
                                    libraryDatabaseRepository.insertBookImageTable(bookImageTable);
                                }
                            } else {
                                Bitmap myLogo = BitmapFactory.decodeResource(context.getResources(), R.drawable.noon_logo);
                                ByteArrayOutputStream stream = new ByteArrayOutputStream();
                                myLogo.compress(Bitmap.CompressFormat.JPEG, 100, stream);
                                byte[] bitmapdata = stream.toByteArray();

                                BookImageTable bookImageTable = new BookImageTable();
                                bookImageTable.setBookId(libraryObject.getData().get(j).getId());
                                bookImageTable.setUserId(userId);
                                bookImageTable.setBookImage(bitmapdata);
                                if (libraryDatabaseRepository.getLibraryBookUserId(userId, bookImageTable.getBookId()) == null) {
                                    libraryDatabaseRepository.insertBookImageTable(bookImageTable);
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

            ByteArrayBuffer baf = new ByteArrayBuffer(5000);
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
