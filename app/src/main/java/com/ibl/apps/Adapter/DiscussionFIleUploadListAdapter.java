package com.ibl.apps.Adapter;

import android.content.Context;
import android.content.DialogInterface;
import androidx.databinding.DataBindingUtil;
import android.graphics.Typeface;
import androidx.core.content.res.ResourcesCompat;
import androidx.recyclerview.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.DiscussionsFileUploadItemLayoutBinding;

import java.util.List;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class DiscussionFIleUploadListAdapter extends RecyclerView.Adapter<DiscussionFIleUploadListAdapter.MyViewHolder> implements DroidListener {

    List<String> list;
    Context ctx;
    UserDetails userDetailsObject;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        DiscussionsFileUploadItemLayoutBinding discussionsFileUploadItemLayoutBinding;

        public MyViewHolder(DiscussionsFileUploadItemLayoutBinding discussionsFileUploadItemLayoutBinding) {
            super(discussionsFileUploadItemLayoutBinding.getRoot());
            this.discussionsFileUploadItemLayoutBinding = discussionsFileUploadItemLayoutBinding;
        }
    }

    public DiscussionFIleUploadListAdapter(Context ctx, List<String> list, UserDetails userDetailsObject) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        DiscussionsFileUploadItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.discussions_file_upload_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        holder.discussionsFileUploadItemLayoutBinding.txtDiscussionsFileName.setText(list.get(position).substring(list.get(position).lastIndexOf("/") + 1));
        holder.discussionsFileUploadItemLayoutBinding.imgDiscussionsClose.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                list.remove(position);
                notifyDataSetChanged();
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

    public static boolean isNetworkAvailable(Context context) {
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