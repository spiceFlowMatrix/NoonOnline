package com.ibl.apps.noon;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.NetworkInfo;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.CompoundButton;

import androidx.appcompat.app.AlertDialog;
import androidx.databinding.DataBindingUtil;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.Adapter.CacheEventsListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.SyncTimeTracking;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;
import com.ibl.apps.UserCredentialsManagement.UserRepository;
import com.ibl.apps.noon.databinding.ActivityCatchEventsListBinding;
import com.ibl.apps.noon.databinding.HitLimitDialogBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;

public class CacheEventsListActivity extends BaseActivity {
    ActivityCatchEventsListBinding binding;
    CacheEventsListAdapter cacheEventsListAdapter;
    boolean isClick = false;
    private SyncAPIDatabaseRepository syncAPIDatabaseRepository;
    private String userId;
    private List<SyncAPITable> syncAPITableList = new ArrayList<>();
    private SyncAPITable model = new SyncAPITable();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //setContentView(R.layout.activity_catch_events_list);
    }

    @Override
    protected int getContentView() {
        return R.layout.activity_catch_events_list;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivityCatchEventsListBinding) getBindObj();
        setToolbar(binding.toolBar);
        showBackArrow("Pending Cache Events");
        SharedPreferences sharedPreferences = getSharedPreferences("user", MODE_PRIVATE);
        userId = sharedPreferences.getString("uid", "");
        syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
        binding.toolBar.setNavigationOnClickListener(view -> {
            finish();
        });

        syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId));
        syncAPITableList = syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId));
        for (int i = 0; i < syncAPITableList.size(); i++) {
            model = syncAPITableList.get(i);
        }
        Log.e("CacheEventsListActivity", "onViewReady: " + syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).toString());
        Log.e("CacheEventsListActivity", "onViewReady:size : = " + syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size());
        if (syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)) != null && syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size() != 0) {
            binding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
            binding.txtEmptyEvents.setVisibility(View.GONE);
            cacheEventsListAdapter = new CacheEventsListAdapter(CacheEventsListActivity.this, syncAPITableList);
            binding.rcVerticalLayout.rcVertical.setAdapter(cacheEventsListAdapter);
//            cacheEventsListAdapter.updateData(syncAPITableList);
        } else {
            binding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
            binding.txtEmptyEvents.setVisibility(View.VISIBLE);
        }
       /* if (syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size() >= 50) {
            showHitLimitDialog();
        }*/
        binding.switchClick.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                isClick = isChecked;
                if (isClick) {
                    callSyncAPI(0);
                }
                if (syncAPITableList.size() == 0) {
                    binding.switchClick.setChecked(false);
                    binding.txtEmptyEvents.setVisibility(View.VISIBLE);
                }
            }
        });
    }

    private void callSyncAPI(int position) {
        if (position < 0 || position >= syncAPITableList.size()) {
            return;
        }
        if (syncAPITableList.get(position).getEndpoint_url().contains("ProgessSync/AppTimeTrack")) {
            Log.e("syncAPITableList", "callSyncAPI:--if " + position + " size - " + syncAPITableList.size());
            CallApiForSpendApp(position);
        }

        if (syncAPITableList.size() == 1) {
            binding.switchClick.setChecked(false);
            binding.txtEmptyEvents.setVisibility(View.VISIBLE);
        }
    }

    public void CallApiForSpendApp(final int position) {
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
                    jsonObject.addProperty(Const.ISP, getWifiName(CacheEventsListActivity.this));
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
                                            /*syncTimeTrackingObject.setActivitytime(getUTCTime());
                                            syncTimeTrackingObject.setOuttime("");
                                            SharedPreferences sharedPreferences = getSharedPreferences("spendtime", MODE_PRIVATE);
                                            if (sharedPreferences != null) {
                                                SharedPreferences.Editor editor = sharedPreferences.edit();
                                                editor.clear();
                                                editor.apply();
                                            }*/

                                            courseDatabaseRepository.updateSyncTimeTracking(syncTimeTrackingObject);
                                        }
                                        syncAPITableList.remove(position);
                                        //syncAPIDatabaseRepository.deleteById(model.getId());
                                        cacheEventsListAdapter.notifyItemRemoved(position);
                                        callSyncAPI(position);
                                    }

                                    @Override
                                    public void onError(Throwable e) {
                                        callSyncAPI(position + 1);
                                        try {
                                            if (!userId.equals("")) {
                                                SyncAPITable syncAPITable = new SyncAPITable();
                                                syncAPITable.setApi_name("AppTimeTrack Progressed");
                                                syncAPITable.setEndpoint_url("ProgessSync/AppTimeTrack");
                                                syncAPITable.setParameters(String.valueOf(array));
                                                syncAPITable.setHeaders(PrefUtils.getAuthid(NoonApplication.getContext()));
                                                syncAPITable.setStatus("Errored");
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

    private void showHitLimitDialog() {
        HitLimitDialogBinding hitLimitDialogBinding = DataBindingUtil.inflate(LayoutInflater.from(CacheEventsListActivity.this), R.layout.hit_limit_dialog, null, false);
        AlertDialog.Builder builder = new AlertDialog.Builder(CacheEventsListActivity.this);
        builder.setView(hitLimitDialogBinding.getRoot());

        final AlertDialog alertDialog = builder.create();
        hitLimitDialogBinding.txtNoThanksClick.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                alertDialog.dismiss();
            }
        });

        hitLimitDialogBinding.txtPendingClick.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                alertDialog.dismiss();
                startActivity(new Intent(CacheEventsListActivity.this, CacheEventsListActivity.class));
            }
        });
        alertDialog.show();
    }
}
