package com.ibl.apps.noon;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.ActivityNotFoundException;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Environment;
import android.text.SpannableStringBuilder;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.webkit.MimeTypeMap;
import android.widget.LinearLayout;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.core.content.FileProvider;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.crashlytics.android.Crashlytics;
import com.downloader.Error;
import com.downloader.OnCancelListener;
import com.downloader.OnDownloadListener;
import com.downloader.OnPauseListener;
import com.downloader.OnProgressListener;
import com.downloader.OnStartOrResumeListener;
import com.downloader.PRDownloader;
import com.downloader.Progress;
import com.github.barteksc.pdfviewer.listener.OnLoadCompleteListener;
import com.github.barteksc.pdfviewer.listener.OnPageChangeListener;
import com.github.barteksc.pdfviewer.listener.OnRenderListener;
import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.google.gson.reflect.TypeToken;
import com.ibl.apps.Adapter.AssignmentCommentListAdapter;
import com.ibl.apps.Adapter.AssignmentFileListAdapter;
import com.ibl.apps.Adapter.StudentListAdapter;
import com.ibl.apps.AssignmentManagement.AssignmentRepository;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.DownloadFileManagement.DownloadFileRepository;
import com.ibl.apps.Interface.EncryptDecryptAsyncResponse;
import com.ibl.apps.Interface.ViewFiles;
import com.ibl.apps.Model.AssignmentDetailObject;
import com.ibl.apps.Model.AssignmentSubmission;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.DiscussionsDetails;
import com.ibl.apps.Model.EncryptDecryptObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.SignedUrlObject;
import com.ibl.apps.Model.assignment.AssignmentData;
import com.ibl.apps.Model.assignment.CommentResponse;
import com.ibl.apps.Model.assignment.FileUploadResponse;
import com.ibl.apps.Model.assignment.StudentDetailData;
import com.ibl.apps.Model.assignment.SubmissionFiles;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.databinding.AssignmentDetailLayoutBinding;
import com.ibl.apps.noon.databinding.AssignmentfilesItemLayoutBinding;
import com.ibl.apps.noon.databinding.DialogViewerItemLayoutBinding;
import com.ibl.apps.noon.databinding.FragmentAssignmentLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.util.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.VideoEncryptDecrypt.EncrypterDecryptAlgo;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;

import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import ir.sohreco.androidfilechooser.ExternalStorageNotAvailableException;
import ir.sohreco.androidfilechooser.FileChooser;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.HttpException;
import retrofit2.Response;

import static com.ibl.apps.noon.AssignmentAddActivity.fileUploadList;

//todo dd assignment detail activity
public class AssignmentDetailActivity extends BaseActivity implements View.OnClickListener, ViewFiles {

    private final static int READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE = 13;
    AssignmentDetailLayoutBinding assignmentDetailLayoutBinding;
    ArrayList<String> fileuploadlist = new ArrayList<>();
    UserDetails userDetailsObject;
    String userId = "0";
    String assignmnetuserId, assignmnetid, comment;
    FileChooser.Builder builder;
    boolean mainClickFlag = false;
    String userRoleName = "";
    List<AssignmentDetailObject.Files> discussionsFile = new ArrayList<>();
    CoursePriviewObject.Assignment assignment;
    byte[] iv;
    AlgorithmParameterSpec paramSpec;
    SecretKey key;
    SecretKey keyFromKeydata;
    byte[] keyData;
    AssignmentCommentListAdapter assignmentCommentListAdapter;
    int pageNumber = 1;
    String perpagerecord = "10";
    boolean isLoad = true;
    StudentDetailData dataObject;
    int flag;
    FragmentAssignmentLayoutBinding fragmentAssignmentLayoutBinding;
    Dialog dialog;
    int studentId;
    String CourseName = "";
    String LessonName = "";
    private CompositeDisposable disposable = new CompositeDisposable();
    private RecyclerViewLoadMoreScroll scrollListener;
    private Context context;
    private DownloadFileRepository downloadFileRepository;
    private AssignmentRepository assignmentRepository;

    static String getMimeType1(@NonNull File file, String selectedFilePath) {
        String type = null;
        int fileType = -1;
        final String url = file.toString();
        final String extension = MimeTypeMap.getFileExtensionFromUrl(url);
        if (extension != null) {
            type = MimeTypeMap.getSingleton().getMimeTypeFromExtension(extension.toLowerCase());
        }

        return type;
    }

    public static void showNoSpaceAlert(Context activity) {

        ((Activity) activity).runOnUiThread(new Runnable() {
            public void run() {
                try {
                    SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.error_no_space));
                    androidx.appcompat.app.AlertDialog.Builder builder = new androidx.appcompat.app.AlertDialog.Builder(activity);
                    builder.setTitle(activity.getResources().getString(R.string.validation_warning));
                    builder.setMessage(message)
                            .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    // Yes button clicked, do something
                                    dialog.dismiss();
                                }
                            });

                    builder.show();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

//    @Override
//    public void onConfigurationChanged(Configuration newConfig) {
//        super.onConfigurationChanged(newConfig);
//        setDynamaticallyOriantation(newConfig.orientation);
//    }

    @Override
    protected int getContentView() {
        return R.layout.assignment_detail_layout;
    }

    @SuppressLint("RestrictedApi")
    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        assignmentDetailLayoutBinding = (AssignmentDetailLayoutBinding) getBindObj();
        context = this;
        assignmentRepository = new AssignmentRepository();
        downloadFileRepository = new DownloadFileRepository();
        if (getIntent() != null) {
            CourseName = getIntent().getStringExtra("coursename");
            LessonName = getIntent().getStringExtra("lessonname");
            flag = getIntent().getIntExtra(Const.Flag, 0);
            assignment = new Gson().fromJson(getIntent().getStringExtra(Const.Assignment), new TypeToken<CoursePriviewObject.Assignment>() {
            }.getType());

            try {
                key = new SecretKeySpec(Const.ALGO_SECRATE_KEY_NAME.getBytes(), Const.ALGO_SECRET_KEY_GENERATOR);
                keyData = key.getEncoded();
                keyFromKeydata = new SecretKeySpec(keyData, 0, keyData.length, Const.ALGO_SECRET_KEY_GENERATOR); //if you want to store key bytes to db so its just how to //recreate back key from bytes array
                iv = new byte[Const.IV_LENGTH];
                paramSpec = new IvParameterSpec(iv);

            } catch (Exception e) {
                e.printStackTrace();
            }

            new PrefUtils.MyAsyncTask(userDetails -> {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();
                    if (userDetails.getRoleName() != null) {
                        userRoleName = userDetails.getRoleName().get(0);
                        if (!TextUtils.isEmpty(userRoleName)) {
                            if (userRoleName.equals(Const.TeacherKEY)) {
                                assignmentDetailLayoutBinding.studentlay.setVisibility(View.VISIBLE);
                                assignmentDetailLayoutBinding.fabAssignmentReview.setVisibility(View.VISIBLE);
                            } else {
                                flagPassApiCallComments();
                                assignmentDetailLayoutBinding.studentlay.setVisibility(View.GONE);
                                assignmentDetailLayoutBinding.fabAssignmentReview.setVisibility(View.VISIBLE);
                            }
                        }
                    }
                }
                return null;
            }).execute();

        }

        if (isNetworkAvailable(getApplicationContext())) {
            flagPassApiCallComments();

            if (!TextUtils.isEmpty(assignment.getName())) {
                assignmentDetailLayoutBinding.assignmentName.setText(assignment.getName());
            }

            if (!TextUtils.isEmpty(assignment.getDescription())) {
                assignmentDetailLayoutBinding.assignmentDescription.setText(assignment.getDescription());
            }
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(AssignmentDetailActivity.this);
            linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
            assignmentDetailLayoutBinding.rcVerticalLay.rcVertical.setLayoutManager(new LinearLayoutManager(AssignmentDetailActivity.this, LinearLayoutManager.HORIZONTAL, false));
            AssignmentFileListAdapter adp = new AssignmentFileListAdapter(AssignmentDetailActivity.this, assignment.getAssignmentfiles(), userDetailsObject, CourseName, LessonName);
            assignmentDetailLayoutBinding.rcVerticalLay.rcVertical.setAdapter(adp);
            scrollListener = new RecyclerViewLoadMoreScroll(linearLayoutManager);
            scrollListener.setOnLoadMoreListener(() -> {
                if (isLoad) {
                    assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                    flagPassApiCallComments();
                } else {
                    assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                }
            });


            adp.notifyDataSetChanged();
            assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
            ArrayList<CoursePriviewObject.Assignmentfiles> assignmentfiles = assignment.getAssignmentfiles();
            for (int i = 0; i < assignment.getAssignmentfiles().size(); i++) {
                callApifetchSignedUrl(assignment.getAssignmentfiles().get(i).getFiles().getId(), "0", assignment.getAssignmentfiles().get(i), assignmentfiles);
            }
            //CallApiAssignmentList(AssignmentId);
        } else {
            showNetworkAlert(AssignmentDetailActivity.this);
        }

        setToolbar(assignmentDetailLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.item_2));
        setOnClickListener();
        int orientation = getResources().getConfiguration().orientation;
        //setDynamaticallyOriantation(orientation);
    }

//    @Override
//    protected void onResume() {
//        super.onResume();
//        flagPassApiCallComments();
//    }

    public void setDynamaticallyOriantation(int orientation) {
        if (orientation == Configuration.ORIENTATION_LANDSCAPE) {
            assignmentDetailLayoutBinding.mainLay.setOrientation(LinearLayout.HORIZONTAL);

            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.MATCH_PARENT, .5f);
            assignmentDetailLayoutBinding.Lay.setLayoutParams(params);

            LinearLayout.LayoutParams params1 = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.MATCH_PARENT, .5f);
            assignmentDetailLayoutBinding.commentLay.setLayoutParams(params1);

        } else if (orientation == Configuration.ORIENTATION_PORTRAIT) {
            assignmentDetailLayoutBinding.mainLay.setOrientation(LinearLayout.VERTICAL);

            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT);
            assignmentDetailLayoutBinding.Lay.setLayoutParams(params);

            LinearLayout.LayoutParams params1 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT);
            assignmentDetailLayoutBinding.commentLay.setLayoutParams(params1);
        }
    }

    @SuppressLint("ClickableViewAccessibility")
    public void setOnClickListener() {
        assignmentDetailLayoutBinding.rcVerticalLayoutComment.rcVertical.setHasFixedSize(true);
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);
        linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
        assignmentDetailLayoutBinding.rcVerticalLayoutComment.rcVertical.setLayoutManager(linearLayoutManager);
        assignmentCommentListAdapter = new AssignmentCommentListAdapter(getApplicationContext(), userDetailsObject, this);
        assignmentDetailLayoutBinding.rcVerticalLayoutComment.rcVertical.setAdapter(assignmentCommentListAdapter);
        assignmentDetailLayoutBinding.fabAssignmentReview.setOnClickListener(this);
        assignmentDetailLayoutBinding.cardCommentpicker.setOnClickListener(this);
        assignmentDetailLayoutBinding.studentlay.setOnClickListener(this);

        assignmentDetailLayoutBinding.txtbox.setOnTouchListener((v, event) -> {
            final int DRAWABLE_LEFT = 0;
            final int DRAWABLE_TOP = 1;
            final int DRAWABLE_RIGHT = 2;
            final int DRAWABLE_BOTTOM = 3;

            if (event.getAction() == MotionEvent.ACTION_UP) {
                if (event.getRawX() >= (assignmentDetailLayoutBinding.txtbox.getRight() - assignmentDetailLayoutBinding.txtbox.getCompoundDrawables()[DRAWABLE_RIGHT].getBounds().width())) {
                    // your action here
                    assignmnetid = assignment.getId();
                    assignmnetuserId = userDetailsObject.getId();
                    comment = assignmentDetailLayoutBinding.txtbox.getText().toString().trim();
                    if (!TextUtils.isEmpty(comment)) {
                        falgAddCommentApiCall();
                    } else {
                        Toast.makeText(getApplicationContext(), getString(R.string.validation_enter_comment), Toast.LENGTH_LONG).show();
                    }
                    return true;
                }
            }
            return false;
        });
    }

    public void openStudentListDialog(String assignmnetid) {
        dialog = new Dialog(this, android.R.style.Theme_Black_NoTitleBar_Fullscreen);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
        dialog.setCancelable(true);
        fragmentAssignmentLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(this), R.layout.fragment_assignment_layout, null, false);
        dialog.setContentView(fragmentAssignmentLayoutBinding.getRoot());
        dialog.show();

        setToolbar(fragmentAssignmentLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.item_2));
        fragmentAssignmentLayoutBinding.toolbarLayout.toolBar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                dialog.dismiss();
            }
        });
        if (flag == 1) {
            disposable.add(assignmentRepository.assignmentStudentDetail(assignmnetid)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<RestResponse<ArrayList<StudentDetailData>>>() {
                        @Override
                        public void onSuccess(RestResponse<ArrayList<StudentDetailData>> commentResponseRestResponse) {
                            fragmentAssignmentLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                            StudentListAdapter adp = new StudentListAdapter(AssignmentDetailActivity.this, commentResponseRestResponse.getData(), userDetailsObject, new StudentListAdapter.OnItemClicked() {
                                @Override
                                public void onItemClick(int position) {
                                    dataObject = commentResponseRestResponse.getData().get(position);
                                    if (!TextUtils.isEmpty(String.valueOf(dataObject.getName()))) {
                                        assignmentDetailLayoutBinding.txtStudentName.setText(dataObject.getName());
                                    } else {
                                        assignmentDetailLayoutBinding.txtStudentName.setText(getString(R.string.user));
                                    }
                                    studentId = dataObject.getId();
//                                    SharedPreferences sharedPreferences = getSharedPreferences("studentid", MODE_PRIVATE);
//                                    SharedPreferences.Editor editor = sharedPreferences.edit();
//                                s    editor.putInt("sid", studentId);
//                                    editor.apply();
                                    pageNumber = 1;
                                    flagPassApiCallCommentsTeacher(dataObject.getId());
                                    dialog.dismiss();
                                }
                            });
                            fragmentAssignmentLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
                            adp.notifyDataSetChanged();

                        }

                        @Override
                        public void onError(Throwable e) {
                            Log.e("onError", "----" + e.getMessage());
                        }
                    }));
        } else {
            disposable.add(assignmentRepository.assignmentStudentDetailChapter(assignmnetid)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<RestResponse<ArrayList<StudentDetailData>>>() {
                        @Override
                        public void onSuccess(RestResponse<ArrayList<StudentDetailData>> commentResponseRestResponse) {
                            fragmentAssignmentLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                            StudentListAdapter adp = new StudentListAdapter(AssignmentDetailActivity.this, commentResponseRestResponse.getData(), userDetailsObject, new StudentListAdapter.OnItemClicked() {
                                @Override
                                public void onItemClick(int position) {
                                    dataObject = commentResponseRestResponse.getData().get(position);
                                    if (!TextUtils.isEmpty(String.valueOf(dataObject.getName()))) {
                                        assignmentDetailLayoutBinding.txtStudentName.setText(dataObject.getName());
                                    } else {
                                        assignmentDetailLayoutBinding.txtStudentName.setText(getString(R.string.user));
                                    }
                                    pageNumber = 1;
                                    studentId = dataObject.getId();
                                    flagPassApiCallCommentsTeacher(dataObject.getId());
                                    dialog.dismiss();
                                }
                            });
                            fragmentAssignmentLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
                            adp.notifyDataSetChanged();
                        }

                        @Override
                        public void onError(Throwable e) {
                            Log.e("onError", "----" + e.getMessage());
                        }
                    }));
        }
//        AssignmentObject.FillesData dataObject = new AssignmentObject.FillesData();
//        dataObject.setSubmissioncount("5");
//        dataObject.setName("HAMZA RahiMy");
//        datalist.add(dataObject);
//
//        dataObject = new AssignmentObject.FillesData();
//        dataObject.setSubmissioncount("6");
//        dataObject.setName("JOHN Parker");
//        datalist.add(dataObject);
//
//        dataObject = new AssignmentObject.FillesData();
//        dataObject.setSubmissioncount("56");
//        dataObject.setName("Crane Power");
//        datalist.add(dataObject);
//
//        dataObject = new AssignmentObject.FillesData();
//        dataObject.setSubmissioncount("7");
//        dataObject.setName("Austine Power");
//        datalist.add(dataObject);
//
//        dataObject = new AssignmentObject.FillesData();
//        dataObject.setSubmissioncount("12");
//        dataObject.setName("Bruce Wayne");
//        datalist.add(dataObject);
//
//        AssignmentObject assignmentObject = new AssignmentObject();
//        assignmentObject.setData(datalist);

    }

    private void CallApiCommentAssignment(String assignmentid, String userid, String comment) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.assignmentid, assignmentid);
            jsonObject.put(Const.userid, userid);
            jsonObject.put(Const.comment, comment);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(assignmentRepository.assignmentAddComment(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AssignmentSubmission>() {
                    @Override
                    public void onSuccess(AssignmentSubmission assignmentaddComment) {
                        Toast.makeText(getApplicationContext(), assignmentaddComment.getMessage(), Toast.LENGTH_SHORT).show();
                        assignmentDetailLayoutBinding.txtbox.setText("");
                        pageNumber = 1;
                        flagPassApiCallComments();
                        assignmentDetailLayoutBinding.rcVerticalLayoutComment.rcVertical.scrollToPosition(0);
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            //AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            //Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    private void CallApiCommentAssignmentChapter(String assignmentid, String userid, String comment) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.assignmentid, assignmentid);
            jsonObject.put(Const.userid, userid);
            jsonObject.put(Const.comment, comment);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(assignmentRepository.assignmentAddCommentChapter(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AssignmentSubmission>() {
                    @Override
                    public void onSuccess(AssignmentSubmission assignmentaddComment) {
                        Toast.makeText(getApplicationContext(), assignmentaddComment.getMessage(), Toast.LENGTH_SHORT).show();
                        assignmentDetailLayoutBinding.txtbox.setText("");
                        pageNumber = 1;
                        flagPassApiCallComments();
                        assignmentDetailLayoutBinding.rcVerticalLayoutComment.rcVertical.scrollToPosition(0);
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            //AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            //Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    private void CallApiCommentAssignment(String assignmentid, String userid, String comment, String teacherId) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.assignmentid, assignmentid);
            jsonObject.put(Const.userid, userid);
            jsonObject.put(Const.comment, comment);
            jsonObject.put(Const.teacherid, teacherId);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(assignmentRepository.assignmentAddComment(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AssignmentSubmission>() {
                    @Override
                    public void onSuccess(AssignmentSubmission assignmentaddComment) {
                        Toast.makeText(getApplicationContext(), assignmentaddComment.getMessage(), Toast.LENGTH_SHORT).show();
                        assignmentDetailLayoutBinding.txtbox.setText("");
                        pageNumber = 1;
                        if (userRoleName.equals(Const.TeacherKEY)) {
                            flagPassApiCallCommentsTeacher(studentId);
                        } else {
                            flagPassApiCallComments();
                        }

                        assignmentDetailLayoutBinding.rcVerticalLayoutComment.rcVertical.scrollToPosition(0);
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            //AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            //Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    private void CallApiCommentAssignmentChapter(String assignmentid, String userid, String comment, String teacherId) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.assignmentid, assignmentid);
            jsonObject.put(Const.userid, userid);
            jsonObject.put(Const.comment, comment);
            jsonObject.put(Const.teacherid, teacherId);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(assignmentRepository.assignmentAddCommentChapter(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AssignmentSubmission>() {
                    @Override
                    public void onSuccess(AssignmentSubmission assignmentaddComment) {
                        Toast.makeText(getApplicationContext(), assignmentaddComment.getMessage(), Toast.LENGTH_SHORT).show();
                        assignmentDetailLayoutBinding.txtbox.setText("");
                        pageNumber = 1;
                        flagPassApiCallComments(userid);
                        assignmentDetailLayoutBinding.rcVerticalLayoutComment.rcVertical.scrollToPosition(0);
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            // HttpException error = (HttpException) e;
                            //AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            //Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    public void falgAddCommentApiCall() {
        if (flag == 1) {
            if (!TextUtils.isEmpty(userRoleName)) {
                if (userRoleName.equals(Const.TeacherKEY)) {
                    if (Validate()) {
                        CallApiCommentAssignment(assignmnetid, String.valueOf(dataObject.getId()), comment, assignmnetuserId);
                    }
                } else {
                    CallApiCommentAssignment(assignmnetid, assignmnetuserId, comment);
                }
            }
        } else {
            if (!TextUtils.isEmpty(userRoleName)) {
                if (userRoleName.equals(Const.TeacherKEY)) {
                    if (Validate()) {
                        CallApiCommentAssignmentChapter(assignmnetid, String.valueOf(dataObject.getId()), comment, assignmnetuserId);
                    }
                } else {
                    CallApiCommentAssignmentChapter(assignmnetid, assignmnetuserId, comment);
                }
            }
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        SharedPreferences sharedPreferences = getSharedPreferences("studentid", MODE_PRIVATE);
        if (requestCode == 200) {
            if (userRoleName.equals(Const.TeacherKEY)) {
                flagPassApiCallCommentsTeacher(studentId);
            } else {
                flagPassApiCallComments();
            }
        }
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.fabAssignmentReview:
                if (!TextUtils.isEmpty(userRoleName)) {
                    if (userRoleName.equals(Const.TeacherKEY)) {
                        if (Validate()) {
                            if (dataObject != null) {
                                Intent i = new Intent(getApplicationContext(), AssignmentReviewActivity.class);
                                i.putExtra(Const.Assignment, new Gson().toJson(assignment));
                                i.putExtra(Const.userid, dataObject.getId());
                                i.putExtra(Const.Flag, flag);
                                startActivityForResult(i, 200);
                            }
                        } else {
                            Toast.makeText(this, getString(R.string.please_select_student), Toast.LENGTH_SHORT).show();
                        }
                    } else {
                        Intent intent = new Intent(getApplicationContext(), AssignmentAddActivity.class);
                        intent.putExtra(Const.Assignment, new Gson().toJson(assignment));
                        intent.putExtra(Const.Flag, flag);
                        startActivityForResult(intent, 200);
                    }
                }

                break;
            case R.id.cardCommentpicker:
                if (!TextUtils.isEmpty(userRoleName)) {
                    if (userRoleName.equals(Const.TeacherKEY)) {
                        if (Validate()) {
                            if (dataObject != null) {
                                fileuploadlist.clear();
                                int permissionCheck = ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE);
                                if (permissionCheck != PackageManager.PERMISSION_GRANTED) {
                                    ActivityCompat.requestPermissions(this,
                                            new String[]{Manifest.permission.READ_EXTERNAL_STORAGE},
                                            READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE);
                                } else {
                                    addFileChooserFragment(String.valueOf(dataObject.getId()));
                                }
                            } else {
                                Toast.makeText(this, getString(R.string.please_select_student), Toast.LENGTH_SHORT).show();
                            }
                        }
                    } else {
                        fileuploadlist.clear();
                        int permissionCheck = ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE);
                        if (permissionCheck != PackageManager.PERMISSION_GRANTED) {
                            ActivityCompat.requestPermissions(this,
                                    new String[]{Manifest.permission.READ_EXTERNAL_STORAGE},
                                    READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE);
                        } else {
                            addFileChooserFragment(userId);
                        }
                    }
                }
                break;
            case R.id.studentlay:
                openStudentListDialog(assignment.getId());
                break;
        }
    }

    private void addFileChooserFragmentBool(boolean clickflag) {

        assignmentDetailLayoutBinding.filechooserFragment.setVisibility(View.VISIBLE);

        builder = new FileChooser.Builder(FileChooser.ChooserType.FILE_CHOOSER,
                new FileChooser.ChooserListener() {
                    @Override
                    public void onSelect(String path) {
                        assignmentDetailLayoutBinding.filechooserFragment.setVisibility(View.GONE);
                        if (!TextUtils.isEmpty(path)) {
                            String[] selectedFilePaths = path.split(FileChooser.FILE_NAMES_SEPARATOR);

                            if (clickflag) {
                                mainClickFlag = true;
                                fileuploadlist.clear();
                                fileuploadlist.addAll(Arrays.asList(selectedFilePaths));

                                for (int i = 0; i < fileuploadlist.size(); i++) {
                                    AssignmentDetailObject.Files assignmentFileModel = new AssignmentDetailObject.Files();
                                    assignmentFileModel.setFilename(fileuploadlist.get(i));
                                    discussionsFile.add(assignmentFileModel);
                                }
                            } else {
                                mainClickFlag = false;
                                ArrayList<String> fileuploadlist = new ArrayList<>();
                                fileuploadlist.addAll(Arrays.asList(selectedFilePaths));
                            }

                        } else {
                            Toast.makeText(AssignmentDetailActivity.this, R.string.no_file_chosen, Toast.LENGTH_SHORT).show();
                        }
                    }
                })//.setMultipleFileSelectionEnabled(true)
                // Word document,PDF file,Powerpoint file,Excel file,GIF file,JPG file ,PNG file,Text file ,Video files
                .setFileFormats(new String[]{".jpg", ".png", ".jpeg", ".gif", ".3gp", ".mpg", ".mpeg", ".mpe", ".mp4", ".avi", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".docx", ".doc"});

        try {
            getSupportFragmentManager().beginTransaction().replace(R.id.filechooserFragment, builder.build()).commit();
        } catch (ExternalStorageNotAvailableException e) {
            Toast.makeText(this, R.string.no_external_storage, Toast.LENGTH_SHORT).show();
            e.printStackTrace();
        }
    }

    private void CallApiGetAssignmentComments(String assignmentId, String id) {

        disposable.add(assignmentRepository.getAssignmentComments(String.valueOf(pageNumber), perpagerecord, assignmentId, id)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse<CommentResponse>>() {
                    @SuppressLint("RestrictedApi")
                    @Override
                    public void onSuccess(RestResponse<CommentResponse> assignmentData) {

//                        pageNumber++;
                        pageNumber = 1;
                        assignmentCommentListAdapter.setCards(assignmentData.getData().getDetails());
                        assignmentCommentListAdapter.notifyDataSetChanged();
//                        if (assignmentData.getData() == 0) {
//                            isLoad = false;
//                        }
                        scrollListener.setLoaded();
                        hideDialog();


                        assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        if (assignmentData.getData().isIsapproved()) {
                            assignmentDetailLayoutBinding.fabAssignmentChecked.setVisibility(View.VISIBLE);
                            assignmentDetailLayoutBinding.fabAssignmentReview.setVisibility(View.GONE);
                        } else {
                            assignmentDetailLayoutBinding.fabAssignmentChecked.setVisibility(View.GONE);
                            assignmentDetailLayoutBinding.fabAssignmentReview.setVisibility(View.VISIBLE);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            DiscussionsDetails discussionsDetails = new Gson().fromJson(error.response().errorBody().string(), DiscussionsDetails.class);
                            Toast.makeText(getApplicationContext(), discussionsDetails.getMessage(), Toast.LENGTH_SHORT).show();

                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    private void CallApiGetAssignmentCommentsChapter(String assignmentId, String id) {

        disposable.add(assignmentRepository.getAssignmentCommentsChapter(String.valueOf(pageNumber), perpagerecord, assignmentId, id)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse<CommentResponse>>() {
                    @SuppressLint("RestrictedApi")
                    @Override
                    public void onSuccess(RestResponse<CommentResponse> assignmentData) {

//                        pageNumber++;
                        pageNumber = 1;
                        assignmentCommentListAdapter.setCards(assignmentData.getData().getDetails());
                        assignmentCommentListAdapter.notifyDataSetChanged();
//                        if (assignmentData.getData() == 0) {
//                            isLoad = false;
//                        }
                        scrollListener.setLoaded();
                        hideDialog();


                        assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        if (assignmentData.getData().isIsapproved()) {
                            assignmentDetailLayoutBinding.fabAssignmentChecked.setVisibility(View.VISIBLE);
                            assignmentDetailLayoutBinding.fabAssignmentReview.setVisibility(View.GONE);
                        } else {
                            assignmentDetailLayoutBinding.fabAssignmentChecked.setVisibility(View.GONE);
                            assignmentDetailLayoutBinding.fabAssignmentReview.setVisibility(View.VISIBLE);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            DiscussionsDetails discussionsDetails = new Gson().fromJson(error.response().errorBody().string(), DiscussionsDetails.class);
                            Toast.makeText(getApplicationContext(), discussionsDetails.getMessage(), Toast.LENGTH_SHORT).show();

                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        finish();
    }

    public void flagPassApiCallComments() {
        if (flag == 1) {
            //For Lesson
            if (userDetailsObject != null && !userDetailsObject.getId().equals(""))
                CallApiGetAssignmentComments(assignment.getId(), userDetailsObject.getId());
        } else {
            //for Chapter
            if (userDetailsObject != null && !userDetailsObject.getId().equals(""))
                CallApiGetAssignmentCommentsChapter(assignment.getId(), userDetailsObject.getId());
        }
    }

    public void flagPassApiCallComments(String teacherId) {
        if (flag == 1) {
            CallApiGetAssignmentComments(assignment.getId(), userDetailsObject.getId());
        } else {
            CallApiGetAssignmentCommentsChapter(assignment.getId(), teacherId);
        }
    }

    public void flagPassApiCallCommentsTeacher(int id) {
        if (flag == 1) {
            CallApiGetAssignmentComments(assignment.getId(), String.valueOf(id));
        } else {
            CallApiGetAssignmentCommentsChapter(assignment.getId(), String.valueOf(id));
        }
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

    public boolean Validate() {
        if (dataObject != null && dataObject.getId() != 0) {
            return true;
        }
        Toast.makeText(this, getString(R.string.please_select_student), Toast.LENGTH_SHORT).show();
        return false;
    }

    private void callApifetchAssignmentSignedUrl(int fileID, String lessionID, SubmissionFiles model, Dialog dialog, AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding, String extension) {
        switch (model.getFiles().getFiletypeid()) {
            case 1:
                disposable.add(downloadFileRepository.fetchSignedUrl(String.valueOf(fileID), lessionID)
                        .subscribeOn(Schedulers.io())
                        .observeOn(AndroidSchedulers.mainThread())
                        .subscribeWith(new DisposableSingleObserver<SignedUrlObject>() {
                            @Override
                            public void onSuccess(SignedUrlObject signedUrlObject) {
                                new DownloadAssignmentStatusTask(context,
                                        signedUrlObject.getData().getUrl(),
                                        model, dialog, assignmentfilesItemLayoutBinding, 1, extension).execute();
                            }

                            @Override
                            public void onError(Throwable e) {

                                try {
                                    HttpException error = (HttpException) e;
                                    SignedUrlObject signedUrlObject = new Gson().fromJson(error.response().errorBody().string(), SignedUrlObject.class);
                                    // Log.e(Const.LOG_NOON_TAG, "==SignedUrl==" + signedUrlObject.getMessage());
                                    Toast.makeText(context, signedUrlObject.getMessage(), Toast.LENGTH_SHORT).show();
                                } catch (Exception e1) {
                                    Toast.makeText(context, e1.getMessage(), Toast.LENGTH_SHORT).show();
                                    //showError(e);
                                }
                                //hideDialog();
                                Crashlytics.log(Log.ERROR, context.getString(R.string.app_name), e.getMessage());
                            }
                        }));
                break;
            case 3:
                new DownloadAssignmentStatusTask(context,
                        model.getFiles().getUrl(),
                        model, dialog, assignmentfilesItemLayoutBinding, 3, extension).execute();
                break;
            case 6:
            case 7:
            case 8:
                disposable.add(downloadFileRepository.fetchSignedUrl(String.valueOf(fileID), lessionID)
                        .subscribeOn(Schedulers.io())
                        .observeOn(AndroidSchedulers.mainThread())
                        .subscribeWith(new DisposableSingleObserver<SignedUrlObject>() {
                            @Override
                            public void onSuccess(SignedUrlObject signedUrlObject) {
                                new DownloadAssignmentStatusTask(context,
                                        signedUrlObject.getData().getUrl(),
                                        model, dialog, assignmentfilesItemLayoutBinding, 8, extension).execute();
                            }

                            @Override
                            public void onError(Throwable e) {

                                try {
                                    HttpException error = (HttpException) e;
                                    SignedUrlObject signedUrlObject = new Gson().fromJson(error.response().errorBody().string(), SignedUrlObject.class);
                                    // Log.e(Const.LOG_NOON_TAG, "==SignedUrl==" + signedUrlObject.getMessage());
                                    Toast.makeText(context, signedUrlObject.getMessage(), Toast.LENGTH_SHORT).show();
                                } catch (Exception e1) {
                                    Toast.makeText(context, e1.getMessage(), Toast.LENGTH_SHORT).show();
                                    //showError(e);
                                }
                                //hideDialog();
                                Crashlytics.log(Log.ERROR, context.getString(R.string.app_name), e.getMessage());
                            }
                        }));
                break;
        }
    }

    public void OpenAnyFile(File file, int filetypeid, SubmissionFiles model) {
        MimeTypeMap myMime = MimeTypeMap.getSingleton();
        Intent newIntent = new Intent(Intent.ACTION_VIEW);
        String mimeType = getMimeType1(file, String.valueOf(Uri.fromFile(file)));
        Uri uri = Uri.fromFile(file);

        Uri apkURI = FileProvider.getUriForFile(
                context,
                context.getApplicationContext()
                        .getPackageName() + ".provider", file);
        newIntent.setDataAndType(apkURI, mimeType);
        newIntent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
       /* newIntent.setDataAndType(uri,mimeType);
        newIntent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);*/
        try {
            context.startActivity(newIntent);
        } catch (ActivityNotFoundException e) {
            Toast.makeText(context, "No handler for this type of file.", Toast.LENGTH_LONG).show();
        }
    }

    private String fileExt(String url) {
        if (url.contains("?")) {
            url = url.substring(0, url.indexOf("?"));
        }
        if (url.lastIndexOf(".") == -1) {
            return null;
        } else {
            String ext = url.substring(url.lastIndexOf(".") + 1);
            if (ext.contains("%")) {
                ext = ext.substring(0, ext.indexOf("%"));
            }
            if (ext.contains("/")) {
                ext = ext.substring(0, ext.indexOf("/"));
            }
            return ext.toLowerCase();

        }
    }

    private void openFile(String decryptpath, String filenamedoc) {
        String yourFilePath = context.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath();
        PRDownloader.download(decryptpath, String.valueOf(Environment.getExternalStorageDirectory()), filenamedoc).build().start(new OnDownloadListener() {
            @Override
            public void onDownloadComplete() {
                Intent newIntent = new Intent(Intent.ACTION_VIEW);
                String path = Environment.getExternalStorageDirectory() + filenamedoc;
                newIntent.setDataAndType(Uri.parse(path), "*/*");
                newIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                startActivity(newIntent);
            }

            @Override
            public void onError(Error error) {

            }
        });
    }

    private void openPdf(DialogViewerItemLayoutBinding dialogViewerItemLayoutBinding, Dialog dialog, SubmissionFiles model) {

        String yourFilePath = context.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath();
        String EncryptFileName = model.getFiles().getFilename();
        String filetype = model.getFiles().getFiletypename();
        String str = model.getFiles().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
        String selectedVideoPath = Const.destPath + str;
        File outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
        String encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str;
        String decryptpath = null;
        try {
            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (NoSuchPaddingException e) {
            e.printStackTrace();
        } catch (InvalidKeyException e) {
            e.printStackTrace();
        } catch (InvalidAlgorithmParameterException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {
            PRDownloader.download(decryptpath, yourFilePath, Const.dir_fileName + Const.PDFextension).build().start(new OnDownloadListener() {
                @Override
                public void onDownloadComplete() {
                    dialog.show();
                    File file = new File(context.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath() + File.separator + Const.dir_fileName + Const.PDFextension);
                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fromFile(file)
                            .enableSwipe(true)
                            .enableDoubletap(true)
                            .defaultPage(0)
                            .onLoad(new OnLoadCompleteListener() {
                                @Override
                                public void loadComplete(int i) {
                                    dialogViewerItemLayoutBinding.pdfViewLayout.progressDialogLay.placeholder.setVisibility(View.GONE);
                                }
                            })
                            .onPageChange(new OnPageChangeListener() {
                                @Override
                                public void onPageChanged(int page, int pageCount) {
                                    dialogViewerItemLayoutBinding.pdfViewLayout.txtPageCount.setText(context.getString(R.string.page) + " " + (page + 1) + " " + context.getString(R.string.of) + "  " + pageCount);
                                    int countper = (int) ((page + 1) * 100 / pageCount);
                                    dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setProgress(countper);
                                }
                            })
                            .enableAnnotationRendering(true)
                            .swipeHorizontal(true)
                            .onRender(new OnRenderListener() {
                                @Override
                                public void onInitiallyRendered(int i) {
                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fitToWidth(dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.getCurrentPage());
                                }
                            })
                            .enableAntialiasing(true)
                            .password(null)
                            .load();
                }

                @Override
                public void onError(Error error) {
                }
            });
        } catch (Exception e) {
            e.printStackTrace();

        }

        dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialog.dismiss();
            }
        });

    }

    public void callApifetchSignedUrl(String fileID, String
            lessionID, CoursePriviewObject.Assignmentfiles model, ArrayList<CoursePriviewObject.Assignmentfiles> assignmentfiles) {


        disposable.add(downloadFileRepository.fetchSignedUrl(fileID, lessionID)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<SignedUrlObject>() {
                    @Override
                    public void onSuccess(SignedUrlObject signedUrlObject) {
                        new DownloadStatusTask(AssignmentDetailActivity.this,
                                signedUrlObject.getData().getUrl(),
                                model).execute();
                    }

                    @Override
                    public void onError(Throwable e) {

                        try {
                            HttpException error = (HttpException) e;
                            SignedUrlObject signedUrlObject = new Gson().fromJson(error.response().errorBody().string(), SignedUrlObject.class);
                            // Log.e(Const.LOG_NOON_TAG, "==SignedUrl==" + signedUrlObject.getMessage());
                            Toast.makeText(AssignmentDetailActivity.this, signedUrlObject.getMessage(), Toast.LENGTH_SHORT).show();
                        } catch (Exception e1) {
                            //Log.e(Const.LOG_NOON_TAG, "==SignedUrl==" + e.getMessage());

                            Toast.makeText(AssignmentDetailActivity.this, e1.getMessage(), Toast.LENGTH_SHORT).show();
                            //showError(e);
                        }
                        //hideDialog();
                        Crashlytics.log(Log.ERROR, AssignmentDetailActivity.this.getString(R.string.app_name), e.getMessage());
                    }
                }));
    }

    @Override
    public void openDialogView(SubmissionFiles submissionFiles, int filetypeid) {
        final Dialog dialog = new Dialog(AssignmentDetailActivity.this, android.R.style.Theme_Translucent_NoTitleBar);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        //dialogimage.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
        dialog.setCancelable(true);
        AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(AssignmentDetailActivity.this), R.layout.assignmentfiles_item_layout, null, false);
        dialog.setContentView(assignmentfilesItemLayoutBinding.getRoot());
        String filename = submissionFiles.getFiles().getFilename();
        String[] strings = filename.split("\\.");
        String extension = strings[1];

        switch (filetypeid) {
            case 1:
                //PDf
                callApifetchAssignmentSignedUrl(Integer.parseInt(String.valueOf(submissionFiles.getFiles().getId())), "0", submissionFiles, dialog, assignmentfilesItemLayoutBinding, extension);
                break;
            case 2:
                //Video
                break;
            case 3:
                //Image
                callApifetchAssignmentSignedUrl(Integer.parseInt(submissionFiles.getId()), "0", submissionFiles, dialog, assignmentfilesItemLayoutBinding, extension);
                //openImage(dialogimage,submissionFiles,assignmentfilesItemLayoutBinding);
                break;
            case 6:
                //excel
                callApifetchAssignmentSignedUrl(Integer.parseInt(String.valueOf(submissionFiles.getFiles().getId())), "0", submissionFiles, dialog, assignmentfilesItemLayoutBinding, extension);
                break;
            case 7:
            case 8:
                //docx
                callApifetchAssignmentSignedUrl(Integer.parseInt(String.valueOf(submissionFiles.getFiles().getId())), "0", submissionFiles, dialog, assignmentfilesItemLayoutBinding, extension);
                break;

        }


    }

    public void OpenAnyFile(File file, String str) {
        String selectpath = String.valueOf(file);
        File outFile = new File(selectpath.substring(0, selectpath.lastIndexOf("/")) + "/" + str);
        String encrypted_path = selectpath.substring(0, selectpath.lastIndexOf("/")) + "/" + str;
        String decryptpath = null;
        try {
            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);

        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (NoSuchPaddingException e) {
            e.printStackTrace();
        } catch (InvalidKeyException e) {
            e.printStackTrace();
        } catch (InvalidAlgorithmParameterException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }

        // new AssignmentFileListAdapter.DownloadFile(decryptpath, selectpath).execute();
    }

    private void openImage(Dialog dialog, SubmissionFiles submissionFiles, AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding) {
        PRDownloader.download(submissionFiles.getFiles().getUrl(), String.valueOf(Environment.getExternalStorageDirectory()), submissionFiles.getFiles().getName())
                .build()
                .start(new OnDownloadListener() {
                    @Override
                    public void onDownloadComplete() {
                        dialog.show();
                        String imageUri = Environment.getExternalStorageDirectory() + "/" + submissionFiles.getFiles().getName();
                        GlideApp.with(context)
                                .load(Uri.parse(imageUri))
                                .error(R.drawable.assignment_icon)
                                .into(assignmentfilesItemLayoutBinding.quizImage);
                    }

                    @Override
                    public void onError(Error error) {

                    }
                });
    }

    public void OpenAnyDocFile(File file, String str, String filetypeid, SubmissionFiles assignmentfiles) {
        String selectpath = String.valueOf(file);
        File outFile = new File(selectpath.substring(0, selectpath.lastIndexOf("/")) + "/" + str);
        String encrypted_path = selectpath.substring(0, selectpath.lastIndexOf("/")) + "/" + str;
        String decryptpath = null;
        try {
            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);

        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (NoSuchPaddingException e) {
            e.printStackTrace();
        } catch (InvalidKeyException e) {
            e.printStackTrace();
        } catch (InvalidAlgorithmParameterException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        String[] extension = selectpath.split("\\.");
        for (String s : extension) {
            Log.e("extension", "OpenAnyFile: " + s);
        }
        String ext = "." + extension[2];
//        Log.e(extension, "OpenAnyFile: " + extension);

        if (filetypeid.equals("6") || filetypeid.equals("7") || filetypeid.equals("8")) {
            new DownloadAllFile(decryptpath, selectpath, ext).execute();
        }
//        } else if (filetypeid.equals("7")) {
//            new DownloadAllFile(decryptpath, selectpath,extension).execute();
//        } else if (filetypeid.equals("8")) {
//            new DownloadPPTFile(decryptpath, selectpath).execute();
//        }

    }

    private void CallApiAssignmentList(String assignmentId) {
        showDialog(getString(R.string.loading));
        disposable.add(assignmentRepository.fetchAssignmentsDetails(assignmentId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AssignmentDetailObject>() {
                    @Override
                    public void onSuccess(AssignmentDetailObject assignmentDetailObject) {

                        if (assignmentDetailObject.getData() == null) {
                            String myjson = "{\"response_code\":0,\"message\":\"Assignment Detail\",\"status\":\"Success\",\"data\":{\"id\":9,\"name\":\"lesson-1cc\",\"description\":\"sadasd\",\"code\":\"AS-1559885237645\",\"chapter\":{\"id\":129,\"name\":\"testing math\",\"code\":\"CH-1559890738457\",\"courseid\":176,\"quizid\":null,\"itemorder\":0,\"assignmentDetails\":null},\"assignmentfiles\":[{\"id\":4,\"files\":{\"id\":24644,\"name\":\"HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4\",\"filename\":\"HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4\",\"description\":\"HD\",\"filetypename\":\"Video\",\"url\":\"https://storage.googleapis.com/t24-primary-video-storage/HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4?GoogleAccessId=paras-chodavadiya@training24-197210.iam.gserviceaccount.com&Expires=1559917036&Signature=MrShz7X3GG2LJ1t0wGsuHTkD8ZviSPKrYhCaN%2FoIo7hnMuOOJ1mYIg32x16xp%2BHxBHeG4rDJEFV8%2BjfqfUFqK9uKIlyeRJiX%2BVrsO9D6NBqwpKUN%2Bs9R3jnA8%2BLcfGa%2BeO4my1F40iuWlScL8qy8xbdzWH3FTUsgtecXksDfJbdrQsu%2FutKyHGjXE2hWzwoIGPynqqkbThCeZeIo4CJEjjkUpGL84CUPr6CVSZMW1r0KvodMbwog0C7tL2YYQCh5ddmyT1Cj%2BikbVjkKjzf0Psshg43Jvbo9q6xHEt1p5cu4zZB8BCQiOnPZjHNruJ5Wars%2FnWergiRj46nkhQBXTQ%3D%3D\",\"filesize\":21564929,\"filetypeid\":2,\"totalpages\":0,\"duration\":null}},{\"id\":5,\"files\":{\"id\":24506,\"name\":\"SampleVideo_1280x720_1mb_fd294bc7-c033-4a50-ab39-64cf339311e0.mp4\",\"filename\":\"SampleVideo_1280x720_1mb_fd294bc7-c033-4a50-ab39-64cf339311e0.mp4\",\"description\":null,\"filetypename\":\"Video\",\"url\":\"https://storage.googleapis.com/t24-primary-video-storage/SampleVideo_1280x720_1mb_fd294bc7-c033-4a50-ab39-64cf339311e0.mp4?GoogleAccessId=paras-chodavadiya@training24-197210.iam.gserviceaccount.com&Expires=1559917036&Signature=ObxKWjVdAQXXqUwLY5LOmfKnqXGhm5apPBGrgsfGJ6nr4zp5zXZTi6lmna35qolnMcgPmNDByrDNl0hI3ZRAHm7IaPkl29B5xtvRG8Lgxz3NI%2B3FKImOzas1p3psV8c13iJjnG4rX%2B4jE9vUrXIrvnD8O3eoyL01L6uJAROkdg7METw5qSdbFk322OSUcEpqR3WFUZwHpDcheEUE7ebH3HhoxuLFOOYKR%2FH5zQDZHUlgGR1SjkmA6aW70ZJSLbhbGpGZYJbBUAPdYID%2FjGv5YpWNlW5aqLGxZhCQCTgj3RVdST6EqK9ieh14jfOCRKnYnZn6%2FK8ipfdZr74oSyFtXA%3D%3D\",\"filesize\":1055736,\"filetypeid\":2,\"totalpages\":0,\"duration\":null}},{\"id\":6,\"files\":{\"id\":24644,\"name\":\"HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4\",\"filename\":\"HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4\",\"description\":\"HD\",\"filetypename\":\"Video\",\"url\":\"https://storage.googleapis.com/t24-primary-video-storage/HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4?GoogleAccessId=paras-chodavadiya@training24-197210.iam.gserviceaccount.com&Expires=1559917036&Signature=MrShz7X3GG2LJ1t0wGsuHTkD8ZviSPKrYhCaN%2FoIo7hnMuOOJ1mYIg32x16xp%2BHxBHeG4rDJEFV8%2BjfqfUFqK9uKIlyeRJiX%2BVrsO9D6NBqwpKUN%2Bs9R3jnA8%2BLcfGa%2BeO4my1F40iuWlScL8qy8xbdzWH3FTUsgtecXksDfJbdrQsu%2FutKyHGjXE2hWzwoIGPynqqkbThCeZeIo4CJEjjkUpGL84CUPr6CVSZMW1r0KvodMbwog0C7tL2YYQCh5ddmyT1Cj%2BikbVjkKjzf0Psshg43Jvbo9q6xHEt1p5cu4zZB8BCQiOnPZjHNruJ5Wars%2FnWergiRj46nkhQBXTQ%3D%3D\",\"filesize\":21564929,\"filetypeid\":2,\"totalpages\":0,\"duration\":null}}]}}";
                            Gson gson = new Gson();
                            assignmentDetailObject = gson.fromJson(myjson, AssignmentDetailObject.class);
                        }

                        assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        assignmentDetailLayoutBinding.assignmentName.setText(assignmentDetailObject.getData().getName());
                        assignmentDetailLayoutBinding.assignmentDescription.setText(assignmentDetailObject.getData().getDescription());
                        Log.e("assignmentBinding", "-----");
                        //Collections.addAll(assignment);

                       /* ArrayList<AssignmentDetailObject.Assignmentfiles> assignmentfilesArrayList = new ArrayList<>();
                        Collections.addAll(assignmentfilesArrayList, assignmentDetailObject.getData().getAssignmentfiles());
                        assignmentDetailLayoutBinding.rcVerticalLay.rcVertical.setLayoutManager(new LinearLayoutManager(getApplicationContext(), LinearLayoutManager.HORIZONTAL, false));
                        AssignmentFileListAdapter adp = new AssignmentFileListAdapter(getApplicationContext(), assignmentfilesArrayList, userDetailsObject);
                        assignmentDetailLayoutBinding.rcVerticalLay.rcVertical.setAdapter(adp);
                        adp.notifyDataSetChanged();*/
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {


                        String myjson = "{\"response_code\":0,\"message\":\"Assignment Detail\",\"status\":\"Success\",\"data\":{\"id\":9,\"name\":\"lesson-1cc\",\"description\":\"sadasd\",\"code\":\"AS-1559885237645\",\"chapter\":{\"id\":129,\"name\":\"testing math\",\"code\":\"CH-1559890738457\",\"courseid\":176,\"quizid\":null,\"itemorder\":0,\"assignmentDetails\":null},\"assignmentfiles\":[{\"id\":4,\"files\":{\"id\":24644,\"name\":\"HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4\",\"filename\":\"HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4\",\"description\":\"HD\",\"filetypename\":\"Video\",\"url\":\"https://storage.googleapis.com/t24-primary-video-storage/HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4?GoogleAccessId=paras-chodavadiya@training24-197210.iam.gserviceaccount.com&Expires=1559917036&Signature=MrShz7X3GG2LJ1t0wGsuHTkD8ZviSPKrYhCaN%2FoIo7hnMuOOJ1mYIg32x16xp%2BHxBHeG4rDJEFV8%2BjfqfUFqK9uKIlyeRJiX%2BVrsO9D6NBqwpKUN%2Bs9R3jnA8%2BLcfGa%2BeO4my1F40iuWlScL8qy8xbdzWH3FTUsgtecXksDfJbdrQsu%2FutKyHGjXE2hWzwoIGPynqqkbThCeZeIo4CJEjjkUpGL84CUPr6CVSZMW1r0KvodMbwog0C7tL2YYQCh5ddmyT1Cj%2BikbVjkKjzf0Psshg43Jvbo9q6xHEt1p5cu4zZB8BCQiOnPZjHNruJ5Wars%2FnWergiRj46nkhQBXTQ%3D%3D\",\"filesize\":21564929,\"filetypeid\":2,\"totalpages\":0,\"duration\":null}},{\"id\":5,\"files\":{\"id\":24506,\"name\":\"SampleVideo_1280x720_1mb_fd294bc7-c033-4a50-ab39-64cf339311e0.mp4\",\"filename\":\"SampleVideo_1280x720_1mb_fd294bc7-c033-4a50-ab39-64cf339311e0.mp4\",\"description\":null,\"filetypename\":\"Video\",\"url\":\"https://storage.googleapis.com/t24-primary-video-storage/SampleVideo_1280x720_1mb_fd294bc7-c033-4a50-ab39-64cf339311e0.mp4?GoogleAccessId=paras-chodavadiya@training24-197210.iam.gserviceaccount.com&Expires=1559917036&Signature=ObxKWjVdAQXXqUwLY5LOmfKnqXGhm5apPBGrgsfGJ6nr4zp5zXZTi6lmna35qolnMcgPmNDByrDNl0hI3ZRAHm7IaPkl29B5xtvRG8Lgxz3NI%2B3FKImOzas1p3psV8c13iJjnG4rX%2B4jE9vUrXIrvnD8O3eoyL01L6uJAROkdg7METw5qSdbFk322OSUcEpqR3WFUZwHpDcheEUE7ebH3HhoxuLFOOYKR%2FH5zQDZHUlgGR1SjkmA6aW70ZJSLbhbGpGZYJbBUAPdYID%2FjGv5YpWNlW5aqLGxZhCQCTgj3RVdST6EqK9ieh14jfOCRKnYnZn6%2FK8ipfdZr74oSyFtXA%3D%3D\",\"filesize\":1055736,\"filetypeid\":2,\"totalpages\":0,\"duration\":null}},{\"id\":6,\"files\":{\"id\":24644,\"name\":\"HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4\",\"filename\":\"HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4\",\"description\":\"HD\",\"filetypename\":\"Video\",\"url\":\"https://storage.googleapis.com/t24-primary-video-storage/HD_testing_ef13f9fd-04a6-4e61-a41d-f75536612fe9.mp4?GoogleAccessId=paras-chodavadiya@training24-197210.iam.gserviceaccount.com&Expires=1559917036&Signature=MrShz7X3GG2LJ1t0wGsuHTkD8ZviSPKrYhCaN%2FoIo7hnMuOOJ1mYIg32x16xp%2BHxBHeG4rDJEFV8%2BjfqfUFqK9uKIlyeRJiX%2BVrsO9D6NBqwpKUN%2Bs9R3jnA8%2BLcfGa%2BeO4my1F40iuWlScL8qy8xbdzWH3FTUsgtecXksDfJbdrQsu%2FutKyHGjXE2hWzwoIGPynqqkbThCeZeIo4CJEjjkUpGL84CUPr6CVSZMW1r0KvodMbwog0C7tL2YYQCh5ddmyT1Cj%2BikbVjkKjzf0Psshg43Jvbo9q6xHEt1p5cu4zZB8BCQiOnPZjHNruJ5Wars%2FnWergiRj46nkhQBXTQ%3D%3D\",\"filesize\":21564929,\"filetypeid\":2,\"totalpages\":0,\"duration\":null}}]}}";
                        Gson gson = new Gson();
                        AssignmentDetailObject assignmentDetailObject = gson.fromJson(myjson, AssignmentDetailObject.class);
                        assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        // assignmentDetailLayoutBinding.assignmentName.setText(assignmentDetailObject.getData().getName());
                        // assignmentDetailLayoutBinding.assignmentDescription.setText(assignmentDetailObject.getData().getDescription());

                        ArrayList<AssignmentDetailObject.Assignmentfiles> assignmentfilesArrayList = new ArrayList<>();
                        Collections.addAll(assignmentfilesArrayList, assignmentDetailObject.getData().getAssignmentfiles());
                        assignmentDetailLayoutBinding.rcVerticalLay.rcVertical.setLayoutManager(new LinearLayoutManager(AssignmentDetailActivity.this, LinearLayoutManager.HORIZONTAL, false));
                        //AssignmentFileListAdapter adp = new AssignmentFileListAdapter(AssignmentDetailActivity.this, assignmentfilesArrayList, userDetailsObject);
                        // assignmentDetailLayoutBinding.rcVerticalLay.rcVertical.setAdapter(adp);
                        // adp.notifyDataSetChanged();

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            //AssignmentDetailObject assignmentDetailObject = new Gson().fromJson(error.response().errorBody().string(), AssignmentDetailObject.class);
                            // Toast.makeText(getApplicationContext(), assignmentDetailObject.getMessage(), Toast.LENGTH_SHORT).show();

                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));

    }

    public class DownloadAssignmentStatusTask extends AsyncTask<String, String, String> {

        String fileURL;
        //LibraryGradeInnerListAdapter.MyViewHolder holder;
        SubmissionFiles model;
        int downloadId = 0;
        long progressval = 0;
        Context context;
        String encrypted_path;
        String decryptpath = "";
        private Dialog dialog;
        private AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding;
        private int type;
        private String extension;

        DownloadAssignmentStatusTask(Context context, String url, SubmissionFiles model, Dialog dialog, AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding, int type, String extension) {
            this.context = context;
            this.fileURL = url;
            this.model = model;
            this.dialog = dialog;
            this.assignmentfilesItemLayoutBinding = assignmentfilesItemLayoutBinding;
            this.type = type;
            this.extension = extension;
        }


//        @Override
//        protected void onPreExecute() {
//
//            mProgressDialog = new ProgressDialog(context);
//            mProgressDialog.setMessage("Downloading file…");
//            mProgressDialog.setIndeterminate(false);
//            mProgressDialog.setMax(100);
//            mProgressDialog.setProgressStyle(ProgressDialog.STYLE_HORIZONTAL);
//            mProgressDialog.setCancelable(true);
//            mProgressDialog.show();
//            super.onPreExecute();
//
//        }

        @Override
        protected String doInBackground(String... strings) {
            String userIdSlash = "";

            String EncryptFileName = model.getFiles().getFilename();
            String filetype = model.getFiles().getFiletypename();
            String fileName = model.getFiles().getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);
            String downloadFilePath = Const.destPath + userIdSlash + fileName;

            try {

                String str = model.getFiles().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
                PRDownloader.cleanUp(1);

                downloadId = PRDownloader.download(fileURL, Const.destPath, fileName)
                        .build()
                        .setOnStartOrResumeListener(new OnStartOrResumeListener() {
                            @Override
                            public void onStartOrResume() {
                                // Toast.makeText(AssignmentDetailActivity.this, AssignmentDetailActivity.this.getString(R.string.downloading) + model.get(), Toast.LENGTH_SHORT).show();
                            }
                        })
                        .setOnPauseListener(new OnPauseListener() {
                            @Override
                            public void onPause() {

                            }
                        })
                        .setOnCancelListener(new OnCancelListener() {
                            @Override
                            public void onCancel() {

                            }
                        })
                        .setOnProgressListener(new OnProgressListener() {
                            @Override
                            public void onProgress(Progress progress) {
                                progressval = (progress.currentBytes * 100) / progress.totalBytes;
                                // mProgressDialog.setProgress((int) progressval);
                                //Log.e("-------------", "==== 4444444444 =====" + progressval);
                            }
                        })
                        .start(new OnDownloadListener() {
                            @Override
                            public void onDownloadComplete() {


                                EncryptDecryptObject encryptDecryptObject = new EncryptDecryptObject();
                                encryptDecryptObject.setSelectedVideoPath(downloadFilePath);
                                encryptDecryptObject.setFilename(str);

                                new SetEncryptDecryptTask(new EncryptDecryptAsyncResponse() {
                                    @Override
                                    public EncryptDecryptObject getEncryptDecryptObjects(EncryptDecryptObject encryptDecryptObjects) {

                                        try {


                                            switch (type) {
                                                case 1:
                                                    String selectedVideoPath = encryptDecryptObject.getSelectedVideoPath();
                                                    String filename = encryptDecryptObject.getFilename();
                                                    File outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
                                                    encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename;
                                                    decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);

                                                    final Dialog dialogpdf = new Dialog(context, android.R.style.Theme_Translucent_NoTitleBar);
                                                    dialogpdf.requestWindowFeature(Window.FEATURE_NO_TITLE);
                                                    dialogpdf.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                                                    dialogpdf.setCancelable(true);
                                                    DialogViewerItemLayoutBinding dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(context), R.layout.dialog_viewer_item_layout, null, false);
                                                    dialogpdf.setContentView(dialogViewerItemLayoutBinding.getRoot());
                                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
                                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(CourseName);
                                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(LessonName);
                                                    openPdf(dialogViewerItemLayoutBinding, dialogpdf, model);
                                                    break;
                                                case 3:
                                                    String selectedVideoPathimg = encryptDecryptObject.getSelectedVideoPath();
                                                    String filenameimg = encryptDecryptObject.getFilename();
                                                    File outFileimg = new File(selectedVideoPathimg.substring(0, selectedVideoPathimg.lastIndexOf("/")) + "/" + str);
                                                    encrypted_path = selectedVideoPathimg.substring(0, selectedVideoPathimg.lastIndexOf("/")) + "/" + filenameimg;
                                                    decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFileimg), encrypted_path);

                                                    dialog.show();
                                                    GlideApp.with(context)
                                                            .load(decryptpath)
                                                            .error(R.drawable.ic_no_image_found)
                                                            .into(assignmentfilesItemLayoutBinding.quizImage);
                                                    break;

                                                case 6:
                                                case 7:
                                                case 8:

                                                    String EncryptFileName = model.getFiles().getFilename();
                                                    //String filetype = assignmentfiles.getFiles().getFiletypename();
                                                    String fileName = model.getFiles().getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);
                                                    String str = model.getFiles().getId() + "_" + model.getFiles().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + model.getFiles().getFiletypename() + Const.extension;
                                                    File file = new File(Const.destPath + "/" + fileName);
                                                    OpenAnyDocFile(file, str, String.valueOf(model.getFiles().getFiletypeid()), model);
                                                    /*String selectedPath = encryptDecryptObject.getSelectedVideoPath();
                                                    String filenamedoc = model.getFiles().getFilename();
                                                    FileData outFiledoc = new FileData(selectedPath.substring(0, selectedPath.lastIndexOf("/")) + "/" + str);
                                                    encrypted_path = selectedPath.substring(0, selectedPath.lastIndexOf("/")) + "/" + filenamedoc;
                                                    decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFiledoc), encrypted_path);
                                                    openFile(decryptpath, filenamedoc);*/

                                                    break;
                                            }

                                            //openDialogViewer(context, decryptpath, model);
                                            //mProgressDialog.dismiss();


                                        } catch (NoSuchAlgorithmException e) {
                                            Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (NoSuchPaddingException e) {
                                            Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidKeyException e) {
                                            Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidAlgorithmParameterException e) {
                                            Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (IOException e) {
                                            if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                                                showNoSpaceAlert(AssignmentDetailActivity.this);
                                            }
                                            Crashlytics.log(Log.ERROR, AssignmentDetailActivity.this.getString(R.string.app_name), e.getMessage());
                                            e.printStackTrace();
                                        }

                                        return null;
                                    }
                                }, encryptDecryptObject).execute();
                            }

                            @Override
                            public void onError(Error error) {
                                if (error.isServerError()) {
                                    Log.e("downloaderror---", "onError: " + error);
                                    //Toast.makeText(ctx, "=== DOWNLAOD ServerError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isConnectionError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ConnectionError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isENOSPCError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ENOSPCError (No space left on device)===", Toast.LENGTH_SHORT).show();
                                    showNoSpaceAlert(AssignmentDetailActivity.this);
                                }
                                //hideDialog();
                                Crashlytics.log(Log.ERROR, AssignmentDetailActivity.this.getString(R.string.app_name), error.getError());
                            }
                        });

            } catch (Exception e) {

            }
            return null;
        }

        @Override
        protected void onPostExecute(String unused) {
            // mProgressDialog.dismiss();
        }
    }

    public class DownloadStatusTask extends AsyncTask<String, String, String> {

        String fileURL;
        //LibraryGradeInnerListAdapter.MyViewHolder holder;
        CoursePriviewObject.Assignmentfiles model;
        int downloadId = 0;
        long progressval = 0;
        Context context;
        String encrypted_path;
        String decryptpath = "";

        public DownloadStatusTask(Context context, String fileURL, CoursePriviewObject.Assignmentfiles model) {
            this.fileURL = fileURL;
            //this.holder = holder;
            this.model = model;
            this.context = context;

        }

//        @Override
//        protected void onPreExecute() {
//
//            mProgressDialog = new ProgressDialog(context);
//            mProgressDialog.setMessage("Downloading file…");
//            mProgressDialog.setIndeterminate(false);
//            mProgressDialog.setMax(100);
//            mProgressDialog.setProgressStyle(ProgressDialog.STYLE_HORIZONTAL);
//            mProgressDialog.setCancelable(true);
//            mProgressDialog.show();
//            super.onPreExecute();
//
//        }

        @Override
        protected String doInBackground(String... strings) {
            String userIdSlash = "";

            String EncryptFileName = model.getFiles().getFilename();
            String filetype = model.getFiles().getFiletypename();
            String fileName = model.getFiles().getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);
            String downloadFilePath = Const.destPath + userIdSlash + fileName;

            try {

                String str = model.getFiles().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
                PRDownloader.cleanUp(1);

                downloadId = PRDownloader.download(fileURL, Const.destPath + userIdSlash, fileName)
                        .build()
                        .setOnStartOrResumeListener(new OnStartOrResumeListener() {
                            @Override
                            public void onStartOrResume() {
                                // Toast.makeText(AssignmentDetailActivity.this, AssignmentDetailActivity.this.getString(R.string.downloading) + model.get(), Toast.LENGTH_SHORT).show();
                            }
                        })
                        .setOnPauseListener(new OnPauseListener() {
                            @Override
                            public void onPause() {

                            }
                        })
                        .setOnCancelListener(new OnCancelListener() {
                            @Override
                            public void onCancel() {

                            }
                        })
                        .setOnProgressListener(new OnProgressListener() {
                            @Override
                            public void onProgress(Progress progress) {
                                progressval = (progress.currentBytes * 100) / progress.totalBytes;
                                // mProgressDialog.setProgress((int) progressval);
                                //Log.e("-------------", "==== 4444444444 =====" + progressval);
                            }
                        })
                        .start(new OnDownloadListener() {
                            @Override
                            public void onDownloadComplete() {
                                Log.d("download_id", "onDownloadComplete: " + downloadId);

                                EncryptDecryptObject encryptDecryptObject = new EncryptDecryptObject();
                                encryptDecryptObject.setSelectedVideoPath(downloadFilePath);
                                encryptDecryptObject.setFilename(str);

                                new SetEncryptDecryptTask(new EncryptDecryptAsyncResponse() {
                                    @Override
                                    public EncryptDecryptObject getEncryptDecryptObjects(EncryptDecryptObject encryptDecryptObjects) {

                                        try {

                                            String selectedVideoPath = encryptDecryptObject.getSelectedVideoPath();
                                            String filename = encryptDecryptObject.getFilename();
                                            File outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
                                            encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename;
                                            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
                                            //openDialogViewer(context, decryptpath, model);
                                            //mProgressDialog.dismiss();


                                        } catch (NoSuchAlgorithmException e) {
                                            Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (NoSuchPaddingException e) {
                                            Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidKeyException e) {
                                            Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidAlgorithmParameterException e) {
                                            Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (IOException e) {
                                            if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                                                showNoSpaceAlert(AssignmentDetailActivity.this);
                                            }
                                            Crashlytics.log(Log.ERROR, AssignmentDetailActivity.this.getString(R.string.app_name), e.getMessage());
                                            e.printStackTrace();
                                        }

                                        return null;
                                    }
                                }, encryptDecryptObject).execute();
                            }

                            @Override
                            public void onError(Error error) {
                                if (error.isServerError()) {
                                    Log.e("onError", "onError: " + downloadId);
                                    //Toast.makeText(contex, "=== DOWNLAOD ServerError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isConnectionError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ConnectionError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isENOSPCError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ENOSPCError (No space left on device)===", Toast.LENGTH_SHORT).show();
                                    showNoSpaceAlert(AssignmentDetailActivity.this);
                                }
                                //hideDialog();
                                Crashlytics.log(Log.ERROR, AssignmentDetailActivity.this.getString(R.string.app_name), error.getError());
                            }
                        });

            } catch (Exception e) {

            }
            return null;
        }

        @Override
        protected void onPostExecute(String unused) {
            // mProgressDialog.dismiss();
        }
    }

    public class SetEncryptDecryptTask extends AsyncTask<Void, Void, EncryptDecryptObject> {

        EncryptDecryptObject encryptDecryptObject;
        EncryptDecryptAsyncResponse delegate = null;

        public SetEncryptDecryptTask(EncryptDecryptAsyncResponse delegate, EncryptDecryptObject encryptDecryptObject) {
            this.encryptDecryptObject = encryptDecryptObject;
            this.delegate = delegate;
        }

        @Override

        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected EncryptDecryptObject doInBackground(Void... params) {

            try {
                String selectedVideoPath = encryptDecryptObject.getSelectedVideoPath();
                String filename = encryptDecryptObject.getFilename();
                File inFile = new File(selectedVideoPath);
                File outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename);
                EncrypterDecryptAlgo.encrypt(key, paramSpec, new FileInputStream(inFile), new FileOutputStream(outFile));
                inFile.delete();

            } catch (NoSuchAlgorithmException e) {
                ((Activity) AssignmentDetailActivity.this).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (NoSuchPaddingException e) {
                ((Activity) AssignmentDetailActivity.this).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (InvalidKeyException e) {
                ((Activity) AssignmentDetailActivity.this).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (InvalidAlgorithmParameterException e) {
                ((Activity) AssignmentDetailActivity.this).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(AssignmentDetailActivity.this, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (IOException e) {
                //encryptArray = new ArrayList<>();
                //downloadArray = new ArrayList<>();
                if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                    showNoSpaceAlert(AssignmentDetailActivity.this);
                }
                Crashlytics.log(Log.ERROR, AssignmentDetailActivity.this.getString(R.string.app_name), e.getMessage());
                e.printStackTrace();
            }
            return encryptDecryptObject;
        }

        @Override
        protected void onPostExecute(EncryptDecryptObject encryptDecryptObject) {
            delegate.getEncryptDecryptObjects(encryptDecryptObject);
        }

    }

    private class DownloadAllFile extends AsyncTask<Void, Void, String> {
        private String decryptpath;
        private String selectpath;
        private String extension;
        private int downloadId;

        DownloadAllFile(String decryptpath, String selectpath, String extension) {

            this.decryptpath = decryptpath;
            this.selectpath = selectpath;
            this.extension = extension;
        }

        @Override
        protected String doInBackground(Void... voids) {
            Log.d("xldownload---", "doInBackground: " + decryptpath);
            downloadId = PRDownloader.download(decryptpath, String.valueOf(Environment.getExternalStorageDirectory()), Const.dir_fileName + extension)
                    .build()
                    .start(new OnDownloadListener() {
                        @Override
                        public void onDownloadComplete() {
                            Intent newIntent = new Intent(Intent.ACTION_VIEW);
                            String path = Environment.getExternalStorageDirectory() + File.separator + Const.dir_fileName + extension;
                            newIntent.setDataAndType(Uri.parse(path), "*/*");

                            newIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);

                            try {
                                startActivity(newIntent);
                            } catch (ActivityNotFoundException e) {
                                Toast.makeText(context, "No handler for this type of file.", Toast.LENGTH_LONG).show();
                            }
                        }

                        @Override
                        public void onError(Error error) {

                        }
                    });
            return null;
        }
    }

    private void addFileChooserFragment(String userNewId) {
        assignmentDetailLayoutBinding.filechooserFragment.setVisibility(View.VISIBLE);
        builder = new FileChooser.Builder(FileChooser.ChooserType.FILE_CHOOSER,
                (FileChooser.ChooserListener) path -> {
                    assignmentDetailLayoutBinding.filechooserFragment.setVisibility(View.GONE);
                    assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                    if (!TextUtils.isEmpty(path)) {
                        String[] selectedFilePaths = path.split(FileChooser.FILE_NAMES_SEPARATOR);
                        //fileUploadAdapter.addFile(selectedFilePaths[0]);
                        uploadFile(selectedFilePaths[0], userNewId);
                    } else {
                        Toast.makeText(AssignmentDetailActivity.this, R.string.no_file_chosen, Toast.LENGTH_SHORT).show();
                    }
                })

                //.setMultipleFileSelectionEnabled(true)
                // Word document,PDF file,Powerpoint file,Excel file,GIF file,JPG file ,PNG file,Text file ,Video files
                .setFileFormats(new String[]{".jpg", ".png", ".jpeg", ".gif", ".3gp", ".mpg", ".mpeg", ".mpe", ".mp4", ".avi", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".docx", ".doc"});

        try {
            getSupportFragmentManager().beginTransaction().replace(R.id.filechooserFragment, builder.build()).commit();
        } catch (ExternalStorageNotAvailableException e) {
            Toast.makeText(this, R.string.no_external_storage, Toast.LENGTH_SHORT).show();
            e.printStackTrace();
        }
    }

    private void uploadFile(String selectedFilePath, String userNewId) {
        setButtonEnable(false);
        File file = new File(selectedFilePath);
        if (isNetworkAvailable(AssignmentDetailActivity.this)) {
            int type = getMimeType(file, selectedFilePath);
            RequestBody requestFile;
            switch (type) {
                case 1:
                    requestFile = RequestBody.create(MediaType.parse("application/pdf"), file);
                    break;
                case 2:
                    requestFile = RequestBody.create(MediaType.parse("video/*"), file);
                    break;
                case 3:
                    requestFile = RequestBody.create(MediaType.parse("image/*"), file);
                    break;
                case 6:
                case 7:
                case 8:
                    requestFile = RequestBody.create(MediaType.parse("*/*"), file);
                    break;
                default:
                    // Error
                    Toast.makeText(this, "No file chooser", Toast.LENGTH_SHORT).show();
                    return;
            }

//            String typeid = String.valueOf(type);
//            int filetypeid = Integer.valueOf(typeid);
            MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
            RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), String.valueOf(type));
            RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
            RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");
            RequestBody totalpages = RequestBody.create(MediaType.parse("text/plain"), "0");

            Call<RestResponse<FileUploadResponse>> call = assignmentRepository.uploadAssignmentFile(body, fileTypeId, duration, filesize, totalpages);
            call.enqueue(new Callback<RestResponse<FileUploadResponse>>() {
                @Override
                public void onResponse(@NonNull Call<RestResponse<FileUploadResponse>> call, @NonNull Response<RestResponse<FileUploadResponse>> response) {
                    setButtonEnable(true);
                    assert response.body() != null;
                    if (response.body().getResponse_code().equalsIgnoreCase("0")) {
                        fileUploadList.clear();
                        fileUploadList.add(String.valueOf(response.body().getData().getId()));
                        assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        if (fileUploadList != null && fileUploadList.size() != 0) {
                            if (isNetworkAvailable(AssignmentDetailActivity.this)) {
                                createdAssigmentSubmission(userNewId);
                            } else {
                                showNetworkAlert(AssignmentDetailActivity.this);
                            }
                        }
                    }
                }

                @Override
                public void onFailure(@NonNull Call<RestResponse<FileUploadResponse>> call, @NonNull Throwable t) {
                    setButtonEnable(true);
                    assignmentDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                }
            });
        }

    }

    public void createdAssigmentSubmission(String userNewId) {
        if (flag == 1) {
            submitAssignmentLession(userNewId);
        } else {
            submitAssignment(userNewId);
        }
    }

    private void submitAssignmentLession(String userNewId) {
        Gson gson = new Gson();
        JsonParser parser = new JsonParser();
        HashMap<String, Object> hashMap = new HashMap<>();
        hashMap.put(Const.assignmentid, assignment.getId());
        hashMap.put(Const.issubmission, true);
        hashMap.put(Const.userid, userNewId);
        hashMap.put(Const.files, fileUploadList);

        showDialog(getString(R.string.loading));
        disposable.add(assignmentRepository.submitAssignmentLession((JsonObject) parser.parse(gson.toJson(hashMap)))
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse<AssignmentData>>() {
                    @Override
                    public void onSuccess(RestResponse<AssignmentData> assignmentDataRestResponse) {
                        hideDialog();
                        Toast.makeText(getApplicationContext(), assignmentDataRestResponse.getMessage(), Toast.LENGTH_SHORT).show();
                        Log.e("DDDDDD","D"+userRoleName);
                        if (!TextUtils.isEmpty(userRoleName)) {
                            if (userRoleName.equals(Const.TeacherKEY)) {
                                flagPassApiCallCommentsTeacher(Integer.parseInt(userNewId));
                            } else {
                                flagPassApiCallComments();
                            }

                        }

                        //PrivousScreen();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));
    }

    private void submitAssignment(String userNewId) {
        Gson gson = new Gson();
        JsonParser parser = new JsonParser();
        HashMap<String, Object> hashMap = new HashMap<>();
        hashMap.put(Const.assignmentid, assignment.getId());
        hashMap.put(Const.issubmission, true);
        hashMap.put(Const.userid, userNewId);
        hashMap.put(Const.files, fileUploadList);

        showDialog(getString(R.string.loading));
        disposable.add(assignmentRepository.submitAssignment((JsonObject) parser.parse(gson.toJson(hashMap)))
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse<AssignmentData>>() {
                    @Override
                    public void onSuccess(RestResponse<AssignmentData> assignmentDataRestResponse) {
                        hideDialog();
                        Toast.makeText(getApplicationContext(), assignmentDataRestResponse.getMessage(), Toast.LENGTH_SHORT).show();
                        //PrivousScreen();
                        if (!TextUtils.isEmpty(userRoleName)) {
                            if (userRoleName.equals(Const.TeacherKEY)) {
                                flagPassApiCallCommentsTeacher(Integer.parseInt(userNewId));
                            } else {
                                flagPassApiCallComments();
                            }

                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));
    }


    @NonNull
    static int getMimeType(@NonNull File file, String selectedFilePath) {
        String type = null;
        int fileType = -1;
        final String url = file.toString();
        final String extension = MimeTypeMap.getFileExtensionFromUrl(url);
        if (extension != null) {
            type = MimeTypeMap.getSingleton().getMimeTypeFromExtension(extension.toLowerCase());
            Log.e("type", "getMimeType: " + type);
        }
        if (type == null) {
            if (selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xls")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlsx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("csv")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dbf")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dif")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("ods")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("prn")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("slk")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xla")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlam")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlsb")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlt")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlsm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xltm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xltx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlw")) {
                fileType = 6;
            } else if (selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("doc")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("docx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("docs")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("docm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dot")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dotm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dotx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("odt")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("rtf")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("txt")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("wps")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xml")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xps")) {
                fileType = 7;
            } else if (selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("pptx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("ppt")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("pptm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("potx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("potm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("odp")) {
                fileType = 8;
            }
//            } else {
//                fileType = 0;
//            }
        } else {
            if (type.contains("image")) {
                fileType = 3;
            } else if (type.contains("video")) {
                fileType = 2;
            } else if (type.contains("pdf")) {
                fileType = 1;
            } else if (type.contains("excel")) {
                fileType = 6;
            } else if (type.contains("msword")) {
                fileType = 7;
            } else if (type.contains("vnd.ms-powerpoint")) {
                fileType = 8;
            }
        }
        return fileType;
    }

    private void setButtonEnable(boolean isEnable) {
        assignmentDetailLayoutBinding.txtbox.setEnabled(isEnable);
        assignmentDetailLayoutBinding.txtbox.setAlpha(isEnable ? 1f : 0.5f);
    }


}
