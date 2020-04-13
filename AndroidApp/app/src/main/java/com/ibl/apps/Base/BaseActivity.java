package com.ibl.apps.Base;

import android.accounts.NetworkErrorException;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Configuration;
import androidx.databinding.DataBindingUtil;
import androidx.databinding.ViewDataBinding;
import android.graphics.Typeface;
import android.os.Bundle;
import androidx.annotation.CallSuper;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;
import androidx.core.content.res.ResourcesCompat;
import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.inputmethod.InputMethodManager;
import android.widget.TextView;
import android.widget.Toast;

import com.auth0.android.Auth0;
import com.auth0.android.authentication.AuthenticationAPIClient;
import com.auth0.android.authentication.AuthenticationException;
import com.auth0.android.callback.BaseCallback;
import com.auth0.android.result.Credentials;
import com.crashlytics.android.Crashlytics;
import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.google.firebase.FirebaseApp;
import com.google.firebase.analytics.FirebaseAnalytics;
import com.google.gson.Gson;
import com.ibl.apps.FeedbackManagement.FeedbackRepository;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.ReminderObject;
import com.ibl.apps.Model.UserObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.UserDatabaseRepository;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.UserCredentialsManagement.UserRepository;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.util.JWTUtils;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.TimerTask;
import com.ibl.apps.noon.LoginActivity;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedInputStream;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.net.SocketTimeoutException;
import java.net.URL;
import java.net.URLConnection;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

import cz.msebera.android.httpclient.util.ByteArrayBuffer;
import io.fabric.sdk.android.Fabric;
import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;


public abstract class BaseActivity extends AppCompatActivity implements DroidListener {

    ProgressDialog mProgressDialog;
    ViewDataBinding bingObj;
    public static ApiService apiService;
    private CompositeDisposable disposable = new CompositeDisposable();
    private FirebaseAnalytics mFirebaseAnalytics;
    UserDetails userDetailsObject = new UserDetails();
    String userId = "0";
    DroidNet mDroidNet;
    public static boolean isNetworkConnected;
    private TimerTask timerTask;
    private UserRepository userRepository;
    private UserDatabaseRepository userDatabaseRepository;
    //LogoutPopupLayoutBinding logoutPopupLayoutBinding;

    @SuppressLint("NewApi")
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        bingObj = DataBindingUtil.setContentView(this, getContentView());
        onViewReady(savedInstanceState, getIntent());
        FirebaseApp.initializeApp(this);
//        getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
        userDatabaseRepository = new UserDatabaseRepository();
        apiService = ApiClient.getClient(getApplicationContext()).create(ApiService.class);
        Fabric.with(this, new Crashlytics());
//        try {
////            File filename = new File(Environment.getExternalStorageDirectory() + "/logfile.txt");
//            File filename = new File(Const.destPath + "logfile.txt");
//            filename.createNewFile();
//            String cmd = "logcat -d -f " + filename.getAbsolutePath();
//            Runtime.getRuntime().exec(cmd);
//            Log.e("logfile", "onCreate: " + Runtime.getRuntime().exec(cmd));
//        } catch (IOException e) {
//            // TODO Auto-generated catch block
//            e.printStackTrace();
//        }
        mFirebaseAnalytics = FirebaseAnalytics.getInstance(this);
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetails.getId();
                    if (isNetworkAvailable(BaseActivity.this)) {
                        refreshToken();
                    }
                }
                return null;
            }

        }).execute();
    }

    @CallSuper
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        //To be used by child activities
    }

    protected void showBackArrow(String title) {
        ActionBar supportActionBar = getSupportActionBar();
        if (supportActionBar != null) {
            supportActionBar.setDisplayHomeAsUpEnabled(true);
            supportActionBar.setDisplayShowHomeEnabled(true);
            getSupportActionBar().setTitle(title);
        }
    }

    protected void setToolbar(Toolbar toolbar) {
        setSupportActionBar(toolbar);
    }

    public void showDialog(String message) {
        /*if (mProgressDialog == null) {
            mProgressDialog = new ProgressDialog(new ContextThemeWrapper(this, R.style.MyProgressDialog));
            mProgressDialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            mProgressDialog.setCancelable(true);
            mProgressDialog.setCanceledOnTouchOutside(false);
        }
        mProgressDialog.setMessage(message);
        mProgressDialog.show();*/


        if (mProgressDialog == null) {
            mProgressDialog = new ProgressDialog(this);
            mProgressDialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            mProgressDialog.setCancelable(true);
            mProgressDialog.setCanceledOnTouchOutside(false);

          /*  SpannableString spannableString =  new SpannableString(message);
            CalligraphyTypefaceSpan typefaceSpan = new CalligraphyTypefaceSpan(TypefaceUtils.load(getAssets(), "fonts/Lato-Regular.ttf"));
            spannableString.setSpan(typefaceSpan, 0, message.length(), Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
*/
        }
        mProgressDialog.setMessage(message);
        mProgressDialog.show();

    }

    protected void hideDialog() {
        if (mProgressDialog != null && mProgressDialog.isShowing()) {
            mProgressDialog.dismiss();
        }
    }

    protected boolean isShowing() {
        if (mProgressDialog == null) {
            return false;
        }

        return mProgressDialog.isShowing();
    }

    protected void showToast(String message) {
        Toast.makeText(this, message, Toast.LENGTH_SHORT).show();
    }

    protected void showToast(int message) {
        Toast.makeText(this, message, Toast.LENGTH_SHORT).show();
    }

    protected void showLog(String Tag, String message) {
        Log.e(Tag, message);
    }

    protected void openActivity(Class destination) {
        startActivity(new Intent(this, destination));
        overridePendingTransitionEnter();
    }

    @Override
    public void finish() {
        super.finish();
        overridePendingTransitionExit();
    }

    protected void openFragment(int container, Fragment fragment) {
        FragmentManager fragmentManager = getSupportFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        fragmentTransaction.replace(container, fragment);
        fragmentTransaction.commit();
    }

    public void openFragmentWithBackStack(int container, Fragment fragment) {
        FragmentManager fragmentManager = getSupportFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        fragmentTransaction.addToBackStack(fragment.getClass().getSimpleName());
        fragmentTransaction.add(container, fragment);
        fragmentTransaction.commit();
    }

    protected abstract int getContentView();

    public void Visible(View ivBack) {
        ivBack.setVisibility(View.VISIBLE);
    }

    public void Gone(View ivBack) {
        ivBack.setVisibility(View.GONE);
    }

    public ViewDataBinding getBindObj() {
        return bingObj;
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

    public static void showNetworkAlert(Context activity) {
        try {
            SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.validation_check_internet));
            AlertDialog.Builder builder = new AlertDialog.Builder(activity);
            builder.setTitle(R.string.validation_warning);
            builder.setMessage(message)
                    .setPositiveButton(R.string.button_ok, new DialogInterface.OnClickListener() {
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

    protected void showSnackBar(View layout, String message) {
        /*Snackbar snackbar = Snackbar.make(layout, message, Snackbar.LENGTH_LONG);
        snackbar.show();*/
        showToast(message);
    }

    public void hideKeyBoard(View view) {
        InputMethodManager imm = (InputMethodManager) getSystemService(INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(view.getWindowToken(), 0);
    }

    public void showError(Throwable e) {
        String message = "";
        try {
            if (isNetworkAvailable(this)) {
                if (e instanceof IOException) {
                    message = getString(R.string.error_unknown);
                } else if (e instanceof HttpException) {
                    HttpException error = (HttpException) e;
                    String errorBody = error.response().errorBody().string();
                    JSONObject jObj = new JSONObject(errorBody);
                    message = jObj.getString(getString(R.string.error_unknown));
                } else if (e instanceof SocketTimeoutException) {
                    message = getString(R.string.error_time_out);
                } else if (e instanceof NetworkErrorException) {
                    message = getString(R.string.error_no_internet_connection);
                }
                if (TextUtils.isEmpty(message)) {
                    message = getString(R.string.error_unknown);
                }
                Toast.makeText(getApplicationContext(), message, Toast.LENGTH_LONG).show();
            } else {
                //showNetworkAlert(this);
            }

        } catch (IOException e1) {
            e1.printStackTrace();
        } catch (JSONException e1) {
            e1.printStackTrace();
        } catch (Exception e1) {
            e1.printStackTrace();
        }

        /*Snackbar snackbar = Snackbar.make(layout, message, Snackbar.LENGTH_LONG);
        View sbView = snackbar.getView();
        TextView textView = sbView.findViewById(android.support.design.R.id.snackbar_text);
        textView.setTextColor(getResources().getColor(R.color.colorWhite));
        snackbar.show();*/
    }

    /**
     * Overrides the pending Activity transition by performing the "Enter" animation.
     */
    protected void overridePendingTransitionEnter() {
        overridePendingTransition(R.anim.slide_from_right, R.anim.slide_to_left);
    }

    /**
     * Overrides the pending Activity transition by performing the "Exit" animation.
     */
    protected void overridePendingTransitionExit() {
        overridePendingTransition(R.anim.slide_from_left, R.anim.slide_to_right);
    }

    public void locallyLogout() {
        String accessToken = "";
        String idToken = "";
        String authid = PrefUtils.getAuthid(NoonApplication.getContext());
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
        reminderObject.setReminderCount(1);

        String refreshedToken = "";
        if (!TextUtils.isEmpty(PrefUtils.getrefreshedToken(this))) {
            refreshedToken = PrefUtils.getrefreshedToken(this);
        }

//        SharedPreferences.Editor sharedPreferences = getSharedPreferences(Const.privacyPolicy, Context.MODE_PRIVATE).edit();
//        sharedPreferences.clear();
//        sharedPreferences.apply();
        SharedPreferences.Editor sharedPreferences = getSharedPreferences("user", Context.MODE_PRIVATE).edit();
        sharedPreferences.clear();
        sharedPreferences.apply();

        PrefUtils.clearSharedPreferences(getApplicationContext());
        PrefUtils.storeReminderTokenKey(getApplicationContext(), reminderObject);
        PrefUtils.storerefreshedToken(getApplicationContext(), refreshedToken);
        finish();

        Intent i = new Intent(NoonApplication.getContext(), LoginActivity.class);
        i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        NoonApplication.getContext().startActivity(i);

    }

    public void callApiLogoutUser() {
        userRepository = new UserRepository();
        disposable.add(userRepository.logout()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UserObject>() {
                    @Override
                    public void onSuccess(UserObject user) {
                        locallyLogout();
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        locallyLogout();
                        try {
                            HttpException error = (HttpException) e;
                            UserObject userObject = new Gson().fromJson(error.response().errorBody().string(), UserObject.class);
                            //showSnackBar(mainDashboardLayoutBinding.drawerLayout, userObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    public void showLogoutAlert(Context activity) {
        try {

            SpannableStringBuilder message = setTypeface(activity, getResources().getString(R.string.validation_logout));
            AlertDialog.Builder builder = new AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                            if (isNetworkAvailable(activity)) {
                                callApiLogoutUser();
                            } else {
                                locallyLogout();
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

    public static byte[] getByteArrayImage(String url) {
        try {
            URL imageUrl = new URL(url);
            URLConnection ucon = imageUrl.openConnection();

            InputStream is = ucon.getInputStream();
            BufferedInputStream bis = new BufferedInputStream(is);

            ByteArrayBuffer baf = new ByteArrayBuffer(5000);
            int current = 0;
            while ((current = bis.read()) != -1) {
                baf.append((byte) current);
            }

            return baf.toByteArray();
        } catch (Exception e) {
            Log.e("ImageManager", "Error: " + e.toString());
        }
        return null;
    }

    public void refreshToken() {

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
                            SimpleDateFormat inputFormat = new SimpleDateFormat("E MMM dd HH:mm:ss Z yyyy");
                            ExpiresAtDate = (Date) inputFormat.parse(authTokenObject.getExpiresAt());
                            CurrantAtDate = new Date();

                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY==ExpiresAtDate==" + ExpiresAtDate);
                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY==CurrantAtDate==" + CurrantAtDate);

                            // if (CurrantAtDate.compareTo(ExpiresAtDate) >= 0) {

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
                                                split = JWTUtils.decoded(credentials.getIdToken());
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
                                                PrefUtils.storeAuthid(BaseActivity.this, sub);

                                            } catch (Exception e) {
                                                // Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---Exception----222----" + e.getMessage());

                                                runOnUiThread(new Runnable() {
                                                    @Override
                                                    public void run() {
                                                        Toast.makeText(getApplicationContext(), e.getMessage(), Toast.LENGTH_LONG).show();

                                                        // Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY--CATCH-credentials--------" + e.getMessage());
                                                    }
                                                });
                                            }
                                        }

                                        @Override
                                        public void onFailure(AuthenticationException error) {
                                            //FAILURE
                                            //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---Exception---000-----" + error.getMessage());
                                            runOnUiThread(new Runnable() {
                                                @Override
                                                public void run() {
                                                    //Toast.makeText(getApplicationContext(), "==AuthenticationException onFailure==" + error.getMessage(), Toast.LENGTH_LONG).show();
                                                    //Log.e(Const.LOG_NOON_TAG, "BASEACTIVITY---AuthenticationException--------" + error.getMessage());
                                                }
                                            });
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

    public static void freeMemory(Context context) {

        try {
            File dir = context.getCacheDir();
            deleteDir(dir);
        } catch (Exception e) {
            e.printStackTrace();
        }
        System.runFinalization();
        Runtime.getRuntime().gc();
        System.gc();
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

    @Override
    protected void onResume() {

        mFirebaseAnalytics.setCurrentScreen(this, getClass().getSimpleName(), null);

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {

                if (userDetails != null) {
                    String userId = "";
                    String userName = "";
                    String fullName = "";
                    if (userDetails.getId() != null) {
                        userId = userDetails.getId();
                    }

                    if (userDetails.getUsername() != null) {
                        userName = userDetails.getUsername();
                    }

                    if (userDetails.getFullName() != null) {
                        fullName = userDetails.getFullName();
                    }
                    String NOON_CLASSNAME = "NoonApplication";

                    if (getClass().getSimpleName().equals("SplashActivity")) {
                        NOON_CLASSNAME = "SplashScreen";
                    } else if (getClass().getSimpleName().equals("LoginActivity")) {
                        NOON_CLASSNAME = "LoginScreen";
                    } else if (getClass().getSimpleName().equals("MainDashBoardActivity")) {
                        NOON_CLASSNAME = "CourseBrowsingScreen";
                    } else if (getClass().getSimpleName().equals("ChapterActivity")) {
                        NOON_CLASSNAME = "CourseDetailScreen";
                    } else if (getClass().getSimpleName().equals("SearchActivity")) {
                        NOON_CLASSNAME = "SearchScreen";
                    } else if (getClass().getSimpleName().equals("ResetPasswordActivity")) {
                        NOON_CLASSNAME = "ResetPasswordScreen";
                    }

                    Bundle bundle = new Bundle();
                    bundle.putString(Const.NOON_USERID, userId);
                    bundle.putString(Const.NOON_USERNAME, userName);
                    bundle.putString(Const.NOON_FULLNAME, fullName);
                    bundle.putString(Const.NOON_GRADEID, "0");
                    bundle.putString(Const.NOON_LESSONID, "0");
                    bundle.putString(Const.NOON_CHAPTERID, "0");
                    bundle.putString(Const.NOON_FILEID, "0");
                    bundle.putString(Const.NOON_QUIZID, "0");
                    mFirebaseAnalytics.logEvent(NOON_CLASSNAME, bundle);
                }
                return null;
            }
        }).execute();
        timerTask = new TimerTask(NoonApplication.getContext());
        timerTask.doTimerTask();

        super.onResume();
    }

    @Override
    protected void onStop() {
        timerTask.stopTimerTask();
        super.onStop();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        if (mProgressDialog != null && mProgressDialog.isShowing()) {
            mProgressDialog.dismiss();
        }
        TimerTask timerTask = new TimerTask(NoonApplication.getContext());
        timerTask.stopTimerTask();
        System.runFinalization();
        Runtime.getRuntime().gc();
        System.gc();
    }

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }


    public void setLocale(String language) {
        Locale locale = new Locale(language);
        Locale.setDefault(locale);
        Configuration configuration = new Configuration();
        configuration.locale = locale;
        getBaseContext().getResources().updateConfiguration(configuration, getBaseContext().getResources().getDisplayMetrics());

        SharedPreferences.Editor editor = getSharedPreferences("Settings", MODE_PRIVATE).edit();
        editor.putString("My_Language", language);
        editor.apply();
    }

    public void loadLocale() {
        SharedPreferences preferences = getSharedPreferences("Settings", Activity.MODE_PRIVATE);
        String selectLan = preferences.getString("My_Language", "");
        setLocale(selectLan);
    }

    @Override
    protected void onPause() {
        timerTask.stopTimerTask();
        super.onPause();
    }
}
