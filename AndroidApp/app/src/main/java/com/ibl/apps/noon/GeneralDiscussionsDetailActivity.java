package com.ibl.apps.noon;

import android.Manifest;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.graphics.Typeface;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.widget.CompoundButton;
import android.widget.Toast;

import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.core.content.res.ResourcesCompat;
import androidx.localbroadcastmanager.content.LocalBroadcastManager;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.ibl.apps.Adapter.DiscussionFIlePreviewListAdapter;
import com.ibl.apps.Adapter.DiscussionsCommentListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.DiscussionManagement.DiscussionRepository;
import com.ibl.apps.Interface.ViewDiscussionsFiles;
import com.ibl.apps.Model.AddComment;
import com.ibl.apps.Model.AddDiscussionTopic;
import com.ibl.apps.Model.DiscussionsDetails;
import com.ibl.apps.Model.GetAllComment;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UploadTopicFile;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.databinding.DiscussionsDetailLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.util.LoadMoreData.OnLoadMoreListener;
import com.ibl.apps.util.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.Validator;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import ir.sohreco.androidfilechooser.ExternalStorageNotAvailableException;
import ir.sohreco.androidfilechooser.FileChooser;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.HttpException;

import static com.ibl.apps.util.Const.GradeID;


public class GeneralDiscussionsDetailActivity extends BaseActivity implements View.OnClickListener, ViewDiscussionsFiles {

    DiscussionsDetailLayoutBinding discussionsDetailLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();
    private final static int READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE = 13;
    String topicId, topicname;
    final Handler h = new Handler();
    boolean mainisprivate = false;
    UserDetails userDetailsObject = new UserDetails();
    String userId = "0";
    FileChooser.Builder builder;
    ArrayList<String> fileuploadlist = new ArrayList<>();
    String GradeId, CourseName, ActivityFlag, LessonID, QuizID;
    DiscussionsCommentListAdapter discussionsCommentListAdapter;
    int pageNumber = 1;
    String perpagerecord = "10";
    private RecyclerViewLoadMoreScroll scrollListener;
    List<GetAllComment.Data> commentList = new ArrayList<>();
    boolean isLoad = true;
    DiscussionFIlePreviewListAdapter discussionFIlePreviewListAdapter;
    List<DiscussionsDetails.Files> discussionsFile = new ArrayList<>();
    boolean mainClickFlag = false;
    boolean enableDisabl = false;
    String AddtionalLibrary, AddtionalDiscussions, AddtionalAssignment = "";
    private DiscussionRepository discussionRepository;

    @Override
    protected int getContentView() {
        return R.layout.discussions_detail_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        discussionsDetailLayoutBinding = (DiscussionsDetailLayoutBinding) getBindObj();
        discussionRepository = new DiscussionRepository();
        if (getIntent() != null) {
            topicId = getIntent().getStringExtra(Const.topicId);
            topicname = getIntent().getStringExtra(Const.topicname);
            mainisprivate = Boolean.parseBoolean(getIntent().getStringExtra(Const.isprivate));
            GradeId = getIntent().getStringExtra(GradeID);
            CourseName = getIntent().getStringExtra(Const.CourseName);
            ActivityFlag = getIntent().getStringExtra(Const.ActivityFlag);
            LessonID = getIntent().getStringExtra(Const.LessonID);
            QuizID = getIntent().getStringExtra(Const.QuizID);
            AddtionalLibrary = getIntent().getStringExtra(Const.AddtionalLibrary);
            AddtionalAssignment = getIntent().getStringExtra(Const.AddtionalAssignment);
            AddtionalDiscussions = getIntent().getStringExtra(Const.AddtionalDiscussions);

            PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
                @Override
                public UserDetails getLocalUserDetails(UserDetails userDetails) {
                    if (userDetails != null) {
                        userDetailsObject = userDetails;
                        userId = userDetailsObject.getId();
                        if (isNetworkAvailable(getApplicationContext())) {
                            CallApiDiscussionsDetails(topicId);
                        } else {
                            showNetworkAlert(GeneralDiscussionsDetailActivity.this);
                        }
                    }
                    return null;
                }

            }).execute();
            CallApi();
        }
        setToolbar(discussionsDetailLayoutBinding.toolBar);
        //showBackArrow(topicname);
        showBackArrow("Discussions");
        setOnClickListener();

        discussionsDetailLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);
        linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
        discussionsDetailLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(linearLayoutManager);
        discussionsCommentListAdapter = new DiscussionsCommentListAdapter(getApplicationContext(), commentList, userDetailsObject, discussionsDetailLayoutBinding.rcVerticalLayout.rcVertical);
        discussionsDetailLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(discussionsCommentListAdapter);
        scrollListener = new RecyclerViewLoadMoreScroll(linearLayoutManager);
        scrollListener.setOnLoadMoreListener(new OnLoadMoreListener() {
            @Override
            public void onLoadMore() {
                if (isLoad) {
                    discussionsDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                    CallApiGetComments(topicId);
                } else {
                    discussionsDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                }
            }
        });
        discussionsDetailLayoutBinding.rcVerticalLayout.rcVertical.addOnScrollListener(scrollListener);
        discussionsDetailLayoutBinding.rcVerticalLay.rcVertical.setLayoutManager(new LinearLayoutManager(getApplicationContext(), LinearLayoutManager.HORIZONTAL, false));
        discussionFIlePreviewListAdapter = new DiscussionFIlePreviewListAdapter(getApplicationContext(), discussionsFile, userDetailsObject, enableDisabl, this);
        discussionsDetailLayoutBinding.rcVerticalLay.rcVertical.setAdapter(discussionFIlePreviewListAdapter);

        LocalBroadcastManager.getInstance(this).registerReceiver(mMessageReceiver, new IntentFilter(getString(R.string.brodcastTAG)));
    }

    private BroadcastReceiver mMessageReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String topid = intent.getStringExtra(Const.topicId);
            commentList.clear();
            pageNumber = 1;
            CallApiGetComments(topid);
        }
    };

    public void setOnClickListener() {

        discussionsDetailLayoutBinding.discussionsDelete.setOnClickListener(this);
        discussionsDetailLayoutBinding.discussionsEdit.setOnClickListener(this);
        discussionsDetailLayoutBinding.discussionsCheckEdit.setOnClickListener(this);
        discussionsDetailLayoutBinding.cardCommentpicker.setOnClickListener(this);
        discussionsDetailLayoutBinding.cardAddFilePicker.setOnClickListener(this);
        discussionsDetailLayoutBinding.shareSwitch.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                mainisprivate = isChecked;
            }
        });
        discussionsDetailLayoutBinding.txtbox.setOnTouchListener(new View.OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                final int DRAWABLE_LEFT = 0;
                final int DRAWABLE_TOP = 1;
                final int DRAWABLE_RIGHT = 2;
                final int DRAWABLE_BOTTOM = 3;

                if (event.getAction() == MotionEvent.ACTION_UP) {
                    if (event.getRawX() >= (discussionsDetailLayoutBinding.txtbox.getRight() - discussionsDetailLayoutBinding.txtbox.getCompoundDrawables()[DRAWABLE_RIGHT].getBounds().width())) {
                        // your action here
                        String comment = discussionsDetailLayoutBinding.txtbox.getText().toString().trim();

                        if (!TextUtils.isEmpty(comment)) {
                            CallApiCommentDiscussions(topicId, comment, new ArrayList<>());
                        } else {
                            Toast.makeText(getApplicationContext(), getString(R.string.validation_enter_comment), Toast.LENGTH_LONG).show();
                        }
                        return true;
                    }
                }
                return false;
            }
        });
    }

    public void enableDisableView(boolean enableDisable, boolean callApiEdit) {

        if (enableDisable) {
            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.GONE);
            discussionsDetailLayoutBinding.discussionsCheckEdit.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.edtdiscussionsTopicName.setEnabled(true);
            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setEnabled(true);
            discussionsDetailLayoutBinding.edtdiscussionsTopicName.requestFocus();
            discussionsDetailLayoutBinding.discussionsTopicNameWrapper.setHintEnabled(true);
            discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setHintEnabled(true);
            discussionsDetailLayoutBinding.edtdiscussionsTopicName.setBackground(getResources().getDrawable(R.drawable.text_box_underline_activated));
            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setBackground(getResources().getDrawable(R.drawable.text_box_underline_activated));
            discussionsDetailLayoutBinding.laySwitch.setVisibility(View.GONE);
            discussionsDetailLayoutBinding.cardAddFilePicker.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.cardAddFilePicker.setClickable(true);
            discussionsDetailLayoutBinding.cardAddFilePicker.setCardBackgroundColor(getResources().getColor(R.color.colorGreen));

        } else {
            if (callApiEdit) {
                String title = discussionsDetailLayoutBinding.edtdiscussionsTopicName.getText().toString().trim();
                String description = discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.getText().toString().trim();

                if (mainClickFlag) {
                    if (fileuploadlist != null && fileuploadlist.size() != 0) {
                        if (validateFields()) {
                            if (isNetworkAvailable(this)) {
                                new setFileWithDataTask(new asynDelegate() {
                                    @Override
                                    public void asynDelegateMethod(ArrayList<Integer> strings) {
                                        CallApiUpdateDiscussions(topicId, title, description, mainisprivate, strings);
                                    }
                                }).execute();
                            } else {
                                showNetworkAlert(GeneralDiscussionsDetailActivity.this);
                            }
                        }
                    }
                } else {
                    if (validateFields()) {
                        if (isNetworkAvailable(this)) {
                            CallApiUpdateDiscussions(topicId, title, description, mainisprivate, new ArrayList<>());
                        } else {
                            showNetworkAlert(GeneralDiscussionsDetailActivity.this);
                        }
                    }
                }

            } else {
                discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.VISIBLE);
                discussionsDetailLayoutBinding.discussionsCheckEdit.setVisibility(View.GONE);
                discussionsDetailLayoutBinding.edtdiscussionsTopicName.setEnabled(false);
                discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setEnabled(false);
                discussionsDetailLayoutBinding.discussionsTopicNameWrapper.setHintEnabled(false);
                discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setHintEnabled(false);
                discussionsDetailLayoutBinding.edtdiscussionsTopicName.setBackground(getResources().getDrawable(R.drawable.text_box_underline));
                discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setBackground(getResources().getDrawable(R.drawable.text_box_underline));
                discussionsDetailLayoutBinding.txtbox.requestFocus();
                discussionsDetailLayoutBinding.laySwitch.setVisibility(View.GONE);
                discussionsDetailLayoutBinding.cardAddFilePicker.setVisibility(View.GONE);
                discussionsDetailLayoutBinding.cardAddFilePicker.setClickable(false);
                discussionsDetailLayoutBinding.cardAddFilePicker.setCardBackgroundColor(getResources().getColor(R.color.colormorelightGrayColor));
            }
        }
    }

    private boolean validateFields() {
        if (!Validator.checkEmpty(discussionsDetailLayoutBinding.edtdiscussionsTopicName)) {
            hideKeyBoard(discussionsDetailLayoutBinding.edtdiscussionsTopicName);
            discussionsDetailLayoutBinding.edtdiscussionsTopicName.setError(getString(R.string.validation_enter_title));
            return false;
        } else {
            hideKeyBoard(discussionsDetailLayoutBinding.edtdiscussionsTopicName);
            discussionsDetailLayoutBinding.discussionsTopicNameWrapper.setErrorEnabled(false);
        }

        return true;
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.discussionsDelete:
                showDeleteAlert(GeneralDiscussionsDetailActivity.this);
                break;
            case R.id.discussionsEdit:
                enableDisableView(true, false);
                break;
            case R.id.discussionsCheckEdit:
                enableDisableView(false, true);
                break;
            case R.id.cardCommentpicker:
                fileuploadlist.clear();
                int permissionCheck = ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE);
                if (permissionCheck != PackageManager.PERMISSION_GRANTED) {
                    ActivityCompat.requestPermissions(this,
                            new String[]{Manifest.permission.READ_EXTERNAL_STORAGE},
                            READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE);
                } else {
                    addFileChooserFragment(false);
                }
                break;
            case R.id.cardAddFilePicker:
                fileuploadlist.clear();
                int permissionCheck1 = ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE);
                if (permissionCheck1 != PackageManager.PERMISSION_GRANTED) {
                    ActivityCompat.requestPermissions(this,
                            new String[]{Manifest.permission.READ_EXTERNAL_STORAGE},
                            READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE);
                } else {
                    addFileChooserFragment(true);
                }
                break;
        }
    }

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }


    public void showDeleteAlert(Context activity) {
        try {
            SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.validation_delete));
            androidx.appcompat.app.AlertDialog.Builder builder = new androidx.appcompat.app.AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                            if (isNetworkAvailable(activity)) {
                                CallApiDeleteDiscussions(topicId);
                            } else {
                                showNetworkAlert(GeneralDiscussionsDetailActivity.this);
                            }
                        }
                    });
            builder.setNegativeButton(activity.getResources().getString(R.string.button_cancel), new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                    dialog.cancel();
                }
            });

            builder.show();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void CallApiCommentDiscussions(String topicId, String comment, ArrayList<Integer> filesidArraylist) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.topicId, topicId);
            jsonObject.put(Const.comment, comment);
            jsonObject.put(Const.filesid, new JSONArray(filesidArraylist));
            jsonObject.put(Const.ispublic, true);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(discussionRepository.AddComment(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AddComment>() {
                    @Override
                    public void onSuccess(AddComment addComment) {
                        //hideDialog();
                        Toast.makeText(getApplicationContext(), addComment.getMessage(), Toast.LENGTH_SHORT).show();
                        discussionsDetailLayoutBinding.txtbox.setText("");
                        pageNumber = 1;
                        commentList.clear();
                        CallApiGetComments(topicId);
                        discussionsDetailLayoutBinding.rcVerticalLayout.rcVertical.scrollToPosition(0);
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    private void CallApiDiscussionsDetails(String topicId) {

        showDialog(getString(R.string.loading));
        disposable.add(discussionRepository.DiscussionsDetails(topicId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DiscussionsDetails>() {
                    @Override
                    public void onSuccess(DiscussionsDetails discussionsDetails) {
                        discussionsDetailLayoutBinding.edtdiscussionsTopicName.setText(discussionsDetails.getData().getTitle());


                        if (!TextUtils.isEmpty(discussionsDetails.getData().getDescription())) {
                            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setText(discussionsDetails.getData().getDescription());
                        } else {
                            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setVisibility(View.GONE);
                        }

                        if (discussionsDetails.getData().getFiles() != null) {
                            if (discussionsDetails.getData().getFiles().size() != 0) {
                                discussionsFile.clear();
                                discussionsFile.addAll(discussionsDetails.getData().getFiles());
                                discussionFIlePreviewListAdapter.notifyDataSetChanged();
                            }
                        }

                        mainisprivate = Boolean.parseBoolean(discussionsDetails.getData().getIsprivate());
                        discussionsDetailLayoutBinding.shareSwitch.setChecked(mainisprivate);
                        enableDisableView(false, false);
                        //hideDialog();
                        CallApiGetComments(topicId);
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

    private void CallApi() {
        h.postDelayed(new Runnable() {
            @Override
            public void run() {
                CallApiGetComments(topicId);
                h.postDelayed(this, 60000);
            }
        }, 60000);
    }

    private void CallApiGetComments(String topicId) {
        disposable.add(discussionRepository.GetComments(String.valueOf(pageNumber), perpagerecord, topicId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<GetAllComment>() {
                    @Override
                    public void onSuccess(GetAllComment getComment) {

                        pageNumber++;
                        commentList.addAll(getComment.getData());
                        discussionsCommentListAdapter.notifyDataSetChanged();
                        scrollListener.setLoaded();
                        hideDialog();
                        if (getComment.getData().size() == 0) {
                            isLoad = false;
                        }
                        discussionsDetailLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
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

    private void CallApiUpdateDiscussions(String topicId, String title, String description, boolean isprivate, ArrayList<Integer> filesidArraylist) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.id, topicId);
            jsonObject.put(Const.title, title);
            jsonObject.put(Const.description, description);
            jsonObject.put(Const.isprivate, isprivate);
            jsonObject.put(Const.filesid, new JSONArray(filesidArraylist));
            jsonObject.put(Const.courseid, GradeId);
            jsonObject.put(Const.ispublic, true);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(discussionRepository.UpdateDiscussionTopic(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AddDiscussionTopic>() {
                    @Override
                    public void onSuccess(AddDiscussionTopic addDiscussionTopic) {

                        Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                        discussionsDetailLayoutBinding.edtdiscussionsTopicName.setText(addDiscussionTopic.getData().getTitle());
                        showBackArrow(addDiscussionTopic.getData().getTitle());

                        if (!TextUtils.isEmpty(addDiscussionTopic.getData().getDescription())) {
                            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setText(addDiscussionTopic.getData().getDescription());
                        } else {
                            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setVisibility(View.GONE);
                        }
                        discussionsDetailLayoutBinding.shareSwitch.setChecked(Boolean.parseBoolean(addDiscussionTopic.getData().getIsprivate()));

                        enableDisableView(false, false);
                        mainClickFlag = false;
                        fileuploadlist.clear();
                        discussionsFile.clear();


                        if (addDiscussionTopic.getData().getFiles() != null) {
                            if (addDiscussionTopic.getData().getFiles().size() != 0) {

                                for (int i = 0; i < addDiscussionTopic.getData().getFiles().size(); i++) {
                                    DiscussionsDetails.Files discussionFile = new DiscussionsDetails.Files();
                                    discussionFile.setDuration(addDiscussionTopic.getData().getFiles().get(i).getDuration());
                                    discussionFile.setFileName(addDiscussionTopic.getData().getFiles().get(i).getFileName());
                                    discussionFile.setDescription(addDiscussionTopic.getData().getFiles().get(i).getDescription());
                                    discussionFile.setFileSize(addDiscussionTopic.getData().getFiles().get(i).getFileSize());
                                    discussionFile.setFileTypeId(addDiscussionTopic.getData().getFiles().get(i).getFileTypeId());
                                    discussionFile.setId(addDiscussionTopic.getData().getFiles().get(i).getId());
                                    discussionFile.setName(addDiscussionTopic.getData().getFiles().get(i).getName());
                                    discussionFile.setSignedUrl(addDiscussionTopic.getData().getFiles().get(i).getSignedUrl());
                                    discussionFile.setTopicId(addDiscussionTopic.getData().getFiles().get(i).getTopicId());
                                    discussionFile.setTotalPages(addDiscussionTopic.getData().getFiles().get(i).getTotalPages());
                                    discussionFile.setUrl(addDiscussionTopic.getData().getFiles().get(i).getUrl());
                                    discussionsFile.add(discussionFile);

                                }

                                discussionFIlePreviewListAdapter.notifyDataSetChanged();
                            }
                        }


                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();

                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }

                    }
                }));

    }

    private void CallApiDeleteDiscussions(String topicId) {

        showDialog(getString(R.string.loading));
        disposable.add(discussionRepository.DeleteTopic(topicId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UploadImageObject>() {
                    @Override
                    public void onSuccess(UploadImageObject uploadImageObject) {
                        hideDialog();
                        PrivousScreen();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();

                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));

    }

    @Override
    public void openDialogView(DiscussionsDetails.Files files, String filetypeid) {

    }

    public class setFileWithDataTask extends AsyncTask<Void, Void, ArrayList<Integer>> {

        asynDelegate delegate;

        public setFileWithDataTask(asynDelegate delegate) {
            this.delegate = delegate;
        }

        @Override
        protected void onPreExecute() {
            showDialog(getString(R.string.loading));
            super.onPreExecute();
        }

        @Override
        protected ArrayList<Integer> doInBackground(Void... params) {
            ArrayList<Integer> filesidArraylist = new ArrayList<>();

            for (int i = 0; i < fileuploadlist.size(); i++) {
                File file = new File(fileuploadlist.get(i));
                byte[] bitmapImage = new byte[(int) file.length()];
                if (bitmapImage != null) {
                    if (isNetworkAvailable(GeneralDiscussionsDetailActivity.this)) {
                        //Log.e("selectedFilePaths", "BITMAP" + bitmapImage);

                        RequestBody requestFile = RequestBody.create(MediaType.parse("*/*"), file);
                        MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
                        RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), "1");
                        RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
                        RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");

                        Call<UploadTopicFile> call = discussionRepository.UploadTopicFile(body, fileTypeId, duration, filesize);
                        call.enqueue(new Callback<UploadTopicFile>() {
                            @Override
                            public void onResponse(Call<UploadTopicFile> call, retrofit2.Response<UploadTopicFile> response) {

                                if (response.isSuccessful()) {
                                    UploadTopicFile uploadImageObject = response.body();
                                    filesidArraylist.add(Integer.valueOf(uploadImageObject.getFile_id()));
                                    delegate.asynDelegateMethod(filesidArraylist);
                                } else {
                                    ResponseBody errorBody = response.errorBody();
                                    Gson gson = new Gson();
                                    try {
                                        UploadTopicFile uploadTopicFile = gson.fromJson(errorBody.string(), UploadTopicFile.class);
                                        //Toast.makeText(getApplicationContext(), uploadTopicFile.getMessage(), Toast.LENGTH_LONG).show();

                                    } catch (Exception e) {
                                        e.printStackTrace();
                                        showError(e);
                                    }
                                }
                            }

                            @Override
                            public void onFailure(Call<UploadTopicFile> call, Throwable t) {
                                hideDialog();
                            }
                        });

                    }
                }
            }
            return filesidArraylist;
        }

        @Override
        protected void onPostExecute(ArrayList<Integer> strings) {

        }
    }

    public class setCommentFileWithDataTask extends AsyncTask<Void, Void, ArrayList<Integer>> {

        asynDelegate delegate;
        ArrayList<String> newfileuploadlist = new ArrayList<>();

        public setCommentFileWithDataTask(asynDelegate delegate, ArrayList<String> fileuploadlist) {
            this.delegate = delegate;
            this.newfileuploadlist = fileuploadlist;
        }

        @Override
        protected void onPreExecute() {
            showDialog(getString(R.string.loading));
            super.onPreExecute();
        }

        @Override
        protected ArrayList<Integer> doInBackground(Void... params) {

            ArrayList<Integer> filesidArraylist = new ArrayList<>();

            for (int i = 0; i < newfileuploadlist.size(); i++) {
                File file = new File(newfileuploadlist.get(i));
                if (isNetworkAvailable(GeneralDiscussionsDetailActivity.this)) {
                    RequestBody requestFile = RequestBody.create(MediaType.parse("*/*"), file);
                    MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
                    RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), "1");
                    RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
                    RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");

                    Call<UploadTopicFile> call = discussionRepository.UploadAddCommentFile(body, fileTypeId, duration, filesize);
                    call.enqueue(new Callback<UploadTopicFile>() {
                        @Override
                        public void onResponse(Call<UploadTopicFile> call, retrofit2.Response<UploadTopicFile> response) {

                            if (response.isSuccessful()) {
                                UploadTopicFile uploadImageObject = response.body();
                                filesidArraylist.add(Integer.valueOf(uploadImageObject.getFile_id()));
                                delegate.asynDelegateMethod(filesidArraylist);

                            } else {
                                ResponseBody errorBody = response.errorBody();
                                Gson gson = new Gson();
                                try {
                                    UploadTopicFile uploadTopicFile = gson.fromJson(errorBody.string(), UploadTopicFile.class);
                                    Toast.makeText(getApplicationContext(), uploadTopicFile.getMessage(), Toast.LENGTH_LONG).show();
                                    hideDialog();
                                } catch (Exception e) {
                                    e.printStackTrace();
                                    showError(e);
                                }
                            }
                        }

                        @Override
                        public void onFailure(Call<UploadTopicFile> call, Throwable t) {
                            hideDialog();
                        }
                    });

                }

            }
            return filesidArraylist;
        }

        @Override
        protected void onPostExecute(ArrayList<Integer> strings) {

        }
    }

    public interface asynDelegate {
        public void asynDelegateMethod(ArrayList<Integer> strings);
    }

    private void addFileChooserFragment(boolean clickflag) {

        discussionsDetailLayoutBinding.filechooserFragment.setVisibility(View.VISIBLE);

        builder = new FileChooser.Builder(FileChooser.ChooserType.FILE_CHOOSER,
                new FileChooser.ChooserListener() {
                    @Override
                    public void onSelect(String path) {
                        discussionsDetailLayoutBinding.filechooserFragment.setVisibility(View.GONE);
                        if (!TextUtils.isEmpty(path)) {
                            String[] selectedFilePaths = path.split(FileChooser.FILE_NAMES_SEPARATOR);

                            if (clickflag) {
                                mainClickFlag = true;
                                fileuploadlist.clear();
                                fileuploadlist.addAll(Arrays.asList(selectedFilePaths));

                                for (int i = 0; i < fileuploadlist.size(); i++) {
                                    DiscussionsDetails.Files discussionsFileModel = new DiscussionsDetails.Files();
                                    discussionsFileModel.setFileName(fileuploadlist.get(i));
                                    discussionsFile.add(discussionsFileModel);
                                }

                                discussionFIlePreviewListAdapter.notifyDataSetChanged();
                                discussionsDetailLayoutBinding.cardAddFilePicker.setClickable(false);
                                discussionsDetailLayoutBinding.cardAddFilePicker.setCardBackgroundColor(getResources().getColor(R.color.colormorelightGrayColor));
                            } else {
                                mainClickFlag = false;
                               /* fileUploadList.clear();
                                fileUploadList.addAll(Arrays.asList(selectedFilePaths));*/

                                ArrayList<String> fileuploadlist = new ArrayList<>();
                                fileuploadlist.addAll(Arrays.asList(selectedFilePaths));

                                new setCommentFileWithDataTask(new asynDelegate() {
                                    @Override
                                    public void asynDelegateMethod(ArrayList<Integer> strings) {
                                        CallApiCommentDiscussions(topicId, "", strings);
                                    }
                                }, fileuploadlist).execute();

                            }

                        } else {
                            Toast.makeText(GeneralDiscussionsDetailActivity.this, R.string.no_file_chosen, Toast.LENGTH_SHORT).show();
                        }
                    }
                })//.setMultipleFileSelectionEnabled(true)
                // Word document,PDF file,Powerpoint file,Excel file,GIF file,JPG file ,PNG file,Text file ,Video files
                .setFileFormats(new String[]{".noon", ".jpg", ".png", ".jpeg", ".gif", ".3gp", ".mpg", ".mpeg", ".mpe", ".mp4", ".avi", ".wav", ".mp3", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".docx", ".doc"});

        try {
            getSupportFragmentManager().beginTransaction().replace(R.id.filechooserFragment, builder.build()).commit();
        } catch (ExternalStorageNotAvailableException e) {
            Toast.makeText(this, R.string.no_external_storage, Toast.LENGTH_SHORT).show();
            e.printStackTrace();
        }
    }

    @Override
    public void onBackPressed() {
        /*Fragment f = getSupportFragmentManager().findFragmentById(R.id.filechooserFragment);
        if (f instanceof FileChooser) {
            discussionsDetailLayoutBinding.filechooserFragment.setVisibility(View.GONE);
            getSupportFragmentManager().beginTransaction()
                    .remove(f)
                    .commit();
        } else {
            finish();
        }*/

        PrivousScreen();
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

    public void PrivousScreen() {
        Intent i = new Intent(getApplicationContext(), MainDashBoardActivity.class);
        i.putExtra(Const.isDiscussions, true);
        startActivity(i);
        finish();
    }


}
