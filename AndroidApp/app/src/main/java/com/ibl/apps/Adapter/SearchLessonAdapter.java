package com.ibl.apps.Adapter;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.support.v7.widget.RecyclerView;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Filter;
import android.widget.Filterable;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.Model.SearchObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.noon.ChapterActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CourseInnerItemLayoutBinding;

import java.io.File;
import java.util.ArrayList;

import static android.view.View.GONE;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class SearchLessonAdapter extends RecyclerView.Adapter<SearchLessonAdapter.MyViewHolder> implements Filterable, DroidListener {

    ArrayList<SearchObject.Lessons> listProg;
    ArrayList<SearchObject.Lessons> contactListFiltered;
    Context ctx;
    String userId;
    String userIdWithoutSlash;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;


    public class MyViewHolder extends RecyclerView.ViewHolder {
        CourseInnerItemLayoutBinding courseInnerItemLayoutBinding;

        public MyViewHolder(CourseInnerItemLayoutBinding courseInnerItemLayoutBinding) {
            super(courseInnerItemLayoutBinding.getRoot());
            this.courseInnerItemLayoutBinding = courseInnerItemLayoutBinding;
        }
    }

    public SearchLessonAdapter(Context ctx, ArrayList<SearchObject.Lessons> listProg, String userId) {
        this.listProg = listProg;
        this.ctx = ctx;
        this.contactListFiltered = listProg;
        this.userId = userId + "/";
        this.userIdWithoutSlash = userId;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        CourseInnerItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                R.layout.course_inner_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        SearchObject.Lessons model = contactListFiltered.get(position);

        holder.courseInnerItemLayoutBinding.progressBarLayout.progressBar.setVisibility(View.GONE);
        holder.courseInnerItemLayoutBinding.tvProgress.setVisibility(View.GONE);

        if (!TextUtils.isEmpty(model.getLessonfileid())) {

            if (!model.getLessonfileid().equals("0")) {
                holder.courseInnerItemLayoutBinding.txtFileName.setText(model.getName());

                if (!TextUtils.isEmpty(model.getFiletypename())) {
                    if (model.getFiletypename().equals(Const.PDFfileType)) {
                        holder.courseInnerItemLayoutBinding.imageFileIcon.setImageResource(R.drawable.ic_pdf);
                    } else if (model.getFiletypename().equals(Const.VideofileType)) {
                        holder.courseInnerItemLayoutBinding.imageFileIcon.setImageResource(R.drawable.ic_video);
                    } else if (model.getFiletypename().equals(Const.ImagefileType)) {
                        holder.courseInnerItemLayoutBinding.imageFileIcon.setImageResource(R.drawable.ic_photo);
                    }
                }

                File directory = new File(Const.destPath + userId);
                if (!directory.exists()) {
                    directory.mkdirs();
                }
                String str = model.getLessonfileid() + "_" + model.getLessonfilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + model.getFiletypename() + Const.extension;
                File file = new File(Const.destPath + userId, str);
                if (file.exists()) {
                    holder.courseInnerItemLayoutBinding.imgPlayContent.setVisibility(View.VISIBLE);
                    holder.courseInnerItemLayoutBinding.imgDeleteContent.setVisibility(View.VISIBLE);
                    holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(GONE);
                } else {
                    holder.courseInnerItemLayoutBinding.imgPlayContent.setVisibility(GONE);
                    holder.courseInnerItemLayoutBinding.imgDeleteContent.setVisibility(GONE);
                    holder.courseInnerItemLayoutBinding.imgdownloadContent.setVisibility(View.VISIBLE);

                    if (isNetworkAvailable(ctx)) {
                        holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                    } else {
                        holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
                    }
                }
            } else {
                holder.courseInnerItemLayoutBinding.imageFileIcon.setImageResource(R.drawable.ic_quiz);
                holder.courseInnerItemLayoutBinding.txtFileName.setText(model.getName());
                holder.courseInnerItemLayoutBinding.txtfileType.setText("Quiz");
                holder.courseInnerItemLayoutBinding.imgdownloadContentLayout.setVisibility(GONE);

                QuizMainObject quizMainObject = AppDatabase.getAppDatabase(ctx).quizAnswerDao().getquizData(userIdWithoutSlash, model.getId());
                if (quizMainObject != null) {
                    holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                } else {
                    if (isNetworkAvailable(ctx)) {
                        holder.courseInnerItemLayoutBinding.disableLay.setVisibility(GONE);
                    } else {
                        holder.courseInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
                    }
                }
            }
        }

        holder.courseInnerItemLayoutBinding.disableLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showNetworkAlert(ctx);
            }
        });

        holder.courseInnerItemLayoutBinding.courseInnerCardview.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent i = new Intent(ctx, ChapterActivity.class);
                i.putExtra(Const.GradeID, model.getCourseid());
                i.putExtra(Const.CourseName, model.getCoursename());
                i.putExtra(Const.ActivityFlag, "1");
                i.putExtra(Const.LessonID, model.getId());
                i.putExtra(Const.QuizID, model.getLessonquizid());
                ctx.startActivity(i);
            }
        });
    }

    @Override
    public int getItemCount() {
        return contactListFiltered.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }

    @Override
    public Filter getFilter() {
        return new Filter() {
            @Override
            protected FilterResults performFiltering(CharSequence charSequence) {
                String charString = charSequence.toString();
                if (charString.isEmpty()) {
                    contactListFiltered = listProg;
                } else {

                    ArrayList<SearchObject.Lessons> filteredList = new ArrayList<>();
                    for (SearchObject.Lessons row : listProg) {
                        if (row.getName().toLowerCase().contains(charString.toLowerCase())) {
                            filteredList.add(row);
                        }
                    }

                    contactListFiltered = filteredList;
                }

                FilterResults filterResults = new FilterResults();
                filterResults.values = contactListFiltered;
                return filterResults;
            }

            @Override
            protected void publishResults(CharSequence charSequence, FilterResults filterResults) {
                contactListFiltered = (ArrayList<SearchObject.Lessons>) filterResults.values;
                notifyDataSetChanged();
            }
        };
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
            android.support.v7.app.AlertDialog.Builder builder = new android.support.v7.app.AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(activity.getResources().getString(R.string.validation_Connect_internet))
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