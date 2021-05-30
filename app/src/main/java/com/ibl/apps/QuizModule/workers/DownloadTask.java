
package com.ibl.apps.QuizModule.workers;

import android.annotation.SuppressLint;
import android.content.Context;
import android.os.AsyncTask;
import android.os.Environment;
import android.os.Handler;
import android.util.Log;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

public class DownloadTask {

    private static final String TAG = "MMMMMM";
    private String downloadUrl = "https://dari-staging-apps.noon-online.com/api/v1/CompleteQuizFiles/";
    private String downloadFileName = "";

    public DownloadTask(Context context, String quizId) {
        this.downloadUrl = this.downloadUrl+quizId;
        this.downloadFileName = quizId+".zip";
        //Start Downloading Task
        new DownloadingTask().execute();
    }

    @SuppressLint("StaticFieldLeak")
    private class DownloadingTask extends AsyncTask<Void, Void, Void> {

        File outputFile = null;
        String path = null;

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            Log.e("onPreExecute", "Download started");
        }

        @Override
        protected Void doInBackground(Void... arg0) {
            try {
                URL url = new URL(downloadUrl);//Create Download URl
                HttpURLConnection c = (HttpURLConnection) url.openConnection();//Open Url Connection
                c.setRequestMethod("GET");//Set Request Method to "GET" since we are grtting data
                c.connect();//connect the URL Connection

                //If Connection response is not OK then show Logs
                if (c.getResponseCode() != HttpURLConnection.HTTP_OK) {
                    Log.e(TAG, "Server returned HTTP " + c.getResponseCode()
                            + " " + c.getResponseMessage());
                }

//                File dir = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS) + "/noon");
//                File dir = new File(Environment.getExternalStorageDirectory() + "/noon");
                File dir = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES), "noon");

                if (!dir.exists()) {
                    dir.mkdirs();
                    Log.e(TAG, "Directory Created.");
                    Log.e("path >>", "" + dir.getAbsolutePath());
                }

                path = dir.getAbsolutePath();

                //Create Output file in Main File
                outputFile = new File(dir, downloadFileName);

                //Create New File if not present
                if (!outputFile.exists()) {
                    outputFile.createNewFile();
                }

                FileOutputStream fos = new FileOutputStream(outputFile);//Get OutputStream for NewFile Location

                InputStream is = c.getInputStream();//Get InputStream for connection

                byte[] buffer = new byte[1024];//Set buffer type
                int len1 = 0;//init length
                while ((len1 = is.read(buffer)) != -1) {
                    fos.write(buffer, 0, len1);//Write new file
                }

                //Close all connection after doing task
                fos.close();
                is.close();
            } catch (Exception e) {
                e.printStackTrace();
                outputFile = null;
                Log.e(TAG, "Download Error Exception " + e.getMessage());
            }
            return null;
        }

        @Override
        protected void onPostExecute(Void result) {
            try {
                if (outputFile != null) {
                    unzipFolder(outputFile.getAbsolutePath(), path);
                    Log.e("onPostExecute", "Download completed"+" >>> "+outputFile.getAbsolutePath()+" >>> "+path);
                } else {
                    Log.e("onPostExecute", "Download failed");
                    new Handler().postDelayed(new Runnable() {
                        @Override
                        public void run() {
                            Log.e("onPostExecute", "Download again");
                        }
                    }, 3000);
                    Log.e(TAG, "Download Failed");
                }
            } catch (Exception e) {
                e.printStackTrace();
                new Handler().postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        Log.e("onPostExecute", "Download again");
                    }
                }, 3000);
                Log.e(TAG, "Download Failed with Exception - " + e.getLocalizedMessage());
            }
            super.onPostExecute(result);
        }
    }

    public void unzipFolder(String zipFile, String location) throws IOException {
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

    public void deleteRecursive(File fileOrDirectory) {
        if (fileOrDirectory.isDirectory()) {
            for (File child : fileOrDirectory.listFiles()) {
                deleteRecursive(child);
                Log.e("deleteRecursive >>","Zip file deleted.");
            }
        }
        fileOrDirectory.delete();
    }
}
