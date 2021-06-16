package com.ibl.apps.noon;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.graphics.Typeface;
import android.os.AsyncTask;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.widget.Toast;

import androidx.appcompat.app.AlertDialog;
import androidx.core.content.res.ResourcesCompat;

import com.auth0.android.Auth0;
import com.auth0.android.authentication.AuthenticationAPIClient;
import com.auth0.android.authentication.AuthenticationException;
import com.auth0.android.callback.BaseCallback;
import com.auth0.android.result.Credentials;
import com.google.firebase.auth.FirebaseAuth;
import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.LoginObject;
import com.ibl.apps.Model.SyncRecords;
import com.ibl.apps.Model.TemsCondition;
import com.ibl.apps.Model.UserObject;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.LessonDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.UserDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.QuizUserResult;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.UserCredentialsManagement.UserRepository;
import com.ibl.apps.noon.databinding.LoginLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.JWTUtils;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.Validator;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Objects;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;


public class LoginActivity extends BaseActivity implements View.OnClickListener {

    LoginLayoutBinding loginLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();
    private FirebaseAuth mAuth;
    UserRepository userRepository;
    UserDatabaseRepository userDatabaseRepository;
    LessonDatabaseRepository lessonDatabaseRepository;
    private String macAddress, ipAddress;

    @Override
    protected int getContentView() {
        return R.layout.login_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        loginLayoutBinding = (LoginLayoutBinding) getBindObj();
        mAuth = FirebaseAuth.getInstance();
        userRepository = new UserRepository();
        userDatabaseRepository = new UserDatabaseRepository();
        lessonDatabaseRepository = new LessonDatabaseRepository();

        try {
            PackageInfo pInfo = getPackageManager().getPackageInfo(getPackageName(), 0);
            String version = pInfo.versionName;
            loginLayoutBinding.verstionName.setText("version " + version);
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
        setOnClickListener();
    }

    public void setOnClickListener() {
        Typeface typeface = ResourcesCompat.getFont(LoginActivity.this, R.font.bahij_helvetica_neue_bold);
        loginLayoutBinding.loginPasswordWrapper.setTypeface(typeface);

        loginLayoutBinding.cardLogin.setOnClickListener(this);
        loginLayoutBinding.cardSignup.setOnClickListener(this);
        loginLayoutBinding.forgotPassword.setOnClickListener(this);
        loginLayoutBinding.createNewAccount.setOnClickListener(this);
        loginLayoutBinding.txtPrivacyPolicy.setOnClickListener(this);
       /* loginLayoutBinding.signupTial.setOnClickListener(this);
        loginLayoutBinding.cardLoginTrial.setOnClickListener(this);*/
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.cardLogin:
                if (validateFields()) {
                    String email = loginLayoutBinding.loginEmail.getText().toString().trim();
                    String password = loginLayoutBinding.loginPassword.getText().toString().trim();

                    if (isNetworkAvailable(this)) {
                        Log.e("LOGIN", "---1---");
                        callAutoApiLoginUser(email, password);
                    } else {
                        LocalLogin(email, password, null);
                    }
                }
                break;

            //This implementation has removed from application & it's related to old sign API.
            case R.id.cardSignup:
                if (validateFields()) {
                    if (!Validator.checkPassword(loginLayoutBinding.loginPassword)) {
                        hideKeyBoard(loginLayoutBinding.loginPassword);
                        loginLayoutBinding.loginPasswordWrapper.setError(getString(R.string.validation_passwordStrong));
                    } else {
                        hideKeyBoard(loginLayoutBinding.loginPassword);
                        loginLayoutBinding.loginPasswordWrapper.setErrorEnabled(false);

                        String email = loginLayoutBinding.loginEmail.getText().toString().trim();
                        String password = loginLayoutBinding.loginPassword.getText().toString().trim();

                        if (isNetworkAvailable(this)) {
                            callSignupUser(email, password);
                        } else {
                            showNetworkAlert(LoginActivity.this);
                        }
                    }
                }
                break;

            case R.id.forgotPassword:
                openActivity(ForgotPasswordActivity.class);
                break;

            case R.id.createNewAccount:
                openActivity(SignUpActivity.class);
                break;

            case R.id.txt_privacy_policy:
                showDialog(getResources().getString(R.string.loading));
                disposable.add(userRepository.getTerms()
                        .subscribeOn(Schedulers.io())
                        .observeOn(AndroidSchedulers.mainThread())
                        .subscribeWith(new DisposableSingleObserver<TemsCondition>() {
                            @Override
                            public void onSuccess(TemsCondition temsCondition) {
                                if (temsCondition != null && temsCondition.getData() != null && temsCondition.getData().getTerms() != null) {
                                    if (temsCondition.getResponse_code().equals("0")) {
                                        privarcyPolicyDialog(temsCondition.getData().getTerms());
                                        hideDialog();
                                    }
                                }
                            }

                            @Override
                            public void onError(Throwable e) {
                                hideDialog();
                            }
                        }));
                break;

           /* case R.id.signupTial:
                //openActivity(SignUpTrialActivity.class);
                break;*/

            //  case R.id.cardLoginTrial:

                /*if (validateFields()) {
                    String email = loginLayoutBinding.loginEmail.getText().toString().trim();
                    String password = loginLayoutBinding.loginPassword.getText().toString().trim();
                    Intent intent = new Intent(LoginActivity.this, MainDashBoardTrialActivity.class);
                    startActivity(intent);
                    *//*showDialog(getString(R.string.loading));
                    if (isNetworkAvailable(this)) {
                        mAuth.signInWithEmailAndPassword(email, password)
                                .addOnCompleteListener(new OnCompleteListener<AuthResult>() {
                                    @Override
                                    public void onComplete(@NonNull Task<AuthResult> task) {
                                        if (task.isSuccessful()) {
                                            Toast.makeText(getApplicationContext(), "Login successful!", Toast.LENGTH_LONG).show();
                                            hideDialog();

                                            Intent intent = new Intent(LoginActivity.this, MainDashBoardTrialActivity.class);
                                            startActivity(intent);
                                        } else {
                                            Log.e("TAG", "onComplete: " + task.getException().getMessage());
                                            Toast.makeText(getApplicationContext(), "Login failed! Please try again later", Toast.LENGTH_LONG).show();
                                            hideDialog();
                                        }
                                    }
                                });
                    } else {
                        Toast.makeText(getApplicationContext(), "No Internet! Please try again later", Toast.LENGTH_LONG).show();
//                        LocalLogin(email, password, null);
                    }*//*
                }*/

            //  break;
        }
    }


    private void privarcyPolicyDialog(String terms) {
        AlertDialog.Builder alert = new AlertDialog.Builder(LoginActivity.this);
        WebView webView = new WebView(LoginActivity.this);
        webView.getSettings().setJavaScriptEnabled(true);
        webView.getSettings().setJavaScriptCanOpenWindowsAutomatically(true);
        webView.getSettings().setLoadsImagesAutomatically(true);
        webView.getSettings().setPluginState(WebSettings.PluginState.ON);
        webView.loadDataWithBaseURL(null, terms, "text/html", "UTF-8", null);
        alert.setView(webView);
        alert.create();
        alert.show();
    }

    private boolean validateFields() {
        if (!Validator.checkEmpty(loginLayoutBinding.loginEmail)) {
            hideKeyBoard(loginLayoutBinding.loginEmail);
            loginLayoutBinding.loginEmailWrapper.setError(getString(R.string.validation_enterEmail));
            return false;
        } else {
            hideKeyBoard(loginLayoutBinding.loginEmail);
            loginLayoutBinding.loginEmailWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmail(loginLayoutBinding.loginEmail)) {
            hideKeyBoard(loginLayoutBinding.loginEmail);
            loginLayoutBinding.loginEmailWrapper.setError(getString(R.string.validation_validEmail));
            return false;
        } else {
            hideKeyBoard(loginLayoutBinding.loginEmail);
            loginLayoutBinding.loginEmailWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(loginLayoutBinding.loginPassword)) {
            hideKeyBoard(loginLayoutBinding.loginPassword);
            loginLayoutBinding.loginPasswordWrapper.setError(getString(R.string.validation_enterPassword));
            return false;
        } else {
            hideKeyBoard(loginLayoutBinding.loginPassword);
            loginLayoutBinding.loginPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkLength(loginLayoutBinding.loginPassword)) {
            hideKeyBoard(loginLayoutBinding.loginPassword);
            loginLayoutBinding.loginPasswordWrapper.setError(getString(R.string.validation_passwordLenght));
            return false;
        } else {
            hideKeyBoard(loginLayoutBinding.loginPassword);
            loginLayoutBinding.loginPasswordWrapper.setErrorEnabled(false);
        }
        return true;
    }

    public void LocalLogin(String email, String password, Throwable e) {
        Log.e("LOGIN", "---3---");
        showDialog(getString(R.string.loading));
        LoginObject loginModel = userDatabaseRepository.getUserLoginModel();
        if (loginModel != null) {
            LoginObject loginObject = userDatabaseRepository.getLoginDetail(email, password);
            LoginObject loginEmail = userDatabaseRepository.getLoginEmailDetail(email);
            LoginObject loginPassword = userDatabaseRepository.getLoginPasswordDetail(password);
            if (loginObject != null && loginEmail != null && loginPassword != null) {
                PrefUtils.clearSharedPreferences(getApplicationContext());
                PrefUtils.storeAuthid(getApplicationContext(), loginObject.getSub());
                hideDialog();
                openActivity(MainDashBoardActivity.class);
                finish();
            } else {
                hideDialog();

                if (!isNetworkAvailable(this)) {
                    if (loginEmail == null) {
                        Toast.makeText(getApplicationContext(), R.string.invalid_email_validation, Toast.LENGTH_LONG).show();
                    } else if (loginPassword == null) {
                        Toast.makeText(getApplicationContext(), R.string.validation_password, Toast.LENGTH_LONG).show();
                    } else {
                        Toast.makeText(getApplicationContext(), R.string.no_internet, Toast.LENGTH_LONG).show();
                    }
                } else {
                    if (loginEmail == null) {
                        Toast.makeText(getApplicationContext(), R.string.invalid_email_validation, Toast.LENGTH_LONG).show();
                    } else if (loginPassword == null) {
                        Toast.makeText(getApplicationContext(), R.string.validation_password, Toast.LENGTH_LONG).show();
                    }
                }
            }
        } else {
            hideDialog();
            if (!isNetworkAvailable(getApplicationContext())) {
                Toast.makeText(getApplicationContext(), R.string.no_internet, Toast.LENGTH_LONG).show();
            } else {
                showError(e);
            }
        }
    }

    //This implementation has removed from application & it's related to old sign API.
    public void callSignupUser(String email, String password) {

        showDialog(getString(R.string.loading));

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.email, email);
            jsonObject.put(Const.password, password);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        disposable.add(apiService.signUpUser(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<LoginObject>() {
                    @Override
                    public void onSuccess(LoginObject loginUser) {
                        try {
                            callAutoApiLoginUser(email, password);
                        } catch (Exception e) {
                            showError(e);
                            hideDialog();
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            LoginObject userObject = new Gson().fromJson(error.response().errorBody().string(), LoginObject.class);
                            showSnackBar(loginLayoutBinding.mainLoginLayout, userObject.getMessage());
                        } catch (Exception e1) {
                            showError(e1);
                        }
                    }
                }));
    }

    public void callAutoApiLoginUser(String email, String password) {
        Log.e("LOGIN", "---2---");
//        showDialog(getString(R.string.loading));
        showDialog(getString(R.string.reistering_device));

        //Log.e("1", "---email--------" + email);
        //Log.e("1", "---password--------" + password);

        Auth0 account = new Auth0(getApplicationContext());
        account.setOIDCConformant(true);
        AuthenticationAPIClient authentication = new AuthenticationAPIClient(account);
        // CredentialsManager manager = new CredentialsManager(authentication, new SharedPreferencesStorage(this));
        authentication
                .login(email, password, getString(R.string.com_auth0_database_connection))
                .setAudience(getString(R.string.com_auth0_audience))
                .setScope(getString(R.string.com_auth0_scope))
                .start(new BaseCallback<Credentials, AuthenticationException>() {
                    @Override
                    public void onSuccess(Credentials payload) {
                        try {

                            // Log.e(Const.LOG_NOON_TAG, "====1===" + payload);

                           /* Log.e("1", "---getIdToken--------" + payload.getIdToken());
                            Log.e("2", "-----getRefreshToken------" + payload.getRefreshToken());
                            Log.e("3", "---getScope--------" + payload.getScope());
                            Log.e("5", "----getType-------" + payload.getType());
                            Log.e("1", "---getAccessToken--------" + payload.getAccessToken());
                            Log.e("1", "---getExpiresIn--------" + payload.getExpiresIn());
                            Log.e("1", "---getExpiresAt--------" + payload.getExpiresAt());*/

                            String[] split = new String[0];
                            split = JWTUtils.decoded(Objects.requireNonNull(payload.getIdToken()));
                            String jsonBody = JWTUtils.getJson(split[1]);
                            JSONObject jsonObj = new JSONObject(jsonBody);

                            String exp = jsonObj.get(Const.LOG_NOON_EXP).toString();
                            String sub = jsonObj.get(Const.LOG_NOON_SUB).toString();
                            String aud = jsonObj.get(Const.LOG_NOON_AUD).toString();
                            String iss = jsonObj.get(Const.LOG_NOON_ISS).toString();
                            String iat = jsonObj.get(Const.LOG_NOON_IAT).toString();
                            String username = jsonObj.get(Const.LOG_NOON_NAME).toString();
                            String nickname = jsonObj.get(Const.LOG_NOON_NICKNAME).toString();
                            JSONArray jsonnoonRoles = new JSONArray(jsonObj.getString(Const.LOG_NOON_ROLES));
                            ArrayList<String> roles = new ArrayList<>();
                            if (jsonnoonRoles != null) {
                                for (int i = 0; i < jsonnoonRoles.length(); i++) {
                                    roles.add(jsonnoonRoles.getString(i));
                                }
                            }

                            AuthTokenObject authTokenObject = new AuthTokenObject();
                            authTokenObject.setExp(exp);
                            authTokenObject.setSub(sub);
                            authTokenObject.setAud(aud);
                            authTokenObject.setIss(iss);
                            authTokenObject.setIat(iat);
                            authTokenObject.setUsername(username);
                            authTokenObject.setRoles(roles);
                            authTokenObject.setAccessToken(payload.getAccessToken());
                            authTokenObject.setType(payload.getType());
                            authTokenObject.setIdToken(payload.getIdToken());
                            authTokenObject.setRefreshToken(payload.getRefreshToken());
                            authTokenObject.setExpiresIn(payload.getExpiresIn());
                            authTokenObject.setScope(payload.getScope());
                            authTokenObject.setExpiresAt(String.valueOf(payload.getExpiresAt()));
                            userDatabaseRepository.insertAuthTokenData(authTokenObject);
                            Log.e("storeAuthid", "onSuccess: " + sub);
                            PrefUtils.storeAuthid(LoginActivity.this, sub);

                            callApiUserLogin(email, password, sub);
                            Log.e("LOGIN", "--- Auth0 onSuccess ---");
                            /*manager.clearCredentials();
                            manager.saveCredentials(payload);*/

                        } catch (Exception e) {
                            e.printStackTrace();
                            //Log.e(Const.LOG_NOON_TAG, "---Exception--------" + e.getMessage());
                        }
                    }

                    @Override
                    public void onFailure(AuthenticationException error) {

                        Log.e("LOGIN", "---Auth0 onFailure---" + error);
                        //showError(error);
                        //Log.e(Const.LOG_NOON_TAG, "====2===" + error);

                        //Log.e(Const.LOG_NOON_TAG, "---onFailure--------" + error.getMessage());
                        hideDialog();

                        runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
//                                LocalLogin(email, password, error);
                                // Toast.makeText(LoginActivity.this, error.getDescription(), Toast.LENGTH_SHORT).show();
                                //Log.e(Const.LOG_NOON_TAG, error.getMessage());
                                if (error.getDescription().contains("Wrong"))
                                    Toast.makeText(getApplicationContext(), R.string.validation_email_password_incorrect, Toast.LENGTH_LONG).show();
                                else
                                    Toast.makeText(getApplicationContext(), R.string.many_attempt, Toast.LENGTH_LONG).show();
                            }
                        });
                    }
                });
    }

    public void callApiUserLogin(String email, String password, String sub) {

        String refreshedToken = "";
        if (!TextUtils.isEmpty(PrefUtils.getrefreshedToken(this))) {
            refreshedToken = PrefUtils.getrefreshedToken(this);
        }

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.email, email);
            jsonObject.put(Const.password, password);
            jsonObject.put(Const.deviceType, Const.var_deviceType);
            jsonObject.put(Const.deviceToken, refreshedToken); // FirebaseInstanceId.getInstance().getToken()
            jsonObject.put(Const.version, Const.var_androidOSversion);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        disposable.add(userRepository.loginUser(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<LoginObject>() {
                    @Override
                    public void onSuccess(LoginObject loginUser) {

                        // Log.e(Const.LOG_NOON_TAG, "====3===" + loginUser);
                        try {
                            LoginObject loginUser1 = new LoginObject();
                            loginUser1.setMessage(loginUser.getMessage());
                            loginUser1.setToken(loginUser.getToken());
                            loginUser1.setStatus(loginUser.getStatus());
                            loginUser1.setResponse_code(loginUser.getResponse_code());
                            loginUser1.setEmail(email);
                            loginUser1.setPassword(password);
                            loginUser1.setUserid(loginUser.getUserid());
                            loginUser1.setSub(sub);
                            SharedPreferences sharedPreferences = getSharedPreferences("user", Context.MODE_PRIVATE);
                            SharedPreferences.Editor editor = sharedPreferences.edit();
                            if (editor != null) {
                                editor.clear();
                                editor.putString("uid", loginUser.getUserid());
                                editor.apply();
                            }
                            userDatabaseRepository.insertLoginData(loginUser1);

                            callApiUserDetails(loginUser.getUserid(), sub, email, password);

                        } catch (Exception e) {
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR===" + e);
                            showError(e);
                        }
                    }

                    @Override
                    public void onError(Throwable e) {

                        // Log.e(Const.LOG_NOON_TAG, "====4===" + e);

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            LoginObject userObject = new Gson().fromJson(error.response().errorBody().string(), LoginObject.class);
                            showSnackBar(loginLayoutBinding.mainLoginLayout, userObject.getMessage());
                        } catch (Exception e1) {
                            //showError(e1);
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR==1111=" + e.getMessage());

                            LocalLogin(email, password, e1);
                        }
                    }
                }));
    }

    private void callApiUserDetails(String userId, String sub, String email, String password) {
        try {
//            SharedPreferences sharedPreferences = getSharedPreferences("rolename", MODE_PRIVATE);
//            String outtimrsave = sharedPreferences.getString("userrolename", "");


            disposable.add(userRepository.fetchUser(userId)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<UserObject>() {
                        @SuppressLint("StaticFieldLeak")
                        @Override
                        public void onSuccess(UserObject userObject) {

                            // Log.e(Const.LOG_NOON_TAG, "====5===" + userObject);
                            SharedPreferences preferences = getSharedPreferences("rolename", MODE_PRIVATE);
                            SharedPreferences.Editor editor = preferences.edit();
                            if (editor != null) {
                                editor.putString("userrolename", userObject.getData().getRoleName().get(0));
                                editor.apply();
                            }

                            new AsyncTask<Void, Void, String>() {
                                @Override
                                protected String doInBackground(Void... voids) {

                                    byte[] bitmapImage = null;
                                    if (!TextUtils.isEmpty(userObject.getData().getProfilepicurl())) {
                                        bitmapImage = getByteArrayImage(userObject.getData().getProfilepicurl());
                                    }

                                    UserDetails userDetails = new UserDetails();
                                    userDetails.setPhonenumber(userObject.getData().getPhonenumber());
                                    userDetails.setReminder(userObject.getData().getReminder());
                                    userDetails.setRoleName(userObject.getData().getRoleName());
                                    userDetails.setIntervals(userObject.getData().getIntervals());
                                    userDetails.setIstimeouton(userObject.getData().getIstimeouton());
                                    userDetails.setId(userObject.getData().getId());
                                    userDetails.setUsername(userObject.getData().getUsername());
                                    userDetails.setBio(userObject.getData().getBio());
                                    userDetails.setProfilepicurl(userObject.getData().getProfilepicurl());
                                    userDetails.setEmail(userObject.getData().getEmail());
                                    userDetails.setRoles(userObject.getData().getRoles());
                                    userDetails.setFullName(userObject.getData().getFullName());
                                    userDetails.setIs_skippable(userObject.getData().getIs_skippable());
                                    userDetails.setTimeout(userObject.getData().getTimeout());
                                    userDetails.setSub(sub);
                                    userDetails.setProfilepicImage(bitmapImage);
                                    userDetails.setIs_discussion_authorized(userObject.getData().getIs_discussion_authorized());
                                    userDetails.setIs_library_authorized(userObject.getData().getIs_library_authorized());
                                    userDetails.setIs_assignment_authorized(userObject.getData().getIs_assignment_authorized());
                                    userDatabaseRepository.deleteByUserId(sub);
                                    userDatabaseRepository.insertUserDetails(userDetails);

                                    return null;
                                }
                            }.execute();

                            openActivity(MainDashBoardActivity.class);
                            callApiGetSyncRecords(userId);
                            finish();
                        }

                        @Override
                        public void onError(Throwable e) {

                            //  Log.e(Const.LOG_NOON_TAG, "====6===" + e);
                            hideDialog();
                            try {
                                HttpException error = (HttpException) e;
                                UserObject userObject = new Gson().fromJson(error.response().errorBody().string(), UserObject.class);
                                showSnackBar(loginLayoutBinding.mainLoginLayout, userObject.getMessage());
                            } catch (Exception e1) {
                                LocalLogin(email, password, e);
                                //showError(e);
                                //Log.e(Const.LOG_NOON_TAG, "===ERROR===2222" + e.getMessage());
                            }
                        }
                    }));

        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    public void callApiGetSyncRecords(String userId) {

        disposable.add(userRepository.GetSyncRecords()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<SyncRecords>() {
                    @Override
                    public void onSuccess(SyncRecords syncRecords) {
                        try {
                            if (syncRecords.getData() != null) {
                                SyncRecords.Data syncData = syncRecords.getData();

                                if (syncData != null) {
                                    //Log.e(Const.LOG_NOON_TAG, "===getProgressdata===" + syncData.getProgressdata());
                                    //Log.e(Const.LOG_NOON_TAG, "===getTimerdata===" + syncData.getTimerdata());

                                    if (syncData.getProgressdata() != null && syncData.getProgressdata().size() != 0) {
                                        for (int i = 0; i < syncData.getProgressdata().size(); i++) {
                                            LessonProgress lessonProgress = lessonDatabaseRepository.getItemProgressData(syncData.getProgressdata().get(i).getLessonProgressId(), userId);
                                            if (lessonProgress != null) {
                                                lessonDatabaseRepository.updateLessonUserIdWise(syncData.getProgressdata().get(i).getLessonId(),
                                                        syncData.getProgressdata().get(i).getLessonProgress().split("\\.")[0],
                                                        syncData.getProgressdata().get(i).getGradeId(),
                                                        syncData.getProgressdata().get(i).getUserId(),
                                                        syncData.getProgressdata().get(i).getTotalRecords(),
                                                        syncData.getProgressdata().get(i).getQuizId(),
                                                        true,
                                                        syncData.getProgressdata().get(i).getFileId(),
                                                        syncData.getProgressdata().get(i).getLessonProgressId());

                                            } else {
                                                lessonProgress = new LessonProgress();
                                                lessonProgress.setUserId(syncData.getProgressdata().get(i).getUserId());
                                                lessonProgress.setLessonProgress(syncData.getProgressdata().get(i).getLessonProgress().split("\\.")[0]);
                                                lessonProgress.setLessonId(syncData.getProgressdata().get(i).getLessonId());
                                                lessonProgress.setQuizId(syncData.getProgressdata().get(i).getQuizId());
                                                lessonProgress.setStatus(true);
                                                lessonProgress.setGradeId(syncData.getProgressdata().get(i).getGradeId());
                                                lessonProgress.setTotalRecords(syncData.getProgressdata().get(i).getTotalRecords());
                                                lessonProgress.setFileId(syncData.getProgressdata().get(i).getFileId());
                                                lessonDatabaseRepository.insertLessonProgressData(lessonProgress);
                                            }
                                        }
                                    }
                                    QuizDatabaseRepository quizDatabaseRepository = new QuizDatabaseRepository();
                                    if (syncData.getTimerdata() != null && syncData.getTimerdata().size() != 0) {
                                        for (int i = 0; i < syncData.getTimerdata().size(); i++) {
                                            QuizUserResult quizUserResult = new QuizUserResult();
                                            quizUserResult.setUserId(syncData.getTimerdata().get(i).getUserId());
                                            quizUserResult.setStatus(true);
                                            quizUserResult.setYourScore(syncData.getTimerdata().get(i).getYourScore());
                                            quizUserResult.setPassingScore(syncData.getTimerdata().get(i).getPassingScore());
                                            quizUserResult.setQuizTime(syncData.getTimerdata().get(i).getQuizTime());
                                            quizUserResult.setQuizId(syncData.getTimerdata().get(i).getQuizId());
                                            quizDatabaseRepository.insertAllQuizUserResult(quizUserResult);
                                        }
                                    }
                                }
                            }

                        } catch (Exception e) {
                            //showError(e);
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR===3333" + e.getMessage());
                        }

                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        try {
                            HttpException error = (HttpException) e;
                            SyncRecords syncRecords = new Gson().fromJson(error.response().errorBody().string(), SyncRecords.class);
                            //Log.e(Const.LOG_NOON_TAG, "===SyncRecords=ERROR==" + syncRecords.getMessage());
                            SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
                            if (!userId.equals("")) {
                                SyncAPITable syncAPITable = new SyncAPITable();

                                syncAPITable.setApi_name(getString(R.string.sync_record_progressed));
                                syncAPITable.setEndpoint_url("ProgessSync/GetSyncRecords");
                                syncAPITable.setParameters("");
                                syncAPITable.setHeaders(PrefUtils.getAuthid(LoginActivity.this));
                                syncAPITable.setStatus(getString(R.string.errored_status));
                                syncAPITable.setDescription(e.getMessage());
                                syncAPITable.setCreated_time(getUTCTime());
                                syncAPITable.setUserid(Integer.parseInt(userId));
                                syncAPIDatabaseRepository.insertSyncData(syncAPITable);
                            }
                            hideDialog();
                        } catch (Exception e1) {
                            //showError(e1);
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR===44444" + e.getMessage());
                        }
                    }
                }));
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        disposable.dispose();
    }

    @Override
    protected void onStop() {
        super.onStop();
    }
}
