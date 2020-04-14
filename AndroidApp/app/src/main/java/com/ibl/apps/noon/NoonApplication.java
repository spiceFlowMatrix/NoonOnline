package com.ibl.apps.noon;

import android.arch.lifecycle.Lifecycle;
import android.arch.lifecycle.LifecycleObserver;
import android.arch.lifecycle.OnLifecycleEvent;
import android.arch.lifecycle.ProcessLifecycleOwner;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Handler;
import android.support.multidex.MultiDex;
import android.support.multidex.MultiDexApplication;
import android.support.v7.app.AppCompatDelegate;
import android.util.Log;

import com.crashlytics.android.Crashlytics;
import com.downloader.PRDownloader;
import com.downloader.PRDownloaderConfig;
import com.droidnet.DroidNet;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.Model.SyncTimeTracking;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;
import com.ibl.apps.Service.NetworkChangeReceiver;
import com.ibl.apps.Utils.Const;

import java.io.File;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;
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

        CallApiForSpendApp();
        callApiForInterval();
        //callLogfiles();
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
            syncTimeTrackingObject = AppDatabase.getAppDatabase(getContext()).syncTimeTrackingDao().getSyncTimeTrack(Integer.parseInt(userId));
        }
        if (syncTimeTrackingObject != null) {
            syncTimeTrackingObject.setOuttime(getUTCTime());
            AppDatabase.getAppDatabase(getContext()).syncTimeTrackingDao().updateSyncTimeTracking(syncTimeTrackingObject);
        }
    }

    private void CallApiForSpendApp() {
        try {
            SharedPreferences sharedPreferences = getSharedPreferences("spendtime", MODE_PRIVATE);
            String outtimrsave = sharedPreferences.getString("totaltime", "");

            SharedPreferences sharedPreferences1 = getSharedPreferences("NetworkSpeed", MODE_PRIVATE);
            String networkSpeed = sharedPreferences1.getString("downloadspeed", "");

            SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
            String userId = sharedPreferencesuser.getString("uid", "");

            if (userId != null && !userId.isEmpty()) {
                SyncTimeTrackingObject syncTimeTrackingObject = AppDatabase.getAppDatabase(getContext()).syncTimeTrackingDao().getSyncTimeTrack(Integer.parseInt(userId));
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
                        ApiService apiService = ApiClient.getClient(getContext()).create(ApiService.class);
                        CompositeDisposable disposable = new CompositeDisposable();
                        disposable.add(apiService.getSyncTimeTracking(array).subscribeOn(Schedulers.io())
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
                                            AppDatabase.getAppDatabase(getContext()).syncTimeTrackingDao().updateSyncTimeTracking(syncTimeTrackingObject);
                                        }
                                    }

                                    @Override
                                    public void onError(Throwable e) {

                                    }
                                }));
                    }
                }
            }
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
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