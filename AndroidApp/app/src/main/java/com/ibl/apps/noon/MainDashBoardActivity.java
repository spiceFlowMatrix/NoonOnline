package com.ibl.apps.noon;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.location.Location;
import android.location.LocationListener;
import android.net.Uri;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.telephony.TelephonyManager;
import android.text.SpannableStringBuilder;
import android.text.TextUtils;
import android.text.format.Formatter;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;

import androidx.annotation.NonNull;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AlertDialog;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentTransaction;
import androidx.viewpager.widget.ViewPager;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.android.material.navigation.NavigationView.OnNavigationItemSelectedListener;
import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.ibl.apps.Adapter.ViewPagerAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.DeviceManagement.DeviceManagementRepository;
import com.ibl.apps.Fragment.ComplaintFragment;
import com.ibl.apps.Fragment.CourseFragment;
import com.ibl.apps.Fragment.GradeFragment;
import com.ibl.apps.Fragment.LibraryFragment;
import com.ibl.apps.Fragment.ProfileFragment;
import com.ibl.apps.Fragment.ReportFragment;
import com.ibl.apps.Interface.BackInterface;
import com.ibl.apps.Model.deviceManagement.registeruser.DeviceRegisterModel;
import com.ibl.apps.Model.versionUpdate.VersionUpdateModel;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Service.TimeOut.SyncEventReceiver;
import com.ibl.apps.noon.databinding.LogoutPopupLayoutBinding;
import com.ibl.apps.noon.databinding.MainDashboardLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.SingleShotLocationProvider;
import com.ibl.apps.versionUpdateManagement.VersionUpdateRepository;

import java.io.IOException;
import java.net.NetworkInterface;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.Response;

import static com.ibl.apps.util.Const.deviceStatus;


/**
 * Created by iblinfotech on 10/09/18.
 */

public class MainDashBoardActivity extends BaseActivity implements View.OnClickListener, OnNavigationItemSelectedListener, BackInterface, LocationListener {

    MainDashboardLayoutBinding mainDashboardLayoutBinding;
    private boolean doubleBackToExitPressedOnce = false;
    MenuItem prevMenuItem;
    //Fragments
    GradeFragment gradeFragment;
    ProfileFragment profileFragment;
    // GeneralDiscussionsFragment generalDiscussionsFragment;
    LibraryFragment libraryFragment;
    ReportFragment progressReportFragment;
    LogoutPopupLayoutBinding logoutPopupLayoutBinding;
    SharedPreferences sharedPreferences;

    boolean hideVisible = false;
    private UserDetails userDetailsObject;
    private String userId;
    private String userRoleName;
    private UserDetails userDetail;
    CourseDatabaseRepository courseDatabaseRepository;
    private String deviceStatusCode;
    private String ipAddress;

    @Override
    protected int getContentView() {
        return R.layout.main_dashboard_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);

        mainDashboardLayoutBinding = (MainDashboardLayoutBinding) getBindObj();
        setSupportActionBar(mainDashboardLayoutBinding.appBarLayout.toolBar);
        //callApiForInterval();
        callApiForVersionUpdate();
        courseDatabaseRepository = new CourseDatabaseRepository();

        sharedPreferences = getSharedPreferences("rolename", MODE_PRIVATE);
        userRoleName = sharedPreferences.getString("userrolename", "");

        if (!TextUtils.isEmpty(userRoleName)) {
            if (!userRoleName.equals("Parent")) {
                mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().removeItem(R.id.action_item4);
            }
        }
        //showDailog();
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, mainDashboardLayoutBinding.drawerLayout, mainDashboardLayoutBinding.appBarLayout.toolBar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        mainDashboardLayoutBinding.drawerLayout.setDrawerListener(toggle);
        toggle.syncState();
        mainDashboardLayoutBinding.navView.setNavigationItemSelectedListener(this);
        mainDashboardLayoutBinding.appBarLayout.toolBar.setNavigationIcon(null);
        mainDashboardLayoutBinding.appBarLayout.toolbarTitle.setText(getResources().getString(R.string.my_courses));
        mainDashboardLayoutBinding.appBarLayout.toolbarButtonLay.setVisibility(View.VISIBLE);

        //Spinner For Language Selection
        List<String> languageSelectcategories = new ArrayList<>();
        languageSelectcategories.add(0, getResources().getString(R.string.dari_school));
        languageSelectcategories.add(1, getResources().getString(R.string.pashto_school));

        // Creating adapter for spinner
        ArrayAdapter<String> languageSelectAdapter = new ArrayAdapter<>(this, R.layout.include_spinner, languageSelectcategories);

        // Drop down layout style - list view with radio button
        languageSelectAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        mainDashboardLayoutBinding.appBarLayout.languageSelectSpinner.setAdapter(languageSelectAdapter);

        mainDashboardLayoutBinding.appBarLayout.languageSelectSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int position, long l) {
              /*  if (position == 0) {
                    setLocale("ar");
                    recreate();
                } else if (position == 1) {
                    setLocale("ps");
                    recreate();
                }*/
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });

        //add this line to display menu1 when the activity is loaded
        //displaySelectedScreen(R.id.nav_menu1);
        mainDashboardLayoutBinding.drawerLayout.setDrawerLockMode(DrawerLayout.LOCK_MODE_LOCKED_CLOSED);

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @SuppressLint("RestrictedApi")
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userDetail = userDetails;
                    userId = userDetailsObject.getId();
                    if (userDetails.getRoleName() != null) {
//                        SharedPreferences preferences = getSharedPreferences("rolename", MODE_PRIVATE);
//                        SharedPreferences.Editor editor = preferences.edit();
//                        editor.putString("userrolename", userRoleName);
//                        editor.apply();

//                        SharedPreferences sharedPreferences = getSharedPreferences("rolename", MODE_PRIVATE);
//                        userRoleName = sharedPreferences.getString("userrolename", "");
                        userRoleName = userDetails.getRoleName().get(0);
//                        if (!TextUtils.isEmpty(userRoleName)) {
//                            if (!userRoleName.equals("Parent")) {
//                                mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().removeItem(R.id.action_item4);
//                            }
//                        }

                        callAPIDeviceManagement();
                        SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();

                        List<SyncAPITable> syncAPITableList = syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId));

                        handleSyncButton();

                        if (syncAPITableList.size() >= 50) {
                            NoonApplication.cacheStatus = 2;
                            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                            if (editor != null) {
                                editor.clear();
                                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                editor.apply();
                            }
                            SharedPreferences deviceSharedPreferences = getSharedPreferences("deviceStatus", MODE_PRIVATE);
                            deviceStatusCode = deviceSharedPreferences.getString("deviceStatusCode", "");
                            if (deviceStatusCode != null && deviceStatusCode.equals("0")) {
                                showHitLimitDialog(MainDashBoardActivity.this);
                            }
                        }

                        mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.setOnNavigationItemSelectedListener(
                                new BottomNavigationView.OnNavigationItemSelectedListener() {
                                    @Override
                                    public boolean onNavigationItemSelected(@NonNull MenuItem item) {
                                        if (userDetail != null && userDetail.getRoleName().get(0).equals("Parent")) {
                                            switch (item.getItemId()) {
                                                case R.id.action_item1:
                                                    mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.setCurrentItem(0);
                                                    mainDashboardLayoutBinding.appBarLayout.toolbarTitle.setText(getResources().getString(R.string.my_courses));
                                                    mainDashboardLayoutBinding.appBarLayout.toolbarButtonLay.setVisibility(View.VISIBLE);
                                                    mainDashboardLayoutBinding.appBarLayout.toolbarresetPassLay.setVisibility(View.GONE);
                                                    mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                                    break;
                                                case R.id.action_item2:
                                                    mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.GONE);
                                                    mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.setCurrentItem(1);
                                                    break;
                                                case R.id.action_item4:
                                                    mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.GONE);
                                                    mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.setCurrentItem(2);
                                                    break;
                                                case R.id.action_item3:
                                                    mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                                    mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.setCurrentItem(3);
                                                    setPos2View();
                                                    break;
                                            }
                                        } else {
                                            switch (item.getItemId()) {
                                                case R.id.action_item1:
                                                    mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.setCurrentItem(0);
                                                    mainDashboardLayoutBinding.appBarLayout.toolbarTitle.setText(getResources().getString(R.string.my_courses));
                                                    mainDashboardLayoutBinding.appBarLayout.toolbarButtonLay.setVisibility(View.VISIBLE);
                                                    mainDashboardLayoutBinding.appBarLayout.toolbarresetPassLay.setVisibility(View.GONE);
                                                    mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                                    break;
                                                case R.id.action_item2:
                                                    mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.GONE);
                                                    mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.setCurrentItem(1);
                                                    break;

                                                case R.id.action_item3:
                                                    mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                                    mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.setCurrentItem(2);
                                                    setPos2View();
                                                    break;
                                            }
                                        }

                                        return false;
                                    }
                                });
                        mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.addOnPageChangeListener(new ViewPager.OnPageChangeListener() {
                            @Override
                            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {

                                if (userDetail != null && userDetail.getRoleName().get(0).equals("Parent")) {
                                    switch (position) {
                                        case 0:
                                            break;
                                        case 1:
                                            break;
                                        case 2:
                                            //generalDiscussionsFragment.DiscussionsFragment();
                                            break;
                                        case 3:
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                            setPos2View();
                                            break;
                                    }
                                } else {
                                    switch (position) {
                                        case 0:
                                            break;
                                        case 1:
                                            break;
                                        case 2:
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                            setPos2View();
                                            break;
                                    }
                                }

                            }

                            @Override
                            public void onPageSelected(int position) {
                                if (userDetail != null && userDetail.getRoleName().get(0).equals("Parent")) {
                                    switch (position) {
                                        case 0:
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                            mainDashboardLayoutBinding.appBarLayout.toolbarButtonLay.setVisibility(View.VISIBLE);
                                            mainDashboardLayoutBinding.appBarLayout.toolbarresetPassLay.setVisibility(View.GONE);
                                            break;
                                        case 1:
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.GONE);
                                            break;

                                        case 2:
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.GONE);
                                            break;
                                        case 3:
                                            mainDashboardLayoutBinding.appBarLayout.toolbarButtonLay.setVisibility(View.GONE);
                                            mainDashboardLayoutBinding.appBarLayout.toolbarresetPassLay.setVisibility(View.VISIBLE);
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                            break;
                                    }
                                } else {
                                    switch (position) {
                                        case 0:
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                            mainDashboardLayoutBinding.appBarLayout.toolbarButtonLay.setVisibility(View.VISIBLE);
                                            mainDashboardLayoutBinding.appBarLayout.toolbarresetPassLay.setVisibility(View.GONE);
                                            break;
                                        case 1:
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.GONE);
                                            break;

                                        case 2:
                                            mainDashboardLayoutBinding.appBarLayout.toolbarButtonLay.setVisibility(View.GONE);
                                            mainDashboardLayoutBinding.appBarLayout.toolbarresetPassLay.setVisibility(View.VISIBLE);
                                            mainDashboardLayoutBinding.appBarLayout.toolBar.setVisibility(View.VISIBLE);
                                            break;
                                    }
                                }


                                if (prevMenuItem != null) {
                                    prevMenuItem.setChecked(false);
                                } else {
                                    mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().getItem(0).setChecked(false);
                                }
                                mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().getItem(position).setChecked(true);
                                prevMenuItem = mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().getItem(position);
                            }

                            @Override
                            public void onPageScrollStateChanged(int state) {

                            }
                        });
                        setupViewPager(mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager);
                        SyncEventReceiver.setupAlarm(MainDashBoardActivity.this);
                    }
                }
                SharedPreferences sharedPreferences = getSharedPreferences("user", MODE_PRIVATE);
                userId = sharedPreferences.getString("uid", "");

                assert userId != null;
                if (!userId.equals("")) {
                    SyncTimeTrackingObject syncTimeTrackingObject = courseDatabaseRepository.getSyncTimeTrackById(Integer.parseInt(userId));
                    if (syncTimeTrackingObject != null) {
                        syncTimeTrackingObject.setLatitude("");
                        syncTimeTrackingObject.setLongitude("");
                        syncTimeTrackingObject.setOperatingsystem(Const.var_deviceType);
                        syncTimeTrackingObject.setHardwareplatform(Build.MODEL);
                        syncTimeTrackingObject.setServiceprovider(getCarierName());
                        syncTimeTrackingObject.setVersion(Build.VERSION.RELEASE);
                        syncTimeTrackingObject.setUserid(Integer.parseInt(userId));
                        courseDatabaseRepository.updateSyncTimeTracking(syncTimeTrackingObject);
                    } else {
                        SyncTimeTrackingObject syncTimeTrackingObjectinsert = new SyncTimeTrackingObject();
                        syncTimeTrackingObjectinsert.setLatitude("");
                        syncTimeTrackingObjectinsert.setLongitude("");
                        syncTimeTrackingObjectinsert.setOperatingsystem(Const.var_deviceType);
                        syncTimeTrackingObjectinsert.setHardwareplatform(Build.MODEL);
                        syncTimeTrackingObjectinsert.setServiceprovider(getCarierName());
                        syncTimeTrackingObjectinsert.setVersion(Build.VERSION.RELEASE);
                        syncTimeTrackingObjectinsert.setActivitytime(getUTCTime());
                        syncTimeTrackingObjectinsert.setUserid(Integer.parseInt(userId));
                        courseDatabaseRepository.insertSyncTimeTrackingData(syncTimeTrackingObjectinsert);
                    }
                }
                return null;
            }
        }).execute();
        setOnClickListener();
        /*if (userDetails!=null&&!userDetails.getRoleName().get(0).equals("Parent")) {
            mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().removeItem(R.id.action_item4);
//            mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().add(R.id.action_item4);
        }else{
            userDetails=new UserDetails();
            List<String> list=new ArrayList<>();
            list.add("");
            userDetails.setRoleName(list);
            if (!userDetails.getRoleName().get(0).equals("Parent")) {
                mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().removeItem(R.id.action_item4);
//            mainDashboardLayoutBinding.appBarLayout.contentMain.bottomNavigation.getMenu().add(R.id.action_item4);
            }
        }*/

    }

    private void handleSyncButton() {
        SharedPreferences sharedPreferencesuser = getSharedPreferences("cacheStatus", MODE_PRIVATE);
        String flagStatus = sharedPreferencesuser.getString("FlagStatus", "");

        SharedPreferences deviceSharedPreferences = getSharedPreferences("deviceStatus", MODE_PRIVATE);
        deviceStatusCode = deviceSharedPreferences.getString("deviceStatusCode", "");
        Log.e("deviceStatusCode", "setUp: flagStatus" + deviceStatusCode);

        if (deviceStatusCode != null && deviceStatusCode.equals("0")) {
            switch (flagStatus) {
                case "1":
                    mainDashboardLayoutBinding.appBarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_pending);
                    break;
                case "2":
                    mainDashboardLayoutBinding.appBarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_error);
                    break;
                case "3":
                    mainDashboardLayoutBinding.appBarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_syncing);
                    break;
                case "4":
                    GlideApp.with(MainDashBoardActivity.this)
                            .load(R.drawable.ic_cache_empty)
                            .error(R.drawable.ic_cache_empty)
                            .into(mainDashboardLayoutBinding.appBarLayout.cacheEventsStatusBtn);
                    break;
            }
        } else {
            GlideApp.with(MainDashBoardActivity.this)
                    .load(R.drawable.ic_cache_empty)
                    .error(R.drawable.ic_cache_empty)
                    .into(mainDashboardLayoutBinding.appBarLayout.cacheEventsStatusBtn);
        }
    }

    private void callAPIDeviceManagement() {
        WifiManager manager = (WifiManager) getApplicationContext().getSystemService(Context.WIFI_SERVICE);
        WifiInfo info = null;
        if (manager != null) {
            info = manager.getConnectionInfo();
            //macAddress = info.getMacAddress();
            ipAddress = Formatter.formatIpAddress(manager.getConnectionInfo().getIpAddress());
        }

        //OS
        JsonObject jsonOs = new JsonObject();
        jsonOs.addProperty(Const.name, Const.var_deviceType);
        jsonOs.addProperty(Const.version, Build.VERSION.RELEASE);

        //tag
        JsonArray jsonArray = new JsonArray();
        JsonObject j = new JsonObject();
        j.addProperty(Const.name, "");
        jsonArray.add(j);

        //main
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty(Const.macAddress, getWifiMacAddress());
        jsonObject.addProperty(Const.ipAddress, ipAddress);
        jsonObject.addProperty(Const.modelName, Build.MODEL);
        jsonObject.addProperty(Const.modelNumber, Build.SERIAL);
        jsonObject.add(Const.operatingSystem, jsonOs);
        jsonObject.add(Const.tags, jsonArray);

        CompositeDisposable disposable = new CompositeDisposable();
        DeviceManagementRepository deviceManagementRepository = new DeviceManagementRepository();
        disposable.add(deviceManagementRepository.registerDeviceDetail(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<Response<DeviceRegisterModel>>() {
                    @Override
                    public void onSuccess(Response<DeviceRegisterModel> deviceListModel) {

                        try {
                            if ((deviceListModel.errorBody() != null)) {

                                Long errorCode = new Gson().fromJson(deviceListModel.errorBody().string(), DeviceRegisterModel.class).getResponseCode();

                                if (errorCode == 2) {
                                    deviceStatus = 2;
                                    SharedPreferences deviceStatusPreferences = getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = deviceStatusPreferences.edit();
                                    editor.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                    editor.apply();
                                } else if (errorCode == 3) {
                                    deviceStatus = 3;
                                    SharedPreferences deviceStatusPreferences = getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = deviceStatusPreferences.edit();
                                    editor.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                    editor.apply();
                                }
                            }

                            if (deviceListModel.body() != null && deviceListModel.body().getResponseCode() == 0) {
                                deviceStatus = 0;
                                SharedPreferences deviceStatusPreferences = getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                SharedPreferences.Editor editor = deviceStatusPreferences.edit();
                                editor.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                editor.apply();
                            }


                            handleSyncButton();
                            setOnClickListener();

                            Log.e("deviceStatusCode", "onSuccess: DASH" + String.valueOf(deviceStatus));
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                    }
                }));
    }

    public String getWifiMacAddress() {
        try {
            String interfaceName = "wlan0";
            List<NetworkInterface> interfaces = Collections.list(NetworkInterface.getNetworkInterfaces());
            for (NetworkInterface intf : interfaces) {
                if (!intf.getName().equalsIgnoreCase(interfaceName)) {
                    continue;
                }

                byte[] mac = intf.getHardwareAddress();
                if (mac == null) {
                    return "";
                }

                StringBuilder buf = new StringBuilder();
                for (byte aMac : mac) {
                    buf.append(String.format("%02X:", aMac));
                }
                if (buf.length() > 0) {
                    buf.deleteCharAt(buf.length() - 1);
                }
                return buf.toString();
            }
        } catch (Exception ex) {
            ex.getMessage();
        } // for now eat exceptions
        return "";
    }


    private void showDailog() {
        SharedPreferences sharedPrefs = getSharedPreferences("RATER", 0);
        if (sharedPrefs.getBoolean("NO THANKS", false)) {
        } else {
            SharedPreferences.Editor prefsEditor = sharedPrefs.edit();
            //YOUR CODE TO SHOW DIALOG
            long time = sharedPrefs.getLong("displayedTime", 0);
            if (time < System.currentTimeMillis() - 172800000) {
                displayDialog();
                prefsEditor.putLong("displayedTime", System.currentTimeMillis()).apply();
            }
            prefsEditor.apply();
        }
    }

    private void displayDialog() {
        // TODO Auto-generated method stub
        DialogInterface.OnClickListener dialogClickListener = new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                switch (which) {
                    case DialogInterface.BUTTON_POSITIVE:
                        //Yes button clicked
                        SharedPreferences prefs = getSharedPreferences("RATER", 0);
                        SharedPreferences.Editor editor = prefs.edit();
                        editor.putBoolean("NO THANKS", true);
                        editor.apply();
                        break;

                    case DialogInterface.BUTTON_NEGATIVE:
                        //No button clicked
                        //Saving a boolean on no thanks button click

//                        Intent in = new Intent(android.content.Intent.ACTION_VIEW);
//                        in.setData(Uri.parse(url));
//                        startActivity(in);

                        break;
                }
            }
        };

        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("Updates are Available!!");
        builder.setMessage("Please update app for more features")
                .setPositiveButton("OK", dialogClickListener).
                setNegativeButton("CANCEL", dialogClickListener).show();

    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        try {
            if (requestCode == 100 && data != null) {
                if (resultCode == RESULT_OK) {
                    enabledLocation(this);
                } else {
                    SharedPreferences sharedPreferences = getSharedPreferences("user", MODE_PRIVATE);
                    userId = sharedPreferences.getString("uid", "");

                    assert userId != null;
                    if (!userId.equals("")) {
                        SyncTimeTrackingObject syncTimeTrackingObject = courseDatabaseRepository.getSyncTimeTrackById(Integer.parseInt(userId));
                        if (syncTimeTrackingObject != null) {
                            syncTimeTrackingObject.setLatitude("");
                            syncTimeTrackingObject.setLongitude("");
                            syncTimeTrackingObject.setOperatingsystem(Const.var_deviceType);
                            syncTimeTrackingObject.setHardwareplatform(Build.MODEL);
                            syncTimeTrackingObject.setServiceprovider(getCarierName());
                            syncTimeTrackingObject.setVersion(Build.VERSION.RELEASE);
                            syncTimeTrackingObject.setUserid(Integer.parseInt(userId));
                            courseDatabaseRepository.updateSyncTimeTracking(syncTimeTrackingObject);
                        } else {
                            SyncTimeTrackingObject syncTimeTrackingObjectinsert = new SyncTimeTrackingObject();
                            syncTimeTrackingObjectinsert.setLatitude("");
                            syncTimeTrackingObjectinsert.setLongitude("");
                            syncTimeTrackingObjectinsert.setOperatingsystem(Const.var_deviceType);
                            syncTimeTrackingObjectinsert.setHardwareplatform(Build.MODEL);
                            syncTimeTrackingObjectinsert.setServiceprovider(getCarierName());
                            syncTimeTrackingObjectinsert.setVersion(Build.VERSION.RELEASE);
                            syncTimeTrackingObjectinsert.setActivitytime(getUTCTime());
                            syncTimeTrackingObjectinsert.setUserid(Integer.parseInt(userId));
                            courseDatabaseRepository.insertSyncTimeTrackingData(syncTimeTrackingObjectinsert);
                        }
                    }
                }

            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public void setPos2View() {
        mainDashboardLayoutBinding.appBarLayout.toolbarButtonLay.setVisibility(View.GONE);
        mainDashboardLayoutBinding.appBarLayout.toolbarresetPassLay.setVisibility(View.VISIBLE);

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {

            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {

                if (userDetails != null) {
                    if (userDetails.getUsername() != null) {
                        String ProfileUsername = userDetails.getUsername();
                        if (!TextUtils.isEmpty(ProfileUsername)) {
                            mainDashboardLayoutBinding.appBarLayout.toolbarTitle.setText(ProfileUsername);
                        }
                    }
                }
                return null;
            }

        }).execute();

        profileFragment.checkCurrScreenApi();
        mainDashboardLayoutBinding.appBarLayout.editprofileBtn.setImageResource(R.drawable.ic_check_black);
        profileFragment.hideVisibleLay(true);
        hideVisible = false;
    }

    private void setupViewPager(ViewPager viewPager) {

        ViewPagerAdapter adapter = new ViewPagerAdapter(getSupportFragmentManager());
        gradeFragment = new GradeFragment();
        libraryFragment = new LibraryFragment();
        //generalDiscussionsFragment = new GeneralDiscussionsFragment();
        profileFragment = new ProfileFragment();
        progressReportFragment = new ReportFragment();
        adapter.addFragment(gradeFragment);
        adapter.addFragment(libraryFragment);
        if (userDetail != null && userDetail.getRoleName().get(0).equals("Parent")) {
            adapter.addFragment(progressReportFragment);
        }

        //adapter.addFragment(generalDiscussionsFragment);
        adapter.addFragment(profileFragment);
        viewPager.setAdapter(adapter);

        if (getIntent().hasExtra(Const.isDiscussions)) {
            Boolean isDiscussions = getIntent().getExtras().getBoolean(Const.isDiscussions, false);
            if (isDiscussions) {
                viewPager.setCurrentItem(2);
                if (getIntent().hasExtra(Const.isNotification)) {
                    Boolean isNotification = getIntent().getExtras().getBoolean(Const.isNotification, false);
                    if (isNotification) {
                        Intent i = new Intent(MainDashBoardActivity.this, GeneralDiscussionsDetailActivity.class);
                        i.putExtra(Const.topicId, getIntent().getStringExtra(Const.topicId));
                        i.putExtra(Const.topicname, getIntent().getStringExtra(Const.topicname));
                        i.putExtra(Const.isprivate, getIntent().getStringExtra(Const.isprivate));
                        i.putExtra(Const.GradeID, getIntent().getStringExtra(Const.GradeID));
                        i.putExtra(Const.CourseName, getIntent().getStringExtra(Const.CourseName));
                        i.putExtra(Const.ActivityFlag, getIntent().getStringExtra(Const.ActivityFlag));
                        i.putExtra(Const.LessonID, getIntent().getStringExtra(Const.LessonID));
                        i.putExtra(Const.QuizID, getIntent().getStringExtra(Const.QuizID));
                        startActivity(i);
                        finish();
                    }
                }
            }
        }
    }

    public void setOnClickListener() {
        mainDashboardLayoutBinding.appBarLayout.resetPassBtn.setOnClickListener(this);
        mainDashboardLayoutBinding.appBarLayout.serachCourseIMag.setOnClickListener(this);
        mainDashboardLayoutBinding.appBarLayout.feedbackbtn.setOnClickListener(this);
        mainDashboardLayoutBinding.appBarLayout.logOutBtn.setOnClickListener(this);
        mainDashboardLayoutBinding.appBarLayout.editprofileBtn.setOnClickListener(this);
        mainDashboardLayoutBinding.appBarLayout.btnNotification.setOnClickListener(this);
        mainDashboardLayoutBinding.appBarLayout.cacheEventsStatusBtn.setOnClickListener(this);
        mainDashboardLayoutBinding.appBarLayout.txtDevice.setOnClickListener(this);
        Log.e("deviceStatusCode", "setOnClickListener: " + deviceStatusCode);
    }

    @Override
    public void onClick(View view) {

        switch (view.getId()) {
            case R.id.resetPassBtn:
                Intent i = new Intent(MainDashBoardActivity.this, ResetPasswordActivity.class);
                startActivity(i);
                break;
            case R.id.serachCourseIMag:
                Log.e("deviceStatusCode", "setUp: serachCourseIMag" + deviceStatusCode);
                if (deviceStatusCode != null && deviceStatusCode.equals("0")) {
                    Intent i1 = new Intent(MainDashBoardActivity.this, SearchActivity.class);
                    startActivity(i1);
                }
                break;
            case R.id.btnNotification:
                Log.e("deviceStatusCode", "setUp: btnNotification" + deviceStatusCode);
                if (deviceStatusCode != null && deviceStatusCode.equals("0")) {
                    Intent i3 = new Intent(MainDashBoardActivity.this, NotificationActivity.class);
                    startActivity(i3);
                }
                break;

            case R.id.cacheEventsStatusBtn:
                Log.e("deviceStatusCode", "setUp: cacheEventsStatusBtn" + deviceStatusCode);
                if (deviceStatusCode != null && deviceStatusCode.equals("0")) {
                    Intent cacheIntent = new Intent(MainDashBoardActivity.this, CacheEventsListActivity.class);
                    startActivity(cacheIntent);
                }
                break;
            case R.id.feedbackbtn:
                Log.e("deviceStatusCode", "setUp: feedbackbtn" + deviceStatusCode);
                if (deviceStatusCode != null && deviceStatusCode.equals("0")) {
                    if (isNetworkAvailable(MainDashBoardActivity.this)) {
                        Intent i2 = new Intent(MainDashBoardActivity.this, FeedBackActivity.class);
//                Intent i2 = new Intent(Intent.ACTION_VIEW);
//                i2.setData(Uri.parse(Const.FEEDBACK_URL));
                        startActivity(i2);
                        finish();
                    } else {
                        showNetworkAlert(MainDashBoardActivity.this);
                    }
                }
                break;
            case R.id.logOutBtn:
                showLogoutAlert(MainDashBoardActivity.this);
                //showLogoutDialog();
                break;
            case R.id.editprofileBtn:
                if (profileFragment.validateFields()) {
                    profileFragment.updateUserData();
                    profileFragment.hideVisibleLay(true);
                }
                break;

            case R.id.txtDevice:
                if (isNetworkAvailable(MainDashBoardActivity.this))
                    openActivity(LoginDevicesActivity.class);
                else showNetworkAlert(MainDashBoardActivity.this);
                break;
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
    }

    private void displaySelectedScreen(int itemId) {

        //creating fragment object
        Fragment fragment = null;

        //initializing the fragment object which is selected
        switch (itemId) {
            case R.id.nav_menu1:
                fragment = new GradeFragment();
                break;
            case R.id.nav_menu2:
                fragment = new ProfileFragment();
                break;
            case R.id.nav_menu3:
                fragment = new CourseFragment();
                break;
            case R.id.nav_menu4:
                fragment = new ComplaintFragment();
                break;
            case R.id.nav_menu5:
                break;
            case R.id.nav_menu6:
                break;
            case R.id.nav_menu7:
                break;
        }

        //replacing the fragment
        if (fragment != null) {
            FragmentTransaction ft = getSupportFragmentManager().beginTransaction();
            ft.replace(R.id.contentFrame, fragment);
            ft.commit();
        }

        mainDashboardLayoutBinding.drawerLayout.closeDrawer(GravityCompat.START);
    }

    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem item) {
        //calling the method displayselectedscreen and passing the id of selected menu
        displaySelectedScreen(item.getItemId());
        //make this method blank
        return true;
    }

    @Override
    public void onBackPressed() {
        if (getSupportFragmentManager().getBackStackEntryCount() > 0) {
            getSupportFragmentManager().popBackStack();
        } else if (mainDashboardLayoutBinding.drawerLayout.isDrawerOpen(GravityCompat.START)) {
            mainDashboardLayoutBinding.drawerLayout.closeDrawer(GravityCompat.START);
        } else {
            if (doubleBackToExitPressedOnce) {
                super.onBackPressed();
                return;
            }
            doubleBackToExitPressedOnce = true;
            showSnackBar(mainDashboardLayoutBinding.drawerLayout, getString(R.string.error_again_to_exit));
            new Handler().postDelayed(new Runnable() {
                @Override
                public void run() {
                    doubleBackToExitPressedOnce = false;
                }
            }, 2000);
        }
    }


    public void enabledLocation(Context context) {
        SingleShotLocationProvider.requestSingleUpdate(context, this::getLocation);
    }

    private void getLocation(SingleShotLocationProvider.GPSCoordinates location) {
        SharedPreferences sharedPreferences = getSharedPreferences("user", MODE_PRIVATE);
        userId = sharedPreferences.getString("uid", "");

        assert userId != null;
        if (!userId.equals("")) {
            SyncTimeTrackingObject syncTimeTrackingObject = courseDatabaseRepository.getSyncTimeTrackById(Integer.parseInt(userId));
            if (syncTimeTrackingObject != null) {
                syncTimeTrackingObject.setLatitude(String.valueOf(location.latitude));
                syncTimeTrackingObject.setLongitude(String.valueOf(location.longitude));
                syncTimeTrackingObject.setOperatingsystem(Const.var_deviceType);
                syncTimeTrackingObject.setHardwareplatform(Build.MODEL);
                syncTimeTrackingObject.setServiceprovider(getCarierName());
                syncTimeTrackingObject.setVersion(Build.VERSION.RELEASE);
                syncTimeTrackingObject.setUserid(Integer.parseInt(userId));
                courseDatabaseRepository.updateSyncTimeTracking(syncTimeTrackingObject);
            } else {
                SyncTimeTrackingObject syncTimeTrackingObjectinsert = new SyncTimeTrackingObject();
                syncTimeTrackingObjectinsert.setLatitude(String.valueOf(location.latitude));
                syncTimeTrackingObjectinsert.setLongitude(String.valueOf(location.longitude));
                syncTimeTrackingObjectinsert.setOperatingsystem(Const.var_deviceType);
                syncTimeTrackingObjectinsert.setHardwareplatform(Build.MODEL);
                syncTimeTrackingObjectinsert.setServiceprovider(getCarierName());
                syncTimeTrackingObjectinsert.setVersion(Build.VERSION.RELEASE);
                syncTimeTrackingObjectinsert.setActivitytime(getUTCTime());
                syncTimeTrackingObjectinsert.setUserid(Integer.parseInt(userId));
                courseDatabaseRepository.insertSyncTimeTrackingData(syncTimeTrackingObjectinsert);
            }
        }

    }

    private String getCarierName() {
        TelephonyManager tManager = (TelephonyManager) this.getBaseContext()
                .getSystemService(Context.TELEPHONY_SERVICE);
        String carrierName = tManager.getNetworkOperatorName();
        return carrierName;
    }

    @Override
    public void backfragment() {
        mainDashboardLayoutBinding.appBarLayout.contentMain.courseViewpager.setCurrentItem(0);
    }

    @Override
    public void onLocationChanged(Location location) {
    }

    @Override
    public void onStatusChanged(String s, int i, Bundle bundle) {

    }

    @Override
    public void onProviderEnabled(String s) {

    }

    @Override
    public void onProviderDisabled(String s) {

    }

    private void callApiForVersionUpdate() {
        CompositeDisposable disposable = new CompositeDisposable();
        VersionUpdateRepository versionUpdateRepository = new VersionUpdateRepository();

        disposable.add(versionUpdateRepository.getVersionUpdate()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<VersionUpdateModel>() {
                    @Override
                    public void onSuccess(VersionUpdateModel versionUpdateModel) {
                        try {
                            if (versionUpdateModel.getData() != null && BuildConfig.VERSION_CODE < Integer.parseInt(versionUpdateModel.getData().getVersionCode())) {
                                if (versionUpdateModel.getData().getIsForceUpdate()) {
                                    SpannableStringBuilder message = setTypeface(MainDashBoardActivity.this, getString(R.string.update_version_message));
                                    AlertDialog.Builder builder = new AlertDialog.Builder(MainDashBoardActivity.this);
                                    builder.setTitle(R.string.validation_warning);
                                    builder.setMessage(message)
                                            .setPositiveButton(R.string.version_update, new DialogInterface.OnClickListener() {
                                                public void onClick(DialogInterface dialog, int which) {
                                                    // Yes button clicked, do something
                                                    dialog.dismiss();
                                                    String packageName = getPackageName();
                                                    Log.e("packageName", "onClick: " + packageName);
                                                    Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse("market://details?id=" + packageName));
                                                    if (intent.resolveActivity(getPackageManager()) != null) {
                                                        startActivity(intent);
                                                    } else {
                                                        intent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://play.google.com/store/apps/details?id=" + packageName));
                                                        if (intent.resolveActivity(getPackageManager()) != null)
                                                            startActivity(intent);
                                                    }
                                                }
                                            });

                                    builder.setCancelable(false);
                                    builder.show();

                                } else {
                                    try {
                                        SpannableStringBuilder message = setTypeface(MainDashBoardActivity.this, getString(R.string.update_version_message));
                                        AlertDialog.Builder builder = new AlertDialog.Builder(MainDashBoardActivity.this);
                                        builder.setTitle(R.string.validation_warning);
                                        builder.setMessage(message)
                                                .setPositiveButton(R.string.version_update, new DialogInterface.OnClickListener() {
                                                    public void onClick(DialogInterface dialog, int which) {
                                                        // Yes button clicked, do something
                                                        dialog.dismiss();
                                                        String packageName = getPackageName();
                                                        Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse("market://details?id=" + packageName));
                                                        if (intent.resolveActivity(getPackageManager()) != null) {
                                                            startActivity(intent);
                                                        } else {
                                                            intent = new Intent(Intent.ACTION_VIEW, Uri.parse("https://play.google.com/store/apps/details?id=" + packageName));
                                                            if (intent.resolveActivity(getPackageManager()) != null)
                                                                startActivity(intent);
                                                        }
                                                    }
                                                });

                                        builder.setNegativeButton(R.string.cancel, new DialogInterface.OnClickListener() {
                                            public void onClick(DialogInterface dialog, int arg1) {
                                                dialog.dismiss();
                                            }
                                        });

                                        builder.show();
                                    } catch (Exception e) {
                                        e.printStackTrace();
                                    }
                                }
                            }
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));
    }


   /* long userInteractionTime = 0;

    @Override
    public void onUserInteraction() {
        userInteractionTime = System.currentTimeMillis();
        super.onUserInteraction();
        Log.i("appname", "Interaction");
    }

    @Override
    public void onUserLeaveHint() {
        try {
            long uiDelta = (System.currentTimeMillis() - userInteractionTime);

            super.onUserLeaveHint();
            Log.i("bThere", "Last User Interaction = " + getUTCTime());
            SharedPreferences sharedPreferences = getSharedPreferences("user", Context.MODE_PRIVATE);
            userId = sharedPreferences.getString("uid", "");
            SyncTimeTrackingObject syncTimeTrackingObject = AppDatabase.getAppDatabase(this).syncTimeTrackingDao().getSyncTimeTrack(Integer.parseInt(userId));

            if (syncTimeTrackingObject != null) {

                syncTimeTrackingObject.setOuttime(getUTCTime());
                syncTimeTrackingObject.setUserid(Integer.parseInt(userId));
                AppDatabase.getAppDatabase(this).syncTimeTrackingDao().updateSyncTimeTracking(syncTimeTrackingObject);
            } else {
                SyncTimeTrackingObject syncTimeTracking = new SyncTimeTrackingObject();
                syncTimeTracking.setLatitude("");
                syncTimeTracking.setLongitude("");
                syncTimeTracking.setOperatingsystem(Const.var_deviceType);
                syncTimeTracking.setHardwareplatform(Build.MODEL);
                syncTimeTracking.setServiceprovider(getCarierName());
                syncTimeTracking.setOuttime(getUTCTime());
                syncTimeTracking.setUserid(Integer.parseInt(userId));
                AppDatabase.getAppDatabase(this).syncTimeTrackingDao().insertAll(syncTimeTracking);
            }
            if (uiDelta < 100)
                Log.i("appname", "Home Key Pressed");
            else
                Log.i("appname", "We are leaving, but will probably be back shortly!");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }*/
}