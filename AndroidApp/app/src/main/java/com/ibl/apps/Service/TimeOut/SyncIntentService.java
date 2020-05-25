package com.ibl.apps.Service.TimeOut;

import android.app.ActivityManager;
import android.content.Context;
import android.content.Intent;
import android.text.TextUtils;
import android.util.Log;

import androidx.annotation.NonNull;
import androidx.core.app.JobIntentService;
import androidx.legacy.content.WakefulBroadcastReceiver;

import com.auth0.android.Auth0;
import com.auth0.android.authentication.AuthenticationAPIClient;
import com.auth0.android.authentication.AuthenticationException;
import com.auth0.android.callback.BaseCallback;
import com.auth0.android.result.Credentials;
import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.ibl.apps.LessonManagement.LessonRepository;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.ReminderObject;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UserObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.LessonDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.UserDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.QuizUserResult;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.UserCredentialsManagement.UserRepository;
import com.ibl.apps.UserProfileManagement.UserProfileRepository;
import com.ibl.apps.noon.LoginActivity;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.JWTUtils;
import com.ibl.apps.util.NotificationUtils;
import com.ibl.apps.util.PrefUtils;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Objects;
import java.util.TimeZone;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.HttpException;

public class SyncIntentService extends JobIntentService implements DroidListener {

    private static final String ACTION_START = "ACTION_START";
    private static Context mycontext;
    public static ApiService apiService;
    private CompositeDisposable disposable = new CompositeDisposable();
    static final int JOB_ID = 1000;

    Calendar rightNow = Calendar.getInstance();
    int currentHourIn24Format = rightNow.get(Calendar.HOUR_OF_DAY);
    int diffHour = 0;
    int Hour = 0;
    boolean Istimeouton = false;
    int intervals = 0;
    int reminderCount = 0;
    int reminder = 0;
    String userId = "0";
    String userRoleName = "";
    UserDetails userDetailsObject = new UserDetails();
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;
    private UserProfileRepository userProfileRepository;
    private QuizDatabaseRepository quizDatabaseRepository;

    public static void start(Context context) {
        mycontext = context;
        apiService = ApiClient.getClient(context).create(ApiService.class);


        Intent starter = new Intent(context, SyncIntentService.class);
        SyncIntentService.enqueueWork(context, starter);
    }

    public static void enqueueWork(Context context, Intent intent) {
        intent.setAction(ACTION_START);
        enqueueWork(context, SyncIntentService.class, JOB_ID, intent);
    }

    @Override
    protected void onHandleWork(@androidx.annotation.NonNull Intent intent) {
        try {
            String action = intent.getAction();
            if (ACTION_START.equals(action)) {

                Log.e(Const.LOG_NOON_TAG, "====SERVICE CALL====");


                mDroidNet = DroidNet.getInstance();
                mDroidNet.addInternetConnectivityListener(this);
                if (isNetworkAvailable(mycontext)) {
                    refreshToken();
                }

                boolean isActivityFound = false;

                if (mycontext != null) {

                    try {
                        ActivityManager activityManager = (ActivityManager) mycontext.getSystemService(Context.ACTIVITY_SERVICE);
                        assert activityManager != null;
                        List<ActivityManager.RunningTaskInfo> services = activityManager.getRunningTasks(Integer.MAX_VALUE);

                        if (services != null) {
                            if (services.size() != 0) {
                                if (services.get(0).topActivity.getPackageName().equalsIgnoreCase(mycontext.getPackageName())) {
                                    isActivityFound = true;
                                }
                            }
                        }
                    } catch (Exception ignored) {

                    }
                }

                PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(userDetails -> {

                    if (userDetails != null) {
                        userDetailsObject = userDetails;
                        userId = userDetails.getId();

                        if (userDetails.getIstimeouton() != null) {
                            Istimeouton = Boolean.parseBoolean(userDetails.getIstimeouton());
                        }
                        if (userDetails.getIntervals() != null) {
                            intervals = Integer.parseInt(userDetails.getIntervals());
                        }
                        if (userDetails.getReminder() != null) {
                            reminder = Integer.parseInt(userDetails.getReminder());
                        }

                        if (userDetails.getRoleName() != null) {
                            userRoleName = userDetails.getRoleName().get(0);
                        }

                        if (isNetworkAvailable(mycontext)) {
                            /*---------------------------FOR User Accound FillesData UPDATE-------------------*/
                            // callApiUpdateProfile();

                            /*---------------------------FOR Quiz Timer--------------------------------*/
                            quizDatabaseRepository = new QuizDatabaseRepository();
                            List<QuizUserResult> quizUserResults = quizDatabaseRepository.getAllQuizuserResult(false, userId);

                            /*---------------------------FOR Lesson Progress--------------------------------*/
                            LessonDatabaseRepository lessonDatabaseRepository = new LessonDatabaseRepository();
                            List<LessonProgress> lessonProgressList = lessonDatabaseRepository.getAllLessonProgressData(false, userId);

                            //callApiProgessSyncAdd(lessonProgressList, quizUserResults);
                        }

                        /*--------------------------- For courses need to be deleted from local storage after expiry date in offline Database ----------------------------------*/
                        String userIdSlash = userId + "/";

                        //Log.e("ROLEEEE", "==userRoleName==" + userRoleName);


                        if (userRoleName.equals("Student")) {

                            //Log.e("ROLEEEE", "==equale==");
                            CourseDatabaseRepository courseDatabaseRepository = new CourseDatabaseRepository();
                            CourseObject courseObject = courseDatabaseRepository.getAllCourseObject(userId);

                            if (courseObject != null && courseObject.getData() != null) {
                                for (int i = 0; i < courseObject.getData().size(); i++) {
                                    for (int j = 0; j < courseObject.getData().get(i).getCourses().size(); j++) {

                                        try {

                                            if (!TextUtils.isEmpty(courseObject.getData().get(i).getCourses().get(j).getStartdate()) && !TextUtils.isEmpty(courseObject.getData().get(i).getCourses().get(j).getEnddate())) {
                                                String[] startDateArray = courseObject.getData().get(i).getCourses().get(j).getStartdate().split(" ");
                                                String[] endDateArray = courseObject.getData().get(i).getCourses().get(j).getEnddate().split(" ");

                                                String courseStartDate = startDateArray[0];
                                                String courseEndDate = endDateArray[0];

                                                Date startDate = null;
                                                Date endDate = null;
                                                Date currantDate = null;
                                                String GradeId = courseObject.getData().get(i).getCourses().get(j).getId();

                                                SimpleDateFormat dateformat = new SimpleDateFormat("MM/dd/yyyy");
                                                currantDate = new Date();
                                                startDate = dateformat.parse(courseStartDate);
                                                endDate = dateformat.parse(courseEndDate);

                                                //Log.e(Const.LOG_NOON_TAG, "CONST====GRADEID===" + GradeId);
                                                //Log.e(Const.LOG_NOON_TAG, "CONST====CURRANTDATE==" + currantDate);
                                                //Log.e(Const.LOG_NOON_TAG, "CONST====ENDDATE==" + endDate);
                                                //Log.e(Const.LOG_NOON_TAG, "CONST====courseEndDate===" + courseObject.getData().get(i).getCourses().get(j).getEnddate());

                                                if (currantDate.compareTo(endDate) > 1) {
                                                    //Log.e(Const.LOG_NOON_TAG, "CONST====EXPIRE==");

                                                    if (courseObject.getUserId().equals(userId)) {
                                                        courseObject.getData().get(i).getCourses().get(j).setDeleted(true);

                                                        CoursePriviewObject coursePriviewObject = courseDatabaseRepository.getAllCourseDetailsById(GradeId, userId);
                                                        if (coursePriviewObject != null) {
                                                            for (int k = 0; k < coursePriviewObject.getData().getChapters().size(); k++) {
                                                                if (coursePriviewObject.getData().getChapters().get(k).getLessons() != null) {
                                                                    for (int l = 0; l < coursePriviewObject.getData().getChapters().get(k).getLessons().length; l++) {
                                                                        String fileid = "0";
                                                                        String filename = "";
                                                                        String fileTypename = "";
                                                                        String lessonfileId = "0";

                                                                        if (coursePriviewObject.getData().getChapters().get(k).getLessons() != null && coursePriviewObject.getData().getChapters().get(k).getLessons().length != 0) {
                                                                            if (coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles() != null && coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles().length != 0) {
                                                                                if (coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles().length == 1) {
                                                                                    lessonfileId = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[0].getId();
                                                                                    fileid = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[0].getFiles().getId();
                                                                                    filename = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[0].getFiles().getFilename();
                                                                                    fileTypename = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[0].getFiles().getFiletypename();
                                                                                } else {
                                                                                    lessonfileId = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles().length - 1].getId();
                                                                                    fileid = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles().length - 1].getFiles().getId();
                                                                                    filename = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles().length - 1].getFiles().getFilename();
                                                                                    fileTypename = coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles()[coursePriviewObject.getData().getChapters().get(k).getLessons()[l].getLessonfiles().length - 1].getFiles().getFiletypename();
                                                                                }

                                                                                String str = lessonfileId + "_" + fileid + "_" + filename.replaceFirst(".*-(\\w+).*", "$1") + "_" + fileTypename + Const.extension;
                                                                                File file = new File(Const.destPath + userIdSlash, str);

                                                                                if (file.exists()) {
                                                                                    file.delete();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        courseDatabaseRepository.updateAll(courseObject.getData(), userId);
                                                    }
                                                } else {
                                                    //Log.e(Const.LOG_NOON_TAG, "CONST====NOT EXPIRE==");
                                                }
                                            }

                                        } catch (ParseException e) {
                                            e.printStackTrace();
                                        }
                                    }
                                }

                            }
                        }
                    }
                    return null;
                }).execute();

                if (isActivityFound) {

                    //Log.e(Const.LOG_NOON_TAG, "====isActivityFound===");

                    if (Istimeouton) {

                        //Log.e(Const.LOG_NOON_TAG, "====Istimeouton===");

                        if (!isNetworkAvailable(mycontext)) {

                            //Log.e(Const.LOG_NOON_TAG, "====Not isNetworkAvailable===");

                            try {
                                ReminderObject reminderObject = PrefUtils.getReminderTokenKey(mycontext);

                                //Log.e(Const.LOG_NOON_TAG, "===reminderObject====" + reminderObject);

                                if (reminderObject != null) {

                                    // Log.e(Const.LOG_NOON_TAG, "===getCurrantHour====" + reminderObject.getCurrantHour());

                                    Hour = reminderObject.getCurrantHour();
                                    reminderCount = reminderObject.getReminderCount();
                                    diffHour = currentHourIn24Format - Hour;

                                    //Log.e(Const.LOG_NOON_TAG, "===currantHour====" + currentHourIn24Format);
                                    //Log.e(Const.LOG_NOON_TAG, "===Hour====" + Hour);
                                    //Log.e(Const.LOG_NOON_TAG, "===reminderCount====" + reminderCount);
                                    //Log.e(Const.LOG_NOON_TAG, "===diffHour====" + diffHour);

                                    if (reminderCount > reminder) {

                                        //Log.e(Const.LOG_NOON_TAG, "===reminderCount====" + reminderCount);
                                        locallyLogout();

                                    } else {
                                        if (diffHour != 0) {
                                            if (diffHour == intervals) {
                                                NotificationUtils.showReminderNotification(getApplicationContext());
                                                reminderObject.setCurrantHour(currentHourIn24Format);
                                                reminderObject.setReminderCount(reminderCount + 1);
                                                PrefUtils.storeReminderTokenKey(getApplicationContext(), reminderObject);
                                            }
                                        }
                                    }

                                } else {

                                    //Log.e(Const.LOG_NOON_TAG, "===currantHour==ELSEE==" + currentHourIn24Format);

                                    reminderObject = new ReminderObject();
                                    reminderObject.setCurrantHour(currentHourIn24Format);
                                    PrefUtils.storeReminderTokenKey(getApplicationContext(), reminderObject);
                                }

                            } catch (Exception e) {
                                e.printStackTrace();
                            }
                        } else {

                            //Log.e(Const.LOG_NOON_TAG, "===FRESHHHHH NEW====");

                            PrefUtils.storeReminderTokenKey(getApplicationContext(), new ReminderObject());
                        }
                    }
                }


            }
        } catch (Exception e) {
            e.printStackTrace();
            WakefulBroadcastReceiver.completeWakefulIntent(intent);
        } finally {
            WakefulBroadcastReceiver.completeWakefulIntent(intent);
        }
    }

    public void refreshToken() {
        UserDatabaseRepository userDatabaseRepository = new UserDatabaseRepository();
        String authid = PrefUtils.getAuthid(this);
        if (!TextUtils.isEmpty(authid)) {
            AuthTokenObject authTokenObject = userDatabaseRepository.getAuthTokenData(authid);
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
                            SimpleDateFormat inputFormat = new SimpleDateFormat("E MMM dd HH:mm:ss Z yyyy", Locale.ENGLISH);
                            ExpiresAtDate = inputFormat.parse(authTokenObject.getExpiresAt());
                            CurrantAtDate = new Date();

                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY==ExpiresAtDate==" + ExpiresAtDate);
                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY==CurrantAtDate==" + CurrantAtDate);

                            //if (CurrantAtDate.compareTo(ExpiresAtDate) >= 0) {

                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY==SAME DATE==");
                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---refreshToken-----11---" + refreshToken);
                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---getExpiresAt-----11---" + authTokenObject.getExpiresAt());

                            Auth0 account = new Auth0(getString(R.string.com_auth0_client_id), getString(R.string.com_auth0_domain));
                            account.setOIDCConformant(true);
                            AuthenticationAPIClient client = new AuthenticationAPIClient(account);
                            client.renewAuth(refreshToken)
                                    .addParameter("scope", getString(R.string.com_auth0_scope))
                                    .start(new BaseCallback<Credentials, AuthenticationException>() {
                                        @Override
                                        public void onSuccess(Credentials credentials) {

                                            // Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---SUCCESS--------" + credentials.getExpiresAt());

                                            try {
                                                String[] split = new String[0];
                                                split = JWTUtils.decoded(Objects.requireNonNull(credentials.getIdToken()));
                                                String jsonBody = JWTUtils.getJson(split[1]);
                                                JSONObject jsonObj = new JSONObject(jsonBody);

                                                String exp = jsonObj.get(Const.LOG_NOON_EXP).toString();
                                                String sub = jsonObj.get(Const.LOG_NOON_SUB).toString();

                                                userDatabaseRepository.updateAuthToken(sub,
                                                        credentials.getAccessToken(),
                                                        credentials.getIdToken(),
                                                        credentials.getExpiresIn(),
                                                        credentials.getScope(),
                                                        String.valueOf(credentials.getExpiresAt()),
                                                        exp);
                                                PrefUtils.storeAuthid(mycontext, sub);

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

                            /*} else {
                                // Log.e(Const.LOG_NOON_TAG, "SERVICE==NOT SAME DATE==");
                            }*/

                        } catch (ParseException e) {
                            e.printStackTrace();
                        }
                    }
                }
            }
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

    public void locallyLogout() {

        String accessToken = "";
        String idToken = "";
        String authid = PrefUtils.getAuthid(NoonApplication.getContext());
        UserDatabaseRepository userDatabaseRepository = new UserDatabaseRepository();
        if (!TextUtils.isEmpty(authid)) {
            AuthTokenObject authTokenObject = userDatabaseRepository.getAuthTokenData(authid);
            if (authTokenObject != null) {
                if (authTokenObject.getAccessToken() != null) {
                    accessToken = authTokenObject.getAccessToken();
                }
                if (authTokenObject.getIdToken() != null) {
                    idToken = authTokenObject.getIdToken();
                }
            }
        }
        ReminderObject reminderObject = new ReminderObject();
        reminderObject.setReminderToken(accessToken);
        reminderObject.setIdToken(idToken);
        reminderObject.setAccessToken(accessToken);
        reminderObject.setLogoutDate(Const.currantTime);
        reminderObject.setReminderCount(0);
        PrefUtils.clearSharedPreferences(getApplicationContext());
        PrefUtils.storeReminderTokenKey(getApplicationContext(), reminderObject);

        Intent i = new Intent(mycontext, LoginActivity.class);
        i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        mycontext.startActivity(i);
        callApiLogoutUser();
    }

    public void callApiLogoutUser() {
        UserRepository userRepository = new UserRepository();
        disposable.add(userRepository.logout()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UserObject>() {
                    @Override
                    public void onSuccess(UserObject user) {

                    }

                    @Override
                    public void onError(Throwable e) {
                        try {
                            HttpException error = (HttpException) e;
                            UserObject userObject = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), UserObject.class);
                            //showSnackBar(mainDashboardLayoutBinding.drawerLayout, userObject.getMessage());
                        } catch (Exception ignored) {

                        }
                    }
                }));
    }

    public void callApiProgessSyncAdd(List<LessonProgress> lessonProgressList, List<QuizUserResult> quizUserResults) {

        try {
            LessonDatabaseRepository lessonDatabaseRepository = new LessonDatabaseRepository();
            JsonObject noonAppFullSyncObject = new JsonObject();
            JsonArray lessonProgressArray = PrefUtils.convertToJsonArray(lessonProgressList);
            noonAppFullSyncObject.add(Const.PROGRESSDATA, lessonProgressArray);
            //Log.e(Const.LOG_NOON_TAG, "=====lessonProgressArray===" + lessonProgressArray);

            JsonArray quizResultArray = PrefUtils.convertToJsonArray(quizUserResults);
            noonAppFullSyncObject.add(Const.TIMERDATA, quizResultArray);
            //Log.e(Const.LOG_NOON_TAG, "=====quizResultArray===" + quizResultArray);
            Log.e(Const.LOG_NOON_TAG, "=====noonAppFullSyncObject===" + noonAppFullSyncObject);
            LessonRepository lessonRepository = new LessonRepository();
            disposable.add(lessonRepository.ProgessSyncAdd(noonAppFullSyncObject)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<LessonProgress>() {
                        @Override
                        public void onSuccess(LessonProgress lessonProgress) {

                            if (lessonProgressList != null && lessonProgressList.size() != 0) {
                                for (int i = 0; i < lessonProgressList.size(); i++) {
                                    String quizID = lessonProgressList.get(i).getQuizId();
                                    String lessonID = lessonProgressList.get(i).getLessonId();
                                    if (quizID != null && !TextUtils.isEmpty(quizID)) {
                                        lessonDatabaseRepository.updateQuizIdisStatus(quizID, true, userId);
                                    } else {
                                        lessonDatabaseRepository.updateLessonIdisStatus(lessonID, true, userId);
                                    }
                                }
                            }
                            quizDatabaseRepository = new QuizDatabaseRepository();
                            if (quizUserResults != null && quizUserResults.size() != 0) {
                                for (int i = 0; i < quizUserResults.size(); i++) {
                                    String quizID = quizUserResults.get(i).getQuizId();
                                    quizDatabaseRepository.updatelQuizUserResultStatus(true, quizID, userId);
                                }
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            try {
                                HttpException error = (HttpException) e;
                                LessonProgress lessonProgress = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), LessonProgress.class);
                                SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
                                if (!userId.equals("")) {
                                    SyncAPITable syncAPITable = new SyncAPITable();

                                    syncAPITable.setApi_name("ProgressSyncAdd Progressed");
                                    syncAPITable.setEndpoint_url("ProgessSync/ProgessSyncAdd");
                                    syncAPITable.setParameters(String.valueOf(noonAppFullSyncObject));
                                    syncAPITable.setHeaders(PrefUtils.getAuthid(mycontext));
                                    syncAPITable.setStatus("Errored");
                                    syncAPITable.setDescription(e.getMessage());
                                    syncAPITable.setCreated_time(getUTCTime());
                                    syncAPITable.setUserid(Integer.parseInt(userId));
                                    syncAPIDatabaseRepository.insertSyncData(syncAPITable);
                                }
                                //Log.e(Const.LOG_NOON_TAG, "==lessonProgress==" + lessonProgress);
                            } catch (Exception e1) {
                                e1.printStackTrace();

                            }
                        }
                    }));


        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private String getUTCTime() {

        SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH);
        sdf.setTimeZone(TimeZone.getTimeZone("UTC"));
        String gmtTime = sdf.format(new Date());
//        Log.e("date", "getUTCTime: " + gmtTime);
        return gmtTime;
    }

    public void callApiUpdateProfile() {
        userProfileRepository = new UserProfileRepository();
        if (userDetailsObject != null) {
            String username = userDetailsObject.getUsername();
            String userfullname = userDetailsObject.getFullName();
            String userbio = userDetailsObject.getBio();
            String phonenumber = userDetailsObject.getUsername();
            byte[] bitmapImage = userDetailsObject.getProfilepicImage();

            JsonObject gsonObject = new JsonObject();
            JSONObject jsonObject = new JSONObject();
            try {
                jsonObject.put(Const.username, username);
                jsonObject.put(Const.fullname, userfullname);
                jsonObject.put(Const.bio, userbio);
                jsonObject.put(Const.phonenumber, phonenumber);
            } catch (JSONException e) {
                e.printStackTrace();
            }
            JsonParser jsonParser = new JsonParser();
            gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

            disposable.add(userProfileRepository.updateProfile(gsonObject)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<UserObject>() {
                        @Override
                        public void onSuccess(UserObject userObject) {
                            if (bitmapImage != null) {
                                uploadImage(bitmapImage);
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            try {
                                HttpException error = (HttpException) e;
                                UserObject userObject = new Gson().fromJson(Objects.requireNonNull(error.response().errorBody()).string(), UserObject.class);

                            } catch (Exception ignored) {

                            }
                        }
                    }));
        }


    }

    private void uploadImage(byte[] imageFilePath) {
        RequestBody requestFile = RequestBody.create(MediaType.parse("image/jpeg"), imageFilePath);
        MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadImagePara, "image.jpg", requestFile);

        Call<UploadImageObject> call = userProfileRepository.uploadImage(body);
        call.enqueue(new Callback<UploadImageObject>() {
            @Override
            public void onResponse(@NonNull Call<UploadImageObject> call, @NonNull retrofit2.Response<UploadImageObject> response) {

                if (response.isSuccessful()) {
                    UploadImageObject uploadImageObject = response.body();

                    /*PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
                        @Override
                        public UserDetails getLocalUserDetails(UserDetails userDetails) {

                            byte[] bitmapImage = null;
                            if (!TextUtils.isEmpty(uploadImageObject.getData())) {
                                bitmapImage = getByteArrayImage(uploadImageObject.getData());
                            }
                            AppDatabase.getAppDatabase(mycontext).userDetailDao().updateUserPhoto(userDetails.getId(), uploadImageObject.getData(), bitmapImage);
                            return null;
                        }
                    }).execute();*/

                } else {
                    ResponseBody errorBody = response.errorBody();
                    Gson gson = new Gson();
                    try {
                        assert errorBody != null;
                        UploadImageObject uploadImageObject = gson.fromJson(errorBody.string(), UploadImageObject.class);

                    } catch (Exception e) {
                        e.printStackTrace();

                    }
                }
            }

            @Override
            public void onFailure(@NonNull Call<UploadImageObject> call, @NonNull Throwable t) {

            }
        });
    }

}
