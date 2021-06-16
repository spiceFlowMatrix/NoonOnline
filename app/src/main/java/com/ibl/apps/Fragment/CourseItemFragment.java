package com.ibl.apps.Fragment;

import android.annotation.SuppressLint;
import android.app.Dialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.Observer;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.work.Constraints;
import androidx.work.Data;
import androidx.work.NetworkType;
import androidx.work.OneTimeWorkRequest;
import androidx.work.WorkInfo;
import androidx.work.WorkManager;
import androidx.work.WorkQuery;

import com.downloader.Error;
import com.downloader.OnDownloadListener;
import com.downloader.PRDownloader;
import com.downloader.PRDownloaderConfig;
import com.github.barteksc.pdfviewer.listener.OnLoadCompleteListener;
import com.github.barteksc.pdfviewer.listener.OnPageChangeListener;
import com.github.barteksc.pdfviewer.listener.OnRenderListener;
import com.google.common.util.concurrent.ListenableFuture;
import com.google.firebase.analytics.FirebaseAnalytics;
import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonSyntaxException;
import com.ibl.apps.Adapter.CourseItemInnerListAdapter;
import com.ibl.apps.Adapter.CourseItemListAdapter;
import com.ibl.apps.Adapter.ImageviewAdapter;
import com.ibl.apps.Adapter.QuestionViewAdapter;
import com.ibl.apps.Adapter.SummaryQuestionViewAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Interface.CourseHideResponse;
import com.ibl.apps.Interface.CourseInnerItemInterface;
import com.ibl.apps.Interface.CourseItemAsyncResponse;
import com.ibl.apps.Interface.IOnBackPressed;
import com.ibl.apps.Interface.QuizItemClickInterface;
import com.ibl.apps.Interface.QuizQuestionItemClickInterface;
import com.ibl.apps.Interface.ToolbarHideInterface;
import com.ibl.apps.LessonManagement.LessonRepository;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.DownloadQueueObject;
import com.ibl.apps.Model.ProgressItem;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.QuizManament.QuizRepository;
import com.ibl.apps.QuizModule.entities.AnswerEntity;
import com.ibl.apps.QuizModule.entities.QuestionEntity;
import com.ibl.apps.QuizModule.entities.QuestionWithAnswerEntity;
import com.ibl.apps.QuizModule.entities.QuizEntity;
import com.ibl.apps.QuizModule.entities.QuizWithQuestionEntity;
import com.ibl.apps.QuizModule.workers.FetchQuestionWorker;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.LessonDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.ChapterProgress;
import com.ibl.apps.RoomDatabase.entity.FileProgress;
import com.ibl.apps.RoomDatabase.entity.LessonNewProgress;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.QuizProgress;
import com.ibl.apps.RoomDatabase.entity.QuizUserResult;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.AssignmentDetailActivity;
import com.ibl.apps.noon.CacheEventsListActivity;
import com.ibl.apps.noon.ChapterActivity;
import com.ibl.apps.noon.MainDashBoardActivity;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CourselessonLayoutBinding;
import com.ibl.apps.noon.databinding.DialogViewerItemLayoutBinding;
import com.ibl.apps.noon.databinding.HitLimitDialogBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.WrapContentLinearLayoutManager;

import java.io.File;
import java.lang.reflect.Array;
import java.security.spec.AlgorithmParameterSpec;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Objects;
import java.util.TimeZone;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.TimeUnit;

import javax.crypto.spec.IvParameterSpec;

import fr.maxcom.libmedia.Licensing;
import io.reactivex.Observable;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.annotations.NonNull;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.disposables.Disposable;
import io.reactivex.functions.Consumer;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import io.reactivex.schedulers.Timed;
import retrofit2.HttpException;
import tcking.github.com.giraffeplayer2.DefaultMediaController;
import tcking.github.com.giraffeplayer2.GiraffePlayer;
import tcking.github.com.giraffeplayer2.PlayerListener;
import tcking.github.com.giraffeplayer2.VideoInfo;
import tv.danmaku.ijk.media.player.IjkTimedText;

import static android.content.Context.MODE_PRIVATE;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.chapterProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.fileProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.lessonProgressList;
import static com.ibl.apps.Adapter.CourseItemInnerListAdapter.quizProgressList;
import static com.ibl.apps.Base.BaseActivity.freeMemory;

public class CourseItemFragment extends BaseFragment implements View.OnClickListener, View.OnTouchListener, CourseInnerItemInterface, IOnBackPressed, CourseHideResponse {

    CourselessonLayoutBinding fragmentCourseItemLayoutBinding;

    CoursePriviewObject coursePriviewObject;
    ArrayList<CoursePriviewObject.Chapters> coursePriviewArrayList = new ArrayList<>();
    ArrayList<CoursePriviewObject.Lessons> lessonsArrayList = new ArrayList<>();
    CourseItemListAdapter courseItemListAdapter;
    private CompositeDisposable disposable = new CompositeDisposable();
    DialogViewerItemLayoutBinding dialogViewerItemLayoutBinding;
    ToolbarHideInterface toolbarHideInterface;
    List<QuizMainObject.Questions> quizQuestionsObjectList = new ArrayList<>();

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    private String mParam1;
    private String mParam2;
    String GradeId, CourseName, LessonName;
    String fileType = "";
    String fileTypeName = "";
    String lessonID = "";
    public String quizID = "";
    String videoUri = "";
    String chapterid = "";
    String fileid = "";
    int position = 0;
    int newcurrantProgress = 0;
    int TotalPDFpage = 0;
    int SelectPDFpage = 0;
    File file;
    Button[] myTextViews;
    Disposable Disposabletimer;
    UserDetails userDetailsObject = new UserDetails();
    String userId = "0";
    String ActivityFlag = "";
    String LessonID = "";
    String QuizID = "";
    Boolean isNotification = false;
    String AddtionalLibrary, AddtionalDiscussions, AddtionalAssignment = "";
    public static ArrayList<DownloadQueueObject> queueArray = new ArrayList<DownloadQueueObject>();
    public static boolean isdownload = true;
    public static HashMap<String, ArrayList<DownloadQueueObject>> hashMap = new HashMap<>();
    public static ArrayList<String> fileidarray = new ArrayList<>();
    //public static ArrayList<String> fileidarray = new ArrayList<>();
    SummaryQuestionViewAdapter summaryQuestionViewAdapter;
    byte[] iv;
    static boolean isStatus = false;
    AlgorithmParameterSpec paramSpec;
    static boolean oneTime = true;

    UserDetails userDetailsarr = null;
    private LessonRepository lessonRepository;
    private QuizRepository quizRepository;
    private QuizDatabaseRepository quizDatabaseRepository;
    private CourseDatabaseRepository courseDatabaseRepository;
    private LessonDatabaseRepository lessonDatabaseRepository;
    Observer<Integer> observer;
    private SyncAPIDatabaseRepository syncAPIDatabaseRepository;
    private String gradeName;
    private CoursePriviewObject coursePriviewObjectList;
    private String assignmentSubscription;

    private ArrayList<String> quizIdArrayList = new ArrayList<>();
    private String myQuizId = "";
    AppDatabase appDatabase = AppDatabase.getAppDatabase(getContext());
    private boolean startedQuizDownloading = false;

    private boolean isVisible = false;

    public CourseItemFragment() {
        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Log.e("IIIII", getArguments().toString());
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }

        try {
            toolbarHideInterface = (ToolbarHideInterface) getActivity();
        } catch (ClassCastException e) {
            e.printStackTrace();
        }
        setRetainInstance(true);
    }

    @Override
    public void onViewStateRestored(@Nullable Bundle savedInstanceState) {
        super.onViewStateRestored(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        fragmentCourseItemLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.courselesson_layout, container, false);
        observer = new Observer<Integer>() {
            @Override
            public void onChanged(Integer integer) {

                if (getFragmentManager() != null && integer == 0) {
                    fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.appbarCourseChapter.setVisibility(View.VISIBLE);
                    setUp(fragmentCourseItemLayoutBinding.getRoot());
                    PRDownloader.cancelAll();
                    NoonApplication.isDownloadable = false;
                    ChapterActivity.pageNo1.setValue(-1);
                }

            }
        };
        if (!ChapterActivity.isAdd) {
            ChapterActivity.isAdd = true;
            ChapterActivity.pageNo1.observe(getViewLifecycleOwner(), observer);
        }
        return fragmentCourseItemLayoutBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        lessonRepository = new LessonRepository();
        quizRepository = new QuizRepository();
        quizDatabaseRepository = new QuizDatabaseRepository();
        courseDatabaseRepository = new CourseDatabaseRepository();
        lessonDatabaseRepository = new LessonDatabaseRepository();
        syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();

        queueArray.clear();
        hashMap.clear();
        fileidarray.clear();
        isdownload = true;
        coursePriviewArrayList.clear();
        iv = new byte[Const.IV_LENGTH];
        paramSpec = new IvParameterSpec(iv);
        setOrientationDynamically(getActivity().getResources().getConfiguration().orientation);
        showDialog(getString(R.string.loading));

        Bundle bundle = this.getArguments();
        if (bundle != null) {
            GradeId = bundle.getString(Const.GradeID, "");
            CourseName = bundle.getString(Const.CourseName, "");
            gradeName = bundle.getString("GradeName", "");
            ActivityFlag = bundle.getString(Const.ActivityFlag, "");
            LessonID = bundle.getString(Const.LessonID, "");
            QuizID = bundle.getString(Const.QuizID, "");
            isNotification = bundle.getBoolean(Const.isNotification, false);
            AddtionalLibrary = bundle.getString(Const.AddtionalLibrary, "");
            AddtionalAssignment = bundle.getString(Const.AddtionalAssignment, "");
            AddtionalDiscussions = bundle.getString(Const.AddtionalDiscussions, "");
            fragmentCourseItemLayoutBinding.CourseName.setText(CourseName);
        }

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                userDetailsarr = userDetails;
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();
                    assignmentSubscription = userDetailsObject.getIs_assignment_authorized();
                    loadData();
                    SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();

                    SharedPreferences sharedPreferenceCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                    String flagStatus = sharedPreferenceCache.getString("FlagStatus", "");
                    switch (flagStatus) {
                        case "1":
                            fragmentCourseItemLayoutBinding.cacheEventsStatusBtn.setImageResource(R.drawable.svg_pending);
                            break;
                        case "2":
                            fragmentCourseItemLayoutBinding.cacheEventsStatusBtn.setImageResource(R.drawable.svg_error);
                            break;
                        case "3":
                            fragmentCourseItemLayoutBinding.cacheEventsStatusBtn.setImageResource(R.drawable.svg_sync);
                            break;
                        case "4":
                            GlideApp.with(getActivity())
                                    .load(R.drawable.svg_empty)
                                    .error(R.drawable.svg_empty)
                                    .into(fragmentCourseItemLayoutBinding.cacheEventsStatusBtn);
                            break;
                    }

                    fragmentCourseItemLayoutBinding.cacheEventsStatusBtn.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            startActivity(new Intent(NoonApplication.getContext(), CacheEventsListActivity.class));
                            NoonApplication.isDownloadable = false;
                        }
                    });

                    if (syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size() >= 50) {
                        NoonApplication.cacheStatus = 2;
                        SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                        if (editor != null) {
                            editor.clear();
                            editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                            editor.apply();
                        }
                        showHitLimitDialog(getActivity());
                    }
                    Licensing.allow(NoonApplication.getContext());
                }
                return null;
            }

        }).execute();
        setOnClickListener();
    }

    @Override
    public void onDestroy() {
        if (observer != null) {
            ChapterActivity.isAdd = false;
            ChapterActivity.pageNo1.removeObserver(observer);
        }
        super.onDestroy();
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.pdfviewNext:
                int nextSelectPDFpage = (SelectPDFpage + 1);
                if (nextSelectPDFpage == TotalPDFpage) {
                    int coursePosition = -1;
                    for (int i = 0; i < courseItemListAdapter.getItems().size(); i++) {
                        if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                            coursePosition = i;
                            break;
                        }
                    }
                    if (coursePosition != -1) {
                        if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildCount() > 0) {
                            CourseItemListAdapter.MyViewHolder test = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(coursePosition)));
                            if (test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter() != null) {
                                if (test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter().getItemCount() >= position) {
                                    CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(position + 1));
                                    //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                    ((CourseItemInnerListAdapter) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter()).nextItem(position + 1, holder);
                                }
                            }
                        }
                    }
                } else {
                    //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfViewPager.jumpTo(nextSelectPDFpage);
                }
                break;
            case R.id.pdfviewPrivious:

                if (SelectPDFpage == 0) {

                    int coursePosition = -1;
                    for (int i = 0; i < courseItemListAdapter.getItems().size(); i++) {
                        if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                            coursePosition = i;
                            break;
                        }
                    }
                    if (coursePosition != -1) {
                        if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildCount() > 0) {
                            CourseItemListAdapter.MyViewHolder test = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(coursePosition)));
                            if (test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter() != null) {
                                if (position != 0) {
                                    CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(position - 1));
                                    //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                    ((CourseItemInnerListAdapter) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter()).nextItem(position - 1, holder);

                                } else {
                                    Toast.makeText(getActivity(), R.string.error_no_found_previous_lesson, Toast.LENGTH_LONG).show();
                                }
                            }
                        }
                    }
                } else {
                    //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfViewPager.jumpTo(SelectPDFpage - 1);
                }
                break;
            case R.id.FullScreenImage:
                openDialogViewer(fileType, fileTypeName, videoUri, position, lessonID, newcurrantProgress, quizID, CourseName, LessonName, fileid);
                break;
            case R.id.imageViewDialog:
                openDialogViewer(fileType, fileTypeName, videoUri, position, lessonID, 0, quizID, CourseName, LessonName, fileid);
                break;

            case R.id.txtAssignmnent:
                if (isNetworkAvailable(getActivity())) {
                    CoursePriviewObject.Assignment model = null;
                    for (CoursePriviewObject.Lessons lessons : lessonsArrayList) {
                        if (lessons.getAssignment() != null) {
                            if (lessons.getId().equalsIgnoreCase(lessonID)) {
                                model = lessons.getAssignment();
                            }
                        }
                    }
                    Log.e("AddtionalAssignment", "onClick: " + assignmentSubscription);
                    if (assignmentSubscription != null) {
                        if (assignmentSubscription.equals("true")) {
                            if (model != null) {
                                Intent intent = new Intent(getActivity(), AssignmentDetailActivity.class);
                                intent.putExtra(Const.Assignment, new Gson().toJson(model));
                                intent.putExtra(Const.Flag, 1);
                                intent.putExtra("coursename", CourseName);
                                intent.putExtra("lessonname", LessonName);
                                getActivity().startActivity(intent);
                            } else {
                                Toast.makeText(getActivity(), getActivity().getString(R.string.no_assignment_available), Toast.LENGTH_SHORT).show();
                            }
                        } else {
                            Toast.makeText(getActivity(), getActivity().getResources().getString(R.string.assignment_unauthorize_warning), Toast.LENGTH_SHORT).show();
                        }
                    }
                } else {
                    Log.e("NNNNNN","1");
                    showNetworkAlert(getActivity());
                }
                break;

            case R.id.btnbackcourseItem:
                if (getActivity() != null)
                    startActivity(new Intent(getActivity(), MainDashBoardActivity.class));
                break;
        }
    }

    @Override
    public boolean onTouch(View view, MotionEvent motionEvent) {
        int action = motionEvent.getAction();
        switch (view.getId()) {
            case R.id.transparentImage:
                switch (action) {
                    default:
                        break;
                }
                break;
            case R.id.QuiztransparentImage:
                switch (action) {
                   /* case MotionEvent.ACTION_DOWN:
                        // Disallow ScrollView to intercept touch events.
                        fragmentCourseItemLayoutBinding.mainFragmentCourseScroll.requestDisallowInterceptTouchEvent(true);
                        // Disable touch on transparent view
                        return false;

                    case MotionEvent.ACTION_UP:
                        // Allow ScrollView to intercept touch events.
                        fragmentCourseItemLayoutBinding.mainFragmentCourseScroll.requestDisallowInterceptTouchEvent(false);
                        return true;

                    case MotionEvent.ACTION_MOVE:
                        fragmentCourseItemLayoutBinding.mainFragmentCourseScroll.requestDisallowInterceptTouchEvent(true);
                        return false;
*/
                    default:
                        break;
                }
                break;

        }
        return false;
    }

    @Override
    public void courseInnerItem(Context ctx, String fileType, String fileTypeName, String videoUri, int position, String lessonID, final int currantProgress, String quizID, int playpushflag, String chapterid, String fileid, String LessonName) {
        try {
            System.gc();

            FirebaseAnalytics mFirebaseAnalytics;
            mFirebaseAnalytics = FirebaseAnalytics.getInstance(getActivity());
            mFirebaseAnalytics.setCurrentScreen(getActivity(), getClass().getSimpleName(), null);

            if (userDetailsObject != null) {

                String userName = "";
                String fullName = "";

                if (userDetailsObject.getUsername() != null) {
                    userName = userDetailsObject.getUsername();
                }

                if (userDetailsObject.getFullName() != null) {
                    fullName = userDetailsObject.getFullName();
                }

                Bundle bundle = new Bundle();
                bundle.putString(Const.NOON_USERID, userId);
                bundle.putString(Const.NOON_USERNAME, userName);
                bundle.putString(Const.NOON_FULLNAME, fullName);
                bundle.putString(Const.NOON_GRADEID, GradeId);
                bundle.putString(Const.NOON_LESSONID, lessonID);
                bundle.putString(Const.NOON_CHAPTERID, chapterid);
                bundle.putString(Const.NOON_FILEID, fileid);
                bundle.putString(Const.NOON_QUIZID, quizID);
                mFirebaseAnalytics.logEvent("CourseDetailScreen", bundle);
            }

            if (videoUri != null) {

                this.fileType = fileType;
                this.fileTypeName = fileTypeName;
                this.videoUri = videoUri;
                this.quizID = quizID;
                this.lessonID = lessonID;
                this.LessonName = LessonName;
                this.position = position;
                this.chapterid = chapterid;
                this.fileid = fileid;


                //---------- Check same video is alredy play
                if (fileType.equals("2")) {

                    //Log.e(Const.LOG_NOON_TAG, "=======11111====");

                    boolean isPlayingVideoview = fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().isPlaying();
                    String videoviewTagValue = (String) fragmentCourseItemLayoutBinding.videoViewer.videoView.getTag(R.string.playvideotag_key);
                    if (isPlayingVideoview && !TextUtils.isEmpty(videoviewTagValue)) {
                        if (videoviewTagValue.equalsIgnoreCase(fileid)) {
                            //Toast.makeText(ctx, "not cnay chaneg go back", Toast.LENGTH_SHORT).show();
                            //fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().seekTo(0);
                            return;
                        } else {
                            fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().stop();
                            fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().releaseMediaPlayer();
                        }
                    } else {
                        fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().stop();
                        fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().releaseMediaPlayer();
                    }
                } else {
                    fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().stop();
                    fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().releaseMediaPlayer();
                }

                final int[] currProgress = {currantProgress};
                PRDownloaderConfig config = PRDownloaderConfig.newBuilder()
                        .setDatabaseEnabled(true)
                        .setReadTimeout(30_000)
                        .setConnectTimeout(30_000)
                        .build();
                PRDownloader.initialize(getActivity(), config);

                String yourFilePath = getActivity().getDir(Const.dir_fileName, MODE_PRIVATE).getAbsolutePath();
                try {

                    // For FileData Delete from dataDir
                    File file1 = new File(yourFilePath);
                    String[] files;
                    files = file1.list();
                    for (int i = 0; i < files.length; i++) {
                        File myFile = new File(file1, files[i]);
                        myFile.delete();
                    }
                    // For FileData Delete from catch (Clear catch)
                    File dir = getActivity().getCacheDir();
                    deleteDir(dir);
                    freeMemory(getActivity());
                } catch (Exception e) {
                    e.printStackTrace();
                }

                switch (fileType) {
                    case "1":
                        fragmentCourseItemLayoutBinding.txtText.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View view) {
                                fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().pause();


                                final Dialog dialog = new Dialog(getActivity(), android.R.style.Theme_Translucent_NoTitleBar);
                                dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                                dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                                dialog.setCancelable(true);
                                dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(getActivity()), R.layout.dialog_viewer_item_layout, null, false);
                                dialog.setContentView(dialogViewerItemLayoutBinding.getRoot());
                                dialog.show();

                                SharedPreferences sharedPreferenceCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                String flagStatus = sharedPreferenceCache.getString("FlagStatus", "");
                                switch (flagStatus) {
                                    case "1":
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfCacheEventsStatusBtn.setImageResource(R.drawable.svg_pending);
                                        break;
                                    case "2":
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfCacheEventsStatusBtn.setImageResource(R.drawable.svg_error);
                                        break;
                                    case "3":
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfCacheEventsStatusBtn.setImageResource(R.drawable.svg_sync);
                                        break;
                                    case "4":
                                        GlideApp.with(getActivity())
                                                .load(R.drawable.svg_empty)
                                                .error(R.drawable.svg_empty)
                                                .into(dialogViewerItemLayoutBinding.pdfViewLayout.pdfCacheEventsStatusBtn);
                                        break;
                                }

                                dialogViewerItemLayoutBinding.pdfViewLayout.pdfCacheEventsStatusBtn.setOnClickListener(new View.OnClickListener() {
                                    @Override
                                    public void onClick(View v) {
                                        getActivity().finish();
                                        startActivity(new Intent(NoonApplication.getContext(), CacheEventsListActivity.class));
                                    }
                                });

                                if (syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size() >= 50) {
                                    NoonApplication.cacheStatus = 2;
                                    SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                    if (editor != null) {
                                        editor.clear();
                                        editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                        editor.apply();
                                    }
                                    showHitLimitDialog(NoonApplication.getContext());
                                }
                                Log.e("getProgress", "pdfViewLayout:");
                                if (fileProgressList.size() > 0) {
                                    callApiSyncFiles(fileProgressList);
                                }

                                try {
                                    PRDownloader.download(videoUri, yourFilePath, Const.dir_fileName + Const.PDFextension).build().start(new OnDownloadListener() {
                                        @Override
                                        public void onDownloadComplete() {
                                            file = new File(getActivity().getDir(Const.dir_fileName, MODE_PRIVATE).getAbsolutePath() + File.separator + Const.dir_fileName + Const.PDFextension);

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
                                                            dialogViewerItemLayoutBinding.pdfViewLayout.txtPageCount.setText(getActivity().getString(R.string.page) + " " + (page + 1) + " " + getActivity().getString(R.string.of) + "  " + pageCount);
                                                            int countper = (int) ((page + 1) * 100 / pageCount);
                                                            dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setProgress(countper);
                                                            if (currProgress[0] < countper) {
                                                                new ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid, playpushflag).execute();
                                                                currProgress[0] = countper;
                                                                newcurrantProgress = countper;
                                                            }

                                                            TotalPDFpage = pageCount;
                                                            SelectPDFpage = page;
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
                                            //Log.e(Const.LOG_NOON_TAG, "0000" + "NO Space ----" + error.toString());
                                        }
                                    });
                                } catch (Exception e) {
                                    e.printStackTrace();
                                }

                                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
                                dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(CourseName);
                                dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(LessonName);


                                dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
                                    @Override
                                    public void onClick(View view) {
                                        fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().start();
                                        dialog.dismiss();
                                    }
                                });
                            }
                        });

                        SharedPreferences sharedPreferences = getActivity().getSharedPreferences("pdfurl", MODE_PRIVATE);
                        SharedPreferences.Editor editor = sharedPreferences.edit();
                        editor.putString("pdf", videoUri);
                        editor.apply();


                        break;
                    case "2":

                        /*fragmentCourseItemLayoutBinding.txtText.setOnClickListener(new View.OnClickListener() {
                            @Override
                            public void onClick(View view) {
                                SharedPreferences sharedPreferences= getActivity().getSharedPreferences("pdfurl",Context.MODE_PRIVATE);
                                String pdfurl = sharedPreferences.getString("pdf","");
                                final Dialog dialog = new Dialog(getActivity(), android.R.style.Theme_Translucent_NoTitleBar);
                                dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                                dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                                dialog.setCancelable(true);
                                dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(getActivity()), R.layout.dialog_viewer_item_layout, null, false);
                                dialog.setContentView(dialogViewerItemLayoutBinding.getRoot());
                                dialog.show();


                                try {
                                    PRDownloader.download(pdfurl, yourFilePath, Const.dir_fileName + Const.PDFextension).build().start(new OnDownloadListener() {
                                        @Override
                                        public void onDownloadComplete() {
                                            file = new FileData(getActivity().getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath() + FileData.separator + Const.dir_fileName + Const.PDFextension);

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
                                                            dialogViewerItemLayoutBinding.pdfViewLayout.txtPageCount.setText(getActivity().getString(R.string.page) + " " + (page + 1) + " " + getActivity().getString(R.string.of) + "  " + pageCount);
                                                            int countper = (int) ((page + 1) * 100 / pageCount);
                                                            dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setProgress(countper);
                                                            if (currProgress[0] < countper) {
                                                                new ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid, playpushflag).execute();
                                                                currProgress[0] = countper;
                                                                newcurrantProgress = countper;
                                                            }

                                                            TotalPDFpage = pageCount;
                                                            SelectPDFpage = page;
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
                                            //Log.e(Const.LOG_NOON_TAG, "0000" + "NO Space ----" + error.toString());
                                        }
                                    });
                                } catch (Exception e) {
                                    e.printStackTrace();

                                }

                                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
                                dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(CourseName);
                                dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(LessonName);
                                dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
                                    @Override
                                    public void onClick(View view) {
                                        dialog.dismiss();
                                    }
                                });
                            }
                        });*/
                        if (playpushflag != 0) {
                            toolbarHideInterface.toolbarHide(getActivity(), true, LessonName);
                            fragmentCourseItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
//                            pdfClick(yourFilePath, currProgress, ctx, playpushflag);
                            //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                            if (playpushflag != 0) {
                                setupvideoinlandscapmode();
                                fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.VISIBLE);
                                fragmentCourseItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.VISIBLE);
                                fragmentCourseItemLayoutBinding.courseitemView.setVisibility(View.VISIBLE);
                                fragmentCourseItemLayoutBinding.appbarCourseChapter.setVisibility(View.GONE);

                            } else {
                                fragmentCourseItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.GONE);
                                fragmentCourseItemLayoutBinding.courseitemView.setVisibility(View.GONE);
                                fragmentCourseItemLayoutBinding.appbarCourseChapter.setVisibility(View.VISIBLE);
                            }
                            fragmentCourseItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
                            fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
                            fragmentCourseItemLayoutBinding.videoViewer.videoView.setVisibility(View.GONE);
                            fragmentCourseItemLayoutBinding.videoViewer.appvideoloadingLay.setVisibility(View.VISIBLE);

                            if (fileProgressList.size() > 0) {
                                callApiSyncFiles(fileProgressList);
                            }
//                        fragmentCourseItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.VISIBLE);
//                        fragmentCourseItemLayoutBinding.courseitemView.setVisibility(View.VISIBLE);

                            //Log.e(Const.LOG_NOON_TAG, "====videoUri===" + videoUri);
                            try {
                                PRDownloader.download(videoUri, yourFilePath, Const.dir_fileName + Const.VIDEOextension).build().start(new OnDownloadListener() {
                                    @Override
                                    public void onDownloadComplete() {
                                        try {
                                            if (playpushflag != 0) {
                                                fragmentCourseItemLayoutBinding.videoViewer.videoView.setVisibility(View.VISIBLE);
                                                fragmentCourseItemLayoutBinding.videoViewer.appvideoloadingLay.setVisibility(View.GONE);
                                            }
                                            //holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(getActivity().getResources().getColor(R.color.colorBlack));
                                            if (userDetailsObject != null) {
                                                String userName = "";
                                                if (userDetailsObject != null) {
                                                    if (!TextUtils.isEmpty(userDetailsObject.getUsername())) {
                                                        userName = userDetailsObject.getUsername();
                                                    }
                                                }

                                                File file = new File(getActivity().getDir(Const.dir_fileName, MODE_PRIVATE).getAbsolutePath() + File.separator + Const.dir_fileName + Const.VIDEOextension);
                                                fragmentCourseItemLayoutBinding.videoViewer.videoView.getVideoInfo()
                                                        .setAspectRatio(VideoInfo.AR_MATCH_PARENT)
                                                        .setTitle(CourseName) //config title
                                                        .setSubTitle(LessonName) //config subtitle
                                                        .setusername(userName)//config Username
                                                        .setShowTopBar(true);

                                                Log.e("file------", String.valueOf(file));
                                                fragmentCourseItemLayoutBinding.videoViewer.videoView.setVideoPath(String.valueOf(file));

                                                if (playpushflag != 0) {
                                                    try {
                                                        fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().start();
                                                        fragmentCourseItemLayoutBinding.videoViewer.videoView.setTag(R.string.playvideotag_key, fileid);
                                                    } catch (Exception e) {
                                                        e.printStackTrace();
                                                    }
                                                } else {
                                                    // Toast.makeText(ctx, "HELLO", Toast.LENGTH_SHORT).show();
                                                    fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().stop();
                                                    fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().releaseMediaPlayer();
                                                }

                                                fragmentCourseItemLayoutBinding.videoViewer.videoView.setPlayerListener(new PlayerListener() {
                                                    @Override
                                                    public void onPrepared(GiraffePlayer giraffePlayer) {
                                                        Timer timer = new Timer();
                                                        timer.scheduleAtFixedRate(new TimerTask() {
                                                            @Override
                                                            public void run() {
                                                                try {
                                                                    //Log.e("ADAPTER", "===fileid===" + fileid);

                                                                    int currantpos = fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().getCurrentPosition();
                                                                    int duration = fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().getDuration();
                                                                    int percent = (currantpos + 1) * 100 / duration;
                                                                    if (currProgress[0] >= 100) {
                                                                        timer.cancel();
                                                                    } else if (currProgress[0] < percent) {
                                                                        if (0 != playpushflag) {
                                                                            new ProgressTask(lessonID, String.valueOf(percent), position, quizID, fileid, playpushflag).execute();
                                                                            currProgress[0] = percent;
                                                                        }
                                                                    }
                                                                } catch (Exception e) {
                                                                    timer.cancel();
                                                                }
                                                            }
                                                        }, 1000, 5000);
                                                    }

                                                    @Override
                                                    public void onBufferingUpdate(GiraffePlayer giraffePlayer, int percent) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===02");
                                                    }

                                                    @Override
                                                    public boolean onInfo(GiraffePlayer giraffePlayer, int what, int extra) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===03");
                                                        return false;
                                                    }

                                                    @Override
                                                    public void onCompletion(GiraffePlayer giraffePlayer) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===04" +giraffePlayer.getCurrentPosition());

                                                        try {
                                                            int currantpos = giraffePlayer.getCurrentPosition();
                                                            int duration = giraffePlayer.getDuration();

                                                            if (duration != 0) {
                                                                int percent = (currantpos + 1) * 100 / duration;
                                                                if (percent != 0 && percent >= 90) {
                                                                    new ProgressTask(lessonID, String.valueOf(100), position, quizID, fileid, playpushflag).execute();
                                                                }
                                                            }

                                                            giraffePlayer.stop();

                                                        } catch (Exception e) {
                                                            e.printStackTrace();
                                                        }
                                                    }

                                                    @Override
                                                    public void onSeekComplete(GiraffePlayer giraffePlayer) {
                                                        //  Log.e(Const.LOG_NOON_TAG, "===>>==percent===05" + giraffePlayer.getCurrentPosition());
                                                    }

                                                    @Override
                                                    public boolean onError(GiraffePlayer giraffePlayer, int what, int extra) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===06");
                                                        return false;
                                                    }

                                                    @Override
                                                    public void onPause(GiraffePlayer giraffePlayer) {

                                                    }

                                                    @Override
                                                    public void onRelease(GiraffePlayer giraffePlayer) {

                                                    }

                                                    @Override
                                                    public void onStart(GiraffePlayer giraffePlayer) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===07");

                                                    }

                                                    @Override
                                                    public void onTargetStateChange(int oldState, int newState) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===08");
                                                    }

                                                    @Override
                                                    public void onCurrentStateChange(int oldState, int newState) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===09");
                                                    }

                                                    @Override
                                                    public void onDisplayModelChange(int oldModel, int newModel) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===010");
                                                    }

                                                    @Override
                                                    public void onPreparing(GiraffePlayer giraffePlayer) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===011");
                                                    }

                                                    @Override
                                                    public void onTimedText(GiraffePlayer giraffePlayer, IjkTimedText text) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===012" + text);
                                                    }

                                                    @Override
                                                    public void onLazyLoadProgress(GiraffePlayer giraffePlayer, int progress) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===013");
                                                    }

                                                    @Override
                                                    public void onLazyLoadError(GiraffePlayer giraffePlayer, String message) {
                                                        //Log.e(Const.LOG_NOON_TAG, "===>>==percent===014");

                                                    }

                                                }, new DefaultMediaController.Onbackclick() {
                                                    @Override
                                                    public void backperfome() {
                                                        fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().stop();
                                                        fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().releaseMediaPlayer();
                                                        fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                                                        fragmentCourseItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.GONE);
                                                        fragmentCourseItemLayoutBinding.courseitemView.setVisibility(View.GONE);
                                                        fragmentCourseItemLayoutBinding.appbarCourseChapter.setVisibility(View.VISIBLE);
                                                        setOrientationDynamically(getActivity().getResources().getConfiguration().orientation);
                                                        for (int i = 0; i < courseItemListAdapter.getItems().size(); i++) {
                                                            if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(i) != null) {
                                                                CourseItemListAdapter.MyViewHolder mainHolder = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(i)));
                                                                if (mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter() != null) {
                                                                    if (mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter().getItemCount() >= position) {
                                                                        for (int j = 0; j < mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter().getItemCount(); j++) {
                                                                            if (mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(j) != null) {
                                                                                CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(j));
                                                                                if (holder != null) {
                                                                                    if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                                                                                        if (j == position) {
                                                                                            holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(getActivity().getResources().getColor(R.color.colorBlack));
                                                                                            holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        // holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
//                                                    Toast.makeText(ctx, "onback", Toast.LENGTH_SHORT).show();
                                                    }

                                                    @Override
                                                    public void fullscreenperfome() {
                                                        setupvideoinlandscapmode();
                                                    }
                                                });
                                            }
                                        } catch (Exception e) {
                                            //Log.e(Const.LOG_NOON_TAG, "2222" + "NO Space ====" + e.getMessage());

                                            //Toast.makeText(getActivity(), R.string.error_no_space, Toast.LENGTH_LONG).show();
                                        }
                                    }

                                    @Override
                                    public void onError(Error error) {

                                        courseInnerItem(ctx, fileType, fileTypeName, videoUri, position, lessonID, currantProgress, quizID, playpushflag, chapterid, fileid, LessonName);

                                        //Log.e(Const.LOG_NOON_TAG, "3333" + "NO Space ====" + error.toString());

                                        //Toast.makeText(getActivity(), R.string.error_no_space, Toast.LENGTH_LONG).show();
                                    }
                                });
                            } catch (Exception e) {
                                e.printStackTrace();


                                // Log.e(Const.LOG_NOON_TAG, "4444" + "NO Space" + e.getMessage());
                                //Toast.makeText(getActivity(), R.string.error_no_space, Toast.LENGTH_LONG).show();
                            }
                        }
                        break;
                    case "3":
                        toolbarHideInterface.toolbarHide(getActivity(), false, LessonName);
                        fragmentCourseItemLayoutBinding.appbarCourseChapter.setVisibility(View.GONE);
                        fragmentCourseItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
                        //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                        fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                        fragmentCourseItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.VISIBLE);
                        fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);

                        GlideApp.with(ctx)
                                .load(videoUri)
                                .error(R.drawable.ic_no_image_found)
                                .into(fragmentCourseItemLayoutBinding.imageViewLayout.imageViewDialog);

                        int countper = 100;
                        if (currantProgress < countper) {
                            new ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid, playpushflag).execute();
                        }
                        break;
                    case "10":
                        Log.e("MMMMM", "Inside case 10");
                        CallApiQuestionList(fragmentCourseItemLayoutBinding, getActivity(), quizID, position);
                        break;
                }

                for (int i = 0; i < courseItemListAdapter.getItems().size(); i++) {
                    if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(i) != null) {
                        CourseItemListAdapter.MyViewHolder mainHolder = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(i)));
                        if (mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter() != null) {
                            if (mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter().getItemCount() >= position) {
                                for (int j = 0; j < mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter().getItemCount(); j++) {
                                    if (mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(j) != null) {
                                        CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(j));
                                        if (holder != null) {
                                            if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                                                if (j == position) {
                                                    if (0 != playpushflag) {
                                                        holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(getActivity().getResources().getColor(R.color.colorGreen));
                                                        holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(false);
                                                    }
                                                } else {

                                                    holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(getActivity().getResources().getColor(R.color.colorBlack));
                                                    holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
                                                }
                                            } else {
                                                holder.courseInnerItemLayoutBinding.txtFileName.setTextColor(getActivity().getResources().getColor(R.color.colorBlack));
                                                holder.courseInnerItemLayoutBinding.cardItemLayout.setEnabled(true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public boolean onBackPressed() {
        return false;
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig) {
        super.onConfigurationChanged(newConfig);
        setOrientationDynamically(newConfig.orientation);
    }

    @Override
    public void courseHideItem(Context ctx, String fileType) {
        switch (fileType) {
            case "1":
                toolbarHideInterface.toolbarHide(getActivity(), true, LessonName);
                if (fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.getVisibility() == View.VISIBLE) {
                    fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.videoViewer.appvideoloadingLay.setVisibility(View.GONE);

                    fragmentCourseItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.courseitemView.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.appbarCourseChapter.setVisibility(View.VISIBLE);
                }
                fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().stop();
                fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().releaseMediaPlayer();
                setOrientationDynamically(getActivity().getResources().getConfiguration().orientation);
               /* if (fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.getVisibility() == View.VISIBLE) {
                    fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                }*/
                break;
            case "2":
                toolbarHideInterface.toolbarHide(getActivity(), true, LessonName);
                if (fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.getVisibility() == View.VISIBLE) {
                    fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.videoViewer.appvideoloadingLay.setVisibility(View.GONE);

                    fragmentCourseItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.courseitemView.setVisibility(View.GONE);
                    fragmentCourseItemLayoutBinding.appbarCourseChapter.setVisibility(View.VISIBLE);
                }
                fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().stop();
                fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().releaseMediaPlayer();
                setOrientationDynamically(getActivity().getResources().getConfiguration().orientation);
                break;
            case "3":
                break;
            case "10":
                setOrientationDynamically(getActivity().getResources().getConfiguration().orientation);
                break;
        }
    }

    @Override
    public void setUserVisibleHint(boolean isVisibleToUser) {
        super.setUserVisibleHint(isVisibleToUser);
        if (isVisibleToUser && ChapterActivity.pageNo == 0) {
            if (getFragmentManager() != null) {

            }
        }

    }

    public void setOnClickListener() {
        //fragmentCourseItemLayoutBinding.pdfViewLayout.FullScreenImage.setOnClickListener(this);
        fragmentCourseItemLayoutBinding.imageViewLayout.imageViewDialog.setOnClickListener(this);
        //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewNext.setOnClickListener(this);
        //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setOnClickListener(this);
        //fragmentCourseItemLayoutBinding.pdfViewLayout.transparentImage.setOnTouchListener(this);
        fragmentCourseItemLayoutBinding.quizViewLayout.QuiztransparentImage.setOnTouchListener(this);

        fragmentCourseItemLayoutBinding.txtAssignmnent.setOnClickListener(this);
        fragmentCourseItemLayoutBinding.btnbackcourseItem.setOnClickListener(this);
//        fragmentCourseItemLayoutBinding.txtText.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view) {
//                final Dialog dialog = new Dialog(getActivity(), android.R.style.Theme_Translucent_NoTitleBar);
//                dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
//                dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
//                dialog.setCancelable(true);
//                dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(getActivity()), R.layout.dialog_viewer_item_layout, null, false);
//                dialog.setContentView(dialogViewerItemLayoutBinding.getRoot());
//                dialog.show();
//
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fromFile(file)
//                        .enableSwipe(true) // allows to block changing pages using swipe
//                        .enableDoubletap(true)
//                        .defaultPage(0)
//                        .onLoad(new OnLoadCompleteListener() {
//                            @Override
//                            public void loadComplete(int i) {
//                                dialogViewerItemLayoutBinding.pdfViewLayout.progressDialogLay.placeholder.setVisibility(View.GONE);
//                                hideDialog();
//                            }
//                        })
//                        .onPageChange(new OnPageChangeListener() {
//                            @Override
//                            public void onPageChanged(int page, int pageCount) {
//
//                                dialogViewerItemLayoutBinding.pdfViewLayout.txtPageCount.setText(getActivity().getString(R.string.page) + " " + (page + 1) + " " + getActivity().getString(R.string.of) + "  " + pageCount);
//                                int countper = (int) ((page + 1) * 100 / pageCount);
//                                dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setProgress(countper);
//
//                                if (newcurrantProgress < countper) {
//                                    new ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid).execute();
//                                    newcurrantProgress = countper;
//                                }
//                            }
//                        })
//                        .enableAnnotationRendering(true)
//                        .onRender(new OnRenderListener() {
//                            @Override
//                            public void onInitiallyRendered(int i) {
//                              //  dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fitToWidth(fragmentCourseItemLayoutBinding.pdfViewLayout.pdfViewPager.getCurrentPage());
//                            }
//                        })
//                        .swipeHorizontal(true)
//                        .enableAntialiasing(true)
//                        .load();
//
//
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(CourseName);
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(LessonName);
//                dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
//                    @Override
//                    public void onClick(View view) {
//                        dialog.dismiss();
//                    }
//                });
//            }
//        });
    }

    private void loadData() {
        fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
        fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new WrapContentLinearLayoutManager(NoonApplication.getContext(), LinearLayoutManager.VERTICAL, false));

        courseItemListAdapter = new CourseItemListAdapter(getActivity(), coursePriviewArrayList, this, GradeId, userDetailsObject, ActivityFlag, LessonID, QuizID, isNotification, this, CourseName, assignmentSubscription);
        fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(courseItemListAdapter);

        if (isNetworkAvailable(getActivity())) {
            CallApiCoursePriviewList();
        } else {
            showDialog(getActivity().getString(R.string.loading));
            new setLocalDataTask(new CourseItemAsyncResponse() {
                @Override
                public CoursePriviewObject getCoursePriviewObject(CoursePriviewObject coursePriviewObject) {
                    if (coursePriviewObject != null) {
                        courseItemListAdapter.notifyDataSetChanged();
                    } else {
                        Log.e("NNNNNN","2");
                        showNetworkAlert(getActivity());
                    }

                    hideDialog();
                    return null;
                }
            }, coursePriviewObject, false).execute();
        }
    }

    private void CallApiCoursePriviewList() {
        try {
            showDialog(getString(R.string.loading));
            disposable.add(lessonRepository.fetchCoursePriview(GradeId, userId)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<CoursePriviewObject>() {
                        @Override
                        public void onSuccess(CoursePriviewObject coursePriviewObject) {
                            if(coursePriviewObject != null
                                    && coursePriviewObject.getData() != null
                                    && coursePriviewObject.getData().getChapters().size() > 0){
                                for(CoursePriviewObject.Lessons lesson : coursePriviewObject.getData().getChapters().get(0).getLessons()){
                                    if(lesson.getType() == 2){
                                        myQuizId = lesson.getId();
                                        break;
//                                        quizIdArrayList.add(lesson.getId());
                                    }
                                }
                            }

//                            for(CoursePriviewObject.Chapters chapter: coursePriviewObject.getData().getChapters()){
//
//                            }
                            Log.e("IIIII","Course preview object");
                            Log.e("IIIII",new Gson().toJson(coursePriviewObject));
                            coursePriviewObject.setUserId(userId);
                            courseItemListAdapter = new CourseItemListAdapter(getActivity(), coursePriviewArrayList, CourseItemFragment.this, GradeId, userDetailsObject, ActivityFlag, LessonID, QuizID, isNotification, CourseItemFragment.this, CourseName, assignmentSubscription);
                            fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(courseItemListAdapter);
                            new setLocalDataTask(new CourseItemAsyncResponse() {
                                @Override
                                public CoursePriviewObject getCoursePriviewObject(CoursePriviewObject coursePriviewObject) {
                                    coursePriviewObjectList = coursePriviewObject;
                                    if (coursePriviewObject != null) {
                                       /* if (fileProgressList.size() > 0) {
                                            callApiSyncFiles(fileProgressList);
                                        }*/
                                        if (lessonProgressList.size() >= 0) {
                                            callApiSyncLessonProgress(lessonProgressList);
                                        }
                                        if (quizProgressList.size() >= 0) {
                                            callApiSyncQuiz(quizProgressList);
                                        }
                                        if (chapterProgressList.size() >= 0) {
                                            callApiSyncChapter(chapterProgressList);
                                        }

                                        courseDatabaseRepository.insertCoursePreviewObjectData(coursePriviewObject);
                                    } else {
                                        Log.e("NNNNNN","3");
                                        showNetworkAlert(getActivity());
                                    }
                                    //  courseItemListAdapter.notifyDataSetChanged();
                                    Log.e("notifyChanged--1", "getCoursePriviewObject: ");
                                    hideDialog();
                                    return null;
                                }
                            }, coursePriviewObject, true).execute();

                            observeQuizSyncWork();
                        }

                        @Override
                        public void onError(Throwable e) {
                            hideDialog();

                            try {
                                HttpException error = (HttpException) e;
                                CoursePriviewObject coursePriviewObject = new Gson().fromJson(error.response().errorBody().string(), CoursePriviewObject.class);
                                //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, coursePriviewObject.getMessage());
                                new setLocalDataTask(new CourseItemAsyncResponse() {
                                    @Override
                                    public CoursePriviewObject getCoursePriviewObject(CoursePriviewObject coursePriviewObject) {

                                        if (coursePriviewObject != null) {
                                            courseItemListAdapter.notifyDataSetChanged();
                                        } else {
                                            Log.e("NNNNNN","4");
                                            showNetworkAlert(getActivity());
                                        }
                                        hideDialog();

                                        return null;
                                    }
                                }, coursePriviewObject, false).execute();
                            } catch (Exception e1) {
                                // showError(e);

                                new setLocalDataTask(new CourseItemAsyncResponse() {
                                    @Override
                                    public CoursePriviewObject getCoursePriviewObject(CoursePriviewObject coursePriviewObject) {

                                        if (coursePriviewObject != null) {
                                            courseItemListAdapter.notifyDataSetChanged();
                                        } else {
                                            showError(e);
                                        }
                                        hideDialog();

                                        return null;
                                    }
                                }, coursePriviewObject, false).execute();
                            }
                        }
                    }));

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void callApiSyncLessonProgress(ArrayList<LessonNewProgress> lessonNewProgress) {
        JsonArray array = new JsonArray();
        if (!lessonNewProgress.isEmpty()) {
            for (int i = 0; i < lessonNewProgress.size(); i++) {
                JsonObject jsonObject = new JsonObject();

                try {
                    if (!lessonNewProgress.get(i).getProgress().equals("0")) {
                        jsonObject.addProperty("chapterid", Integer.parseInt(lessonNewProgress.get(i).getChapterId()));
                        jsonObject.addProperty("lessonid", Integer.parseInt(lessonNewProgress.get(i).getLessonId()));
                        jsonObject.addProperty("userid", Integer.parseInt(lessonNewProgress.get(i).getUserId()));
                        jsonObject.addProperty("progress", Integer.parseInt(lessonNewProgress.get(i).getProgress()));
                        array.add(jsonObject);
                    }
                } catch (JsonSyntaxException e) {
                    e.printStackTrace();
                }
            }
        }
        if (array.size() != 0) {
            disposable.add(lessonRepository.getLessonProgressSync(array)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                        @Override
                        public void onSuccess(RestResponse restResponse) {
                            if (restResponse.getResponse_code().equals("0")) {
                                //  Log.e("getLessonProgressSync", "onSuccess: " + array.get(0).getAsString());
                                lessonProgressList.clear();
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            Log.e("onError", "===callApiSyncLessonProgress===: " + e.getMessage());
                            try {
                                if (!userId.equals("")) {
                                    SyncAPITable syncAPITable = new SyncAPITable();

                                    syncAPITable.setApi_name(getString(R.string.lesson_progressed));
                                    syncAPITable.setEndpoint_url("LessonProgress/LessonProgressSync");
                                    syncAPITable.setParameters(String.valueOf(array));
                                    syncAPITable.setHeaders(PrefUtils.getAuthid(getActivity()));
                                    syncAPITable.setStatus(getString(R.string.errored_status));
                                    syncAPITable.setDescription(e.getMessage());
                                    syncAPITable.setCreated_time(getUTCTime());
                                    syncAPITable.setGradeName(gradeName);
                                    syncAPITable.setCourseName(CourseName);
                                    syncAPITable.setUserid(Integer.parseInt(userId));
                                    syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                    NoonApplication.cacheStatus = 2;
                                    SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                    if (editor != null) {
                                        editor.clear();
                                        editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                        editor.apply();
                                    }
                                }
                            } catch (JsonSyntaxException exeption) {
                                exeption.printStackTrace();
                            }

                        }
                    }));
        }
    }

    private void callApiSyncChapter(ArrayList<ChapterProgress> chapterprogress) {
        JsonArray array = new JsonArray();
        if (!chapterprogress.isEmpty()) {
            for (int i = 0; i < chapterprogress.size(); i++) {
                JsonObject jsonObject = new JsonObject();
                try {
                    if (!chapterprogress.get(i).getProgress().equals("0")) {
                        jsonObject.addProperty("courseid", Integer.parseInt(chapterprogress.get(i).getCourseId()));
                        jsonObject.addProperty("chapterid", Integer.parseInt(chapterprogress.get(i).getChapterId()));
                        jsonObject.addProperty("userid", Integer.parseInt(chapterprogress.get(i).getUserId()));
                        jsonObject.addProperty("progress", Integer.parseInt(chapterprogress.get(i).getProgress()));
                        array.add(jsonObject);
                    }
                } catch (JsonSyntaxException e) {
                    e.printStackTrace();
                }
            }

        }
        if (array.size() != 0) {
            disposable.add(lessonRepository.getChapterProgressSync(array).subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                        @Override
                        public void onSuccess(RestResponse restResponse) {
                            if (restResponse.getResponse_code().equals("0")) {
                                chapterProgressList.clear();
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            Log.e("onError", "===callApiSyncChapter===: " + e.getMessage());
                            try {
                                if (!userId.equals("")) {
                                    SyncAPITable syncAPITable = new SyncAPITable();

                                    syncAPITable.setApi_name(getString(R.string.chapter_progressed));
                                    syncAPITable.setEndpoint_url("ChapterProgress/ChapterProgressSync");
                                    syncAPITable.setParameters(String.valueOf(array));
                                    syncAPITable.setHeaders(PrefUtils.getAuthid(getActivity()));
                                    syncAPITable.setStatus(getString(R.string.errored_status));
                                    syncAPITable.setDescription(e.getMessage());
                                    syncAPITable.setCreated_time(getUTCTime());
                                    syncAPITable.setGradeName(gradeName);
                                    syncAPITable.setCourseName(CourseName);
                                    syncAPITable.setUserid(Integer.parseInt(userId));
                                    syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                    NoonApplication.cacheStatus = 2;
                                    SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                    if (editor != null) {
                                        editor.clear();
                                        editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                        editor.apply();
                                    }
                                }
                            } catch (JsonSyntaxException exeption) {
                                exeption.printStackTrace();
                            }
                        }
                    }));
        }
    }

    private void callApiSyncFiles(ArrayList<FileProgress> fileProgressList) {
        JsonArray array = new JsonArray();
        if (!fileProgressList.isEmpty()) {
            for (int i = 0; i < fileProgressList.size(); i++) {
                JsonObject jsonObject = new JsonObject();
                Log.e("getProgress", "callApiSyncFiles: " + fileProgressList.get(i).getProgress());
                try {
                    if (!fileProgressList.get(i).getProgress().equals("0")) {
                        jsonObject.addProperty("lessonid", Integer.parseInt(fileProgressList.get(i).getLessonId()));
                        jsonObject.addProperty("fileid", Integer.parseInt(fileProgressList.get(i).getFileId()));
                        jsonObject.addProperty("userid", Integer.parseInt(fileProgressList.get(i).getUserId()));
                        jsonObject.addProperty("progress", Integer.parseInt(fileProgressList.get(i).getProgress()));
                        array.add(jsonObject);
                    }
                } catch (JsonSyntaxException e) {
                    e.printStackTrace();
                }
            }
        }

        if (array.size() != 0)
            disposable.add(lessonRepository.getFileProgressSync(array).subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                        @Override
                        public void onSuccess(RestResponse restResponse) {
                            if (restResponse.getResponse_code().equals("0")) {
                                fileProgressList.clear();
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            Log.e("onError", "====callApiSyncFiles=====: " + e.getMessage());
                            try {
                                if (!userId.equals("")) {
                                    SyncAPITable syncAPITable = new SyncAPITable();

                                    syncAPITable.setApi_name(getString(R.string.file_progressed));
                                    syncAPITable.setEndpoint_url("FileProgress/FileProgressSync");
                                    syncAPITable.setParameters(String.valueOf(array));
                                    syncAPITable.setHeaders(PrefUtils.getAuthid(getActivity()));
                                    syncAPITable.setStatus(getString(R.string.errored_status));
                                    syncAPITable.setDescription(e.getMessage());
                                    syncAPITable.setCreated_time(getUTCTime());
                                    syncAPITable.setGradeName(gradeName);
                                    syncAPITable.setCourseName(CourseName);
                                    syncAPITable.setUserid(Integer.parseInt(userId));
                                    syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                    NoonApplication.cacheStatus = 2;
                                    SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                    if (editor != null) {
                                        editor.clear();
                                        editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                        editor.apply();
                                    }
                                }
                            } catch (JsonSyntaxException exeption) {
                                exeption.printStackTrace();
                            }

                        }
                    }));
    }

    private void callApiSyncQuiz(ArrayList<QuizProgress> quizProgress) {
        JsonArray array = new JsonArray();
        if (!quizProgress.isEmpty()) {
            for (int i = 0; i < quizProgress.size(); i++) {
                JsonObject jsonObject = new JsonObject();

                try {
                    if (!quizProgress.get(i).getProgress().equals("0")) {
                        jsonObject.addProperty("chapterid", Integer.parseInt(quizProgress.get(i).getChapterId()));
                        jsonObject.addProperty("quizid", Integer.parseInt(quizProgress.get(i).getQuizId()));
                        jsonObject.addProperty("userid", Integer.parseInt(quizProgress.get(i).getUserId()));
                        jsonObject.addProperty("progress", Integer.parseInt(quizProgress.get(i).getProgress()));
                        array.add(jsonObject);
                    }
                } catch (JsonSyntaxException e) {
                    e.printStackTrace();
                }
            }
        }
        if (array.size() != 0) {
            disposable.add(quizRepository.getQuizProgressSync(array).subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                        @Override
                        public void onSuccess(RestResponse restResponse) {
                            if (restResponse.getResponse_code().equals("0")) {
                                quizProgressList.clear();
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            Log.e("onError", "====callApiSyncQuiz====: " + e.getMessage());
                            try {
                                if (!userId.equals("")) {
                                    SyncAPITable syncAPITable = new SyncAPITable();

                                    syncAPITable.setApi_name(getString(R.string.quiz_attempted));
                                    syncAPITable.setEndpoint_url("QuizProgress/QuizProgressSync");
                                    syncAPITable.setParameters(String.valueOf(array));
                                    syncAPITable.setHeaders(PrefUtils.getAuthid(getActivity()));
                                    syncAPITable.setStatus(getString(R.string.errored_status));
                                    syncAPITable.setDescription(e.getMessage());
                                    syncAPITable.setCreated_time(getUTCTime());
                                    syncAPITable.setGradeName(gradeName);
                                    syncAPITable.setCourseName(CourseName);
                                    syncAPITable.setUserid(Integer.parseInt(userId));
                                    syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                    NoonApplication.cacheStatus = 2;
                                    SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                    if (editor != null) {
                                        editor.clear();
                                        editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                        editor.apply();
                                    }
                                }
                            } catch (JsonSyntaxException exeption) {
                                exeption.printStackTrace();
                            }
                        }
                    }));
        }
    }

    private void pdfClick(String yourFilePath, int[] currProgress, Context ctx, int playpushflag) {
        fragmentCourseItemLayoutBinding.txtText.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final Dialog dialog = new Dialog(getActivity(), android.R.style.Theme_Translucent_NoTitleBar);
                dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                dialog.setCancelable(true);
                dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(getActivity()), R.layout.dialog_viewer_item_layout, null, false);
                dialog.setContentView(dialogViewerItemLayoutBinding.getRoot());


                try {
                    PRDownloader.download(videoUri, yourFilePath, Const.dir_fileName + Const.PDFextension).build().start(new OnDownloadListener() {
                        @Override
                        public void onDownloadComplete() {
                            file = new File(getActivity().getDir(Const.dir_fileName, MODE_PRIVATE).getAbsolutePath() + File.separator + Const.dir_fileName + Const.PDFextension);
                            Log.e("pdffile======", String.valueOf(file));

                            if (file.exists()) {
                                dialog.show();
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
                                                dialogViewerItemLayoutBinding.pdfViewLayout.txtPageCount.setText(getActivity().getString(R.string.page) + " " + (page + 1) + " " + getActivity().getString(R.string.of) + "  " + pageCount);
                                                int countper = (int) ((page + 1) * 100 / pageCount);
                                                dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setProgress(countper);
                                                if (currProgress[0] < countper) {
                                                    new ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid, playpushflag).execute();
                                                    currProgress[0] = countper;
                                                    newcurrantProgress = countper;
                                                }

                                                TotalPDFpage = pageCount;
                                                SelectPDFpage = page;
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
                            } else {
                                Toast.makeText(ctx, R.string.pdf_not_availble, Toast.LENGTH_SHORT).show();
                            }
                        }

                        @Override
                        public void onError(Error error) {
                            //Log.e(Const.LOG_NOON_TAG, "0000" + "NO Space ----" + error.toString());
                        }

                    });
                } catch (Exception e) {
                    e.printStackTrace();

                }

                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
                dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(CourseName);
                dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(LessonName);
                dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        dialog.dismiss();
                    }
                });
            }
        });
    }

    public void openDialogViewer(String fileType, String fileTypeName, String videoUri, int position, String lessonID, final int currantProgress, String quizID, String CourseName, String LessonName, String fileid) {
        final Dialog dialog = new Dialog(getActivity(), android.R.style.Theme_Translucent_NoTitleBar);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
        dialog.setCancelable(true);
        dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(getActivity()), R.layout.dialog_viewer_item_layout, null, false);
        dialog.setContentView(dialogViewerItemLayoutBinding.getRoot());
        dialog.show();

        switch (fileType) {
            case "1":
                showDialog(getString(R.string.loading));
                dialogViewerItemLayoutBinding.pdfViewLayout.FullScreenImage.setImageResource(R.drawable.ic_fullscreen_exit);
                dialogViewerItemLayoutBinding.videoViewLayout.videoViewLay.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
                dialogViewerItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);

                dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fromFile(file)
                        .enableSwipe(true) // allows to block changing pages using swipe
                        .enableDoubletap(true)
                        .defaultPage(0)
                        .onLoad(new OnLoadCompleteListener() {
                            @Override
                            public void loadComplete(int i) {
                                dialogViewerItemLayoutBinding.pdfViewLayout.progressDialogLay.placeholder.setVisibility(View.GONE);
                                hideDialog();
                                //Log.e(Const.LOG_NOON_TAG, "=====loadComplete==11==");
                            }
                        })
                        .onPageChange(new OnPageChangeListener() {
                            @Override
                            public void onPageChanged(int page, int pageCount) {

                                dialogViewerItemLayoutBinding.pdfViewLayout.txtPageCount.setText("Page" + " " + page + " " + "of" + " " + pageCount);
                                int countper = (int) ((page + 1) * 100 / pageCount);
                                dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setProgress(countper);

                                if (newcurrantProgress < countper) {
                                    new ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid, 0).execute();
                                    newcurrantProgress = countper;
                                }


                                TotalPDFpage = pageCount;
                                SelectPDFpage = page;
                                if (page == 0) {
                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setText(R.string.previous_lesson);
                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewNext.setText(R.string.greterThen);
                                } else {

                                    if (pageCount == (page + 1)) {
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setText(R.string.lessThen);
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewNext.setText(R.string.Finish);
                                    } else {
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setText(R.string.lessThen);
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewNext.setText(R.string.greterThen);
                                    }
                                }

                            }
                        })
                        .enableAnnotationRendering(true)
                        .onRender(new OnRenderListener() {
                            @Override
                            public void onInitiallyRendered(int i) {
                                // dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fitToWidth(fragmentCourseItemLayoutBinding.pdfViewLayout.pdfViewPager.getCurrentPage());
                            }
                        })
                        .swipeHorizontal(true)
                        .enableAntialiasing(true)
                        .load();

                dialogViewerItemLayoutBinding.pdfViewLayout.FullScreenImage.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        //fragmentCourseItemLayoutBinding.pdfViewLayout.FullScreenImage.setImageResource(R.drawable.ic_fullscreen);
                        dialog.dismiss();
                    }
                });
                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewNext.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        int nextSelectPDFpage = (SelectPDFpage + 1);
                        if (nextSelectPDFpage == TotalPDFpage) {
                            int coursePosition = -1;
                            for (int i = 0; i < courseItemListAdapter.getItems().size(); i++) {
                                if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                                    coursePosition = i;
                                    break;
                                }
                            }
                            if (coursePosition != -1) {
                                if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildCount() > 0) {
                                    CourseItemListAdapter.MyViewHolder test = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(coursePosition)));
                                    if (test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter() != null) {

                                        if (test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter().getItemCount() >= position) {
                                            CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(position + 1));
                                            //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                            dialog.dismiss();
                                            ((CourseItemInnerListAdapter) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter()).nextItem(position + 1, holder);
                                        }
                                    }
                                }
                            }
                        } else {
                            dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.jumpTo(nextSelectPDFpage);
                        }
                    }
                });

                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {

                        if (SelectPDFpage == 0) {
                            int coursePosition = -1;
                            for (int i = 0; i < courseItemListAdapter.getItems().size(); i++) {
                                if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                                    coursePosition = i;
                                    break;
                                }
                            }
                            if (coursePosition != -1) {
                                if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildCount() > 0) {
                                    CourseItemListAdapter.MyViewHolder test = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(coursePosition)));
                                    if (test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter() != null) {
                                        if (position != 0) {
                                            CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(position - 1));
                                            //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                            dialog.dismiss();
                                            ((CourseItemInnerListAdapter) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter()).nextItem(position - 1, holder);
                                        } else {
                                            Toast.makeText(getActivity(), R.string.error_no_found_previous_lesson, Toast.LENGTH_LONG).show();
                                        }
                                    }
                                }
                            }

                        } else {
                            dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.jumpTo(SelectPDFpage - 1);
                        }
                    }
                });

                dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(CourseName);
                dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(LessonName);
                dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        dialog.dismiss();
                    }
                });

                break;
            case "2":
                break;
            case "3":
                dialogViewerItemLayoutBinding.videoViewLayout.videoViewLay.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.VISIBLE);

                GlideApp.with(getActivity())
                        .load(videoUri)
                        .error(R.drawable.ic_no_image_found)
                        .into(dialogViewerItemLayoutBinding.imageViewLayout.imageViewDialog);

                break;
            case "10":
//                setOrientationDynamically(getActivity().getResources().getConfiguration().orientation);
                dialogViewerItemLayoutBinding.videoViewLayout.videoViewLay.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.VISIBLE);
                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
                //CallApiQuestionList(dialogViewerItemLayoutBinding, getActivity(), quizID, currProgress, position, dialog);
                break;
        }
    }

    private void observeQuizSyncWork(){
        if(!myQuizId.equals("")){
            WorkManager.getInstance(getActivity())
                    .getWorkInfosByTagLiveData("quiz-sync")
                    .observe(this, new Observer<List<WorkInfo>>() {
                        @Override
                        public void onChanged(List<WorkInfo> workInfos) {
                            for(WorkInfo workInfo: workInfos){
                                if(workInfo == null){
                                    continue;
                                }

                                // See if work manager is running or enqueued
                                if(workInfo.getState() == WorkInfo.State.RUNNING) {
                                    Data progressData = workInfo.getProgress();
                                    if (progressData.getString("PROGRESS") != null
                                            && progressData.getString("quiz_id") != null) {
                                        if (progressData.getString("quiz_id").equalsIgnoreCase(myQuizId)) {
                                            Log.e("IIIII",
                                                    "Current quiz work manager is running: " + progressData.getInt("PROGRESS", 0));
                                        } else {
                                            Log.e("IIIII",
                                                    "Current quiz work manager is not running: " + progressData.getInt("PROGRESS", 0));
                                        }
                                    }
                                }

                                if(workInfo.getState() == WorkInfo.State.SUCCEEDED && startedQuizDownloading){
                                    Log.e("IIIII","work manager ran successfully");
                                    Data data = workInfo.getOutputData();
                                    if(data.getString("quiz_id") != null && data.getString("quiz_id").equalsIgnoreCase(myQuizId)){
                                        setupvideoinlandscapmode();
                                        toolbarHideInterface.toolbarHide(getActivity(), true, LessonName);
                                        fragmentCourseItemLayoutBinding.appbarCourseChapter.setVisibility(View.VISIBLE);
                                        fragmentCourseItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
                                        //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                        fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                                        fragmentCourseItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
                                        fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.VISIBLE);
                                        fragmentCourseItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.GONE);
                                        fragmentCourseItemLayoutBinding.courseitemView.setVisibility(View.GONE);

                                        //showDialog(getString(R.string.loading));
                                        fragmentCourseItemLayoutBinding.quizViewLayout.progressDialogLay.progressBar.setVisibility(View.VISIBLE);
                                        fragmentCourseItemLayoutBinding.quizViewLayout.resultLayout.mainResultView.setVisibility(View.GONE);
                                        fragmentCourseItemLayoutBinding.quizViewLayout.ProgressButton.removeAllViews();
                                        fragmentCourseItemLayoutBinding.quizViewLayout.fragmentquesionViewpager.setPagingEnabled(false);

                                        QuizWithQuestionEntity quizWithQuestionEntity = appDatabase
                                                .quizDao().getQuizWithQuestionAndAnswer(Long.parseLong(data.getString("quiz_id")));
                                        QuizMainObject quizMainObject = convertDataToQuizMainObject(quizWithQuestionEntity);
                                        setQuizMainView(fragmentCourseItemLayoutBinding, getActivity(), quizMainObject);
                                        quizMainObject.setNewquizId(QuizID);
                                        quizMainObject.setUserId(userId);
//                                    quizDatabaseRepository.insertQuizAnswerData(quizMainObject);
                                        fragmentCourseItemLayoutBinding.quizViewLayout.progressDialogLay.progressBar.setVisibility(View.GONE);
                                    }
                                }
//
//                                if(workInfo.getState() == WorkInfo.State.FAILED){
//                                    Log.e("IIIII","work manager failed");
//                                    Data data = workInfo.getOutputData();
//                                    if(data.getString("quiz_id") != null && data.getString("quiz_id").equalsIgnoreCase(myQuizId)){
////                                    dialogViewerItemLayoutBinding.quizViewLayout.progressDialogLay.progressBar.setVisibility(View.GONE);
//                                        try {
//                                            if (userId != null && !userId.isEmpty()) {
//                                                QuizWithQuestionEntity quizWithQuestionEntity = appDatabase
//                                                        .quizDao().getQuizWithQuestionAndAnswer(Long.parseLong(data.getString("quiz_id")));
//                                                if(quizWithQuestionEntity != null){
//                                                    QuizMainObject quizMainObject = convertDataToQuizMainObject(quizWithQuestionEntity);
//                                                    setQuizMainView(fragmentCourseItemLayoutBinding, getActivity(), quizMainObject);
//                                                    quizMainObject.setNewquizId(QuizID);
//                                                    quizMainObject.setUserId(userId);
//                                                    dialogViewerItemLayoutBinding.quizViewLayout.progressDialogLay.progressBar.setVisibility(View.GONE);
//                                                } else{
//                                                    Toast.makeText(getContext(), "There is some error while fetching quiz data from server, You can wait or try again later.", Toast.LENGTH_LONG)
//                                                            .show();
//                                                }
//                                            }
//                                        } catch (Exception e1) {
//                                            Log.e("MMMMMM", e1.getMessage());
//                                            Toast.makeText(getContext(), "Something went wrong, Please try again later.", Toast.LENGTH_LONG)
//                                                    .show();
//                                        }
//                                    }
//                                }
                            }
                        }
                    });
        }
    }

    private void CallApiQuestionList(CourselessonLayoutBinding dialogViewerItemLayoutBinding, Context ctx, String quizID, int position) {
        try {
            new AsyncTask<Void, Void, String>() {
                @Override
                protected String doInBackground(Void... voids) {
                    quizDatabaseRepository.deleteSelectedAnswer();
                    return null;
                }
            }.execute();

            QuizEntity quizEntity = appDatabase.quizDao().getById(Long. parseLong(quizID));
            if(quizEntity == null){
                Constraints constraints = new Constraints.Builder()
                        .setRequiredNetworkType(NetworkType.CONNECTED)
                        .build();

                Data inputData = new Data.Builder()
                        .putString("quiz_id", quizID)
                        .build();

                OneTimeWorkRequest workRequest = new OneTimeWorkRequest.Builder(FetchQuestionWorker.class)
                        .setConstraints(constraints)
                        .setInputData(inputData)
                        .addTag("quiz-sync")
                        .build();

                WorkManager.getInstance(getActivity()).enqueue(workRequest);
                startedQuizDownloading = true;
                Toast.makeText(getContext(), getContext().getString(R.string.quiz_setup), Toast.LENGTH_LONG)
                        .show();
            }
            else{
                setupvideoinlandscapmode();
                toolbarHideInterface.toolbarHide(getActivity(), true, LessonName);
                dialogViewerItemLayoutBinding.appbarCourseChapter.setVisibility(View.VISIBLE);
                dialogViewerItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
                //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.VISIBLE);
                dialogViewerItemLayoutBinding.textPdfAssignmentLay.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.courseitemView.setVisibility(View.GONE);

                //showDialog(getString(R.string.loading));
                dialogViewerItemLayoutBinding.quizViewLayout.progressDialogLay.progressBar.setVisibility(View.VISIBLE);
                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.mainResultView.setVisibility(View.GONE);
                dialogViewerItemLayoutBinding.quizViewLayout.ProgressButton.removeAllViews();
                dialogViewerItemLayoutBinding.quizViewLayout.fragmentquesionViewpager.setPagingEnabled(false);

                QuizWithQuestionEntity quizWithQuestionEntity = appDatabase
                        .quizDao().getQuizWithQuestionAndAnswer(quizEntity.getId());
                QuizMainObject quizMainObject = convertDataToQuizMainObject(quizWithQuestionEntity);
                setQuizMainView(dialogViewerItemLayoutBinding, ctx, quizMainObject);
                dialogViewerItemLayoutBinding.quizViewLayout.progressDialogLay.progressBar.setVisibility(View.GONE);
            }
        } catch (Exception e) {
            Log.e("IIIII", Objects.requireNonNull(e.getLocalizedMessage()));
            e.printStackTrace();
            Toast.makeText(getContext(), e.getLocalizedMessage(), Toast.LENGTH_SHORT).show();
        }
    }

    public QuizMainObject convertDataToQuizMainObject(QuizWithQuestionEntity quizWithQuestionEntity){
        int numberOfQuestion = 10;
        Log.e("IIIII", "In ConvertDataToQuizMainObject ------------------");
        Log.e("IIIII", quizWithQuestionEntity.getQuiz().getId().toString());

        QuizMainObject quizMainObject = new QuizMainObject();
        quizMainObject.setNewquizId(quizWithQuestionEntity.getQuiz().getId().toString());

        QuizMainObject.Data data = new QuizMainObject.Data();
        data.setQuizid(quizWithQuestionEntity.getQuiz().getId().toString());
        data.setPassmark(quizWithQuestionEntity.getQuiz().getPassMark().toString());

        ArrayList<Long> test = new ArrayList<>();
        // Filter unpicked questions
        ArrayList<QuestionWithAnswerEntity> unPickedQuestionAndAnswers = new ArrayList<>();

        for(QuestionWithAnswerEntity que: quizWithQuestionEntity.getQuestions()){
            test.add(que.getQuestionEntity().getQuizId());
            if(!que.getQuestionEntity().getQuestionPicked()){
                unPickedQuestionAndAnswers.add(que);
            }
        }

        if(unPickedQuestionAndAnswers.size() < numberOfQuestion){
            appDatabase.questionDao().updateAllQuestionToUnpicked();
            unPickedQuestionAndAnswers.clear();
            unPickedQuestionAndAnswers.addAll(quizWithQuestionEntity.getQuestions());
        }

        Log.e("IIIII", new Gson().toJson(test));
        Log.e("IIIII", String.valueOf(unPickedQuestionAndAnswers.size()));

        // Convert it to quizMain Question and Answer
        List<QuizMainObject.Questions> questions = new ArrayList<>();
        for(QuestionWithAnswerEntity questionWithAnswerEntity: unPickedQuestionAndAnswers){
            QuizMainObject.Questions question = new QuizMainObject.Questions();
            question.setId(questionWithAnswerEntity.getQuestionEntity().getId().toString());
            question.setQuestiontext(questionWithAnswerEntity.getQuestionEntity().getQuestionText());
            question.setQuestiontype(questionWithAnswerEntity.getQuestionEntity().getQuestionType().toString());
            question.setQuestiontypeid(questionWithAnswerEntity.getQuestionEntity().getQuestionTypeId().toString());
            question.setExplanation(questionWithAnswerEntity.getQuestionEntity().getExplanation());
            question.setIsmultianswer(questionWithAnswerEntity.getQuestionEntity().getMultiAnswer().toString());
            question.setImages(new QuizMainObject.Images[0]);

            List<QuizMainObject.Answers> answersList = new ArrayList<>();
            QuizMainObject.Answers[] answers =
                    new QuizMainObject.Answers[questionWithAnswerEntity.getAnswers().size()];
            for(AnswerEntity answerEntity: questionWithAnswerEntity.getAnswers()){
                QuizMainObject.Answers answer = new QuizMainObject.Answers();
                answer.setId(answerEntity.getId().toString());
                answer.setQuizid(quizWithQuestionEntity.getQuiz().getId().toString());
                answer.setQuestionid(answerEntity.getQuestionId().toString());
                answer.setAnswer(answerEntity.getAnswer());
                answer.setExtratext(answerEntity.getExtraText());
                answer.setIscorrect(answerEntity.getCorrect().toString());
                answer.setImages(new QuizMainObject.Images[0]);
                answersList.add(answer);
            }
            question.setAnswers(answersList.toArray(answers));
            questions.add(question);
        }

        // Shuffle the unpicked questions
        Collections.shuffle(questions);

        // Picked 10 questions from shuffled question list
        List<QuizMainObject.Questions> finalList = new ArrayList<>(questions.subList(0, numberOfQuestion));

        Log.e("IIIII", String.valueOf(finalList.size()));

        // Update boolean of picked question of question table in database
        long[] idsArr = new long[numberOfQuestion];
        int index = 0;
        for(QuizMainObject.Questions temp : finalList){
            idsArr[index] = Long.parseLong(temp.getId());
            index++;
        }

        appDatabase.questionDao().updateQuestionToPicked(idsArr);

        Log.e("IIIII", "Question Picked this time");
        Log.e("IIIII", new Gson().toJson(idsArr));

        // Set questions in QuizMainObject
        data.setQuestions(finalList);
        quizMainObject.setData(data);
        return quizMainObject;
    }

    public void setQuizMainView(CourselessonLayoutBinding dialogViewerItemLayoutBinding, Context ctx, QuizMainObject quizMainObject) {
        Log.e("MMMMM","Quiz Main View called");
        if (quizMainObject != null) {
            Log.e("MMMMM", "Quiz Main Object is not null");
            quizQuestionsObjectList.clear();
            dialogViewerItemLayoutBinding.quizViewLayout.questionLayout.setVisibility(View.VISIBLE);
            quizQuestionsObjectList = quizMainObject.getData().getQuestions();
            String passingMark = quizMainObject.getData().getPassmark().split("\\.")[0];

            //Log.e(Const.LOG_NOON_TAG, "===== quizQuestionsObjectList ==" + quizQuestionsObjectList.size());

            myTextViews = new Button[quizQuestionsObjectList.size()];
            for (int i = 0; i < quizQuestionsObjectList.size(); i++) {
                LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                params.setMargins(0, 0, 0, 0);
                params.weight = 1.0f;
                Button btn = new Button(getActivity());
                btn.setId(i);
                btn.setBackgroundColor(0);
                dialogViewerItemLayoutBinding.quizViewLayout.ProgressButton.addView(btn, params);
                myTextViews[i] = btn;
                if (i == 0) {
                    btn.setBackgroundColor(getResources().getColor(R.color.colorGreen));
                } else {
                    btn.setBackgroundColor(getResources().getColor(R.color.colorMoreDarkGray));
                }
            }

            QuestionViewAdapter questionViewAdapter = new QuestionViewAdapter(ctx, quizQuestionsObjectList, quizID, new QuizItemClickInterface() {
                @Override
                public void quizItemClick(Context ctx, String nextID, String answerID, String questionID) {
                    goNextPage(dialogViewerItemLayoutBinding, ctx, nextID, answerID, questionID, quizQuestionsObjectList, position, passingMark);

                }
            });
            dialogViewerItemLayoutBinding.quizViewLayout.fragmentquesionViewpager.setAdapter(questionViewAdapter);

            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.rcSummaryLayout.rcVertical.setHasFixedSize(true);
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(getActivity());
            linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.rcSummaryLayout.rcVertical.setLayoutManager(linearLayoutManager);
            summaryQuestionViewAdapter = new SummaryQuestionViewAdapter(ctx, quizQuestionsObjectList, quizID, new QuizQuestionItemClickInterface() {
                @Override
                public void quizQuestionItemClick(Context ctx, String questionID, QuizMainObject.Images[] images, String questiontext, String explanation) {

                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.mainResultView.scrollTo(0, dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.explanationlay.getTop());
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.summarylay.setVisibility(View.GONE);
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.explanationlay.setVisibility(View.VISIBLE);
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatusButton.setVisibility(View.GONE);
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.txtExplainationQuestion.setText(questiontext);
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.txtExplanationDesc.setText(explanation);
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.txtExplainationQuestion.getSettings().setDefaultFontSize((int) getResources().getDimension(R.dimen._7sdp));
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.txtExplanationDesc.getSettings().setDefaultFontSize((int) getResources().getDimension(R.dimen._5sdp));

                    ArrayList<QuizMainObject.Images> listimage = new ArrayList<>();
                    Collections.addAll(listimage, images);
                    ImageviewAdapter imageviewAdapter = new ImageviewAdapter(ctx, listimage, 0);
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.questionExplainationImagePager.setAdapter(imageviewAdapter);
                }
            });
            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.rcSummaryLayout.rcVertical.setAdapter(summaryQuestionViewAdapter);
            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.explanationbackbutton.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.summarylay.setVisibility(View.VISIBLE);
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.explanationlay.setVisibility(View.GONE);
                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatusButton.setVisibility(View.VISIBLE);

                }
            });

            Disposabletimer = Observable.interval(1000L, TimeUnit.MILLISECONDS)
                    .timeInterval()
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribe(new Consumer<Timed<Long>>() {
                        @Override
                        public void accept(@NonNull Timed<Long> longTimed) throws Exception {

                            try {
                                if (longTimed != null && longTimed.value() != null) {
                                    //Log.e(Const.LOG_NOON_TAG, "=====" + longTimed.value());

                                    int hours = (int) (longTimed.value() / 3600);
                                    int minutes = (int) ((longTimed.value() % 3600) / 60);
                                    int seconds = (int) (longTimed.value() % 60);

                                    if (longTimed.value() < 60) {
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMIn.setText(longTimed.value() + " " + ctx.getResources().getString(R.string.seconds));
                                    } else if (longTimed.value() >= 60) {
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMIn.setText(twoDigitString(minutes) + " " + ctx.getResources().getString(R.string.minutes));// + " " + twoDigitString(seconds) + " " + getString(R.string.seconds));
                                    } else if (longTimed.value() >= 3600) {
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMIn.setText(twoDigitString(hours) + " " + ctx.getResources().getString(R.string.hours));// + " " + twoDigitString(minutes) + " " + "minutes" + " " + twoDigitString(seconds) + " " + "seconds");
                                    }
                                }
                            } catch (Exception e) {
                                e.printStackTrace();
                            }
                        }
                    });
        }
    }

    private String twoDigitString(int number) {
        if (number == 0) {
            return "00";
        }

        if (number / 10 == 0) {
            return "0" + number;
        }

        return String.valueOf(number);
    }

    private void goNextPage(CourselessonLayoutBinding dialogViewerItemLayoutBinding, Context ctx, String nextID, String answerID, String questionID, List<QuizMainObject.Questions> quizQuestionsObjectList, int position, String passingMarks) {
        new AsyncTask<Void, Void, String>() {
            @Override
            protected String doInBackground(Void... voids) {
                int viewpagerSize = getItemofviewpager(+1);
                int arraylistSIze = quizQuestionsObjectList.size();
                int answerStatus = quizDatabaseRepository.getTrueAnswer(answerID, questionID, "true");
                getActivity().runOnUiThread(new Runnable() {
                    public void run() {
                        int pagepos = fragmentCourseItemLayoutBinding.quizViewLayout.fragmentquesionViewpager.getCurrentItem();

                        if (myTextViews != null) {
                            if (answerStatus == 1) {
                                myTextViews[pagepos].setBackgroundColor(getResources().getColor(R.color.colorProgress));
                            } else {
                                myTextViews[pagepos].setBackgroundColor(getResources().getColor(R.color.colorRed));
                            }
                            if (viewpagerSize != arraylistSIze) {
                                myTextViews[pagepos + 1].setBackgroundColor(getResources().getColor(R.color.colorGreen));
                            }
                        }
                    }
                });

                if (viewpagerSize == arraylistSIze) {
                    Disposabletimer.dispose();
                    int aTrueCount = quizDatabaseRepository.getCountTrueAnswerSelect(quizID, "true");
                    getActivity().runOnUiThread(new Runnable() {
                        public void run() {
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.trueProgressTextLay.removeAllViews();
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.targetProgressTextLay.removeAllViews();
                            dialogViewerItemLayoutBinding.quizViewLayout.questionLayout.setVisibility(View.GONE);
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.summarylay.setVisibility(View.GONE);
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.mainResultView.setVisibility(View.VISIBLE);
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.resultlay.setVisibility(View.VISIBLE);

                            int aTrueProgress = (int) ((aTrueCount) * 100 / quizQuestionsObjectList.size());
                            int intpassingMarks = Integer.parseInt(passingMarks);
                            int TargetProgress = intpassingMarks - aTrueProgress;
                            int LeftProgress = 100 - intpassingMarks;

                            if (aTrueProgress > intpassingMarks) {
                                LeftProgress = LeftProgress + (TargetProgress);
                                TargetProgress = 0;
                            }
                            //Log.e(Const.LOG_NOON_TAG, "===passingMarks===" + passingMarks);
                            //Log.e(Const.LOG_NOON_TAG, "===aTrueProgress===" + aTrueProgress);
                            //Log.e(Const.LOG_NOON_TAG, "===TargetProgress===" + TargetProgress);
                            //Log.e(Const.LOG_NOON_TAG, "===LeftProgress===" + LeftProgress);

                            ViewTreeObserver vto = dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.trueProgressTextLay.getViewTreeObserver();
                            vto.addOnGlobalLayoutListener(new ViewTreeObserver.OnGlobalLayoutListener() {
                                @Override
                                public void onGlobalLayout() {
                                    if (Build.VERSION.SDK_INT < Build.VERSION_CODES.JELLY_BEAN) {
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.trueProgressTextLay.getViewTreeObserver().removeGlobalOnLayoutListener(this);
                                    } else {
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.trueProgressTextLay.getViewTreeObserver().removeOnGlobalLayoutListener(this);
                                    }
                                    int width = dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.trueProgressTextLay.getMeasuredWidth();
                                    int height = dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.trueProgressTextLay.getMeasuredHeight();

                                    int leftMarginFromInt = ((width * aTrueProgress) / 100);
                                    if (leftMarginFromInt != 0) {

                                        LinearLayout.LayoutParams paramsview = new LinearLayout.LayoutParams(5, 30);
                                        paramsview.setMargins(leftMarginFromInt - 2, 0, 0, 0);
                                        View view = new View(getActivity());
                                        view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.trueProgressTextLay.addView(view, paramsview);

                                        int margSpace = (aTrueProgress > 8) ? 65 : 30;

                                        LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                                        params.setMargins(leftMarginFromInt - margSpace, 0, 0, 0);
                                        TextView aTrueText = new TextView(getActivity());
                                        aTrueText.setText(aTrueProgress + "%" + "\n" + getString(R.string.yourscore));
                                        aTrueText.setTextColor(getResources().getColor(R.color.colorDarkGray));
                                        aTrueText.setTextSize(12);
                                        aTrueText.setGravity(Gravity.CENTER);
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.trueProgressTextLay.addView(aTrueText, params);
                                    }
                                }
                            });

                            ViewTreeObserver vto1 = dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.targetProgressTextLay.getViewTreeObserver();
                            vto1.addOnGlobalLayoutListener(new ViewTreeObserver.OnGlobalLayoutListener() {
                                @Override
                                public void onGlobalLayout() {
                                    if (Build.VERSION.SDK_INT < Build.VERSION_CODES.JELLY_BEAN) {
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.targetProgressTextLay.getViewTreeObserver().removeGlobalOnLayoutListener(this);
                                    } else {
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.targetProgressTextLay.getViewTreeObserver().removeOnGlobalLayoutListener(this);
                                    }
                                    int width = dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.targetProgressTextLay.getMeasuredWidth();
                                    int height = dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.targetProgressTextLay.getMeasuredHeight();

                                    int targetMarginFromInt = ((width * intpassingMarks) / 100);
                                    if (targetMarginFromInt != 0) {

                                        LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                                        params.setMargins(targetMarginFromInt - 65, 0, 0, 0);
                                        TextView aTrueText = new TextView(getActivity());
                                        aTrueText.setText(getString(R.string.passingscore) + "\n" + intpassingMarks + "%");
                                        aTrueText.setTextColor(getResources().getColor(R.color.colorDarkGray));
                                        aTrueText.setTextSize(12);
                                        aTrueText.setGravity(Gravity.CENTER);
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.targetProgressTextLay.addView(aTrueText, params);

                                        LinearLayout.LayoutParams paramsview = new LinearLayout.LayoutParams(5, 30);
                                        paramsview.setMargins(targetMarginFromInt - 4, 0, 0, 0);
                                        View view = new View(getActivity());

                                        if (aTrueProgress > intpassingMarks) {
                                            view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                                        } else {
                                            view.setBackgroundColor(getResources().getColor(R.color.colorRed));
                                        }
                                        dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.targetProgressTextLay.addView(view, paramsview);
                                    }
                                }
                            });

                            ArrayList<ProgressItem> progressItemList = new ArrayList<>();
                            ProgressItem mProgressItem;

                            mProgressItem = new ProgressItem();
                            mProgressItem.progressItemPercentage = aTrueProgress;
                            mProgressItem.color = R.color.colorProgress;
                            progressItemList.add(mProgressItem);

                            mProgressItem = new ProgressItem();
                            mProgressItem.progressItemPercentage = TargetProgress;
                            mProgressItem.color = R.color.colorRed;
                            progressItemList.add(mProgressItem);

                            mProgressItem = new ProgressItem();
                            mProgressItem.progressItemPercentage = LeftProgress;
                            mProgressItem.color = R.color.colorMoreDarkGray;
                            progressItemList.add(mProgressItem);

                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.customProgressbar.getThumb().mutate().setAlpha(0);
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.customProgressbar.initData(progressItemList);
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.customProgressbar.invalidate();

                            if (intpassingMarks <= aTrueProgress) {
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatustxt.setText(R.string.quiz_passed);
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatusButton.setTextColor(getResources().getColor(R.color.colorTextHeader));
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatusButton.setText(R.string.Finish);
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizSummaryButton.setTextColor(getResources().getColor(R.color.colorTextHeader));
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.explanationbackbutton.setTextColor(getResources().getColor(R.color.colorTextHeader));
                                String totalQuesions = String.valueOf(quizQuestionsObjectList.size());
                                String totalAnswers = String.valueOf(aTrueCount);
                                String date = getUTCTime();
                                QuizUserResult getquizUserResult = quizDatabaseRepository.getQuizuserResult(userId, quizID);
                                if (getquizUserResult != null) {
                                    String quizTime = dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMIn.getText().toString();
                                    String yourScore = String.valueOf(aTrueProgress);
                                    String passingScore = passingMarks;
                                    quizDatabaseRepository
                                            .updateQuizUserResult(userId, quizID, quizTime, yourScore, passingScore, totalQuesions, totalAnswers, date);
                                    JsonArray array = new JsonArray();
                                    JsonObject jsonObject = new JsonObject();
                                    if (getquizUserResult != null) {
                                        try {
                                            jsonObject.addProperty("quizid", Integer.parseInt(getquizUserResult.getQuizId()));
                                            jsonObject.addProperty("userid", Integer.parseInt(getquizUserResult.getUserId()));
                                            jsonObject.addProperty("totalquestion", Integer.parseInt(getquizUserResult.getTotalQuitions()));
                                            jsonObject.addProperty("answeredquestion", Integer.parseInt(getquizUserResult.getTotalAnswers()));
                                            jsonObject.addProperty("performdate", getquizUserResult.getQuizDate());
                                            jsonObject.addProperty("passingscore", Integer.parseInt(getquizUserResult.getPassingScore()));
                                            jsonObject.addProperty("score", Integer.parseInt(getquizUserResult.getYourScore()));

                                            array.add(jsonObject);
                                        } catch (JsonSyntaxException e) {
                                            e.printStackTrace();

                                        }

                                        CompositeDisposable disposable = new CompositeDisposable();
                                        disposable.add(quizRepository.getUserQuizResultSync(array).subscribeOn(Schedulers.io())
                                                .observeOn(AndroidSchedulers.mainThread())
                                                .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                                                    @Override
                                                    public void onSuccess(RestResponse restresponse) {
                                                        if (restresponse.getResponse_code().equals("0")) {
                                                            Log.e("onSuccess", "onSuccess: " + restresponse.toString());
                                                        }
                                                    }

                                                    @Override
                                                    public void onError(Throwable e) {
                                                        Log.e("onError", "onError: " + e.getMessage());
                                                        try {
                                                            if (!userId.equals("")) {
                                                                SyncAPITable syncAPITable = new SyncAPITable();

                                                                syncAPITable.setApi_name(getString(R.string.quiz_result_progressed));
                                                                syncAPITable.setEndpoint_url("UserQuizResult/UserQuizResultSync");
                                                                syncAPITable.setParameters(new Gson().toJson(array));
                                                                syncAPITable.setHeaders(PrefUtils.getAuthid(getActivity()));
                                                                syncAPITable.setStatus(getString(R.string.errored_status));
                                                                syncAPITable.setDescription(e.getMessage());
                                                                syncAPITable.setCreated_time(getUTCTime());
                                                                syncAPITable.setGradeName(gradeName);
                                                                syncAPITable.setCourseName(CourseName);
                                                                syncAPITable.setUserid(Integer.parseInt(userId));
                                                                syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                                                NoonApplication.cacheStatus = 2;
                                                                SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                                                SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                                                if (editor != null) {
                                                                    editor.clear();
                                                                    editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                                                    editor.apply();
                                                                }
                                                            }
                                                        } catch (JsonSyntaxException exeption) {
                                                            exeption.printStackTrace();
                                                        }
                                                    }
                                                }));
                                    }
                                } else {
                                    QuizUserResult quizUserResult = new QuizUserResult();
                                    quizUserResult.setQuizId(quizID);
                                    quizUserResult.setUserId(userId);
                                    quizUserResult.setStatus(false);
                                    quizUserResult.setQuizTime(dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMIn.getText().toString());
                                    quizUserResult.setPassingScore(passingMarks);
                                    quizUserResult.setYourScore(String.valueOf(aTrueProgress));
                                    quizUserResult.setQuizDate(date);
                                    quizUserResult.setTotalAnswers(totalAnswers);
                                    quizUserResult.setTotalQuitions(totalQuesions);
                                    quizDatabaseRepository.insertAllQuizUserResult(quizUserResult);

                                    JsonArray array = new JsonArray();
                                    JsonObject jsonObject = new JsonObject();
                                    try {
                                        jsonObject.addProperty("quizid", Integer.parseInt(quizUserResult.getQuizId()));
                                        jsonObject.addProperty("userid", Integer.parseInt(quizUserResult.getUserId()));
                                        jsonObject.addProperty("totalquestion", Integer.parseInt(quizUserResult.getTotalQuitions()));
                                        jsonObject.addProperty("answeredquestion", Integer.parseInt(quizUserResult.getTotalAnswers()));
                                        jsonObject.addProperty("performdate", quizUserResult.getQuizDate());
                                        jsonObject.addProperty("passingscore", Integer.parseInt(quizUserResult.getPassingScore()));
                                        jsonObject.addProperty("score", Integer.parseInt(quizUserResult.getYourScore()));

                                        array.add(jsonObject);
                                    } catch (JsonSyntaxException e) {
                                        e.printStackTrace();
                                    }

                                    disposable.add(quizRepository.getUserQuizResultSync(array).subscribeOn(Schedulers.io())
                                            .observeOn(AndroidSchedulers.mainThread())
                                            .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                                                @Override
                                                public void onSuccess(RestResponse restresponse) {
                                                    if (restresponse.getResponse_code().equals("0")) {
                                                        Log.e("onSuccess", "onSuccess: " + restresponse.toString());
                                                    }
                                                }

                                                @Override
                                                public void onError(Throwable e) {
                                                    Log.e("onError", "onError: " + e.getMessage());
                                                    try {
                                                        if (!userId.equals("")) {
                                                            SyncAPITable syncAPITable = new SyncAPITable();

                                                            syncAPITable.setApi_name(getString(R.string.quiz_result_progressed));
                                                            syncAPITable.setEndpoint_url("UserQuizResult/UserQuizResultSync");
                                                            syncAPITable.setParameters(String.valueOf(array));
                                                            syncAPITable.setHeaders(PrefUtils.getAuthid(getActivity()));
                                                            syncAPITable.setStatus(getString(R.string.errored_status));
                                                            syncAPITable.setDescription(e.getMessage());
                                                            syncAPITable.setCreated_time(getUTCTime());
                                                            syncAPITable.setGradeName(gradeName);
                                                            syncAPITable.setCourseName(CourseName);
                                                            syncAPITable.setUserid(Integer.parseInt(userId));
                                                            syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                                            NoonApplication.cacheStatus = 2;
                                                            SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                                            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                                            if (editor != null) {
                                                                editor.clear();
                                                                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                                                editor.apply();
                                                            }
                                                        }
                                                    } catch (JsonSyntaxException exeption) {
                                                        exeption.printStackTrace();
                                                    }
                                                }
                                            }));
                                }


                            } else {
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatustxt.setText(R.string.quiz_failed);
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatusButton.setTextColor(getResources().getColor(R.color.colorGreen));
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatusButton.setText(R.string.try_again);
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizSummaryButton.setTextColor(getResources().getColor(R.color.colorGreen));
                                dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.explanationbackbutton.setTextColor(getResources().getColor(R.color.colorGreen));
                                String totalQuesions = String.valueOf(quizQuestionsObjectList.size());
                                String totalAnswers = String.valueOf(aTrueCount);
                                String date = getUTCTime();
                                QuizUserResult getquizUserResult = quizDatabaseRepository.getQuizuserResult(userId, quizID);
                                if (getquizUserResult != null) {
                                    String quizTime = dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMIn.getText().toString();
                                    String yourScore = String.valueOf(aTrueProgress);
                                    String passingScore = passingMarks;
                                    quizDatabaseRepository.updateQuizUserResult(userId, quizID, quizTime, yourScore, passingScore, totalQuesions, totalAnswers, date);
                                    JsonArray array = new JsonArray();
                                    JsonObject jsonObject = new JsonObject();
                                    if (getquizUserResult != null && getquizUserResult.getTotalQuitions() != null && getquizUserResult.getTotalAnswers() != null) {
                                        try {
                                            jsonObject.addProperty("quizid", Integer.parseInt(getquizUserResult.getQuizId()));
                                            jsonObject.addProperty("userid", Integer.parseInt(getquizUserResult.getUserId()));
                                            jsonObject.addProperty("totalquestion", Integer.parseInt(getquizUserResult.getTotalQuitions()));
                                            jsonObject.addProperty("answeredquestion", Integer.parseInt(getquizUserResult.getTotalAnswers()));
                                            jsonObject.addProperty("performdate", getquizUserResult.getQuizDate());
                                            jsonObject.addProperty("passingscore", Float.parseFloat(getquizUserResult.getPassingScore()));
                                            jsonObject.addProperty("score", Float.parseFloat(getquizUserResult.getYourScore()));

                                            array.add(jsonObject);
                                        } catch (JsonSyntaxException e) {
                                            e.printStackTrace();

                                        }

                                        disposable.add(quizRepository.getUserQuizResultSync(array).subscribeOn(Schedulers.io())
                                                .observeOn(AndroidSchedulers.mainThread())
                                                .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                                                    @Override
                                                    public void onSuccess(RestResponse restresponse) {
                                                        if (restresponse.getResponse_code().equals("0")) {
                                                            Log.e("onSuccess", "onSuccess: " + restresponse.toString());
                                                        }
                                                    }

                                                    @Override
                                                    public void onError(Throwable e) {
                                                        Log.e("onError", "onError: " + e.getMessage());
                                                        try {
                                                            if (!userId.equals("")) {
                                                                SyncAPITable syncAPITable = new SyncAPITable();

                                                                syncAPITable.setApi_name(getString(R.string.quiz_result_progressed));
                                                                syncAPITable.setEndpoint_url("UserQuizResult/UserQuizResultSync");
                                                                syncAPITable.setParameters(String.valueOf(array));
                                                                syncAPITable.setHeaders(PrefUtils.getAuthid(getActivity()));
                                                                syncAPITable.setStatus(getString(R.string.errored_status));
                                                                syncAPITable.setDescription(e.getMessage());
                                                                syncAPITable.setCreated_time(getUTCTime());
                                                                syncAPITable.setGradeName(gradeName);
                                                                syncAPITable.setCourseName(CourseName);
                                                                syncAPITable.setUserid(Integer.parseInt(userId));
                                                                syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                                                NoonApplication.cacheStatus = 2;
                                                                SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                                                SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                                                if (editor != null) {
                                                                    editor.clear();
                                                                    editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                                                    editor.apply();
                                                                }
                                                            }
                                                        } catch (JsonSyntaxException exeption) {
                                                            exeption.printStackTrace();
                                                        }
                                                    }
                                                }));
                                    }
                                } else {
                                    QuizUserResult quizUserResult = new QuizUserResult();
                                    quizUserResult.setQuizId(quizID);
                                    quizUserResult.setUserId(userId);
                                    quizUserResult.setStatus(false);
                                    quizUserResult.setQuizTime(dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMIn.getText().toString());
                                    quizUserResult.setPassingScore(passingMarks);
                                    quizUserResult.setYourScore(String.valueOf(aTrueProgress));
                                    quizUserResult.setQuizDate(date);
                                    quizUserResult.setTotalAnswers(totalAnswers);
                                    quizUserResult.setTotalQuitions(totalQuesions);
                                    quizDatabaseRepository.insertAllQuizUserResult(quizUserResult);

                                    JsonArray array = new JsonArray();
                                    JsonObject jsonObject = new JsonObject();
                                    try {
                                        jsonObject.addProperty("quizid", Integer.parseInt(quizUserResult.getQuizId()));
                                        jsonObject.addProperty("userid", Integer.parseInt(quizUserResult.getUserId()));
                                        jsonObject.addProperty("totalquestion", Integer.parseInt(quizUserResult.getTotalQuitions()));
                                        jsonObject.addProperty("answeredquestion", Integer.parseInt(quizUserResult.getTotalAnswers()));
                                        jsonObject.addProperty("performdate", quizUserResult.getQuizDate());
                                        jsonObject.addProperty("passingscore", Float.parseFloat(quizUserResult.getPassingScore()));
                                        jsonObject.addProperty("score", Float.parseFloat(quizUserResult.getYourScore()));

                                        array.add(jsonObject);
                                    } catch (JsonSyntaxException e) {
                                        e.printStackTrace();
                                    }

                                    disposable.add(quizRepository.getUserQuizResultSync(array).subscribeOn(Schedulers.io())
                                            .observeOn(AndroidSchedulers.mainThread())
                                            .subscribeWith(new DisposableSingleObserver<RestResponse>() {
                                                @Override
                                                public void onSuccess(RestResponse restresponse) {
                                                    if (restresponse.getResponse_code().equals("0")) {
                                                        Log.e("onSuccess", "onSuccess: " + restresponse.toString());
                                                    }
                                                }

                                                @Override
                                                public void onError(Throwable e) {
                                                    Log.e("onError", "onError: " + e.getMessage());
                                                    try {
                                                        if (!userId.equals("")) {
                                                            SyncAPITable syncAPITable = new SyncAPITable();

                                                            syncAPITable.setApi_name(getString(R.string.quiz_result_progressed));
                                                            syncAPITable.setEndpoint_url("UserQuizResult/UserQuizResultSync");
                                                            syncAPITable.setParameters(String.valueOf(array));
                                                            syncAPITable.setHeaders(PrefUtils.getAuthid(getActivity()));
                                                            syncAPITable.setStatus(getString(R.string.errored_status));
                                                            syncAPITable.setDescription(e.getMessage());
                                                            syncAPITable.setCreated_time(getUTCTime());
                                                            syncAPITable.setGradeName(gradeName);
                                                            syncAPITable.setCourseName(CourseName);
                                                            syncAPITable.setUserid(Integer.parseInt(userId));
                                                            syncAPIDatabaseRepository.insertSyncData(syncAPITable);

                                                            NoonApplication.cacheStatus = 2;
                                                            SharedPreferences sharedPreferencesCache = getActivity().getSharedPreferences("cacheStatus", MODE_PRIVATE);
                                                            SharedPreferences.Editor editor = sharedPreferencesCache.edit();
                                                            if (editor != null) {
                                                                editor.clear();
                                                                editor.putString("FlagStatus", String.valueOf(NoonApplication.cacheStatus));
                                                                editor.apply();
                                                            }
                                                        }
                                                    } catch (JsonSyntaxException exeption) {
                                                        exeption.printStackTrace();
                                                    }
                                                }
                                            }));
                                }

                            }

                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMInFinal.setText(dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.compliteMIn.getText());
                            new ProgressTask(lessonID, String.valueOf(aTrueProgress), position, quizID, fileid, 0).execute();
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizStatusButton.setOnClickListener(new View.OnClickListener() {
                                @Override
                                public void onClick(View view) {
                                    if (intpassingMarks <= aTrueProgress) {
                                        try {
                                            int coursePosition = -1;
                                            for (int i = 0; i < courseItemListAdapter.getItems().size(); i++) {
                                                if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                                                    coursePosition = i + 1;
                                                    break;
                                                }
                                            }
                                            if (coursePosition != -1) {
                                                if (courseItemListAdapter.getItems().size() > coursePosition) {
                                                    if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildCount() > 0) {
                                                        CourseItemListAdapter.MyViewHolder test = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(coursePosition)));
                                                        if (test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter() != null) {
                                                            CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(0));
                                                            fragmentCourseItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
                                                            //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                                            fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                                                            fragmentCourseItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
                                                            fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
                                                            ((CourseItemInnerListAdapter) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter()).nextItem(0, holder);

                                                        }
                                                    }
                                                } else {
                                                    fragmentCourseItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
                                                    //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                                    fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                                                    fragmentCourseItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
                                                    fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
                                                }
                                            }
                                        } catch (Exception e) {
                                            e.printStackTrace();
                                        }
                                    } else {
                                        try {
                                            int coursePosition = -1;
                                            for (int i = 0; i < courseItemListAdapter.getItems().size(); i++) {
                                                if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                                                    coursePosition = i + 1;
                                                    break;
                                                }
                                            }
                                            if (coursePosition != -1) {
                                                if (courseItemListAdapter.getItems().size() > coursePosition) {
                                                    if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildCount() > 0) {
                                                        CourseItemListAdapter.MyViewHolder test = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(coursePosition)));
                                                        if (test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter() != null) {
                                                            CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(0));
                                                            fragmentCourseItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
                                                            //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                                            fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                                                            fragmentCourseItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
                                                            fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
                                                            ((CourseItemInnerListAdapter) test.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getAdapter()).nextItem(0, holder);

                                                        }
                                                    }
                                                } else {
                                                    fragmentCourseItemLayoutBinding.chapterViewLayout.chapterMainView.setVisibility(View.GONE);
                                                    //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                                                    fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setVisibility(View.GONE);
                                                    fragmentCourseItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
                                                    fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
                                                }
                                            }
                                        } catch (Exception e) {
                                            e.printStackTrace();
                                        }
                                        CallApiQuestionList(fragmentCourseItemLayoutBinding, getActivity(), quizID, position);
                                    }
                                }
                            });
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.QuizSummaryButton.setOnClickListener(new View.OnClickListener() {
                                @Override
                                public void onClick(View view) {

                                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.resultlay.setVisibility(View.GONE);
                                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.summarylay.setVisibility(View.VISIBLE);
                                    dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.mainResultView.fullScroll(View.FOCUS_UP);

                                }
                            });
                        }
                    });
                } else {
                    getActivity().runOnUiThread(new Runnable() {
                        public void run() {
                            dialogViewerItemLayoutBinding.quizViewLayout.questionLayout.setVisibility(View.VISIBLE);
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.mainResultView.setVisibility(View.GONE);
                            dialogViewerItemLayoutBinding.quizViewLayout.resultLayout.resultlay.setVisibility(View.GONE);
                            dialogViewerItemLayoutBinding.quizViewLayout.fragmentquesionViewpager.setCurrentItem(getItemofviewpager(+1), true);
                        }
                    });
                }
                return null;
            }
        }.execute();
    }

    private String getUTCTime() {
        SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH);
        sdf.setTimeZone(TimeZone.getTimeZone("UTC"));
        String gmtTime = sdf.format(new Date());
//        Log.e("date", "getUTCTime: " + gmtTime);
        return gmtTime;
    }

    private int getItemofviewpager(int i) {
        return fragmentCourseItemLayoutBinding.quizViewLayout.fragmentquesionViewpager.getCurrentItem() + i;
    }

    public void setOrientationDynamically(int orientation) {
        if (orientation == Configuration.ORIENTATION_LANDSCAPE) {
            try {
                if (fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.getVisibility() == View.VISIBLE) {
                    try {
                        fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().setDisplayModel(GiraffePlayer.DISPLAY_FULL_WINDOW);
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                } else {
                    if (fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.getVisibility() == View.VISIBLE) {
                        setupvideoinlandscapmode();
                    } else {
                        fragmentCourseItemLayoutBinding.mainFragmentCourseLayout.setOrientation(LinearLayout.HORIZONTAL);

                        LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.MATCH_PARENT, 0f);
//                    params.setMargins(15, 15, 0, 15);
                        fragmentCourseItemLayoutBinding.courseLessonCardview.setLayoutParams(params);

                        LinearLayout.LayoutParams params1 = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.MATCH_PARENT, 1f);
                        params1.setMargins(0, 0, 0, 0);
                        fragmentCourseItemLayoutBinding.nestedScrollView.setLayoutParams(params1);
                    }

                    //FrameLayout.LayoutParams params3 = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MATCH_PARENT, FrameLayout.LayoutParams.MATCH_PARENT);
                    //fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setLayoutParams(params3);
//                    fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setLayoutParams(params3);
                    //fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setLayoutParams(params3);
                }

            } catch (Exception e) {
                e.printStackTrace();
            }

        } else if (orientation == Configuration.ORIENTATION_PORTRAIT) {
            try {
                fragmentCourseItemLayoutBinding.mainFragmentCourseLayout.setOrientation(LinearLayout.VERTICAL);
                fragmentCourseItemLayoutBinding.videoViewer.videoView.getPlayer().setDisplayModel(GiraffePlayer.DISPLAY_NORMAL);

                LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                params.setMargins(0, 0, 0, 0);
                fragmentCourseItemLayoutBinding.courseLessonCardview.setLayoutParams(params);

                LinearLayout.LayoutParams params1 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.MATCH_PARENT);
                params1.setMargins(0, 0, 0, 0);
                fragmentCourseItemLayoutBinding.nestedScrollView.setLayoutParams(params1);

                FrameLayout.LayoutParams params3 = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MATCH_PARENT, (int) getResources().getDimension(R.dimen._175sdp));
                fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setLayoutParams(params3);

                FrameLayout.LayoutParams params4 = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MATCH_PARENT, (int) getResources().getDimension(R.dimen._250sdp));
                //fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setLayoutParams(params4);

                FrameLayout.LayoutParams params5 = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MATCH_PARENT, (int) getResources().getDimension(R.dimen._300sdp));
                fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setLayoutParams(params5);

            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public void setupvideoinlandscapmode() {
        int orientation = getActivity().getResources().getConfiguration().orientation;
        if (orientation == Configuration.ORIENTATION_LANDSCAPE) {
            fragmentCourseItemLayoutBinding.mainFragmentCourseLayout.setOrientation(LinearLayout.HORIZONTAL);

            LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.MATCH_PARENT, .5f);
//                    params.setMargins(15, 15, 0, 15);
            fragmentCourseItemLayoutBinding.courseLessonCardview.setLayoutParams(params);

            LinearLayout.LayoutParams params1 = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.MATCH_PARENT, 0.5f);
            params1.setMargins(0, 0, 0, 0);
            fragmentCourseItemLayoutBinding.nestedScrollView.setLayoutParams(params1);

            FrameLayout.LayoutParams params3 = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MATCH_PARENT, FrameLayout.LayoutParams.WRAP_CONTENT);
//            fragmentCourseItemLayoutBinding.videoViewer.videoViewLay.setLayoutParams(params3);
//                    fragmentCourseItemLayoutBinding.pdfViewLayout.pdfviewLay.setLayoutParams(params3);
            fragmentCourseItemLayoutBinding.quizViewLayout.quizViewer.setLayoutParams(params3);
        }
    }

    public void showHitLimitDialog(Context context) {
        HitLimitDialogBinding hitLimitDialogBinding = DataBindingUtil.inflate(LayoutInflater.from(context), R.layout.hit_limit_dialog, null, false);
        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        builder.setView(hitLimitDialogBinding.getRoot());

        final AlertDialog alertDialog = builder.create();
        hitLimitDialogBinding.txtNoThanksClick.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                alertDialog.dismiss();
            }
        });

        hitLimitDialogBinding.txtPendingClick.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                alertDialog.dismiss();
                startActivity(new Intent(context, CacheEventsListActivity.class));
            }
        });
        alertDialog.show();
    }

    public class ProgressTask extends AsyncTask<Void, Void, String> {
        private String lessonID;
        private String progressval;
        private int position = 0;
        private String quizID;
        private String fileid;
        private int playpushflag;

        ProgressTask(String lessonID, String progressval, int position, String quizID, String fileid, int playpushflag) {
            this.lessonID = lessonID;
            this.progressval = progressval;
            this.position = position;
            this.quizID = quizID;
            this.fileid = fileid;
            this.playpushflag = playpushflag;
        }

        @SuppressLint("WrongThread")
        @Override
        protected String doInBackground(Void... params) {
            // TODO: attempt authentication against a network service.

            try {

                if (!TextUtils.isEmpty(quizID)) {
                    LessonProgress lessonProgressPrv = lessonDatabaseRepository.getItemQuizIdProgress(quizID, userId);
                    if (lessonProgressPrv != null) {
                        lessonDatabaseRepository.updateItemQuizIdProgress(quizID, progressval, false, userId);
                    } else {
                        LessonProgress lessonProgress = new LessonProgress();
                        lessonProgress.setUserId(userId);
                        lessonProgress.setLessonProgress(progressval);
                        lessonProgress.setLessonId("0");
                        lessonProgress.setQuizId(quizID);
                        lessonProgress.setStatus(false);
                        lessonProgress.setGradeId(GradeId);
                        lessonProgress.setFileId(fileid);
                        lessonProgress.setTotalRecords(String.valueOf(lessonsArrayList.size()));
                        lessonDatabaseRepository.insertLessonProgressData(lessonProgress);
                    }
                } else {
                    LessonProgress lessonProgressPrv = lessonDatabaseRepository.getItemLessonProgressData(lessonID, fileid, userId);
                    if (lessonProgressPrv != null) {
                        lessonDatabaseRepository.updateItemLessonProgressData(lessonID, progressval, false, fileid, userId);
                    } else {
                        LessonProgress lessonProgress = new LessonProgress();
                        lessonProgress.setUserId(userId);
                        lessonProgress.setLessonId(lessonID);
                        lessonProgress.setQuizId("0");
                        lessonProgress.setLessonProgress(progressval);
                        lessonProgress.setStatus(false);
                        lessonProgress.setGradeId(GradeId);
                        lessonProgress.setFileId(fileid);
                        lessonProgress.setTotalRecords(String.valueOf(lessonsArrayList.size()));
                        lessonDatabaseRepository.insertLessonProgressData(lessonProgress);
                    }
                }

                for (int i = 0; i < coursePriviewArrayList.size(); i++) {
                    ArrayList<CoursePriviewObject.Lessons> lessonsArrayList = new ArrayList<>();

                    if (fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(i) != null) {
                        CourseItemListAdapter.MyViewHolder mainHolder = ((CourseItemListAdapter.MyViewHolder) fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(i)));

                        if (coursePriviewArrayList.get(i).getLessons() != null) {
                            Collections.addAll(lessonsArrayList, coursePriviewArrayList.get(i).getLessons());
                            if (lessonsArrayList.size() != 0) {
                                for (int j = 0; j < lessonsArrayList.size(); j++) {
                                    if (mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(j) != null) {
                                        CourseItemInnerListAdapter.MyViewHolder holder = (CourseItemInnerListAdapter.MyViewHolder) mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildViewHolder(mainHolder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.getChildAt(j));
                                        if (courseItemListAdapter.getItems().get(i).getId().equals(chapterid)) {
                                            if (j == position) {
                                                holder.courseInnerItemLayoutBinding.progressBarLayout.progressBar.setProgress(Integer.parseInt(progressval));
                                                int finalJ = j;
                                                if (playpushflag != 0)
                                                    Objects.requireNonNull(getActivity()).runOnUiThread(new Runnable() {
                                                        @Override
                                                        public void run() {
                                                            holder.courseInnerItemLayoutBinding.tvProgress.setText(progressval + " " + holder.itemView.getContext().getString(R.string.completed1));
                                                            if (lessonsArrayList.get(finalJ).getLessonfiles() != null) {
                                                                String getlessionID = lessonsArrayList.get(finalJ).getId();

                                                                String newfileid = "0";
                                                                if (lessonsArrayList.get(finalJ).getLessonfiles().length != 0) {
                                                                    newfileid = lessonsArrayList.get(finalJ).getLessonfiles()[0].getFiles().getId();
                                                                } else {
//                                                                     newfileid = lessonsArrayList.get(finalJ).getLessonfiles()[lessonsArrayList.get(finalJ).getLessonfiles().length - 1].getFiles().getId();
                                                                }

                                                                if (getlessionID.equals(lessonID)) {
                                                                    if (newfileid.equals(fileid)) {
                                                                        CoursePriviewObject.Lessons model = lessonsArrayList.get(position);
                                                                        model.setProgressVal(Integer.parseInt(progressval));

                                                                    }
                                                                }
                                                            } else {
                                                                String getquizID = lessonsArrayList.get(finalJ).getId();
                                                                if (getquizID.equals(quizID)) {
                                                                    CoursePriviewObject.Lessons model = lessonsArrayList.get(position);
                                                                    model.setProgressVal(Integer.parseInt(progressval));
                                                                }
                                                            }
                                                        }
                                                    });

                                            }
                                        }

//                                        if (lessonsArrayList.get(j).getLessonfiles() != null) {
//                                            String getlessionID = lessonsArrayList.get(j).getId();
//
//                                            String newfileid = "0";
//                                            if (lessonsArrayList.get(j).getLessonfiles().length == 1) {
//                                                newfileid = lessonsArrayList.get(j).getLessonfiles()[0].getFiles().getId();
//                                            } else {
//                                                newfileid = lessonsArrayList.get(j).getLessonfiles()[lessonsArrayList.get(j).getLessonfiles().length - 1].getFiles().getId();
//                                            }
//
//                                            if (getlessionID.equals(lessonID)) {
//                                                if (newfileid.equals(fileid)) {
//                                                    CoursePriviewObject.Lessons model = lessonsArrayList.get(position);
//                                                    model.setProgressVal(Integer.parseInt(progressval));
//                                                }
//                                            }
//                                        } else {
//                                            String getquizID = lessonsArrayList.get(j).getId();
//                                            if (getquizID.equals(quizID)) {
//                                                CoursePriviewObject.Lessons model = lessonsArrayList.get(position);
//                                                model.setProgressVal(Integer.parseInt(progressval));
//                                            }
//                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            } catch (Exception e) {
                e.printStackTrace();
            }
            return null;
        }

        @Override
        protected void onPostExecute(final String success) {
        }
    }

    public class setLocalDataTask extends AsyncTask<Void, Void, CoursePriviewObject> {
        CoursePriviewObject coursePriviewObject;
        CourseItemAsyncResponse delegate = null;
        boolean network = false;

        public setLocalDataTask(CourseItemAsyncResponse delegate, CoursePriviewObject coursePriviewObject, boolean b) {
            this.coursePriviewObject = coursePriviewObject;
            this.delegate = delegate;
            this.network = b;
        }

        @Override
        protected void onPreExecute() {

            super.onPreExecute();
        }

        @Override
        protected CoursePriviewObject doInBackground(Void... params) {

            if (!network) {
                coursePriviewObject = courseDatabaseRepository.getAllCourseDetailsById(GradeId, userId);
                if (coursePriviewObject != null) {

                    coursePriviewArrayList.clear();

                    for (int k = 0; k < coursePriviewObject.getData().getChapters().size(); k++) {
                        if (coursePriviewObject.getData().getChapters().get(k).getLessons() != null) {
                            coursePriviewArrayList.add(coursePriviewObject.getData().getChapters().get(k));
                            Collections.addAll(lessonsArrayList, coursePriviewObject.getData().getChapters().get(k).getLessons());
                            for (int j = 0; j < lessonsArrayList.size(); j++) {
                                if (lessonsArrayList.get(j).getLessonfiles() != null && lessonsArrayList.get(j).getLessonfiles().length != 0) {
                                    String lessonId = lessonsArrayList.get(j).getId();
                                    String fileid = "0";
                                    if (lessonsArrayList.get(j).getLessonfiles().length == 1) {
                                        fileid = lessonsArrayList.get(j).getLessonfiles()[0].getFiles().getId();
                                    } else {
                                        fileid = lessonsArrayList.get(j).getLessonfiles()[lessonsArrayList.get(j).getLessonfiles().length - 1].getFiles().getId();
                                        for (int i = 0; i < lessonsArrayList.get(j).getLessonfiles().length; i++) {

                                            if (i == (lessonsArrayList.get(j).getLessonfiles().length - 1)) {

                                            } else {
                                                String userIdSlash = userId + "/";
                                                String lessonFileID = lessonsArrayList.get(j).getLessonfiles()[i].getId();
                                                String newfileid = lessonsArrayList.get(j).getLessonfiles()[i].getFiles().getId();
                                                String newfilename = lessonsArrayList.get(j).getLessonfiles()[i].getFiles().getFilename();
                                                String newfileTypename = lessonsArrayList.get(j).getLessonfiles()[i].getFiles().getFiletypename();

                                                String str = lessonFileID + "_" + newfileid + "_" + newfilename.replaceFirst(".*-(\\w+).*", "$1") + "_" + newfileTypename + Const.extension;
                                                File file = new File(Const.destPath + userIdSlash, str);

                                                /*if (file.exists()) {
                                                    file.delete();
                                                }*/
                                            }
                                        }
                                    }

                                    LessonProgress lessonProgress = lessonDatabaseRepository.getItemLessonProgressData(lessonId, fileid, userId);
                                    if (lessonProgress != null) {
                                        lessonsArrayList.get(j).setProgressVal(Integer.parseInt(lessonProgress.getLessonProgress()));
                                    }
                                } else {

                                    String quizID = lessonsArrayList.get(j).getId();
                                    LessonProgress lessonProgress = lessonDatabaseRepository.getItemQuizIdProgress(quizID, userId);
                                    if (lessonProgress != null) {
                                        lessonsArrayList.get(j).setProgressVal(Integer.parseInt(lessonProgress.getLessonProgress()));
                                    }
                                }
                            }
                        }
                    }

                }
            } else {

                //Log.e(Const.LOG_NOON_TAG, "======ONLINE USER==" + coursePriviewObject);

                if (coursePriviewObject != null) {

                    coursePriviewArrayList.clear();

                    for (int k = 0; k < coursePriviewObject.getData().getChapters().size(); k++) {
                        if (coursePriviewObject.getData().getChapters().get(k).getLessons() != null && coursePriviewObject.getData().getChapters().get(k).getLessons().length != 0) {
                            coursePriviewArrayList.add(coursePriviewObject.getData().getChapters().get(k));
                            Collections.addAll(lessonsArrayList, coursePriviewObject.getData().getChapters().get(k).getLessons());
                            for (int j = 0; j < lessonsArrayList.size(); j++) {
                                if (lessonsArrayList.get(j).getLessonfiles() != null && lessonsArrayList.get(j).getLessonfiles().length != 0) {
                                    String lessonId = lessonsArrayList.get(j).getId();
                                    String fileid = "0";
                                    if (lessonsArrayList.get(j).getLessonfiles().length == 1) {
                                        fileid = lessonsArrayList.get(j).getLessonfiles()[0].getFiles().getId();
                                    } else {
                                        fileid = lessonsArrayList.get(j).getLessonfiles()[lessonsArrayList.get(j).getLessonfiles().length - 1].getFiles().getId();

                                        for (int i = 0; i < lessonsArrayList.get(j).getLessonfiles().length; i++) {

                                            if (i == (lessonsArrayList.get(j).getLessonfiles().length - 1)) {


                                            } else {
                                                String userIdSlash = userId + "/";
                                                String lessonFileID = lessonsArrayList.get(j).getLessonfiles()[i].getId();
                                                String newfileid = lessonsArrayList.get(j).getLessonfiles()[i].getFiles().getId();
                                                String newfilename = lessonsArrayList.get(j).getLessonfiles()[i].getFiles().getFilename();
                                                String newfileTypename = lessonsArrayList.get(j).getLessonfiles()[i].getFiles().getFiletypename();

                                                String str = lessonFileID + "_" + newfileid + "_" + newfilename.replaceFirst(".*-(\\w+).*", "$1") + "_" + newfileTypename + Const.extension;
                                                File file = new File(Const.destPath + userIdSlash, str);

//                                                if (file.exists()) {
//                                                    file.delete();
//                                                }
                                            }
                                        }
                                    }

                                    LessonProgress lessonProgress = lessonDatabaseRepository.getItemLessonProgressData(lessonId, fileid, userId);
                                    if (lessonProgress != null) {
                                        lessonsArrayList.get(j).setProgressVal(Integer.parseInt(lessonProgress.getLessonProgress()));
                                    }
                                } else {

                                    String quizID = lessonsArrayList.get(j).getId();
                                    LessonProgress lessonProgress = lessonDatabaseRepository.getItemQuizIdProgress(quizID, userId);
                                    if (lessonProgress != null) {
                                        lessonsArrayList.get(j).setProgressVal(Integer.parseInt(lessonProgress.getLessonProgress()));
                                    }

                                }
                            }
                        }
                    }
                }
            }

            return coursePriviewObject;
        }

        @Override
        protected void onPostExecute(CoursePriviewObject coursePriviewObject) {
            delegate.getCoursePriviewObject(coursePriviewObject);
        }
    }

    public static boolean deleteDir(File dir) {
        if (dir != null && dir.isDirectory()) {
            String[] children = dir.list();
            for (int i = 0; i < children.length; i++) {
                boolean success = deleteDir(new File(dir, children[i]));
                if (!success) {
                    return false;
                }
            }
            return dir.delete();
        } else if (dir != null && dir.isFile()) {
            return dir.delete();
        } else {
            return false;
        }
    }
}
