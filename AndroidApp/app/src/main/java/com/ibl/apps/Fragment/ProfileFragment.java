package com.ibl.apps.Fragment;

import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Model.StatisticsObject;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UserObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.Utils.PrefUtils;
import com.ibl.apps.Utils.Validator;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.ProfileLayoutBinding;
import com.theartofdev.edmodo.cropper.CropImage;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableObserver;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.HttpException;

import static android.app.Activity.RESULT_OK;
import static com.ibl.apps.Base.BaseActivity.apiService;


public class ProfileFragment extends BaseFragment implements View.OnClickListener {

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    ProfileLayoutBinding profileLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    List<UserDetails> userDetailsList;
    UserDetails userDetailsObject = new UserDetails();
    String userId = "0";

    public ProfileFragment() {
        // Required empty public constructor
    }

    public static ProfileFragment newInstance(String param1, String param2) {
        ProfileFragment fragment = new ProfileFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        profileLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.profile_layout, container, false);
        return profileLayoutBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        userDetailsList = AppDatabase.getAppDatabase(NoonApplication.getContext()).userDetailDao().getAllUserDetials();
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
        profileLayoutBinding.edtusername.addTextChangedListener(new TextWatcher() {
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                if (!s.equals("")) {
                    callApiUsernameExiest(s.toString().trim());
                }
            }

            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            public void afterTextChanged(Editable s) {

            }
        });
        profileLayoutBinding.edtphonenumber.setMaxLength(10);
        setOnClickListener();
    }

    public void setOnClickListener() {
        profileLayoutBinding.imagPhotoClick.setOnClickListener(this);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.imagPhotoClick:
                CropImage.activity().start(getContext(), this);
                break;
        }
    }

    public void checkCurrScreenApi() {
        callApiStatistics();
        callApiUserDetails();
    }

    public void hideVisibleLay(boolean flag) {

        profileLayoutBinding.userProfileDataLay.setVisibility(View.VISIBLE);

        /*if (flag) {
            profileLayoutBinding.userStatisticsLay.setVisibility(View.VISIBLE);
            profileLayoutBinding.userProfileDataLay.setVisibility(View.GONE);
        } else {
            profileLayoutBinding.userStatisticsLay.setVisibility(View.GONE);
            profileLayoutBinding.userProfileDataLay.setVisibility(View.VISIBLE);
        }*/
    }

    public void updateUserData() {
        String username = profileLayoutBinding.edtusername.getText().toString().trim();
        String userfullname = profileLayoutBinding.edtFirstName.getText().toString().trim();
        String userPhonenumber = profileLayoutBinding.edtphonenumber.getText().toString().trim();
        callApiUpdateProfile(username, userfullname, "", userPhonenumber);
    }

    public boolean validateFields() {
        if (!Validator.checkEmpty(profileLayoutBinding.edtFirstName)) {
            hideKeyBoard(profileLayoutBinding.edtFirstName);
            profileLayoutBinding.FirstNameWrapper.setError(getString(R.string.validation_enterFirstName));
            return false;
        } else {
            hideKeyBoard(profileLayoutBinding.edtFirstName);
            profileLayoutBinding.FirstNameWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(profileLayoutBinding.edtLastyName)) {
            hideKeyBoard(profileLayoutBinding.edtLastyName);
            profileLayoutBinding.LastNameWrapper.setError(getString(R.string.validation_enterLastName));
            return false;
        } else {
            hideKeyBoard(profileLayoutBinding.edtLastyName);
            profileLayoutBinding.LastNameWrapper.setErrorEnabled(false);
        }


        if (!Validator.checkEmpty(profileLayoutBinding.edtemail)) {
            hideKeyBoard(profileLayoutBinding.edtemail);
            profileLayoutBinding.emailWrapper.setError(getString(R.string.validation_enterEmail));
            return false;
        } else {
            hideKeyBoard(profileLayoutBinding.edtemail);
            profileLayoutBinding.emailWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmail(profileLayoutBinding.edtemail)) {
            hideKeyBoard(profileLayoutBinding.edtemail);
            profileLayoutBinding.emailWrapper.setError(getString(R.string.validation_validEmail));
            return false;
        } else {
            hideKeyBoard(profileLayoutBinding.edtemail);
            profileLayoutBinding.emailWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(profileLayoutBinding.edtphonenumber)) {
            hideKeyBoard(profileLayoutBinding.edtphonenumber);
            profileLayoutBinding.phonenumberWrapper.setError(getString(R.string.validation_enterContact));
            return false;
        } else {
            hideKeyBoard(profileLayoutBinding.edtphonenumber);
            profileLayoutBinding.phonenumberWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkMobileLength(profileLayoutBinding.edtphonenumber)) {
            hideKeyBoard(profileLayoutBinding.edtphonenumber);
            profileLayoutBinding.phonenumberWrapper.setError(getString(R.string.validation_validContact));
            return false;
        } else {
            hideKeyBoard(profileLayoutBinding.edtphonenumber);
            profileLayoutBinding.phonenumberWrapper.setErrorEnabled(false);
        }


        if (!Validator.checkEmpty(profileLayoutBinding.edtusername)) {
            hideKeyBoard(profileLayoutBinding.edtusername);
            profileLayoutBinding.usernameWrapper.setError(getString(R.string.validation_enterUserName));
            return false;
        } else {
            hideKeyBoard(profileLayoutBinding.edtusername);
            profileLayoutBinding.usernameWrapper.setErrorEnabled(false);
        }
        return true;
    }

    private void uploadImage(byte[] imageFilePath) {
        RequestBody requestFile = RequestBody.create(MediaType.parse("image/jpeg"), imageFilePath);
        MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadImagePara, "image.jpg", requestFile);

        Call<UploadImageObject> call = apiService.uploadImage(body);
        showDialog(getString(R.string.loading));
        call.enqueue(new Callback<UploadImageObject>() {
            @Override
            public void onResponse(Call<UploadImageObject> call, retrofit2.Response<UploadImageObject> response) {

                hideDialog();

                if (response.isSuccessful()) {
                    UploadImageObject uploadImageObject = response.body();

                    showSnackBar(profileLayoutBinding.mainChangeProfileLayout, uploadImageObject.getMessage());

                    GlideApp.with(getActivity())
                            .load(uploadImageObject.getData())
                            .error(R.drawable.ic_account_circle_black_24dp)
                            .into(profileLayoutBinding.profileImage);

                    PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
                        @Override
                        public UserDetails getLocalUserDetails(UserDetails userDetails) {
                            if (userDetails != null) {
                                AppDatabase.getAppDatabase(getActivity()).userDetailDao().updateUserPhoto(userDetails.getId(), uploadImageObject.getData(), imageFilePath);
                            }
                            return null;
                        }
                    }).execute();

                } else {
                    ResponseBody errorBody = response.errorBody();
                    Gson gson = new Gson();
                    try {
                        UploadImageObject uploadImageObject = gson.fromJson(errorBody.string(), UploadImageObject.class);
                        showSnackBar(profileLayoutBinding.mainChangeProfileLayout, uploadImageObject.getMessage());
                    } catch (Exception e) {
                        e.printStackTrace();
                        showError(e);
                    }
                }
            }

            @Override
            public void onFailure(Call<UploadImageObject> call, Throwable t) {
                hideDialog();
            }
        });
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == CropImage.CROP_IMAGE_ACTIVITY_REQUEST_CODE) {
            CropImage.ActivityResult result = CropImage.getActivityResult(data);
            if (resultCode == RESULT_OK) {
                Uri resultUri = result.getUri();
                try {
                    InputStream is = getActivity().getContentResolver().openInputStream(resultUri);
                    byte[] bitmapImage = getBytes(is);
                    if (bitmapImage != null) {
                        if (isNetworkAvailable(getActivity())) {
                            uploadImage(bitmapImage);
                        } else {
                            PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
                                @Override
                                public UserDetails getLocalUserDetails(UserDetails userDetails) {

                                    if (userDetails != null) {
                                        GlideApp.with(getActivity())
                                                .asBitmap()
                                                .load(bitmapImage)
                                                .error(R.drawable.ic_account_circle_black_24dp)
                                                .into(profileLayoutBinding.profileImage);

                                        AppDatabase.getAppDatabase(getActivity()).userDetailDao().updateUserPhoto(userDetails.getId(), "", bitmapImage);
                                    }

                                    return null;
                                }
                            }).execute();
                        }
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                }

            } else if (resultCode == CropImage.CROP_IMAGE_ACTIVITY_RESULT_ERROR_CODE) {
                Exception error = result.getError();
            }
        }
    }

    public byte[] getBytes(InputStream is) throws IOException {
        ByteArrayOutputStream byteBuff = new ByteArrayOutputStream();

        int buffSize = 1024;
        byte[] buff = new byte[buffSize];

        int len = 0;
        while ((len = is.read(buff)) != -1) {
            byteBuff.write(buff, 0, len);
        }
        return byteBuff.toByteArray();
    }

    public void callApiUsernameExiest(String username) {

        if (isNetworkAvailable(getActivity())) {
            disposable.add(apiService.userExiest(username)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableObserver<String>() {

                        @Override
                        public void onNext(String s) {
                            //Log.e(Const.LOG_NOON_TAG, "==UserName==" + s.toString());
                            if (s.equals("false")) {
                                profileLayoutBinding.usernameWrapper.setError(getString(R.string.validation_username_already_exist));
                            } else {
                                profileLayoutBinding.usernameWrapper.setErrorEnabled(false);
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            showError(e);
                        }

                        @Override
                        public void onComplete() {
                        }

                    }));
        } else {
            if (userDetailsList != null && !userDetailsList.isEmpty()) {
                for (int i = 0; i < userDetailsList.size(); i++) {
                    if (!userDetailsList.get(i).getId().equals(userId)) {
                        if (userDetailsList.get(i).getUsername().equals(username)) {
                            profileLayoutBinding.usernameWrapper.setError(getString(R.string.validation_username_already_exist));
                        } else {
                            profileLayoutBinding.usernameWrapper.setErrorEnabled(false);
                        }
                    }
                }
            }
        }
    }

    public void callApiUpdateProfile(String username, String userfullname, String userbio, String phonenumber) {

        if (isNetworkAvailable(getActivity())) {
            showDialog(getString(R.string.loading));
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

            disposable.add(apiService.updateProfile(gsonObject)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<UserObject>() {
                        @Override
                        public void onSuccess(UserObject userObject) {
                            hideDialog();
                            showSnackBar(profileLayoutBinding.mainChangeProfileLayout, userObject.getMessage());
                            for (int i = 0; i < userDetailsList.size(); i++) {
                                if (userDetailsList.get(i).getId().equals(userId)) {
                                    AppDatabase.getAppDatabase(NoonApplication.getContext()).userDetailDao().updateUserDetails(userId, username, userfullname, phonenumber);
                                }
                            }
                        }

                        @Override
                        public void onError(Throwable e) {
                            hideDialog();
                            try {
                                HttpException error = (HttpException) e;
                                UserObject userObject = new Gson().fromJson(error.response().errorBody().string(), UserObject.class);
                                showSnackBar(profileLayoutBinding.mainChangeProfileLayout, userObject.getMessage());
                            } catch (Exception e1) {
                                showError(e);
                            }
                        }
                    }));
        } else {

            for (int i = 0; i < userDetailsList.size(); i++) {
                if (userDetailsList.get(i).getId().equals(userId)) {

                    //Log.e(Const.LOG_NOON_TAG, "==userId==" + userId);
                    //Log.e(Const.LOG_NOON_TAG, "==username==" + username);
                    //Log.e(Const.LOG_NOON_TAG, "==userfullname==" + userfullname);
                    //Log.e(Const.LOG_NOON_TAG, "==phonenumber==" + phonenumber);

                    AppDatabase.getAppDatabase(NoonApplication.getContext()).userDetailDao().updateUserDetails(userId, username, userfullname, phonenumber);
                }
            }
            Toast.makeText(getActivity(), getString(R.string.validation_profile_Update), Toast.LENGTH_LONG).show();
        }
    }

    public void callApiStatistics() {

        hideVisibleLay(true);

        if (isNetworkAvailable(getActivity())) {
            showDialog(getString(R.string.loading));
            disposable.add(apiService.StatisticUser()
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<StatisticsObject>() {
                        @Override
                        public void onSuccess(StatisticsObject statisticsobject) {

                            if (statisticsobject.getData() != null) {
                                profileLayoutBinding.txtunreadNoti.setText(statisticsobject.getData().getUnreadnotifications());
                                profileLayoutBinding.txtaverageQuizScore.setText(statisticsobject.getData().getAvragequizscore());
                                profileLayoutBinding.txtuAssignmentScore.setText(statisticsobject.getData().getAvrageassignmentscore());
                                profileLayoutBinding.CompliteCourse.setText(statisticsobject.getData().getComplatecourse());
                                profileLayoutBinding.txtAvailableCourse.setText(statisticsobject.getData().getTotalavailablescore());
                                profileLayoutBinding.txtTerminatedCourse.setText(statisticsobject.getData().getTerminatedcourse());
                                profileLayoutBinding.txtTotalCourse.setText(statisticsobject.getData().getTotalcourse());

                                statisticsobject.setUserId(userId);
                                AppDatabase.getAppDatabase(getActivity()).statisticsDao().insertAll(statisticsobject);
                            }
                            hideDialog();
                        }

                        @Override
                        public void onError(Throwable e) {
                            try {
                                HttpException error = (HttpException) e;
                                StatisticsObject userObject = new Gson().fromJson(error.response().errorBody().string(), StatisticsObject.class);
                            } catch (Exception e1) {
                            }
                        }
                    }));
        } else {
            if (userId != null && !userId.isEmpty()) {
                StatisticsObject statisticsobject = AppDatabase.getAppDatabase(getActivity()).statisticsDao().getStatisticsObject(userId);
                if (statisticsobject != null) {

                    if (statisticsobject.getData() != null) {
                        //Log.e(Const.LOG_NOON_TAG, "==statisticsobject==" + statisticsobject);
                        profileLayoutBinding.txtunreadNoti.setText(statisticsobject.getData().getUnreadnotifications());
                        profileLayoutBinding.txtaverageQuizScore.setText(statisticsobject.getData().getAvragequizscore());
                        profileLayoutBinding.txtuAssignmentScore.setText(statisticsobject.getData().getAvrageassignmentscore());
                        profileLayoutBinding.CompliteCourse.setText(statisticsobject.getData().getComplatecourse());
                        profileLayoutBinding.txtAvailableCourse.setText(statisticsobject.getData().getTotalavailablescore());
                        profileLayoutBinding.txtTerminatedCourse.setText(statisticsobject.getData().getTerminatedcourse());
                        profileLayoutBinding.txtTotalCourse.setText(statisticsobject.getData().getTotalcourse());
                    }
                }
            }
        }
    }

    private void callApiUserDetails() {

        if (isNetworkAvailable(getActivity())) {
            showDialog(getString(R.string.loading));
            try {
                if (userId != null && !userId.isEmpty()) {
                    disposable.add(apiService.fetchUser(userId)
                            .subscribeOn(Schedulers.io())
                            .observeOn(AndroidSchedulers.mainThread())
                            .subscribeWith(new DisposableSingleObserver<UserObject>() {
                                @Override
                                public void onSuccess(UserObject userObject) {

                                    //Log.e(Const.LOG_NOON_TAG, "====userOBJECT===" + userObject);

                                    if (userObject.getData() != null) {
                                        String Profilepicurl = userObject.getData().getProfilepicurl();
                                        String userName = userObject.getData().getUsername();
                                        String userFullName = userObject.getData().getFullName();
                                        String userEmail = userObject.getData().getEmail();
                                        String phonenumber = userObject.getData().getPhonenumber();

                                        if (!TextUtils.isEmpty(Profilepicurl)) {
                                            GlideApp.with(NoonApplication.getContext())
                                                    .load(Profilepicurl)
                                                    .placeholder(R.drawable.personprofile)
                                                    .error(R.drawable.personprofile)
                                                    .into(profileLayoutBinding.profileImage);
                                        }

                                        if (!TextUtils.isEmpty(userFullName)) {
                                            profileLayoutBinding.edtFirstName.setText(userFullName);
                                            profileLayoutBinding.txtProfileUsername.setText(userFullName);
                                        }

                                        if (!TextUtils.isEmpty(userFullName)) {
                                            profileLayoutBinding.edtLastyName.setText(userFullName);
                                        }

                                        if (!TextUtils.isEmpty(userEmail)) {
                                            profileLayoutBinding.edtemail.setText(userEmail);
                                            profileLayoutBinding.txtProfileUserEmail.setText(userEmail);
                                        }

                                        if (!TextUtils.isEmpty(userName)) {
                                            profileLayoutBinding.edtusername.setText(userName);
                                        }

                                        if (!TextUtils.isEmpty(phonenumber)) {
                                            profileLayoutBinding.edtphonenumber.setText(phonenumber);
                                        }

                                    }
                                    hideDialog();

                                }

                                @Override
                                public void onError(Throwable e) {
                                    hideDialog();
                                    try {
                                        HttpException error = (HttpException) e;
                                        UserObject userObject = new Gson().fromJson(error.response().errorBody().string(), UserObject.class);

                                    } catch (Exception e1) {
                                        //showError(e);

                                        if (userDetailsObject != null) {
                                            setLocalData();
                                        } else {
                                            showError(e);
                                        }
                                    }
                                }
                            }));
                }

            } catch (Exception e) {
                e.printStackTrace();
            }

        } else {

            //Log.e(Const.LOG_NOON_TAG, "==NOT NETWORK==");

            setLocalData();
        }
    }


    public void setLocalData() {
        if (userDetailsObject != null) {
            String Profilepicurl = userDetailsObject.getProfilepicurl();
            String userName = userDetailsObject.getUsername();
            String userFullName = userDetailsObject.getFullName();
            String userEmail = userDetailsObject.getEmail();
            String phonenumber = userDetailsObject.getPhonenumber();
            byte[] ProfilepicImage = userDetailsObject.getProfilepicImage();

            //Log.e(Const.LOG_NOON_TAG, "==ProfilepicImage==" + ProfilepicImage);

            if (ProfilepicImage != null) {
                GlideApp.with(NoonApplication.getContext())
                        .asBitmap()
                        .load(ProfilepicImage)
                        .placeholder(R.drawable.personprofile)
                        .error(R.drawable.personprofile)
                        .into(profileLayoutBinding.profileImage);
            }

            if (!TextUtils.isEmpty(userFullName)) {
                profileLayoutBinding.edtFirstName.setText(userFullName);
                profileLayoutBinding.txtProfileUsername.setText(userFullName);
            }

            if (!TextUtils.isEmpty(userFullName)) {
                profileLayoutBinding.edtLastyName.setText(userFullName);
            }

            if (!TextUtils.isEmpty(userEmail)) {
                profileLayoutBinding.edtemail.setText(userEmail);
                profileLayoutBinding.txtProfileUserEmail.setText(userEmail);
            }

            if (!TextUtils.isEmpty(userName)) {
                profileLayoutBinding.edtusername.setText(userName);
            }

            if (!TextUtils.isEmpty(phonenumber)) {
                profileLayoutBinding.edtphonenumber.setText(phonenumber);
            }
        }
    }
}
