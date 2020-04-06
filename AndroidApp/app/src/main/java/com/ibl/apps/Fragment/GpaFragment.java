package com.ibl.apps.Fragment;


import androidx.databinding.DataBindingUtil;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import androidx.annotation.NonNull;
import androidx.core.content.ContextCompat;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;

import com.github.mikephil.charting.components.XAxis;
import com.github.mikephil.charting.components.YAxis;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.interfaces.datasets.ILineDataSet;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.parent.GPAProgress;
import com.ibl.apps.Model.parent.LastOnline;
import com.ibl.apps.Model.parent.MChart;
import com.ibl.apps.Model.parent.ParentSpinnerModel;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.CustomView.MultiSelectSpinner;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.TimeAgoLastOnlineClass;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentGpaBinding;

import java.text.ParseException;
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

import static com.ibl.apps.Base.BaseActivity.apiService;
import static com.ibl.apps.Fragment.ProgressReportFragment.selectedIdsForCallback;

public class GpaFragment extends BaseFragment implements View.OnClickListener {

    FragmentGpaBinding fragmentGpaBinding;
    private UserDetails userDetails;
    CompositeDisposable disposable = new CompositeDisposable();
    private int userId = 0;
    private int month = 0;
    private List<ParentSpinnerModel.Data> studentNamecategories;
    private ParentSpinnerModel parentSpinner = new ParentSpinnerModel();

    public GpaFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        fragmentGpaBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_gpa, container, false);
        return fragmentGpaBinding.getRoot();

    }

    @Override
    protected void setUp(View view) {

        //drawAChart();
        if (isNetworkAvailable(NoonApplication.getContext())) {
            fragmentGpaBinding.noInternetLay.setVisibility(View.GONE);
            fragmentGpaBinding.mainlayout.setVisibility(View.VISIBLE);
            showDialog(Objects.requireNonNull(getActivity()).getResources().getString(R.string.loading));
            setUpAllSpinners();
        } else {
            fragmentGpaBinding.mainlayout.setVisibility(View.GONE);
            fragmentGpaBinding.noInternetLay.setVisibility(View.VISIBLE);

        }
        setOnClickListner();
    }

    private void callApiforCurrentGpa(ArrayList<Integer> userId) {
        ApiService apiService = ApiClient.getClient(getContext()).create(ApiService.class);
        JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userId.size(); i++) {
                array.add(userId.get(i));
            }
            jsonObject.add("id", array);
            jsonObject.addProperty("month", 0);
            jsonObject.addProperty("courseid", 0);
            jsonObject.addProperty("content", 0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(apiService.getCurrentGPA(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<GPAProgress>() {
                    @Override
                    public void onSuccess(GPAProgress progressReport) {
                        if (progressReport != null && progressReport.getData() != null) {
                            fragmentGpaBinding.tvGrade.setText(Objects.requireNonNull(getContext()).getString(R.string.current_gpa).concat(" ").concat(progressReport.getData().getGrade()));
                        } else {
                            fragmentGpaBinding.tvGrade.setText(Objects.requireNonNull(getContext()).getString(R.string.current_gpa).concat(" ").concat(" 0"));
                        }
                    }

                    @Override
                    public void onError(Throwable e) {

                    }
                }));

        if (userId.size() == 1) {
            JsonObject jsonobject = new JsonObject();
            try {
                JsonArray array = new JsonArray();
                for (int i = 0; i < userId.size(); i++) {
                    array.add(userId.get(i));
                }

                jsonobject.add("id", array);
                jsonobject.addProperty("month", 0);
                jsonobject.addProperty("courseid", 0);
                jsonobject.addProperty("content", 0);
            } catch (JsonSyntaxException e) {
                e.printStackTrace();
            }
            disposable.add(apiService.getLastOnline(jsonobject)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<LastOnline>() {
                        @Override
                        public void onSuccess(LastOnline lastOnline) {
                            if (lastOnline != null && lastOnline.getData() != null) {
                                TimeAgoLastOnlineClass timeAgo = new TimeAgoLastOnlineClass();
                                String getDate = getDateUTC(lastOnline.getData());
                                Log.e("getDate", "onSuccess: " + lastOnline.getData());
                                String timeagotxt = timeAgo.covertTimeToText(getDate);
                                fragmentGpaBinding.tvLastonline.setText(timeagotxt);
                            } else {
                                fragmentGpaBinding.tvLastonline.setText("--");
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            fragmentGpaBinding.tvLastonline.setText("--");
                        }
                    }));
        } else {
            fragmentGpaBinding.tvLastonline.setText("--");
        }

    }

    private void setUpAllSpinners() {
        callApiForStudentSpinner();
    }

    private void setOnClickListner() {
        fragmentGpaBinding.btnbackProgress.setOnClickListener(this);
    }

    private void callApiForStudentSpinner() {
        String authid = PrefUtils.getAuthid(NoonApplication.getContext());
        if (!TextUtils.isEmpty(authid)) {
            AuthTokenObject authTokenObject = AppDatabase.getAppDatabase(NoonApplication.getContext()).authTokenDao().getauthTokenData(authid);

            if (authTokenObject != null) {
                String sub = "";
                if (authTokenObject.getSub() != null) {
                    sub = authTokenObject.getSub();
                    userDetails = AppDatabase.getAppDatabase(NoonApplication.getContext()).userDetailDao().getUserDetials(sub);
                }
            }
        }
        ApiService apiService = ApiClient.getClient(getContext()).create(ApiService.class);
        disposable.add(apiService.getUserSpinnerData(Integer.parseInt(userDetails.getId()))
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ParentSpinnerModel>() {
                    @Override
                    public void onSuccess(ParentSpinnerModel parentSpinnerModel) {
                        if (parentSpinnerModel != null) {
                            parentSpinner = parentSpinnerModel;
                            setDataInStudentSpinner(parentSpinnerModel);
                            hideDialog();
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));
    }

    private void setDataInStudentSpinner(ParentSpinnerModel parentSpinnerModel) {

        studentNamecategories = parentSpinnerModel.getData();
//        ParentSpinnerModel.FillesData data = new ParentSpinnerModel.FillesData();
//        data.setUsername(Objects.requireNonNull(getString(R.string.all_users)));
//        data.setId(0L);
//        studentNamecategories.add(0, data);
//        MyAdapter studentNameAdapter = new MyAdapter(Objects.requireNonNull(getActivity()), 0, studentNamecategories, this);
//        ArrayAdapter<ParentSpinnerModel.FillesData> studentNamedataAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, studentNamecategories);
//        // Drop down layout style - list view with radio button
//        studentNamedataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);

//        fragmentGpaBinding.spuser.setAdapter(studentNameAdapter);
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

//        for (int i = 0; i < selectedIdsForCallback.size(); i++) {
//            userArray.add(Integer.parseInt(String.valueOf(selectedIdsForCallback.get(i).getId())));
//        }

        callApiforCurrentGpa(userArray);
        setupMonthsSpinner(userArray);
        if (parentSpinnerModel.getData().size() == selectedIdsForCallback.size()) {
            fragmentGpaBinding.allUser.setVisibility(View.VISIBLE);
            fragmentGpaBinding.spUser.setText(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_users));
            fragmentGpaBinding.allUser.setText("( " + userArray.size() + " )");
        } else {
            fragmentGpaBinding.spUser.setText(userArray.size() + " " + getActivity().getResources().getString(R.string.user_selected));
            fragmentGpaBinding.allUser.setVisibility(View.GONE);
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
                            fragmentGpaBinding.allUser.setVisibility(View.VISIBLE);
                            fragmentGpaBinding.spUser.setText(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_users));
                            fragmentGpaBinding.allUser.setText("( " + var1.size() + " )");
                        } else {
                            fragmentGpaBinding.spUser.setText(var1.size() + " " + getActivity().getResources().getString(R.string.user_selected));
                            fragmentGpaBinding.allUser.setVisibility(View.GONE);
                        }
                        ArrayList<Integer> userArray = new ArrayList<>();
                        for (int i = 0; i < var1.size(); i++) {
                            userArray.add(Integer.parseInt(String.valueOf(var1.get(i).getId())));
                        }
                        callApiforCurrentGpa(userArray);
                        setupMonthsSpinner(userArray);

                    }

                    @Override
                    public void onCancel() {

                    }
                });
        fragmentGpaBinding.spUserLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                multiSelectSpinner.show(Objects.requireNonNull(getActivity()).getSupportFragmentManager(), "multiSelectDialog");
            }
        });

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

    private void setupMonthsSpinner(ArrayList<Integer> userId) {
        List<String> usercategories = new ArrayList<>();
        usercategories.add(0, Objects.requireNonNull(getActivity()).getResources().getString(R.string.last_one_month));
        usercategories.add(1, getActivity().getResources().getString(R.string.last_three_month));
        usercategories.add(2, getActivity().getResources().getString(R.string.last_six_month));

        // Creating adapter for spinner
        ArrayAdapter<String> userAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, usercategories);
        // Drop down layout style - list view with radio button
        userAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        fragmentGpaBinding.spmonth.setAdapter(userAdapter);
        fragmentGpaBinding.spmonth.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                switch (i) {
                    case 0:
                        month = 1;
                        break;
                    case 1:
                        month = 3;
                        break;
                    case 2:
                        month = 6;
                        break;
                }
                callApiforGpaChart(userId);
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });
    }

    private void callApiforGpaChart(ArrayList<Integer> userId) {
        JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userId.size(); i++) {
                array.add(userId.get(i));
            }
            jsonObject.add("id", array);
            jsonObject.addProperty("month", month);
            jsonObject.addProperty("courseid", 0);
            jsonObject.addProperty("content", 0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(apiService.getCurrentPoints(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<MChart>() {
                    @Override
                    public void onSuccess(MChart chart) {
                        if (chart != null && chart.getData() != null && !chart.getData().isEmpty()) {
                            fragmentGpaBinding.linechart.setVisibility(View.VISIBLE);
                            fragmentGpaBinding.txtNodata.setVisibility(View.GONE);
                            List<MChart.Data> data = chart.getData();
                            LineDataSet lineDataSet = new LineDataSet(dataValues(data), "");
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
                            int max = getMax(dataValues(data));
                            for (int i = 0; i <= max; i++) {
                                yLabel.add(i + " Hours");
                            }

                            YAxis yAxis = fragmentGpaBinding.linechart.getAxisLeft();
                            yAxis.setDrawGridLines(true);
                            yAxis.setAxisLineColor(Color.parseColor("#00000000"));
                            yAxis.setTextColor(Color.BLACK);
                            yAxis.setValueFormatter((value, axis) -> {
                                if (yLabel.size() >= value) {
                                    return Math.round(value) + " Points";
                                }
                                return "";
                            });
                            final ArrayList<String> xLabel = new ArrayList<>();
                            xLabel.add("month");
                            xLabel.add("Jan");
                            xLabel.add("Feb");
                            xLabel.add("Mar");
                            xLabel.add("Apr");
                            xLabel.add("May");
                            xLabel.add("Jun");
                            xLabel.add("july");
                            xLabel.add("aug");
                            xLabel.add("sept");
                            xLabel.add("oct");
                            xLabel.add("nov");
                            xLabel.add("dec");
                            XAxis xAxis = fragmentGpaBinding.linechart.getXAxis();
                            xAxis.setDrawGridLines(false);
                            xAxis.setDrawAxisLine(true);
                            xAxis.setAxisLineColor(Color.WHITE);
                            xAxis.setTextColor(Color.BLACK);
                            xAxis.setLabelCount(dataValues(data).size(), true);
                            xAxis.setValueFormatter((value, axis) -> {
                                if (xLabel.size() >= value) {
                                    return xLabel.get(Math.round(value));
                                }
                                return "";
                            });
                            ArrayList<ILineDataSet> iLineDataSets = new ArrayList<>();
                            iLineDataSets.add(lineDataSet);
                            LineData data1 = new LineData(iLineDataSets);
                            fragmentGpaBinding.linechart.setBackgroundColor(Color.WHITE);
                            fragmentGpaBinding.linechart.getAxisRight().setDrawLabels(false);
                            fragmentGpaBinding.linechart.getAxisRight().setDrawGridLines(false);
                            fragmentGpaBinding.linechart.getAxisRight().setAxisLineColor(Color.parseColor("#00000000"));
                            fragmentGpaBinding.linechart.getAxisLeft().setDrawLabels(true);
                            fragmentGpaBinding.linechart.getAxisLeft().setEnabled(true);
                            fragmentGpaBinding.linechart.getAxisLeft().setPosition(YAxis.YAxisLabelPosition.OUTSIDE_CHART);
                            fragmentGpaBinding.linechart.getAxisLeft().setGridColor(getResources().getColor(R.color.colorlightDarkGray));
                            fragmentGpaBinding.linechart.getXAxis().setDrawLabels(true);
                            fragmentGpaBinding.linechart.getXAxis().setEnabled(true);
                            fragmentGpaBinding.linechart.getXAxis().setPosition(XAxis.XAxisPosition.BOTTOM);
                            fragmentGpaBinding.linechart.setData(data1);

                            fragmentGpaBinding.linechart.getDescription().setEnabled(false);
                            fragmentGpaBinding.linechart.invalidate();
                            // enable scaling and dragging
                            fragmentGpaBinding.linechart.setDragEnabled(false);
                            fragmentGpaBinding.linechart.setScaleEnabled(false);
                            fragmentGpaBinding.linechart.getLegend().setEnabled(false);
                            Log.e("chart", "onSuccess: " + chart.toString());
                        } else {
                            fragmentGpaBinding.linechart.setVisibility(View.GONE);
                            fragmentGpaBinding.txtNodata.setVisibility(View.VISIBLE);
                        }

                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));

    }


    /*private void drawAChart() {
        LineDataSet lineDataSet = new LineDataSet(dataValues(data), "");
        Drawable drawable = ContextCompat.getDrawable(Objects.requireNonNull(getActivity()), R.drawable.graphview_shadow);
        lineDataSet.setFillDrawable(drawable);
        lineDataSet.setCircleRadius(7f);
        lineDataSet.setLineWidth(3f);
        lineDataSet.setCircleHoleRadius(4f);
        lineDataSet.setDrawCircleHole(true);
        lineDataSet.setDrawIcons(false);
        lineDataSet.setValueFormatter((value, entry, dataSetIndex, viewPortHandler) -> "");
        lineDataSet.setMode(LineDataSet.Mode.HORIZONTAL_BEZIER);
        lineDataSet.setDrawFilled(true);
//        lineDataSet.setGradientColor(Color.BLUE,0);
//        lineDataSet.setMode(LineDataSet.Mode.CUBIC_BEZIER);
//        lineDataSet.setFillColor(getResources().getColor(R.color.colorPrimaryDark));
//        lineDataSet.setDrawFilled(true); // fill color under the graphview
//        lineDataSet.setValueTextSize(9f);
//        lineDataSet.setColor(Color.BLACK); //Change Line color
//        lineDataSet.setCircleColor(Color.BLACK); //Change Circle Color
        final ArrayList<String> yLabel = new ArrayList<>();
        int max = getMax(dataValues(data));
        for (int i = 0; i <= max; i++) {
            yLabel.add(i + " Hours");
        }
        YAxis yAxis = fragmentGpaBinding.linechart.getAxisLeft();
        yAxis.setDrawGridLines(true);
        yAxis.setAxisLineColor(Color.parseColor("#00000000"));
        yAxis.setTextColor(Color.BLACK);
        yAxis.setValueFormatter((value, axis) -> {
            if (yLabel.size() >= value) {
                return Math.round(value) + " Points";
            }
            return "";
        });
        final ArrayList<String> xLabel = new ArrayList<>();
        xLabel.add("Jan");
        xLabel.add("Feb");
        xLabel.add("March");
        xLabel.add("Apr");
        xLabel.add("May");
        xLabel.add("Jun");
        XAxis xAxis = fragmentGpaBinding.linechart.getXAxis();
        xAxis.setDrawGridLines(false);
        xAxis.setDrawAxisLine(true);
        xAxis.setAxisLineColor(Color.WHITE);
        xAxis.setTextColor(Color.BLACK);
        xAxis.setLabelCount(xLabel.size(), true);
        xAxis.setValueFormatter((value, axis) -> {
            if (xLabel.size() >= value) {
                return xLabel.get(Math.round(value));
            }
            return "";
        });
        ArrayList<ILineDataSet> iLineDataSets = new ArrayList<>();
        iLineDataSets.add(lineDataSet);
        LineData data = new LineData(iLineDataSets);
        fragmentGpaBinding.linechart.setBackgroundColor(Color.WHITE);
        fragmentGpaBinding.linechart.getAxisRight().setDrawLabels(false);
        fragmentGpaBinding.linechart.getAxisRight().setDrawGridLines(false);
        fragmentGpaBinding.linechart.getAxisRight().setAxisLineColor(Color.parseColor("#00000000"));
        fragmentGpaBinding.linechart.getAxisLeft().setDrawLabels(true);
        fragmentGpaBinding.linechart.getAxisLeft().setEnabled(true);
        fragmentGpaBinding.linechart.getAxisLeft().setPosition(YAxis.YAxisLabelPosition.OUTSIDE_CHART);
        fragmentGpaBinding.linechart.getAxisLeft().setGridColor(getResources().getColor(R.color.colorGray));
        fragmentGpaBinding.linechart.getXAxis().setDrawLabels(true);
        fragmentGpaBinding.linechart.getXAxis().setEnabled(true);
        fragmentGpaBinding.linechart.getXAxis().setPosition(XAxis.XAxisPosition.BOTTOM_INSIDE);
        fragmentGpaBinding.linechart.setData(data);
        fragmentGpaBinding.linechart.invalidate();
        fragmentGpaBinding.linechart.getDescription().setEnabled(false);
        // enable scaling and dragging
        fragmentGpaBinding.linechart.setDragEnabled(false);
        fragmentGpaBinding.linechart.setScaleEnabled(false);
        fragmentGpaBinding.linechart.getLegend().setEnabled(false);
    }*/

    private ArrayList<Entry> dataValues() {
        ArrayList<Entry> dataVals = new ArrayList<>();
        dataVals.add(new Entry(0, 0));
        dataVals.add(new Entry(1, 1.35f));
        dataVals.add(new Entry(2, 3.15f));
        dataVals.add(new Entry(3, 1.55f));
        dataVals.add(new Entry(4, 3f));
        dataVals.add(new Entry(5, 6f));
        return dataVals;
    }

    private ArrayList<Entry> dataValues(List<MChart.Data> data) {
        ArrayList<Entry> dataVals = new ArrayList<>();
        if (data != null && !data.isEmpty()) {
            for (int i = 0; i < data.size(); i++) {
                dataVals.add(new Entry(data.get(i).getX(), data.get(i).getY()));
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

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.btnbackProgress:
                Objects.requireNonNull(getActivity()).onBackPressed();
                break;
        }
    }

    private String getDate(String ourDate) {

        String dtStart = "2010-10-15T09:27:37Z";
        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.ENGLISH);
        format.setTimeZone(TimeZone.getTimeZone("UTC"));
        try {
            Date date = format.parse(ourDate);
            System.out.println(date);
        } catch (ParseException e) {
            e.printStackTrace();
        }


        return ourDate;
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


}

