package com.ibl.apps.Adapter;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.graphics.Typeface;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.support.annotation.NonNull;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Model.GetAllComment;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.CustomTypefaceSpan;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.DiscussionsFilePreviewItemLayoutBinding;

import java.util.List;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class DiscussionCommentFIlePreviewListAdapter extends RecyclerView.Adapter<DiscussionCommentFIlePreviewListAdapter.MyViewHolder> implements DroidListener {

    List<GetAllComment.Files> list;
    Context ctx;
    UserDetails userDetailsObject;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;


    public class MyViewHolder extends RecyclerView.ViewHolder {
        DiscussionsFilePreviewItemLayoutBinding discussionsFilePreviewItemLayoutBinding;

        public MyViewHolder(DiscussionsFilePreviewItemLayoutBinding discussionsFilePreviewItemLayoutBinding) {
            super(discussionsFilePreviewItemLayoutBinding.getRoot());
            this.discussionsFilePreviewItemLayoutBinding = discussionsFilePreviewItemLayoutBinding;
        }
    }

    DiscussionCommentFIlePreviewListAdapter(Context ctx, List<GetAllComment.Files> list, UserDetails userDetailsObject) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);

    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        DiscussionsFilePreviewItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.discussions_file_preview_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {

        GetAllComment.Files model = list.get(position);
        holder.discussionsFilePreviewItemLayoutBinding.txtDiscussionsFileName.setText(model.getFileName());
        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String fileURL = model.getSignedUrl();
                Intent i = new Intent(Intent.ACTION_VIEW, Uri.parse(fileURL));
                i.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                ctx.startActivity(i);
            }
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

    public static boolean isNetworkAvailable() {
        return isNetworkConnected;
    }

    @Override
    public void onInternetConnectivityChanged(boolean isConnected) {
        isNetworkConnected = isConnected;
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
            android.support.v7.app.AlertDialog.Builder builder = new android.support.v7.app.AlertDialog.Builder(activity);
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