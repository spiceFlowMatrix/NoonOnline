package com.ibl.apps.Adapter;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.graphics.Typeface;
import androidx.core.content.res.ResourcesCompat;
import androidx.recyclerview.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Model.DiscssionsAllTopics;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.noon.GeneralDiscussionsDetailActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.DiscussionsItemLayoutBinding;

import java.util.List;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class GeneralDiscussionsListAdapter extends RecyclerView.Adapter<GeneralDiscussionsListAdapter.MyViewHolder> implements DroidListener {

    List<DiscssionsAllTopics.Data> list;
    Context ctx;
    UserDetails userDetailsObject;
    String GradeId, CourseName, ActivityFlag, LessonID, QuizID;
    String addtionalLibrary, addtionalAssignment, addtionalDiscussions;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;


    public class MyViewHolder extends RecyclerView.ViewHolder {
        DiscussionsItemLayoutBinding discussionsItemLayoutBinding;

        public MyViewHolder(DiscussionsItemLayoutBinding discussionsItemLayoutBinding) {
            super(discussionsItemLayoutBinding.getRoot());
            this.discussionsItemLayoutBinding = discussionsItemLayoutBinding;
        }
    }

    public GeneralDiscussionsListAdapter(Context ctx, List<DiscssionsAllTopics.Data> list, UserDetails userDetailsObject, String gradeId, String courseName, String activityFlag, String lessonID, String quizID, String addtionalLibrary, String addtionalAssignment, String addtionalDiscussions) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        this.GradeId = gradeId;
        this.CourseName = courseName;
        this.ActivityFlag = activityFlag;
        this.LessonID = lessonID;
        this.QuizID = quizID;
        this.addtionalLibrary = addtionalLibrary;
        this.addtionalAssignment = addtionalAssignment;
        this.addtionalDiscussions = addtionalDiscussions;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        DiscussionsItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.discussions_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        DiscssionsAllTopics.Data model = list.get(position);
        if (!TextUtils.isEmpty(model.getTitle())) {
            holder.discussionsItemLayoutBinding.txttopicName.setText(model.getTitle());
        }

      /*  if (!TextUtils.isEmpty(model.getDescription())) {
            holder.discussionsItemLayoutBinding.txttopicDescription.setText(model.getDescription());
        }
*/
        if (!TextUtils.isEmpty(model.getComments())) {

            int commentCount = Integer.parseInt(model.getComments());
            if (commentCount == 0) {
                holder.discussionsItemLayoutBinding.txtcommentCount.setVisibility(View.VISIBLE);
            } else if (commentCount == 1) {
                holder.discussionsItemLayoutBinding.txtcommentCount.setText(model.getComments() + " " + ctx.getString(R.string.comment));
            } else {
                holder.discussionsItemLayoutBinding.txtcommentCount.setText(model.getComments() + " " + ctx.getString(R.string.comments));
            }

        }

        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent i = new Intent(ctx, GeneralDiscussionsDetailActivity.class);
                i.putExtra(Const.topicId, model.getId());
                i.putExtra(Const.topicname, model.getTitle());
                i.putExtra(Const.isprivate, model.getIsPrivate());
                i.putExtra(Const.GradeID, GradeId);
                i.putExtra(Const.CourseName, CourseName);
                i.putExtra(Const.ActivityFlag, ActivityFlag);
                i.putExtra(Const.LessonID, LessonID);
                i.putExtra(Const.QuizID, QuizID);
                i.putExtra(Const.AddtionalLibrary, addtionalLibrary);
                i.putExtra(Const.AddtionalAssignment, addtionalAssignment);
                i.putExtra(Const.AddtionalDiscussions, addtionalDiscussions);

                ctx.startActivity(i);
                ((Activity) ctx).finish();
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