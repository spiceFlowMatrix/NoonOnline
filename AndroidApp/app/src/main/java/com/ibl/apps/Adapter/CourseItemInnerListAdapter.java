package com.ibl.apps.Adapter;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import androidx.databinding.DataBindingUtil;
import android.graphics.PorterDuff;
import android.graphics.Typeface;
import android.graphics.drawable.Drawable;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Handler;
import androidx.annotation.NonNull;
import androidx.annotation.RequiresApi;
import androidx.core.content.ContextCompat;
import androidx.core.content.res.ResourcesCompat;
import androidx.appcompat.app.AlertDialog;
import androidx.recyclerview.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.auth0.android.Auth0;
import com.auth0.android.authentication.AuthenticationAPIClient;
import com.auth0.android.authentication.AuthenticationException;
import com.auth0.android.callback.BaseCallback;
import com.auth0.android.result.Credentials;
import com.crashlytics.android.Crashlytics;
import com.downloader.Error;
import com.downloader.OnCancelListener;
import com.downloader.OnDownloadListener;
import com.downloader.OnPauseListener;
import com.downloader.OnProgressListener;
import com.downloader.OnStartOrResumeListener;
import com.downloader.PRDownloader;
import com.downloader.Progress;
import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.google.gson.Gson;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Fragment.CourseItemFragment;
import com.ibl.apps.Interface.CourseHideResponse;
import com.ibl.apps.Interface.CourseInnerItemInterface;
import com.ibl.apps.Interface.EncryptDecryptAsyncResponse;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.DownloadProgress;
import com.ibl.apps.Model.DownloadQueueObject;
import com.ibl.apps.Model.EncryptDecryptObject;
import com.ibl.apps.Model.IntervalTableObject;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.Model.SignedUrlObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.ChapterProgress;
import com.ibl.apps.RoomDatabase.entity.FileDownloadStatus;
import com.ibl.apps.RoomDatabase.entity.FileProgress;
import com.ibl.apps.RoomDatabase.entity.LessonNewProgress;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.QuizProgress;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.util.JWTUtils;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.VideoEncryptDecrypt.EncrypterDecryptAlgo;
import com.ibl.apps.noon.AssignmentDetailActivity;
import com.ibl.apps.noon.MainDashBoardActivity;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CourseInnerItemLayoutBinding;
import com.ibl.apps.noon.databinding.DeletePopupLayoutBinding;
import com.ibl.apps.noon.databinding.DownloadPopupLayoutBinding;

import org.json.JSONObject;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.Comparator;
import java.util.Date;
import java.util.Locale;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.TimeUnit;

import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import pub.devrel.easypermissions.EasyPermissions;

import static android.view.View.GONE;
import static android.view.View.VISIBLE;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class CourseItemInnerListAdapter extends RecyclerView.Adapter<CourseItemInnerListAdapter.MyViewHolder> implements View.OnClickListener, DroidListener {

    ArrayList<CoursePriviewObject.Lessons> list;
    int lessonProg = 0;
    CourseInnerItemInterface courseInnerItemInterface;
    CourseHideResponse courseHideResponse;
    boolean isisLastItemViewed;
    boolean isPlayFirstItem;
    Context ctx;
    File inFile;
    File outFile;
    SecretKey key;
    byte[] keyData;
    byte[] iv;
    String encrypted_path;
    String selectedVideoPath;
    SecretKey keyFromKeydata;
    AlgorithmParameterSpec paramSpec;
    private CompositeDisposable disposable = new CompositeDisposable();
    public static ApiService apiService;
    private ArrayList<CoursePriviewObject.Assignment> assignments;
    private String Coursename;
    private String gradeId;
    String chapterid;
    UserDetails userObject;
    String userIdSlash = "";
    String userId = "";
    String activityFlag = "";
    String lessonID = "";
    String QuizID = "";
    Boolean isNotification = false;
    ProgressDialog mProgressDialog;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;
    public static boolean isdialog = false;
    int chapterPro;
    //private HashMap<String, ArrayList<DownloadQueueObject>> hashMap = new HashMap<>();
    private ArrayList<String> fileid = new ArrayList<>();
    private Timer t = new Timer();
    private java.util.TimerTask mTimerTask;
    private int second = 0;
    private Handler handler = new Handler();
    public static ArrayList<FileProgress> fileProgressList = new ArrayList<>();
    public static ArrayList<QuizProgress> quizProgressList = new ArrayList<>();
    public static ArrayList<LessonNewProgress> lessonProgressList = new ArrayList<>();
    public static ArrayList<ChapterProgress> chapterProgressList = new ArrayList<>();


    public class MyViewHolder extends RecyclerView.ViewHolder {
        public CourseInnerItemLayoutBinding courseInnerItemLayoutBinding;

        public MyViewHolder(CourseInnerItemLayoutBinding itemhistorylayoutBinding) {
            super(itemhistorylayoutBinding.getRoot());
            this.courseInnerItemLayoutBinding = itemhistorylayoutBinding;
            courseInnerItemLayoutBinding.executePendingBindings();
        }
    }

    public CourseItemInnerListAdapter(Context ctx, ArrayList<CoursePriviewObject.Lessons> list, CourseInnerItemInterface courseInnerItemInterface, boolean isisLastItemViewed, boolean isPlayFirstItem, String chapterid, UserDetails userDetailsObject, String activityFlag, String lessonID, String QuizID, Boolean isNotification, CourseHideResponse courseHideResponse, ArrayList<CoursePriviewObject.Assignment> assignments, String Coursename, String gradeId) {
        this.list = list;
        this.ctx = ctx;
        this.isisLastItemViewed = isisLastItemViewed;
        this.isPlayFirstItem = isPlayFirstItem;
        this.courseInnerItemInterface = courseInnerItemInterface;
        this.courseHideResponse = courseHideResponse;
        apiService = ApiClient.getClient(ctx).create(ApiService.class);
        this.assignments = assignments;
        this.Coursename = Coursename;
        this.gradeId = gradeId;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
        this.chapterid = chapterid;
        this.userObject = userDetailsObject;
        if (userObject != null) {
            userIdSlash = userObject.getId() + "/";
            userId = userObject.getId();
        }

        this.activityFlag = activityFlag;
        this.lessonID = lessonID;
        this.QuizID = QuizID;
        this.isNotification = isNotification;

        try {
            key = new SecretKeySpec(Const.ALGO_SECRATE_KEY_NAME.getBytes(), Const.ALGO_SECRET_KEY_GENERATOR);
            keyData = key.getEncoded();
            keyFromKeydata = new SecretKeySpec(keyData, 0, keyData.length, Const.ALGO_SECRET_KEY_GENERATOR); //if you want to store key bytes to db so its just how to //recreate back key from bytes array
            iv = new byte[Const.IV_LENGTH];
            paramSpec = new IvParameterSpec(iv);

        } catch (Exception e) {
            e.printStackTrace();
        }
        //setHasStableIds(true);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        CourseInnerItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()), R.layout.course_inner_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        CoursePriviewObject.Lessons model = list.get(position);
        //holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);

        CoursePriviewObject.Lessons modelPrv = list.get((position == 0) ? position : position - 1);
        ArrayList<CoursePriviewObject.Lessonfiles> lessonfiles = new ArrayList<>();

        if (activityFlag.equals("1")) {
            if (!QuizID.equals("0")) {
                if (QuizID.equals(model.getId())) {
                    courseInnerItemInterface.courseInnerItem(ctx, "10", "", "", position, "0", 0, model.getId(), 0, chapterid, "0", model.getName());
                }
            } else {
                if (lessonID.equals(model.getId())) {
                    playItem(position, 1, holder, true);
                }
            }
        }

        if (position == 0) {
            if (isisLastItemViewed) {
                if (isPlayFirstItem) {
                    isPlayFirstItem = false;
                    if (activityFlag.equals("0")) {
                        playItem(position, 0, holder, true);
                    }
                    holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                } else {
                    holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                }
            } else {
                // Log.e("==Is_skippable==", "======" + userObject.getIs_skippable());
                if (userObject != null) {
                    String Is_skippable = "";
                    if (userObject.getId() != null) {
                        Is_skippable = userObject.getIs_skippable();
                        //Log.e("==Is_skippable==", "======" + Is_skippable);

                        if (!TextUtils.isEmpty(Is_skippable)) {
                            if (Is_skippable.equals("true")) {
                                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                            } else {
                                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                            }
                        }
                    }
                }
            }
        } else {

            // Log.e("==Is_skippable==", "===else===" + userObject.getIs_skippable());

            if (modelPrv.getProgressVal() == 100) {
                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
            } else {
                if (userObject != null) {
                    String Is_skippable = "";

                    if (userObject.getId() != null) {
                        Is_skippable = userObject.getIs_skippable();
                        if (!TextUtils.isEmpty(Is_skippable)) {
                            if (Is_skippable.equals("true")) {
                                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                            } else {
                                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                            }
                        }
                    }
                }
            }
        }

        holder.courseInnerItemLayoutBinding.progressBarLayout.progressBar.getProgressDrawable().setColorFilter(ContextCompat.getColor(ctx, R.color.colorProgress), PorterDuff.Mode.SRC_IN);
        holder.courseInnerItemLayoutBinding.progressBarLayout.progressBar.setProgress(model.getProgressVal());

//        holder.courseInnerItemLayoutBinding.tvProgress.setText(String.valueOf(model.getProgressVal()).concat(" ").concat(holder.itemView.getContext().getString(R.string.completed1)));
        // ChapterProgress chapterProgress = AppDatabase.getAppDatabase(ctx).chapterProgressDao().getChapterProgress(gradeId,chapterid);

        if (position == list.size() - 1) {
            int pro = chapterPro / list.size();
           /* callApiSyncFiles(fileProgressList);
            callApiSyncLessonProgress(lessonProgressList);
            callApiSyncQuiz(quizProgressList);
            callApiSyncChapter(chapterProgressList);*/
            ChapterProgress progress = AppDatabase.getAppDatabase(ctx).chapterProgressDao().getChapterProgress(gradeId, chapterid);
            if (progress != null && progress.getChapterId().equals(chapterid) && progress.getCourseId().equals(gradeId)) {
                ChapterProgress chapterProgress = new ChapterProgress();
                chapterProgress.setUserId(userId);
                chapterProgress.setCourseId(gradeId);
                chapterProgress.setChapterId(chapterid);
                chapterProgress.setProgress(String.valueOf(pro));
                AppDatabase.getAppDatabase(ctx).chapterProgressDao().updateChapterProgress(chapterProgress);
                chapterProgressList.add(chapterProgress);
            } else {
                ChapterProgress chapterProgress = new ChapterProgress();
                chapterProgress.setUserId(userId);
                chapterProgress.setCourseId(gradeId);
                chapterProgress.setChapterId(chapterid);
                chapterProgress.setProgress(String.valueOf(pro));
                AppDatabase.getAppDatabase(ctx).chapterProgressDao().insertAll(chapterProgress);
                chapterProgressList.add(chapterProgress);
            }

        }


        int lessonprogress = 0;
//        holder.courseInnerItemLayoutBinding.tvProgress.setText(model.getProgressVal() + "% " + ctx.getResources().getString(R.string.completed1));
        if (model.getLessonfiles() != null && model.getLessonfiles().length != 0) {
            holder.courseInnerItemLayoutBinding.txtFileName.setText(model.getName());
            Collections.addAll(lessonfiles, model.getLessonfiles());
            if (lessonfiles.size() != 0) {

                Collections.sort(lessonfiles, new Comparator<CoursePriviewObject.Lessonfiles>() {
                    @Override
                    public int compare(CoursePriviewObject.Lessonfiles lessonfiles, CoursePriviewObject.Lessonfiles t1) {
                        return lessonfiles.getFiles().getFiletypeid().compareTo(t1.getFiles().getFiletypeid());
                    }
                });
                for (int i = 0; i < lessonfiles.size(); i++) {
                    CoursePriviewObject.Lessonfiles lessonsModel = lessonfiles.get(i);
                    LessonProgress lessonProgressPrv = AppDatabase.getAppDatabase(NoonApplication.getContext()).lessonProgressDao().getItemLessonProgress(model.getId(), lessonsModel.getFiles().getId(), userId);
                    if (lessonProgressPrv != null && !lessonProgressPrv.getLessonProgress().isEmpty()) {
                        lessonprogress += Integer.parseInt(lessonProgressPrv.getLessonProgress());
                        lessonProg = Integer.parseInt(lessonProgressPrv.getLessonProgress());
                        model.setProgressVal(lessonProg);
                        holder.courseInnerItemLayoutBinding.tvProgress.setText((lessonProgressPrv.getLessonProgress()).concat(" ").concat(holder.itemView.getContext().getString(R.string.completed1)));
                    } else {
                        holder.courseInnerItemLayoutBinding.tvProgress.setText(("0").concat(" ").concat(holder.itemView.getContext().getString(R.string.completed1)));
                    }

                    FileProgress progress = AppDatabase.getAppDatabase(ctx).fileProgressDao().getFileProgress(lessonsModel.getFiles().getId(), model.getId());
                    if (progress != null && progress.getLessonId().equals(model.getId()) && progress.getFileId().equals(lessonsModel.getFiles().getId())) {
                        FileProgress fileProgress = new FileProgress();
                        /*if (i == lessonfiles.size() - 1) {
                            chapterPro += model.getProgressVal();
                        }*/
                        fileProgress.setUserId(userId);
                        fileProgress.setLessonId(model.getId());
                        fileProgress.setFileId(lessonsModel.getFiles().getId());
                        fileProgress.setProgress(String.valueOf(model.getProgressVal()));
                        AppDatabase.getAppDatabase(ctx).fileProgressDao().updateFileProgress(fileProgress);
                        fileProgressList.add(fileProgress);


                    } else {
                        FileProgress fileProgress = new FileProgress();
                        /*if (i == lessonfiles.size() - 1) {
                            chapterPro += model.getProgressVal();
                        }*/
                        fileProgress.setUserId(userId);
                        fileProgress.setLessonId(model.getId());
                        fileProgress.setFileId(lessonsModel.getFiles().getId());
                        fileProgress.setProgress(String.valueOf(model.getProgressVal()));
                        AppDatabase.getAppDatabase(ctx).fileProgressDao().insertAll(fileProgress);
                        fileProgressList.add(fileProgress);
                    }

                    /*if (lessonfiles.size() == 1) {
                        lessonsModel = lessonfiles.get(0);
                    } else {
                        lessonsModel = lessonfiles.get(lessonfiles.size() - 1);
                    }*/

                    holder.courseInnerItemLayoutBinding.txtfileType.setText(lessonsModel.getFiles().getFiletypename());
                    holder.courseInnerItemLayoutBinding.txtLanguagemin.setText(lessonsModel.getFiles().getFilesize());

                    if (lessonsModel.getFiles().getFiletypename().equals(Const.PDFfileType)) {
                        //holder.itemView.getLayoutParams().height = ViewGroup.LayoutParams.WRAP_CONTENT;
                        //holder.courseInnerItemLayoutBinding.imageFileIcon.setImageResource(R.drawable.ic_pdf);
                        //holder.itemView.getLayoutParams().height = 0;
                    } else if (lessonsModel.getFiles().getFiletypename().equals(Const.VideofileType)) {
                        // holder.itemView.getLayoutParams().height = ViewGroup.LayoutParams.WRAP_CONTENT;
                        //holder.courseInnerItemLayoutBinding.imageFileIcon.setImageResource(R.drawable.ic_video);
                    } else if (lessonsModel.getFiles().getFiletypename().equals(Const.ImagefileType)) {
                        // holder.itemView.getLayoutParams().height = ViewGroup.LayoutParams.WRAP_CONTENT;
                        //holder.itemView.getLayoutParams().height = 0;
                        //holder.courseInnerItemLayoutBinding.imageFileIcon.setImageResource(R.drawable.ic_photo);
                    }

                    holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(GONE);
                    holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(GONE);
                    Drawable drawable = ctx.getResources().getDrawable(R.drawable.circular_progressbar);
                    holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setSecondaryProgress(100); // Secondary Progress
                    holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setMax(100); // Maximum Progress
                    holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setProgressDrawable(drawable);

                    File directory = new File(Const.destPath + userIdSlash);
                    if (!directory.exists()) {
                        directory.mkdirs();
                    }

                    //Log.e("FILES00", "----files---" + directory);
                    String str = lessonsModel.getId() + "_" + lessonsModel.getFiles().getId() + "_" + lessonsModel.getFiles().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + lessonsModel.getFiles().getFiletypename() + Const.extension;
                    File file = new File(Const.destPath + userIdSlash, str);
                    //Log.e("FILES00", "----files-str--" + str);
                    if (file.exists()) {
                        holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(View.VISIBLE);
                        holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                        if (!isNetworkAvailable(ctx)) {
                            holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.GONE);
                        }
                    } else {

                        holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(GONE);
                        holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(View.VISIBLE);

                        if (isNetworkAvailable(ctx)) {
                            if (isNotification) {
                                if (lessonID.equals(model.getId())) {
                                    holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
                                } else {
                                    holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                                    holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
                                }
                            } else {
                                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                            }
                        } else {
                            holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
                        }
                    }
                }
                LessonNewProgress lessonNewProgress1 = AppDatabase.getAppDatabase(NoonApplication.getContext()).lessonnewProgressDao().getLessonProgress(lessonID, chapterid);
                if (lessonNewProgress1 != null && lessonNewProgress1.getLessonId().equals(lessonID) && lessonNewProgress1.getChapterId().equals(chapterid)) {
                    LessonNewProgress lessonNewProgress = new LessonNewProgress();
                    lessonNewProgress.setUserId(userId);
                    lessonNewProgress.setChapterId(chapterid);
                    lessonNewProgress.setLessonId(model.getId());
                    lessonNewProgress.setProgress(String.valueOf(lessonprogress / lessonfiles.size()));
                    chapterPro += lessonprogress / lessonfiles.size();
                    AppDatabase.getAppDatabase(NoonApplication.getContext()).lessonnewProgressDao().updateLessonProgress(lessonNewProgress);
                    lessonProgressList.add(lessonNewProgress);

                } else {
                    LessonNewProgress lessonNewProgress = new LessonNewProgress();
                    lessonNewProgress.setUserId(userId);
                    lessonNewProgress.setChapterId(chapterid);
                    lessonNewProgress.setLessonId(model.getId());
                    lessonNewProgress.setProgress(String.valueOf(lessonprogress / lessonfiles.size()));
                    chapterPro += lessonprogress / lessonfiles.size();
                    AppDatabase.getAppDatabase(NoonApplication.getContext()).lessonnewProgressDao().insertAll(lessonNewProgress);
                    lessonProgressList.add(lessonNewProgress);
                }
//                holder.courseInnerItemLayoutBinding.tvProgress.setText(String.valueOf(lessonprogress/lessonfiles.size()).concat(" ").concat(holder.itemView.getContext().getString(R.string.completed1)));
            }
        } else if (!TextUtils.isEmpty(model.getNumquestions())) {
            holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(GONE);
            holder.courseInnerItemLayoutBinding.imgquizContent.setVisibility(VISIBLE);
            //holder.courseInnerItemLayoutBinding.imageFileIcon.setImageResource(R.drawable.ic_quiz);
            holder.courseInnerItemLayoutBinding.txtFileName.setText(model.getName());
            holder.courseInnerItemLayoutBinding.txtfileType.setText("Quiz");
            holder.courseInnerItemLayoutBinding.txtLanguagemin.setText(model.getNumquestions() + " " + "Questions");
            QuizProgress progress = AppDatabase.getAppDatabase(ctx).quizProgressDao().getQuizProgress(model.getId(), chapterid);
            if (progress != null && progress.getQuizId().equals(model.getId()) && progress.getChapterId().equals(chapterid)) {
                QuizProgress quizProgress = new QuizProgress();
                quizProgress.setUserId(userId);
                quizProgress.setQuizId(model.getId());
                quizProgress.setChapterId(chapterid);
                quizProgress.setProgress(String.valueOf(model.getProgressVal()));
                holder.courseInnerItemLayoutBinding.tvProgress.setText(String.valueOf(model.getProgressVal()).concat(" ").concat(holder.itemView.getContext().getString(R.string.completed1)));
                AppDatabase.getAppDatabase(ctx).quizProgressDao().updateQuizProgress(quizProgress);
                quizProgressList.add(quizProgress);
            } else {
                QuizProgress quizProgress = new QuizProgress();
                quizProgress.setUserId(userId);
                quizProgress.setQuizId(model.getId());
                quizProgress.setChapterId(chapterid);
                quizProgress.setProgress(String.valueOf(model.getProgressVal()));
                AppDatabase.getAppDatabase(ctx).quizProgressDao().insertAll(quizProgress);
                holder.courseInnerItemLayoutBinding.tvProgress.setText(String.valueOf(0).concat(" ").concat(holder.itemView.getContext().getString(R.string.completed1)));
                quizProgressList.add(quizProgress);
            }

            QuizMainObject quizMainObject = AppDatabase.getAppDatabase(ctx).quizAnswerDao().getquizData(userId, model.getId());

            if (quizMainObject != null) {
                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
            } else {
                if (isNetworkAvailable(ctx)) {
                    if (isNotification) {
                        if (!TextUtils.isEmpty(model.getId())) {
                            if (QuizID.equals(model.getId())) {
                                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
                            } else {
                                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                            }
                        }
                    } else {
                        holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                    }
                } else {
                    holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
                }
            }
        } else if (model.getAssignment() != null) {
            // Assignment View
            if (isNetworkAvailable(ctx)) {
                holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(GONE);
                holder.courseInnerItemLayoutBinding.imgquizContent.setVisibility(VISIBLE);

                if (model.getAssignment() != null) {
                    if (model.getAssignment().getId() != null) {
                        holder.courseInnerItemLayoutBinding.tag.setText(model.getAssignment().getId());
                    }

                    if (model.getAssignment().getName() != null) {
                        holder.courseInnerItemLayoutBinding.txtFileName.setText(model.getAssignment().getName());
                    }
                    holder.courseInnerItemLayoutBinding.txtfileType.setText(ctx.getResources().getString(R.string.assignment));
                }
                //holder.courseInnerItemLayoutBinding.txtFileName.setText(assignments.get(i).getName());
//                        holder.courseInnerItemLayoutBinding.txtFileName.setText(ctx.getResources().getString(R.string.chapter_assignment));

            } else {
                holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(GONE);
                holder.courseInnerItemLayoutBinding.imgquizContent.setVisibility(VISIBLE);
                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(VISIBLE);
                if (model.getAssignment() != null) {
                    if (model.getAssignment().getId() != null) {
                        holder.courseInnerItemLayoutBinding.tag.setText(model.getAssignment().getId());
                    }

                    if (model.getAssignment().getName() != null) {
                        holder.courseInnerItemLayoutBinding.txtFileName.setText(model.getAssignment().getName());
                    }
                    holder.courseInnerItemLayoutBinding.txtfileType.setText(ctx.getResources().getString(R.string.assignment));
                }
                //holder.courseInnerItemLayoutBinding.txtFileName.setText(assignments.get(i).getName());
//

            }
            holder.courseInnerItemLayoutBinding.tvProgress.setText(String.valueOf(model.getProgressVal()).concat(" ").concat(holder.itemView.getContext().getString(R.string.completed1)));
            /*holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(GONE);
            holder.courseInnerItemLayoutBinding.imgquizContent.setVisibility(VISIBLE);
            holder.courseInnerItemLayoutBinding.txtFileName.setText(ctx.getResources().getString(R.string.chapter_assignment));
            holder.courseInnerItemLayoutBinding.txtfileType.setText("Assignment");*/
            //holder.courseInnerItemLayoutBinding.txtLanguagemin.setText(model.getNumquestions() + " " + "Questions");
            // QuizMainObject quizMainObject = AppDatabase.getAppDatabase(ctx).quizAnswerDao().getquizData(userId, model.getId());

//            if (quizMainObject != null) {
//                holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
//            } else {
//                if (isNetworkAvailable(ctx)) {
//                    if (isNotification) {
//                        if (QuizID.equals(model.getId())) {
//                            holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
//                        } else {
//                            holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
//                        }
//                    } else {
//                        holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
//                    }
//                } else {
//                    holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
//                }
//            }
        }

        holder.courseInnerItemLayoutBinding.imgdownloadContent.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(false);
                holder.courseInnerItemLayoutBinding.imgdownloadContent.setEnabled(false);
                // Log.e("INTERVALLLLLL", "=====11==");
                long milliseconds = System.currentTimeMillis();
                IntervalTableObject myintervalobject = AppDatabase.getAppDatabase(ctx).intervalDao().getAllInterval();
                if (myintervalobject != null) {
                    // Log.e("INTERVALLLLLL", "=====22==");

                    if (myintervalobject.getLocalinterval() != null) {

                        //Log.e("INTERVALLLLLL", "=====44==");

                        long currantminutes = TimeUnit.MILLISECONDS.toMinutes(milliseconds);
                        long localminutes = TimeUnit.MILLISECONDS.toMinutes(Long.parseLong(myintervalobject.getLocalinterval()));
                        long finallocalminute = currantminutes - localminutes;
                        long getinterval = Long.parseLong(myintervalobject.getInterval());

                        //Log.e("INTERVALLLLLL", "=====myintervalobject====" + finallocalminute + " ==== " + getinterval);

                        if (finallocalminute >= getinterval) {
                            //Log.e("INTERVALLLLLL", "=====66==");
                            showDialogcancle(position, holder);
                            //showCustomDialogcancle(position, holder);
                            AppDatabase.getAppDatabase(ctx).intervalDao().updateItem(String.valueOf(milliseconds), myintervalobject.getIntervalTableID());
                        } else {
                            //Log.e("INTERVALLLLLL", "=====77==");
                            startDownload(position, holder);
                        }
                    } else {
                        // Log.e("INTERVALLLLLL", "=====55==");
                        showDialogcancle(position, holder);
                        //showCustomDialogcancle(position, holder);
                        AppDatabase.getAppDatabase(ctx).intervalDao().updateItem(String.valueOf(milliseconds), myintervalobject.getIntervalTableID());
                    }
                } else {
                    //Log.e("INTERVALLLLLL", "=====33==");
                    startDownload(position, holder);
                }
            }

        });

        holder.courseInnerItemLayoutBinding.imgcloseContent.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(VISIBLE);
                holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
                holder.courseInnerItemLayoutBinding.imgdownloadContent.setEnabled(true);
                holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(View.GONE);
                holder.courseInnerItemLayoutBinding.PushResumeLay.setVisibility(View.GONE);

                if (CourseItemFragment.hashMap.size() != 0) {
                    CoursePriviewObject.Lessons model = list.get(position);
                    CourseItemFragment.hashMap.remove(model.getId());
                    for (int i = 0; i < CourseItemFragment.fileidarray.size(); i++) {
                        if (CourseItemFragment.fileidarray.get(i).equals(model.getId())) {
                            CourseItemFragment.fileidarray.remove(i);
                        }
                    }
                }
                if (CourseItemFragment.queueArray.size() != 0) {

                    CoursePriviewObject.Lessons model = list.get(position);
                    ArrayList<CoursePriviewObject.Lessonfiles> lessonfiles = new ArrayList<>();
                    Collections.addAll(lessonfiles, model.getLessonfiles());
                    if (lessonfiles.size() != 0 && lessonfiles != null) {
                        for (int j = 0; j < lessonfiles.size(); j++) {
                            CoursePriviewObject.Lessonfiles lessonsModel = lessonfiles.get(j);

                           /* if (lessonfiles.size() == 1) {
                                lessonsModel = lessonfiles.get(0);
                            } else {
                                lessonsModel = lessonfiles.get(lessonfiles.size() - 1);
                            }*/

                            for (int i = 0; i < CourseItemFragment.queueArray.size(); i++) {
                                if (CourseItemFragment.queueArray.get(i).getFileid().equals(lessonsModel.getFiles().getId())) {
                                    CourseItemFragment.queueArray.remove(i);
                                }
                            }
                        }
                    }
                }
            }
        });

        holder.courseInnerItemLayoutBinding.cardItemLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(false);
                holder.courseInnerItemLayoutBinding.imgdownloadContent.setEnabled(false);
                if (holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.getVisibility() != VISIBLE) {
                    if (holder.courseInnerItemLayoutBinding.txtfileType.getText().toString().equals(ctx.getResources().getString(R.string.assignment))) {
                        String id = holder.courseInnerItemLayoutBinding.tag.getText().toString();
                        if (isNetworkAvailable(ctx)) {
                            if (assignments != null && assignments.size() != 0) {
                                for (int i = 0; i < assignments.size(); i++) {
                                    if (id.equals(assignments.get(i).getId())) {
                                        CoursePriviewObject.Assignment assignment = assignments.get(i);
                                        Intent intent = new Intent(ctx, AssignmentDetailActivity.class);
                                        intent.putExtra(Const.Assignment, new Gson().toJson(assignment));
                                        intent.putExtra("coursename", Coursename);
                                        intent.putExtra("lessonname", assignments.get(i).getName());
                                        holder.itemView.getContext().startActivity(intent);
                                    }
                                }
                            }
                        } else {
                            showNetworkAlert(ctx);
                        }

                    } else {
                        playItem(position, 1, holder, false);
                    }
                }
            }
        });

        holder.courseInnerItemLayoutBinding.disableLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (isNotification) {
                    holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                    holder.courseInnerItemLayoutBinding.cardItemLayout.performClick();
                } else {
                    showNetworkAlert(ctx);
                }
            }
        });

        holder.courseInnerItemLayoutBinding.imgDeleteContent.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showDialogDelete(position, holder);
                //showCustomDialogDelete(position, holder);
            }
        });
    }


    public void showCustomDialogDelete(int pos, MyViewHolder holder) {
        DeletePopupLayoutBinding deletePopupLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(ctx), R.layout.delete_popup_layout, null, false);
        AlertDialog.Builder builder = new AlertDialog.Builder(ctx);

        //setting the view of the builder to our custom view that we already inflated
        builder.setView(deletePopupLayoutBinding.getRoot());

        //finally creating the alert dialog and displaying it
        final AlertDialog alertDialog = builder.create();
        //alertDialog.getWindow().setBackgroundDrawable(new ColorDrawable(android.graphics.Color.TRANSPARENT));
        deletePopupLayoutBinding.txtDeleteOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                alertDialog.dismiss();
                CoursePriviewObject.Lessons model = list.get(pos);
                ArrayList<CoursePriviewObject.Lessonfiles> lessonfiles = new ArrayList<>();
                Collections.addAll(lessonfiles, model.getLessonfiles());
                if (lessonfiles.size() != 0 && lessonfiles != null) {
                    for (int i = 0; i < lessonfiles.size(); i++) {
                        CoursePriviewObject.Lessonfiles lessonsModel = lessonfiles.get(i);
                    /*if (lessonfiles.size() == 1) {
                        lessonsModel = lessonfiles.get(0);
                    } else {
                        lessonsModel = lessonfiles.get(lessonfiles.size() - 1);
                    }*/

                        String str = lessonsModel.getId() + "_" + lessonsModel.getFiles().getId() + "_" + lessonsModel.getFiles().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + lessonsModel.getFiles().getFiletypename() + Const.extension;
                        File file = new File(Const.destPath + userIdSlash, str);
                        if (file.exists()) {
                            file.delete();
                        }

                        notifyItemChanged(pos);


                        String delItemPath = Const.destPath + userIdSlash + str;


                        // Log.e("======", "=======" + delItemPath + " === " + selectedVideoPath);

                        if (!TextUtils.isEmpty(selectedVideoPath)) {
                            if (selectedVideoPath.equalsIgnoreCase(delItemPath)) {
                                //Log.e("======", "===0000====" + delItemPath + " === " + selectedVideoPath);
                                courseHideResponse.courseHideItem(ctx, lessonsModel.getFiles().getFiletypeid());
                                holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(ctx.getResources().getColor(R.color.colorBlack));
                            }
                        }

                    }
                }
            }
        });
        alertDialog.show();

    }

    public void showDialogDelete(int pos, MyViewHolder holder) {
        try {
            SpannableStringBuilder message = setTypeface(ctx, ctx.getResources().getString(R.string.are_you_sure_delete));
            AlertDialog myAlertDialog = null;
            AlertDialog.Builder builder = new AlertDialog.Builder(ctx);

            builder.setMessage(message);
            builder.setPositiveButton(R.string.ok, new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialogInterface, int arg1) {
                    dialogInterface.dismiss();

                    CoursePriviewObject.Lessons model = list.get(pos);
                    ArrayList<CoursePriviewObject.Lessonfiles> lessonfiles = new ArrayList<>();
                    lessonfiles.clear();
                    Collections.addAll(lessonfiles, model.getLessonfiles());
                    if (lessonfiles.size() != 0 && lessonfiles != null) {
                        for (int i = 0; i < lessonfiles.size(); i++) {
                            CoursePriviewObject.Lessonfiles lessonsModel = lessonfiles.get(i);
                        /*if (lessonfiles.size() == 1) {
                            lessonsModel = lessonfiles.get(0);
                        } else {
                            lessonsModel = lessonfiles.get(lessonfiles.size() - 1);
                        }*/

                            String str = lessonsModel.getId() + "_" + lessonsModel.getFiles().getId() + "_" + lessonsModel.getFiles().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + lessonsModel.getFiles().getFiletypename() + Const.extension;
                            File file = new File(Const.destPath + userIdSlash, str);
                            if (file.exists()) {
                                file.delete();
                            }

                            notifyItemChanged(pos);
                            String delItemPath = Const.destPath + userIdSlash + str;
                            // Log.e("======", "=======" + delItemPath + " === " + selectedVideoPath);

                            if (!TextUtils.isEmpty(selectedVideoPath)) {
                                if (selectedVideoPath.equalsIgnoreCase(delItemPath)) {
                                    //Log.e("======", "===0000====" + delItemPath + " === " + selectedVideoPath);
                                    courseHideResponse.courseHideItem(ctx, lessonsModel.getFiles().getFiletypeid());
                                    holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(ctx.getResources().getColor(R.color.colorBlack));
                                }
                            }

                        }
                    }
                }
            });

            builder.setNegativeButton(R.string.cancel, new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialogInterface, int i) {
                    dialogInterface.dismiss();
                }
            });
            builder.setCancelable(false);
            myAlertDialog = builder.create();
            myAlertDialog.show();

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public void multiplefiledownload(ArrayList<DownloadQueueObject> downloadQueueObjects, MyViewHolder holder) {
//        static hashmap
        if (downloadQueueObjects != null && !downloadQueueObjects.isEmpty()) {

            if (!CourseItemFragment.isdownload) {
                holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(false);
                CourseItemFragment.isdownload = true;
                downloadQueueObjects.get(0).getHolder().courseInnerItemLayoutBinding.imgdownloadContent.setEnabled(false);

                callApifetchMultipleUrl(downloadQueueObjects, holder);
            } else {
                if (CourseItemFragment.hashMap.size() >= 2) {
                    holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
//                downloadQueueObjects.get(0).getHolder().courseInnerItemLayoutBinding.imgdownloadContent.setEnabled(true);
                    Toast.makeText(ctx, ctx.getResources().getString(R.string.after_download_complete), Toast.LENGTH_SHORT).show();
                } else {
                    CourseItemFragment.hashMap.put(downloadQueueObjects.get(0).getLemodel().getId(), downloadQueueObjects);
                    CourseItemFragment.fileidarray.add(downloadQueueObjects.get(0).getLemodel().getId());
                    holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                    holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(View.VISIBLE);
                    holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setProgress(0);
                    holder.courseInnerItemLayoutBinding.PushResumeLay.setVisibility(VISIBLE);
                    holder.courseInnerItemLayoutBinding.imgPushContent.setVisibility(GONE);
                    holder.courseInnerItemLayoutBinding.imgcloseContent.setVisibility(VISIBLE);
                }
            }
        }
    }

    public void downloadProcess(DownloadQueueObject downloadQueueObject) {
        if (CourseItemFragment.isdownload) {
            CourseItemFragment.isdownload = false;
            downloadQueueObject.getHolder().courseInnerItemLayoutBinding.imgdownloadContent.setEnabled(false);
            callApifetchSignedUrl(downloadQueueObject);
        } else {
            downloadQueueObject.getHolder().courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
            downloadQueueObject.getHolder().courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(View.VISIBLE);
            downloadQueueObject.getHolder().courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setProgress(0);
            downloadQueueObject.getHolder().courseInnerItemLayoutBinding.PushResumeLay.setVisibility(VISIBLE);
            downloadQueueObject.getHolder().courseInnerItemLayoutBinding.imgPushContent.setVisibility(GONE);
            downloadQueueObject.getHolder().courseInnerItemLayoutBinding.imgcloseContent.setVisibility(VISIBLE);
        }
    }


    public void playItem(int position, int playpushflag, MyViewHolder holder, boolean downloadFlag) {

        CoursePriviewObject.Lessons model = list.get(position);
        ArrayList<CoursePriviewObject.Lessonfiles> lessonfiles = new ArrayList<>();

        if (model.getLessonfiles() != null) {
            Collections.addAll(lessonfiles, model.getLessonfiles());
            Collections.sort(lessonfiles, new Comparator<CoursePriviewObject.Lessonfiles>() {
                @Override
                public int compare(CoursePriviewObject.Lessonfiles t1, CoursePriviewObject.Lessonfiles t2) {
                    return t1.getFiles().getFiletypeid().compareTo(t2.getFiles().getFiletypeid());
                }
            });
            if (lessonfiles.size() != 0) {
                for (int i = 0; i < lessonfiles.size(); i++) {
                    CoursePriviewObject.Lessonfiles lessonsModel = lessonfiles.get(i);


                    lessonsModel = lessonfiles.get(i);
                    String str = lessonsModel.getId() + "_" + lessonsModel.getFiles().getId() + "_" + lessonsModel.getFiles().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + lessonsModel.getFiles().getFiletypename() + Const.extension;
                    try {
                        File file = null;
                      /*  if (str.contains("/")) {
                            StringBuilder file1 = new StringBuilder();
                            String[] filename1 = str.split("/");
                            for (String word : filename1) {
                                file1.append(word);
                            }
                            file = new File(Const.destPath + userIdSlash, file1.toString());
                        } else {
                        }*/
                        file = new File(Const.destPath + userIdSlash, str);
                        Log.d("file_____", String.valueOf(file));
                        if (file.exists()) {
                            String filepath = String.valueOf(file);
                            if (lessonsModel.getFiles().getFiletypeid().equals("1")) {
                                holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(ctx.getResources().getColor(R.color.colorBlack));
                                selectedVideoPath = Const.destPath + userIdSlash + str;

                                if (str.contains("/")) {
                                    String[] filename1 = str.split("/");

                                    for (String name : filename1) {
                                        str = name;
                                    }
                                }

                                outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
                                encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str;
                                String decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
                                Log.d("file_____", String.valueOf(decryptpath));
                                courseInnerItemInterface.courseInnerItem(ctx, lessonsModel.getFiles().getFiletypeid(), lessonsModel.getFiles().getFiletypename(), decryptpath, position, model.getId(), model.getProgressVal(), "", 0, chapterid, lessonsModel.getFiles().getId(), model.getName());

                            } else if (lessonsModel.getFiles().getFiletypeid().equals("2")) {
                                holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(ctx.getResources().getColor(R.color.colorBlack));
                                selectedVideoPath = Const.destPath + userIdSlash + str;
                                if (str.contains("/")) {
                                    String[] filename1 = str.split("/");

                                    for (String name : filename1) {
                                        str = name;
                                    }
                                }
                                outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
                                encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str;
                                String decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
                                Log.d("file_____", String.valueOf(decryptpath));
                                courseInnerItemInterface.courseInnerItem(ctx, lessonsModel.getFiles().getFiletypeid(), lessonsModel.getFiles().getFiletypename(), decryptpath, position, model.getId(), model.getProgressVal(), "", playpushflag, chapterid, lessonsModel.getFiles().getId(), model.getName());
                            }


                        } else {
                            //Toast.makeText(ctx, R.string.error_file_not_found, Toast.LENGTH_SHORT).show();
                            if (downloadFlag) {
                                if (isNetworkAvailable(ctx)) {
                                    //showDownloadAlert(holder);
                                }
                            } else {
                                if (i == lessonfiles.size() - 1) {
                                    holder.courseInnerItemLayoutBinding.imgdownloadContent.performClick();
                                    holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(GONE);
                                }
                            }
                        }
                    } catch (NoSuchAlgorithmException e) {
                        e.printStackTrace();
                    } catch (NoSuchPaddingException e) {
                        e.printStackTrace();
                    } catch (InvalidKeyException e) {
                        e.printStackTrace();
                    } catch (InvalidAlgorithmParameterException e) {
                        e.printStackTrace();
                    } catch (SecurityException e) {
                        e.printStackTrace();
                        if (e.getLocalizedMessage().equalsIgnoreCase("Invalid or expired API Key")) {
                            Log.e("SecurityException", "Exception");
                            showAlertDialog(ctx, holder);
                            //showExpiredItemClick(ctx, holder);
                        }
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }
            }
        } else {
            courseInnerItemInterface.courseInnerItem(ctx, "10", "", "", position, "0", 0, model.getId(), playpushflag, chapterid, "0", model.getName());
        }
    }

    public void showAlertDialog(Context context, MyViewHolder holder) {

        try {
            SpannableStringBuilder message = setTypeface(context, context.getResources().getString(R.string.Expired_Key));
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.setTitle(context.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(context.getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                            holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(ctx.getResources().getColor(R.color.colorBlack));
                            holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                            holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(VISIBLE);
                            holder.courseInnerItemLayoutBinding.imgcloseContent.setVisibility(GONE);
                            holder.courseInnerItemLayoutBinding.PushResumeLay.setVisibility(GONE);
                            holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(GONE);
                        }
                    });
            builder.setNegativeButton(context.getResources().getString(R.string.button_cancel), new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                    dialog.cancel();
                }
            });

            builder.show();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    @Override
    public void onClick(View view) {
    }

    public interface openPdf {
        void onPdf();
    }

    public void callApifetchMultipleUrl(ArrayList<DownloadQueueObject> downloadQueueObjects, MyViewHolder holder) {
        ArrayList<String> signedUrl = new ArrayList<>();
        int downloadobjectarraysize = downloadQueueObjects.size();

        for (int i = 0; i < downloadQueueObjects.size(); i++) {
            int finalI = i;
            disposable.add(apiService.fetchSignedUrl(downloadQueueObjects.get(i).getFileid(), downloadQueueObjects.get(i).getLessonID())
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<SignedUrlObject>() {
                        @Override
                        public void onSuccess(SignedUrlObject signedUrlObject) {
                            BaseActivity.freeMemory(ctx);
                            signedUrl.add(signedUrlObject.getData().getUrl());

                            if (signedUrl.size() == downloadobjectarraysize) {

                                new DownloadMultipleTask(signedUrl, downloadQueueObjects.get(0).getHolder(), downloadQueueObjects.get(0).getLemodel(), downloadQueueObjects).execute();
                            }
//                            new DownloadStatusTask(signedUrlObject.getData().getUrl()
//                                    downloadQueueObjects.get(0).getHolder(),
//                                    lessonsModel,
//                                    model,
//                                    downloadQueueObjects)
//                                    .execute();
                        }

                        @Override
                        public void onError(Throwable e) {
                            CourseItemFragment.isdownload = false;
                            CourseItemFragment.hashMap.remove(downloadQueueObjects.get(0).getLemodel().getId());
                            refreshToken(downloadQueueObjects.get(finalI));

                            // handleDownloadCounterWhenFailAndComplite(downloadQueueObjects.get(0));
                            Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                            showDialogNoNetwork();
                        }
                    }));
        }


    }

    public void callApifetchSignedUrl(DownloadQueueObject downloadQueueObject) {

        //showDialog(ctx.getString(R.string.loading));
        String fileID = downloadQueueObject.getFileid();
        String lessionID = downloadQueueObject.getLessonID();
        MyViewHolder holder = downloadQueueObject.getHolder();
        CoursePriviewObject.Lessonfiles lessonsModel = downloadQueueObject.getLessonsModel();
        CoursePriviewObject.Lessons model = downloadQueueObject.getLemodel();

        disposable.add(apiService.fetchSignedUrl(fileID, lessionID)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<SignedUrlObject>() {
                    @Override
                    public void onSuccess(SignedUrlObject signedUrlObject) {

                        //Toast.makeText(ctx, "== API onSuccess = SignedURL= " + signedUrlObject.getData().getUrl(), Toast.LENGTH_SHORT).show();
                        BaseActivity.freeMemory(ctx);

//                        Log.e("00FILEURL","-----file---url--length----"+signedUrlObject.getData().getUrl().length());
                        new DownloadStatusTask(signedUrlObject.getData().getUrl(),
                                holder,
                                lessonsModel,
                                model,
                                downloadQueueObject)
                                .execute();
                    }

                    @Override
                    public void onError(Throwable e) {
                        refreshToken(downloadQueueObject);


                        //handleDownloadCounterWhenFailAndComplite(downloadQueueObject);
                        Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                        showDialogNoNetwork();
                    }
                }));
    }

    public void callApifetchSignedUrl(ArrayList<DownloadQueueObject> downloadQueueObject) {
        //showDialog(ctx.getString(R.string.loading));
        for (DownloadQueueObject download : downloadQueueObject) {
            String fileID = download.getFileid();
            String lessionID = download.getLessonID();
            MyViewHolder holder = download.getHolder();
            CoursePriviewObject.Lessonfiles lessonsModel = download.getLessonsModel();
            CoursePriviewObject.Lessons model = download.getLemodel();

            disposable.add(apiService.fetchSignedUrl(fileID, lessionID)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<SignedUrlObject>() {
                        @Override
                        public void onSuccess(SignedUrlObject signedUrlObject) {

                            //Toast.makeText(ctx, "== API onSuccess = SignedURL= " + signedUrlObject.getData().getUrl(), Toast.LENGTH_SHORT).show();
                            BaseActivity.freeMemory(ctx);

//                        Log.e("00FILEURL","-----file---url--length----"+signedUrlObject.getData().getUrl().length());
                            new DownloadStatusTask(signedUrlObject.getData().getUrl(),
                                    holder,
                                    lessonsModel,
                                    model,
                                    download)
                                    .execute();
                        }

                        @Override
                        public void onError(Throwable e) {
                            refreshToken(download);

                            //handleDownloadCounterWhenFailAndComplite(download);
                            Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                            showDialogNoNetwork();
                        }
                    }));
        }
    }

    public class DownloadMultipleTask extends AsyncTask<Void, Void, String> {
        private ArrayList<String> signedUrl;
        private MyViewHolder holder;
        private CoursePriviewObject.Lessons model;
        private ArrayList<DownloadQueueObject> downloadQueueObjects;
        private int downloadId;
        private ArrayList<DownloadProgress> downloadIdArray = new ArrayList<>();
        private long progressval;

        public DownloadMultipleTask(ArrayList<String> signedUrl, MyViewHolder holder, CoursePriviewObject.Lessons model, ArrayList<DownloadQueueObject> downloadQueueObjects) {
            this.signedUrl = signedUrl;
            this.holder = holder;
            this.model = model;
            this.downloadQueueObjects = downloadQueueObjects;
        }

        @Override
        protected String doInBackground(Void... voids) {
            Log.e("onPostExecute", "doInBackground: ");

            Collections.sort(signedUrl, new Comparator<String>() {
                @Override
                public int compare(String s, String t1) {
                    return s.compareTo(t1);
                }
            });
            Collections.sort(downloadQueueObjects, new Comparator<DownloadQueueObject>() {
                @Override
                public int compare(DownloadQueueObject t1, DownloadQueueObject t2) {
                    return t1.getLessonsModel().getFiles().getFiletypeid().compareTo(t2.getLessonsModel().getFiles().getFiletypeid());
                }
            });
            int totalSize = 0;
            final long[] currentProgress = {0};
            for (int i = 0; i < downloadQueueObjects.size(); i++) {
                String size = downloadQueueObjects.get(i).getLessonsModel().getFiles().getFilesize();
                if (size != null && !size.isEmpty()) {
                    totalSize += Integer.parseInt(size);
                }
            }

            for (int i = 0; i < signedUrl.size(); i++) {

                String EncryptFileName = downloadQueueObjects.get(i).getLessonsModel().getFiles().getFilename();
                String filetype = downloadQueueObjects.get(i).getLessonsModel().getFiles().getFiletypename();
                String fileName = downloadQueueObjects.get(i).getLessonsModel().getId() + "_" + downloadQueueObjects.get(i).getLessonsModel().getFiles().getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);
                String downloadFilePath = Const.destPath + userIdSlash + fileName;

                try {
                    String str = downloadQueueObjects.get(i).getLessonsModel().getId() + "_" + downloadQueueObjects.get(i).getLessonsModel().getFiles().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
                    PRDownloader.cleanUp(1);

                    //Log.e("COUNTER", "======downloadCount===IF===" + downloadCount);

                    int finalDownloadId = downloadId;
                    int finalI = i;
                    int finalTotalSize = totalSize;
                    int finalTotalSize1 = totalSize;
                    downloadId = PRDownloader.download(signedUrl.get(i), Const.destPath + userIdSlash, fileName)
                            .build()
                            .setOnStartOrResumeListener(new OnStartOrResumeListener() {
                                @RequiresApi(api = Build.VERSION_CODES.N)
                                @Override
                                public void onStartOrResume() {
                                    //hideDialog();
                                    if (finalI == 0) {
                                        doTimerTask();
                                        holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                                        holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(VISIBLE);
                                        holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setProgress(0);
                                        holder.courseInnerItemLayoutBinding.PushResumeLay.setVisibility(VISIBLE);
                                        holder.courseInnerItemLayoutBinding.PushResumeLay.setEnabled(true);
                                        holder.courseInnerItemLayoutBinding.imgPushContent.setVisibility(VISIBLE);
                                        holder.courseInnerItemLayoutBinding.imgPushContent.setEnabled(true);
                                        holder.courseInnerItemLayoutBinding.imgcloseContent.setVisibility(GONE);
                                        Log.e("downloadBack", "onStartOrResume:");
                                        CourseItemFragment.hashMap.remove(downloadQueueObjects.get(0).getLemodel().getId());
                                        if (CourseItemFragment.fileidarray != null && !CourseItemFragment.fileidarray.isEmpty()) {
                                            CourseItemFragment.fileidarray.remove(0);
                                        }
                                    }
                                    //Toast.makeText(ctx, "Downloading NO.   " + downloadId + "-----" + model.getName(), Toast.LENGTH_SHORT).show();

                                    new AsyncTask<Void, Void, String>() {
                                        @Override
                                        protected String doInBackground(Void... voids) {
                                            FileDownloadStatus fileDownloadStatus = new FileDownloadStatus();
                                            fileDownloadStatus.setLessonid("0");
                                            fileDownloadStatus.setFileid(downloadQueueObjects.get(finalI).getLessonsModel().getFiles().getId());
                                            fileDownloadStatus.setDownloadID(finalDownloadId);
                                            AppDatabase.getAppDatabase(ctx).fileDownloadStatusDao().insertAll(fileDownloadStatus);
                                            return null;
                                        }
                                    }.execute();

                                    //Log.e("COUNTER", "======onStartOrResume======");
                                    showDialogNoNetwork();

                                }
                            })
                            .setOnPauseListener(new OnPauseListener() {
                                @Override
                                public void onPause() {
                                    //Log.e("COUNTER", "======setOnPauseListener======");
                                    showDialogNoNetwork();

                                }
                            })
                            .setOnCancelListener(new OnCancelListener() {
                                @Override
                                public void onCancel() {
                                    //handleDownloadCounterWhenFailAndComplite(downloadQueueObjects.get(finalI));
                                    //Log.e("COUNTER", "======setOnCancelListener======");
                                    showDialogNoNetwork();
                                    PRDownloader.cancelAll();
                                    CourseItemFragment.isdownload = false;
                                    CourseItemFragment.hashMap.remove(downloadQueueObjects.get(0).getLemodel().getId());
                                }
                            })
                            .setOnProgressListener(new OnProgressListener() {
                                @Override
                                public void onProgress(Progress progress) {

                                    long currentSize = 0;
                                    long totalSize = 0;
                                    for (DownloadProgress download : downloadIdArray) {
                                        if (downloadId == download.getDownloadId()) {
                                            download.setTotal(progress.totalBytes);
                                        }
                                        totalSize += download.getTotal();
                                    }

                                    for (DownloadProgress download : downloadIdArray) {
                                        if (downloadId == download.getDownloadId()) {
                                            download.setProgress(progress.currentBytes);
                                            currentSize += download.getProgress();
                                        } else {
                                            currentSize += download.getProgress();
                                        }
                                    }

                                    long progressval = (currentSize * 100) / totalSize;
                                    if (finalI == downloadQueueObjects.size() - 1) {
                                        holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setProgress((int) progressval);
                                    }
                                    if (progressval == 100) {

                                    }
//                                    Log.e("-------------", "==== 4444444444 Total =====" + totalSize);
//                                    Log.e("-------------", "==== 4444444444 Current =====" + currentSize);
//                                    Log.e("-------------", "==== 4444444444 Per=====" + progressval);


                                    //Log.e("COUNTER", "======setOnProgressListener======");
//                                    showDialogNoNetwork();

                                }
                            })
                            .start(new OnDownloadListener() {
                                @Override
                                public void onDownloadComplete() {

                                    //Log.e("COUNTER", "======onDownloadComplete======");
                                    BaseActivity.freeMemory(ctx);

                                    EncryptDecryptObject encryptDecryptObject = new EncryptDecryptObject();
                                    encryptDecryptObject.setSelectedVideoPath(downloadFilePath);
                                    if (str.contains("/")) {
                                        StringBuilder file1 = new StringBuilder();
                                        String[] filename1 = str.split("/");

                                        for (String name : filename1) {
                                            encryptDecryptObject.setFilename(name);
                                        }
                                    } else {
                                        encryptDecryptObject.setFilename(str);
                                    }

                                    encryptDecryptObject.setHolder(holder);
                                    new SetEncryptDecryptTask(new EncryptDecryptAsyncResponse() {
                                        @Override
                                        public EncryptDecryptObject getEncryptDecryptObjects(EncryptDecryptObject encryptDecryptObjects) {

                                            //handleDownloadCounterWhenFailAndComplite(downloadQueueObjects.get(finalI));
                                            try {
                                                BaseActivity.freeMemory(ctx);
                                                String selectedVideoPath = encryptDecryptObject.getSelectedVideoPath();
                                                String filename = encryptDecryptObject.getFilename();
                                                MyViewHolder holder = encryptDecryptObject.getHolder();

                                                encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename;
                                                EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);

                                                Log.e("encrypted_path", "getEncryptDecryptObjects: " + encrypted_path);
                                                Log.e("000FILESIZE", "---queueArray---" + CourseItemFragment.queueArray.size() + "--lessonfiles---" + lessonfiles.size());

//                                                if (lessonfiles.size() == CourseItemFragment.queueArray.size()) {
//                                                    holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
//                                                    holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(GONE);
//                                                }

                                            /*if (CourseItemFragment.queueArray.size() > 0) {
                                                CourseItemFragment.queueArray.remove(downloadQueueObject);
                                                CourseItemFragment.isdownload = true;
                                                if (CourseItemFragment.queueArray.size() != 0) {
                                                    downloadProcess(CourseItemFragment.queueArray.get(0));
                                                }
                                            }*/

                                                if (finalI == signedUrl.size() - 1) {
                                                    com.downloader.Status fileStatus = PRDownloader.getStatus(downloadId);
                                                    FileDownloadStatus fileDownloadStatus = AppDatabase.getAppDatabase(ctx).fileDownloadStatusDao().getItemFileDownloadStatus(downloadQueueObjects.get(finalI).getLessonsModel().getFiles().getId());
                                                    if (fileDownloadStatus != null) {
                                                        AppDatabase.getAppDatabase(ctx).fileDownloadStatusDao().updateItemFileDownloadStatus(downloadQueueObjects.get(finalI).getLessonsModel().getFiles().getId(), fileStatus.toString(), (int) progressval);
                                                    }
                                                    DownloadProgress downloadProgress = downloadIdArray.get(downloadIdArray.size() - 1);
                                                    com.downloader.Status status = PRDownloader.getStatus((int) downloadProgress.getDownloadId());
                                                    if (status.toString().equals("UNKNOWN")) {
                                                        mTimerTask.cancel();
                                                        SharedPreferences sharedPreferences = ctx.getSharedPreferences("speed", Context.MODE_PRIVATE);
                                                        String time = sharedPreferences.getString("network", "");
                                                        assert time != null;
                                                        String[] tim = time.split(":");

                                                        if (!time.isEmpty()) {
                                                            String tTime = tim[1];
                                                            int totalTime = Integer.parseInt(tTime);
                                                            int speed = finalTotalSize1 / totalTime;
                                                            SharedPreferences sharedPreferences1 = ctx.getSharedPreferences("NetworkSpeed", Context.MODE_PRIVATE);
                                                            SharedPreferences.Editor editor = sharedPreferences1.edit();
                                                            editor.putString("downloadspeed", speed + " Kbps");
                                                            editor.apply();
                                                            WifiManager wifiManager = (WifiManager) ctx.getSystemService(Context.WIFI_SERVICE);
                                                            WifiInfo wifiInfo = wifiManager.getConnectionInfo();
                                                            if (wifiInfo != null) {
                                                                Integer linkSpeed = wifiInfo.getLinkSpeed(); //measured using WifiInfo.LINK_SPEED_UNITS
                                                                Log.e("timerrrr", String.valueOf(linkSpeed));
                                                            }
                                                            Log.e("timerrrr", String.valueOf(speed));
                                                        }
                                                        holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
                                                        holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                                                        holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(GONE);
                                                        holder.courseInnerItemLayoutBinding.PushResumeLay.setVisibility(GONE);
                                                        holder.courseInnerItemLayoutBinding.imgPushContent.setVisibility(GONE);
                                                        holder.courseInnerItemLayoutBinding.imgResumeContent.setVisibility(GONE);
                                                        holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(VISIBLE);
                                                        CourseItemFragment.isdownload = false;
                                                        if (CourseItemFragment.hashMap != null && !CourseItemFragment.hashMap.isEmpty()) {
                                                            String key = null;
                                                            for (String pair : CourseItemFragment.hashMap.keySet()) {
                                                                key = pair;
                                                            }
                                                            if (CourseItemFragment.fileidarray != null && !CourseItemFragment.fileidarray.isEmpty()) {
                                                                key = CourseItemFragment.fileidarray.get(0);
                                                            }
                                                            multiplefiledownload(CourseItemFragment.hashMap.get(key), CourseItemFragment.hashMap.get(key).get(0).getHolder());
                                                            //Toast.makeText(ctx, "start", Toast.LENGTH_SHORT).show();
                                                        }
                                                    }
                                                }
//                                                Log.e("status", status.toString());
                                                /*if (downloadIdArray.size() == signedUrl.size() && finalI == signedUrl.size() - 1) {
                                                    holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
                                                    holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                                                    holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(GONE);
                                                    holder.courseInnerItemLayoutBinding.PushResumeLay.setVisibility(GONE);
                                                    holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(VISIBLE);
                                                }*/

                                            } catch (NoSuchAlgorithmException e) {
                                                Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                                e.printStackTrace();
                                            } catch (NoSuchPaddingException e) {
                                                Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                                e.printStackTrace();
                                            } catch (InvalidKeyException e) {
                                                Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                                e.printStackTrace();
                                            } catch (InvalidAlgorithmParameterException e) {
                                                Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                                e.printStackTrace();
                                            } catch (SecurityException e) {
                                                if (e.getLocalizedMessage().equalsIgnoreCase("Invalid or expired API Key")) {
                                                    showAlertDialog(ctx, holder);
                                                }
                                                e.printStackTrace();
                                            } catch (IOException e) {
                                                if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                                                    showNoSpaceAlert(ctx);
                                                }
                                                Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                                                e.printStackTrace();
                                            }
                                            return null;
                                        }
                                    }, encryptDecryptObject).execute();
                                }

                                @Override
                                public void onError(Error error) {
                                    CourseItemFragment.isdownload = false;
                                    CourseItemFragment.hashMap.remove(downloadQueueObjects.get(0).getLemodel().getId());
                                    //handleDownloadCounterWhenFailAndComplite(downloadQueueObjects.get(finalI));
                                    if (error.isServerError()) {
                                        //Toast.makeText(ctx, "=== DOWNLAOD ServerError===", Toast.LENGTH_SHORT).show();
                                    } else if (error.isConnectionError()) {
                                        //Toast.makeText(ctx, "=== DOWNLAOD ConnectionError===", Toast.LENGTH_SHORT).show();
                                    } else if (error.isENOSPCError()) {
                                        //Toast.makeText(ctx, "=== DOWNLAOD ENOSPCError (No space left on device)===", Toast.LENGTH_SHORT).show();
                                        showNoSpaceAlert(ctx);
                                    }
                                    //hideDialog();
                                    Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), error.getError());
                                    showDialogNoNetwork();

                                    //Log.e("COUNTER", "======onError==0101====");
                                }
                            });
                    downloadIdArray.add(new DownloadProgress(downloadId));


                } catch (Exception e) {
                    e.printStackTrace();
                    //Toast.makeText(ctx, "=== DOWNLAOD CATCH===" + e.getMessage(), Toast.LENGTH_SHORT).show();
                    //Log.e("COUNTER", "======Exception======");
                }
            }
            return null;
        }

        @Override
        protected void onPostExecute(final String success) {

            Log.e("onPostExecute", "onPostExecute:");
            holder.courseInnerItemLayoutBinding.imgPushContent.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    holder.courseInnerItemLayoutBinding.imgResumeContent.setVisibility(View.VISIBLE);
                    holder.courseInnerItemLayoutBinding.imgPushContent.setVisibility(GONE);
                    PRDownloader.pause(downloadId);
                }
            });

            holder.courseInnerItemLayoutBinding.imgResumeContent.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    holder.courseInnerItemLayoutBinding.imgResumeContent.setVisibility(GONE);
                    holder.courseInnerItemLayoutBinding.imgPushContent.setVisibility(View.VISIBLE);
                    PRDownloader.resume(downloadId);
                }
            });
        }
    }

    public void doTimerTask() {

        mTimerTask = new TimerTask() {
            public void run() {
                handler.post(new Runnable() {
                    public void run() {
                        second++;
                        SimpleDateFormat sdf = new SimpleDateFormat("mm:ss", Locale.ENGLISH);
                        Calendar now = Calendar.getInstance();
                        now.set(Calendar.HOUR, 0);
                        now.set(Calendar.MINUTE, 0);

                        now.set(Calendar.SECOND, second);
                        String time = sdf.format(now.getTime());

                        SharedPreferences sharedPreferences = ctx.getSharedPreferences("speed", Context.MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPreferences.edit();
                        editor.putString("network", time);
                        editor.apply();
                        //Log.e("time", time);
                       /*if (nCounter<10){
                           tvCheckin.setText("CheckOut (00:0" + nCounter +")");
                       }else {
                           tvCheckin.setText("CheckOut (00:" + nCounter +")");
                       }*/
                        // update TextView


                        //Log.d("TIMER", "TimerTask run");
                    }
                });
            }
        };

        // public void schedule (TimerTask task, long delay, long period)
        t.schedule(mTimerTask, 1000, 1000);  //

    }


    public class DownloadStatusTask extends AsyncTask<Void, Void, String> {

        String fileURL;
        MyViewHolder holder;
        CoursePriviewObject.Lessonfiles lessonsModel;
        int downloadId = 0;
        long progressval = 0;
        CoursePriviewObject.Lessons model;
        DownloadQueueObject downloadQueueObject;

        public DownloadStatusTask(String fileURL, MyViewHolder holder, CoursePriviewObject.Lessonfiles lessonsModel, CoursePriviewObject.Lessons model, DownloadQueueObject downloadQueueObject) {
            this.fileURL = fileURL;
            this.holder = holder;
            this.lessonsModel = lessonsModel;
            this.model = model;
            this.downloadQueueObject = downloadQueueObject;
        }

        @Override
        protected String doInBackground(Void... params) {
            // TODO: attempt authentication against a network service.

            String EncryptFileName = lessonsModel.getFiles().getFilename();
            String filetype = lessonsModel.getFiles().getFiletypename();
            String fileName = lessonsModel.getId() + "_" + lessonsModel.getFiles().getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);

            String downloadFilePath = Const.destPath + userIdSlash + fileName;

            //Log.e(Const.LOG_NOON_TAG, "====downloadFilePath===" + downloadFilePath);

            try {
                String str = lessonsModel.getId() + "_" + lessonsModel.getFiles().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
                PRDownloader.cleanUp(1);

                //Log.e("COUNTER", "======downloadCount===IF===" + downloadCount);

                downloadId = PRDownloader.download(fileURL, Const.destPath + userIdSlash, fileName)
                        .build()
                        .setOnStartOrResumeListener(new OnStartOrResumeListener() {
                            @Override
                            public void onStartOrResume() {
                                //hideDialog();
                                holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                                holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(View.VISIBLE);
                                holder.courseInnerItemLayoutBinding.PushResumeLay.setVisibility(View.VISIBLE);
                                holder.courseInnerItemLayoutBinding.imgPushContent.setVisibility(View.VISIBLE);
                                holder.courseInnerItemLayoutBinding.imgcloseContent.setVisibility(GONE);

                                Toast.makeText(ctx, ctx.getString(R.string.downloading) + model.getName(), Toast.LENGTH_SHORT).show();

                                new AsyncTask<Void, Void, String>() {
                                    @Override
                                    protected String doInBackground(Void... voids) {
                                        FileDownloadStatus fileDownloadStatus = new FileDownloadStatus();
                                        fileDownloadStatus.setLessonid("0");
                                        fileDownloadStatus.setFileid(lessonsModel.getFiles().getId());
                                        fileDownloadStatus.setDownloadID(downloadId);
                                        AppDatabase.getAppDatabase(ctx).fileDownloadStatusDao().insertAll(fileDownloadStatus);
                                        return null;
                                    }
                                }.execute();

                                //Log.e("COUNTER", "======onStartOrResume======");
                                showDialogNoNetwork();

                            }
                        })
                        .setOnPauseListener(new OnPauseListener() {
                            @Override
                            public void onPause() {
                                //Log.e("COUNTER", "======setOnPauseListener======");
                                showDialogNoNetwork();

                            }
                        })
                        .setOnCancelListener(new OnCancelListener() {
                            @Override
                            public void onCancel() {
                                //handleDownloadCounterWhenFailAndComplite(downloadQueueObject);
                                //Log.e("COUNTER", "======setOnCancelListener======");
                                showDialogNoNetwork();
                                PRDownloader.cancelAll();
                            }
                        })
                        .setOnProgressListener(new OnProgressListener() {
                            @Override
                            public void onProgress(Progress progress) {
                                progressval = (progress.currentBytes * 100) / progress.totalBytes;
                                holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setProgress((int) progressval);
                                Log.e("-------------", "==== 4444444444 =====" + progressval);
                                //Log.e("COUNTER", "======setOnProgressListener======");
                                showDialogNoNetwork();

                            }
                        })
                        .start(new OnDownloadListener() {
                            @Override
                            public void onDownloadComplete() {

                                //Log.e("COUNTER", "======onDownloadComplete======");

                                BaseActivity.freeMemory(ctx);

                                EncryptDecryptObject encryptDecryptObject = new EncryptDecryptObject();
                                encryptDecryptObject.setSelectedVideoPath(downloadFilePath);
                                encryptDecryptObject.setFilename(str);
                                encryptDecryptObject.setHolder(holder);
                                new SetEncryptDecryptTask(new EncryptDecryptAsyncResponse() {
                                    @Override
                                    public EncryptDecryptObject getEncryptDecryptObjects(EncryptDecryptObject encryptDecryptObjects) {

                                        //handleDownloadCounterWhenFailAndComplite(downloadQueueObject);
                                        try {
                                            BaseActivity.freeMemory(ctx);
                                            String selectedVideoPath = encryptDecryptObject.getSelectedVideoPath();
                                            String filename = encryptDecryptObject.getFilename();
                                            MyViewHolder holder = encryptDecryptObject.getHolder();

                                            encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename;
                                            EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
                                            holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                                            holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(GONE);
                                            holder.courseInnerItemLayoutBinding.PushResumeLay.setVisibility(GONE);
                                            holder.courseInnerItemLayoutBinding.layPlayContent.setVisibility(VISIBLE);

                                            Log.e("000FILESIZE", "---queueArray---" + CourseItemFragment.queueArray.size()
                                                    + "--lessonfiles---" + lessonfiles.size());
                                            if (lessonfiles.size() == CourseItemFragment.queueArray.size()) {
                                                holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                                                holder.courseInnerItemLayoutBinding.progressBarSpinnerLayout.progressBar.setVisibility(GONE);
                                            }

                                            /*if (CourseItemFragment.queueArray.size() > 0) {
                                                CourseItemFragment.queueArray.remove(downloadQueueObject);
                                                CourseItemFragment.isdownload = true;
                                                if (CourseItemFragment.queueArray.size() != 0) {
                                                    downloadProcess(CourseItemFragment.queueArray.get(0));
                                                }
                                            }*/


                                            com.downloader.Status fileStatus = PRDownloader.getStatus(downloadId);
                                            FileDownloadStatus fileDownloadStatus = AppDatabase.getAppDatabase(ctx).fileDownloadStatusDao().getItemFileDownloadStatus(lessonsModel.getFiles().getId());
                                            if (fileDownloadStatus != null) {
                                                AppDatabase.getAppDatabase(ctx).fileDownloadStatusDao().updateItemFileDownloadStatus(lessonsModel.getFiles().getId(), fileStatus.toString(), (int) progressval);
                                            }

                                        } catch (NoSuchAlgorithmException e) {
                                            Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (NoSuchPaddingException e) {
                                            Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidKeyException e) {
                                            Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidAlgorithmParameterException e) {
                                            Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (SecurityException e) {
                                            if (e.getLocalizedMessage().equalsIgnoreCase("Invalid or expired API Key")) {
                                                showAlertDialog(ctx, holder);
                                            }
                                            e.printStackTrace();
                                        } catch (IOException e) {
                                            if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                                                showNoSpaceAlert(ctx);
                                            }
                                            Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                                            e.printStackTrace();
                                        }

                                        return null;
                                    }
                                }, encryptDecryptObject).execute();
                            }

                            @Override
                            public void onError(Error error) {
                                //handle
                                // DownloadCounterWhenFailAndComplite(downloadQueueObject);
                                if (error.isServerError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ServerError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isConnectionError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ConnectionError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isENOSPCError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ENOSPCError (No space left on device)===", Toast.LENGTH_SHORT).show();
                                    showNoSpaceAlert(ctx);
                                }
                                //hideDialog();
                                Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), error.getError());
                                showDialogNoNetwork();

                                //Log.e("COUNTER", "======onError==0101====");
                            }
                        });

            } catch (Exception e) {
                //Toast.makeText(ctx, "=== DOWNLAOD CATCH===" + e.getMessage(), Toast.LENGTH_SHORT).show();
                //Log.e("COUNTER", "======Exception======");
            }

            return null;
        }

        @Override
        protected void onPostExecute(final String success) {

            Log.e("onPostExecute", "onPostExecute: ");
            holder.courseInnerItemLayoutBinding.imgPushContent.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    holder.courseInnerItemLayoutBinding.imgResumeContent.setVisibility(View.VISIBLE);
                    holder.courseInnerItemLayoutBinding.imgPushContent.setVisibility(GONE);
                    PRDownloader.pause(downloadId);
                }
            });

            holder.courseInnerItemLayoutBinding.imgResumeContent.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    holder.courseInnerItemLayoutBinding.imgResumeContent.setVisibility(GONE);
                    holder.courseInnerItemLayoutBinding.imgPushContent.setVisibility(View.VISIBLE);
                    PRDownloader.resume(downloadId);
                }
            });
        }
    }

    public static void showNoSpaceAlert(Context activity) {

        ((Activity) activity).runOnUiThread(new Runnable() {
            public void run() {
                try {

                    SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.error_no_space));
                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
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
                inFile = new File(selectedVideoPath);
                outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename);
                EncrypterDecryptAlgo.encrypt(key, paramSpec, new FileInputStream(inFile), new FileOutputStream(outFile));
                inFile.delete();
                BaseActivity.freeMemory(ctx);

            } catch (NoSuchAlgorithmException e) {
                ((Activity) ctx).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (NoSuchPaddingException e) {
                ((Activity) ctx).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (InvalidKeyException e) {
                ((Activity) ctx).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (InvalidAlgorithmParameterException e) {
                ((Activity) ctx).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (IOException e) {
                //encryptArray = new ArrayList<>();
                //downloadArray = new ArrayList<>();
                BaseActivity.freeMemory(ctx);
                if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                    showNoSpaceAlert(ctx);
                }
                Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                e.printStackTrace();
            }
            return encryptDecryptObject;
        }

        @Override
        protected void onPostExecute(EncryptDecryptObject encryptDecryptObject) {
            delegate.getEncryptDecryptObjects(encryptDecryptObject);
        }

    }

    public static boolean isNetworkAvailable(Context context) {
        return isNetworkConnected;
    }

    @Override
    public void onInternetConnectivityChanged(boolean isConnected) {
        if (isConnected) {
            isNetworkConnected = true;
        } else {
            isNetworkConnected = false;
        }
    }

    public void nextItem(int position, MyViewHolder holder) {
        playItem(position, 1, holder, false);
    }

    public void showDialog(String message) {
        if (mProgressDialog == null) {
            mProgressDialog = new ProgressDialog(ctx);
            mProgressDialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            mProgressDialog.setCancelable(true);
            mProgressDialog.setCanceledOnTouchOutside(false);
        }
        mProgressDialog.setMessage(message);
        mProgressDialog.show();
    }

    protected void hideDialog() {
        if (mProgressDialog != null && mProgressDialog.isShowing()) {
            mProgressDialog.dismiss();
        }
    }

    public void handleDownloadCounterWhenFailAndComplite(DownloadQueueObject downloadQueueObject) {

        BaseActivity.freeMemory(ctx);

        if (CourseItemFragment.queueArray.size() > 0) {
            CourseItemFragment.queueArray.remove(downloadQueueObject);
            CourseItemFragment.isdownload = true;
            if (CourseItemFragment.queueArray.size() != 0) {
                downloadProcess(CourseItemFragment.queueArray.get(0));
            }
        }
    }

    public boolean addQueue(DownloadQueueObject downloadQueueObject) {

        BaseActivity.freeMemory(ctx);

        if (CourseItemFragment.queueArray.size() < 10) {
            CourseItemFragment.queueArray.add(downloadQueueObject);
            return true;
        } else {
            //Toast.makeText(ctx, R.string.validation_maximum_download, Toast.LENGTH_SHORT).show();
            return false;
        }
    }

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }

    public void showNetworkAlert(Context activity) {
        try {
            SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.validation_Connect_internet));
            AlertDialog.Builder builder = new AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                        }
                    });

            builder.show();

           /* TextView textView = (TextView) builder.findViewById(android.R.id.message); //to change font size
            // to change font family
             Typeface face = Typeface.createFromAsset(getAssets(),"font/fontFileName.ttf");*/

            //TextView textView = (TextView)  builder.findViewById(android.R.id.message);

            //Typeface typeface = activity.getResources().getFont(R.font.bahij_helvetica_neue_bold);
            //or to support all versions use


        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    /*@Override
    public long getItemId(int position) {
        return position;
    }*/

    @Override
    public int getItemViewType(int position) {
        return position;
    }


    public void showDialogNoNetwork() {

        // boolean isnetwork = NetworkChangeReceiver.isnetwork();

        ConnectivityManager connectivityManager = (ConnectivityManager) ctx.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        boolean isnetwork = activeNetworkInfo != null && activeNetworkInfo.isConnectedOrConnecting();

        //Log.e("COUNTER", "======555555======" + isnetwork);

        if (isnetwork == false) {
            try {
                //Log.e("COUNTER", "======66666======");
                SpannableStringBuilder message = setTypeface(ctx, ctx.getResources().getString(R.string.continueApp));
                AlertDialog myAlertDialog = null;
                AlertDialog.Builder builder = new AlertDialog.Builder(ctx);
                // builder.setTitle("Title");
                builder.setMessage(message);
                builder.setPositiveButton(R.string.ok, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int arg1) {
                        dialog.dismiss();
                        PRDownloader.cancelAll();
                        Intent i = new Intent(ctx, MainDashBoardActivity.class);
                        ctx.startActivity(i);
                    }
                });
                builder.setCancelable(false);
                myAlertDialog = builder.create();

                // Log.e("COUNTER", "======000000======");

                if (!myAlertDialog.isShowing()) {
                    if (!isdialog) {
                        myAlertDialog.show();
                        isdialog = true;
                    }
                }

            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }


    // Alert When First time start download
    public void showDialogcancle(int position, MyViewHolder holder) {
        try {

            AlertDialog myAlertDialog = null;
            SpannableStringBuilder message = setTypeface(ctx, ctx.getResources().getString(R.string.alertTitleWhenFirstTimeDownload));
            AlertDialog.Builder builder = new AlertDialog.Builder(ctx);

            builder.setMessage(message);
            builder.setPositiveButton(R.string.ok, new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int arg1) {
                    dialog.dismiss();
                    startDownload(position, holder);
                }
            });
            builder.setNegativeButton(R.string.cancel, new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int arg1) {
                    dialog.dismiss();
                    holder.courseInnerItemLayoutBinding.imgdownloadContent.setEnabled(true);
                    holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
                }
            });

            builder.setCancelable(false);
            myAlertDialog = builder.create();
            myAlertDialog.show();

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public void showCustomDialogcancle(int position, MyViewHolder holder) {
        DownloadPopupLayoutBinding downloadPopupLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(ctx), R.layout.download_popup_layout, null, false);
        AlertDialog.Builder builder = new AlertDialog.Builder(ctx);

        //setting the view of the builder to our custom view that we already inflated
        builder.setView(downloadPopupLayoutBinding.getRoot());

        //finally creating the alert dialog and displaying it
        final AlertDialog alertDialog = builder.create();
        //alertDialog.getWindow().setBackgroundDrawable(new ColorDrawable(android.graphics.Color.TRANSPARENT));
        downloadPopupLayoutBinding.txtDownloadOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                alertDialog.dismiss();
                startDownload(position, holder);
            }
        });

        downloadPopupLayoutBinding.txtDownloadCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                alertDialog.cancel();
            }
        });
        alertDialog.show();
    }

    ArrayList<CoursePriviewObject.Lessonfiles> lessonfiles = new ArrayList<>();

    private void startDownload(int position, MyViewHolder holder) {
        isdialog = false;

        String[] PERMISSIONS = new String[]{android.Manifest.permission.WRITE_EXTERNAL_STORAGE};
        ArrayList<DownloadQueueObject> downloadQueueObjects = new ArrayList<>();
        if (!EasyPermissions.hasPermissions(ctx, PERMISSIONS)) {
            EasyPermissions.requestPermissions((Activity) ctx, ctx.getResources().getString(R.string.validation_download_permission), 0x01, PERMISSIONS);
        } else {
            Log.e("onPostExecute", "startDownload: ");
            CoursePriviewObject.Lessons model = list.get(position);
            ArrayList<CoursePriviewObject.Lessonfiles> lessonfiles = new ArrayList<>();
            Collections.addAll(lessonfiles, model.getLessonfiles());
            if (lessonfiles.size() != 0) {
                // Call Api for SignedURL
                if (isNetworkAvailable(ctx)) {
                    int arrayvalue = lessonfiles.size();
                    int count = 0;
                    for (int i = 0; i < lessonfiles.size(); i++) {
                        count++;
                        CoursePriviewObject.Lessonfiles lessonsModel = lessonfiles.get(i);
                        String str = lessonsModel.getId() + "_" + lessonsModel.getFiles().getId() + "_" + lessonsModel.getFiles().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + lessonsModel.getFiles().getFiletypename() + Const.extension;
                        File file = new File(Const.destPath + userIdSlash, str);

                        if (file.exists()) {
                            file.delete();
                        }

                        DownloadQueueObject downloadQueueObject = new DownloadQueueObject();
                        downloadQueueObject.setFileid(lessonsModel.getFiles().getId());
                        downloadQueueObject.setLessonID("0");
                        downloadQueueObject.setHolder(holder);
                        downloadQueueObject.setLessonsModel(lessonsModel);
                        downloadQueueObject.setLemodel(model);

                        downloadQueueObjects.add(downloadQueueObject);
                        if (count == arrayvalue) {
                            lessonfiles.clear();
                            Log.e("Lession files array", String.valueOf(lessonfiles.size()));
                        }
                       /* if (addQueue(downloadQueueObject)) {
                            downloadProcess(downloadQueueObject);
                        }*/
                    }
                    multiplefiledownload(downloadQueueObjects, holder);
                } else {
                    showNetworkAlert(ctx);
                }
            }
        }

    }


    public void refreshToken(DownloadQueueObject downloadQueueObject) {

        String authid = PrefUtils.getAuthid(ctx);
        if (!TextUtils.isEmpty(authid)) {
            AuthTokenObject authTokenObject = AppDatabase.getAppDatabase(ctx).authTokenDao().getauthTokenData(authid);
            if (authTokenObject != null) {

                String accessToken = "";
                String idToken = "";
                String refreshToken = "";

                if (authTokenObject.getAccessToken() != null) {
                    accessToken = authTokenObject.getAccessToken();
                }
                if (authTokenObject.getIdToken() != null) {
                    idToken = authTokenObject.getIdToken();
                }

                if (authTokenObject.getRefreshToken() != null) {
                    refreshToken = authTokenObject.getRefreshToken();
                }

                if (!TextUtils.isEmpty(accessToken) && !TextUtils.isEmpty(idToken) && !TextUtils.isEmpty(refreshToken)) {

                    if (!TextUtils.isEmpty(authTokenObject.getExpiresAt())) {
                        try {
                            Date ExpiresAtDate = null;
                            Date CurrantAtDate = null;
                            SimpleDateFormat inputFormat = new SimpleDateFormat("E MMM dd HH:mm:ss Z yyyy");
                            ExpiresAtDate = inputFormat.parse(authTokenObject.getExpiresAt());
                            CurrantAtDate = new Date();

                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY==ExpiresAtDate==" + ExpiresAtDate);
                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY==CurrantAtDate==" + CurrantAtDate);

                            //if (CurrantAtDate.compareTo(ExpiresAtDate) >= 0) {

                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY==SAME DATE==");
                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---refreshToken-----11---" + refreshToken);
                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---getExpiresAt-----11---" + authTokenObject.getExpiresAt());

                            Auth0 account = new Auth0(ctx.getString(R.string.com_auth0_client_id), ctx.getString(R.string.com_auth0_domain));
                            account.setOIDCConformant(true);
                            AuthenticationAPIClient client = new AuthenticationAPIClient(account);
                            client.renewAuth(refreshToken)
                                    .addParameter("scope", ctx.getString(R.string.com_auth0_scope))
                                    .start(new BaseCallback<Credentials, AuthenticationException>() {
                                        @Override
                                        public void onSuccess(Credentials credentials) {

                                            // Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---SUCCESS--------" + credentials.getExpiresAt());

                                            try {
                                                String[] split = new String[0];
                                                split = JWTUtils.decoded(credentials.getIdToken());
                                                String jsonBody = JWTUtils.getJson(split[1]);
                                                JSONObject jsonObj = new JSONObject(jsonBody);

                                                String exp = jsonObj.get(Const.LOG_NOON_EXP).toString();
                                                String sub = jsonObj.get(Const.LOG_NOON_SUB).toString();

                                                AppDatabase.getAppDatabase(ctx).authTokenDao().updateToken(sub,
                                                        credentials.getAccessToken(),
                                                        credentials.getIdToken(),
                                                        credentials.getExpiresIn(),
                                                        credentials.getScope(),
                                                        String.valueOf(credentials.getExpiresAt()),
                                                        exp);
                                                PrefUtils.storeAuthid(ctx, sub);

                                                // callApifetchSignedUrl(downloadQueueObject);

                                            } catch (Exception e) {
                                                // Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---Exception----222----" + e.getMessage());

                                               /* ((Activity) mycontext).runOnUiThread(new Runnable() {
                                                    @Override
                                                    public void run() {
                                                        Toast.makeText(getApplicationContext(), e.getMessage(), Toast.LENGTH_LONG).show();

                                                        // Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY--CATCH-credentials--------" + e.getMessage());
                                                    }
                                                });*/

                                            }
                                        }

                                        @Override
                                        public void onFailure(AuthenticationException error) {
                                            //FAILURE
                                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---Exception---000-----" + error.getMessage());
                                           /* ((Activity) mycontext).runOnUiThread(new Runnable() {
                                                @Override
                                                public void run() {
                                                    //Toast.makeText(getApplicationContext(), "==AuthenticationException onFailure==" + error.getMessage(), Toast.LENGTH_LONG).show();
                                                    //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---AuthenticationException--------" + error.getMessage());
                                                }
                                            });*/

                                        }
                                    });


                        } catch (ParseException e) {
                            e.printStackTrace();
                        }
                    }
                }
            }
        }
    }
}