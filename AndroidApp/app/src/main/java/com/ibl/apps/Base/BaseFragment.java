package com.ibl.apps.Base;

import android.accounts.NetworkErrorException;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Typeface;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.Toast;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Utils.CustomTypefaceSpan;
import com.ibl.apps.noon.R;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.net.SocketTimeoutException;
import java.util.AbstractCollection;
import java.util.ArrayList;

import retrofit2.HttpException;

import static android.content.Context.INPUT_METHOD_SERVICE;


public abstract class BaseFragment extends Fragment implements DroidListener {

    protected ArrayList<Integer> userIdArray =new ArrayList<>();
    private ProgressDialog mProgressDialog;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(false);
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public void onViewCreated(View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        setUp(view);
    }

    protected abstract void setUp(View view);

    @Override
    public void onDestroyView() {
        /* Unbind all views before destroy activity. */
        super.onDestroyView();
        if (mProgressDialog != null && mProgressDialog.isShowing()) {
            mProgressDialog.dismiss();
        }
    }

    protected void showBackArrow(String title) {
        ActionBar supportActionBar = ((AppCompatActivity) getActivity()).getSupportActionBar();
        if (supportActionBar != null) {
            supportActionBar.setDisplayHomeAsUpEnabled(true);
            supportActionBar.setDisplayShowHomeEnabled(true);
            supportActionBar.setTitle(title);
        }
    }

    protected void setToolbar(Toolbar toolbar) {
        ((AppCompatActivity) getActivity()).setSupportActionBar(toolbar);
    }

    protected void showDialog(String message) {
        if (mProgressDialog == null) {
            mProgressDialog = new ProgressDialog(getActivity());
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

    protected boolean isShowing() {
        if (mProgressDialog == null) {
            return false;
        }

        return mProgressDialog.isShowing();
    }

    protected void showToast(String message) {
        Toast.makeText(getActivity(), message, Toast.LENGTH_SHORT).show();
    }

    protected void showLog(String Tag, String message) {
        Log.e(Tag, message);
    }

    protected void openActivity(Class destination) {
        startActivity(new Intent(getContext(), destination));
        overridePendingTransitionEnter();
    }

   /* @Override
    public void finish() {
        super.getActivity().finish();
        overridePendingTransitionExit();
    }*/

    protected void openFragment(int container, Fragment fragment) {
        FragmentManager fragmentManager = getChildFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        fragmentTransaction.replace(container, fragment);
        fragmentTransaction.commit();
    }

    public void openFragmentWithBackStack(int container, Fragment fragment) {
        FragmentManager fragmentManager = getChildFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        fragmentTransaction.addToBackStack(fragment.getClass().getSimpleName());
        fragmentTransaction.add(container, fragment);
        fragmentTransaction.commit();
    }

    public void Visible(View ivBack) {
        ivBack.setVisibility(View.VISIBLE);
    }

    public void Gone(View ivBack) {
        ivBack.setVisibility(View.GONE);
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
            android.support.v7.app.AlertDialog.Builder builder = new android.support.v7.app.AlertDialog.Builder(activity);
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
       /* Snackbar snackbar = Snackbar.make(layout, message, Snackbar.LENGTH_LONG);
        snackbar.show();*/
        showToast(message);
    }

    public void hideKeyBoard(View view) {
        InputMethodManager imm = (InputMethodManager) getActivity().getSystemService(INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(view.getWindowToken(), 0);
    }

    public void showError(Throwable e) {
        String message = "";
        try {
            if (isNetworkAvailable(getActivity())) {
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

                String finalMessage = message;
                getActivity().runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(getActivity(), finalMessage, Toast.LENGTH_LONG).show();
                    }
                });
                //Toast.makeText(getActivity(), message, Toast.LENGTH_LONG).show();
            } else {
                //showNetworkAlert(getActivity());
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
        getActivity().overridePendingTransition(R.anim.slide_from_right, R.anim.slide_to_left);
    }

    /**
     * Overrides the pending Activity transition by performing the "Exit" animation.
     */
    protected void overridePendingTransitionExit() {
        getActivity().overridePendingTransition(R.anim.slide_from_left, R.anim.slide_to_right);
    }

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }

//    public boolean onBackPressed() {
//        return false;
//    }




}
