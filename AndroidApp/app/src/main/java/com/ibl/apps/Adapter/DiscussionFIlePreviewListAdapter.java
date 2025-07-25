package com.ibl.apps.Adapter;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.graphics.Typeface;
import android.net.Uri;
import android.os.AsyncTask;
import androidx.core.content.res.ResourcesCompat;
import androidx.recyclerview.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.crashlytics.android.Crashlytics;
import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.google.gson.Gson;
import com.ibl.apps.Interface.EncryptDecryptAsyncResponse;
import com.ibl.apps.Interface.ViewDiscussionsFiles;
import com.ibl.apps.Model.DiscussionsDetails;
import com.ibl.apps.Model.EncryptDecryptObject;
import com.ibl.apps.Model.SignedUrlObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.util.VideoEncryptDecrypt.EncrypterDecryptAlgo;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.AssignmentfilesItemLayoutBinding;
import com.ibl.apps.noon.databinding.DiscussionsFilePreviewItemLayoutBinding;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.util.List;

import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class DiscussionFIlePreviewListAdapter extends RecyclerView.Adapter<DiscussionFIlePreviewListAdapter.MyViewHolder> implements DroidListener {

    List<DiscussionsDetails.Files> list;
    Context ctx;
    UserDetails userDetailsObject;
    boolean enableDisabl;
    private ViewDiscussionsFiles viewDiscussionsFiles;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;
    byte[] iv;
    AlgorithmParameterSpec paramSpec;
    SecretKey key;
    SecretKey keyFromKeydata;
    byte[] keyData;
    CompositeDisposable disposable = new CompositeDisposable();

    public class MyViewHolder extends RecyclerView.ViewHolder {
        DiscussionsFilePreviewItemLayoutBinding discussionsFilePreviewItemLayoutBinding;

        public MyViewHolder(DiscussionsFilePreviewItemLayoutBinding discussionsFilePreviewItemLayoutBinding) {
            super(discussionsFilePreviewItemLayoutBinding.getRoot());
            this.discussionsFilePreviewItemLayoutBinding = discussionsFilePreviewItemLayoutBinding;
        }
    }

    public DiscussionFIlePreviewListAdapter(Context ctx, List<DiscussionsDetails.Files> list, UserDetails userDetailsObject, boolean enableDisabl, ViewDiscussionsFiles viewDiscussionsFiles) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        this.enableDisabl = enableDisabl;
        this.viewDiscussionsFiles = viewDiscussionsFiles;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        DiscussionsFilePreviewItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.discussions_file_preview_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {

        DiscussionsDetails.Files model = list.get(position);
        holder.discussionsFilePreviewItemLayoutBinding.txtDiscussionsFileName.setText(model.getFileName());

       /* if (enableDisabl) {
            holder.discussionsFilePreviewItemLayoutBinding.imgDiscussionsClose.setVisibility(View.VISIBLE);
        } else {
            holder.discussionsFilePreviewItemLayoutBinding.imgDiscussionsClose.setVisibility(View.GONE);
        }*/
        holder.discussionsFilePreviewItemLayoutBinding.imgDiscussionsClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                list.remove(position);
                notifyDataSetChanged();
            }
        });

//        holder.discussionsFilePreviewItemLayoutBinding.pdfopenLay.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view) {
//                viewDiscussionsFiles.openDialogView(model, model.getFileTypeId());
//            }
//        });

        holder.itemView.setOnClickListener(view -> {
            String fileURL = model.getSignedUrl();
            Intent i = new Intent(Intent.ACTION_VIEW, Uri.parse(fileURL));
            i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            ctx.startActivity(i);
        });
    }


    @Override
    public int getItemCount() {
        return list.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
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

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }

    public static void showNetworkAlert(Context activity) {
        try {
            SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.validation_Connect_internet));
            androidx.appcompat.app.AlertDialog.Builder builder = new androidx.appcompat.app.AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
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
}