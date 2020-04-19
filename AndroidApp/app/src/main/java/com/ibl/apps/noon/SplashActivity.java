package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.text.TextUtils;

import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.UserDatabaseRepository;
import com.ibl.apps.noon.databinding.SplashLayoutBinding;
import com.ibl.apps.util.PrefUtils;

import io.reactivex.disposables.CompositeDisposable;


public class SplashActivity extends BaseActivity {

    SplashLayoutBinding splashLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    @Override
    protected int getContentView() {
        return R.layout.splash_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        splashLayoutBinding = (SplashLayoutBinding) getBindObj();

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {

                String authid = PrefUtils.getAuthid(SplashActivity.this);
                if (!TextUtils.isEmpty(authid)) {

                    UserDatabaseRepository userDatabaseRepository = new UserDatabaseRepository();
                    AuthTokenObject authTokenObject = userDatabaseRepository.getAuthTokenData(authid);
                    if (authTokenObject != null) {

                        //Log.e(Const.LOG_NOON_TAG,"==AuthTokenObject=="+authTokenObject.toString());

                        String accessToken = "";
                        String idToken = "";

                        if (authTokenObject.getAccessToken() != null) {
                            accessToken = authTokenObject.getAccessToken();
                        }
                        if (authTokenObject.getIdToken() != null) {
                            idToken = authTokenObject.getIdToken();
                        }

                        if (!TextUtils.isEmpty(accessToken) && !TextUtils.isEmpty(idToken)) {
                            openActivity(MainDashBoardActivity.class);
                        } else {
                            openActivity(LoginActivity.class);
                        }
                    } else {
                        openActivity(LoginActivity.class);
                    }
                } else {
                    openActivity(LoginActivity.class);
                }
                finish();

            }
        }, 5000);
    }
}
