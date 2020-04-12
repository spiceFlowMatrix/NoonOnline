package com.ibl.apps.noon;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.MenuItem;
import android.view.View;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.CheckForgetKey;
import com.ibl.apps.Model.LoginObject;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.UserCredentialsManagement.UserRepository;
import com.ibl.apps.noon.databinding.ForgotPasswordLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.Validator;

import org.json.JSONException;
import org.json.JSONObject;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;


public class ForgotPasswordActivity extends BaseActivity implements View.OnClickListener {

    ForgotPasswordLayoutBinding forgotPasswordLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();
    CheckForgetKey checkForgetKeyModel;
    UserRepository userRepository;

    @Override
    protected int getContentView() {
        return R.layout.forgot_password_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        forgotPasswordLayoutBinding = (ForgotPasswordLayoutBinding) getBindObj();
        setToolbar(forgotPasswordLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.forgot_password_header));
        userRepository = new UserRepository();
        forgotPasswordLayoutBinding.passcode.setMaxLength(6);
        setOnClickListener();

    }

    public void setOnClickListener() {
        forgotPasswordLayoutBinding.cardsubmit.setOnClickListener(this);
        forgotPasswordLayoutBinding.cardpasscode.setOnClickListener(this);
        forgotPasswordLayoutBinding.cardforgotPasswrd.setOnClickListener(this);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.cardsubmit:
                if (validateEmailAddress()) {
                    String email = forgotPasswordLayoutBinding.forgotEmail.getText().toString().trim();
                    if (isNetworkAvailable(this)) {
                        callEmailAddressAPI(email);
                    } else {
                        showNetworkAlert(ForgotPasswordActivity.this);
                    }
                }
                break;
            case R.id.cardpasscode:
                if (validatePasscode()) {
                    String passcode = forgotPasswordLayoutBinding.passcode.getText().toString().trim();
                    if (isNetworkAvailable(this)) {
                        callPasscodeAPI(passcode);
                    } else {
                        showNetworkAlert(ForgotPasswordActivity.this);
                    }
                }
                break;
            case R.id.cardforgotPasswrd:
                if (validateChangePassword()) {
                    String newPassword = forgotPasswordLayoutBinding.edtNewPassword.getText().toString().trim();
                    String confirmPassword = forgotPasswordLayoutBinding.edtConfirmPassword.getText().toString().trim();
                    if (isNetworkAvailable(this)) {
                        callApiChangePassword(newPassword, confirmPassword);
                    } else {
                        showNetworkAlert(ForgotPasswordActivity.this);
                    }
                }

                break;
        }
    }

    private boolean validateEmailAddress() {
        if (!Validator.checkEmpty(forgotPasswordLayoutBinding.forgotEmail)) {
            hideKeyBoard(forgotPasswordLayoutBinding.forgotEmail);
            forgotPasswordLayoutBinding.forgotEmailWrapper.setError(getString(R.string.validation_enterEmail));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.forgotEmail);
            forgotPasswordLayoutBinding.forgotEmailWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmail(forgotPasswordLayoutBinding.forgotEmail)) {
            hideKeyBoard(forgotPasswordLayoutBinding.forgotEmail);
            forgotPasswordLayoutBinding.forgotEmailWrapper.setError(getString(R.string.validation_validEmail));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.forgotEmail);
            forgotPasswordLayoutBinding.forgotEmailWrapper.setErrorEnabled(false);
        }
        return true;
    }

    private boolean validatePasscode() {
        if (!Validator.checkEmpty(forgotPasswordLayoutBinding.passcode)) {
            hideKeyBoard(forgotPasswordLayoutBinding.passcode);
            forgotPasswordLayoutBinding.passcodeWrapper.setError(getString(R.string.validation_passcode));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.passcode);
            forgotPasswordLayoutBinding.passcodeWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkLength(forgotPasswordLayoutBinding.passcode)) {
            hideKeyBoard(forgotPasswordLayoutBinding.passcode);
            forgotPasswordLayoutBinding.passcodeWrapper.setError(getString(R.string.validation_passcodelength));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.passcode);
            forgotPasswordLayoutBinding.passcodeWrapper.setErrorEnabled(false);
        }
        return true;
    }

    private boolean validateChangePassword() {

        if (!Validator.checkEmpty(forgotPasswordLayoutBinding.edtNewPassword)) {
            hideKeyBoard(forgotPasswordLayoutBinding.edtNewPassword);
            forgotPasswordLayoutBinding.newPasswordWrapper.setError(getResources().getString(R.string.validation_enterNewPassword));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.edtNewPassword);
            forgotPasswordLayoutBinding.newPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkLength(forgotPasswordLayoutBinding.edtNewPassword)) {
            hideKeyBoard(forgotPasswordLayoutBinding.edtNewPassword);
            forgotPasswordLayoutBinding.newPasswordWrapper.setError(getString(R.string.validation_passwordLenght));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.edtNewPassword);
            forgotPasswordLayoutBinding.newPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkPassword(forgotPasswordLayoutBinding.edtNewPassword)) {
            hideKeyBoard(forgotPasswordLayoutBinding.edtNewPassword);
            forgotPasswordLayoutBinding.newPasswordWrapper.setError(getString(R.string.validation_passwordStrong));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.edtNewPassword);
            forgotPasswordLayoutBinding.newPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(forgotPasswordLayoutBinding.edtConfirmPassword)) {
            hideKeyBoard(forgotPasswordLayoutBinding.edtConfirmPassword);
            forgotPasswordLayoutBinding.confirmPasswordWrapper.setError(getResources().getString(R.string.validation_enterConfirmPassword));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.edtConfirmPassword);
            forgotPasswordLayoutBinding.confirmPasswordWrapper.setErrorEnabled(false);
        }
        if (!Validator.comparePWD(forgotPasswordLayoutBinding.edtNewPassword, forgotPasswordLayoutBinding.edtConfirmPassword)) {
            hideKeyBoard(forgotPasswordLayoutBinding.edtConfirmPassword);
            forgotPasswordLayoutBinding.confirmPasswordWrapper.setError(getResources().getString(R.string.validation_passwordNotMatch));
            return false;
        } else {
            hideKeyBoard(forgotPasswordLayoutBinding.edtConfirmPassword);
            forgotPasswordLayoutBinding.confirmPasswordWrapper.setErrorEnabled(false);
        }

        return true;
    }

    public void callEmailAddressAPI(String email) {

        showDialog(getString(R.string.loading));
        disposable.add(userRepository.ForgotPasswordEmail(email)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UploadImageObject>() {
                    @Override
                    public void onSuccess(UploadImageObject uploadImageObject) {
                        Toast.makeText(getApplicationContext(), uploadImageObject.getMessage(), Toast.LENGTH_LONG).show();
                        hideVisibleLayout(1);
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            UploadImageObject uploadImageObject = new Gson().fromJson(error.response().errorBody().string(), UploadImageObject.class);
                            //forgotPasswordLayoutBinding.forgotEmailWrapper.setError(getString(R.string.user_not_exiest));
                            Toast.makeText(getApplicationContext(), uploadImageObject.getMessage(), Toast.LENGTH_LONG).show();
                        } catch (Exception e1) {
                            //showError(e1);
                            //Log.e(Const.LOG_NOON_TAG, "===ERROR==1111=" + e.getMessage());
                        }
                    }
                }));
    }

    public void callPasscodeAPI(String passcode) {

        showDialog(getString(R.string.loading));
        disposable.add(userRepository.CheckForgetKey(passcode)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<CheckForgetKey>() {
                    @Override
                    public void onSuccess(CheckForgetKey checkForgetKey) {
                        checkForgetKeyModel = checkForgetKey;
                        hideVisibleLayout(2);
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            CheckForgetKey checkForgetKey = new Gson().fromJson(error.response().errorBody().string(), CheckForgetKey.class);
                            forgotPasswordLayoutBinding.passcodeWrapper.setError(getString(R.string.validation_invalidpasscode));
                        } catch (Exception e1) {
                            showError(e1);
                        }
                    }
                }));
    }

    public void callApiChangePassword(String newPassword, String confirmPassword) {

        showDialog(getString(R.string.loading));
        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.password, newPassword);
            jsonObject.put(Const.confirmPassword, confirmPassword);
            jsonObject.put(Const.email, checkForgetKeyModel.getData().getEmail());
            jsonObject.put(Const.authId, checkForgetKeyModel.getData().getAuthId());
            jsonObject.put(Const.isforgot, checkForgetKeyModel.getData().getIsforgot());
            jsonObject.put(Const.forgotkey, checkForgetKeyModel.getData().getForgotkey());

        } catch (JSONException e) {
            e.printStackTrace();
        }
        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());
        disposable.add(userRepository.UpdatePassword(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UploadImageObject>() {

                    @Override
                    public void onSuccess(UploadImageObject uploadImageObject) {
                        hideDialog();
                        SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
                        String userId = sharedPreferencesuser.getString("uid", "");

                        LoginObject loginObject = AppDatabase.getAppDatabase(getApplicationContext()).loginDao().getLoginModelById(userId);
                        if (loginObject != null) {
                            loginObject.setPassword(newPassword);
                            AppDatabase.getAppDatabase(getApplicationContext()).loginDao().updateModel(loginObject);
                        }
                        Toast.makeText(getApplicationContext(), uploadImageObject.getMessage(), Toast.LENGTH_LONG).show();
                        finish();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            UploadImageObject uploadImageObject = new Gson().fromJson(error.response().errorBody().string(), UploadImageObject.class);
                            Toast.makeText(getApplicationContext(), uploadImageObject.getMessage(), Toast.LENGTH_LONG).show();
                        } catch (Exception e1) {
                            showError(e);
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
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                onBackPressed();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        finish();
    }

    public void hideVisibleLayout(int hideVisible) {
        if (hideVisible == 1) {
            forgotPasswordLayoutBinding.layEmailAddress.setVisibility(View.GONE);
            forgotPasswordLayoutBinding.layPasscode.setVisibility(View.VISIBLE);
            forgotPasswordLayoutBinding.layforgotPassword.setVisibility(View.GONE);
        } else if (hideVisible == 2) {
            forgotPasswordLayoutBinding.layEmailAddress.setVisibility(View.GONE);
            forgotPasswordLayoutBinding.layPasscode.setVisibility(View.GONE);
            forgotPasswordLayoutBinding.layforgotPassword.setVisibility(View.VISIBLE);
        } else {
            forgotPasswordLayoutBinding.layEmailAddress.setVisibility(View.VISIBLE);
            forgotPasswordLayoutBinding.layPasscode.setVisibility(View.GONE);
            forgotPasswordLayoutBinding.layforgotPassword.setVisibility(View.GONE);
        }
    }
}
