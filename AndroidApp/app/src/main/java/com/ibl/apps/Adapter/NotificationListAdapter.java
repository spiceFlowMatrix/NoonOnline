package com.ibl.apps.Adapter;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.graphics.Typeface;
import androidx.annotation.NonNull;
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
import com.ibl.apps.Model.NotificationObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.util.TimeAgoClass;
import com.ibl.apps.noon.ChapterActivity;
import com.ibl.apps.noon.MainDashBoardActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.NotificationItemLayoutBinding;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class NotificationListAdapter extends RecyclerView.Adapter<NotificationListAdapter.MyViewHolder> implements DroidListener {

    List<NotificationObject.Data> list;
    Context ctx;
    UserDetails userDetailsObject;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        NotificationItemLayoutBinding notificationItemLayoutBinding;

        public MyViewHolder(NotificationItemLayoutBinding notificationItemLayoutBinding) {
            super(notificationItemLayoutBinding.getRoot());
            this.notificationItemLayoutBinding = notificationItemLayoutBinding;
        }
    }

    public NotificationListAdapter(Context ctx, List<NotificationObject.Data> list) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);

    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        NotificationItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.notification_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        NotificationObject.Data model = list.get(position);
        if (!TextUtils.isEmpty(model.getTag())) {

            String notificationText = '\u200f' + model.getTag() + '\u200f';
            holder.notificationItemLayoutBinding.txtnotificationTag.setText(notificationText);
        }
        if (!TextUtils.isEmpty(model.getDateCreated())) {
            TimeAgoClass timeAgo = new TimeAgoClass();
            String getDate = getDate(model.getDateCreated());
            String timeagotxt = timeAgo.covertTimeToText(getDate);
            holder.notificationItemLayoutBinding.txtnotificationTime.setText(timeagotxt);
        }

      /*  if (!TextUtils.isEmpty(model.getIsRead())) {
            String isRead = model.getIsRead();
            if (isRead.equals("true")) {
                holder.notificationItemLayoutBinding.txtIsRead.setVisibility(View.INVISIBLE);
            } else {
                holder.notificationItemLayoutBinding.txtIsRead.setVisibility(View.INVISIBLE);
            }
        } else {
            holder.notificationItemLayoutBinding.txtIsRead.setVisibility(View.INVISIBLE);
        }*/

        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //Toast.makeText(ctx, "====" + model.getType() + "====", Toast.LENGTH_LONG).show();

                //holder.notificationItemLayoutBinding.txtIsRead.setVisibility(View.INVISIBLE);
                list.get(position).setIsRead("true");
                notifyDataSetChanged();
                String notiType = model.getType();
                Intent intent = null;

                switch (notiType) {
                    case "1":
                        intent = new Intent(ctx, MainDashBoardActivity.class);
                        intent.putExtra(Const.isNotification, true);
                        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                        ctx.startActivity(intent);
                        break;
                    case "3":
                        if (model.getCourse().getId() != null) {
                            intent = new Intent(ctx, ChapterActivity.class);
                            intent.putExtra(Const.isNotification, true);
                            intent.putExtra(Const.GradeID, model.getCourse().getId());
                            intent.putExtra(Const.CourseName, model.getCourse().getName());
                            intent.putExtra(Const.ActivityFlag, "0");
                            intent.putExtra(Const.LessonID, model.getLessionId());
                            intent.putExtra(Const.QuizID, "0");
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                            ctx.startActivity(intent);
                        }
                        break;
                    case "4":
                        if (model.getDiscussion() != null) {
                            intent = new Intent(ctx, ChapterActivity.class);
                            intent.putExtra(Const.isDiscussions, true);
                            intent.putExtra(Const.isNotification, true);
                            intent.putExtra(Const.GradeID, model.getDiscussion().getCourseid());
                            intent.putExtra(Const.topicId, model.getDiscussion().getId());
                            intent.putExtra(Const.topicname, model.getDiscussion().getTitle());
                            intent.putExtra(Const.isprivate, model.getDiscussion().getIsprivate());
                            intent.putExtra(Const.CourseName, "");
                            intent.putExtra(Const.ActivityFlag, "0");
                            intent.putExtra(Const.LessonID, "");
                            intent.putExtra(Const.QuizID, "");
                            intent.putExtra(Const.profileurl, model.getUser().getProfilepicurl());
                            intent.putExtra(Const.userName, model.getUser().getUsername());
                            intent.putExtra(Const.createdtime, " ");
                            intent.putExtra(Const.iseditable, model.getDiscussion().isIseditable());
                            intent.putExtra(Const.likecount, model.getDiscussion().getLikecount());
                            intent.putExtra(Const.dislikecount, model.getDiscussion().getDislikecount());
                            intent.putExtra(Const.like, model.getDiscussion().isLiked());
                            intent.putExtra(Const.dislike, model.getDiscussion().isDisliked());
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                            ctx.startActivity(intent);
                        }
                        break;
                    case "6":
                        if (model.getDiscussion() != null) {
                            intent = new Intent(ctx, ChapterActivity.class);
                            intent.putExtra(Const.isDiscussions, true);
                            intent.putExtra(Const.isNotification, true);
                            intent.putExtra(Const.GradeID, model.getDiscussion().getCourseid());
                            intent.putExtra(Const.topicId, model.getDiscussion().getId());
                            intent.putExtra(Const.topicname, model.getDiscussion().getTitle());
                            intent.putExtra(Const.isprivate, model.getDiscussion().getIsprivate());
                            intent.putExtra(Const.CourseName, "");
                            intent.putExtra(Const.ActivityFlag, "0");
                            intent.putExtra(Const.LessonID, "");
                            intent.putExtra(Const.QuizID, "");
                            intent.putExtra(Const.profileurl, model.getUser().getProfilepicurl());
                            intent.putExtra(Const.userName, model.getUser().getUsername());
                            intent.putExtra(Const.createdtime, " ");
                            intent.putExtra(Const.iseditable, model.getDiscussion().isIseditable());
                            intent.putExtra(Const.likecount, model.getDiscussion().getLikecount());
                            intent.putExtra(Const.dislikecount, model.getDiscussion().getDislikecount());
                            boolean islike = model.getDiscussion().isLiked();
                            boolean isDislike = model.getDiscussion().isDisliked();
                            intent.putExtra(Const.like, model.getDiscussion().isLiked());
                            intent.putExtra(Const.dislike, model.getDiscussion().isDisliked());
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                            ctx.startActivity(intent);
                        }
                        break;
                    case "7":
                        if (model.getDiscussion() != null) {
                            intent = new Intent(ctx, ChapterActivity.class);
                            intent.putExtra(Const.isDiscussions, true);
                            intent.putExtra(Const.isNotification, true);
                            intent.putExtra(Const.GradeID, model.getDiscussion().getCourseid());
                            intent.putExtra(Const.topicId, model.getDiscussion().getId());
                            intent.putExtra(Const.topicname, model.getDiscussion().getTitle());
                            intent.putExtra(Const.isprivate, model.getDiscussion().getIsprivate());
                            intent.putExtra(Const.CourseName, "");
                            intent.putExtra(Const.ActivityFlag, "0");
                            intent.putExtra(Const.LessonID, "");
                            intent.putExtra(Const.QuizID, "");
                            intent.putExtra(Const.profileurl, model.getUser().getProfilepicurl());
                            intent.putExtra(Const.userName, model.getUser().getUsername());
                            intent.putExtra(Const.createdtime, " ");
                            intent.putExtra(Const.iseditable, model.getDiscussion().isIseditable());
                            intent.putExtra(Const.likecount, model.getDiscussion().getLikecount());
                            intent.putExtra(Const.dislikecount, model.getDiscussion().getDislikecount());
                            intent.putExtra(Const.like, model.getDiscussion().isLiked());
                            intent.putExtra(Const.dislike, model.getDiscussion().isDisliked());
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                            ctx.startActivity(intent);
                        }
                        break;
                    case "8":
                        if (model.getCourse().getId() != null) {
                            intent = new Intent(ctx, ChapterActivity.class);
                            intent.putExtra(Const.isNotification, true);
                            intent.putExtra(Const.GradeID, model.getCourse().getId());
                            intent.putExtra(Const.CourseName, model.getCourse().getName());
                            intent.putExtra(Const.ActivityFlag, "0");
                            intent.putExtra(Const.LessonID, "0");
                            intent.putExtra(Const.QuizID, "0");
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                            ctx.startActivity(intent);
                        }
                        break;
                    case "9":
                        if (model.getCourse().getId() != null) {
                            intent = new Intent(ctx, ChapterActivity.class);
                            intent.putExtra(Const.isNotification, true);
                            intent.putExtra(Const.GradeID, model.getCourse().getId());
                            intent.putExtra(Const.CourseName, model.getCourse().getName());
                            intent.putExtra(Const.ActivityFlag, "0");
                            intent.putExtra(Const.LessonID, "0");
                            intent.putExtra(Const.QuizID, model.getQuizId());
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                            ctx.startActivity(intent);
                        }
                        break;
                    case "11":
                        if (model.getDiscussion() != null) {
                            intent = new Intent(ctx, MainDashBoardActivity.class);
                            intent.putExtra(Const.isDiscussions, true);
                            intent.putExtra(Const.isNotification, true);
                            intent.putExtra(Const.GradeID, model.getDiscussion().getCourseid());
                            intent.putExtra(Const.topicId, model.getDiscussion().getId());
                            intent.putExtra(Const.topicname, model.getDiscussion().getTitle());
                            intent.putExtra(Const.isprivate, model.getDiscussion().getIsprivate());
                            intent.putExtra(Const.CourseName, "");
                            intent.putExtra(Const.ActivityFlag, "0");
                            intent.putExtra(Const.LessonID, "");
                            intent.putExtra(Const.QuizID, "");
                            intent.putExtra(Const.profileurl, model.getUser().getProfilepicurl());
                            intent.putExtra(Const.userName, model.getUser().getUsername());
                            intent.putExtra(Const.createdtime, " ");
                            intent.putExtra(Const.iseditable, model.getDiscussion().isIseditable());
                            intent.putExtra(Const.likecount, model.getDiscussion().getLikecount());
                            intent.putExtra(Const.dislikecount, model.getDiscussion().getDislikecount());
                            intent.putExtra(Const.like, model.getDiscussion().isLiked());
                            intent.putExtra(Const.dislike, model.getDiscussion().isDisliked());
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                            ctx.startActivity(intent);
                        }
                        break;
                    case "12":
                        if (model.getDiscussion() != null) {
                            intent = new Intent(ctx, MainDashBoardActivity.class);
                            intent.putExtra(Const.isDiscussions, true);
                            intent.putExtra(Const.isNotification, true);
                            intent.putExtra(Const.GradeID, model.getDiscussion().getCourseid());
                            intent.putExtra(Const.topicId, model.getDiscussion().getId());
                            intent.putExtra(Const.topicname, model.getDiscussion().getTitle());
                            intent.putExtra(Const.isprivate, model.getDiscussion().getIsprivate());
                            intent.putExtra(Const.CourseName, "");
                            intent.putExtra(Const.ActivityFlag, "0");
                            intent.putExtra(Const.LessonID, "");
                            intent.putExtra(Const.QuizID, "");
                            intent.putExtra(Const.profileurl, model.getUser().getProfilepicurl());
                            intent.putExtra(Const.userName, model.getUser().getUsername());
                            intent.putExtra(Const.createdtime, " ");
                            intent.putExtra(Const.iseditable, model.getDiscussion().isIseditable());
                            intent.putExtra(Const.likecount, model.getDiscussion().getLikecount());
                            intent.putExtra(Const.dislikecount, model.getDiscussion().getDislikecount());
                            intent.putExtra(Const.like, model.getDiscussion().isLiked());
                            intent.putExtra(Const.dislike, model.getDiscussion().isDisliked());
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TASK);
                            ctx.startActivity(intent);
                        }
                        break;

                }
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

    private String getDate(String ourDate) {
        try {
            SimpleDateFormat formatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss aaa", Locale.ENGLISH);
            formatter.setTimeZone(TimeZone.getTimeZone("UTC"));
            Date value = formatter.parse(ourDate);

            SimpleDateFormat dateFormatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH); //this format changeable
            dateFormatter.setTimeZone(TimeZone.getDefault());
            ourDate = dateFormatter.format(value);

            //Log.d("ourDate", ourDate);
        } catch (Exception e) {
            ourDate = "00-00-0000 00:00";
        }
        return ourDate;
    }
}