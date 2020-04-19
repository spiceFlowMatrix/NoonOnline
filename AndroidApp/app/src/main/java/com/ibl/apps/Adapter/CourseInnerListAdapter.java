package com.ibl.apps.Adapter;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.graphics.PorterDuff;
import android.graphics.Typeface;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import androidx.core.content.ContextCompat;
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
import com.google.gson.Gson;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDatabaseRepository;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.noon.ChapterActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CourseInnerLayoutBinding;

import java.util.List;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class CourseInnerListAdapter extends RecyclerView.Adapter<CourseInnerListAdapter.MyViewHolder> implements DroidListener {
    List<CourseObject.Courses> list;
    Context ctx;
    UserDetails userDetailsObject;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;
    CourseDatabaseRepository courseDatabaseRepository;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        CourseInnerLayoutBinding courseInnerLayoutBinding;

        public MyViewHolder(CourseInnerLayoutBinding itemhistorylayoutBinding) {
            super(itemhistorylayoutBinding.getRoot());
            this.courseInnerLayoutBinding = itemhistorylayoutBinding;
        }
    }

    public CourseInnerListAdapter(Context ctx, List<CourseObject.Courses> list, UserDetails userDetailsObject) {
        courseDatabaseRepository = new CourseDatabaseRepository();
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        CourseInnerLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.course_inner_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        CourseObject.Courses model = list.get(position);

        if (!TextUtils.isEmpty(model.getName())) {
            holder.courseInnerLayoutBinding.txtgradeLanguageName.setText(model.getName());
        }

        if (!TextUtils.isEmpty(model.getDescription())) {
            holder.courseInnerLayoutBinding.txtgradeLanguageDesc.setText(model.getDescription());
        }

        holder.courseInnerLayoutBinding.progressBarLayout.progressBar.getProgressDrawable().setColorFilter(ContextCompat.getColor(ctx, R.color.colorProgress), PorterDuff.Mode.SRC_IN);
        holder.courseInnerLayoutBinding.progressBarLayout.progressBar.setProgress(model.getProgressVal());

        if (isNetworkAvailable(ctx)) {

            GlideApp.with(ctx)
                    .load(model.getImage())
                    .placeholder(R.drawable.ic_no_image_found)
                    .error(R.drawable.ic_no_image_found)
                    .dontAnimate()
                    .into(holder.courseInnerLayoutBinding.mainCourseImage);

            holder.courseInnerLayoutBinding.mainImageCourse.setVisibility(View.GONE);
            holder.courseInnerLayoutBinding.mainInnerCoursLAy.setClickable(true);

        } else {

            GlideApp.with(ctx)
                    .asBitmap()
                    .load(model.getCourseImage())
                    .placeholder(R.drawable.ic_no_image_found)
                    .error(R.drawable.ic_no_image_found)
                    .into(holder.courseInnerLayoutBinding.mainCourseImage);

            new AsyncTask<Void, Void, String>() {
                @Override
                protected String doInBackground(Void... voids) {

                    try {
                        CoursePriviewObject coursePriviewObject;
                        String userId = userDetailsObject.getId();
                        coursePriviewObject = courseDatabaseRepository.getAllCourseDetailsById(model.getId(), userId);

                        if (coursePriviewObject != null) {
                            ((Activity) ctx).runOnUiThread(new Runnable() {
                                public void run() {
                                    holder.courseInnerLayoutBinding.mainImageCourse.setVisibility(View.GONE);
                                    holder.courseInnerLayoutBinding.mainInnerCoursLAy.setClickable(true);
                                }
                            });
                        } else {
                            ((Activity) ctx).runOnUiThread(new Runnable() {
                                public void run() {
                                    holder.courseInnerLayoutBinding.mainImageCourse.setVisibility(View.VISIBLE);
                                    holder.courseInnerLayoutBinding.mainInnerCoursLAy.setClickable(false);
                                }
                            });
                        }
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                    return null;
                }
            }.execute();

        }

        holder.courseInnerLayoutBinding.mainInnerCoursLAy.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                Gson gson = new Gson();
                String Addtionalservice = gson.toJson(model.getAddtionalservice());

                Intent i = new Intent(ctx, ChapterActivity.class);
                i.putExtra(Const.GradeID, model.getId());
                i.putExtra(Const.CourseName, model.getName());
                i.putExtra(Const.ActivityFlag, "0");
                i.putExtra(Const.LessonID, "0");
                i.putExtra(Const.QuizID, "0");
                i.putExtra(Const.Addtionalservice, Addtionalservice);

                ctx.startActivity(i);
            }
        });

        holder.courseInnerLayoutBinding.mainImageCourse.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showNetworkAlert(ctx);
            }
        });
    }


    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
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

    public static Boolean isNetwork(Context context) {
        ConnectivityManager connectivityManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        return activeNetworkInfo != null && activeNetworkInfo.isConnectedOrConnecting();
    }

    public static Boolean isWifiConnected(Context context) {
        if (isNetwork(context)) {
            ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
            return (cm.getActiveNetworkInfo().getType() == ConnectivityManager.TYPE_WIFI);
        }
        return false;
    }

    public static Boolean isEthernetConnected(Context context) {
        if (isNetwork(context)) {
            ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
            return (cm.getActiveNetworkInfo().getType() == ConnectivityManager.TYPE_ETHERNET);
        }
        return false;
    }

    public static Boolean isMobileDataConnected(Context context) {
        if (isNetwork(context)) {
            ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
            return (cm.getActiveNetworkInfo().getType() == ConnectivityManager.TYPE_MOBILE);
        }
        return false;
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