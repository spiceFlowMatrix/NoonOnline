package com.ibl.apps.noon;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.ColorStateList;
import android.graphics.Typeface;
import android.os.AsyncTask;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.DatePicker;
import android.widget.Toast;

import androidx.appcompat.app.AlertDialog;
import androidx.core.content.res.ResourcesCompat;

import com.auth0.android.Auth0;
import com.auth0.android.authentication.AuthenticationAPIClient;
import com.auth0.android.authentication.AuthenticationException;
import com.auth0.android.callback.BaseCallback;
import com.auth0.android.result.Credentials;
import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.GradeSpinner;
import com.ibl.apps.Model.LoginObject;
import com.ibl.apps.Model.SyncRecords;
import com.ibl.apps.Model.TemsCondition;
import com.ibl.apps.Model.TrileSignupObject;
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
import com.ibl.apps.noon.databinding.ActivitySignUpBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.JWTUtils;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.Validator;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.InputStream;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Iterator;
import java.util.List;
import java.util.Objects;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

public class SignUpActivity extends BaseActivity implements View.OnClickListener {
    ActivitySignUpBinding activitySignUpBinding;
    private CompositeDisposable disposable = new CompositeDisposable();
    int mYear, mMonth, mDay;
    private int citiesInAdapter;
    ArrayList<GradeSpinner.Grade> gradeArrayList = new ArrayList<>();
    List<GradeSpinner.Grade> gradecategories = new ArrayList<GradeSpinner.Grade>();
    String gradeId;
    private UserRepository userRepository;
    private UserDatabaseRepository userDatabaseRepository;
    private LessonDatabaseRepository lessonDatabaseRepository;


    @Override
    protected int getContentView() {
        return R.layout.activity_sign_up;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        activitySignUpBinding = (ActivitySignUpBinding) getBindObj();
        userRepository = new UserRepository();
        userDatabaseRepository = new UserDatabaseRepository();
        lessonDatabaseRepository = new LessonDatabaseRepository();

        setOnClickListener();
        setupView();
    }

    private void setupView() {
        Typeface typeface = ResourcesCompat.getFont(SignUpActivity.this, R.font.bahij_helvetica_neue_bold);
        activitySignUpBinding.signupPasswordWrapper.setTypeface(typeface);
        activitySignUpBinding.signupConfirmPasswordWrapper.setTypeface(typeface);

        GenderSpinner();
        FBSkypySpinner();
        Aboutusspinner();
        Gradespinner();
        loadProvince();
    }

    private void Gradespinner() {
        //For Grade Spinner
       /* List<String> gradecategories = new ArrayList<String>();
        gradecategories.add(6, "Grade 6");
        gradecategories.add(7, "Grade 7");
        gradecategories.add(8, "Grade 8");
        gradecategories.add(9, "Grade 9");
        gradecategories.add(10, "Grade 10");
        gradecategories.add(11, "Grade 11");
        gradecategories.add(12, "Grade 12");*/


        GradeSpinner.Grade gradeModel = new GradeSpinner.Grade();
        gradeModel.setId("6");
        gradeModel.setName(getResources().getString(R.string.grade_6));
        gradecategories.add(gradeModel);

        gradeModel = new GradeSpinner.Grade();
        gradeModel.setId("7");
        gradeModel.setName(getResources().getString(R.string.grade_7));
        gradecategories.add(gradeModel);

        gradeModel = new GradeSpinner.Grade();
        gradeModel.setId("8");
        gradeModel.setName(getResources().getString(R.string.grade_8));
        gradecategories.add(gradeModel);

        gradeModel = new GradeSpinner.Grade();
        gradeModel.setId("9");
        gradeModel.setName(getResources().getString(R.string.grade_9));
        gradecategories.add(gradeModel);

        gradeModel = new GradeSpinner.Grade();
        gradeModel.setId("10");
        gradeModel.setName(getResources().getString(R.string.grade_10));
        gradecategories.add(gradeModel);

        gradeModel = new GradeSpinner.Grade();
        gradeModel.setId("11");
        gradeModel.setName(getResources().getString(R.string.grade_11));
        gradecategories.add(gradeModel);

        gradeModel = new GradeSpinner.Grade();
        gradeModel.setId("12");
        gradeModel.setName(getResources().getString(R.string.grade_12));
        gradecategories.add(gradeModel);

        // Creating adapter for spinner
        ArrayAdapter<GradeSpinner.Grade> gradeArrayAdapter = new ArrayAdapter<GradeSpinner.Grade>(this, R.layout.include_spinner_item, gradecategories);

        // Drop down layout style - list view with radio button
        gradeArrayAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        activitySignUpBinding.gradeSpinner.setAdapter(gradeArrayAdapter);
        activitySignUpBinding.gradeSpinner.setSelection(0);
        activitySignUpBinding.gradeSpinner.setOnItemSelectedListener(
                new AdapterView.OnItemSelectedListener() {
                    public void onItemSelected(AdapterView<?> parent, View view, int pos, long id) {
                        // String getid = event_type.get(position).getId();
                        // Object item = parent.getItemAtPosition(pos);
//                                        System.out.println(item.toString());     //prints the text in spinner item.


                        gradeId = gradecategories.get(pos).getId();

                    }

                    public void onNothingSelected(AdapterView<?> parent) {
                    }
                });


        //Grade Api Calling
         /*disposable.add(apiService.getSpinnerData()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<GradeSpinner>() {
                    @Override
                    public void onSuccess(GradeSpinner grades) {

                        Log.e("onSuccess", "--4386789349673489-" + grades.getGradeArrayList());

                        gradeArrayList.addAll(grades.getGradeArrayList());
                        ArrayAdapter<GradeSpinner.Grade> gradeArrayAdapter = new ArrayAdapter<GradeSpinner.Grade>(SignUpActivity.this, R.layout.include_spinner_item, gradeArrayList);
                        gradeArrayAdapter.setDropDownViewResource(R.layout.include_spinner_item_popup_item);
                        activitySignUpBinding.gradeSpinner.setAdapter(gradeArrayAdapter);


                       *//* activitySignUpBinding.gradeSpinner.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                            @Override
                            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {

                            }
                        });*//*

                        activitySignUpBinding.gradeSpinner.setOnItemSelectedListener(
                                new AdapterView.OnItemSelectedListener() {
                                    public void onItemSelected(AdapterView<?> parent, View view, int pos, long id) {

                                        Object item = parent.getItemAtPosition(pos);
                                        gradeName = item.toString();
//                                        System.out.println(item.toString());     //prints the text in spinner item.

                                    }

                                    public void onNothingSelected(AdapterView<?> parent) {
                                    }
                                });


                    }

                    @Override
                    public void onError(Throwable e) {
                        Log.e("onError", "----" + e.getMessage());
                    }
                }));*/
    }

    private void Aboutusspinner() {
        //For about us Spinner
        List<String> aboutuscategories = new ArrayList<String>();
        aboutuscategories.add(0, getResources().getString(R.string.friend_relative));
        aboutuscategories.add(1, getResources().getString(R.string.school));
        aboutuscategories.add(2, getResources().getString(R.string.social_media));
        aboutuscategories.add(3, getResources().getString(R.string.billboard));
        aboutuscategories.add(4, getResources().getString(R.string.sales_agent));

        // Creating adapter for spinner
        ArrayAdapter<String> aboutusdataAdapter = new ArrayAdapter<String>(this, R.layout.include_spinner_item, aboutuscategories);

        // Drop down layout style - list view with radio button
        aboutusdataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        activitySignUpBinding.aboutusSpinner.setAdapter(aboutusdataAdapter);
        activitySignUpBinding.aboutusSpinner.setOnItemSelectedListener(
                new AdapterView.OnItemSelectedListener() {
                    public void onItemSelected(AdapterView<?> parent, View view, int pos, long id) {

                    }

                    public void onNothingSelected(AdapterView<?> parent) {
                    }
                });
    }

    private void FBSkypySpinner() {
        //For FB/Skypy Spinner
        List<String> FBSkypycategories = new ArrayList<String>();
        FBSkypycategories.add(0, getResources().getString(R.string.facebook));
        FBSkypycategories.add(1, getResources().getString(R.string.skype));

        // Creating adapter for spinner
        ArrayAdapter<String> FBSkypydataAdapter = new ArrayAdapter<String>(this, R.layout.include_spinner_item, FBSkypycategories);
        // Drop down layout style - list view with radio button
        FBSkypydataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        activitySignUpBinding.fbSkypySpinner.setAdapter(FBSkypydataAdapter);

    }

    private void GenderSpinner() {
        //For Gender Spinner
        List<String> Gendercategories = new ArrayList<String>();
        Gendercategories.add(0, getResources().getString(R.string.male));
        Gendercategories.add(1, getResources().getString(R.string.female));

        // Creating adapter for spinner
        ArrayAdapter<String> genderdataAdapter = new ArrayAdapter<String>(this, R.layout.include_spinner_item, Gendercategories);

        // Drop down layout style - list view with radio button
        genderdataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        activitySignUpBinding.genderSpinner.setAdapter(genderdataAdapter);


        activitySignUpBinding.genderSpinner.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {

            }
        });
    }

    public String loadJSONFromAsset() {
        String json = null;
        try {
            InputStream is = getAssets().open("provicen.json");

            Log.e("InputStream", "----" + is);
            int size = is.available();
            byte[] buffer = new byte[size];
            is.read(buffer);
            is.close();
            json = new String(buffer, "UTF-8");
        } catch (IOException ex) {
            ex.printStackTrace();
            return null;
        }
        return json;
    }

    public void loadProvince() {
        try {

            ArrayList<String> province = new ArrayList<>();
            JSONObject obj = new JSONObject(loadJSONFromAsset());
            String key = null;
            //province.add(0, "Select");

            ArrayAdapter<String> provincedataAdapter = new ArrayAdapter<String>(this, R.layout.include_spinner_item, province);

            // Drop down layout style -
            provincedataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
            activitySignUpBinding.provinceSpinner.setAdapter(provincedataAdapter);


            activitySignUpBinding.provinceSpinner.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                @Override
                public void onItemClick(AdapterView<?> adapterView, View view, int pos, long l) {
                    setCitiesInAdapter(province.get(pos), obj);
                }
            });


            for (Iterator<String> it = obj.keys(); it.hasNext(); ) {
                key = it.next();
                province.add(key);
                provincedataAdapter.notifyDataSetChanged();
                Log.e("KEYY STATE---------- ", key);
            }

            // Creating adapter for spinner


//            JSONArray m_jArry = obj.getJSONArray("formules");
//            ArrayList<HashMap<String, String>> formList = new ArrayList<HashMap<String, String>>();
//            HashMap<String, String> m_li;
//
//            for (int i = 0; i < m_jArry.length(); i++) {
//                JSONObject jo_inside = m_jArry.getJSONObject(i);
//
//                Log.d("Details-->", jo_inside.getString("formule"));
//                String formula_value = jo_inside.getString("formule");
//                String url_value = jo_inside.getString("url");
//
//                //Add your values in your `ArrayList` as below:
//                m_li = new HashMap<String, String>();
//                m_li.put("formule", formula_value);
//                m_li.put("url", url_value);
//
//                formList.add(m_li);
//            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void setCitiesInAdapter(String province, JSONObject obj) {
        JSONObject city;
        try {


            ArrayList<String> cityspinner = new ArrayList<>();
            //cityspinner.add(0, "Select");

//            if (!province.equals("0")) {
            city = obj.getJSONObject(province);
            for (Iterator<String> it = city.keys(); it.hasNext(); ) {
                String newkey = it.next();
                cityspinner.add(newkey);
                Log.e("KEYY CITY---------- ", newkey);
//                }
            }

            Log.e("KEY------ ", "" + cityspinner);

            // Creating adapter for spinner
            ArrayAdapter<String> cityedataAdapter = new ArrayAdapter<String>(this, R.layout.include_spinner_item, cityspinner);

            // Drop down layout style - list view with radio button
            cityedataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
            activitySignUpBinding.citySpinner.setAdapter(cityedataAdapter);
            //activitySignUpBinding.citySpinner.setText(cityedataAdapter.getItem(0));

            activitySignUpBinding.citySpinner.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                @Override
                public void onItemClick(AdapterView<?> adapterView, View view, int pos, long l) {

                }
            });
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    private void setOnClickListener() {
        activitySignUpBinding.cardviewsignup.setOnClickListener(this);
        activitySignUpBinding.txtAlreadyAccount.setOnClickListener(this::onClick);
        activitySignUpBinding.edtDateOfBirth.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                activitySignUpBinding.wrraperDateOfBirth.setDefaultHintTextColor(ColorStateList.valueOf(getResources().getColor(R.color.colorPrimary)));
                // your action here
                final Calendar c = Calendar.getInstance();
                mYear = c.get(Calendar.YEAR);
                mMonth = c.get(Calendar.MONTH);
                mDay = c.get(Calendar.DAY_OF_MONTH);


                DatePickerDialog datePickerDialog = new DatePickerDialog(SignUpActivity.this,
                        new DatePickerDialog.OnDateSetListener() {

                            @Override
                            public void onDateSet(DatePicker view, int year,
                                                  int monthOfYear, int dayOfMonth) {

                                Calendar calendar = Calendar.getInstance();
                                calendar.set(year, monthOfYear, dayOfMonth);

                                SimpleDateFormat format = new SimpleDateFormat("dd/MM/yyyy");
                                String strDate = format.format(calendar.getTime());

                                activitySignUpBinding.edtDateOfBirth.setText(strDate);

                            }
                        }, mYear, mMonth, mDay);
                //datePickerDialog.setLocale(new Locale("ar"));
                datePickerDialog.show();
            }
        });
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.cardviewsignup:
                if (validateFields()) {

                    String fbSkype = "";
                    String email = activitySignUpBinding.signupEmail.getText().toString().trim();
                    String password = activitySignUpBinding.signupPassword.getText().toString().trim();
                    String confimpassword = activitySignUpBinding.signupConfirmPassword.getText().toString().trim();
                    String personalName = activitySignUpBinding.signupName.getText().toString().trim();
                    String fatherName = activitySignUpBinding.signupFatherNameSurname.getText().toString().trim();
                    String gender = activitySignUpBinding.genderSpinner.getText().toString().trim();
                    String birthday = activitySignUpBinding.edtDateOfBirth.getText().toString().trim();
                    String birthplace = activitySignUpBinding.signupbirthplace.getText().toString().trim();
                    String province = activitySignUpBinding.provinceSpinner.getText().toString().trim();
                    String city = activitySignUpBinding.citySpinner.getText().toString().trim();
                    String villageArea = activitySignUpBinding.signupVillageArea.getText().toString().trim();
                    String school = activitySignUpBinding.signupSchool.getText().toString().trim();
                    String tazkiranumber = activitySignUpBinding.signupTazKiraNumber.getText().toString().trim();
                    String phonenumber = activitySignUpBinding.signupPhoneNumber.getText().toString().trim();
//                        String fbSkype = activitySignUpBinding.fbSkypySpinner.getText().toString().trim();
                    String signupFbSkypy = activitySignUpBinding.signupFbSkypy.getText().toString().trim();
                    String aboutUs = activitySignUpBinding.aboutusSpinner.getText().toString().trim();
                    String gradeID = gradeId;// activitySignUpBinding.gradeSpinner.getText().toString().trim();
//                        String gradeID = String.valueOf(getgrades(gradeArrayList, activitySignUpBinding.gradeSpinner.getText().toString().trim()));

                    if (activitySignUpBinding.fbSkypySpinner.getText().equals("Select")) {
                        fbSkype = "";
                    } else {
                        fbSkype = activitySignUpBinding.fbSkypySpinner.getText().toString().trim();
                    }

                    if (isNetworkAvailable(this)) {
                        callApiSignLogin(email, password, confimpassword, personalName, fatherName, gender, birthday, birthplace, province, city, villageArea, school, gradeID, tazkiranumber,
                                phonenumber, fbSkype, signupFbSkypy, aboutUs);
                    } else {
                        Toast.makeText(this, getResources().getString(R.string.error_no_internet_connection), Toast.LENGTH_SHORT).show();
                    }


                }
                break;

            case R.id.txtAlreadyAccount:
                openActivity(LoginActivity.class);
                break;

            case R.id.txtTerms:
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
        }

    }

//    private void privarcyPolicyDialog(String terms) {
//        AlertDialog.Builder alert = new AlertDialog.Builder(SignUpActivity.this);
//        WebView webView = new WebView(SignUpActivity.this);
////        webView.setScrollBarStyle(View.SCROLLBARS_INSIDE_OVERLAY);
//        webView.loadDataWithBaseURL(null, terms, "text/html", "UTF-8", null);
//        webView.requestLayout();
//        webView.clearView();
//        webView.clearCache(true);
//        webView.setWebViewClient(new WebViewClient() {
//            @Override
//            public boolean shouldOverrideUrlLoading(WebView view, String url) {
//                view.loadUrl(url);
//                return true;
//            }
//        });
//
////        alert.setNegativeButton("Agree", new DialogInterface.OnClickListener() {
////            @Override
////            public void onClick(DialogInterface dialog, int i) {
////                SharedPreferences sharedPreferences = getSharedPreferences(Const.privacyPolicy, Context.MODE_PRIVATE);
////                SharedPreferences.Editor editor = sharedPreferences.edit();
////                editor.putBoolean(Const.isAgree, true);
////                editor.apply();
////                dialog.dismiss();
////            }
////        });
//
//        alert.setView(webView);
////        alert.setCancelable(false);
//        alert.create();
//        alert.show();
//    }

    private void privarcyPolicyDialog(String terms) {
        AlertDialog.Builder alert = new AlertDialog.Builder(Objects.requireNonNull(SignUpActivity.this));
        WebView webView = new WebView(SignUpActivity.this);
        webView.getSettings().setJavaScriptEnabled(true);
        webView.getSettings().setJavaScriptCanOpenWindowsAutomatically(true);
        webView.getSettings().setLoadsImagesAutomatically(true);
        webView.getSettings().setPluginState(WebSettings.PluginState.ON);
        webView.loadDataWithBaseURL(null, terms, "text/html", "UTF-8", null);
        alert.setView(webView);
        alert.create();
        alert.show();
    }

    public void callApiSignLogin(String email, String password, String repeatpassword, String personalName, String fatherName, String gender, String birthday, String birthplace, String province, String city, String villageArea, String school, String gradeID, String tazkiranumber, String phonenumber, String fbSkype, String signupFbSkypy, String aboutUs) {

        showDialog(getString(R.string.loading));

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.Email, email);
            jsonObject.put(Const.Password, password);
            jsonObject.put(Const.RepeatPassword, repeatpassword);
            jsonObject.put(Const.StudentName, personalName);
            jsonObject.put(Const.FatherName, fatherName);
            jsonObject.put(Const.Gender, gender);
            jsonObject.put(Const.DateOfBirth, birthday);
            jsonObject.put(Const.PlaceOfBirth, birthplace);
            jsonObject.put(Const.Province, province);
            jsonObject.put(Const.City, city);
            jsonObject.put(Const.Village, villageArea);
            jsonObject.put(Const.SchoolName, school);
            jsonObject.put(Const.GradeId, gradeID);
            jsonObject.put(Const.TazkiraNo, tazkiranumber);
            jsonObject.put(Const.Phone, phonenumber);
            jsonObject.put(Const.SoicalMediaLinked, fbSkype);
            jsonObject.put(Const.SocialMediaAccount, signupFbSkypy);
            jsonObject.put(Const.Reference, aboutUs);


        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        disposable.add(userRepository.SignupTrialUser(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<TrileSignupObject>() {
                    @Override
                    public void onSuccess(TrileSignupObject signupTrialUser) {
                        Log.e(Const.LOG_NOON_TAG, "====SUCCESS===" + signupTrialUser);
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
                            TrileSignupObject signupTrialUser = new Gson().fromJson(error.response().errorBody().string(), TrileSignupObject.class);
                            Log.e(Const.LOG_NOON_TAG, "====ERROR===" + signupTrialUser);
                            showToast(signupTrialUser.getMessage());

                        } catch (Exception e1) {

                        }
                    }
                }));
    }

    public void callAutoApiLoginUser(String email, String password) {
        Log.e("LOGIN", "---2---");
        showDialog(getString(R.string.loading));

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
                            split = JWTUtils.decoded(payload.getIdToken());
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
                            PrefUtils.storeAuthid(SignUpActivity.this, sub);

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
                        //Log.e(Const.LOG_NOON_TAG, "====2===" + error);
                        //Log.e(Const.LOG_NOON_TAG, "---onFailure--------" + error.getMessage());
                        hideDialog();

                        runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                LocalLogin(email, password, error);
                                //Log.e(Const.LOG_NOON_TAG, error.getMessage());
                                //Toast.makeText(getApplicationContext(), R.string.validation_email_password_incorrect, Toast.LENGTH_LONG).show();
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
                            showToast(userObject.getMessage());
                            //showSnackBar(loginLayoutBinding.mainLoginLayout, userObject.getMessage());
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

            disposable.add(userRepository.fetchUser(userId)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<UserObject>() {
                        @Override
                        public void onSuccess(UserObject userObject) {
                            SharedPreferences preferences = getSharedPreferences("rolename", MODE_PRIVATE);
                            SharedPreferences.Editor editor = preferences.edit();
                            if (editor != null) {
                                editor.putString("userrolename", userObject.getData().getRoleName().get(0));
                                editor.apply();
                            }
                            // Log.e(Const.LOG_NOON_TAG, "====5===" + userObject);
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
                                showToast(userObject.getMessage());

                                //showSnackBar(activitySignUpBinding.mainLoginLayout, userObject.getMessage());
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

                                syncAPITable.setApi_name("SyncRecords Progressed");
                                syncAPITable.setEndpoint_url("ProgessSync/GetSyncRecords");
                                syncAPITable.setParameters("");
                                syncAPITable.setHeaders(PrefUtils.getAuthid(SignUpActivity.this));
                                syncAPITable.setStatus("Errored");
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

    public void LocalLogin(String email, String password, Throwable e) {
        Log.e("LOGIN", "---3---");
        showDialog(getString(R.string.loading));
        LoginObject loginObject = userDatabaseRepository.getLoginDetail(email, password);
        if (loginObject != null) {
            PrefUtils.clearSharedPreferences(getApplicationContext());
            PrefUtils.storeAuthid(getApplicationContext(), loginObject.getSub());
            hideDialog();
            openActivity(MainDashBoardActivity.class);
            finish();
        } else {
            hideDialog();
            Toast.makeText(getApplicationContext(), R.string.validation_email_password_incorrect, Toast.LENGTH_LONG).show();
            /*if (isNetworkAvailable(this)) {
                //showError(e);
            } else {
                Toast.makeText(getApplicationContext(), R.string.validation_email_password_incorrect, Toast.LENGTH_LONG).show();
            }*/
        }
    }

    private boolean validateFields() {
        /*For Email Validation*/
        if (!Validator.checkEmpty(activitySignUpBinding.signupEmail)) {
            hideKeyBoard(activitySignUpBinding.signupEmail);
            activitySignUpBinding.signupEmailWrapper.setError(getString(R.string.validation_enterEmail));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupEmail);
            activitySignUpBinding.signupEmailWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmail(activitySignUpBinding.signupEmail)) {
            hideKeyBoard(activitySignUpBinding.signupEmail);
            activitySignUpBinding.signupEmailWrapper.setError(getString(R.string.validation_validEmail));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupEmail);
            activitySignUpBinding.signupEmailWrapper.setErrorEnabled(false);
        }

        /*For Password Validation*/
        if (!Validator.checkEmpty(activitySignUpBinding.signupPassword)) {
            hideKeyBoard(activitySignUpBinding.signupPassword);
            activitySignUpBinding.signupPasswordWrapper.setError(getString(R.string.validation_enterPassword));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupPassword);
            activitySignUpBinding.signupPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkLength(activitySignUpBinding.signupPassword)) {
            hideKeyBoard(activitySignUpBinding.signupPassword);
            activitySignUpBinding.signupPasswordWrapper.setError(getString(R.string.validation_passwordLenght));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupPassword);
            activitySignUpBinding.signupPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(activitySignUpBinding.signupConfirmPassword)) {
            hideKeyBoard(activitySignUpBinding.signupConfirmPassword);
            activitySignUpBinding.signupConfirmPasswordWrapper.setError(getResources().getString(R.string.validation_enterConfirmPassword));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupConfirmPassword);
            activitySignUpBinding.signupConfirmPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.comparePWD(activitySignUpBinding.signupPassword, activitySignUpBinding.signupConfirmPassword)) {
            hideKeyBoard(activitySignUpBinding.signupConfirmPassword);
            activitySignUpBinding.signupConfirmPasswordWrapper.setError(getResources().getString(R.string.validation_passwordNotMatch));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupConfirmPassword);
            activitySignUpBinding.signupConfirmPasswordWrapper.setErrorEnabled(false);
        }

        /*For Name Validation*/
        if (!Validator.checkEmpty(activitySignUpBinding.signupName)) {
            hideKeyBoard(activitySignUpBinding.signupName);
            activitySignUpBinding.signupNameWrapper.setError(getString(R.string.validation_enterName));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupName);
            activitySignUpBinding.signupNameWrapper.setErrorEnabled(false);
        }

        /*For FatherName/Surname Validation*/
        if (!Validator.checkEmpty(activitySignUpBinding.signupFatherNameSurname)) {
            hideKeyBoard(activitySignUpBinding.signupFatherNameSurname);
            activitySignUpBinding.signupFatherNameSurnameWrapper.setError(getString(R.string.validation_enterFatherNameSurname));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupFatherNameSurname);
            activitySignUpBinding.signupFatherNameSurnameWrapper.setErrorEnabled(false);
        }

        //For Gender Spinner Validation
        if (!Validator.checkEmpty(activitySignUpBinding.genderSpinner)) {
            hideKeyBoard(activitySignUpBinding.genderSpinner);
            activitySignUpBinding.wrraperSpinnerGender.setError(getString(R.string.ValidationGender));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.genderSpinner);
            activitySignUpBinding.wrraperSpinnerGender.setErrorEnabled(false);
        }

      /*  if (activitySignUpBinding.genderSpinner.getText().toString().equals("Select")) {
            Log.e("genderSpinner", "---if--");
            activitySignUpBinding.txtGender.setTextColor(getResources().getColor(R.color.colorRed));
            activitySignUpBinding.genderSpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background_red));
            return false;
        } else {
            Log.e("genderSpinner", "---else--");
            activitySignUpBinding.genderSpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background));
        }
*/
        /*For edtDateOfBirth Validation*/
        if (!Validator.checkEmpty(activitySignUpBinding.edtDateOfBirth)) {
            hideKeyBoard(activitySignUpBinding.edtDateOfBirth);
            activitySignUpBinding.wrraperDateOfBirth.setError(getString(R.string.validation_enterBirthdate));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.edtDateOfBirth);
            activitySignUpBinding.wrraperDateOfBirth.setErrorEnabled(false);
        }

        //For Province Spinner Validation
        if (!Validator.checkEmpty(activitySignUpBinding.provinceSpinner)) {
            hideKeyBoard(activitySignUpBinding.provinceSpinner);
            activitySignUpBinding.wrraperSpinnerProvince.setError(getString(R.string.ValidationProvince));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.provinceSpinner);
            activitySignUpBinding.wrraperSpinnerProvince.setErrorEnabled(false);
        }

        if (!activitySignUpBinding.chkTerms.isChecked()) {
            showToast(getResources().getString(R.string.accept_term_condition));
            return false;
        }
       /*
        if (activitySignUpBinding.provinceSpinner.getText().toString().equals("Select")) {
            Log.e("provinceSpinner", "--if---");
            activitySignUpBinding.txtProvince.setTextColor(getResources().getColor(R.color.colorRed));
            activitySignUpBinding.provinceSpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background_red));
            return false;
        } else {
            Log.e("provinceSpinner", "--else---");
            activitySignUpBinding.provinceSpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background));
        }*/

        //For City/District Spinner Validation

        if (!Validator.checkEmpty(activitySignUpBinding.citySpinner)) {
            hideKeyBoard(activitySignUpBinding.citySpinner);
            activitySignUpBinding.wrraperSpinnerCity.setError(getString(R.string.ValidationCity));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.citySpinner);
            activitySignUpBinding.wrraperSpinnerCity.setErrorEnabled(false);
        }

     /*   if (activitySignUpBinding.citySpinner.getText().toString().equals("Select")) {
            Log.e("citySpinner", "---if--");
            activitySignUpBinding.txtCity.setTextColor(getResources().getColor(R.color.colorRed));
            activitySignUpBinding.citySpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background_red));
            return false;
        } else {
            Log.e("citySpinner", "---else--");
            activitySignUpBinding.citySpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background));
        }*/

        /*  *//*For birthplace Validation*//*
        if (!Validator.checkEmpty(activitySignUpBinding.signupbirthplace)) {
            hideKeyBoard(activitySignUpBinding.signupbirthplace);
            activitySignUpBinding.signupbirthplaceWrapper.setError(getString(R.string.validation_enterBirthPlace));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupbirthplace);
            activitySignUpBinding.signupbirthplaceWrapper.setErrorEnabled(false);
        }*/

        /*For VillageArea Validation*/
        if (!Validator.checkEmpty(activitySignUpBinding.signupVillageArea)) {
            hideKeyBoard(activitySignUpBinding.signupVillageArea);
            activitySignUpBinding.signupVillageAreaWrapper.setError(getString(R.string.validation_enterVillageArea));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupVillageArea);
            activitySignUpBinding.signupVillageAreaWrapper.setErrorEnabled(false);
        }

        /* For School Validation*/
        if (!Validator.checkEmpty(activitySignUpBinding.signupSchool)) {
            hideKeyBoard(activitySignUpBinding.signupSchool);
            activitySignUpBinding.signupSchoolWrapper.setError(getString(R.string.validation_enterSchool));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupSchool);
            activitySignUpBinding.signupSchoolWrapper.setErrorEnabled(false);
        }

        //For Grade Spinner
        if (!Validator.checkEmpty(activitySignUpBinding.gradeSpinner)) {
            hideKeyBoard(activitySignUpBinding.gradeSpinner);
            activitySignUpBinding.wrraperSpinnerGrade.setError(getString(R.string.ValidationGrade));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.gradeSpinner);
            activitySignUpBinding.wrraperSpinnerGrade.setErrorEnabled(false);
        }

       /* if (activitySignUpBinding.gradeSpinner.getText().toString().trim().equals("Select")) {
            Log.e("gradeSpinner", "---if--");
            activitySignUpBinding.txtGrade.setTextColor(getResources().getColor(R.color.colorRed));
            activitySignUpBinding.gradeSpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background_red));
            return false;
        } else {
            Log.e("gradeSpinner", "---else--");
            activitySignUpBinding.gradeSpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background));
        }*/

        /*For TazKiraNumber Validation*/
      /*  if (!Validator.checkEmpty(activitySignUpBinding.signupTazKiraNumber)) {
            hideKeyBoard(activitySignUpBinding.signupTazKiraNumber);
            activitySignUpBinding.signupTazKiraNumberWrapper.setError(getString(R.string.validation_enterTazkiraNumber));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupTazKiraNumber);
            activitySignUpBinding.signupTazKiraNumberWrapper.setErrorEnabled(false);
        }*/

        /*For PhoneNumber Validation*/
        if (!Validator.checkEmpty(activitySignUpBinding.signupPhoneNumber)) {
            hideKeyBoard(activitySignUpBinding.signupPhoneNumber);
            activitySignUpBinding.signupPhoneNumberWrapper.setError(getString(R.string.ValidationPhoneNumber));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupPhoneNumber);
            activitySignUpBinding.signupPhoneNumberWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkMobileLength(activitySignUpBinding.signupPhoneNumber)) {
            hideKeyBoard(activitySignUpBinding.signupPhoneNumber);
            activitySignUpBinding.signupPhoneNumberWrapper.setError(getString(R.string.ValidationContactNumber));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.signupPhoneNumber);
            activitySignUpBinding.signupPhoneNumberWrapper.setErrorEnabled(false);
        }


        //For about us Spinner Validation
        if (!Validator.checkEmpty(activitySignUpBinding.aboutusSpinner)) {
            hideKeyBoard(activitySignUpBinding.aboutusSpinner);
            activitySignUpBinding.wrraperSpinnerAboutus.setError(getString(R.string.ValidationAboutus));
            return false;
        } else {
            hideKeyBoard(activitySignUpBinding.aboutusSpinner);
            activitySignUpBinding.wrraperSpinnerAboutus.setErrorEnabled(false);
        }



      /*  if (activitySignUpBinding.aboutusSpinner.getText().toString().equals("Select")) {
            Log.e("aboutusSpinner", "---if--");
            activitySignUpBinding.txtaboutus.setTextColor(getResources().getColor(R.color.colorRed));
            activitySignUpBinding.aboutusSpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background_red));
            return false;
        } else {
            Log.e("aboutusSpinner", "---else--");
            activitySignUpBinding.aboutusSpinner.setBackground(getResources().getDrawable(R.drawable.spinner_background));
        }*/
        return true;


    }

    public int getCitiesInAdapter() {
        return citiesInAdapter;
    }

    private int getgrades(ArrayList<GradeSpinner.Grade> mArrayList, String str) {
        for (int i = 0; i < mArrayList.size(); i++) {
            if (mArrayList.get(i).getName().indexOf(str) != -1) {
                return Integer.parseInt(mArrayList.get(i).getId());
            }
        }
        return -1;
    }
}

