package com.ibl.apps.Adapter;

import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import androidx.databinding.DataBindingUtil;
import android.graphics.Typeface;
import androidx.core.content.res.ResourcesCompat;
import androidx.recyclerview.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;

import com.downloader.Error;
import com.downloader.OnDownloadListener;
import com.downloader.PRDownloader;
import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.github.barteksc.pdfviewer.listener.OnLoadCompleteListener;
import com.github.barteksc.pdfviewer.listener.OnPageChangeListener;
import com.github.barteksc.pdfviewer.listener.OnRenderListener;
import com.ibl.apps.Model.assignment.AssignmentData;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.AssignmentFileItemLayoutBinding;
import com.ibl.apps.noon.databinding.DialogViewerItemLayoutBinding;

import java.io.File;
import java.util.ArrayList;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class AssignmentFileListCommentAdapter extends RecyclerView.Adapter<AssignmentFileListCommentAdapter.MyViewHolder> implements DroidListener {

    ArrayList<AssignmentData> list;
    Context ctx;
    UserDetails userDetailsObject;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        AssignmentFileItemLayoutBinding assignmentFileItemLayoutBinding;

        public MyViewHolder(AssignmentFileItemLayoutBinding assignmentFileItemLayoutBinding) {
            super(assignmentFileItemLayoutBinding.getRoot());
            this.assignmentFileItemLayoutBinding = assignmentFileItemLayoutBinding;
        }
    }

    public AssignmentFileListCommentAdapter(Context ctx, ArrayList<AssignmentData> list, UserDetails userDetailsObject) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        AssignmentFileItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.assignment_file_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
//        holder.assignmentFileItemLayoutBinding.myAssignmentText.setText(list.get(position).getFiles().getName());

        int imageDrawable = R.drawable.ic_file;
        AssignmentData dataRestResponse = list.get(position);
//        switch (dataRestResponse.getSubmissionfiles().get(position).getFiles().get(position).) {
//            case "1":
//                imageDrawable = R.drawable.ic_pdf_icon;
//                break;
//            case "2":
//                imageDrawable = R.drawable.ic_video;
//                break;
//            case "3":
//                imageDrawable = R.drawable.ic_photo;
//                break;
//            case "6":
//                imageDrawable = R.drawable.ic_excel;
//                break;
//            case "7":
//                imageDrawable = R.drawable.ic_docs;
//                break;
//
//        }
        holder.assignmentFileItemLayoutBinding.assignmentImgIcon.setImageResource(imageDrawable);
        holder.assignmentFileItemLayoutBinding.docLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Log.e("Dialog", "-----");
                final Dialog dialog = new Dialog(ctx, android.R.style.Theme_Translucent_NoTitleBar);
                dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                dialog.setCancelable(true);
                DialogViewerItemLayoutBinding dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(ctx), R.layout.dialog_viewer_item_layout, null, false);
                dialog.setContentView(dialogViewerItemLayoutBinding.getRoot());
                dialog.show();
                String yourFilePath = ctx.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath();

                try {
                    PRDownloader.download("", yourFilePath, Const.dir_fileName + Const.PDFextension).build().start(new OnDownloadListener() {
                        @Override
                        public void onDownloadComplete() {
                            Log.e("Dialog", "-----");
                            File file = new File(ctx.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath() + File.separator + Const.dir_fileName + Const.PDFextension);

                            dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fromFile(file)
                                    .enableSwipe(true)
                                    .enableDoubletap(true)
                                    .defaultPage(0)
                                    .onLoad(new OnLoadCompleteListener() {
                                        @Override
                                        public void loadComplete(int i) {
                                            dialogViewerItemLayoutBinding.pdfViewLayout.progressDialogLay.placeholder.setVisibility(View.GONE);
                                        }
                                    })
                                    .onPageChange(new OnPageChangeListener() {
                                        @Override
                                        public void onPageChanged(int page, int pageCount) {
                                            dialogViewerItemLayoutBinding.pdfViewLayout.txtPageCount.setText(ctx.getString(R.string.page) + " " + (page + 1) + " " + ctx.getString(R.string.of) + "  " + pageCount);
                                            int countper = (int) ((page + 1) * 100 / pageCount);
                                            dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setProgress(countper);
                                           /* if (currProgress[0] < countper) {
                                                new CourseItemFragment.ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid).execute();
                                                currProgress[0] = countper;
                                                newcurrantProgress = countper;
                                            }

                                            TotalPDFpage = pageCount;
                                            SelectPDFpage = page;*/
                                        }
                                    })
                                    .enableAnnotationRendering(true)
                                    .swipeHorizontal(true)
                                    .onRender(new OnRenderListener() {
                                        @Override
                                        public void onInitiallyRendered(int i) {
                                            dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fitToWidth(dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.getCurrentPage());
                                        }
                                    })
                                    .enableAntialiasing(true)
                                    .password(null)
                                    .load();
                        }

                        @Override
                        public void onError(Error error) {
                            //Log.e(Const.LOG_NOON_TAG, "0000" + "NO Space ----" + error.toString());
                        }
                    });
                } catch (Exception e) {
                    e.printStackTrace();

                }
//
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(CourseName);
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(LessonName);
                dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View view) {
                        dialog.dismiss();
                    }
                });
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