package com.ibl.apps.noon;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.NetworkInfo;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.CompoundButton;

import androidx.appcompat.app.AlertDialog;
import androidx.databinding.DataBindingUtil;

import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.Adapter.CacheEventsListAdapter;
import com.ibl.apps.Base.BaseActivity;
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
import com.ibl.apps.UserCredentialsManagement.UserRepository;
import com.ibl.apps.noon.databinding.ActivityCatchEventsListBinding;
import com.ibl.apps.noon.databinding.HitLimitDialogBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.chapterProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.fileProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.lessonProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.quizProgressList;

public class CacheEventsListActivity extends BaseActivity {
    ActivityCatchEventsListBinding binding;
    CacheEventsListAdapter cacheEventsListAdapter;
    public static boolean isClick = false;
    private SyncAPIDatabaseRepository syncAPIDatabaseRepository;
    private String userId;
    private List<SyncAPITable> syncAPITableList = new ArrayList<>();
    private CompositeDisposable disposable;
    private LessonRepository lessonRepository;
    private QuizRepository quizRepository;
    private LessonDatabaseRepository lessonDatabaseRepository;
    private QuizDatabaseRepository quizDatabaseRepository;
    ProgressDialog mProgressDialog = null;

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
        disposable = new CompositeDisposable();
        lessonRepository = new LessonRepository();
        lessonDatabaseRepository = new LessonDatabaseRepository();
        quizDatabaseRepository = new QuizDatabaseRepository();
        quizRepository = new QuizRepository();

        setToolbar(binding.toolBar);
        showBackArrow(getString(R.string.pending_cache_events));
        if (Build.VERSION.SDK_INT <= Build.VERSION_CODES.KITKAT) {
            binding.txtCacheTitle.setTextSize(35);
        }
        SharedPreferences sharedPreferences = getSharedPreferences("user", MODE_PRIVATE);
        userId = sharedPreferences.getString("uid", "");
        syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
        binding.toolBar.setNavigationOnClickListener(view -> {
            finish();
            Intent intent1 = new Intent(CacheEventsListActivity.this, MainDashBoardActivity.class);
            startActivity(intent1);
        });

        syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId));
        syncAPITableList = syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId));

//        Log.e("CacheEventsListActivity", "onViewReady: " + syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).toString());
//        Log.e("CacheEventsListActivity", "onViewReady:size := " + syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size());
        if (syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)) != null && syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size() != 0) {
            binding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
            binding.txtEmptyEvents.setVisibility(View.GONE);
            cacheEventsListAdapter = new CacheEventsListAdapter(CacheEventsListActivity.this, syncAPITableList);
            binding.rcVerticalLayout.rcVertical.setAdapter(cacheEventsListAdapter);
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
                if (isNetworkAvailable(CacheEventsListActivity.this)) {
                    if (isClick) {
                        if (mProgressDialog == null) {
                            mProgressDialog = new ProgressDialog(CacheEventsListActivity.this);
                            mProgressDialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
                            mProgressDialog.setCancelable(false);
                            mProgressDialog.setCanceledOnTouchOutside(false);

                        }
                        mProgressDialog.setMessage(getString(R.string.loading));
                        binding.switchClick.setChecked(true);
                        callSyncAPI(0);
                    } else {
                        binding.switchClick.setChecked(false);
                    }
                } else {
                    binding.switchClick.setChecked(false);
                    showNetworkAlert(CacheEventsListActivity.this);
                }
                if (syncAPITableList.size() == 0) {
                    NoonApplication.cacheStatus = 4;
                    SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
                    SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                    if (editor != null) {
                        editor.clear();
                        editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                        editor.apply();
                    }
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
        mProgressDialog.show();
        if (syncAPITableList.get(position).getEndpoint_url().contains("ProgessSync/AppTimeTrack")) {
            JsonArray jsonArray = new Gson().fromJson(syncAPITableList.get(position).getParameters(), JsonArray.class);
            CallApiForSpendAppCacheScreen(jsonArray, position);

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
            List<QuizUserResult> quizUserResults = quizDatabaseRepository.getAllQuizuserResult(false, userId);
            List<LessonProgress> lessonProgressList = lessonDatabaseRepository.getAllLessonProgressData(false, userId);
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
            mProgressDialog.dismiss();
            NoonApplication.cacheStatus = 4;
            SharedPreferences sharedPreferencesCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
            if (editor != null) {
                editor.clear();
                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                editor.apply();
            }
            binding.switchClick.setChecked(false);
            binding.txtEmptyEvents.setVisibility(View.VISIBLE);
        }
    }

    public void CallApiForSpendAppCacheScreen(JsonArray jsonArray, final int position) {
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

                        disposable.add(userRepository.getSyncTimeTracking(jsonArray).subscribeOn(Schedulers.io())
                                .observeOn(AndroidSchedulers.mainThread())
                                .subscribeWith(new DisposableSingleObserver<SyncTimeTracking>() {
                                    @Override
                                    public void onSuccess(SyncTimeTracking syncTimeTracking) {
                                        if (syncTimeTracking != null && syncTimeTracking.getResponse_code().equals("0") && position > 0 && position < syncAPITableList.size()) {
                                            syncAPITableList.remove(position);
                                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(userId));
                                            cacheEventsListAdapter.notifyItemRemoved(position);
                                            callSyncAPI(position);
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
                                        callSyncAPI(position + 1);
                                    }
                                }));
                    }
                }
                if (syncAPITableList.size() == 0) {
                    binding.switchClick.setChecked(false);
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
                    Log.e("wifiInfo", "getWifiName:= " + wifiInfo.getSSID());
                    return wifiInfo.getSSID();
                }
            }
        }
        return null;
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

    private void callApiSyncLessonProgress(ArrayList<LessonNewProgress> lessonNewProgress, int position) {
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
                            cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncAPI(position);
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
                            callSyncAPI(position + 1);
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
                            cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncAPI(position);
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
                            callSyncAPI(position + 1);
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
                            cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncAPI(position);
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
                            callSyncAPI(position + 1);
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
                            cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncAPI(position);
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
                            callSyncAPI(position + 1);
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
                            syncAPIDatabaseRepository.deleteById(Integer.parseInt(CacheEventsListActivity.this.userId));
                            cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncAPI(position);
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
                        callSyncAPI(position + 1);
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
                                            LessonProgress lessonProgress = lessonDatabaseRepository.getItemProgressData(syncData.getProgressdata().get(i).getLessonProgressId(), CacheEventsListActivity.this.userId);
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
                                syncAPIDatabaseRepository.deleteById(Integer.parseInt(CacheEventsListActivity.this.userId));
                                cacheEventsListAdapter.notifyItemRemoved(position);
                                callSyncAPI(position);
                            }

                        } catch (Exception e) {
                            //showError(e);
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR===3333" + e.getMessage());
                        }

                        hideDialog();
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

                            callSyncAPI(position + 1);
                            HttpException error = (HttpException) e;
                            SyncRecords syncRecords = new Gson().fromJson(error.response().errorBody().string(), SyncRecords.class);
                            //Log.e(Const.LOG_NOON_TAG, "===SyncRecords=ERROR==" + syncRecords.getMessage());
                            hideDialog();
                        } catch (Exception e1) {
                            //showError(e1);
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR===44444" + e.getMessage());
                        }
                    }
                }));
    }

    public void callApiProgessSyncAdd(List<LessonProgress> lessonProgressList, List<QuizUserResult> quizUserResults, int position, JsonObject jsonArray) {

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
                            cacheEventsListAdapter.notifyItemRemoved(position);
                            callSyncAPI(position);
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

                                callSyncAPI(position + 1);
                                HttpException error = (HttpException) e;
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

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        finishAffinity();
        Intent intent1 = new Intent(CacheEventsListActivity.this, MainDashBoardActivity.class);
        startActivity(intent1);
    }
}
