package com.ibl.apps.noon;

import android.content.Intent;
import android.graphics.Typeface;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.AutoCompleteTextView;
import android.widget.SearchView;

import androidx.core.content.res.ResourcesCompat;

import com.google.gson.Gson;
import com.ibl.apps.Adapter.SearchCoursesAdapter;
import com.ibl.apps.Adapter.SearchLessonAdapter;
import com.ibl.apps.Adapter.SpinnerItemAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.CourseManagement.CourseRepository;
import com.ibl.apps.Model.AllGradeObject;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.SearchObject;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.LessonDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.databinding.SearchLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;
import com.mancj.slideup.SlideUp;
import com.mancj.slideup.SlideUpBuilder;

import java.lang.reflect.Field;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.List;
import java.util.Objects;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;


public class SearchActivity extends BaseActivity implements View.OnClickListener {

    SearchLayoutBinding searchLayoutBinding;
    CompositeDisposable disposable = new CompositeDisposable();
    SlideUp slideUp;
    SearchCoursesAdapter searchCoursesAdapter;
    SearchLessonAdapter searchLessonAdapter;
    SpinnerItemAdapter spinneradp;
    ArrayList<AllGradeObject.Data> spinnerGradelist = new ArrayList<>();
    ArrayList<SearchObject.Courses> listProg = new ArrayList<>();
    ArrayList<SearchObject.Lessons> listProglesson = new ArrayList<>();
    public String SearchText = "", bygrade = "";
    String userId = "0";
    UserDetails userDetailsObject = new UserDetails();
    private CourseRepository courseRepository; //13 usage
    private CourseDatabaseRepository courseDatabaseRepository;
    private LessonDatabaseRepository lessonDatabaseRepository;

    @Override
    protected int getContentView() {
        return R.layout.search_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        searchLayoutBinding = (SearchLayoutBinding) getBindObj();
        courseRepository = new CourseRepository();
        courseDatabaseRepository = new CourseDatabaseRepository();
        lessonDatabaseRepository = new LessonDatabaseRepository();

        Typeface typeface = ResourcesCompat.getFont(SearchActivity.this, R.font.bahij_helvetica_neue_bold);
        searchLayoutBinding.advanceSearchLayout.chknew.setTypeface(typeface);
        searchLayoutBinding.advanceSearchLayout.chkongoing.setTypeface(typeface);
        searchLayoutBinding.advanceSearchLayout.chkterminated.setTypeface(typeface);
        searchLayoutBinding.advanceSearchLayout.chkcompleted.setTypeface(typeface);


        setToolbar(searchLayoutBinding.toolbarLayout.toolBar);

        searchLayoutBinding.toolbarLayout.searchBarLayout.setVisibility(View.VISIBLE);
        searchLayoutBinding.toolbarLayout.coursesearchview.setEnabled(false);
        searchLayoutBinding.toolbarLayout.coursesearchview.setActivated(true);
        searchLayoutBinding.toolbarLayout.coursesearchview.setQueryHint(getString(R.string.search_hint));
        searchLayoutBinding.toolbarLayout.coursesearchview.onActionViewExpanded();
        searchLayoutBinding.toolbarLayout.coursesearchview.setIconified(false);
        searchLayoutBinding.toolbarLayout.coursesearchview.setQuery("", false);
        searchLayoutBinding.toolbarLayout.coursesearchview.clearFocus();

        try {
            Field mDrawable = SearchView.class.getDeclaredField("mSearchHintIcon");
            mDrawable.setAccessible(true);
            Drawable drawable = (Drawable) mDrawable.get(searchLayoutBinding.toolbarLayout.coursesearchview);
            drawable.setBounds(0, 0, 0, 0);
        } catch (Exception e) {
            e.printStackTrace();
        }

        AutoCompleteTextView search_text = searchLayoutBinding.toolbarLayout.coursesearchview.findViewById(searchLayoutBinding.toolbarLayout.coursesearchview.getContext().getResources().getIdentifier("android:id/search_src_text", null, null));
        search_text.setTypeface(typeface);

        searchLayoutBinding.toolbarLayout.coursesearchview.setOnQueryTextListener(new SearchView.OnQueryTextListener() {

            @Override
            public boolean onQueryTextChange(String newText) {

                try {
                    if (TextUtils.isEmpty(newText)) {
                        if (!slideUp.isVisible()) {
                            SearchText = "";
                            bygrade = "";

                            searchLayoutBinding.advanceSearchLayout.chknew.setChecked(false);
                            searchLayoutBinding.advanceSearchLayout.chkongoing.setChecked(false);
                            searchLayoutBinding.advanceSearchLayout.chkterminated.setChecked(false);
                            searchLayoutBinding.advanceSearchLayout.chkcompleted.setChecked(false);
                            searchLayoutBinding.advanceSearchLayout.spinnerSearchGrade.setSelection(0);
                            callApiSearchDetials(SearchText, getfilterString(), bygrade);
                        } else {
                            SearchText = "";
                        }

                    } else {
                        SearchText = newText;
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                }

                return true;
            }

            @Override
            public boolean onQueryTextSubmit(String query) {
                SearchText = query;
                searchLayoutBinding.rcCourseLayout.rcVertical.setVisibility(View.VISIBLE);
                searchLayoutBinding.rcLessonLayout.rcVertical.setVisibility(View.VISIBLE);
                callApiSearchDetials(SearchText, "", bygrade);
                return true;
            }
        });

        searchLayoutBinding.advanceSearchLayout.spinnerSearchGrade.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parentView, View selectedItemView, int position, long id) {

                if (spinnerGradelist.get(position).getId().equals("0")) {
                    bygrade = "";
                } else {
                    bygrade = spinnerGradelist.get(position).getId();
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {
            }
        });

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {

            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();

                    AllGradeObject.Data allGrade = new AllGradeObject.Data();
                    allGrade.setId("0");
                    allGrade.setName(getString(R.string.none));
                    spinnerGradelist.add(allGrade);

                    CourseObject courseObject = courseDatabaseRepository.getAllCourseObject(userId);
                    if (courseObject != null && courseObject.getData() != null) {
                        for (int i = 0; i < courseObject.getData().size(); i++) {

                            String courseId = courseObject.getData().get(i).getId();
                            String courseName = courseObject.getData().get(i).getName();

                            allGrade = new AllGradeObject.Data();
                            allGrade.setId(courseId);
                            allGrade.setName(courseName);
                            spinnerGradelist.add(allGrade);
                        }
                    }
                    spinneradp = new SpinnerItemAdapter(SearchActivity.this, spinnerGradelist);
                    searchLayoutBinding.advanceSearchLayout.spinnerSearchGrade.setAdapter(spinneradp);
                }
                return null;
            }

        }).execute();
        SlideDown();
        setOnClickListener();
        showBackArrow("");
    }

    public void setOnClickListener() {
        searchLayoutBinding.toolbarLayout.advanceSearchMenu.setOnClickListener(this);
        searchLayoutBinding.advanceSearchLayout.cardsearch.setOnClickListener(this);
        searchLayoutBinding.advanceSearchLayout.cardclear.setOnClickListener(this);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.advanceSearchMenu:
                if (!slideUp.isVisible()) {
                    slideUp.show();
                } else {
                    slideUp.hide();
                }
                break;
            case R.id.cardsearch:
                slideUp.hide();
                callApiSearchDetials(SearchText, getfilterString(), bygrade);
                break;
            case R.id.cardclear:
                slideUp.hide();
                searchLayoutBinding.toolbarLayout.coursesearchview.setQuery("", false);
                SearchText = "";
                bygrade = "";

                searchLayoutBinding.advanceSearchLayout.chknew.setChecked(false);
                searchLayoutBinding.advanceSearchLayout.chkongoing.setChecked(false);
                searchLayoutBinding.advanceSearchLayout.chkterminated.setChecked(false);
                searchLayoutBinding.advanceSearchLayout.chkcompleted.setChecked(false);
                searchLayoutBinding.advanceSearchLayout.spinnerSearchGrade.setSelection(0);
                callApiSearchDetials("", getfilterString(), "");
                break;
        }
    }

    public void SlideDown() {
        slideUp = new SlideUpBuilder(searchLayoutBinding.advanceSearchLayout.mainAdvanceSearchLayout)
                .withStartGravity(Gravity.TOP)
                .withLoggingEnabled(true)
                .withStartState(SlideUp.State.HIDDEN)
                .build();

    }

    public String getfilterString() {
        String strfilter = "";
        strfilter = strfilter + ((searchLayoutBinding.advanceSearchLayout.chknew.isChecked()) ? "1," : "");
        strfilter = strfilter + ((searchLayoutBinding.advanceSearchLayout.chkongoing.isChecked()) ? "2," : "");
        strfilter = strfilter + ((searchLayoutBinding.advanceSearchLayout.chkterminated.isChecked()) ? "3," : "");
        strfilter = strfilter + ((searchLayoutBinding.advanceSearchLayout.chkcompleted.isChecked()) ? "4," : "");
        return strfilter;
    }

    public void callApiSearchDetials(String SearchText, String filter, String bygrade) {

        if (isNetworkAvailable(this)) {
            showDialog(getString(R.string.loading));
            disposable.add(courseRepository.SearchDetails(0, 0, SearchText, filter, bygrade)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<SearchObject>() {
                        @Override
                        public void onSuccess(SearchObject searchObject) {

                            try {

                                listProg.clear();
                                listProglesson.clear();

                                if (searchObject.getData().getCourses() != null) {
                                    Collections.addAll(listProg, searchObject.getData().getCourses());
                                    searchLayoutBinding.rcCourseLayout.rcVertical.setHasFixedSize(true);
                                    searchCoursesAdapter = new SearchCoursesAdapter(SearchActivity.this, listProg, userId);
                                    searchLayoutBinding.rcCourseLayout.rcVertical.setAdapter(searchCoursesAdapter);
                                }

                                if (searchObject.getData().getLessons() != null) {
                                    Collections.addAll(listProglesson, searchObject.getData().getLessons());
                                    searchLayoutBinding.rcLessonLayout.rcVertical.setHasFixedSize(true);
                                    searchLessonAdapter = new SearchLessonAdapter(SearchActivity.this, listProglesson, userId);
                                    searchLayoutBinding.rcLessonLayout.rcVertical.setAdapter(searchLessonAdapter);
                                }
                                getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
                                hideDialog();
                            } catch (Exception e1) {
                                showError(e1);
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            try {
                                HttpException error = (HttpException) e;
                                SearchObject userObject = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), SearchObject.class);
                                showSnackBar(searchLayoutBinding.mainChangePasswordLayout, userObject.getMessage());
                                listProg.clear();
                                listProglesson.clear();
                                searchCoursesAdapter.notifyDataSetChanged();
                                searchLessonAdapter.notifyDataSetChanged();
                                spinneradp.notifyDataSetChanged();
                            } catch (Exception e1) {
                                //showError(e);
                                LocalSearchData(filter, e);
                            }

                            hideDialog();
                        }
                    }));
        } else {
            // get FillesData from DATABASE
            LocalSearchData(filter, null);


        }
    }

    public void LocalSearchData(String filter, Throwable error) {
        try {
            listProg.clear();
            listProglesson.clear();

            searchLayoutBinding.rcCourseLayout.rcVertical.setHasFixedSize(true);
            searchCoursesAdapter = new SearchCoursesAdapter(SearchActivity.this, listProg, userId);
            searchLayoutBinding.rcCourseLayout.rcVertical.setAdapter(searchCoursesAdapter);

            searchLayoutBinding.rcLessonLayout.rcVertical.setHasFixedSize(true);
            searchLessonAdapter = new SearchLessonAdapter(SearchActivity.this, listProglesson, userId);
            searchLayoutBinding.rcLessonLayout.rcVertical.setAdapter(searchLessonAdapter);

            if (!TextUtils.isEmpty(SearchText)) {
                showDialog(getString(R.string.loading));
                /*---  Search Course and lesson  ---*/

                CourseObject courseObject = courseDatabaseRepository.getAllCourseObject(userId);

                //  Log.e("lessonfileName", "DATABASE===courseObject===" + courseObject);

                if (courseObject != null && courseObject.getData() != null) {
                    for (int i = 0; i < courseObject.getData().size(); i++) {
                        for (int j = 0; j < courseObject.getData().get(i).getCourses().size(); j++) {

                            Log.e("lessonfileName", "DATABASE===isDeleted===" + courseObject.getData().get(i).getCourses().get(j).isDeleted());

                            if (!courseObject.getData().get(i).getCourses().get(j).isDeleted()) {
                                /*------   Search Course    ------*/

                                Log.e("lessonfileName", "DATABASE===isDeleted==00=" + courseObject.getData().get(i).getCourses().get(j).isDeleted());

                                String gradeId = courseObject.getData().get(i).getId();
                                String gradeName = courseObject.getData().get(i).getName();
                                String courseId = courseObject.getData().get(i).getCourses().get(j).getId();
                                String courseName = courseObject.getData().get(i).getCourses().get(j).getName();
                                String courseDescription = courseObject.getData().get(i).getCourses().get(j).getDescription();
                                String courseStartDate = courseObject.getData().get(i).getCourses().get(j).getStartdate();
                                String courseEndDate = courseObject.getData().get(i).getCourses().get(j).getEnddate();
                                Date startDate = null;
                                Date endDate = null;
                                Date currantDate = null;

                                SimpleDateFormat dateformat = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
                                try {

                                    currantDate = new Date();
                                    startDate = dateformat.parse(courseStartDate);
                                    endDate = dateformat.parse(courseEndDate);

                                    Log.e(Const.LOG_NOON_TAG, "====ENDDATE==" + dateformat.format(endDate));
                                    Log.e(Const.LOG_NOON_TAG, "====CURRANTDATE==" + dateformat.format(currantDate));
                                    Log.e(Const.LOG_NOON_TAG, "====STARTDATE==" + dateformat.format(startDate));


                                } catch (ParseException e) {
                                    e.printStackTrace();
                                }

                                if (TextUtils.isEmpty(gradeName)) {
                                    gradeName = "";
                                }

                                if (TextUtils.isEmpty(courseName)) {
                                    courseName = "";
                                }

                                if (TextUtils.isEmpty(courseDescription)) {
                                    courseDescription = "";
                                }


                                Log.e("lessonfileName", "DATABASE===bygrade===" + bygrade);
                                Log.e("lessonfileName", "DATABASE===courseName===" + courseName);

                                if (bygrade.equals("0") || bygrade.equals("")) {
                                    if (courseName.toString().trim().contains(SearchText.toString().trim())
                                            || courseDescription.toString().trim().contains(SearchText.toString().trim())
                                    ) {

                                        Log.e("lessonfileName", "DATABASE===if===");

                                        SearchObject.GradeDetails gradeDetails = new SearchObject.GradeDetails();
                                        gradeDetails.setId(gradeId);
                                        gradeDetails.setName(gradeName);

                                        SearchObject.Courses searchCourseObject = new SearchObject.Courses();
                                        searchCourseObject.setId(courseId);
                                        searchCourseObject.setName(courseName);
                                        searchCourseObject.setGradeDetails(new SearchObject.GradeDetails[]{gradeDetails});

                                        byte[] bitmapImage = courseDatabaseRepository.getCourseImage(userId, courseId);
                                        if (bitmapImage != null) {
                                            searchCourseObject.setCourseImage(bitmapImage);
                                        }

                                        int totalTrueLesson = lessonDatabaseRepository.getItemGradeIdProgress(courseId, "100", userId);
                                        String totalLEssonITEm = lessonDatabaseRepository.getLessonStringProgress(courseId, userId);
                                        if (totalLEssonITEm != null) {
                                            int totalCount = Integer.parseInt(totalLEssonITEm);
                                            if (totalCount != 0 && totalTrueLesson != 0) {
                                                int countper = (int) ((totalTrueLesson + 1) * 100 / totalCount);
                                                if (countper != 0) {
                                                    searchCourseObject.setProgressVal(countper);
                                                }
                                            }
                                        }


                                        if (filter.equals("")) {
                                            listProg.add(searchCourseObject);
                                        } else {

                                            List<String> FilterList = Arrays.asList(filter.split(","));
                                            for (int k = 0; k < FilterList.size(); k++) {

                                                if (FilterList.get(k).equals("1")) {
                                                    Log.e("FILTER", "111");
                                                    if (currantDate != null && startDate != null) {
                                                        Date newDate = subtractDays(currantDate, i);
                                                        if (startDate.compareTo(newDate) >= 0) {
                                                            listProg.add(searchCourseObject);
                                                        }
                                                    }
                                                } else if (FilterList.get(k).equals("2")) {
                                                    Log.e("FILTER", "222");
                                                    if (currantDate != null && startDate != null && endDate != null) {
                                                        if (startDate.compareTo(currantDate) >= 0 && endDate.compareTo(currantDate) <= 0) {
                                                            listProg.add(searchCourseObject);
                                                        }
                                                    }
                                                } else if (FilterList.get(k).equals("3")) {
                                                    Log.e("FILTER", "333");
                                                    if (currantDate != null && endDate != null) {
                                                        if (endDate.compareTo(currantDate) <= 0) {
                                                            listProg.add(searchCourseObject);
                                                        }
                                                    }
                                                } else if (FilterList.get(k).equals("4")) {
                                                    Log.e("FILTER", "444");
                                                }
                                            }
                                        }
                                    }
                                } else {
                                    if (courseName.toString().trim().contains(SearchText.toString().trim())
                                            || courseDescription.toString().trim().contains(SearchText.toString().trim())
                                    ) {

                                        if (gradeId.toString().trim().equals(bygrade.toString().trim())) {
                                            SearchObject.GradeDetails gradeDetails = new SearchObject.GradeDetails();
                                            gradeDetails.setId(gradeId);
                                            gradeDetails.setName(gradeName);

                                            SearchObject.Courses searchCourseObject = new SearchObject.Courses();
                                            searchCourseObject.setId(courseId);
                                            searchCourseObject.setName(courseName);
                                            searchCourseObject.setGradeDetails(new SearchObject.GradeDetails[]{gradeDetails});

                                            byte[] bitmapImage = courseDatabaseRepository.getCourseImage(userId, courseId);
                                            if (bitmapImage != null) {
                                                searchCourseObject.setCourseImage(bitmapImage);
                                            }

                                            int totalTrueLesson = lessonDatabaseRepository.getItemGradeIdProgress(courseId, "100", userId);
                                            String totalLEssonITEm = lessonDatabaseRepository.getLessonStringProgress(courseId, userId);
                                            if (totalLEssonITEm != null) {
                                                int totalCount = Integer.parseInt(totalLEssonITEm);
                                                if (totalCount != 0 && totalTrueLesson != 0) {
                                                    int countper = (int) ((totalTrueLesson + 1) * 100 / totalCount);
                                                    if (countper != 0) {
                                                        searchCourseObject.setProgressVal(countper);
                                                    }
                                                }
                                            }


                                            if (filter.equals("")) {
                                                listProg.add(searchCourseObject);
                                            } else {

                                                List<String> FilterList = Arrays.asList(filter.split(","));
                                                for (int k = 0; k < FilterList.size(); k++) {

                                                    if (FilterList.get(k).equals("1")) {
                                                        Log.e("FILTER", "111");
                                                        if (currantDate != null && startDate != null) {
                                                            Date newDate = subtractDays(currantDate, i);
                                                            if (startDate.compareTo(newDate) >= 0) {
                                                                listProg.add(searchCourseObject);
                                                            }
                                                        }
                                                    } else if (FilterList.get(k).equals("2")) {
                                                        Log.e("FILTER", "222");
                                                        if (currantDate != null && startDate != null && endDate != null) {
                                                            if (startDate.compareTo(currantDate) >= 0 && endDate.compareTo(currantDate) <= 0) {
                                                                listProg.add(searchCourseObject);
                                                            }
                                                        }
                                                    } else if (FilterList.get(k).equals("3")) {
                                                        Log.e("FILTER", "333");
                                                        if (currantDate != null && endDate != null) {
                                                            if (endDate.compareTo(currantDate) <= 0) {
                                                                listProg.add(searchCourseObject);
                                                            }
                                                        }
                                                    } else if (FilterList.get(k).equals("4")) {
                                                        Log.e("FILTER", "444");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }


                                CoursePriviewObject coursePriviewObject = courseDatabaseRepository.getAllCourseDetailsById(courseId, userId);
                                if (coursePriviewObject != null) {

                                    for (int k = 0; k < coursePriviewObject.getData().getChapters().size(); k++) {
                                        if (coursePriviewObject.getData().getChapters().get(k).getLessons() != null) {
                                            for (int l = 0; l < coursePriviewObject.getData().getChapters().get(k).getLessons().length; l++) {

                                                String chapterName = coursePriviewObject.getData().getChapters().get(k).getName();
                                                String lessonId = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getId();
                                                String lessonName = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getName();
                                                String lessonDescription = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getDescription();
                                                String fileid = "0", quizID = "0";
                                                String lessonfileName = "";
                                                String lessonfileDescription = "";
                                                String lessonFileTypeid = "";
                                                String lessonFileTypeName = "";
                                                String lessonFileName = "";
                                                int progressVal = 0;
                                                int lessonFilePos = 0;


                                                if (coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles() != null) {
                                                    if (coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles().length == 1) {
                                                        lessonFilePos = 0;
                                                    } else {
                                                        lessonFilePos = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles().length - 1;
                                                    }

                                                    fileid = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[lessonFilePos].getFiles().getId();
                                                    lessonfileName = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[lessonFilePos].getFiles().getName();
                                                    lessonfileDescription = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[lessonFilePos].getFiles().getDescription();
                                                    lessonFileTypeid = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[lessonFilePos].getFiles().getFiletypeid();
                                                    lessonFileTypeName = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[lessonFilePos].getFiles().getFiletypename();
                                                    lessonFileName = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[lessonFilePos].getFiles().getFilename();

                                                    LessonProgress lessonProgress = lessonDatabaseRepository.getItemLessonProgressData(lessonId, fileid, userId);
                                                    if (lessonProgress != null) {
                                                        progressVal = Integer.parseInt(lessonProgress.getLessonProgress());
                                                    }
                                                } else {
                                                    quizID = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getId();
                                                    lessonfileName = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getName();
                                                    lessonfileDescription = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getDescription();

                                                    LessonProgress lessonProgress = lessonDatabaseRepository.getItemQuizIdProgress(quizID, userId);
                                                    if (lessonProgress != null) {
                                                        progressVal = Integer.parseInt(lessonProgress.getLessonProgress());
                                                    }
                                                }


                                                if (TextUtils.isEmpty(chapterName)) {
                                                    chapterName = "";
                                                }

                                                if (TextUtils.isEmpty(lessonName)) {
                                                    lessonName = "";
                                                }

                                                if (TextUtils.isEmpty(lessonDescription)) {
                                                    lessonDescription = "";
                                                }


                                                if (bygrade.equals("0") || bygrade.equals("")) {

                                                    if (lessonName.toString().trim().contains(SearchText.toString().trim())
                                                            || lessonDescription.toString().trim().contains(SearchText.toString().trim())) {

                                                        SearchObject.Lessons searchLessonObject = new SearchObject.Lessons();
                                                        searchLessonObject.setCourseid(courseId);
                                                        searchLessonObject.setChapterid(coursePriviewObject.getData().getChapters().get(k).getId());
                                                        searchLessonObject.setId(lessonId);
                                                        searchLessonObject.setName(lessonName);
                                                        searchLessonObject.setCoursename(courseName);
                                                        searchLessonObject.setProgressval(String.valueOf(progressVal));
                                                        searchLessonObject.setLessonfileid(fileid);
                                                        searchLessonObject.setLessonquizid(quizID);
                                                        searchLessonObject.setFiletypename(lessonFileTypeName);
                                                        searchLessonObject.setLessonfilename(lessonFileName);
                                                        listProglesson.add(searchLessonObject);

                                                    }
                                                } else {
                                                    if (gradeId.toString().trim().equals(bygrade.toString().trim())) {
                                                        if (lessonName.toString().trim().contains(SearchText.toString().trim())
                                                                || lessonDescription.toString().trim().contains(SearchText.toString().trim())) {

                                                            SearchObject.Lessons searchLessonObject = new SearchObject.Lessons();
                                                            searchLessonObject.setCourseid(courseId);
                                                            searchLessonObject.setChapterid(coursePriviewObject.getData().getChapters().get(k).getId());
                                                            searchLessonObject.setId(lessonId);
                                                            searchLessonObject.setName(lessonName);
                                                            searchLessonObject.setCoursename(courseName);
                                                            searchLessonObject.setProgressval(String.valueOf(progressVal));
                                                            searchLessonObject.setLessonfileid(fileid);
                                                            searchLessonObject.setLessonquizid(quizID);
                                                            searchLessonObject.setFiletypename(lessonFileTypeName);
                                                            searchLessonObject.setLessonfilename(lessonFileName);
                                                            listProglesson.add(searchLessonObject);

                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            } else {
                                courseObject.getData().get(i).getCourses().remove(j);
                            }

                        }
                    }
                    searchCoursesAdapter.notifyDataSetChanged();
                    searchLessonAdapter.notifyDataSetChanged();
                    hideDialog();
                } else {
                    showError(error);
                }
            }


        } catch (Exception e1) {
            showError(e1);
            hideDialog();
        }

    }


    public static Date subtractDays(Date date, int days) {
        GregorianCalendar cal = new GregorianCalendar();
        cal.setTime(date);
        cal.add(Calendar.DATE, -days);

        return cal.getTime();
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                onBackPressed();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }
}


