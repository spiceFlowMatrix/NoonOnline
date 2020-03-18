package com.ibl.apps.noon;

import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Dialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.databinding.DataBindingUtil;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.support.design.widget.TextInputLayout;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.LocalBroadcastManager;
import android.support.v7.widget.LinearLayoutManager;
import android.text.SpannableStringBuilder;
import android.text.TextUtils;
import android.text.method.KeyListener;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.CompoundButton;
import android.widget.LinearLayout;
import android.widget.Toast;

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
import com.ibl.apps.Adapter.DiscussionFIlePreviewListAdapter;
import com.ibl.apps.Adapter.DiscussionsCommentListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Interface.ViewDiscussionsFiles;
import com.ibl.apps.Model.AddComment;
import com.ibl.apps.Model.AddDiscussionTopic;
import com.ibl.apps.Model.DiscussionsDetails;
import com.ibl.apps.Model.EncryptDecryptObject;
import com.ibl.apps.Model.GetAllComment;
import com.ibl.apps.Model.TopicLike;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UploadTopicFile;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.Utils.LoadMoreData.OnLoadMoreListener;
import com.ibl.apps.Utils.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.Utils.PrefUtils;
import com.ibl.apps.Utils.TimeAgoClass;
import com.ibl.apps.Utils.Validator;
import com.ibl.apps.Utils.VideoEncryptDecrypt.EncrypterDecryptAlgo;
import com.ibl.apps.noon.databinding.AssignmentfilesItemLayoutBinding;
import com.ibl.apps.noon.databinding.DiscussionsDetailLayoutBinding;
import com.ibl.apps.noon.databinding.PdfviewNewLayoutBinding;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Objects;
import java.util.TimeZone;

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
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.HttpException;

import static com.ibl.apps.Utils.Const.GradeID;


public class DiscussionsDetailActivity extends BaseActivity implements View.OnClickListener, ViewDiscussionsFiles {

    DiscussionsDetailLayoutBinding discussionsDetailLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();
    private final static int READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE = 13;
    String topicId, topicname;
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
    byte[] iv;
    AlgorithmParameterSpec paramSpec;
    SecretKey key;
    SecretKey keyFromKeydata;
    byte[] keyData;
    JsonObject jsonObject = new JsonObject();
    String profileurl, userName, cretedTime;
    private boolean iseditable, isLiked, isDisliked;
    private int likeCounts, dislikeCount;

    @Override
    protected int getContentView() {
        return R.layout.discussions_detail_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        discussionsDetailLayoutBinding = (DiscussionsDetailLayoutBinding) getBindObj();

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
            profileurl = getIntent().getStringExtra(Const.profileurl);
            userName = getIntent().getStringExtra(Const.userName);
            cretedTime = getIntent().getStringExtra(Const.createdtime);
            iseditable = getIntent().getBooleanExtra(Const.iseditable, false);
            likeCounts = getIntent().getIntExtra(Const.likecount, 0);
            dislikeCount = getIntent().getIntExtra(Const.dislikecount, 0);
            isLiked = getIntent().getBooleanExtra(Const.liked, false);
            isDisliked = getIntent().getBooleanExtra(Const.disliked, false);


            PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
                @Override
                public UserDetails getLocalUserDetails(UserDetails userDetails) {
                    if (userDetails != null) {
                        userDetailsObject = userDetails;
                        userId = userDetailsObject.getId();
                        if (isNetworkAvailable(getApplicationContext())) {
                            CallApiDiscussionsDetails(topicId);
                        } else {
                            showNetworkAlert(DiscussionsDetailActivity.this);
                        }
                    }
                    return null;
                }

            }).execute();
        }

        try {
            key = new SecretKeySpec(Const.ALGO_SECRATE_KEY_NAME.getBytes(), Const.ALGO_SECRET_KEY_GENERATOR);
            keyData = key.getEncoded();
            keyFromKeydata = new SecretKeySpec(keyData, 0, keyData.length, Const.ALGO_SECRET_KEY_GENERATOR); //if you want to store key bytes to db so its just how to //recreate back key from bytes array
            iv = new byte[Const.IV_LENGTH];
            paramSpec = new IvParameterSpec(iv);

        } catch (Exception e) {
            e.printStackTrace();
        }

        setToolbar(discussionsDetailLayoutBinding.toolBar);
        showBackArrow(getString(R.string.item_4));

        if (isLiked) {
            discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up);
            discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
        } else if (isDisliked) {
            discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
            discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down_sky);
        } else {
            discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
            discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
        }
        discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
        discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));

        if (iseditable) {
            discussionsDetailLayoutBinding.discussionsDelete.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.VISIBLE);
        } else {
            discussionsDetailLayoutBinding.discussionsDelete.setVisibility(View.GONE);
            discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.GONE);
        }
        setOnClickListener();
        CallApiGetComments(topicId);
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
        discussionFIlePreviewListAdapter = new DiscussionFIlePreviewListAdapter(DiscussionsDetailActivity.this, discussionsFile, userDetailsObject, enableDisabl, this);
        discussionsDetailLayoutBinding.rcVerticalLay.rcVertical.setAdapter(discussionFIlePreviewListAdapter);

        LocalBroadcastManager.getInstance(this).registerReceiver(mMessageReceiver, new IntentFilter(getString(R.string.brodcastTAG)));

    }

    private BroadcastReceiver mMessageReceiver = new BroadcastReceiver() {
        @Override
        public void onReceive(Context context, Intent intent) {
            String topid = intent.getStringExtra(Const.topicId);
            if (topicId.equals(topid)) {
                commentList.clear();
                pageNumber = 1;
                CallApiGetComments(topid);
            }
        }
    };

    @SuppressLint("ClickableViewAccessibility")
    public void setOnClickListener() {

        if (userName != null && !userName.isEmpty()) {
            discussionsDetailLayoutBinding.txtUsername.setText(userName);
        } else {
            discussionsDetailLayoutBinding.txtUsername.setText(getString(R.string.user));
        }

        discussionsDetailLayoutBinding.txtTime.setText(cretedTime);

        GlideApp.with(NoonApplication.getContext())
                .load(profileurl)
                .placeholder(R.drawable.profile)
                .error(R.drawable.profile)
                .into(discussionsDetailLayoutBinding.commentprofileImage);

        discussionsDetailLayoutBinding.discussionsDelete.setOnClickListener(this);
        discussionsDetailLayoutBinding.discussionsEdit.setOnClickListener(this);
        discussionsDetailLayoutBinding.discussionsCheckEdit.setOnClickListener(this);
        discussionsDetailLayoutBinding.cardCommentpicker.setOnClickListener(this);
        discussionsDetailLayoutBinding.cardAddFilePicker.setOnClickListener(this);
        discussionsDetailLayoutBinding.thumbUpImage.setOnClickListener(this);
        discussionsDetailLayoutBinding.thumbDownImage.setOnClickListener(this);
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

            //editable (editscreen)
            TextInputLayout.LayoutParams deslayoutParams = new LinearLayout.LayoutParams(TextInputLayout.LayoutParams.MATCH_PARENT, TextInputLayout.LayoutParams.WRAP_CONTENT);
            deslayoutParams.setMargins((int) getResources().getDimension(R.dimen._10sdp), 0, 0, 0);
            discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setLayoutParams(deslayoutParams);

            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setKeyListener((KeyListener) discussionsDetailLayoutBinding.edtdiscussionsTopicName.getTag());

            TextInputLayout.LayoutParams namelayoutParams = new LinearLayout.LayoutParams(TextInputLayout.LayoutParams.MATCH_PARENT, TextInputLayout.LayoutParams.WRAP_CONTENT);
            namelayoutParams.setMargins((int) getResources().getDimension(R.dimen._10sdp), 0, 0, 0);
            discussionsDetailLayoutBinding.discussionsTopicNameWrapper.setLayoutParams(namelayoutParams);
            discussionsDetailLayoutBinding.edtdiscussionsTopicName.setKeyListener((KeyListener) discussionsDetailLayoutBinding.edtdiscussionsTopicName.getTag());

            discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setText(discussionsDetailLayoutBinding.txtDescription.getText().toString().trim());
            discussionsDetailLayoutBinding.txtDescription.setVisibility(View.GONE);

            discussionsDetailLayoutBinding.userInfoLay.setVisibility(View.GONE);
            discussionsDetailLayoutBinding.profileLay.setVisibility(View.GONE);
            if (!iseditable) {
                discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.GONE);
            }

            discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.GONE);
            discussionsDetailLayoutBinding.discussionsCheckEdit.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.edtdiscussionsTopicName.setEnabled(true);
            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setEnabled(true);
            discussionsDetailLayoutBinding.edtdiscussionsTopicName.requestFocus();
            discussionsDetailLayoutBinding.discussionsTopicNameWrapper.setHintEnabled(true);
            discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setHintEnabled(true);
            discussionsDetailLayoutBinding.edtdiscussionsTopicName.setBackground(getResources().getDrawable(R.drawable.text_box_underline_activated));
            discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setBackground(getResources().getDrawable(R.drawable.text_box_underline_activated));
            discussionsDetailLayoutBinding.laySwitch.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.cardAddFilePicker.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.cardpickerLay.setVisibility(View.VISIBLE);
            discussionsDetailLayoutBinding.cardAddFilePicker.setClickable(true);
            discussionsDetailLayoutBinding.cardAddFilePicker.setCardBackgroundColor(getResources().getColor(R.color.colorGreen));

        } else {
            if (callApiEdit) {
                String title = Objects.requireNonNull(discussionsDetailLayoutBinding.edtdiscussionsTopicName.getText()).toString().trim();
//                String description = Objects.requireNonNull(discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.getText()).toString().trim();
                String description = Objects.requireNonNull(discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.getText()).toString().trim();


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
                                showNetworkAlert(DiscussionsDetailActivity.this);
                            }
                        }
                    }
                } else {
                    if (validateFields()) {
                        if (isNetworkAvailable(this)) {
                            CallApiUpdateDiscussions(topicId, title, description, mainisprivate, new ArrayList<>());
                        } else {
                            showNetworkAlert(DiscussionsDetailActivity.this);
                        }
                    }
                }

            } else {
                if (Build.VERSION.SDK_INT <= Build.VERSION_CODES.KITKAT) {
                    TextInputLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(TextInputLayout.LayoutParams.MATCH_PARENT, TextInputLayout.LayoutParams.WRAP_CONTENT);
                    layoutParams.setMargins(0, (int) getResources().getDimension(R.dimen._minus5sdp), 0, 0);
                    discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setLayoutParams(layoutParams);

                    discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setMaxLines(2);
                    discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setEllipsize(TextUtils.TruncateAt.END);
                } else {
                    TextInputLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(TextInputLayout.LayoutParams.MATCH_PARENT, TextInputLayout.LayoutParams.WRAP_CONTENT);
                    layoutParams.setMargins(0, (int) getResources().getDimension(R.dimen._minus20sdp), 0, 0);
                    discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setLayoutParams(layoutParams);

                    discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setTag(discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.getKeyListener());
                    discussionsDetailLayoutBinding.edtddiscussionsTopicDescription.setKeyListener(null);
                }

                TextInputLayout.LayoutParams namelayoutParams = new LinearLayout.LayoutParams(TextInputLayout.LayoutParams.MATCH_PARENT, TextInputLayout.LayoutParams.WRAP_CONTENT);
                namelayoutParams.setMargins(0, 0, 0, 0);
                discussionsDetailLayoutBinding.discussionsTopicNameWrapper.setLayoutParams(namelayoutParams);
                discussionsDetailLayoutBinding.edtdiscussionsTopicName.setTag(discussionsDetailLayoutBinding.edtdiscussionsTopicName.getKeyListener());
                discussionsDetailLayoutBinding.edtdiscussionsTopicName.setKeyListener(null);

                discussionsDetailLayoutBinding.userInfoLay.setVisibility(View.VISIBLE);
                if (iseditable) {
                    discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.VISIBLE);
                }

//                discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.VISIBLE);
                discussionsDetailLayoutBinding.txtDescription.setVisibility(View.VISIBLE);
                discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setVisibility(View.GONE);
                discussionsDetailLayoutBinding.profileLay.setVisibility(View.VISIBLE);
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
                discussionsDetailLayoutBinding.cardpickerLay.setVisibility(View.VISIBLE);
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
                showDeleteAlert(DiscussionsDetailActivity.this);
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

            case R.id.thumbUpImage:
                if (isLiked) {
                    jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.topicid, topicId);
                    jsonObject.addProperty(Const.like, false);
                    jsonObject.addProperty(Const.dislike, false);
                    callApiLikeDislike(jsonObject);
                } else {
                    jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.topicid, topicId);
                    jsonObject.addProperty(Const.like, true);
                    jsonObject.addProperty(Const.dislike, false);
                    callApiLikeDislike(jsonObject);
                }
                break;
            case R.id.thumbDownImage:
                if (isDisliked) {
                    jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.topicid, topicId);
                    jsonObject.addProperty(Const.like, false);
                    jsonObject.addProperty(Const.dislike, false);
                    callApiLikeDislike(jsonObject);
                } else {
                    jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.topicid, topicId);
                    jsonObject.addProperty(Const.like, false);
                    jsonObject.addProperty(Const.dislike, true);
                    callApiLikeDislike(jsonObject);
                }
                break;
        }
    }

    private void callApiLikeDislike(JsonObject jsonObject) {
        disposable.add(apiService.getDiscussionTopicLike(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<TopicLike>() {
                    @Override
                    public void onSuccess(TopicLike topicLike) {
                        if (topicLike.getResponse_code().equals("0")) {

                            if ((isLiked && jsonObject.get(Const.like).getAsBoolean()) || ((isDisliked && jsonObject.get(Const.dislike).getAsBoolean())))
                                return;
                            if (!jsonObject.get(Const.like).getAsBoolean() && !jsonObject.get(Const.dislike).getAsBoolean()) {
                                if (isDisliked) {
                                    dislikeCount--;
                                    isDisliked = false;

                                    discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
                                } else if (isLiked) {
                                    likeCounts--;
                                    isLiked = false;

                                    discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
                                }
                            } else if (jsonObject.get(Const.like).getAsBoolean()) {
                                likeCounts++;
                                if (isDisliked) {
                                    dislikeCount--;
                                }
                                isDisliked = false;
                                isLiked = true;
                                discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up);
                                discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
                            } else {
                                dislikeCount++;
                                if (isLiked) {
                                    likeCounts--;
                                }
                                isDisliked = true;
                                isLiked = false;
                                discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
                                discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down_sky);
                            }
                            discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
                            discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));

//                            if (jsonObject.get(Const.like).getAsBoolean()) {
//
//
//
//                                /*if (isLiked) {
//                                    likeCountdup = likeCounts + 1;
//                                    discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCountdup));
//                                    if (isDisliked) {
//                                        dislikeCountdup = dislikeCount - 1;
//                                        discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCountdup));
//                                    } else if (!isLiked && !isDisliked) {
//                                        if (dislikeCount!=0) {
//                                            dislikeCountdup = dislikeCount - 1;
//                                            discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCountdup));
//                                        }else {
//                                            discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
//                                        }
//                                    }
//                                } else {
//                                    discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
//                                    discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
//                                }*/
//                                discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up);
//                                discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
//                            } else {
//
//                                if (isLiked || isDisliked){
//                                    if (isLiked){
//                                        likeCounts ++;
//                                        dislikeCount --;
//                                    }else {
//                                        likeCounts --;
//                                        dislikeCount ++;
//                                    }
//                                    discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
//                                    discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
//                                }else {
//                                    if (isLiked){
//                                        likeCounts ++;
//                                    }else {
//                                        dislikeCount ++;
//                                    }
//                                    discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
//                                    discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
//                                }
//                                //                                if (!isDisliked) {
////                                    dislikeCountdup = dislikeCount + 1;
////                                    discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCountdup));
////                                    if (isLiked) {
////                                        likeCountdup = likeCounts - 1;
////                                        discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCountdup));
////                                    }else if (!isLiked && !isDisliked) {
////                                        if (likeCounts!=0) {
////                                            likeCountdup = dislikeCount - 1;
////                                            discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCountdup));
////                                        }else {
////                                            discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
////                                        }
////                                    }
////                                } else {
////                                    discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
////                                    discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
////                                }
//                                discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down_sky);
//                                discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
//                            }
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        Log.e("onError", "onError: " + e.getMessage());
                    }
                }));
    }


    public void showDeleteAlert(Context activity) {
        try {
            SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.validation_delete));
            android.support.v7.app.AlertDialog.Builder builder = new android.support.v7.app.AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                            if (isNetworkAvailable(activity)) {
                                CallApiDeleteDiscussions(topicId);
                            } else {
                                showNetworkAlert(DiscussionsDetailActivity.this);
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
            jsonObject.put(Const.ispublic, false);


        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(apiService.AddComment(gsonObject)
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
                            AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), AddDiscussionTopic.class);
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
        disposable.add(apiService.DiscussionsDetails(topicId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DiscussionsDetails>() {
                    @Override
                    public void onSuccess(DiscussionsDetails discussionsDetails) {
                        if (discussionsDetails.getData() != null) {

                            TimeAgoClass timeAgo = new TimeAgoClass();
                            String getDate = getDate(discussionsDetails.getData().getCreateddate());
                            String timeagotxt = timeAgo.covertTimeToText(getDate);
                            discussionsDetailLayoutBinding.txtTime.setText(timeagotxt);

                            discussionsDetailLayoutBinding.edtdiscussionsTopicName.setText(topicname);
                            if (!TextUtils.isEmpty(discussionsDetails.getData().getDescription())) {
                                discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setVisibility(View.GONE);
                                discussionsDetailLayoutBinding.txtDescription.setVisibility(View.VISIBLE);
                                discussionsDetailLayoutBinding.txtDescription.setText(discussionsDetails.getData().getDescription());
                            }
                            userName = discussionsDetails.getData().getUser().getUsername();
                            profileurl = discussionsDetails.getData().getUser().getProfilepicurl();
                            if (userName != null && !userName.isEmpty()) {
                                discussionsDetailLayoutBinding.txtUsername.setText(userName);
                            } else {
                                discussionsDetailLayoutBinding.txtUsername.setText(getString(R.string.user));
                            }


                            GlideApp.with(NoonApplication.getContext())
                                    .load(profileurl)
                                    .placeholder(R.drawable.profile)
                                    .error(R.drawable.profile)
                                    .into(discussionsDetailLayoutBinding.commentprofileImage);

                            if (discussionsDetails.getData().isLiked()) {
                                discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up);
                                discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
                            } else if (discussionsDetails.getData().isDisliked()) {
                                discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
                                discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down_sky);
                            } else {
                                discussionsDetailLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
                                discussionsDetailLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
                            }
                            discussionsDetailLayoutBinding.txtLikeCount.setText(String.valueOf(discussionsDetails.getData().getLikecount()));
                            discussionsDetailLayoutBinding.txtDisLikeCount.setText(String.valueOf(discussionsDetails.getData().getDislikecount()));

                            if (discussionsDetails.getData().isIseditable()) {
                                discussionsDetailLayoutBinding.discussionsDelete.setVisibility(View.VISIBLE);
                                discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.VISIBLE);
                            } else {
                                discussionsDetailLayoutBinding.discussionsDelete.setVisibility(View.GONE);
                                discussionsDetailLayoutBinding.discussionsEdit.setVisibility(View.GONE);
                            }
                            cretedTime = timeagotxt;
                            iseditable = discussionsDetails.getData().isIseditable();
                            isLiked = discussionsDetails.getData().isLiked();
                            isDisliked = discussionsDetails.getData().isDisliked();
                            likeCounts = discussionsDetails.getData().getLikecount();
                            dislikeCount = discussionsDetails.getData().getDislikecount();

                            if (discussionsDetails.getData().getFiles() != null) {
                                if (discussionsDetails.getData().getFiles().size() != 0) {
                                    discussionsFile.clear();
                                    discussionsFile.addAll(discussionsDetails.getData().getFiles());
                                    discussionFIlePreviewListAdapter.notifyDataSetChanged();
                                }
                            }
                            mainisprivate = Boolean.parseBoolean(discussionsDetails.getData().getIsprivate());
                            discussionsDetailLayoutBinding.shareSwitch.setChecked(mainisprivate);

                        } else {
                            discussionsDetailLayoutBinding.edtdiscussionsTopicName.setText(topicname);

                        }
                        enableDisableView(false, false);
                        //hideDialog();
                        CallApiGetComments(topicId);
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            DiscussionsDetails discussionsDetails = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), DiscussionsDetails.class);
                            Toast.makeText(getApplicationContext(), discussionsDetails.getMessage(), Toast.LENGTH_SHORT).show();

                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    private String getDate(String ourDate) {
        try {
            SimpleDateFormat formatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss aaa", Locale.ENGLISH);
            formatter.setTimeZone(TimeZone.getTimeZone("UTC"));
            Date value = formatter.parse(ourDate);

            SimpleDateFormat dateFormatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH); //this format changeable
            dateFormatter.setTimeZone(TimeZone.getDefault());
            ourDate = dateFormatter.format(value);

        } catch (Exception e) {
            ourDate = "00-00-0000 00:00";
        }
        return ourDate;
    }

    private void CallApiGetComments(String topicId) {
        disposable.add(apiService.GetComments(String.valueOf(pageNumber), perpagerecord, topicId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<GetAllComment>() {
                    @Override
                    public void onSuccess(GetAllComment getComment) {
                        if (getComment != null && getComment.getData() != null) {
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
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            DiscussionsDetails discussionsDetails = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), DiscussionsDetails.class);
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
            jsonObject.put(Const.ispublic, false);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(apiService.UpdateDiscussionTopic(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AddDiscussionTopic>() {
                    @Override
                    public void onSuccess(AddDiscussionTopic addDiscussionTopic) {

                        Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                        discussionsDetailLayoutBinding.edtdiscussionsTopicName.setText(addDiscussionTopic.getData().getTitle());
                        showBackArrow(getString(R.string.item_4));

                        if (!TextUtils.isEmpty(addDiscussionTopic.getData().getDescription())) {
                            discussionsDetailLayoutBinding.discussionsTopicDescriptionWrapper.setVisibility(View.GONE);
                            discussionsDetailLayoutBinding.txtDescription.setVisibility(View.VISIBLE);
                            discussionsDetailLayoutBinding.txtDescription.setText(addDiscussionTopic.getData().getDescription());
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
                            AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), AddDiscussionTopic.class);
                            Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                        } catch (Exception e1) {
                            showError(e);
                        }

                    }
                }));

    }

    private void CallApiDeleteDiscussions(String topicId) {

        showDialog(getString(R.string.loading));
        disposable.add(apiService.DeleteTopic(topicId)
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
                            AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), AddDiscussionTopic.class);
                            Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();

                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));

    }

    @Override
    public void openDialogView(DiscussionsDetails.Files model, String filetypeid) {
        final Dialog dialog = new Dialog(DiscussionsDetailActivity.this, android.R.style.Theme_Translucent_NoTitleBar);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        //dialogimage.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
        dialog.setCancelable(true);
        AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(DiscussionsDetailActivity.this), R.layout.assignmentfiles_item_layout, null, false);
        dialog.setContentView(assignmentfilesItemLayoutBinding.getRoot());
        String filename = model.getFileName();
        String[] strings = filename.split("\\.");
        String extension = strings[1];
        new DownloadStatusTask(DiscussionsDetailActivity.this, model.getUrl(), model, 1, extension, dialog, assignmentfilesItemLayoutBinding).execute();
        // callApifetchAssignmentSignedUrl(model.getId(), "0", model, extension, dialog, assignmentfilesItemLayoutBinding);

    }

    public class DownloadStatusTask extends AsyncTask<String, String, String> {

        String fileURL;
        DiscussionsDetails.Files model;
        private int type;
        private String extension;
        private Dialog dialog;
        private AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding;
        int downloadId = 0;
        long progressval = 0;
        Context context;


        public DownloadStatusTask(Context context, String url, DiscussionsDetails.Files model, int type, String extension, Dialog dialog, AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding) {
            this.context = context;
            this.fileURL = url;
            this.model = model;
            this.type = type;
            this.extension = extension;
            this.dialog = dialog;
            this.assignmentfilesItemLayoutBinding = assignmentfilesItemLayoutBinding;
        }

        @Override
        protected String doInBackground(String... strings) {
            String userIdSlash = "";

            String EncryptFileName = model.getFileName();
            String filetype = model.getFileTypeId();
            String fileName = model.getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);
            String downloadFilePath = Const.destPath + userIdSlash + fileName;

            try {

                String str = model.getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
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

                                final Dialog dialogpdf = new Dialog(context, android.R.style.Theme_Translucent_NoTitleBar);
                                dialogpdf.requestWindowFeature(Window.FEATURE_NO_TITLE);
                                Objects.requireNonNull(dialogpdf.getWindow()).setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                                dialogpdf.setCancelable(true);
                                PdfviewNewLayoutBinding pdfviewNewLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(context), R.layout.pdfview_new_layout, null, false);
                                dialogpdf.setContentView(pdfviewNewLayoutBinding.getRoot());
                                pdfviewNewLayoutBinding.pdfviewLay.setVisibility(View.VISIBLE);
                                pdfviewNewLayoutBinding.pdfCourseName.setText("");
                                pdfviewNewLayoutBinding.pdflessonName.setText("");
                                openPdf(pdfviewNewLayoutBinding, dialogpdf, model);

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
                                    //showNoSpaceAlert(ctx);
                                }
                                //hideDialog();
                                Crashlytics.log(Log.ERROR, DiscussionsDetailActivity.this.getString(R.string.app_name), error.getError());
                            }
                        });

            } catch (Exception e) {

            }
            return null;
        }

        @Override
        protected void onPostExecute(String unused) {
        }
    }

    private void openPdf(PdfviewNewLayoutBinding dialogViewerItemLayoutBinding, Dialog dialog, DiscussionsDetails.Files model) {

        String yourFilePath = DiscussionsDetailActivity.this.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath();
        String EncryptFileName = model.getFileName();
        String filetype = model.getFileTypeId();
        String str = model.getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
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
                    Log.e("Dialog", "-----");
                    File file = new File(DiscussionsDetailActivity.this.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath() + File.separator + Const.dir_fileName + Const.PDFextension);
                    Log.e("Dialog", "--FileData---" + file);
                    dialogViewerItemLayoutBinding.pdfViewPager.fromFile(file)
                            .enableSwipe(true)
                            .enableDoubletap(true)
                            .defaultPage(0)
                            .onLoad(new OnLoadCompleteListener() {
                                @Override
                                public void loadComplete(int i) {
                                    dialogViewerItemLayoutBinding.progressDialogLay.placeholder.setVisibility(View.GONE);
                                }
                            })
                            .onPageChange(new OnPageChangeListener() {
                                @Override
                                public void onPageChanged(int page, int pageCount) {
                                    dialogViewerItemLayoutBinding.txtPageCount.setText(DiscussionsDetailActivity.this.getString(R.string.page) + " " + (page + 1) + " " + DiscussionsDetailActivity.this.getString(R.string.of) + "  " + pageCount);
                                    int countper = (int) ((page + 1) * 100 / pageCount);
                                    dialogViewerItemLayoutBinding.progressBarLayout.progressBar.setProgress(countper);
//                                             if (currProgress[0] < countper) {
//                                                new CourseItemFragment.ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid).execute();
//                                                currProgress[0] = countper;
//                                                newcurrantProgress = countper;
//                                            }
//
//                                            TotalPDFpage = pageCount;
//                                            SelectPDFpage = page;
                                }
                            })
                            .enableAnnotationRendering(true)
                            .swipeHorizontal(true)
                            .onRender(new OnRenderListener() {
                                @Override
                                public void onInitiallyRendered(int i) {
                                    dialogViewerItemLayoutBinding.pdfViewPager.fitToWidth(dialogViewerItemLayoutBinding.pdfViewPager.getCurrentPage());
                                }
                            })
                            .enableAntialiasing(true)
                            .password(null)
                            .load();
                }

                @Override
                public void onError(Error error) {
                    //Log.e(Const.LOG_NOON_TAG, "0000" + "NO Space ----" + error.toString());
                }
            });
        } catch (Exception e) {
            e.printStackTrace();

        }

//                dialogViewerItemLayoutBinding.pdfviewLay.setVisibility(View.VISIBLE);
//                dialogViewerItemLayoutBinding.pdfCourseName.setText(CourseName);
//                dialogViewerItemLayoutBinding.pdflessonName.setText(LessonName);
        dialogViewerItemLayoutBinding.backPdfButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialog.dismiss();
            }
        });

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
                    if (isNetworkAvailable(DiscussionsDetailActivity.this)) {
                        //Log.e("selectedFilePaths", "BITMAP" + bitmapImage);

                        RequestBody requestFile = RequestBody.create(MediaType.parse("*/*"), file);
                        MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
                        RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), "1");
                        RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
                        RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");

                        Call<UploadTopicFile> call = apiService.UploadTopicFile(body, fileTypeId, duration, filesize);
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
                if (isNetworkAvailable(DiscussionsDetailActivity.this)) {
                    RequestBody requestFile = RequestBody.create(MediaType.parse("*/*"), file);
                    MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
                    RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), "1");
                    RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
                    RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");

                    Call<UploadTopicFile> call = apiService.UploadAddCommentFile(body, fileTypeId, duration, filesize);
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
                            Toast.makeText(DiscussionsDetailActivity.this, R.string.no_file_chosen, Toast.LENGTH_SHORT).show();
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
        Intent i = new Intent(getApplicationContext(), ChapterActivity.class);
        i.putExtra(GradeID, GradeId);
        i.putExtra(Const.CourseName, CourseName);
        i.putExtra(Const.ActivityFlag, ActivityFlag);
        i.putExtra(Const.LessonID, LessonID);
        i.putExtra(Const.QuizID, QuizID);
        i.putExtra(Const.isDiscussions, true);
        i.putExtra(Const.AddtionalLibrary, AddtionalLibrary);
        i.putExtra(Const.AddtionalAssignment, AddtionalAssignment);
        i.putExtra(Const.AddtionalDiscussions, AddtionalDiscussions);
        startActivity(i);
        finish();
    }


}
