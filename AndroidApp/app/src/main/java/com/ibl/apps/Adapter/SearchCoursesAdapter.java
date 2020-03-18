package com.ibl.apps.Adapter;

import android.app.Activity;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.graphics.Typeface;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Filter;
import android.widget.Filterable;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.SearchObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.CustomTypefaceSpan;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.noon.ChapterActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.SearchCoursesLayoutBinding;

import java.util.ArrayList;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class SearchCoursesAdapter extends RecyclerView.Adapter<SearchCoursesAdapter.MyViewHolder> implements Filterable, DroidListener {

    ArrayList<SearchObject.Courses> listProg;
    ArrayList<SearchObject.Courses> contactListFiltered;
    Context ctx;
    String userId;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        SearchCoursesLayoutBinding searchCoursesLayoutBinding;

        public MyViewHolder(SearchCoursesLayoutBinding searchCoursesLayoutBinding) {
            super(searchCoursesLayoutBinding.getRoot());
            this.searchCoursesLayoutBinding = searchCoursesLayoutBinding;
        }
    }

    public SearchCoursesAdapter(Context ctx, ArrayList<SearchObject.Courses> listProg, String userId) {
        this.listProg = listProg;
        this.ctx = ctx;
        this.contactListFiltered = listProg;
        this.userId = userId;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        SearchCoursesLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                R.layout.search_courses_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        SearchObject.Courses model = contactListFiltered.get(position);

        if (isNetworkAvailable(ctx)) {
            GlideApp.with(ctx)
                    .load(model.getImage())
                    .error(R.drawable.ic_no_image_found)
                    .placeholder(R.drawable.ic_no_image_found)
                    .into(holder.searchCoursesLayoutBinding.img);
            holder.searchCoursesLayoutBinding.progressBarsearchCourse.progressBar.setProgress(50);

        } else {
            GlideApp.with(ctx)
                    .asBitmap()
                    .load(model.getCourseImage())
                    .error(R.drawable.ic_no_image_found)
                    .placeholder(R.drawable.ic_no_image_found)
                    .into(holder.searchCoursesLayoutBinding.img);
            holder.searchCoursesLayoutBinding.progressBarsearchCourse.progressBar.setProgress(model.getProgressVal());

            new AsyncTask<Void, Void, String>() {
                @Override
                protected String doInBackground(Void... voids) {

                    try {
                        CoursePriviewObject coursePriviewObject;
                        coursePriviewObject = AppDatabase.getAppDatabase(ctx).courseDetailsDao().getAllCourseDetails(model.getId(), userId);

                        if (coursePriviewObject != null) {
                            ((Activity) ctx).runOnUiThread(new Runnable() {
                                public void run() {
                                    holder.searchCoursesLayoutBinding.disableserachCourseLay.setVisibility(View.GONE);
                                }
                            });
                        } else {
                            ((Activity) ctx).runOnUiThread(new Runnable() {
                                public void run() {
                                    holder.searchCoursesLayoutBinding.disableserachCourseLay.setVisibility(View.VISIBLE);
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

        String gradeAppendString = "";
        if (model.getGradeDetails() != null) {
            if (model.getGradeDetails().length != 0) {
                for (int i = 0; i < model.getGradeDetails().length; i++) {
                    if (gradeAppendString.length() == 0) {
                        gradeAppendString = model.getGradeDetails()[i].getName();
                    } else {
                        gradeAppendString = gradeAppendString + "," + model.getGradeDetails()[i].getName();
                    }
                }
            }
        }
        holder.searchCoursesLayoutBinding.txtGrade.setText(gradeAppendString);
        holder.searchCoursesLayoutBinding.txtTitle.setText(model.getName());
        holder.searchCoursesLayoutBinding.myserachCourse.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent i = new Intent(ctx, ChapterActivity.class);
                i.putExtra(Const.GradeID, model.getId());
                i.putExtra(Const.CourseName, model.getName());
                i.putExtra(Const.ActivityFlag, "0");
                ctx.startActivity(i);
            }
        });
        holder.searchCoursesLayoutBinding.disableserachCourseLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showNetworkAlert(ctx);
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

                /*try {
                    String charString = charSequence.toString();
                    if (charString.isEmpty()) {
                        contactListFiltered = listProg;
                    } else {
                        ArrayList<SearchObject.Courses> filteredList = new ArrayList<>();
                        for (SearchObject.Courses row : listProg) {
                            try {
                                // name match condition. this might differ depending on your requirement
                                // here we are looking for name or phone number match
                                if (row.getName().toLowerCase().contains(charString.toLowerCase())) {
                                    filteredList.add(row);
                                }


                                if (!SearchActivity.bygrade.equals("0")) {
                                    if (row.getGradeDetails() != null) {
                                        if (row.getGradeDetails().length != 0) {
                                            for (int i = 0; i < row.getGradeDetails().length; i++) {
                                                if (row.getGradeDetails()[i].getName().equals(SearchActivity.bygrade)) {
                                                    Log.e(Const.LOG_NOON_TAG, "===222===" + SearchActivity.bygrade);
                                                    filteredList.add(row);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                contactListFiltered = filteredList;
                            } catch (Exception e) {
                                e.printStackTrace();
                            }
                        }
                    }

                    FilterResults filterResults = new FilterResults();
                    filterResults.values = contactListFiltered;
                    return filterResults;

                } catch (Exception e) {
                    e.printStackTrace();
                }*/

                return null;

            }

            @Override
            protected void publishResults(CharSequence charSequence, FilterResults filterResults) {
                contactListFiltered = (ArrayList<SearchObject.Courses>) filterResults.values;
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