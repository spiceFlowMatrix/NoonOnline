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
import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.LessonManagement.LessonRepository;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.SyncRecords;
import com.ibl.apps.Model.SyncTimeTracking;
import com.ibl.apps.QuizManament.QuizRepository;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.LessonDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.ChapterProgress;
import com.ibl.apps.RoomDatabase.entity.FileProgress;
import com.ibl.apps.RoomDatabase.entity.LessonNewProgress;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.QuizProgress;
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
import java.util.ArrayList;
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

import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.chapterProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.fileProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.lessonProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.quizProgressList;

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
    public static Handler h = new Handler();
    public static boolean IS_CACHE_TRYING = false;
    public static boolean isDownloadable = false;
    SyncAPIDatabaseRepository syncAPIDatabaseRepository;
    LessonRepository lessonRepository;
    QuizRepository quizRepository;
    private List<SyncAPITable> syncAPITableList = new ArrayList<>();

    // This flag should be set to true to enable VectorDrawable support for API < 21
    static {
        AppCompatDelegate.setCompatVectorFromResourcesEnabled(true);
    }

    private CompositeDisposable disposable = new CompositeDisposable();
    private String userId;

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
        syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
        lessonRepository = new LessonRepository();
        quizRepository = new QuizRepository();
        SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
        userId = sharedPreferencesuser.getString("uid", "");
        callApiForBackgroundInterval();


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
                            //Log.e(Const.LOG_NOON_TAG, "=====noonAppFullSyncObject==onSuccess=");
                        }

                        @Override
                        public void onError(Throwable e) {
                            HttpException error = (HttpException) e;
                            //LessonProgress lessonProgress = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), LessonProgress.class);
                            if (!userId.equals("")) {
                                SyncAPITable syncAPITable = new SyncAPITable();
                                syncAPITable.setApi_name(getString(R.string.sync_progress_add));
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
                                                syncAPITable.setApi_name(getString(R.string.app_time_track_progressed));
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
            if (userId == null && userId.isEmpty())
                return;
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
                    syncAPITable.setApi_name(getString(R.string.app_time_track_progressed));
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


    private void callApiForBackgroundInterval() {
//        isDownloadable = false;
        SharedPreferences sharedPreferences = getSharedPreferences("user", MODE_PRIVATE);
        String SyncUserId = sharedPreferences.getString("uid", "");

        if (SyncUserId.isEmpty())
            return;
        syncAPITableList = syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(SyncUserId));
        final Handler h = new Handler();
        h.postDelayed(new Runnable() {
            @Override
            public void run() {
                SharedPreferences shared = context.getSharedPreferences("interval", MODE_PRIVATE);
                boolean isbackground = shared.getBoolean("iscall", false);
                if (isbackground && isNetworkAvailable() && !isDownloadable) {
                    Log.e("isbackground", "CACHE EVENTS");
                    syncAPITableList = syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(SyncUserId));
                    if (syncAPITableList.size() != 0) {
                        //binding.switchClick.setChecked(true);
                        callSyncADIBackground(0);
                    }
                }
                h.postDelayed(this, 10000);
            }
        }, 10000);
    }

    private void callSyncADIBackground(int position) {

        if (position < 0 || position >= syncAPITableList.size()) {
            return;
        }
        //mProgressDialog.show();
        if (syncAPITableList.get(position).getEndpoint_url().contains("ProgessSync/AppTimeTrack")) {
            JsonArray jsonArray = new Gson().fromJson(syncAPITableList.get(position).getParameters(), JsonArray.class);
            CallApiForSpendApp(jsonArray, position);

            NoonApplication.cacheStatus = 3;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }
//            Log.e("syncAPITableList", "callSyncAPI:--if " + position + " size - " + syncAPITableList.size());

        } else if (syncAPITableList.get(position).getEndpoint_url().contains("LessonProgress/LessonProgressSync")) {
            //            JsonArray jsonArray = new Gson().fromJson(syncAPITableList.get(position).getParameters(), JsonArray.class);
            callApiSyncLessonProgress(lessonProgressList, position);

            NoonApplication.cacheStatus = 3;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }
        } else if (syncAPITableList.get(position).getEndpoint_url().contains("ChapterProgress/ChapterProgressSync")) {
            callApiSyncChapter(chapterProgressList, position);

            NoonApplication.cacheStatus = 3;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }

        } else if (syncAPITableList.get(position).getEndpoint_url().contains("FileProgress/FileProgressSync")) {
            callApiSyncFiles(fileProgressList, position);

            NoonApplication.cacheStatus = 3;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }

        } else if (syncAPITableList.get(position).getEndpoint_url().contains("ProgessSync/ProgessSyncAdd")) {
            SharedPreferences sharedPreferences = getSharedPreferences("user", MODE_PRIVATE);
            String SyncUserId = sharedPreferences.getString("uid", "");
            List<QuizUserResult> quizUserResults = quizDatabaseRepository.getAllQuizuserResult(false, SyncUserId);
            List<LessonProgress> lessonProgressList = lessonDatabaseRepository.getAllLessonProgressData(false, SyncUserId);
            JsonObject jsonArray = new Gson().fromJson(syncAPITableList.get(position).getParameters(), JsonObject.class);
            callApiProgessSyncAdd(lessonProgressList, quizUserResults, position, jsonArray);

            NoonApplication.cacheStatus = 3;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }

        } else if (syncAPITableList.get(position).getEndpoint_url().contains("QuizProgress/QuizProgressSync")) {
            callApiSyncQuiz(quizProgressList, position);

            NoonApplication.cacheStatus = 3;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }

        } else if (syncAPITableList.get(position).getEndpoint_url().contains("ProgessSync/GetSyncRecords")) {
            callApiGetSyncRecords(position);

            NoonApplication.cacheStatus = 3;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }

        } else if (syncAPITableList.get(position).getEndpoint_url().contains("UserQuizResult/UserQuizResultSync")) {
            JsonArray jsonArray = new Gson().fromJson(syncAPITableList.get(position).getParameters(), JsonArray.class);
            getUserQuizResultSync(jsonArray, position);

            NoonApplication.cacheStatus = 3;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }

        }

        if (syncAPITableList.size() == 1) {
            //mProgressDialog.dismiss();
            NoonApplication.cacheStatus = 4;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }
        }
    }

    public void CallApiForSpendApp(JsonArray jsonArray, final int position) {
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

                        disposable.add(userRepository.getSyncTimeTracking(jsonArray).subscribeOn(Schedulers.io())
                                .observeOn(AndroidSchedulers.mainThread())
                                .subscribeWith(new DisposableSingleObserver<SyncTimeTracking>() {
                                    @Override
                                    public void onSuccess(SyncTimeTracking syncTimeTracking) {
                                        if (syncTimeTracking != null && syncTimeTracking.getResponse_code().equals("0")) {
                                            syncAPITableList.remove(position);
                                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                                            //cacheEventsListAdapter.notifyItemRemoved(position);
                                            callSyncADIBackground(position);
                                        }
                                    }

                                    @Override
                                    public void onError(Throwable e) {
                                        NoonApplication.cacheStatus = 2;
                                        SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                        SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                        if (editor != null) {
                                            editor.clear();
                                            editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                            editor.apply();
                                        }
                                        callSyncADIBackground(position + 1);
                                    }
                                }));
                    }
                }
            }
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
    }

    private void callApiSyncLessonProgress(ArrayList<LessonNewProgress> lessonNewProgress,
                                           int position) {
        JsonArray array = new JsonArray();
        if (!lessonNewProgress.isEmpty()) {
            for (int i = 0; i < lessonNewProgress.size(); i++) {
                JsonObject jsonObject = new JsonObject();

                try {
                    // if (!lessonNewProgress.get(i).getProgress().equals("0")) {
                    jsonObject.addProperty("chapterid", Integer.parseInt(lessonNewProgress.get(i).getChapterId()));
                    jsonObject.addProperty("lessonid", Integer.parseInt(lessonNewProgress.get(i).getLessonId()));
                    jsonObject.addProperty("userid", Integer.parseInt(lessonNewProgress.get(i).getUserId()));
                    jsonObject.addProperty("progress", Integer.parseInt(lessonNewProgress.get(i).getProgress()));
                    array.add(jsonObject);
                    // }
                } catch (JsonSyntaxException e) {
                    e.printStackTrace();
                }
            }
        }

        disposable.add(lessonRepository.getLessonProgressSync(array)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                    @Override
                    public void onSuccess(RestResponse restResponse) {
                        if (restResponse.getResponse_code().equals("0")) {
                            //  Log.e("getLessonProgressSync", "onSuccess: " + array.get(0).getAsString());
                            lessonProgressList.clear();
                            syncAPITableList.remove(position);
                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                            //cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncADIBackground(position);
                        }

                    }

                    @Override
                    public void onError(Throwable e) {
                        NoonApplication.cacheStatus = 2;
                        SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                        if (editor != null) {
                            editor.clear();
                            editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                            editor.apply();
                        }
                        Log.e("onError", "===callApiSyncLessonProgress===: " + e.getMessage());
                        try {
                            callSyncADIBackground(position + 1);
                        } catch (JsonSyntaxException exeption) {
                            exeption.printStackTrace();
                        }

                    }
                }));
    }

    private void callApiSyncChapter(ArrayList<ChapterProgress> chapterprogress, int position) {
        JsonArray array = new JsonArray();
        if (!chapterprogress.isEmpty()) {
            for (int i = 0; i < chapterprogress.size(); i++) {
                JsonObject jsonObject = new JsonObject();
                try {
                    jsonObject.addProperty("courseid", Integer.parseInt(chapterprogress.get(i).getCourseId()));
                    jsonObject.addProperty("chapterid", Integer.parseInt(chapterprogress.get(i).getChapterId()));
                    jsonObject.addProperty("userid", Integer.parseInt(chapterprogress.get(i).getUserId()));
                    jsonObject.addProperty("progress", Integer.parseInt(chapterprogress.get(i).getProgress()));
                    array.add(jsonObject);
                } catch (JsonSyntaxException e) {
                    e.printStackTrace();
                }
            }

        }

        disposable.add(lessonRepository.getChapterProgressSync(array).subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                    @Override
                    public void onSuccess(RestResponse restResponse) {
                        if (restResponse.getResponse_code().equals("0")) {
                            chapterProgressList.clear();
                            syncAPITableList.remove(position);
                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                            //cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncADIBackground(position);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        Log.e("onError", "===callApiSyncChapter===: " + e.getMessage());
                        try {
                            NoonApplication.cacheStatus = 2;
                            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                            if (editor != null) {
                                editor.clear();
                                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                editor.apply();
                            }
                            callSyncADIBackground(position + 1);
                        } catch (JsonSyntaxException exeption) {
                            exeption.printStackTrace();
                        }
                    }
                }));
    }

    private void callApiSyncFiles(ArrayList<FileProgress> fileProgressList, int position) {

        JsonArray array = new JsonArray();
        if (!fileProgressList.isEmpty()) {
            for (int i = 0; i < fileProgressList.size(); i++) {
                JsonObject jsonObject = new JsonObject();

                try {
                    jsonObject.addProperty("lessonid", Integer.parseInt(fileProgressList.get(i).getLessonId()));
                    jsonObject.addProperty("fileid", Integer.parseInt(fileProgressList.get(i).getFileId()));
                    jsonObject.addProperty("userid", Integer.parseInt(fileProgressList.get(i).getUserId()));
                    jsonObject.addProperty("progress", Integer.parseInt(fileProgressList.get(i).getProgress()));
                    array.add(jsonObject);
                } catch (JsonSyntaxException e) {
                    e.printStackTrace();
                }
            }
        }

        disposable.add(lessonRepository.getFileProgressSync(array).subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                    @Override
                    public void onSuccess(RestResponse restResponse) {
                        if (restResponse.getResponse_code().equals("0")) {
                            fileProgressList.clear();
                            syncAPITableList.remove(position);
                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                            //cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncADIBackground(position);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        NoonApplication.cacheStatus = 2;
                        SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                        if (editor != null) {
                            editor.clear();
                            editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                            editor.apply();
                        }
                        Log.e("onError", "====callApiSyncFiles=====: " + e.getMessage());
                        try {
                            callSyncADIBackground(position + 1);
                        } catch (JsonSyntaxException exeption) {
                            exeption.printStackTrace();
                        }

                    }
                }));
    }

    private void callApiSyncQuiz(ArrayList<QuizProgress> quizProgress, int position) {
        JsonArray array = new JsonArray();
        if (!quizProgress.isEmpty()) {
            for (int i = 0; i < quizProgress.size(); i++) {
                JsonObject jsonObject = new JsonObject();

                try {
                    jsonObject.addProperty("chapterid", Integer.parseInt(quizProgress.get(0).getChapterId()));
                    jsonObject.addProperty("quizid", Integer.parseInt(quizProgress.get(0).getQuizId()));
                    jsonObject.addProperty("userid", Integer.parseInt(quizProgress.get(0).getUserId()));
                    jsonObject.addProperty("progress", Integer.parseInt(quizProgress.get(0).getProgress()));
                    array.add(jsonObject);
                } catch (JsonSyntaxException e) {
                    e.printStackTrace();
                }
            }
        }

        disposable.add(quizRepository.getQuizProgressSync(array).subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                    @Override
                    public void onSuccess(RestResponse restResponse) {
                        if (restResponse.getResponse_code().equals("0")) {
                            quizProgressList.clear();
                            syncAPITableList.remove(position);
                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                            //cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncADIBackground(position);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        NoonApplication.cacheStatus = 2;
                        SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                        if (editor != null) {
                            editor.clear();
                            editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                            editor.apply();
                        }
                        Log.e("onError", "====callApiSyncQuiz====: " + e.getMessage());
                        try {
                            callSyncADIBackground(position + 1);
                        } catch (JsonSyntaxException exeption) {
                            exeption.printStackTrace();
                        }
                    }
                }));
    }

    //hold
    private void getUserQuizResultSync(JsonArray quizUserResult, int position) {
        disposable.add(quizRepository.getUserQuizResultSync(quizUserResult).subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                    @Override
                    public void onSuccess(RestResponse restresponse) {
                        if (restresponse.getResponse_code().equals("0")) {
                            Log.e("onSuccess", "onSuccess: " + restresponse.toString());
                            syncAPITableList.remove(position);
                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                            //cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncADIBackground(position);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        NoonApplication.cacheStatus = 2;
                        SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                        if (editor != null) {
                            editor.clear();
                            editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                            editor.apply();
                        }
                        callSyncADIBackground(position + 1);
                        Log.e("onError", "onError: " + e.getMessage());
                    }
                }));
    }

    public void callApiGetSyncRecords(int position) {
        UserRepository userRepository = new UserRepository();
        disposable.add(userRepository.GetSyncRecords()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<SyncRecords>() {
                    @Override
                    public void onSuccess(SyncRecords syncRecords) {

                        try {
                            if (syncRecords.getData() != null) {
                                SyncRecords.Data syncData = syncRecords.getData();

                                if (syncData != null) {
                                    //Log.e(Const.LOG_NOON_TAG, "===getProgressdata===" + syncData.getProgressdata());
                                    //Log.e(Const.LOG_NOON_TAG, "===getTimerdata===" + syncData.getTimerdata());
                                    LessonDatabaseRepository lessonDatabaseRepository = new LessonDatabaseRepository();
                                    if (syncData.getProgressdata() != null && syncData.getProgressdata().size() != 0) {
                                        for (int i = 0; i < syncData.getProgressdata().size(); i++) {
                                            LessonProgress lessonProgress = lessonDatabaseRepository.getItemProgressData(syncData.getProgressdata().get(i).getLessonProgressId(), userId);
                                            if (lessonProgress != null) {
                                                lessonDatabaseRepository.updateLessonUserIdWise(syncData.getProgressdata().get(i).getLessonId(),
                                                        syncData.getProgressdata().get(i).getLessonProgress().split("\\.")[0],
                                                        syncData.getProgressdata().get(i).getGradeId(),
                                                        syncData.getProgressdata().get(i).getUserId(),
                                                        syncData.getProgressdata().get(i).getTotalRecords(),
                                                        syncData.getProgressdata().get(i).getQuizId(),
                                                        true,
                                                        syncData.getProgressdata().get(i).getFileId(),
                                                        syncData.getProgressdata().get(i).getLessonProgressId());

                                            } else {
                                                lessonProgress = new LessonProgress();
                                                lessonProgress.setUserId(syncData.getProgressdata().get(i).getUserId());
                                                lessonProgress.setLessonProgress(syncData.getProgressdata().get(i).getLessonProgress().split("\\.")[0]);
                                                lessonProgress.setLessonId(syncData.getProgressdata().get(i).getLessonId());
                                                lessonProgress.setQuizId(syncData.getProgressdata().get(i).getQuizId());
                                                lessonProgress.setStatus(true);
                                                lessonProgress.setGradeId(syncData.getProgressdata().get(i).getGradeId());
                                                lessonProgress.setTotalRecords(syncData.getProgressdata().get(i).getTotalRecords());
                                                lessonProgress.setFileId(syncData.getProgressdata().get(i).getFileId());
                                                lessonDatabaseRepository.insertLessonProgressData(lessonProgress);
                                            }
                                        }
                                    }
                                    if (syncData.getTimerdata() != null && syncData.getTimerdata().size() != 0) {
                                        for (int i = 0; i < syncData.getTimerdata().size(); i++) {
                                            QuizUserResult quizUserResult = new QuizUserResult();
                                            quizUserResult.setUserId(syncData.getTimerdata().get(i).getUserId());
                                            quizUserResult.setStatus(true);
                                            quizUserResult.setYourScore(syncData.getTimerdata().get(i).getYourScore());
                                            quizUserResult.setPassingScore(syncData.getTimerdata().get(i).getPassingScore());
                                            quizUserResult.setQuizTime(syncData.getTimerdata().get(i).getQuizTime());
                                            quizUserResult.setQuizId(syncData.getTimerdata().get(i).getQuizId());
                                            quizDatabaseRepository.insertAllQuizUserResult(quizUserResult);
                                        }
                                    }
                                }
                                syncAPITableList.remove(position);
                                syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                                //cacheEventsListAdapter.notifyItemRemoved(position);
                                callSyncADIBackground(position);
                            }

                        } catch (Exception e) {
                            //showError(e);
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR===3333" + e.getMessage());
                        }

                    }

                    @Override
                    public void onError(Throwable e) {
                        try {
                            NoonApplication.cacheStatus = 2;
                            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                            if (editor != null) {
                                editor.clear();
                                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                editor.apply();
                            }

                            callSyncADIBackground(position + 1);
                            retrofit2.HttpException error = (retrofit2.HttpException) e;
                            SyncRecords syncRecords = new Gson().fromJson(error.response().errorBody().string(), SyncRecords.class);
                            //Log.e(Const.LOG_NOON_TAG, "===SyncRecords=ERROR==" + syncRecords.getMessage());
                        } catch (Exception e1) {
                            //showError(e1);
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR===44444" + e.getMessage());
                        }
                    }
                }));
    }

    public void callApiProgessSyncAdd
            (List<LessonProgress> lessonProgressList, List<QuizUserResult> quizUserResults,
             int position, JsonObject jsonArray) {

        try {
            JsonObject noonAppFullSyncObject = new JsonObject();
            JsonArray lessonProgressArray = PrefUtils.convertToJsonArray(lessonProgressList);
            noonAppFullSyncObject.add(Const.PROGRESSDATA, lessonProgressArray);
            //Log.e(Const.LOG_NOON_TAG, "=====lessonProgressArray===" + lessonProgressArray);

            JsonArray quizResultArray = PrefUtils.convertToJsonArray(quizUserResults);
            noonAppFullSyncObject.add(Const.TIMERDATA, quizResultArray);
            //Log.e(Const.LOG_NOON_TAG, "=====quizResultArray===" + quizResultArray);
            Log.e(Const.LOG_NOON_TAG, "=====noonAppFullSyncObject===" + noonAppFullSyncObject);
            LessonRepository lessonRepository = new LessonRepository();
            disposable.add(lessonRepository.ProgessSyncAdd(jsonArray)
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
                            QuizDatabaseRepository quizDatabaseRepository = new QuizDatabaseRepository();
                            if (quizUserResults != null && quizUserResults.size() != 0) {
                                for (int i = 0; i < quizUserResults.size(); i++) {
                                    String quizID = quizUserResults.get(i).getQuizId();
                                    quizDatabaseRepository.updatelQuizUserResultStatus(true, quizID, userId);
                                }
                            }
                            syncAPITableList.remove(position);
                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                            //cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncADIBackground(position);
                        }

                        @Override
                        public void onError(Throwable e) {
                            try {
                                NoonApplication.cacheStatus = 2;
                                SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                if (editor != null) {
                                    editor.clear();
                                    editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                    editor.apply();
                                }

                                callSyncADIBackground(position + 1);
                                retrofit2.HttpException error = (retrofit2.HttpException) e;
                                LessonProgress lessonProgress = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), LessonProgress.class);
                                //Log.e(Const.LOG_NOON_TAG, "==lessonProgress==" + lessonProgress);
                            } catch (Exception e1) {
                                e1.printStackTrace();

                            }
                        }
                    }));


        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}