package com.ibl.apps.Fragment;

import android.graphics.Color;
import android.graphics.Typeface;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;

import androidx.annotation.NonNull;
import androidx.core.content.ContextCompat;
import androidx.core.content.res.ResourcesCompat;
import androidx.databinding.DataBindingUtil;

import com.github.mikephil.charting.components.XAxis;
import com.github.mikephil.charting.components.YAxis;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.data.LineData;
import com.github.mikephil.charting.data.LineDataSet;
import com.github.mikephil.charting.data.PieData;
import com.github.mikephil.charting.data.PieDataSet;
import com.github.mikephil.charting.data.PieEntry;
import com.github.mikephil.charting.formatter.PercentFormatter;
import com.github.mikephil.charting.interfaces.datasets.ILineDataSet;
import com.github.mikephil.charting.listener.ChartTouchListener;
import com.github.mikephil.charting.listener.OnChartGestureListener;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.parent.Chart;
import com.ibl.apps.Model.parent.CourseSpinnerData;
import com.ibl.apps.Model.parent.GraphXAxisValueFormatter;
import com.ibl.apps.Model.parent.LastOnline;
import com.ibl.apps.Model.parent.ParentSpinnerModel;
import com.ibl.apps.Model.parent.ProgressReport;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.ParentControlManagement.ParentControlRepository;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentAssignmentQuizePerformanceBinding;
import com.ibl.apps.util.CustomView.MultiSelectSpinner;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.TimeAgoLastOnlineClass;

import java.text.DecimalFormat;
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

import static com.ibl.apps.Fragment.ProgressReportFragment.selectedIdsForCallback;

public class AssignmentQuizePerformanceFragment extends BaseFragment implements View.OnClickListener {
    FragmentAssignmentQuizePerformanceBinding fragmentAssignmentQuizeBinding;

    int[] color1 = {Color.rgb(185, 249, 246), Color.rgb(250, 209, 209)};//186,247,247-cyan

    public static int flag;
    private UserDetails userDetails;
    CompositeDisposable disposable = new CompositeDisposable();
    private ApiService apiService;
    private int userId;
    private List<ParentSpinnerModel.Data> studentNamecategories;
    private int couresId;
    private ArrayList<Integer> userIdArray = new ArrayList<>();
    private ParentSpinnerModel parentSpinner = new ParentSpinnerModel();
    private ParentControlRepository parentControlRepository;

    public static AssignmentQuizePerformanceFragment newInstance(String param1, String param2) {
        AssignmentQuizePerformanceFragment fragment = new AssignmentQuizePerformanceFragment();
        Bundle args = new Bundle();
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    private void setonClickListner() {
        fragmentAssignmentQuizeBinding.btnbackProgress.setOnClickListener(this);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.btnbackProgress:
                Objects.requireNonNull(getActivity()).onBackPressed();
                break;
        }
    }

    @Override
    protected void setUp(View view) {
        showDialog(Objects.requireNonNull(getActivity()).getResources().getString(R.string.loading));
        parentControlRepository = new ParentControlRepository();
        apiService = ApiClient.getClient(getContext()).create(ApiService.class);
        getflag();
        //getQuizProgressPieChartApi();
        setUpAllSpinners();
        setonClickListner();
        setTitle();
    }

    private void setUpAllSpinners() {
        callApiForStudentSpinner();
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

        disposable.add(parentControlRepository.getUserSpinnerData(Integer.parseInt(userDetails.getId()))
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ParentSpinnerModel>() {
                    @Override
                    public void onSuccess(ParentSpinnerModel parentSpinnerModel) {
                        if (parentSpinnerModel != null) {
                            setupStudentSpinner(parentSpinnerModel);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {

                    }
                }));
    }

    private void setupStudentSpinner(ParentSpinnerModel parentSpinnerModel) {

        studentNamecategories = parentSpinnerModel.getData();
//        ParentSpinnerModel.FillesData data = new ParentSpinnerModel.FillesData();
//        data.setUsername(Objects.requireNonNull(getString(R.string.all_users)));
//        data.setId(0L);
//        studentNamecategories.add(0, data);
        // MyAdapter studentNameAdapter = new MyAdapter(Objects.requireNonNull(getActivity()), 0, studentNamecategories, this);
//        ArrayAdapter<ParentSpinnerModel.FillesData> studentNamedataAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, studentNamecategories);
//        // Drop down layout style - list view with radio button
//        studentNamedataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);

        // fragmentAssignmentQuizeBinding.studentName.setAdapter(studentNameAdapter);
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

        callApiForCourseSpinner(userArray);
        if (parentSpinnerModel.getData().size() == selectedIdsForCallback.size()) {
            fragmentAssignmentQuizeBinding.allUser.setVisibility(View.VISIBLE);
            fragmentAssignmentQuizeBinding.spUser.setText(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_users));
            fragmentAssignmentQuizeBinding.allUser.setText("( " + userArray.size() + " )");
        } else {
            fragmentAssignmentQuizeBinding.spUser.setText(userArray.size() + " " + getActivity().getResources().getString(R.string.user_selected));
            fragmentAssignmentQuizeBinding.allUser.setVisibility(View.GONE);
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
                            fragmentAssignmentQuizeBinding.allUser.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.spUser.setText(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_users));
                            fragmentAssignmentQuizeBinding.allUser.setText("( " + var1.size() + " )");
                        } else {
                            fragmentAssignmentQuizeBinding.spUser.setText(var1.size() + " " + getActivity().getResources().getString(R.string.user_selected));
                            fragmentAssignmentQuizeBinding.allUser.setVisibility(View.GONE);
                        }
                        ArrayList<Integer> userArray = new ArrayList<>();
                        for (int i = 0; i < var1.size(); i++) {
                            userArray.add(Integer.parseInt(String.valueOf(var1.get(i).getId())));
                        }
                        callApiForCourseSpinner(userArray);

                    }

                    @Override
                    public void onCancel() {

                    }
                });
        fragmentAssignmentQuizeBinding.spUserLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                multiSelectSpinner.show(getActivity().getSupportFragmentManager(), "multiSelectDialog");
            }
        });

        /*if (MyAdapter.userId.isEmpty()) {
            for (int j = 0; j < parentSpinnerModel.getData().size(); j++) {
                if (parentSpinnerModel.getData().get(j).getId() != 0L && !MyAdapter.userId.contains(Integer.parseInt(String.valueOf(parentSpinnerModel.getData().get(j).getId())))) {
                    MyAdapter.userId.add(Integer.valueOf(String.valueOf(parentSpinner.getData().get(j).getId())));
                }
            }
            ((MyAdapter) fragmentAssignmentQuizeBinding.studentName.getAdapter()).notifyDataSetChanged();
            callApiForCourseSpinner(MyAdapter.userId);
        } else {
            ((MyAdapter) fragmentAssignmentQuizeBinding.studentName.getAdapter()).notifyDataSetChanged();
            callApiForCourseSpinner(MyAdapter.userId);

            //fragmentProgressReportBinding.studentName.setSelection(position);

        }*/
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

    private String getDate(String ourDate) {

        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.ENGLISH);
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

    private void callApiForCourseSpinner(ArrayList<Integer> userArray) {
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
        disposable.add(parentControlRepository.getCourseData(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<CourseSpinnerData>() {
                    @Override
                    public void onSuccess(CourseSpinnerData courseSpinnerData) {
                        if (courseSpinnerData != null) {
                            List<CourseSpinnerData.Data> coursecategories = new ArrayList<>();
                            CourseSpinnerData.Data data = new CourseSpinnerData.Data();
                            data.setName(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_courses));
                            data.setId(0L);
                            coursecategories.add(data);
                            coursecategories.addAll(courseSpinnerData.getData());
                            ArrayAdapter<CourseSpinnerData.Data> courseSpinnerAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, coursecategories);
                            getPerformanceQuizSpinner(courseSpinnerAdapter, coursecategories, userArray);
                            //getOverAllQuizSpinner(courseSpinnerAdapter, coursecategories);
                            getOverTimeQuizSpinner(courseSpinnerAdapter, coursecategories, userArray);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {

                    }
                }));
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
                                Log.e("getDate", "onSuccess: " + lastOnline.getData());
                                String timeagotxt = timeAgo.covertTimeToText(getDate);

                                fragmentAssignmentQuizeBinding.tvLastSeen.setText(timeagotxt);
                            } else {
                                fragmentAssignmentQuizeBinding.tvLastSeen.setText("--");
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            fragmentAssignmentQuizeBinding.tvLastSeen.setText("--");
                        }
                    }));
        } else {
            fragmentAssignmentQuizeBinding.tvLastSeen.setText("--");
        }
    }

    private void getOverTimeQuizSpinner(ArrayAdapter<CourseSpinnerData.Data> courseSpinnerAdapter, List<CourseSpinnerData.Data> coursecategories, ArrayList<Integer> userArray) {
        fragmentAssignmentQuizeBinding.assignmentCourseSpinner.setAdapter(courseSpinnerAdapter);
        fragmentAssignmentQuizeBinding.assignmentCourseSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                CourseSpinnerData.Data data = coursecategories.get(i);
                couresId = Integer.parseInt(String.valueOf(data.getId()));
                if (flag == 0) {
                    callApiForQuizChart(userArray);
                } else {
                    callApiForAssignmentChart(userArray);
                }

            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });
    }

//    private void getOverAllQuizSpinner(ArrayAdapter<CourseSpinnerData.FillesData> courseSpinnerAdapter, List<CourseSpinnerData.FillesData> coursecategories) {
//        fragmentAssignmentQuizeBinding.spcourseOverall.setAdapter(courseSpinnerAdapter);
//        fragmentAssignmentQuizeBinding.spcourseOverall.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
//            @Override
//            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
//                CourseSpinnerData.FillesData data = coursecategories.get(i);
//                couresId = Integer.parseInt(String.valueOf(data.getId()));
//                if (flag == 0) {
//                    callApiPieChartForOverAllQuiz();
//                } else {
//                    callApiPieChartForAssignment();
//                }
//
//            }
//
//            @Override
//            public void onNothingSelected(AdapterView<?> adapterView) {
//
//            }
//        });
//    }

    private void getPerformanceQuizSpinner(ArrayAdapter<CourseSpinnerData.Data> courseSpinnerAdapter, List<CourseSpinnerData.Data> coursecategories, ArrayList<Integer> userArray) {
        courseSpinnerAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        fragmentAssignmentQuizeBinding.spcourse.setAdapter(courseSpinnerAdapter);
        fragmentAssignmentQuizeBinding.spcourse.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                CourseSpinnerData.Data data = coursecategories.get(i);
                couresId = Integer.parseInt(String.valueOf(data.getId()));
                if (flag == 0) {
                    getQuizProgressPieChartApi(userArray);
                } else {
                    getAssignmentProgressPieChartApi(userArray);
                }

            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });
    }

    private void setTitle() {
        if (flag == 0) {
            fragmentAssignmentQuizeBinding.tvTitle.setText(getResources().getString(R.string.performance_in_quiz));
            fragmentAssignmentQuizeBinding.tvTitleOvertime.setText(getResources().getString(R.string.quizzes_progress_overtime));
//            fragmentAssignmentQuizeBinding.tvTitleOverall.setText(R.string.overall_performance_in_quiz);
        } else {
            fragmentAssignmentQuizeBinding.tvTitle.setText(getResources().getString(R.string.performance_in_assignments));
            fragmentAssignmentQuizeBinding.tvTitleOvertime.setText(getResources().getString(R.string.assignment_progress_overtime));
//            fragmentAssignmentQuizeBinding.tvTitleOverall.setText(R.string.overall_performance_in_assignments);
        }
    }

    private void getflag() {
        Bundle bundle = getArguments();
        if (bundle != null) {
            flag = bundle.getInt("flag");
        }

    }

    private void getQuizProgressPieChartApi(ArrayList<Integer> userArray) {
        JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userArray.size(); i++) {
                array.add(userArray.get(i));
            }
            jsonObject.add("id", array);
            jsonObject.addProperty("month", 0);
            jsonObject.addProperty("courseid", couresId);
            jsonObject.addProperty("content", 0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(parentControlRepository.getQuizProgress(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ProgressReport>() {
                    @Override
                    public void onSuccess(ProgressReport chart) {
                        if (chart.getData() != null) {
                            fragmentAssignmentQuizeBinding.tvTakenQuiz.setText(NoonApplication.getContext().getString(R.string.total_quiztaken).concat(chart.getData().getTotaltries()));
                            DecimalFormat decimalFormat = new DecimalFormat("0.00");
                            fragmentAssignmentQuizeBinding.txtPieScore.setText(NoonApplication.getContext().getString(R.string.score_pie_chart).concat("\n").concat(decimalFormat.format(chart.getData().getOverallscore())).concat("%"));
                            fragmentAssignmentQuizeBinding.txtPerNodata.setVisibility(View.GONE);
                            fragmentAssignmentQuizeBinding.txtPieScore.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.pieChart.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.pieChart.setUsePercentValues(true);
                            PieDataSet dataSet = new PieDataSet(piedataValues(chart.getData()), "");
                            PieData data = new PieData(dataSet);
                            data.setValueFormatter(new PercentFormatter());
                            fragmentAssignmentQuizeBinding.pieChart.setData(data);
                            fragmentAssignmentQuizeBinding.pieChart.setEntryLabelColor(Color.BLACK);
                            fragmentAssignmentQuizeBinding.pieChart.getDescription().setText("");
                            fragmentAssignmentQuizeBinding.pieChart.setDrawHoleEnabled(true);
                            fragmentAssignmentQuizeBinding.pieChart.setTransparentCircleRadius(25f);
                            fragmentAssignmentQuizeBinding.pieChart.setHoleRadius(55f);
                            fragmentAssignmentQuizeBinding.pieChart.getLegend().setEnabled(false);

                            dataSet.setColors(color1);
                            data.setValueTextSize(13f);
                            data.setValueTextColor(Color.BLACK);
                            fragmentAssignmentQuizeBinding.pieChart.setTouchEnabled(false);
                            data.setValueFormatter((value, entry, dataSetIndex, viewPortHandler) -> Math.round(value) + "%");

                            Typeface typeface = ResourcesCompat.getFont(Objects.requireNonNull(getActivity()), R.font.bahij_helvetica_neue_bold);
                            data.setValueTypeface(typeface);
                            fragmentAssignmentQuizeBinding.pieChart.setEntryLabelTypeface(typeface);
                            fragmentAssignmentQuizeBinding.pieChart.invalidate();
                        } else {
                            fragmentAssignmentQuizeBinding.tvTakenQuiz.setText(NoonApplication.getContext().getString(R.string.total_quiztaken).concat("0"));
                            fragmentAssignmentQuizeBinding.txtPerNodata.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.pieChart.setVisibility(View.GONE);
                            fragmentAssignmentQuizeBinding.txtPieScore.setVisibility(View.GONE);
                        }

                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));
    }

    private void getAssignmentProgressPieChartApi(ArrayList<Integer> userArray) {
        JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userArray.size(); i++) {
                array.add(userArray.get(i));
            }
            jsonObject.add("id", array);
            jsonObject.addProperty("month", 0);
            jsonObject.addProperty("courseid", couresId);
            jsonObject.addProperty("content", 0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(parentControlRepository.getAssignmentProgress(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ProgressReport>() {
                    @Override
                    public void onSuccess(ProgressReport chart) {
                        if (chart.getData() != null) {
                            fragmentAssignmentQuizeBinding.tvTakenQuiz.setText(NoonApplication.getContext().getString(R.string.total_submission).concat(chart.getData().getTotaltries()));
                            DecimalFormat decimalFormat = new DecimalFormat("0.00");
                            fragmentAssignmentQuizeBinding.txtPieScore.setText(NoonApplication.getContext().getString(R.string.score_pie_chart).concat("\n").concat(decimalFormat.format(chart.getData().getOverallscore())).concat("%"));
                            fragmentAssignmentQuizeBinding.txtPerNodata.setVisibility(View.GONE);
                            fragmentAssignmentQuizeBinding.txtPieScore.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.pieChart.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.pieChart.setUsePercentValues(true);
                            PieDataSet dataSet = new PieDataSet(piedataValues(chart.getData()), "");
                            PieData data = new PieData(dataSet);
                            data.setValueFormatter(new PercentFormatter());
                            fragmentAssignmentQuizeBinding.pieChart.setData(data);

                            fragmentAssignmentQuizeBinding.pieChart.setEntryLabelColor(Color.BLACK);
                            fragmentAssignmentQuizeBinding.pieChart.getDescription().setText("");
                            fragmentAssignmentQuizeBinding.pieChart.setDrawHoleEnabled(true);
                            fragmentAssignmentQuizeBinding.pieChart.setTransparentCircleRadius(25f);
                            fragmentAssignmentQuizeBinding.pieChart.setHoleRadius(55f);
                            fragmentAssignmentQuizeBinding.pieChart.getLegend().setEnabled(false);

                            dataSet.setColors(color1);
                            data.setValueTextSize(13f);
                            data.setValueTextColor(Color.BLACK);
                            fragmentAssignmentQuizeBinding.pieChart.setTouchEnabled(false);
                            data.setValueFormatter((value, entry, dataSetIndex, viewPortHandler) -> Math.round(value) + "%");

                            Typeface typeface = ResourcesCompat.getFont(Objects.requireNonNull(getActivity()), R.font.bahij_helvetica_neue_bold);
                            data.setValueTypeface(typeface);
                            fragmentAssignmentQuizeBinding.pieChart.setEntryLabelTypeface(typeface);
                            fragmentAssignmentQuizeBinding.pieChart.invalidate();
                        } else {
                            fragmentAssignmentQuizeBinding.tvTakenQuiz.setText(NoonApplication.getContext().getString(R.string.total_submission).concat("0"));
                            fragmentAssignmentQuizeBinding.txtPerNodata.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.pieChart.setVisibility(View.GONE);
                            fragmentAssignmentQuizeBinding.txtPieScore.setVisibility(View.GONE);
                        }

                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));
    }

  /*  private void callApiPieChartForAssignment() {
  JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userIdArray.size(); i++) {
                array.add(userIdArray.get(i));
            }
            jsonObject.add("id",array);
            jsonObject.addProperty("month",0);
            jsonObject.addProperty("courseid",couresId);
            jsonObject.addProperty("content",0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(apiService.getAssignmentPieChartData(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ProgressReport>() {
                    @Override
                    public void onSuccess(ProgressReport progressReport) {
                        if (progressReport.getResponse_code().equals("0")) {
                            if (progressReport.getData() != null) {
                                pieChartForOverAll(progressReport.getData());
                                fragmentAssignmentQuizeBinding.txtNodataOverall.setVisibility(View.GONE);
                                fragmentAssignmentQuizeBinding.pieChartoverAll.setVisibility(View.VISIBLE);
                            } else {
                                fragmentAssignmentQuizeBinding.txtNodataOverall.setVisibility(View.VISIBLE);
                                fragmentAssignmentQuizeBinding.pieChartoverAll.setVisibility(View.GONE);
                            }

                        }
                    }

                    @Override
                    public void onError(Throwable e) {

                    }
                }));
    }

    private void callApiPieChartForOverAllQuiz() {
    JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userIdArray.size(); i++) {
                array.add(userIdArray.get(i));
            }
            jsonObject.add("id",array);
            jsonObject.addProperty("month",0);
            jsonObject.addProperty("courseid",couresId);
            jsonObject.addProperty("content",0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(apiService.getQuizPieChartData(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ProgressReport>() {
                    @Override
                    public void onSuccess(ProgressReport progressReport) {
                        if (progressReport.getResponse_code().equals("0")) {
                            if (progressReport.getData() != null) {
                                pieChartForOverAll(progressReport.getData());
                                fragmentAssignmentQuizeBinding.txtNodataOverall.setVisibility(View.GONE);
                                fragmentAssignmentQuizeBinding.pieChartoverAll.setVisibility(View.VISIBLE);
                            } else {
                                fragmentAssignmentQuizeBinding.txtNodataOverall.setVisibility(View.VISIBLE);
                                fragmentAssignmentQuizeBinding.pieChartoverAll.setVisibility(View.GONE);
                            }

                        }
                    }

                    @Override
                    public void onError(Throwable e) {

                    }
                }));
    }

    private void pieChartForOverAll(ProgressReport.FillesData quizPieChart) {

        fragmentAssignmentQuizeBinding.pieChartoverAll.setUsePercentValues(true);
        PieDataSet dataSet = new PieDataSet(piedataValues(quizPieChart), "");

        PieData data = new PieData(dataSet);
        data.setValueFormatter(new PercentFormatter());
        fragmentAssignmentQuizeBinding.pieChartoverAll.setData(data);
        fragmentAssignmentQuizeBinding.pieChartoverAll.setEntryLabelColor(Color.BLACK);
        fragmentAssignmentQuizeBinding.pieChartoverAll.getDescription().setText("");

        fragmentAssignmentQuizeBinding.pieChartoverAll.setDrawHoleEnabled(true);
        fragmentAssignmentQuizeBinding.pieChartoverAll.setTransparentCircleRadius(25f);
        fragmentAssignmentQuizeBinding.pieChartoverAll.setHoleRadius(55f);
        fragmentAssignmentQuizeBinding.pieChartoverAll.getLegend().setEnabled(false);

        dataSet.setColors(color1);
        data.setValueTextSize(13f);
        data.setValueTextColor(Color.BLACK);
        fragmentAssignmentQuizeBinding.pieChartoverAll.setTouchEnabled(false);
        data.setValueFormatter((value, entry, dataSetIndex, viewPortHandler) -> Math.round(value) + "%");

        Typeface typeface = ResourcesCompat.getFont(Objects.requireNonNull(getActivity()), R.font.bahij_helvetica_neue_bold);
        if (typeface != null) {
            data.setValueTypeface(typeface);
            fragmentAssignmentQuizeBinding.pieChartoverAll.setEntryLabelTypeface(typeface);
            fragmentAssignmentQuizeBinding.pieChartoverAll.invalidate();
        }
    }*/

    private ArrayList<PieEntry> piedataValues(ProgressReport.Data data) {
        ArrayList<PieEntry> yvalues = new ArrayList<>();
        float score = Float.parseFloat(data.getScore());
        float failScore = 100 - score;
        if (Math.round(score) == 100) {
            yvalues.add(new PieEntry(score, "Passed"));
            color1 = new int[]{Color.rgb(185, 249, 246)};
        } else if (score != 0) {
            yvalues.add(new PieEntry(score, "Passed"));
            yvalues.add(new PieEntry(failScore, "Failed"));
            color1 = new int[]{Color.rgb(185, 249, 246), Color.rgb(250, 209, 209)};
        } else {
            yvalues.add(new PieEntry(failScore, "Failed"));
            color1 = new int[]{Color.rgb(250, 209, 209)};
        }

        return yvalues;
    }

    private ArrayList<PieEntry> piedataValues() {
        ArrayList<PieEntry> yvalues = new ArrayList<>();
        yvalues.add(new PieEntry(57, "Passed"));
        yvalues.add(new PieEntry(43, "Failed"));
        return yvalues;
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        fragmentAssignmentQuizeBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_assignment_quize_performance, container, false);
        return fragmentAssignmentQuizeBinding.getRoot();
    }


//    private void callApiForAssignmentChart() {
//        disposable.add(apiService.getAssignmentChartData(userId, couresId)
//                .subscribeOn(Schedulers.io())
//                .observeOn(AndroidSchedulers.mainThread())
//                .subscribeWith(new DisposableSingleObserver<MChart>() {
//                    @Override
//                    public void onSuccess(MChart chart) {
//                        if (chart != null && chart.getData() != null && chart.getData() != null) {
//                            fragmentAssignmentQuizeBinding.lineChart.setVisibility(View.VISIBLE);
//                            fragmentAssignmentQuizeBinding.txtNodata.setVisibility(View.GONE);
//                            List<MChart.FillesData> data = chart.getData();
//                            LineDataSet lineDataSet = new LineDataSet(dataValues(data), "");
//                            Drawable drawable = ContextCompat.getDrawable(Objects.requireNonNull(getActivity()), R.drawable.graphview_shadow);
//                            lineDataSet.setFillDrawable(drawable);
//                            lineDataSet.setCircleRadius(6f);
//                            lineDataSet.setLineWidth(3f);
//                            lineDataSet.setCircleHoleRadius(4f);
//                            lineDataSet.setDrawCircleHole(true);
//                            lineDataSet.setDrawIcons(false);
//                            lineDataSet.setValueFormatter((value, entry, dataSetIndex, viewPortHandler) -> "");
//                            lineDataSet.setMode(LineDataSet.Mode.HORIZONTAL_BEZIER);   //set curves in ghaphview
//                            lineDataSet.setDrawFilled(true);
////        lineDataSet.setGradientColor(Color.BLUE,0);
////        lineDataSet.setMode(LineDataSet.Mode.CUBIC_BEZIER);
////        lineDataSet.setFillColor(getResources().getColor(R.color.colorPrimaryDark));
////        lineDataSet.setDrawFilled(true); // fill color under the graphview
////        lineDataSet.setValueTextSize(9f);
////        lineDataSet.setColor(Color.BLACK); //Change Line color
////        lineDataSet.setCircleColor(Color.BLACK); //Change Circle Color
//                            final ArrayList<String> yLabel = new ArrayList<>();
//                            int max = getMax(dataValues(data));
//                            for (int i = 0; i <= max; i++) {
//                                yLabel.add(i + " Hours");
//                            }
//
//                            YAxis yAxis = fragmentAssignmentQuizeBinding.lineChart.getAxisLeft();
//                            yAxis.setDrawGridLines(true);
//                            yAxis.setAxisLineColor(Color.parseColor("#00000000"));
//                            yAxis.setTextColor(Color.BLACK);
//                            yAxis.setValueFormatter((value, axis) -> {
//                                if (yLabel.size() >= value) {
//                                    return Math.round(value) + " Points";
//                                }
//                                return "";
//                            });
//                            final ArrayList<String> xLabel = new ArrayList<>();
//                            xLabel.add("month");
//                            xLabel.add("Jan");
//                            xLabel.add("Feb");
//                            xLabel.add("Mar");
//                            xLabel.add("Apr");
//                            xLabel.add("May");
//                            xLabel.add("Jun");
//                            xLabel.add("july");
//                            xLabel.add("aug");
//                            xLabel.add("sept");
//                            xLabel.add("oct");
//                            xLabel.add("nov");
//                            xLabel.add("dec");
//                            XAxis xAxis = fragmentAssignmentQuizeBinding.lineChart.getXAxis();
//                            xAxis.setDrawGridLines(false);
//                            xAxis.setDrawAxisLine(true);
//                            xAxis.setAxisLineColor(Color.WHITE);
//                            xAxis.setTextColor(Color.BLACK);
//                            //xAxis.setLabelCount(dataValues(data).size(), true);
//                            xAxis.setValueFormatter((value, axis) -> {
//                                if (xLabel.size() >= value) {
//                                    return xLabel.get(Math.round(value));
//                                }
//                                return "";
//                            });
//                            ArrayList<ILineDataSet> iLineDataSets = new ArrayList<>();
//                            iLineDataSets.add(lineDataSet);
//                            LineData data1 = new LineData(iLineDataSets);
//                            fragmentAssignmentQuizeBinding.lineChart.setBackgroundColor(Color.WHITE);
//                            fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setDrawLabels(false);
//                            fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setDrawGridLines(false);
//                            fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setAxisLineColor(Color.parseColor("#00000000"));
//                            fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setDrawLabels(true);
//                            fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setEnabled(true);
//                            fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setPosition(YAxis.YAxisLabelPosition.OUTSIDE_CHART);
//                            fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setGridColor(getResources().getColor(R.color.colorlightDarkGray));
//                            fragmentAssignmentQuizeBinding.lineChart.getXAxis().setDrawLabels(true);
//                            fragmentAssignmentQuizeBinding.lineChart.getXAxis().setEnabled(true);
//                            fragmentAssignmentQuizeBinding.lineChart.getXAxis().setPosition(XAxis.XAxisPosition.BOTTOM_INSIDE);
//                            fragmentAssignmentQuizeBinding.lineChart.setData(data1);
//                            fragmentAssignmentQuizeBinding.lineChart.invalidate();
//                            fragmentAssignmentQuizeBinding.lineChart.getDescription().setEnabled(false);
//
//                            // enable scaling and dragging
//                            fragmentAssignmentQuizeBinding.lineChart.setDragEnabled(false);
//                            fragmentAssignmentQuizeBinding.lineChart.setScaleEnabled(false);
//                            fragmentAssignmentQuizeBinding.lineChart.getLegend().setEnabled(false);
//                            Log.e("chart", "onSuccess: " + chart.toString());
//                        } else {
//                            fragmentAssignmentQuizeBinding.lineChart.setVisibility(View.GONE);
//                            fragmentAssignmentQuizeBinding.txtNodata.setVisibility(View.VISIBLE);
//                        }
//
//                        hideDialog();
//                    }
//
//                    @Override
//                    public void onError(Throwable e) {
//                        hideDialog();
//                    }
//                }));
//    }

    private void callApiForAssignmentChart(ArrayList<Integer> userArray) {
        JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userArray.size(); i++) {
                array.add(userArray.get(i));
            }

            jsonObject.add("id", array);
            jsonObject.addProperty("month", 0);
            jsonObject.addProperty("courseid", couresId);
            jsonObject.addProperty("content", 0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(parentControlRepository.getAssignmentChartData(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<Chart>() {
                    @Override
                    public void onSuccess(Chart chart) {
                        if (chart != null && chart.getData() != null && chart.getData() != null) {
                            fragmentAssignmentQuizeBinding.lineChart.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.txtNodata.setVisibility(View.GONE);
                            Chart.Data data = chart.getData();
                            drawDateChart(data);
//                            drawMonthChart(data);
                        } else {
                            fragmentAssignmentQuizeBinding.lineChart.setVisibility(View.GONE);
                            fragmentAssignmentQuizeBinding.txtNodata.setVisibility(View.VISIBLE);
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
        fragmentAssignmentQuizeBinding.lineChart.clear();
        fragmentAssignmentQuizeBinding.lineChart.resetZoom();
        fragmentAssignmentQuizeBinding.lineChart.zoomOut();
        LineDataSet lineDataSet = new LineDataSet(dataForMonth(data.getMo()), "");

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

        int max = getMax(dataForMonth(data.getMo()));
        for (int i = 0; i <= max; i++) {
            yLabel.add(i + " %");
        }
        YAxis yAxis = fragmentAssignmentQuizeBinding.lineChart.getAxisLeft();
        yAxis.setDrawGridLines(true);
        yAxis.setAxisLineColor(Color.parseColor("#00000000"));
        yAxis.setTextColor(Color.BLACK);
        yAxis.setValueFormatter((value, axis) -> {
            if (yLabel.size() >= value) {
                return new DecimalFormat("##.##").format(value) + " %";
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
        XAxis xAxis = fragmentAssignmentQuizeBinding.lineChart.getXAxis();
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
//        fragmentAssignmentQuizeBinding.lineChart.animateX(500);
        fragmentAssignmentQuizeBinding.lineChart.setBackgroundColor(Color.WHITE);
        fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setDrawLabels(false);
        fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setDrawGridLines(false);
        fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setAxisLineColor(Color.parseColor("#00000000"));
        fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setDrawLabels(true);
        fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setEnabled(true);
        fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setPosition(YAxis.YAxisLabelPosition.OUTSIDE_CHART);
        fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setGridColor(getResources().getColor(R.color.colorlightDarkGray));
        fragmentAssignmentQuizeBinding.lineChart.getXAxis().setDrawLabels(true);
        fragmentAssignmentQuizeBinding.lineChart.getXAxis().setEnabled(true);
        fragmentAssignmentQuizeBinding.lineChart.getXAxis().setPosition(XAxis.XAxisPosition.BOTTOM_INSIDE);
        fragmentAssignmentQuizeBinding.lineChart.setData(data1);
        fragmentAssignmentQuizeBinding.lineChart.getDescription().setEnabled(false);
        fragmentAssignmentQuizeBinding.lineChart.getXAxis().setGranularity(1);
        fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setGranularity(1);
        fragmentAssignmentQuizeBinding.lineChart.setDragEnabled(true);
        fragmentAssignmentQuizeBinding.lineChart.setScaleEnabled(true);
        fragmentAssignmentQuizeBinding.lineChart.getLegend().setEnabled(false);
        fragmentAssignmentQuizeBinding.lineChart.setOnChartGestureListener(new OnChartGestureListener() {
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
                float scaleX = fragmentAssignmentQuizeBinding.lineChart.getScaleX();
                float scaleY = fragmentAssignmentQuizeBinding.lineChart.getScaleY();
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
        fragmentAssignmentQuizeBinding.lineChart.invalidate();
    }


    private ArrayList<Entry> dataForMonth(List<Chart.Mo> data) {
        ArrayList<Entry> dataVals = new ArrayList<>();
        if (data != null && !data.isEmpty()) {
            for (int i = 0; i < data.size(); i++) {
                dataVals.add(new Entry(i, data.get(i).getY()));
            }

        }
        return dataVals;
    }

    private void drawDateChart(Chart.Data data) {
        fragmentAssignmentQuizeBinding.lineChart.clear();
        fragmentAssignmentQuizeBinding.lineChart.resetZoom();
        fragmentAssignmentQuizeBinding.lineChart.zoomOut();
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
            yLabel.add(i + " %");
        }
        YAxis yAxis = fragmentAssignmentQuizeBinding.lineChart.getAxisLeft();
        yAxis.setDrawGridLines(true);
        yAxis.setAxisLineColor(Color.parseColor("#00000000"));
        yAxis.setTextColor(Color.BLACK);
        yAxis.setValueFormatter((value, axis) -> {
            if (yLabel.size() >= value) {
                return new DecimalFormat("##.##").format(value) + " %";
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
        XAxis xAxis = fragmentAssignmentQuizeBinding.lineChart.getXAxis();
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
//        fragmentAssignmentQuizeBinding.lineChart.animateX(500);
        fragmentAssignmentQuizeBinding.lineChart.setBackgroundColor(Color.WHITE);
        fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setDrawLabels(false);
        fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setDrawGridLines(false);
        fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setAxisLineColor(Color.parseColor("#00000000"));
        fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setDrawLabels(true);
        fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setEnabled(true);
        fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setPosition(YAxis.YAxisLabelPosition.OUTSIDE_CHART);
        fragmentAssignmentQuizeBinding.lineChart.getAxisLeft().setGridColor(getResources().getColor(R.color.colorlightDarkGray));
        fragmentAssignmentQuizeBinding.lineChart.getXAxis().setDrawLabels(true);
        fragmentAssignmentQuizeBinding.lineChart.getXAxis().setEnabled(true);
        fragmentAssignmentQuizeBinding.lineChart.getXAxis().setPosition(XAxis.XAxisPosition.BOTTOM_INSIDE);
        fragmentAssignmentQuizeBinding.lineChart.setData(data1);
        fragmentAssignmentQuizeBinding.lineChart.getXAxis().setGranularity(1);
        fragmentAssignmentQuizeBinding.lineChart.getAxisRight().setGranularity(1);
        fragmentAssignmentQuizeBinding.lineChart.getDescription().setEnabled(false);
        fragmentAssignmentQuizeBinding.lineChart.setDragEnabled(true);
        fragmentAssignmentQuizeBinding.lineChart.setScaleEnabled(true);
        fragmentAssignmentQuizeBinding.lineChart.getLegend().setEnabled(false);
        fragmentAssignmentQuizeBinding.lineChart.setOnChartGestureListener(new OnChartGestureListener() {
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
                float scaleX = fragmentAssignmentQuizeBinding.lineChart.getScaleX();
                float scaleY = fragmentAssignmentQuizeBinding.lineChart.getScaleY();
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
        fragmentAssignmentQuizeBinding.lineChart.invalidate();
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


    private void callApiForQuizChart(ArrayList<Integer> userArray) {
        JsonObject jsonObject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userArray.size(); i++) {
                array.add(userArray.get(i));
            }
            jsonObject.add("id", array);
            jsonObject.addProperty("month", 0);
            jsonObject.addProperty("courseid", couresId);
            jsonObject.addProperty("content", 0);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(parentControlRepository.getQuizChartData(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<Chart>() {
                    @Override
                    public void onSuccess(Chart chart) {
                        if (chart != null && chart.getData() != null && chart.getData() != null) {
                            fragmentAssignmentQuizeBinding.lineChart.setVisibility(View.VISIBLE);
                            fragmentAssignmentQuizeBinding.txtNodata.setVisibility(View.GONE);
                            Chart.Data data = chart.getData();
//                            drawMonthChart(data);
                            drawDateChart(data);
                        } else {
                            fragmentAssignmentQuizeBinding.lineChart.setVisibility(View.GONE);
                            fragmentAssignmentQuizeBinding.txtNodata.setVisibility(View.VISIBLE);
                        }

                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));
    }


//    private ArrayList<Entry> dataValues(List<MChart.FillesData> data) {
//        ArrayList<Entry> dataVals = new ArrayList<>();
//        if (data != null && !data.isEmpty()) {
//            for (int i = 0; i < data.size(); i++) {
//                dataVals.add(new Entry(data.get(i).getX(), data.get(i).getY()));
//            }
//
//        }
//        return dataVals;
//    }

//    private ArrayList<Entry> dataValues() {
//        ArrayList<Entry> dataVals = new ArrayList<>();
//        dataVals.add(new Entry(0, 8));
//        dataVals.add(new Entry(1, 2f));
//        dataVals.add(new Entry(2, 4f));
//        dataVals.add(new Entry(3, 0f));
//        dataVals.add(new Entry(4, 4.5f));
//        dataVals.add(new Entry(5, 2.75f));
//        return dataVals;
//    }

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

}
