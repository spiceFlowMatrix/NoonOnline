package com.ibl.apps.Fragment;

import android.app.AlertDialog;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.Typeface;
import android.graphics.drawable.Drawable;
import android.location.Location;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.core.content.ContextCompat;
import androidx.core.content.res.ResourcesCompat;
import androidx.databinding.DataBindingUtil;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.github.mikephil.charting.components.XAxis;
import com.github.mikephil.charting.components.YAxis;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.highlight.Highlight;
import com.github.mikephil.charting.interfaces.datasets.ILineDataSet;
import com.github.mikephil.charting.listener.ChartTouchListener;
import com.github.mikephil.charting.listener.OnChartGestureListener;
import com.github.mikephil.charting.listener.OnChartValueSelectedListener;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.BitmapDescriptorFactory;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.maps.model.TileOverlay;
import com.google.android.gms.maps.model.TileOverlayOptions;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.google.maps.android.heatmaps.Gradient;
import com.google.maps.android.heatmaps.HeatmapTileProvider;
import com.google.maps.android.ui.IconGenerator;
import com.ibl.apps.Adapter.UserLocationDetailListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Interface.BackInterface;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.LocationData;
import com.ibl.apps.Model.parent.Chart;
import com.ibl.apps.Model.parent.GraphXAxisValueFormatter;
import com.ibl.apps.Model.parent.LastOnline;
import com.ibl.apps.Model.parent.ParentGraphPoints;
import com.ibl.apps.Model.parent.ParentSpinnerModel;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.ParentControlManagement.ParentControlRepository;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.UserDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentProgressReportBinding;
import com.ibl.apps.noon.databinding.MarkerViewBinding;
import com.ibl.apps.util.CustomView.MultiSelectSpinner;
import com.ibl.apps.util.LoadMoreData.OnLoadMoreListener;
import com.ibl.apps.util.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.TimeAgoLastOnlineClass;
import com.ibl.apps.util.WorkaroundMapFragment;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.InputStream;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Objects;
import java.util.Scanner;
import java.util.TimeZone;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;

import static android.content.Context.MODE_PRIVATE;


public class ProgressReportFragment extends BaseFragment implements View.OnClickListener {
    FragmentProgressReportBinding fragmentProgressReportBinding;
    public static ArrayList<ParentSpinnerModel.Data> selectedIdsForCallback = new ArrayList<>();
    BackInterface backInterface;
    private UserDetails userDetails;
    private CompositeDisposable disposable = new CompositeDisposable();
    private ApiService apiService;
    private int userId;
    private int months;
    private GoogleMap mMap;
    Location myLocation;
    boolean markerClicked;
    private LocationData locationdata;
    private ArrayList<LocationData.Data> data = new ArrayList<>();
    private HeatmapTileProvider mProvider;
    private TileOverlay mOverlay;
    private List<ParentSpinnerModel.Data> studentNamecategories;
    private ParentSpinnerModel parentSpinner = new ParentSpinnerModel();
    private RecyclerViewLoadMoreScroll scrollListener;
    private boolean isLoad = true;
    int pagenumber = 1;
    int perpagerecord = 20;
    List<ParentGraphPoints.Data> paretnData = new ArrayList<>();
    private ParentControlRepository parentControlRepository;
    private UserDatabaseRepository userDatabaseRepository;
    private String deviceStatusCode;


    public static ProgressReportFragment newInstance(String param1, String param2) {
        return new ProgressReportFragment();
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Override
    protected void setUp(View view) {
        showDialog(Objects.requireNonNull(getActivity()).getResources().getString(R.string.loading));
        parentControlRepository = new ParentControlRepository();
        userDatabaseRepository = new UserDatabaseRepository();
//        callApiLocationData();

        SharedPreferences deviceSharedPreferences = getActivity().getSharedPreferences("deviceStatus", MODE_PRIVATE);
        deviceStatusCode = deviceSharedPreferences.getString("deviceStatusCode", "");
        Log.e("deviceStatusCode", "setUp:progress " + deviceStatusCode);
        if (deviceStatusCode != null)
            switch (deviceStatusCode) {
                case "0":
                    fragmentProgressReportBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                    fragmentProgressReportBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                    fragmentProgressReportBinding.nestedScrollView.setVisibility(View.VISIBLE);
                    setUpAllSpinners();
                    setonclicklistner();

                    break;
                case "2":
                    fragmentProgressReportBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.VISIBLE);
                    fragmentProgressReportBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                    //fragmentProgressReportBinding.advanceSearchLayout.mainAdvanceSearchLayout.setVisibility(View.GONE);
                    fragmentProgressReportBinding.nestedScrollView.setVisibility(View.GONE);
                    break;
                case "3":
                    fragmentProgressReportBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                    fragmentProgressReportBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.VISIBLE);
                    //fragmentProgressReportBinding.advanceSearchLayout.mainAdvanceSearchLayout.setVisibility(View.GONE);
                    fragmentProgressReportBinding.nestedScrollView.setVisibility(View.GONE);
                    break;
                case "4":
                    Toast.makeText(getActivity(), getActivity().getResources().getString(R.string.invalid_mac_ip), Toast.LENGTH_LONG).show();
                    break;
            }


        backInterface = (BackInterface) getActivity();
        if (!selectedIdsForCallback.isEmpty()) {
            selectedIdsForCallback.clear();
        }
        //addHeatMap();
    }

    private void setUpAllSpinners() {
        callApiForStudentSpinner();
    }

    private void callApiForStudentSpinner() {
        String authid = PrefUtils.getAuthid(NoonApplication.getContext());
        if (!TextUtils.isEmpty(authid)) {
            AuthTokenObject authTokenObject = userDatabaseRepository.getAuthTokenData(authid);

            if (authTokenObject != null) {
                String sub = "";
                if (authTokenObject.getSub() != null) {
                    sub = authTokenObject.getSub();
                    userDetails = userDatabaseRepository.getUserDetails(sub);
                }
            }
        }

        disposable.add(parentControlRepository.getUserSpinnerData(Integer.parseInt(userDetails.getId()))
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ParentSpinnerModel>() {
                    @Override
                    public void onSuccess(ParentSpinnerModel parentSpinnerModel) {

                        if (parentSpinnerModel != null) {
                            setDataInStudentSpinner(parentSpinnerModel);
                            parentSpinner = parentSpinnerModel;
                        }
                    }

                    @Override
                    public void onError(Throwable e) {

                    }
                }));
    }

    private void setDataInStudentSpinner(ParentSpinnerModel parentSpinnerModel) {

        studentNamecategories = parentSpinnerModel.getData();
//        ParentSpinnerModel.FillesData data = new ParentSpinnerModel.FillesData();
//        data.setUsername(Objects.requireNonNull(getString(R.string.all_users)));
//        data.setId(0L);
//        studentNamecategories.add(0, data);
        // MyAdapter studentNameAdapter = new MyAdapter(Objects.requireNonNull(getActivity()), 0, studentNamecategories, this);
//        ArrayAdapter<ParentSpinnerModel.FillesData> studentNamedataAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, studentNamecategories);
//        // Drop down layout style - list view with radio button
//        studentNamedataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);

        ArrayList<Integer> userArray = new ArrayList<>();
        if (selectedIdsForCallback.isEmpty()) {
            for (int j = 0; j < parentSpinnerModel.getData().size(); j++) {
                if (parentSpinnerModel.getData().get(j).getId() != 0L) {
                    selectedIdsForCallback.add(parentSpinnerModel.getData().get(j));
                    userArray.add(Integer.parseInt(String.valueOf(parentSpinnerModel.getData().get(j).getId())));
                }
            }
        } else {
            for (int i = 0; i < selectedIdsForCallback.size(); i++) {
                userArray.add(Integer.parseInt(String.valueOf(selectedIdsForCallback.get(i).getId())));
            }

        }
//        if (!selectedIdsForCallback.isEmpty()) {
//            for (int i = 0; i < selectedIdsForCallback.size(); i++) {
//                userArray.add(Integer.parseInt(String.valueOf(selectedIdsForCallback.get(i).getId())));
//            }
//        }

        callMonthSpinner(userArray);
        if (parentSpinnerModel.getData().size() == selectedIdsForCallback.size()) {
            fragmentProgressReportBinding.allUser.setVisibility(View.VISIBLE);
            fragmentProgressReportBinding.spUser.setText(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_users));
            fragmentProgressReportBinding.allUser.setText("( " + userArray.size() + " )");
        } else {
            fragmentProgressReportBinding.spUser.setText(userArray.size() + " " + getActivity().getResources().getString(R.string.user_selected));
            fragmentProgressReportBinding.allUser.setVisibility(View.GONE);
        }

        MultiSelectSpinner multiSelectSpinner = new MultiSelectSpinner()
                .title(getActivity().getResources().getString(R.string.select_user)) //setting title for dialog
                .titleSize(25)
                .positiveText(getActivity().getResources().getString(R.string.done))
                .negativeText(getActivity().getResources().getString(R.string.cancel))
                .setMinSelectionLimit(1) //you can set minimum checkbox selection limit (Optional)
                .setMaxSelectionLimit(studentNamecategories.size()) //you can set maximum checkbox selection limit (Optional)
                .multiSelectList((ArrayList<ParentSpinnerModel.Data>) studentNamecategories)
                .onSubmit(new MultiSelectSpinner.SubmitCallbackListener() {
                    @Override
                    public void onSelected(ArrayList<ParentSpinnerModel.Data> var1, ArrayList<String> var2, String var3) {
                        if (parentSpinnerModel.getData().size() == selectedIdsForCallback.size()) {
                            fragmentProgressReportBinding.allUser.setVisibility(View.VISIBLE);
                            fragmentProgressReportBinding.spUser.setText(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_users));
                            fragmentProgressReportBinding.allUser.setText("( " + var1.size() + " )");
                        } else {
                            fragmentProgressReportBinding.spUser.setText(var1.size() + " " + getActivity().getResources().getString(R.string.user_selected));
                            fragmentProgressReportBinding.allUser.setVisibility(View.GONE);
                        }
                        ArrayList<Integer> userArray = new ArrayList<>();
                        for (int i = 0; i < var1.size(); i++) {
                            userArray.add(Integer.parseInt(String.valueOf(var1.get(i).getId())));
                        }
                        callMonthSpinner(userArray);

                    }

                    @Override
                    public void onCancel() {

                    }
                });
        fragmentProgressReportBinding.spUserLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                multiSelectSpinner.show(Objects.requireNonNull(getActivity()).getSupportFragmentManager(), "multiSelectDialog");
            }
        });

        //fragmentProgressReportBinding.studentName.setAdapter(studentNameAdapter);


        /*ArrayAdapter<ParentSpinnerModel.FillesData> studentNamedataAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, studentNamecategories);
        // Drop down layout style - list view with radio button
        studentNamedataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);*/
       /* List<ParentSpinnerModel.FillesData> finalStudentNamecategories = studentNamecategories;
        fragmentProgressReportBinding.studentName.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                CheckBox checkBox =view.findViewById(R.id.checkbox);
                checkBox.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                    @Override
                    public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {

                    }
                });
                ParentSpinnerModel.FillesData data = finalStudentNamecategories.get(i);
                userId = Integer.parseInt(String.valueOf(data.getId()));
                userIdArray.add(userId);

            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });*/

    }

    private String getDateUTC(String ourDate) {
        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.ENGLISH);
        format.setTimeZone(TimeZone.getTimeZone("UTC"));
        String returnDate = null;
        try {
            Date date = format.parse(ourDate);
            format.setTimeZone(TimeZone.getDefault());
            returnDate = format.format(date);
        } catch (ParseException e) {
            e.printStackTrace();
        }
        return returnDate;
    }

    private void callApiLocationData(ArrayList<Integer> userArray) {
        JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userArray.size(); i++) {
                array.add(userArray.get(i));
            }

            jsonObject.add("id", array);
            jsonObject.addProperty("month", 0);
            jsonObject.addProperty("courseid", 0);
            jsonObject.addProperty("content", 0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }

        disposable.add(parentControlRepository.getUsersLocation(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<LocationData>() {
                    @Override
                    public void onSuccess(LocationData locationData) {
                        locationdata = locationData;
                        if (locationData != null) {

                            data = (ArrayList<LocationData.Data>) locationData.getData();

                            SupportMapFragment mapFragment = (WorkaroundMapFragment) getChildFragmentManager().findFragmentById(R.id.map);
                            assert mapFragment != null;
                            mapFragment.getMapAsync(new OnMapReadyCallback() {
                                @Override
                                public void onMapReady(GoogleMap googleMap) {
                                    mMap = googleMap;
                                    mMap.clear();
                                    if (data != null) {
                                        for (int i = 0; i < data.size(); i++) {
                                            if (data.get(i).getLat() != null && data.get(i).getLng() != null && data.get(i).getUser().getUsername() != null) {
                                                LatLng latLng = new LatLng(Double.parseDouble(data.get(i).getLat()), Double.parseDouble(data.get(i).getLng()));
                                                mMap.addMarker(new MarkerOptions().position(latLng).icon(BitmapDescriptorFactory.fromBitmap(getMarkerIconWithLabel(data.get(i).getUser().getUsername())))).showInfoWindow();
                                                mMap.moveCamera(CameraUpdateFactory.newLatLng(latLng));
                                            }
                                        }
                                    }


                                    mMap.getUiSettings().setMapToolbarEnabled(false);
                                    mMap.getUiSettings().setZoomControlsEnabled(true);
                                    mMap.setMapType(GoogleMap.MAP_TYPE_NORMAL);
                                    markerClicked = false;
                                    //parent scrollview in xml, give your scrollview id value

                                }
                            });
                            ((WorkaroundMapFragment) Objects.requireNonNull(getChildFragmentManager().findFragmentById(R.id.map)))
                                    .setListener(new WorkaroundMapFragment.OnTouchListener() {
                                        @Override
                                        public void onTouch() {
                                            fragmentProgressReportBinding.nestedScrollView.requestDisallowInterceptTouchEvent(true);
                                        }
                                    });

                        }
                    }

                    @Override
                    public void onError(Throwable e) {

                        ((WorkaroundMapFragment) Objects.requireNonNull(getChildFragmentManager().findFragmentById(R.id.map)))
                                .setListener(new WorkaroundMapFragment.OnTouchListener() {
                                    @Override
                                    public void onTouch() {
                                        fragmentProgressReportBinding.nestedScrollView.requestDisallowInterceptTouchEvent(true);
                                    }
                                });
                    }
                }));
    }

    private void callMonthSpinner(ArrayList<Integer> userArray) {
        if (userArray.size() == 1) {
            JsonObject jsonobject = new JsonObject();
            try {
                JsonArray array = new JsonArray();
                for (int i = 0; i < userArray.size(); i++) {
                    array.add(userArray.get(i));
                }

                jsonobject.add("id", array);
                jsonobject.addProperty("month", 0);
                jsonobject.addProperty("courseid", 0);
                jsonobject.addProperty("content", 0);
            } catch (JsonSyntaxException e) {
                e.printStackTrace();
            }
            disposable.add(parentControlRepository.getLastOnline(jsonobject)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<LastOnline>() {
                        @Override
                        public void onSuccess(LastOnline lastOnline) {
                            if (lastOnline != null && lastOnline.getData() != null) {
                                TimeAgoLastOnlineClass timeAgo = new TimeAgoLastOnlineClass();
                                String getDate = getDateUTC(lastOnline.getData());
                                String timeagotxt = timeAgo.covertTimeToText(getDate);
                                fragmentProgressReportBinding.tvLastonline.setText(timeagotxt);
                            } else {
                                fragmentProgressReportBinding.tvLastonline.setText("--");
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            fragmentProgressReportBinding.tvLastonline.setText("--");
                        }
                    }));
        } else {
            fragmentProgressReportBinding.tvLastonline.setText("--");
        }

        /*if (userDetails != null) {
            if (userDetails.isIsallowmap()) {
                fragmentProgressReportBinding.mapLay.setVisibility(View.VISIBLE);
                callApiLocationData(userArray);
            } else {
                fragmentProgressReportBinding.mapLay.setVisibility(View.GONE);
            }
        }*/

        List<String> monthscategories = new ArrayList<>();
        monthscategories.add(0, Objects.requireNonNull(getActivity()).getResources().getString(R.string.last_one_month));
        monthscategories.add(1, getActivity().getResources().getString(R.string.last_three_month));
        monthscategories.add(2, getActivity().getResources().getString(R.string.last_six_month));
        // Creating adapter for spinner
        ArrayAdapter<String> monthsdataAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, monthscategories);
        // Drop down layout style - list view with radio button
        monthsdataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);

        fragmentProgressReportBinding.monthsSpinner.setAdapter(monthsdataAdapter);
        fragmentProgressReportBinding.monthsSpinner.setSelection(2);
        fragmentProgressReportBinding.monthsSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                switch (i) {
                    case 0:
                        months = 1;
                        drawAChart(userArray);
                        break;
                    case 1:
                        months = 3;
                        drawAChart(userArray);
                        break;
                    case 2:
                        months = 6;
                        drawAChart(userArray);
                        break;
                }

            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });
    }


    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        fragmentProgressReportBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_progress_report, container, false);
        return fragmentProgressReportBinding.getRoot();
    }

    private void setonclicklistner() {
        fragmentProgressReportBinding.GPALay.setOnClickListener(this);
        fragmentProgressReportBinding.overAllLay.setOnClickListener(this);
        fragmentProgressReportBinding.assignmentLay.setOnClickListener(this);
        fragmentProgressReportBinding.quizLay.setOnClickListener(this);
        fragmentProgressReportBinding.btnbackProgress.setOnClickListener(this);
    }


    @Override
    public void onClick(View view) {

        switch (view.getId()) {
            case R.id.overAllLay:
                openFragmentBackstack(new OverAllProgressFragment(), "OverAllProgressFragment");
                break;
            case R.id.quizLay:
                openFragmentBackstack(new AssignmentQuizePerformanceFragment(), "AssignmentQuizePerformanceFragment", 0);
                break;
            case R.id.assignmentLay:
                openFragmentBackstack(new AssignmentQuizePerformanceFragment(), "AssignmentQuizePerformanceFragment", 1);
                break;
            case R.id.GPALay:
                openFragmentBackstack(new GpaFragment(), "GpaFragment");
                break;
            case R.id.btnbackProgress:
                backInterface.backfragment();
                //Objects.requireNonNull(getActivity()).onBackPressed();
                break;
        }
    }


    private void openFragmentBackstack(Fragment fragment, String tag) {
        Fragment removefragment = Objects.requireNonNull(getActivity()).getSupportFragmentManager().findFragmentByTag("ProgressReportFragment");
        assert removefragment != null;
        Objects.requireNonNull(getActivity()).getSupportFragmentManager()
                .beginTransaction()
                .remove(removefragment)
                .addToBackStack(tag)
                .replace(R.id.frame, fragment, tag)
                .commit();
    }

    private void openFragmentBackstack(Fragment fragment, String tag, int flag) {
        Bundle bundle = new Bundle();
        bundle.putInt("flag", flag);
        Fragment removefragment = Objects.requireNonNull(getActivity()).getSupportFragmentManager().findFragmentByTag("ProgressReportFragment");
        assert removefragment != null;
        Objects.requireNonNull(getActivity()).getSupportFragmentManager()
                .beginTransaction()
                .remove(removefragment)
                .addToBackStack(tag)
                .replace(R.id.frame, fragment, tag)
                .commit();
        fragment.setArguments(bundle);
    }

    public String setLocale(String language) {
        Locale locale = new Locale(language);

        Locale.setDefault(locale);
        Configuration configuration = new Configuration();
        configuration.locale = locale;
        Objects.requireNonNull(getActivity()).getBaseContext().getResources().updateConfiguration(configuration, getActivity().getBaseContext().getResources().getDisplayMetrics());

        SharedPreferences.Editor editor = getActivity().getSharedPreferences("Settings", MODE_PRIVATE).edit();
        editor.putString("My_Language", language);
        editor.apply();
        return language;
    }

    private void drawAChart(ArrayList<Integer> userArray) {
        callApiForChart(userArray);
    }

    private void callApiForChart(ArrayList<Integer> userArray) {
        JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userArray.size(); i++) {
                array.add(userArray.get(i));
            }

            jsonObject.add("id", array);
            jsonObject.addProperty("month", months);
            jsonObject.addProperty("courseid", 0);
            jsonObject.addProperty("content", 0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }

        disposable.add(parentControlRepository.getOverAllChartData(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<Chart>() {
                    @Override
                    public void onSuccess(Chart chart) {
                        if (chart != null && chart.getData() != null && chart.getData() != null) {
                            fragmentProgressReportBinding.lineChart.setVisibility(View.VISIBLE);
                            fragmentProgressReportBinding.txtNodata.setVisibility(View.GONE);
                            Chart.Data data = chart.getData();
//                            drawMonthChart(data);
                            drawDateChart(data);

                        } else {
                            fragmentProgressReportBinding.lineChart.setVisibility(View.GONE);
                            fragmentProgressReportBinding.txtNodata.setVisibility(View.VISIBLE);
                        }
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));
    }

    private void drawMonthChart(Chart.Data data) {
        fragmentProgressReportBinding.lineChart.clear();
        fragmentProgressReportBinding.lineChart.resetZoom();
        fragmentProgressReportBinding.lineChart.zoomOut();
        LineDataSet lineDataSet = new LineDataSet(dataForMonth(data.getMo()), "");

        Drawable drawable = ContextCompat.getDrawable(NoonApplication.getContext(), R.drawable.graphview_shadow);
        lineDataSet.setFillDrawable(drawable);
        lineDataSet.setCircleRadius(6f);
        lineDataSet.setLineWidth(3f);
        lineDataSet.setCircleHoleRadius(4f);
        lineDataSet.setDrawCircleHole(true);
        lineDataSet.setDrawIcons(false);
        lineDataSet.setValueFormatter((value, entry, dataSetIndex, viewPortHandler) -> "");
        lineDataSet.setMode(LineDataSet.Mode.HORIZONTAL_BEZIER);   //set curves in ghaphview
        lineDataSet.setDrawFilled(true);
//        lineDataSet.setGradientColor(Color.BLUE,0);
//        lineDataSet.setMode(LineDataSet.Mode.CUBIC_BEZIER);
//        lineDataSet.setFillColor(getResources().getColor(R.color.colorPrimaryDark));
//        lineDataSet.setDrawFilled(true); // fill color under the graphview
//        lineDataSet.setValueTextSize(9f);
//        lineDataSet.setColor(Color.BLACK); //Change Line color
//        lineDataSet.setCircleColor(Color.BLACK); //Change Circle Color
        final ArrayList<String> yLabel = new ArrayList<>();

        int max = getMax(dataForMonth(data.getMo()));
        for (int i = 0; i <= max; i++) {
            yLabel.add(i + " Hours");
        }
        YAxis yAxis = fragmentProgressReportBinding.lineChart.getAxisLeft();
        yAxis.setDrawGridLines(true);
        yAxis.setAxisLineColor(Color.parseColor("#00000000"));
        yAxis.setTextColor(Color.BLACK);
        yAxis.setValueFormatter((value, axis) -> {
            if (yLabel.size() >= value) {
                return Math.round(value) + " Hours";
            }
            return "";
        });
        final ArrayList<String> xLabel = new ArrayList<>();
//                            xLabel.add("month");
        xLabel.add("Jan");
        xLabel.add("Feb");
        xLabel.add("Mar");
        xLabel.add("Apr");
        xLabel.add("May");
        xLabel.add("Jun");
        xLabel.add("July");
        xLabel.add("Aug");
        xLabel.add("Sept");
        xLabel.add("Oct");
        xLabel.add("Nov");
        xLabel.add("Dec");
        XAxis xAxis = fragmentProgressReportBinding.lineChart.getXAxis();
        xAxis.setDrawGridLines(false);
        xAxis.setDrawAxisLine(true);
        xAxis.setAxisLineColor(Color.WHITE);
        xAxis.setTextColor(Color.BLACK);

//                            xAxis.setValueFormatter((value, axis) -> {
//                                if (xLabel.size() >= value) {
//                                    return xLabel.get(Math.round(value));
//                                }
//                                return "";
//                            });
        xAxis.setValueFormatter(new GraphXAxisValueFormatter(data.getMo(), 1, data.getDa()));
        ArrayList<ILineDataSet> iLineDataSets = new ArrayList<>();
        iLineDataSets.add(lineDataSet);
        LineData data1 = new LineData(iLineDataSets);
        fragmentProgressReportBinding.lineChart.setBackgroundColor(Color.WHITE);
//        Animation animation = AnimationUtils.loadAnimation(NoonApplication.getContext(),
//                android.R.anim.slide_out_right);
//        fragmentProgressReportBinding.lineChart.animateX(2000,Easing.EaseInCubic);
//        fragmentProgressReportBinding.lineChart.setAnimation(animation);
        fragmentProgressReportBinding.lineChart.getAxisRight().setDrawLabels(false);
        fragmentProgressReportBinding.lineChart.getAxisRight().setDrawGridLines(false);
        fragmentProgressReportBinding.lineChart.getAxisRight().setAxisLineColor(Color.parseColor("#00000000"));
        fragmentProgressReportBinding.lineChart.getAxisLeft().setDrawLabels(true);
        fragmentProgressReportBinding.lineChart.getAxisLeft().setEnabled(true);
        fragmentProgressReportBinding.lineChart.getAxisLeft().setPosition(YAxis.YAxisLabelPosition.OUTSIDE_CHART);
        fragmentProgressReportBinding.lineChart.getAxisLeft().setGridColor(getResources().getColor(R.color.colorlightDarkGray));
        fragmentProgressReportBinding.lineChart.getXAxis().setDrawLabels(true);
        fragmentProgressReportBinding.lineChart.getXAxis().setEnabled(true);
        fragmentProgressReportBinding.lineChart.getXAxis().setPosition(XAxis.XAxisPosition.BOTTOM_INSIDE);
        fragmentProgressReportBinding.lineChart.setData(data1);
        fragmentProgressReportBinding.lineChart.getDescription().setEnabled(false);
        fragmentProgressReportBinding.lineChart.getXAxis().setGranularity(1);
        fragmentProgressReportBinding.lineChart.getAxisRight().setGranularity(1);
        fragmentProgressReportBinding.lineChart.setDragEnabled(true);
        fragmentProgressReportBinding.lineChart.setScaleEnabled(true);
        fragmentProgressReportBinding.lineChart.getLegend().setEnabled(false);

//        fragmentProgressReportBinding.lineChart.setOnChartValueSelectedListener(new OnChartValueSelectedListener() {
//            @Override
//            public void onValueSelected(Entry e, Highlight h) {
////                CustomMarkerView markerView = new CustomMarkerView(getContext(), R.layout.marker_view);
//                MarkerViewBinding binding = DataBindingUtil.inflate(LayoutInflater.from(getActivity()), R.layout.marker_view, null, false);
//                //Now we need an AlertDialog.Builder object
//                AlertDialog.Builder builder = new AlertDialog.Builder(Objects.requireNonNull(getActivity()));
//
//                builder.setView(binding.getRoot());
//                final AlertDialog alertDialog = builder.create();
//
//                //ForMonthChart
////                String month = String.valueOf(data.getMo().get((int) e.getY()).getY());
//                binding.tvContent.setText("Date :" + data.getMo().get((int) e.getX()).getX());
////                markerView.setmodel(data.getMo().get((int) e.getX()));
//                fragmentProgressReportBinding.lineChart.invalidate();
////                fragmentProgressReportBinding.lineChart.setMarkerView((IMarker) binding.getRoot());
//            }
//
//            @Override
//            public void onNothingSelected() {
//                Toast.makeText(getContext(), "onNothingSelected", Toast.LENGTH_SHORT).show();
//            }
//        });

//ForMonthChart
        fragmentProgressReportBinding.lineChart.setOnChartValueSelectedListener(new OnChartValueSelectedListener() {
            @Override
            public void onValueSelected(Entry e, Highlight h) {

//                CustomMarkerView markerView = new CustomMarkerView(getContext(), R.layout.marker_view);
                MarkerViewBinding binding = DataBindingUtil.inflate(LayoutInflater.from(getActivity()), R.layout.marker_view, null, false);
                AlertDialog.Builder builder = new AlertDialog.Builder(Objects.requireNonNull(getActivity()));

                builder.setView(binding.getRoot());
                final AlertDialog alertDialog = builder.create();

                if (!alertDialog.isShowing()) {
                    paretnData.clear();
                    pagenumber = 1;
                }

                callApiForUserLocationList(String.valueOf(data.getDa().get((int) e.getX()).getX()), binding, alertDialog);
                LinearLayoutManager linearLayoutManager = new LinearLayoutManager(NoonApplication.getContext());
                linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
                binding.rvUserList.rcVertical.setLayoutManager(linearLayoutManager);
                scrollListener = new RecyclerViewLoadMoreScroll(linearLayoutManager);

                scrollListener.setOnLoadMoreListener(new OnLoadMoreListener() {
                    @Override
                    public void onLoadMore() {
                        if (isLoad) {
                            binding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                            callApiForUserLocationList(String.valueOf(data.getDa().get((int) e.getX()).getX()), binding, alertDialog);
                        } else {
                            binding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        }
                    }
                });
                binding.rvUserList.rcVertical.addOnScrollListener(scrollListener);

                //ForDateChart
//                String date = String.valueOf(data.getDa().get((int) e.getY()).getY());
//                binding.tvContent.setText("Date : " + data.getDa().get((int) e.getX()).getX());
//                markerView.setmodel(data.getMo().get((int) e.getX()));
                fragmentProgressReportBinding.lineChart.invalidate();

//                fragmentProgressReportBinding.lineChart.setMarkerView(markerView);
                alertDialog.show();
                Objects.requireNonNull(alertDialog.getWindow()).setLayout(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
            }

            @Override
            public void onNothingSelected() {
                //Toast.makeText(getContext(), "onNothingSelected", Toast.LENGTH_SHORT).show();
            }
        });

        // fragmentProgressReportBinding.lineChart.zoom(1.5f,1.5f,0,0);
        fragmentProgressReportBinding.lineChart.setOnChartGestureListener(new OnChartGestureListener() {
            @Override
            public void onChartGestureStart(MotionEvent me, ChartTouchListener.ChartGesture lastPerformedGesture) {
//                Log.e("zoommmmm", "onChartGestureStart: "+me );
            }

            @Override
            public void onChartGestureEnd(MotionEvent me, ChartTouchListener.ChartGesture lastPerformedGesture) {
//                Log.e("zoommmmm", "onChartGestureEnd: "+me );
            }

            @Override
            public void onChartLongPressed(MotionEvent me) {
//                Log.e("zoommmmm", "onChartLongPressed: "+me );
            }

            @Override
            public void onChartDoubleTapped(MotionEvent me) {
                float scaleX = fragmentProgressReportBinding.lineChart.getScaleX();
                float scaleY = fragmentProgressReportBinding.lineChart.getScaleY();
                if (scaleX <= 1 && scaleY <= 1) {
                    drawMonthChart(data);
                } else {
                    drawDateChart(data);
                }
//                Log.e("zoommmmm", "onChartDoubleTapped: "+me );
            }

            @Override
            public void onChartSingleTapped(MotionEvent me) {
//                Log.e("zoommmmm", "onChartSingleTapped: "+me );
            }

            @Override
            public void onChartFling(MotionEvent me1, MotionEvent me2, float velocityX, float velocityY) {
//                Log.e("zoommmmm", "onChartFling: "+velocityX+"\n"+velocityY );
            }

            @Override
            public void onChartScale(MotionEvent me, float scaleX, float scaleY) {
                if (scaleX <= 1) {
                    drawMonthChart(data);
                } else {
                    drawDateChart(data);
                }
                //lineChart.setVisibleXRange(0, 10);

                Log.e("zoommmmm", "onChartScale: " + me);
                Log.e("dxdy", "onChartTranslate: " + scaleX + scaleY);
            }

            @Override
            public void onChartTranslate(MotionEvent me, float dX, float dY) {
//                Log.e("zoommmmm", "onChartTranslate: "+me );

            }
        });
        fragmentProgressReportBinding.lineChart.invalidate();
    }

    private void drawDateChart(Chart.Data data) {
        fragmentProgressReportBinding.lineChart.clear();
        fragmentProgressReportBinding.lineChart.resetZoom();
        fragmentProgressReportBinding.lineChart.zoomOut();
        LineDataSet lineDataSet = new LineDataSet(dataForDate(data.getDa()), "");

        Drawable drawable = ContextCompat.getDrawable(Objects.requireNonNull(getActivity()), R.drawable.graphview_shadow);
        lineDataSet.setFillDrawable(drawable);
        lineDataSet.setCircleRadius(6f);
        lineDataSet.setLineWidth(3f);
        lineDataSet.setCircleHoleRadius(4f);
        lineDataSet.setDrawCircleHole(true);
        lineDataSet.setDrawIcons(false);
        lineDataSet.setValueFormatter((value, entry, dataSetIndex, viewPortHandler) -> "");
        lineDataSet.setMode(LineDataSet.Mode.HORIZONTAL_BEZIER);   //set curves in ghaphview
        lineDataSet.setDrawFilled(true);

//        lineDataSet.setGradientColor(Color.BLUE,0);
//        lineDataSet.setMode(LineDataSet.Mode.CUBIC_BEZIER);
//        lineDataSet.setFillColor(getResources().getColor(R.color.colorPrimaryDark));
//        lineDataSet.setDrawFilled(true); // fill color under the graphview
//        lineDataSet.setValueTextSize(9f);
//        lineDataSet.setColor(Color.BLACK); //Change Line color
//        lineDataSet.setCircleColor(Color.BLACK); //Change Circle Color

        final ArrayList<String> yLabel = new ArrayList<>();

        int max = getMax(dataForDate(data.getDa()));
        for (int i = 0; i <= max; i++) {
            yLabel.add(i + " Hours");
        }
        YAxis yAxis = fragmentProgressReportBinding.lineChart.getAxisLeft();
        yAxis.setDrawGridLines(true);
        yAxis.setAxisLineColor(Color.parseColor("#00000000"));
        yAxis.setTextColor(Color.BLACK);
        yAxis.setValueFormatter((value, axis) -> {
            if (yLabel.size() >= value) {
                return Math.round(value) + " Hours";
            }
            return "";
        });
        final ArrayList<String> xLabel = new ArrayList<>();
//                            xLabel.add("month");
        xLabel.add("Jan");
        xLabel.add("Feb");
        xLabel.add("Mar");
        xLabel.add("Apr");
        xLabel.add("May");
        xLabel.add("Jun");
        xLabel.add("July");
        xLabel.add("Aug");
        xLabel.add("Sept");
        xLabel.add("Oct");
        xLabel.add("Nov");
        xLabel.add("Dec");
        XAxis xAxis = fragmentProgressReportBinding.lineChart.getXAxis();
        xAxis.setDrawGridLines(false);
        xAxis.setDrawAxisLine(true);
        xAxis.setAxisLineColor(Color.WHITE);
        xAxis.setTextColor(Color.BLACK);

//                            xAxis.setValueFormatter((value, axis) -> {
//                                if (xLabel.size() >= value) {
//                                    return xLabel.get(Math.round(value));
//                                }
//                                return "";
//                            });
        xAxis.setValueFormatter(new GraphXAxisValueFormatter(data.getMo(), 2, data.getDa()));
        ArrayList<ILineDataSet> iLineDataSets = new ArrayList<>();
        iLineDataSets.add(lineDataSet);
        LineData data1 = new LineData(iLineDataSets);
        fragmentProgressReportBinding.lineChart.setBackgroundColor(Color.WHITE);
//        Animation animation = AnimationUtils.loadAnimation(NoonApplication.getContext(),
//                android.R.anim.slide_out_right);
//        fragmentProgressReportBinding.lineChart.animateX(2000, Easing.EaseInCubic);
//        fragmentProgressReportBinding.lineChart.setAnimation(animation);

        fragmentProgressReportBinding.lineChart.getAxisRight().setDrawLabels(false);
        fragmentProgressReportBinding.lineChart.getAxisRight().setDrawGridLines(false);
        fragmentProgressReportBinding.lineChart.getAxisRight().setAxisLineColor(Color.parseColor("#00000000"));
        fragmentProgressReportBinding.lineChart.getAxisLeft().setDrawLabels(true);
        fragmentProgressReportBinding.lineChart.getAxisLeft().setEnabled(true);
        fragmentProgressReportBinding.lineChart.getAxisLeft().setPosition(YAxis.YAxisLabelPosition.OUTSIDE_CHART);
        fragmentProgressReportBinding.lineChart.getAxisLeft().setGridColor(getResources().getColor(R.color.colorlightDarkGray));
        fragmentProgressReportBinding.lineChart.getXAxis().setDrawLabels(true);
        fragmentProgressReportBinding.lineChart.getXAxis().setEnabled(true);
        fragmentProgressReportBinding.lineChart.getXAxis().setPosition(XAxis.XAxisPosition.BOTTOM_INSIDE);
        fragmentProgressReportBinding.lineChart.setData(data1);
        fragmentProgressReportBinding.lineChart.getXAxis().setGranularity(1);
        fragmentProgressReportBinding.lineChart.getAxisRight().setGranularity(1);
        fragmentProgressReportBinding.lineChart.getDescription().setEnabled(false);
        fragmentProgressReportBinding.lineChart.setDragEnabled(true);
        fragmentProgressReportBinding.lineChart.setScaleEnabled(true);
        fragmentProgressReportBinding.lineChart.getLegend().setEnabled(false);


//        fragmentProgressReportBinding.lineChart.setOnChartValueSelectedListener(new OnChartValueSelectedListener() {
//            @Override
//            public void onValueSelected(Entry e, Highlight h) {
////                if (e.getX()==data.getDa().size()-1)
//                Log.i("xaxis", "onValueSelected: " + e.getX() + "\n" + data.getDa().size());
//                CustomMarkerView markerView = new CustomMarkerView(getContext(), R.layout.marker_view);
//                markerView.setmodel(data.getDa().get((int) e.getX()));
//                fragmentProgressReportBinding.lineChart.invalidate();
//                fragmentProgressReportBinding.lineChart.setMarkerView(markerView);
//            }
//
//            @Override
//            public void onNothingSelected() {
//                Toast.makeText(getContext(), "onNothingSelected", Toast.LENGTH_SHORT).show();
//            }
//        });


        //Pending task of user location detail  //ForDateChart
        fragmentProgressReportBinding.lineChart.setOnChartValueSelectedListener(new OnChartValueSelectedListener() {
            @Override
            public void onValueSelected(Entry e, Highlight h) {

//                CustomMarkerView markerView = new CustomMarkerView(getContext(), R.layout.marker_view);
                MarkerViewBinding binding = DataBindingUtil.inflate(LayoutInflater.from(getActivity()), R.layout.marker_view, null, false);
                AlertDialog.Builder builder = new AlertDialog.Builder(Objects.requireNonNull(getActivity()));

                builder.setView(binding.getRoot());
                final AlertDialog alertDialog = builder.create();

                if (!alertDialog.isShowing()) {
                    paretnData.clear();
                    pagenumber = 1;
                }
                callApiForUserLocationList(data.getDa().get((int) e.getX()).getX(), binding, alertDialog);
                LinearLayoutManager linearLayoutManager = new LinearLayoutManager(NoonApplication.getContext());
                linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
                binding.rvUserList.rcVertical.setLayoutManager(linearLayoutManager);
                scrollListener = new RecyclerViewLoadMoreScroll(linearLayoutManager);


                scrollListener.setOnLoadMoreListener(new OnLoadMoreListener() {
                    @Override
                    public void onLoadMore() {
                        if (isLoad) {
                            binding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                            callApiForUserLocationList(data.getDa().get((int) e.getX()).getX(), binding, alertDialog);
                        } else {
                            binding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        }
                    }
                });
                binding.rvUserList.rcVertical.addOnScrollListener(scrollListener);

                //ForDateChart
//                String date = String.valueOf(data.getDa().get((int) e.getY()).getY());
//                binding.tvContent.setText("Date : " + data.getDa().get((int) e.getX()).getX());
//                markerView.setmodel(data.getMo().get((int) e.getX()));
                fragmentProgressReportBinding.lineChart.invalidate();

//                fragmentProgressReportBinding.lineChart.setMarkerView(markerView);
                alertDialog.show();
                Objects.requireNonNull(alertDialog.getWindow()).setLayout(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
            }

            @Override
            public void onNothingSelected() {
                //Toast.makeText(getContext(), "onNothingSelected", Toast.LENGTH_SHORT).show();
            }
        });


        fragmentProgressReportBinding.lineChart.setOnChartGestureListener(new OnChartGestureListener() {
            @Override
            public void onChartGestureStart(MotionEvent me, ChartTouchListener.ChartGesture lastPerformedGesture) {
//                Log.e("zoommmmm", "onChartGestureStart: "+me );
                /*switch (me.getAction()) {
                    case MotionEvent.ACTION_DOWN:
                        fragmentProgressReportBinding.nestedScrollView.requestDisallowInterceptTouchEvent(true);
                        break;
                    case MotionEvent.ACTION_UP:
                        fragmentProgressReportBinding.nestedScrollView.requestDisallowInterceptTouchEvent(true);
                        break;
                }*/
            }

            @Override
            public void onChartGestureEnd(MotionEvent me, ChartTouchListener.ChartGesture lastPerformedGesture) {
//                Log.e("zoommmmm", "onChartGestureEnd: "+me );
            }

            @Override
            public void onChartLongPressed(MotionEvent me) {
//                Log.e("zoommmmm", "onChartLongPressed: "+me );
            }

            @Override
            public void onChartDoubleTapped(MotionEvent me) {
                float scaleX = fragmentProgressReportBinding.lineChart.getScaleX();
                float scaleY = fragmentProgressReportBinding.lineChart.getScaleY();
                if (scaleX <= 1 && scaleY <= 1) {
                    drawMonthChart(data);
                } else {
                    drawDateChart(data);
                }
//                Log.e("zoommmmm", "onChartDoubleTapped: "+me );
            }

            @Override
            public void onChartSingleTapped(MotionEvent me) {
//                Log.e("zoommmmm", "onChartSingleTapped: "+me );
            }

            @Override
            public void onChartFling(MotionEvent me1, MotionEvent me2, float velocityX, float velocityY) {
//                Log.e("zoommmmm", "onChartFling: "+velocityX+"\n"+velocityY );
            }

            @Override
            public void onChartScale(MotionEvent me, float scaleX, float scaleY) {

                if (scaleX <= 1) {
                    drawMonthChart(data);
                } else {
                    drawDateChart(data);
                }
                //lineChart.setVisibleXRange(0, 10);

                Log.e("zoommmmm", "onChartScale: " + me);
                Log.e("dxdy", "onChartTranslate: " + scaleX + scaleY);
            }


            @Override
            public void onChartTranslate(MotionEvent me, float dX, float dY) {
//                Log.e("zoommmmm", "onChartTranslate: "+me );
            }
        });
        fragmentProgressReportBinding.lineChart.invalidate();
    }


    private void callApiForUserLocationList(String date, MarkerViewBinding binding, AlertDialog alertDialog) {
        disposable.add(parentControlRepository.getViewChildActivity(date, pagenumber, perpagerecord)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ParentGraphPoints>() {
                    @Override
                    public void onSuccess(ParentGraphPoints parentGraphPoints) {
                        if (parentGraphPoints != null) {
                            if (parentGraphPoints.getData() != null && !parentGraphPoints.getData().isEmpty()) {
                                pagenumber++;
                                paretnData.addAll(parentGraphPoints.getData());
                                UserLocationDetailListAdapter adapter = new UserLocationDetailListAdapter(getActivity(), paretnData);
                                binding.rvUserList.rcVertical.setAdapter(adapter);
                                scrollListener.setLoaded();
                                hideDialog();
                                if (parentGraphPoints.getData().size() == 0) {
                                    isLoad = false;
                                }
                                binding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);

                            }
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        e.printStackTrace();
                        Log.e("FeedBack||onError", "onError: " + e.getMessage());
                    }
                }));
    }

//    private void drawAChartArabic() {
//        LineDataSet lineDataSet = new LineDataSet(dataValues(data), "");
//        Drawable drawable = ContextCompat.getDrawable(Objects.requireNonNull(getActivity()), R.drawable.graphview_shadow);
//        lineDataSet.setFillDrawable(drawable);
//        lineDataSet.setCircleRadius(6f);
//        lineDataSet.setLineWidth(3f);
//        lineDataSet.setCircleHoleRadius(4f);
//        lineDataSet.setDrawCircleHole(true);
//        lineDataSet.setDrawIcons(false);
//        lineDataSet.setValueFormatter((value, entry, dataSetIndex, viewPortHandler) -> "");
//        lineDataSet.setMode(LineDataSet.Mode.HORIZONTAL_BEZIER);
//        lineDataSet.setDrawFilled(true);
////        lineDataSet.setGradientColor(Color.BLUE,0);
////        lineDataSet.setMode(LineDataSet.Mode.CUBIC_BEZIER);
////        lineDataSet.setFillColor(getResources().getColor(R.color.colorPrimaryDark));
////        lineDataSet.setDrawFilled(true); // fill color under the graphview
////        lineDataSet.setValueTextSize(9f);
////        lineDataSet.setColor(Color.BLACK); //Change Line color
////        lineDataSet.setCircleColor(Color.BLACK); //Change Circle Color
//        final ArrayList<String> yLabel = new ArrayList<>();
//        int max = getMax(dataValues(data));
//        for (int i = 0; i <= max; i++) {
//            yLabel.add(i + " Hours");
//        }
//
//        YAxis yAxis = fragmentProgressReportBinding.lineChart.getAxisRight();
//        yAxis.setDrawGridLines(true);
//        yAxis.setAxisLineColor(Color.parseColor("#00000000"));
//        yAxis.setTextColor(Color.BLACK);
//        yAxis.setValueFormatter((value, axis) -> {
//            if (yLabel.size() >= value) {
//                return Math.round(value) + " Hours";
//            }
//            return "";
//        });
//        final ArrayList<String> xLabel = new ArrayList<>();
//        xLabel.add("Jan");
//        xLabel.add("Feb");
//        xLabel.add("Mar");
//        xLabel.add("Apr");
//        xLabel.add("May");
//        xLabel.add("Jun");
//        XAxis xAxis = fragmentProgressReportBinding.lineChart.getXAxis();
//        xAxis.setDrawGridLines(false);
//        xAxis.setDrawAxisLine(true);
//        xAxis.setAxisLineColor(Color.WHITE);
//        xAxis.setTextColor(Color.BLACK);
//        xAxis.setLabelCount(xLabel.size(), true);
//        xAxis.setValueFormatter((value, axis) -> {
//            if (xLabel.size() >= value) {
//                return xLabel.get(Math.round(value));
//            }
//            return "";
//        });
//        ArrayList<ILineDataSet> iLineDataSets = new ArrayList<>();
//        iLineDataSets.add(lineDataSet);
//        LineData data = new LineData(iLineDataSets);
//        fragmentProgressReportBinding.lineChart.setBackgroundColor(Color.WHITE);
//        fragmentProgressReportBinding.lineChart.getAxisLeft().setDrawLabels(false);
//        fragmentProgressReportBinding.lineChart.getAxisLeft().setDrawGridLines(false);
//        fragmentProgressReportBinding.lineChart.getAxisLeft().setAxisLineColor(Color.parseColor("#00000000"));
//        fragmentProgressReportBinding.lineChart.getAxisRight().setDrawLabels(true);
//        fragmentProgressReportBinding.lineChart.getAxisRight().setEnabled(true);
//        fragmentProgressReportBinding.lineChart.getAxisRight().setPosition(YAxis.YAxisLabelPosition.OUTSIDE_CHART);
//        fragmentProgressReportBinding.lineChart.getAxisRight().setGridColor(getResources().getColor(R.color.colorlightDarkGray));
//        fragmentProgressReportBinding.lineChart.getXAxis().setDrawLabels(true);
//        fragmentProgressReportBinding.lineChart.getXAxis().setEnabled(true);
//        fragmentProgressReportBinding.lineChart.getXAxis().setPosition(XAxis.XAxisPosition.BOTTOM_INSIDE);
//        fragmentProgressReportBinding.lineChart.setData(data);
//        fragmentProgressReportBinding.lineChart.invalidate();
//        fragmentProgressReportBinding.lineChart.getDescription().setEnabled(false);
//
//        // enable scaling and dragging
//        fragmentProgressReportBinding.lineChart.setDragEnabled(false);
//        fragmentProgressReportBinding.lineChart.setScaleEnabled(false);
//        fragmentProgressReportBinding.lineChart.getLegend().setEnabled(false);
//    }

    /* private ArrayList<Entry> dataValues(List<Chart.FillesData> data) {
         ArrayList<Entry> dataVals = new ArrayList<>();
         if (data != null && !data.isEmpty()) {
             for (int i = 0; i < data.size(); i++) {
                 dataVals.add(new Entry(data.get(i).getX(), data.get(i).getY()));
             }

         }
         return dataVals;
     }*/
    private ArrayList<Entry> dataForMonth(List<Chart.Mo> data) {
        ArrayList<Entry> dataVals = new ArrayList<>();
        if (data != null && !data.isEmpty()) {
            for (int i = 0; i < data.size(); i++) {
                dataVals.add(new Entry(i, data.get(i).getY()));
            }

        }
        return dataVals;
    }

    private ArrayList<Entry> dataForDate(List<Chart.Da> data) {
        ArrayList<Entry> dataVals = new ArrayList<>();
        if (data != null && !data.isEmpty()) {
            for (int i = 0; i < data.size(); i++) {
                dataVals.add(new Entry(i, data.get(i).getY()));
            }

        }
        return dataVals;
    }

    public int getMax(ArrayList<Entry> list) {
        int max = Integer.MIN_VALUE;
        for (int i = 0; i < list.size(); i++) {
            Entry entry = list.get(i);
            int min = Math.round(entry.getY());
            if (min > max) {
                max = (int) entry.getY();
            }
        }
        return max;
    }

    private void addHeatMap() {
        List<LatLng> list = null;

        ArrayList<LatLng> latLngs = new ArrayList<>();


        // Get the data: latitude/longitude positions of police stations.

        latLngs.add(new LatLng(12, 20));
        latLngs.add(new LatLng(15, 30));
        latLngs.add(new LatLng(22, 40));


        // Create a heat map tile provider, passing it the latlngs of the police stations.
        int[] colors = {
                Color.rgb(102, 225, 0), // green
                Color.rgb(255, 0, 0)    // red
        };

        float[] startPoints = {
                0.2f, 1f
        };

        Gradient gradient = new Gradient(colors, startPoints);

        mProvider = new HeatmapTileProvider.Builder()
                .data(latLngs)
                .gradient(gradient)
                .build();
        // Add a tile overlay to the map, using the heat map tile provider.
        mOverlay = mMap.addTileOverlay(new TileOverlayOptions().tileProvider(mProvider));
        mProvider.setOpacity(0.7);

    }

    private ArrayList<LatLng> readItems(int resource) throws JSONException {
        ArrayList<LatLng> list = new ArrayList<>();
        InputStream inputStream = getResources().openRawResource(resource);
        String json = new Scanner(inputStream).useDelimiter("\\A").next();
        JSONArray array = new JSONArray(json);
        for (int i = 0; i < array.length(); i++) {
            JSONObject object = array.getJSONObject(i);
            double lat = object.getDouble("lat");
            double lng = object.getDouble("lng");
            list.add(new LatLng(lat, lng));
        }
        return list;
    }

    Bitmap getMarkerIconWithLabel(String label) {
        IconGenerator iconGenerator = new IconGenerator(NoonApplication.getContext());
        View markerView = LayoutInflater.from(NoonApplication.getContext()).inflate(R.layout.marker_detail_layout, null);
        ImageView imgMarker = markerView.findViewById(R.id.marker);
        TextView tvLabel = markerView.findViewById(R.id.txt_lable);
        imgMarker.setImageResource(R.drawable.marker);
        // imgMarker.setRotation(angle);
        tvLabel.setText(label);
        Typeface typeface = ResourcesCompat.getFont(Objects.requireNonNull(getActivity()), R.font.bahij_helvetica_neue_bold);
        tvLabel.setTypeface(typeface);
        iconGenerator.setContentView(markerView);
        iconGenerator.setBackground(null);
        return iconGenerator.makeIcon(label);
    }


}