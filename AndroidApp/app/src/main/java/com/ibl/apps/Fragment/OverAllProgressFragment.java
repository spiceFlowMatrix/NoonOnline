package com.ibl.apps.Fragment;


import android.os.Bundle;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;

import androidx.annotation.NonNull;
import androidx.databinding.DataBindingUtil;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.parent.CourseSpinnerData;
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
import com.ibl.apps.noon.databinding.FragmentOverAllProgressBinding;
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

import static android.view.View.GONE;
import static com.ibl.apps.Base.BaseActivity.apiService;
import static com.ibl.apps.Fragment.ProgressReportFragment.selectedIdsForCallback;

public class OverAllProgressFragment extends BaseFragment implements View.OnClickListener {

    FragmentOverAllProgressBinding fragmentoverAllProgressBinding;
    private UserDetails userDetails;
    CompositeDisposable disposable = new CompositeDisposable();
    private int userId = 0;
    private int courseId = 0;
    private int month = 0;
    private int content = 0;
    private ArrayList<Integer> userIdArray = new ArrayList<>();
    private List<ParentSpinnerModel.Data> studentNamecategories;
    private ParentSpinnerModel parentSpinner = new ParentSpinnerModel();
    private ParentControlRepository parentControlRepository;


    public OverAllProgressFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        fragmentoverAllProgressBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_over_all_progress, container, false);
        return fragmentoverAllProgressBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        showDialog(Objects.requireNonNull(getActivity()).getResources().getString(R.string.loading));
        //setUpProgress();
        parentControlRepository = new ParentControlRepository();
        callApiForStudentSpinner();
        setonClickListner();
    }

    private void setonClickListner() {
        fragmentoverAllProgressBinding.btnbackProgress.setOnClickListener(this);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.btnbackProgress:
                Objects.requireNonNull(getActivity()).onBackPressed();
                break;
        }
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
                            ArrayList<CourseSpinnerData.Data> coursecategories = new ArrayList<>();
                            CourseSpinnerData.Data data = new CourseSpinnerData.Data();
                            data.setName(NoonApplication.getContext().getResources().getString(R.string.all_courses));
                            data.setId(0L);
                            coursecategories.add(data);
                            coursecategories.addAll(courseSpinnerData.getData());

                            ArrayAdapter<CourseSpinnerData.Data> courseAdapter = new ArrayAdapter<>(NoonApplication.getContext(), R.layout.include_spinner, coursecategories);
                            courseAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
                            fragmentoverAllProgressBinding.spCourse.setAdapter(courseAdapter);
                            fragmentoverAllProgressBinding.spCourse.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
                                @Override
                                public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                                    courseId = Integer.parseInt(String.valueOf(coursecategories.get(i).getId()));
                                    setUpContentSpinner(userArray);
                                }

                                @Override
                                public void onNothingSelected(AdapterView<?> adapterView) {

                                }
                            });
                            hideDialog();
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
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
                                String timeagotxt = timeAgo.covertTimeToText(getDate);
                                fragmentoverAllProgressBinding.tvLastonline.setText(timeagotxt);
                            } else {
                                fragmentoverAllProgressBinding.tvLastonline.setText("--");
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            fragmentoverAllProgressBinding.tvLastonline.setText("--");
                        }
                    }));
        } else {
            fragmentoverAllProgressBinding.tvLastonline.setText("--");
        }
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


    private void setUpContentSpinner(ArrayList<Integer> userArray) {
        List<String> contentCategories = new ArrayList<>();
        contentCategories.add(0, Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_content));
        contentCategories.add(1, Objects.requireNonNull(getActivity()).getResources().getString(R.string.lesson));
        contentCategories.add(2, Objects.requireNonNull(getActivity()).getResources().getString(R.string.quiz));
        contentCategories.add(3, getActivity().getResources().getString(R.string.assignment));

        ArrayAdapter<String> contentAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, contentCategories);
        contentAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        fragmentoverAllProgressBinding.spContent.setAdapter(contentAdapter);
        fragmentoverAllProgressBinding.spContent.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                switch (i) {
                    case 0:
                        content = 0;
                        break;
                    case 1:
                        content = 1;
                        break;
                    case 2:
                        content = 2;
                        break;
                    case 3:
                        content = 3;
                        break;

                }
                setUpMonthSpinner(userArray);
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });
    }

    private void setUpMonthSpinner(ArrayList<Integer> userArray) {
        List<String> monthscategories = new ArrayList<>();
        monthscategories.add(0, Objects.requireNonNull(getActivity()).getResources().getString(R.string.last_one_month));
        monthscategories.add(1, getActivity().getResources().getString(R.string.last_three_month));
        monthscategories.add(2, getActivity().getResources().getString(R.string.last_six_month));

        ArrayAdapter<String> monthsdataAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, monthscategories);
        monthsdataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        fragmentoverAllProgressBinding.spMonths.setAdapter(monthsdataAdapter);
        fragmentoverAllProgressBinding.spMonths.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
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
                callApiForOverAllProgress(userArray);
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {

            }
        });
    }

    private void callApiForOverAllProgress(ArrayList<Integer> userArray) {
        JsonObject jsonobject = new JsonObject();
        try {
            JsonArray array = new JsonArray();
            for (int i = 0; i < userArray.size(); i++) {
                array.add(userArray.get(i));
            }
            jsonobject.add("id", array);
            jsonobject.addProperty("month", month);
            jsonobject.addProperty("courseid", courseId);
            jsonobject.addProperty("content", content);
        } catch (JsonSyntaxException e) {
            e.printStackTrace();
        }
        disposable.add(parentControlRepository.getOverAllProgress(jsonobject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ProgressReport>() {
                    @Override
                    public void onSuccess(ProgressReport progressReport) {
                        if (progressReport != null && progressReport.getData() != null) {
                            fragmentoverAllProgressBinding.backgroundProgressbar.setVisibility(View.VISIBLE);
                            fragmentoverAllProgressBinding.backgroundProgressbar.setMax(100); // Maximum Progress
                            fragmentoverAllProgressBinding.backgroundProgressbar.setSecondaryProgress(100); //
                            fragmentoverAllProgressBinding.backgroundProgressbar.setProgress(Math.round(Float.parseFloat(progressReport.getData().getScore())));
                            DecimalFormat decimalFormat = new DecimalFormat("0.00");
                            fragmentoverAllProgressBinding.txtProgress.setText(String.valueOf(decimalFormat.format(Float.parseFloat(progressReport.getData().getScore()))).concat(" %"));
                        } else {
                            fragmentoverAllProgressBinding.backgroundProgressbar.setVisibility(View.GONE);
                            fragmentoverAllProgressBinding.txtProgress.setText(R.string.no_data_found);
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
//        MyAdapter studentNameAdapter = new MyAdapter(Objects.requireNonNull(getActivity()), 0, studentNamecategories, this);
//        ArrayAdapter<ParentSpinnerModel.FillesData> studentNamedataAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, studentNamecategories);
//        // Drop down layout style - list view with radio button
//        studentNamedataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);

//        fragmentoverAllProgressBinding.studentName.setAdapter(studentNameAdapter);

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
            fragmentoverAllProgressBinding.allUser.setVisibility(View.VISIBLE);
            fragmentoverAllProgressBinding.spUser.setText(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_users));
            fragmentoverAllProgressBinding.allUser.setText("( " + userArray.size() + " )");
        } else {
            fragmentoverAllProgressBinding.spUser.setText(userArray.size() + " " + getActivity().getResources().getString(R.string.user_selected));
            fragmentoverAllProgressBinding.allUser.setVisibility(GONE);
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
                            fragmentoverAllProgressBinding.allUser.setVisibility(View.VISIBLE);
                            fragmentoverAllProgressBinding.spUser.setText(Objects.requireNonNull(getActivity()).getResources().getString(R.string.all_users));
                            fragmentoverAllProgressBinding.allUser.setText("( " + var1.size() + " )");
                        } else {
                            fragmentoverAllProgressBinding.spUser.setText(var1.size() + " " + getActivity().getResources().getString(R.string.user_selected));
                            fragmentoverAllProgressBinding.allUser.setVisibility(GONE);
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
        fragmentoverAllProgressBinding.spUserLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                multiSelectSpinner.show(Objects.requireNonNull(getActivity()).getSupportFragmentManager(), "multiSelectDialog");
            }
        });

        /*if (MyAdapter.userId.isEmpty()) {
            for (int j = 0; j < parentSpinnerModel.getData().size(); j++) {
                if (parentSpinnerModel.getData().get(j).getId() != 0L && !MyAdapter.userId.contains(Integer.parseInt(String.valueOf(parentSpinnerModel.getData().get(j).getId())))) {
                    MyAdapter.userId.add(Integer.valueOf(String.valueOf(parentSpinnerModel.getData().get(j).getId())));
                }
            }
            ((MyAdapter) fragmentoverAllProgressBinding.studentName.getAdapter()).notifyDataSetChanged();
            callApiForCourseSpinner(MyAdapter.userId);

        } else {
            ((MyAdapter)fragmentoverAllProgressBinding.studentName.getAdapter()).notifyDataSetChanged();
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
                            parentSpinner = parentSpinnerModel;
                            studentNamecategories = parentSpinnerModel.getData();
                            setDataInStudentSpinner(parentSpinnerModel);
                        }
                       /* List<ParentSpinnerModel.FillesData> studentNamecategories = new ArrayList<>();
                        studentNamecategories = parentSpinnerModel.getData();
                        ArrayAdapter<ParentSpinnerModel.FillesData> studentNamedataAdapter = new ArrayAdapter<>(Objects.requireNonNull(getActivity()), R.layout.include_spinner, studentNamecategories);
                        // Drop down layout style - list view with radio button
                        studentNamedataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
                        fragmentoverAllProgressBinding.studentName.setAdapter(studentNamedataAdapter);
                        List<ParentSpinnerModel.FillesData> finalStudentNamecategories = studentNamecategories;
                        fragmentoverAllProgressBinding.studentName.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
                            @Override
                            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                                ParentSpinnerModel.FillesData data = finalStudentNamecategories.get(i);
                                userId = Integer.parseInt(String.valueOf(data.getId()));
                                userIdArray =new ArrayList<>();
                                userIdArray.add(userId);
                                callApiForCourseSpinner(userIdArray);
                            }

                            @Override
                            public void onNothingSelected(AdapterView<?> adapterView) {

                            }
                        });*/
                    }

                    @Override
                    public void onError(Throwable e) {

                    }
                }));
    }

}
