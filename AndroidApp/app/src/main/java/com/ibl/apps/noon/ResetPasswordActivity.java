package com.ibl.apps.noon;

import android.content.Intent;
import android.graphics.Typeface;
import android.os.Bundle;
import androidx.core.content.res.ResourcesCompat;

import android.view.MenuItem;
import android.view.View;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.UserObject;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.Validator;
import com.ibl.apps.noon.databinding.ResetPasswordLayoutBinding;

import org.json.JSONException;
import org.json.JSONObject;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;


public class ResetPasswordActivity extends BaseActivity implements View.OnClickListener {

    ResetPasswordLayoutBinding resetPasswordLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    @Override
    protected int getContentView() {
        return R.layout.reset_password_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        resetPasswordLayoutBinding = (ResetPasswordLayoutBinding) getBindObj();
//        resetPasswordLayoutBinding.edtOldPassword.setTypeface(Typeface.DEFAULT);
//        resetPasswordLayoutBinding.edtOldPassword.setTransformationMethod(new PasswordTransformationMethod());
        setOnClickListener();
        setToolbar(resetPasswordLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.reset_password));

        Typeface typeface = ResourcesCompat.getFont(ResetPasswordActivity.this, R.font.bahij_helvetica_neue_bold);
        resetPasswordLayoutBinding.oldPasswordWrapper.setTypeface(typeface);
        resetPasswordLayoutBinding.newPasswordWrapper.setTypeface(typeface);
        resetPasswordLayoutBinding.confirmPasswordWrapper.setTypeface(typeface);

        resetPasswordLayoutBinding.toolbarLayout.resetButton.setVisibility(View.VISIBLE);
    }

    public void setOnClickListener() {
        resetPasswordLayoutBinding.toolbarLayout.resetButton.setOnClickListener(this);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.resetButton:
                if (validateFields()) {

                    String newPassword = resetPasswordLayoutBinding.edtNewPassword.getText().toString().trim();
                    String oldPassword = resetPasswordLayoutBinding.edtOldPassword.getText().toString().trim();
                    String confirmPassword = resetPasswordLayoutBinding.edtConfirmPassword.getText().toString().trim();

                    callApiChangePassword(newPassword, oldPassword, confirmPassword);

                }
                break;
        }
    }

    private boolean validateFields() {

        if (!Validator.checkEmpty(resetPasswordLayoutBinding.edtOldPassword)) {
            hideKeyBoard(resetPasswordLayoutBinding.edtOldPassword);
            resetPasswordLayoutBinding.oldPasswordWrapper.setError(getResources().getString(R.string.validation_enterOldPassword));
            return false;
        } else {
            hideKeyBoard(resetPasswordLayoutBinding.edtOldPassword);
            resetPasswordLayoutBinding.oldPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(resetPasswordLayoutBinding.edtNewPassword)) {
            hideKeyBoard(resetPasswordLayoutBinding.edtNewPassword);
            resetPasswordLayoutBinding.newPasswordWrapper.setError(getResources().getString(R.string.validation_enterNewPassword));
            return false;
        } else {
            hideKeyBoard(resetPasswordLayoutBinding.edtNewPassword);
            resetPasswordLayoutBinding.newPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkLength(resetPasswordLayoutBinding.edtNewPassword)) {
            hideKeyBoard(resetPasswordLayoutBinding.edtNewPassword);
            resetPasswordLayoutBinding.newPasswordWrapper.setError(getString(R.string.validation_passwordLenght));
            return false;
        } else {
            hideKeyBoard(resetPasswordLayoutBinding.edtNewPassword);
            resetPasswordLayoutBinding.newPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkPassword(resetPasswordLayoutBinding.edtNewPassword)) {
            hideKeyBoard(resetPasswordLayoutBinding.edtNewPassword);
            resetPasswordLayoutBinding.newPasswordWrapper.setError(getString(R.string.validation_passwordStrong));
            return false;
        } else {
            hideKeyBoard(resetPasswordLayoutBinding.edtNewPassword);
            resetPasswordLayoutBinding.newPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(resetPasswordLayoutBinding.edtConfirmPassword)) {
            hideKeyBoard(resetPasswordLayoutBinding.edtConfirmPassword);
            resetPasswordLayoutBinding.confirmPasswordWrapper.setError(getResources().getString(R.string.validation_enterConfirmPassword));
            return false;
        } else {
            hideKeyBoard(resetPasswordLayoutBinding.edtConfirmPassword);
            resetPasswordLayoutBinding.confirmPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.comparePWD(resetPasswordLayoutBinding.edtNewPassword, resetPasswordLayoutBinding.edtConfirmPassword)) {
            hideKeyBoard(resetPasswordLayoutBinding.edtConfirmPassword);
            resetPasswordLayoutBinding.confirmPasswordWrapper.setError(getResources().getString(R.string.validation_passwordNotMatch));
            return false;
        } else {
            hideKeyBoard(resetPasswordLayoutBinding.edtConfirmPassword);
            resetPasswordLayoutBinding.confirmPasswordWrapper.setErrorEnabled(false);
        }


        return true;
    }

    public void callApiChangePassword(String newPassword, String oldPassword, String confirmPassword) {

        showDialog(getString(R.string.loading));
        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put(Const.new_password, newPassword);
            jsonObject.put(Const.old_password, oldPassword);
            jsonObject.put(Const.confirm_password, confirmPassword);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());
        disposable.add(apiService.changePassword(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UserObject>() {

                    @Override
                    public void onSuccess(UserObject userObject) {
                        hideDialog();
                        resetValue();
                        showSnackBar(resetPasswordLayoutBinding.mainChangePasswordLayout, userObject.getMessage());
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();

                        try {
                            HttpException error = (HttpException) e;
                            UserObject userObject = new Gson().fromJson(error.response().errorBody().string(), UserObject.class);
                            showSnackBar(resetPasswordLayoutBinding.mainChangePasswordLayout, userObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    public void resetValue() {
        resetPasswordLayoutBinding.edtNewPassword.setText("");
        resetPasswordLayoutBinding.edtOldPassword.setText("");
        resetPasswordLayoutBinding.edtConfirmPassword.setText("");
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
}


