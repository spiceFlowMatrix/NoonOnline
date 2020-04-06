package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;
import android.text.InputFilter;
import android.view.MenuItem;
import android.view.View;
import android.widget.CompoundButton;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.google.gson.reflect.TypeToken;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.assignment.AssignmentData;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.InputFilterMinMax;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.Validator;
import com.ibl.apps.noon.databinding.AssignmentReviewLayoutBinding;

import org.json.JSONException;
import org.json.JSONObject;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;


public class AssignmentReviewActivity extends BaseActivity implements View.OnClickListener {

    AssignmentReviewLayoutBinding assignmentReviewLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    UserDetails userDetailsObject = new UserDetails();
    String userId = "0";
    boolean isapproved = false;
    CoursePriviewObject.Assignment assignment;
    int userSelectedId = 0;
    int flag;
    String remark, score;
    int scoreValue;

    @Override

    protected int getContentView() {
        return R.layout.assignment_review_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        assignmentReviewLayoutBinding = (AssignmentReviewLayoutBinding) getBindObj();

        if (getIntent() != null) {
            flag = getIntent().getIntExtra(Const.Flag, 0);
            assignment = new Gson().fromJson(getIntent().getStringExtra(Const.Assignment), new TypeToken<CoursePriviewObject.Assignment>() {
            }.getType());


            // userSelectedId = String.valueOf(datalist.getId());
        }

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();
                }
                return null;
            }

        }).execute();

        setToolbar(assignmentReviewLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.assignment_review));
        setOnClickListener();
    }

    public void setOnClickListener() {
        assignmentReviewLayoutBinding.assignmentscore.setFilters(new InputFilter[]{new InputFilterMinMax("0", "100")});

        assignmentReviewLayoutBinding.cardComplete.setOnClickListener(this);
        assignmentReviewLayoutBinding.assignmentPassSwitch.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                isapproved = isChecked;
            }
        });
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.cardComplete:

                remark = assignmentReviewLayoutBinding.assignmentReview.getText().toString().trim();
                score = assignmentReviewLayoutBinding.assignmentscore.getText().toString().trim();
                scoreValue = Integer.parseInt(score);

                if (validateFields()) {
                    if (isNetworkAvailable(this)) {
                        userSelectedId = getIntent().getIntExtra("userid", 0);
                        flagPassApiCall();
                    } else {
                        showNetworkAlert(AssignmentReviewActivity.this);
                    }
                }
                break;
        }
    }

    private void CallApiAddTeacherAssignment(String id, int userId, String teacherId, boolean isapproved, int score, String remark) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            //jsonObject.put(Const.id, "0");
            jsonObject.put(Const.assignmentid, id);
            jsonObject.put(Const.userid, userId);
            jsonObject.put(Const.teacherid, teacherId);
            jsonObject.put(Const.isapproved, isapproved);
            jsonObject.put(Const.score, score);
            jsonObject.put(Const.remark, remark);
//            jsonObject.put("isapproved", false);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(apiService.addTeaherTopicSubmission(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse<AssignmentData>>() {
                    @Override
                    public void onSuccess(RestResponse<AssignmentData> assignmentDataRestResponse) {
                        hideDialog();
                        Toast.makeText(getApplicationContext(), assignmentDataRestResponse.getMessage(), Toast.LENGTH_SHORT).show();
                        finish();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {

                            //AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            //Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    private void CallApiAddTeacherAssignmentChapter(String id, int userId, String teacherId, boolean isapproved, int score, String remark) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            //jsonObject.put(Const.id, "0");
            jsonObject.put(Const.assignmentid, id);
            jsonObject.put(Const.userid, userId);
            jsonObject.put(Const.teacherid, teacherId);
            jsonObject.put(Const.isapproved, isapproved);
            jsonObject.put(Const.score, score);
            jsonObject.put(Const.remark, remark);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(apiService.addTeaherTopicSubmissionChapter(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse<AssignmentData>>() {
                    @Override
                    public void onSuccess(RestResponse<AssignmentData> assignmentDataRestResponse) {
                        hideDialog();
                        Toast.makeText(getApplicationContext(), assignmentDataRestResponse.getMessage(), Toast.LENGTH_SHORT).show();
                        finish();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {

                            //AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            //Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }


    public void flagPassApiCall() {
        if (flag == 1) {
            CallApiAddTeacherAssignment(assignment.getId(), userSelectedId, userDetailsObject.getId(), isapproved, scoreValue, remark);
        } else {
            CallApiAddTeacherAssignmentChapter(assignment.getId(), userSelectedId, userDetailsObject.getId(), isapproved, scoreValue, remark);
        }
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        finish();
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


    private boolean validateFields() {
        if (!Validator.checkEmpty(assignmentReviewLayoutBinding.assignmentReview)) {
            hideKeyBoard(assignmentReviewLayoutBinding.assignmentReview);
            assignmentReviewLayoutBinding.assignmentReviewWrapper.setError(getString(R.string.validation_remark));
            return false;
        } else {
            hideKeyBoard(assignmentReviewLayoutBinding.assignmentReview);
            assignmentReviewLayoutBinding.assignmentReviewWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(assignmentReviewLayoutBinding.assignmentscore)) {
            hideKeyBoard(assignmentReviewLayoutBinding.assignmentscore);
            assignmentReviewLayoutBinding.assignmentscoreWrapper.setError(getString(R.string.validation_Score));
            return false;
        } else {
            hideKeyBoard(assignmentReviewLayoutBinding.assignmentscore);
            assignmentReviewLayoutBinding.assignmentscoreWrapper.setErrorEnabled(false);
        }

        return true;
    }


}
