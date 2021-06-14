package com.ibl.apps.Adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import android.graphics.Typeface;
import androidx.annotation.NonNull;
import androidx.core.content.res.ResourcesCompat;
import androidx.recyclerview.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Interface.ViewFiles;
import com.ibl.apps.Model.assignment.SubmissionFiles;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.AssignmentFileItemLayoutBinding;

import java.util.ArrayList;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class AssignmentAddCommentFileListAdapter extends RecyclerView.Adapter<AssignmentAddCommentFileListAdapter.MyViewHolder> implements DroidListener {

    ArrayList<SubmissionFiles> list;
    Context ctx;
    UserDetails userDetailsObject;
    private ViewFiles viewFilesinterface;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        AssignmentFileItemLayoutBinding assignmentFileItemLayoutBinding;

        public MyViewHolder(AssignmentFileItemLayoutBinding assignmentFileItemLayoutBinding) {
            super(assignmentFileItemLayoutBinding.getRoot());
            this.assignmentFileItemLayoutBinding = assignmentFileItemLayoutBinding;
        }
    }

    AssignmentAddCommentFileListAdapter(Context ctx, ArrayList<SubmissionFiles> list, UserDetails userDetailsObject, ViewFiles viewFilesinterface) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        this.viewFilesinterface = viewFilesinterface;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        AssignmentFileItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.assignment_file_item_layout, parent, false);

        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        SubmissionFiles submissionFiles = list.get(position);
        int imageDrawable = R.drawable.ic_file;
        switch (list.get(position).getFiles().getFiletypeid()) {
            case 1:
                imageDrawable = R.drawable.ic_pdf_icon;
                break;
            case 2:
                imageDrawable = R.drawable.ic_video;
                break;
            case 3:
                imageDrawable = R.drawable.ic_photo;
                break;
            case 6:
                imageDrawable = R.drawable.ic_excel;
                break;
            case 7:
                imageDrawable = R.drawable.ic_docs;
                break;

        }
        holder.assignmentFileItemLayoutBinding.assignmentImgIcon.setImageResource(imageDrawable);
        holder.assignmentFileItemLayoutBinding.docLay.setOnClickListener(view -> viewFilesinterface.openDialogView(submissionFiles, list.get(position).getFiles().getFiletypeid()));
    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
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
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), (dialog, which) -> {
                        // Yes button clicked, do something
                        dialog.dismiss();
                    });

            builder.show();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}