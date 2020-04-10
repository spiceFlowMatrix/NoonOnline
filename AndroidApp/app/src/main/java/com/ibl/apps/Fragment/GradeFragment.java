package com.ibl.apps.Fragment;

import android.Manifest;
import android.annotation.SuppressLint;
import android.content.Context;
import android.content.DialogInterface;
import android.content.IntentSender;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.HandlerThread;
import android.os.Message;
import android.os.Process;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebSettings;
import android.webkit.WebView;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.core.app.ActivityCompat;
import androidx.databinding.DataBindingUtil;

import com.google.android.gms.common.api.GoogleApiClient;
import com.google.android.gms.common.api.PendingResult;
import com.google.android.gms.common.api.Status;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.location.LocationSettingsRequest;
import com.google.android.gms.location.LocationSettingsResult;
import com.google.android.gms.location.LocationSettingsStatusCodes;
import com.ibl.apps.Adapter.CourseListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.CourseManagement.CourseRepository;
import com.ibl.apps.Interface.CourseAsyncResponse;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.IntervalObject;
import com.ibl.apps.Model.IntervalTableObject;
import com.ibl.apps.RoomDatabase.dao.SyncTimeTrackingDao;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Service.CourseImageManager;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.GradeLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.SingleShotLocationProvider;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Objects;
import java.util.TimeZone;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import pub.devrel.easypermissions.EasyPermissions;
import tcking.github.com.giraffeplayer2.LazyLoadManager;
import tcking.github.com.giraffeplayer2.VideoInfo;

import static tcking.github.com.giraffeplayer2.GiraffePlayer.MSG_CTRL_PLAYING;


public class GradeFragment extends BaseFragment implements View.OnClickListener, LocationListener {

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";
    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    GradeLayoutBinding gradeLayoutBinding;
    List<CourseObject> Courselist = new ArrayList<>();
    List<CourseObject.Data> CourselistData = new ArrayList<>();

    private CompositeDisposable disposable = new CompositeDisposable();
    String[] PERMISSIONS = new String[]{Manifest.permission.CAMERA, Manifest.permission.WRITE_EXTERNAL_STORAGE, Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.ACCESS_COARSE_LOCATION};
    private int SpinnerGradeId = 0;
    private String SearchText = "";
    CourseListAdapter adp;
    UserDetails userDetailsObject;
    String userId = "0";
    private static LocationManager manager;
    boolean isAgree = false;
    private CourseRepository courseRepository;

    public GradeFragment() {
        // Required empty public constructor
    }

    public static GradeFragment newInstance(String param1, String param2) {
        GradeFragment fragment = new GradeFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
        courseRepository = new CourseRepository();
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);

        if (requestCode == 0x01) {
            if (grantResults.length > 0) {
                if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
//                    SharedPreferences sharedPreferences = Objects.requireNonNull(getActivity()).getSharedPreferences(Const.privacyPolicy, Context.MODE_PRIVATE);
//                    if (!sharedPreferences.getBoolean(Const.isAgree, false)) {
//                        showDialog(getResources().getString(R.string.loading));
//                        disposable.add(apiService.getTerms()
//                                .subscribeOn(Schedulers.io())
//                                .observeOn(AndroidSchedulers.mainThread())
//                                .subscribeWith(new DisposableSingleObserver<TemsCondition>() {
//                                    @Override
//                                    public void onSuccess(TemsCondition temsCondition) {
//                                        if (temsCondition.getResponse_code().equals("0")) {
//                                            privarcyPolicyDialog(temsCondition.getData().getTerms());
//                                            hideDialog();
//                                        }
//                                    }
//
//                                    @Override
//                                    public void onError(Throwable e) {
//                                        hideDialog();
//                                    }
//                                }));
//                    }
                    getCurrentLocation();
                }
            }
        }
    }

    @SuppressLint("NewApi")
    @Override
    protected void setUp(View view) {

        showDialog(NoonApplication.getContext().getResources().getString(R.string.loading));
        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();
                    SharedPreferences sharedPreferences = (Objects.requireNonNull(getActivity())).getSharedPreferences("user", Context.MODE_PRIVATE);
                    SharedPreferences.Editor editor = sharedPreferences.edit();
                    editor.putString("uid", userId);
                    editor.apply();
                    if (isNetworkAvailable(getActivity())) {
                        loadData(SearchText, SpinnerGradeId);
                    } else {
                        new setLocalDataTask(new CourseAsyncResponse() {
                            @Override
                            public List<CourseObject.Data> getLocalUserDetails(List<CourseObject.Data> courseListl) {
                                gradeLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                adp = new CourseListAdapter(getActivity(), courseListl, userDetailsObject);
                                gradeLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
                                adp.notifyDataSetChanged();
                                hideDialog();
                                return null;
                            }
                        }).execute();
                    }
                }
                return null;
            }

        }).execute();
        if (!EasyPermissions.hasPermissions(Objects.requireNonNull(getActivity()), PERMISSIONS)) {
            EasyPermissions.requestPermissions(this, NoonApplication.getContext().getResources().getString(R.string.validation_download_permission), 0x01, PERMISSIONS);

        } else {
//            SharedPreferences sharedPreferences = getActivity().getSharedPreferences(Const.privacyPolicy, Context.MODE_PRIVATE);
//            if (!sharedPreferences.getBoolean(Const.isAgree, false)) {
//                showDialog(getResources().getString(R.string.loading));
//                disposable.add(apiService.getTerms()
//                        .subscribeOn(Schedulers.io())
//                        .observeOn(AndroidSchedulers.mainThread())
//                        .subscribeWith(new DisposableSingleObserver<TemsCondition>() {
//                            @Override
//                            public void onSuccess(TemsCondition temsCondition) {
//                                if (temsCondition.getResponse_code().equals("0")) {
//                                    privarcyPolicyDialog(temsCondition.getData().getTerms());
//                                    hideDialog();
//                                }
//                            }
//
//                            @Override
//                            public void onError(Throwable e) {
//                                hideDialog();
//                            }
//                        }));
//            }
            getCurrentLocation();
        }
    }

    private void getCurrentLocation() {
        manager = (LocationManager) Objects.requireNonNull(getActivity()).getSystemService(Context.LOCATION_SERVICE);
        assert manager != null;
        boolean statusOfGPS = manager.isProviderEnabled(LocationManager.GPS_PROVIDER);

        if (!statusOfGPS) {
            displayLocationSettingsRequest(getContext());
        } else {
            enabledLocation(getContext());
            if (ActivityCompat.checkSelfPermission(Objects.requireNonNull(getContext()), Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(getContext(), Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
                return;
            }
            List<String> provider = manager.getProviders(true);
            manager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, this);
            for (int i = 0; i < provider.size(); i++) {
                manager.getLastKnownLocation(provider.get(i));
            }

        }

    }

    public void displayLocationSettingsRequest(Context context) {
        GoogleApiClient googleApiClient = new GoogleApiClient.Builder(context)
                .addApi(LocationServices.API).build();
        googleApiClient.connect();

        LocationRequest locationRequest = LocationRequest.create();
        locationRequest.setPriority(LocationRequest.PRIORITY_HIGH_ACCURACY);
        locationRequest.setInterval(10000);
        locationRequest.setFastestInterval(10000 / 2);

        LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder().addLocationRequest(locationRequest);
        builder.setAlwaysShow(true);

        PendingResult<LocationSettingsResult> result = LocationServices.SettingsApi.checkLocationSettings(googleApiClient, builder.build());
        result.setResultCallback(result1 -> {
            final Status status = result1.getStatus();
            switch (status.getStatusCode()) {
                case LocationSettingsStatusCodes.SUCCESS:
//                        openLocation();
                    enabledLocation(getContext());
                    Log.i("SUCCESS", "All location settings are satisfied.");
                    break;
                case LocationSettingsStatusCodes.RESOLUTION_REQUIRED:
                    Log.i("RESOLUTION_REQUIRED", "Location settings are not satisfied. Show the user a dialog to upgrade location settings ");

                    try {
                        // Show the dialog by calling startResolutionForResult(), and check the result
                        // in onActivityResult().
                        status.startResolutionForResult(getActivity(), 100);
                    } catch (IntentSender.SendIntentException e) {
                        Log.i("SendIntentException", "PendingIntent unable to execute request.");
                    }
                    break;
                case LocationSettingsStatusCodes.SETTINGS_CHANGE_UNAVAILABLE:
                    Log.i("setting Change", "Location settings are inadequate, cannot be fixed here.");
                    break;
            }
        });
    }

    public void enabledLocation(Context context) {
        // when you need location
        // if inside activity context = this;

        SingleShotLocationProvider.requestSingleUpdate(context, this::getLocation);
    }

    private double deg2rad(double deg) {
        return (deg * Math.PI / 180.0);
    }

    private double rad2deg(double rad) {
        return (rad * 180.0 / Math.PI);
    }

    private void getLocation(SingleShotLocationProvider.GPSCoordinates location) {
        if (location != null) {
           // SyncTimeTrackingDao syncTimeTrackingDao = AppDatabase.getAppDatabase(getActivity()).syncTimeTrackingDao();
            SharedPreferences sharedPreferences = NoonApplication.getContext().getSharedPreferences("user", Context.MODE_PRIVATE);
            if (sharedPreferences != null) {
                userId = sharedPreferences.getString("uid", "");
                assert userId != null;

                if (!userId.equals("")) {
                    SyncTimeTrackingObject syncTimeTrackingObject = courseRepository.getSyncTimeTrack(Integer.parseInt(userId));
                    if (syncTimeTrackingObject != null) {
                        //SyncTimeTrackingObject syncTimeTrackingObject = new SyncTimeTrackingObject();
                        syncTimeTrackingObject.setLatitude(String.valueOf(location.latitude));
                        syncTimeTrackingObject.setLongitude(String.valueOf(location.longitude));
                        syncTimeTrackingObject.setOperatingsystem(Const.var_deviceType);
                        syncTimeTrackingObject.setHardwareplatform(Build.MODEL);
                        syncTimeTrackingObject.setServiceprovider(getCarierName());
                        syncTimeTrackingObject.setVersion(Build.VERSION.RELEASE);
                        syncTimeTrackingObject.setUserid(Integer.parseInt(userId));
                        courseRepository.updateSyncTimeTracking(syncTimeTrackingObject);
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
                        courseRepository.insertAll(syncTimeTrackingObjectinsert);
                    }
                }
            }

        }
    }

    private String getUTCTime() {
        SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH);
        sdf.setTimeZone(TimeZone.getTimeZone("UTC"));
        String gmtTime = sdf.format(new Date());
        Log.e("date", "getUTCTime: " + gmtTime);
        return gmtTime;
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        gradeLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.grade_layout, container, false);
        return gradeLayoutBinding.getRoot();
    }

    public void setOnClickListener() {

    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {

        }
    }

    private void loadData(String searchKeyword, int SpinnerGradeId) {
        try {
            showDialog(NoonApplication.getContext().getResources().getString(R.string.loading));
            disposable.add(courseRepository.fetchCourseList(0, 0, searchKeyword, SpinnerGradeId)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<CourseObject>() {
                        @Override
                        public void onSuccess(CourseObject apiCourseObject) {

                            new setOnlineDataTask(new CourseAsyncResponse() {
                                @Override
                                public List<CourseObject.Data> getLocalUserDetails(List<CourseObject.Data> courseListl) {

                                    gradeLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                    adp = new CourseListAdapter(getActivity(), courseListl, userDetailsObject);
                                    gradeLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
                                    adp.notifyDataSetChanged();
                                    fetchIntervalAPI();

                                    return null;
                                }
                            }, apiCourseObject).execute();
                        }

                        @Override
                        public void onError(Throwable e) {
                            hideDialog();
                            try {
                                //showSnackBar(gradeLayoutBinding.mainGradeLayout, apiCourseObject.getMessage());
                                new setLocalDataTask(new CourseAsyncResponse() {
                                    @Override
                                    public List<CourseObject.Data> getLocalUserDetails(List<CourseObject.Data> courseListl) {

                                        if (courseListl != null) {
                                            gradeLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                            adp = new CourseListAdapter(getActivity(), courseListl, userDetailsObject);
                                            gradeLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
                                            adp.notifyDataSetChanged();
                                        } else {
                                            showError(e);
                                        }

                                        hideDialog();
                                        return null;
                                    }
                                }).execute();

                            } catch (Exception e1) {
                                //showError(e);

                                new setLocalDataTask(new CourseAsyncResponse() {
                                    @Override
                                    public List<CourseObject.Data> getLocalUserDetails(List<CourseObject.Data> courseListl) {

                                        if (courseListl != null) {
                                            gradeLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                            adp = new CourseListAdapter(getActivity(), courseListl, userDetailsObject);
                                            gradeLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
                                            adp.notifyDataSetChanged();
                                        } else {
                                            showError(e);
                                        }
                                        hideDialog();
                                        return null;
                                    }
                                }).execute();
                            }
                        }
                    }));

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void fetchIntervalAPI() {
        try {
            disposable.add(courseRepository.fetchinterval()
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<IntervalObject>() {
                        @Override
                        public void onSuccess(IntervalObject intervalObject) {
                            IntervalTableObject intervalTableObject = courseRepository.getAllInterval();
                            if (intervalTableObject != null) {
                                courseRepository.updateINterval(intervalObject.getData().getInterval(), intervalTableObject.getIntervalTableID());
                            } else {
                                intervalTableObject = new IntervalTableObject();
                                intervalTableObject.setInterval(intervalObject.getData().getInterval());
                                courseRepository.insertAll(intervalTableObject);
                            }

                            hideDialog();
                        }

                        @Override
                        public void onError(Throwable e) {
                            try {
                                //showSnackBar(gradeLayoutBinding.mainGradeLayout, apiCourseObject.getMessage());
                            } catch (Exception e1) {
                                //showError(e);
                            }
                            hideDialog();
                        }
                    }));

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onLocationChanged(Location location) {
        // Get System TELEPHONY service reference
        manager.removeUpdates(this);

    }

    private String getCarierName() {

        TelephonyManager tManager = (TelephonyManager) Objects.requireNonNull(getActivity()).getBaseContext()
                .getSystemService(Context.TELEPHONY_SERVICE);
        return tManager.getNetworkOperatorName();
    }

    @Override
    public void onStatusChanged(String s, int i, Bundle bundle) {
        Log.d("latlong----", "onLocationStatusChanged: " + s);
    }

    @Override
    public void onProviderEnabled(String s) {
        Log.d("latlong----", "onLocationEnabledChanged: " + s);
    }

    @Override
    public void onProviderDisabled(String s) {
        Log.d("latlong----", "onLocationDisabledChanged: " + s);
    }

    public class setLocalDataTask extends AsyncTask<Void, Void, List<CourseObject.Data>> {

        CourseAsyncResponse delegate = null;

        setLocalDataTask(CourseAsyncResponse delegate) {
            this.delegate = delegate;
        }

        @Override
        protected void onPreExecute() {
            showDialog(NoonApplication.getContext().getResources().getString(R.string.loading));
            super.onPreExecute();
        }

        @Override
        protected List<CourseObject.Data> doInBackground(Void... params) {

            try {
                Courselist.clear();
                CourselistData.clear();

                if (courseRepository.getAllCourse(userId) != null) {
                    if (courseRepository.getAllCourse(userId).size() != 0) {
                        Courselist = courseRepository.getAllCourse(userId);
                        for (int i = 0; i < Courselist.size(); i++) {
                            CourselistData = Courselist.get(i).getData();
                            if (CourselistData != null && CourselistData.size() != 0) {
                                for (int j = 0; j < CourselistData.size(); j++) {
                                    if (CourselistData.get(j).getCourses().size() != 0) {

                                        for (int k = 0; k < CourselistData.get(j).getCourses().size(); k++) {
                                            //Log.e(Const.LOG_NOON_TAG, "=====GRADE FRAGMENT=IDDDD==" + CourselistData.get(j).getCourses().get(k).getId());
                                            if (!CourselistData.get(j).getCourses().get(k).isDeleted()) {


                                                byte[] bitmapImage = courseRepository.getCourseImage(userId, CourselistData.get(j).getCourses().get(k).getId());
                                                Log.e(Const.LOG_NOON_TAG, "=====GRADE FRAGMENT=bitmapImage==" + bitmapImage);

                                                if (bitmapImage != null) {
                                                    CourselistData.get(j).getCourses().get(k).setCourseImage(bitmapImage);
                                                }

                                                int totalTrueLesson = courseRepository.getItemgradeIdProgress(CourselistData.get(j).getCourses().get(k).getId(), "100", userId);
                                                String totalLEssonITEm = courseRepository.getStringProgress(CourselistData.get(j).getCourses().get(k).getId(), userId);

                                                if (totalLEssonITEm != null) {
                                                    int totalCount = Integer.parseInt(totalLEssonITEm);
                                                    if (totalCount != 0 && totalTrueLesson != 0) {
                                                        int countper = (int) ((totalTrueLesson + 1) * 100 / totalCount);
                                                        if (countper != 0) {
                                                            CourselistData.get(j).getCourses().get(k).setProgressVal(countper);
                                                        }
                                                    }
                                                }
                                            } else {
                                                CourselistData.get(j).getCourses().remove(k);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            } catch (Exception e) {
                e.printStackTrace();
            }
            return CourselistData;
        }

        @Override
        protected void onPostExecute(List<CourseObject.Data> varProgressArr) {
            delegate.getLocalUserDetails(varProgressArr);
        }
    }

    public class setOnlineDataTask extends AsyncTask<Void, Void, List<CourseObject.Data>> {

        CourseObject apiCourseObject;
        CourseAsyncResponse delegate = null;

        public setOnlineDataTask(CourseAsyncResponse delegate, CourseObject apiCourseObject) {
            this.delegate = delegate;
            this.apiCourseObject = apiCourseObject;
        }

        @Override
        protected List<CourseObject.Data> doInBackground(Void... params) {

            try {
                apiCourseObject.setUserId(userDetailsObject.getId());
                Courselist.add(apiCourseObject);

                CourseObject exiestCourseObject = courseRepository.getAllCourseObject(userId);
                if (exiestCourseObject != null) {
                    courseRepository.updateAll(apiCourseObject.getData(), userId);
                } else {
                    courseRepository.insertAll(apiCourseObject);
                }

                for (int i = 0; i < Courselist.size(); i++) {
                    CourselistData = Courselist.get(i).getData();
                    for (int j = 0; j < CourselistData.size(); j++) {
                        for (int k = 0; k < CourselistData.get(j).getCourses().size(); k++) {

                            int totalTrueLesson = courseRepository.getItemgradeIdProgress(CourselistData.get(j).getCourses().get(k).getId(), "100", userId);
                            String totalLEssonITEm = courseRepository.getStringProgress(CourselistData.get(j).getCourses().get(k).getId(), userId);

                            if (totalLEssonITEm != null) {
                                int totalCount = Integer.parseInt(totalLEssonITEm);
                                if (totalCount != 0 && totalTrueLesson != 0) {
                                    int countper = (int) ((totalTrueLesson + 1) * 100 / totalCount);
                                    if (countper != 0) {
                                        CourselistData.get(j).getCourses().get(k).setProgressVal(countper);
                                    }
                                }
                            }
                        }
                    }
                }
                CourseImageManager.start(getActivity(), apiCourseObject, userId);
                startPlayerDownload();

            } catch (Exception e) {
                e.printStackTrace();
            }

            return CourselistData;
        }

        @Override
        protected void onPostExecute(List<CourseObject.Data> CourselistData) {
            delegate.getLocalUserDetails(CourselistData);
        }
    }

    public void startPlayerDownload() {
        HandlerThread internalPlaybackThread;
        internalPlaybackThread = new HandlerThread("GiraffePlayerInternal:Handler", Process.THREAD_PRIORITY_AUDIO);
        internalPlaybackThread.start();
        Handler handler = new Handler(internalPlaybackThread.getLooper(), new Handler.Callback() {
            @Override
            public boolean handleMessage(Message msg) {
                LazyLoadManager.Load(getActivity(), new VideoInfo().getFingerprint(), Message.obtain(msg));

                return false;
            }
        });
        handler.sendEmptyMessage(MSG_CTRL_PLAYING);
    }

    @Override
    public void onResume() {

        if (Courselist != null && Courselist.size() != 0) {

            new AsyncTask<Void, Void, String>() {
                @Override
                protected String doInBackground(Void... voids) {
                    for (int i = 0; i < Courselist.size(); i++) {
                        CourselistData = Courselist.get(i).getData();
                        if (CourselistData != null && CourselistData.size() != 0) {
                            for (int j = 0; j < CourselistData.size(); j++) {
                                for (int k = 0; k < CourselistData.get(j).getCourses().size(); k++) {
                                    int totalTrueLesson = courseRepository.getItemgradeIdProgress(CourselistData.get(j).getCourses().get(k).getId(), "100", userId);
                                    String totalLEssonITEm = courseRepository.getStringProgress(CourselistData.get(j).getCourses().get(k).getId(), userId);

                                    if (totalLEssonITEm != null) {
                                        int totalCount = Integer.parseInt(totalLEssonITEm);
                                        if (totalCount != 0 && totalTrueLesson != 0) {
                                            int countper = (int) ((totalTrueLesson + 1) * 100 / totalCount);
                                            if (countper != 0) {
                                                CourselistData.get(j).getCourses().get(k).setProgressVal(countper);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return null;
                }
            }.execute();


            if (adp != null) {
                adp.notifyDataSetChanged();
            }
        }
        super.onResume();
    }

    private void privarcyPolicyDialog(String terms) {
        AlertDialog.Builder alert = new AlertDialog.Builder(Objects.requireNonNull(getActivity()));
        WebView webView = new WebView(getActivity());
        webView.getSettings().setJavaScriptEnabled(true);
        webView.getSettings().setJavaScriptCanOpenWindowsAutomatically(true);
        webView.getSettings().setLoadsImagesAutomatically(true);
        webView.getSettings().setPluginState(WebSettings.PluginState.ON);
        webView.loadDataWithBaseURL(null, terms, "text/html", "UTF-8", null);
        alert.setView(webView);

        alert.setNegativeButton("Agree", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int i) {
                SharedPreferences sharedPreferences = getActivity().getSharedPreferences(Const.privacyPolicy, Context.MODE_PRIVATE);
                SharedPreferences.Editor editor = sharedPreferences.edit();
                editor.putBoolean(Const.isAgree, true);
                editor.apply();
                dialog.dismiss();
            }
        });

        alert.create();
        alert.show();
    }

}
