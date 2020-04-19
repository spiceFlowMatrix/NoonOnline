package com.ibl.apps.Adapter;

import android.app.Dialog;
import android.content.ActivityNotFoundException;
import android.content.Context;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import android.media.MediaPlayer;
import android.net.Uri;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.Toast;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Model.feedback.FillesData;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.noon.FilesFeedbackActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.ReportProblemActivity;
import com.ibl.apps.noon.databinding.AssignmentfilesItemLayoutBinding;
import com.ibl.apps.noon.databinding.ImageUploadListBinding;

import java.util.ArrayList;

public class ImageUploadAdapter extends RecyclerView.Adapter<ImageUploadAdapter.MyViewHolder> implements DroidListener {
    public static boolean isNetworkConnected = false;
    private final DroidNet mDroidNet;
    private ArrayList<FillesData> images;
    private int flagid;
    private Context context;


    public ImageUploadAdapter(ArrayList<FillesData> images, int flagid) {
        this.images = images;
        this.flagid = flagid;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    public static boolean isNetworkAvailable(Context context) {
        return isNetworkConnected;
    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int i) {
        ImageUploadListBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()), R.layout.image_upload_list, parent, false);
        context = parent.getContext();
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        FillesData model = images.get(position);
        int fileType = model.getFiletype();


        holder.binding.imgRemove.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (flagid == 1 || flagid == 7) {
                    images.remove(position);
                    ReportProblemActivity.fileIdList.remove(position);
                    notifyDataSetChanged();
                } else if (flagid == 2 || flagid == 3 || flagid == 4 || flagid == 5 || flagid == 6) {
                    images.remove(position);
                    FilesFeedbackActivity.fileIdList.remove(position);
                    notifyDataSetChanged();
                }
            }
        });

        if (fileType == 1) {
            holder.binding.uploadedImg.setVisibility(View.VISIBLE);
            holder.binding.videoViewLay.setVisibility(View.GONE);
            GlideApp.with(context)
                    .load(model.getUri())
                    .error(R.drawable.border_image)
                    .into(holder.binding.uploadedImg);
        } else if (fileType == 2) {
            holder.binding.videoViewLay.setVisibility(View.VISIBLE);
            holder.binding.uploadedImg.setVisibility(View.GONE);
            holder.binding.videoView.setVideoURI(model.getUri());
            holder.binding.videoView.setOnPreparedListener(new MediaPlayer.OnPreparedListener() {
                @Override
                public void onPrepared(MediaPlayer mediaPlayer) {
                    mediaPlayer.setLooping(true);
                    mediaPlayer.setVolume(0f, 0f);
                }
            });
//            holder.binding.videoView.requestFocus();
            holder.binding.videoView.start();
        }

        holder.binding.getRoot().setOnClickListener(view -> {
            if (fileType == 1) {
                final Dialog dialog = new Dialog(context, android.R.style.Theme_Translucent_NoTitleBar);
                dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                //dialogimage.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                dialog.setCancelable(true);
                AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(context), R.layout.assignmentfiles_item_layout, null, false);
                dialog.setContentView(assignmentfilesItemLayoutBinding.getRoot());
                dialog.show();

                GlideApp.with(context)
                        .load(model.getUri())
                        .error(R.drawable.ic_no_image_found)
                        .into(assignmentfilesItemLayoutBinding.quizImage);

            } else if (fileType == 2) {
                Intent newIntent = new Intent(Intent.ACTION_VIEW);
                String path = String.valueOf(model.getUri());
                newIntent.setDataAndType(Uri.parse(path), "video/*");
                newIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);

                try {
                    context.startActivity(newIntent);
                } catch (ActivityNotFoundException e) {
                    Toast.makeText(context, "No handler for this type of file.", Toast.LENGTH_LONG).show();
                }
            }
        });
    }

    @Override
    public int getItemCount() {
        return images.size();
    }

    @Override
    public void onInternetConnectivityChanged(boolean isConnected) {
        isNetworkConnected = isConnected;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        ImageUploadListBinding binding;

        public MyViewHolder(ImageUploadListBinding imageUploadListBinding) {
            super(imageUploadListBinding.getRoot());
            this.binding = imageUploadListBinding;
        }
    }
}
