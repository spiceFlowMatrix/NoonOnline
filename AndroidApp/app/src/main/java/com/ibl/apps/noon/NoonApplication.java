package com.ibl.apps.noon;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Handler;
import android.text.TextUtils;
import android.util.Log;

import androidx.appcompat.app.AppCompatDelegate;
import androidx.lifecycle.Lifecycle;
import androidx.lifecycle.LifecycleObserver;
import androidx.lifecycle.OnLifecycleEvent;
import androidx.lifecycle.ProcessLifecycleOwner;
import androidx.multidex.MultiDex;
import androidx.multidex.MultiDexApplication;

import com.bumptech.glide.load.HttpException;
import com.crashlytics.android.Crashlytics;
import com.downloader.PRDownloader;
import com.downloader.PRDownloaderConfig;
import com.droidnet.DroidNet;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.LessonManagement.LessonRepository;
import com.ibl.apps.Model.SyncTimeTracking;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.LessonDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.QuizUserResult;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;
import com.ibl.apps.Service.NetworkChangeReceiver;
import com.ibl.apps.UserCredentialsManagement.UserRepository;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Objects;
import java.util.TimeZone;

import io.fabric.sdk.android.Fabric;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;

/**
 * Created by iblinfotech on 28/08/18.
 */
public class NoonApplication extends MultiDexApplication implements LifecycleObserver {

    private static Context context;
    private BroadcastReceiver mNetworkReceiver;
    public static int cacheStatus = 4;
    public static boolean AppTimeTrack = false;
    private LessonDatabaseRepository lessonDatabaseRepository;
    private QuizDatabaseRepository quizDatabaseRepository;

    // This flag should be set to true to enable VectorDrawable support for API < 21
    static {
        AppCompatDelegate.setCompatVectorFromResourcesEnabled(true);
    }

    @Override
    protected void attachBaseContext(Context base) {
        super.attachBaseContext(base);
        MultiDex.install(this);
        ProcessLifecycleOwner.get().getLifecycle().addObserver(new NoonApplication());
        context = this;
    }

    public static Context getContext() {
        return context;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        Fabric.with(this, new Crashlytics());

        //CallApiForSpendApp();
        //callApiForInterval();
        //callLogfiles();
        callApiForCacheEventsInterval();
        lessonDatabaseRepository = new LessonDatabaseRepository();
        quizDatabaseRepository = new QuizDatabaseRepository();

        SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
        String userId = sharedPreferencesuser.getString("uid", "");
        if (!userId.equals("")) {
            List<LessonProgress> lessonProgressList = lessonDatabaseRepository.getAllLessonProgressData(false, userId);
            List<QuizUserResult> quizUserResults = quizDatabaseRepository.getAllQuizuserResult(false, userId);
            callApiProgessSyncAdd(lessonProgressList, quizUserResults, userId);
        }

        // Enabling database for resume support even after the application is killed:
        PRDownloaderConfig config = PRDownloaderConfig.newBuilder()
                .setDatabaseEnabled(true)
                .build();
        PRDownloader.initialize(getContext(), config);
        DroidNet.init(this);
        mNetworkReceiver = new NetworkChangeReceiver();
        registerNetworkBroadcastForNougat();
    }

    private void callApiForInterval() {
        final Handler h = new Handler();
        h.postDelayed(new Runnable() {
            @Override
            public void run() {
                SharedPreferences shared = context.getSharedPreferences("interval", MODE_PRIVATE);
                boolean isbackground = shared.getBoolean("iscall", false);
                if (isbackground) {
                    if (isNetworkAvailable())
                        CallApiForSpendApp();
                    else
                        saveDataOffline();
                }
                h.postDelayed(this, 300000);
            }
        }, 300000);
    }

    public void callApiProgessSyncAdd(List<LessonProgress> lessonProgressList, List<QuizUserResult> quizUserResults, String userId) {
        LessonRepository lessonRepository = new LessonRepository();
        CompositeDisposable disposable = new CompositeDisposable();
        try {
            JsonObject noonAppFullSyncObject = new JsonObject();
            JsonArray lessonProgressArray = PrefUtils.convertToJsonArray(lessonProgressList);
            noonAppFullSyncObject.add(Const.PROGRESSDATA, lessonProgressArray);
            //Log.e(Const.LOG_NOON_TAG, "=====lessonProgressArray===" + lessonProgressArray);

            JsonArray quizResultArray = PrefUtils.convertToJsonArray(quizUserResults);
            noonAppFullSyncObject.add(Const.TIMERDATA, quizResultArray);
            //Log.e(Const.LOG_NOON_TAG, "=====quizResultArray===" + quizResultArray);
//            Log.e(Const.LOG_NOON_TAG, "=====noonAppFullSyncObject===" + noonAppFullSyncObject);


            disposable.add(lessonRepository.ProgessSyncAdd(noonAppFullSyncObject)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<LessonProgress>() {
                        @Override
                        public void onSuccess(LessonProgress lessonProgress) {

                            if (lessonProgressList != null && lessonProgressList.size() != 0) {
                                for (int i = 0; i < lessonProgressList.size(); i++) {
                                    String quizID = lessonProgressList.get(i).getQuizId();
                                    String lessonID = lessonProgressList.get(i).getLessonId();
                                    if (quizID != null && !TextUtils.isEmpty(quizID)) {
                                        lessonDatabaseRepository.updateQuizIdisStatus(quizID, true, userId);
                                    } else {
                                        lessonDatabaseRepository.updateLessonIdisStatus(lessonID, true, userId);
                                    }
                                }
                            }
                            if (quizUserResults != null && quizUserResults.size() != 0) {
                                for (int i = 0; i < quizUserResults.size(); i++) {
                                    String quizID = quizUserResults.get(i).getQuizId();
                                    quizDatabaseRepository.updatelQuizUserResultStatus(true, quizID, userId);
                                }
                            }
                            Log.e(Const.LOG_NOON_TAG, "=====noonAppFullSyncObject==onSuccess=");
                        }

                        @Override
                        public void onError(Throwable e) {
                            HttpException error = (HttpException) e;
                            //LessonProgress lessonProgress = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), LessonProgress.class);
                            if (!userId.equals("")) {
                                SyncAPITable syncAPITable = new SyncAPITable();
                                syncAPITable.setApi_name("ProgressSyncAdd Progressed");
                                syncAPITable.setEndpoint_url("ProgessSync/ProgessSyncAdd");
                                syncAPITable.setParameters(String.valueOf(noonAppFullSyncObject));
                                syncAPITable.setHeaders(PrefUtils.getAuthid(context));
                                syncAPITable.setStatus(getString(R.string.errored_status));
                                syncAPITable.setDescription(e.getMessage());
                                syncAPITable.setCreated_time(getUTCTime());
                                syncAPITable.setUserid(Integer.parseInt(userId));

                                SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
                                syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                NoonApplication.cacheStatus = 2;
                                SharedPreferences sharedPreferencesCache = context.getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                if (editor != null) {
                                    editor.clear();
                                    editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                    editor.apply();
                                }
                            }
                            //Log.e(Const.LOG_NOON_TAG, "==lessonProgress==" + lessonProgress);
                        }
                    }));


        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void callApiForCacheEventsInterval() {
        final Handler h = new Handler();
        h.postDelayed(new Runnable() {
            @Override
            public void run() {
                SharedPreferences shared = context.getSharedPreferences("interval", MODE_PRIVATE);
                boolean isbackground = shared.getBoolean("iscall", false);
                if (isbackground) {
//                    Log.e("isbackground", "run:5 min ");
                    call5minIntervalData();
                }
                h.postDelayed(this, 300000);
            }
        }, 300000);
    }

    private void callLogfiles() {
        Handler handler = new Handler();
        handler.postDelayed(() -> {
            try {
                File filename = new File(Const.destPath + "logfile.txt");
                filename.createNewFile();
                String cmd = "logcat -d -f " + filename.getAbsolutePath();
                Runtime.getRuntime().exec(cmd);
                Log.e("logfile", "onCreate:" + Runtime.getRuntime().exec(cmd));
            } catch (IOException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        }, 1000);
    }

    private void saveDataOffline() {
        CourseDatabaseRepository courseDatabaseRepository = new CourseDatabaseRepository();
        SharedPreferences sharedPreferences = getSharedPreferences("spendtime", MODE_PRIVATE);
        if (sharedPreferences != null) {
            SharedPreferences.Editor editor = sharedPreferences.edit();
            editor.clear();
            editor.apply();
        }
        SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
        String userId = sharedPreferencesuser.getString("uid", "");
        SyncTimeTrackingObject syncTimeTrackingObject = null;
        if (userId != null) {
            syncTimeTrackingObject = courseDatabaseRepository.getSyncTimeTrackById(Integer.parseInt(userId));
        }
        if (syncTimeTrackingObject != null) {
            syncTimeTrackingObject.setOuttime(getUTCTime());
            courseDatabaseRepository.updateSyncTimeTracking(syncTimeTrackingObject);
        }
    }

    public void CallApiForSpendApp() {
        try {
            SharedPreferences sharedPreferences = getSharedPreferences("spendtime", MODE_PRIVATE);
            String outtimrsave = sharedPreferences.getString("totaltime", "");

            SharedPreferences sharedPreferences1 = getSharedPreferences("NetworkSpeed", MODE_PRIVATE);
            String networkSpeed = sharedPreferences1.getString("downloadspeed", "");

            SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
            String userId = sharedPreferencesuser.getString("uid", "");

            CourseDatabaseRepository courseDatabaseRepository = new CourseDatabaseRepository();

            if (userId != null && !userId.isEmpty()) {
                SyncTimeTrackingObject syncTimeTrackingObject = courseDatabaseRepository.getSyncTimeTrackById(Integer.parseInt(userId));
                if (syncTimeTrackingObject != null) {
                    JsonArray array = new JsonArray();
                    JsonObject jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.userid, Integer.valueOf(userId));
                    jsonObject.addProperty(Const.latitude, String.valueOf(syncTimeTrackingObject.getLatitude()));
                    jsonObject.addProperty(Const.longitude, String.valueOf(syncTimeTrackingObject.getLongitude()));
                    jsonObject.addProperty(Const.serviceprovider, syncTimeTrackingObject.getServiceprovider());
                    jsonObject.addProperty("school", "");
                    jsonObject.addProperty("subjectstaken", "");
                    jsonObject.addProperty("grade", "");
                    jsonObject.addProperty(Const.hardwareplatform, syncTimeTrackingObject.getHardwareplatform());
                    jsonObject.addProperty(Const.operatingsystem, syncTimeTrackingObject.getOperatingsystem());
                    jsonObject.addProperty(Const.version, syncTimeTrackingObject.getVersion());
                    jsonObject.addProperty(Const.ISP, getWifiName(getContext()));
                    jsonObject.addProperty(Const.activitytime, syncTimeTrackingObject.getActivitytime());

                    if (syncTimeTrackingObject.getOuttime() != null && !syncTimeTrackingObject.getOuttime().isEmpty())
                        jsonObject.addProperty(Const.outtime, syncTimeTrackingObject.getOuttime());
                    else
                        jsonObject.addProperty(Const.outtime, outtimrsave);

                    jsonObject.addProperty(Const.networkspeed, networkSpeed);
                    array.add(jsonObject);


                    if (outtimrsave != null && ((syncTimeTrackingObject.getOuttime() != null && !syncTimeTrackingObject.getOuttime().isEmpty()) || !outtimrsave.isEmpty())) {
                        UserRepository userRepository = new UserRepository();
                        CompositeDisposable disposable = new CompositeDisposable();
                        disposable.add(userRepository.getSyncTimeTracking(array).subscribeOn(Schedulers.io())
                                .observeOn(AndroidSchedulers.mainThread())
                                .subscribeWith(new DisposableSingleObserver<SyncTimeTracking>() {
                                    @Override
                                    public void onSuccess(SyncTimeTracking syncTimeTracking) {
                                        if (syncTimeTracking != null && syncTimeTracking.getResponse_code().equals("0")) {
                                            syncTimeTrackingObject.setActivitytime(getUTCTime());
                                            syncTimeTrackingObject.setOuttime("");
                                            SharedPreferences sharedPreferences = getSharedPreferences("spendtime", MODE_PRIVATE);
                                            if (sharedPreferences != null) {
                                                SharedPreferences.Editor editor = sharedPreferences.edit();
                                                editor.clear();
                                                editor.apply();
                                            }
                                            courseDatabaseRepository.updateSyncTimeTracking(syncTimeTrackingObject);
                                        }
                                    }

                                    @Override
                                    public void onError(Throwable e) {
                                        try {
                                            if (!userId.equals("")) {
                                                SyncAPITable syncAPITable = new SyncAPITable();
                                                syncAPITable.setApi_name("AppTimeTrack Progressed");
                                                syncAPITable.setEndpoint_url("ProgessSync/AppTimeTrack");
                                                syncAPITable.setParameters(String.valueOf(array));
                                                syncAPITable.setHeaders(PrefUtils.getAuthid(getContext()));
                                                syncAPITable.setStatus(getString(R.string.errored_status));
                                                syncAPITable.setDescription(e.getMessage());
                                                syncAPITable.setCreated_time(getUTCTime());
                                                syncAPITable.setUserid(Integer.parseInt(userId));
                                                SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
                                                syncAPIDatabaseRepository.insertSyncData(syncAPITable);
                                            }
                                        } catch (Exception exception) {
                                            exception.printStackTrace();
                                        }
                                    }
                                }));
                    }
                }
            }
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
    }

    private void call5minIntervalData() {
        try {

            SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
            String userId = sharedPreferencesuser.getString("uid", "");

            SharedPreferences sharedPreferences = getSharedPreferences("spendtime", MODE_PRIVATE);
            String outtimrsave = sharedPreferences.getString("totaltime", "");

            SharedPreferences sharedPreferences1 = getSharedPreferences("NetworkSpeed", MODE_PRIVATE);
            String networkSpeed = sharedPreferences1.getString("downloadspeed", "");

            CourseDatabaseRepository courseDatabaseRepository = new CourseDatabaseRepository();
            SyncTimeTrackingObject syncTimeTrackingObject = courseDatabaseRepository.getSyncTimeTrackById(Integer.parseInt(userId));

            if (syncTimeTrackingObject != null) {
                JsonArray array = new JsonArray();
                JsonObject jsonObject = new JsonObject();
                jsonObject.addProperty(Const.userid, Integer.valueOf(userId));
                jsonObject.addProperty(Const.latitude, String.valueOf(syncTimeTrackingObject.getLatitude()));
                jsonObject.addProperty(Const.longitude, String.valueOf(syncTimeTrackingObject.getLongitude()));
                jsonObject.addProperty(Const.serviceprovider, syncTimeTrackingObject.getServiceprovider());
                jsonObject.addProperty("school", "");
                jsonObject.addProperty("subjectstaken", "");
                jsonObject.addProperty("grade", "");
                jsonObject.addProperty(Const.hardwareplatform, syncTimeTrackingObject.getHardwareplatform());
                jsonObject.addProperty(Const.operatingsystem, syncTimeTrackingObject.getOperatingsystem());
                jsonObject.addProperty(Const.version, syncTimeTrackingObject.getVersion());
                jsonObject.addProperty(Const.ISP, getWifiName(getContext()));
                jsonObject.addProperty(Const.activitytime, syncTimeTrackingObject.getActivitytime());

                if (syncTimeTrackingObject.getOuttime() != null && !syncTimeTrackingObject.getOuttime().isEmpty())
                    jsonObject.addProperty(Const.outtime, syncTimeTrackingObject.getOuttime());
                else
                    jsonObject.addProperty(Const.outtime, outtimrsave);

                jsonObject.addProperty(Const.networkspeed, networkSpeed);
                array.add(jsonObject);

                SyncAPITable syncAPITable = new SyncAPITable();
                if (!userId.equals("") && !outtimrsave.equals("")) {
//                    Log.e("isback", "call5minIntervalData: " + array.toString());
                    syncAPITable.setApi_name("AppTimeTrack Progressed");
                    syncAPITable.setEndpoint_url("ProgessSync/AppTimeTrack");
                    syncAPITable.setParameters(String.valueOf(array));
                    syncAPITable.setHeaders(PrefUtils.getAuthid(getContext()));
                    syncAPITable.setStatus(context.getResources().getString(R.string.pending_status));
                    syncAPITable.setDescription(getContext().getResources().getString(R.string.apptime_track_pending_description));
                    syncAPITable.setCreated_time(getUTCTime());
                    syncAPITable.setUserid(Integer.parseInt(userId));
                    SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
                    syncAPIDatabaseRepository.insertSyncData(syncAPITable);
                    AppTimeTrack = true;
//                    courseDatabaseRepository.updateSyncTimeTracking(syncTimeTrackingObject);

                    syncTimeTrackingObject.setActivitytime(getUTCTime());
                    syncTimeTrackingObject.setOuttime("");

                    SharedPreferences sharedPreferences2 = getSharedPreferences("spendtime", MODE_PRIVATE);
                    if (sharedPreferences2 != null) {
                        SharedPreferences.Editor editor = sharedPreferences2.edit();
                        editor.clear();
                        editor.apply();
                    }
                    courseDatabaseRepository.updateSyncTimeTracking(syncTimeTrackingObject);

                    cacheStatus = 1;
                    SharedPreferences sharedPreferencesCache = context.getSharedPreferences("cacheStatus", MODE_PRIVATE);
                    SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                    if (editor != null) {
                        editor.clear();
                        editor.putString("FlagStatus", String.valueOf(cacheStatus));
                        editor.apply();
                    }
                }
            }

        } catch (JsonSyntaxException exeption) {
            exeption.printStackTrace();
        }
    }

    private boolean isNetworkAvailable() {
        try {
            ConnectivityManager conMgr = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);

            if (conMgr != null) {
                if (Objects.requireNonNull(conMgr.getNetworkInfo(ConnectivityManager.TYPE_MOBILE)).getState() == NetworkInfo.State.CONNECTED
                        || Objects.requireNonNull(conMgr.getNetworkInfo(ConnectivityManager.TYPE_WIFI)).getState() == NetworkInfo.State.CONNECTED) {

                    return true;

                } else if (Objects.requireNonNull(conMgr.getNetworkInfo(ConnectivityManager.TYPE_MOBILE)).getState() == NetworkInfo.State.DISCONNECTED
                        || Objects.requireNonNull(conMgr.getNetworkInfo(ConnectivityManager.TYPE_WIFI)).getState() == NetworkInfo.State.DISCONNECTED) {

                    return false;
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    public String getMacId() {
        WifiManager wifiManager = (WifiManager) getSystemService(Context.WIFI_SERVICE);
        WifiInfo wifiInfo = wifiManager.getConnectionInfo();
        return wifiInfo.getBSSID();
    }

    public String getWifiName(Context context) {
        WifiManager manager = (WifiManager) context.getApplicationContext().getSystemService(Context.WIFI_SERVICE);
        assert manager != null;
        if (manager.isWifiEnabled()) {
            WifiInfo wifiInfo = manager.getConnectionInfo();
            if (wifiInfo != null) {
                NetworkInfo.DetailedState state = WifiInfo.getDetailedStateOf(wifiInfo.getSupplicantState());
                if (state == NetworkInfo.DetailedState.CONNECTED || state == NetworkInfo.DetailedState.OBTAINING_IPADDR) {
                    return wifiInfo.getSSID();
                }
            }
        }
        return null;
    }

    private String getUTCTime() {
        SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH);
        sdf.setTimeZone(TimeZone.getTimeZone("UTC"));
        String gmtTime = sdf.format(new Date());
        return gmtTime;
    }

    @OnLifecycleEvent(Lifecycle.Event.ON_START)
    public void onMoveToForeground() {
    }

    @Override
    public void onLowMemory() {
        super.onLowMemory();
        DroidNet.getInstance().removeAllInternetConnectivityChangeListeners();
    }

    private void registerNetworkBroadcastForNougat() {
        registerReceiver(mNetworkReceiver, new IntentFilter(ConnectivityManager.CONNECTIVITY_ACTION));
    }

}