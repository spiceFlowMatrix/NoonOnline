package com.ibl.apps.Adapter;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.graphics.Typeface;
import android.support.annotation.NonNull;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Filter;
import android.widget.Filterable;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Model.DiscssionsAllTopics;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.CustomTypefaceSpan;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.Utils.TimeAgoClass;
import com.ibl.apps.noon.DiscussionsDetailActivity;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.DiscussionsItemLayoutBinding;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class DiscussionsListAdapter extends RecyclerView.Adapter<DiscussionsListAdapter.MyViewHolder> implements DroidListener, Filterable {

    List<DiscssionsAllTopics.Data> list;
    List<DiscssionsAllTopics.Data> searchlist;
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

    public DiscussionsListAdapter(Context ctx, List<DiscssionsAllTopics.Data> list, UserDetails userDetailsObject, String gradeId, String courseName, String activityFlag, String lessonID, String quizID, String addtionalLibrary, String addtionalAssignment, String addtionalDiscussions) {
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
        this.searchlist = list;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        DiscussionsItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.discussions_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        DiscssionsAllTopics.Data model = list.get(position);
        if (!TextUtils.isEmpty(model.getTitle())) {
            holder.discussionsItemLayoutBinding.txttopicName.setText(model.getTitle());
        }
//        if (!TextUtils.isEmpty(model.getDescription())) {
////            holder.discussionsItemLayoutBinding.txttopicDescription.setText(model.getDescription());
////        }

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

        if (!TextUtils.isEmpty(model.getCreateddate())) {
            TimeAgoClass timeAgo = new TimeAgoClass();
            String getDate = getDate(model.getCreateddate());
            String timeagotxt = timeAgo.covertTimeToText(getDate);
            holder.discussionsItemLayoutBinding.txtTime.setText(timeagotxt);
        }

        if (model.getUser().getProfilepicurl() != null) {
            if (!TextUtils.isEmpty(model.getUser().getProfilepicurl())) {
                GlideApp.with(NoonApplication.getContext())
                        .load(model.getUser().getProfilepicurl())
                        .placeholder(R.drawable.profile)
                        .error(R.drawable.profile)
                        .into(holder.discussionsItemLayoutBinding.commentprofileImage);
            }
        }


        if (!TextUtils.isEmpty(model.getUser().getUsername())) {
            holder.discussionsItemLayoutBinding.txtUserName.setText(model.getUser().getUsername());
        } else if (!TextUtils.isEmpty(model.getUser().getFullName())) {
            holder.discussionsItemLayoutBinding.txtUserName.setText(model.getUser().getFullName());
        } else if (!TextUtils.isEmpty(model.getUser().getEmail())) {
            String[] separated = model.getUser().getEmail().split("@");
            holder.discussionsItemLayoutBinding.txtUserName.setText(separated[0]);
        } else {
            holder.discussionsItemLayoutBinding.txtUserName.setText(ctx.getResources().getString(R.string.user));
        }

        holder.itemView.setOnClickListener(view -> {
            if (isNetworkAvailable(NoonApplication.getContext())) {
                Intent i = new Intent(ctx, DiscussionsDetailActivity.class);
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
                i.putExtra(Const.profileurl, model.getUser().getProfilepicurl());
                i.putExtra(Const.userName, model.getUser().getUsername());
                i.putExtra(Const.createdtime, holder.discussionsItemLayoutBinding.txtTime.getText().toString().trim());
                i.putExtra(Const.iseditable, model.isIseditable());
                i.putExtra(Const.likecount, model.getLikecount());
                i.putExtra(Const.dislikecount, model.getDislikecount());
                i.putExtra(Const.liked, model.isLiked());
                i.putExtra(Const.disliked, model.isDisliked());

                ctx.startActivity(i);
                ((Activity) ctx).finish();
            } else {
                showNetworkAlert(ctx);
            }
        });
    }

    private String getDate(String ourDate) {
        try {
            SimpleDateFormat formatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss aaa", Locale.ENGLISH);
            formatter.setTimeZone(TimeZone.getTimeZone("UTC"));
            Date value = formatter.parse(ourDate);

            SimpleDateFormat dateFormatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH); //this format changeable
            dateFormatter.setTimeZone(TimeZone.getDefault());
            ourDate = dateFormatter.format(value);

        } catch (Exception e) {
            ourDate = "00-00-0000 00:00";
        }
        return ourDate;
    }


    @Override
    public int getItemCount() {
        return list.size();
    }

    @Override
    public Filter getFilter() {
        return new Filter() {
            @Override
            protected FilterResults performFiltering(CharSequence charSequence) {
                String charString = charSequence.toString();
                if (charString.isEmpty()) {
                    list = searchlist;
                } else {
                    ArrayList<DiscssionsAllTopics.Data> filteredList = new ArrayList<>();
                    for (DiscssionsAllTopics.Data row : searchlist) {
                        // name match condition. this might differ depending on your requirement
                        // here we are looking for name or phone number match
                        if (row.getTitle().toLowerCase().contains(charString.toLowerCase())) {
                            filteredList.add(row);
                        }
                    }

                    list = filteredList;
                }

                FilterResults filterResults = new FilterResults();
                filterResults.values = list;
                return filterResults;
            }

            @Override
            protected void publishResults(CharSequence charSequence, FilterResults filterResults) {
                list = (ArrayList<DiscssionsAllTopics.Data>) filterResults.values;

                // refresh the list with filtered data
                notifyDataSetChanged();
            }
        };
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

    public void setSearchlist(List<DiscssionsAllTopics.Data> searchlist) {
        this.list = searchlist;
        notifyDataSetChanged();
    }

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
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
}