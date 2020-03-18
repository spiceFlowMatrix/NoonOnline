package com.ibl.apps.Adapter;

import android.content.Context;
import android.databinding.DataBindingUtil;
import android.graphics.Typeface;
import android.support.annotation.NonNull;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.google.gson.JsonObject;
import com.ibl.apps.Model.GetAllComment;
import com.ibl.apps.Model.TopicLike;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.CustomTypefaceSpan;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.Utils.TimeAgoClass;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.DiscussionsCommentItemLayoutBinding;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;

import static com.ibl.apps.Base.BaseActivity.apiService;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class DiscussionsCommentListAdapter extends RecyclerView.Adapter<DiscussionsCommentListAdapter.MyViewHolder> implements DroidListener {

    List<GetAllComment.Data> list;
    Context ctx;
    UserDetails userDetailsObject;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        DiscussionsCommentItemLayoutBinding discussionsCommentItemLayoutBinding;

        public MyViewHolder(DiscussionsCommentItemLayoutBinding discussionsCommentItemLayoutBinding) {
            super(discussionsCommentItemLayoutBinding.getRoot());
            this.discussionsCommentItemLayoutBinding = discussionsCommentItemLayoutBinding;
        }
    }

    public DiscussionsCommentListAdapter(Context ctx, List<GetAllComment.Data> list, UserDetails userDetailsObject, RecyclerView recyclerView) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        DiscussionsCommentItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.discussions_comment_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);

    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        GetAllComment.Data model = list.get(position);

        if (model.isLiked()) {
            holder.discussionsCommentItemLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up);
            holder.discussionsCommentItemLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
        } else if (model.isDisliked()) {
            holder.discussionsCommentItemLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down_sky);
            holder.discussionsCommentItemLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
        } else {
            holder.discussionsCommentItemLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
            holder.discussionsCommentItemLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
        }
        if (!TextUtils.isEmpty(model.getUser().getProfilepicurl())) {
            GlideApp.with(NoonApplication.getContext())
                    .load(model.getUser().getProfilepicurl())
                    .placeholder(R.drawable.profile)
                    .error(R.drawable.profile)
                    .into(holder.discussionsCommentItemLayoutBinding.commentprofileImage);
        }
        if (!TextUtils.isEmpty(model.getUser().getUsername())) {
            holder.discussionsCommentItemLayoutBinding.txtcommentUserName.setText(model.getUser().getUsername());
        } else if (!TextUtils.isEmpty(model.getUser().getFullName())) {
            holder.discussionsCommentItemLayoutBinding.txtcommentUserName.setText(model.getUser().getFullName());
        } else if (!TextUtils.isEmpty(model.getUser().getEmail())) {
            String[] separated = model.getUser().getEmail().split("@");
            holder.discussionsCommentItemLayoutBinding.txtcommentUserName.setText(separated[0]);
        } else {
            holder.discussionsCommentItemLayoutBinding.txtcommentUserName.setText(ctx.getResources().getString(R.string.user));
        }

        if (!TextUtils.isEmpty(model.getComment())) {
            holder.discussionsCommentItemLayoutBinding.txtcomment.setText(model.getComment());
            holder.discussionsCommentItemLayoutBinding.txtcomment.setVisibility(View.VISIBLE);
            holder.discussionsCommentItemLayoutBinding.rcVerticalLay.rcVertical.setVisibility(View.GONE);
        } else {
            if (model.getFiles() != null) {

                holder.discussionsCommentItemLayoutBinding.txtcomment.setVisibility(View.GONE);
                holder.discussionsCommentItemLayoutBinding.rcVerticalLay.rcVertical.setVisibility(View.VISIBLE);

                if (model.getFiles().size() != 0) {
                    List<GetAllComment.Files> discussionsFile = new ArrayList<>();
                    discussionsFile.addAll(model.getFiles());
                    holder.discussionsCommentItemLayoutBinding.rcVerticalLay.rcVertical.setLayoutManager(new LinearLayoutManager(ctx, LinearLayoutManager.HORIZONTAL, false));
                    DiscussionCommentFIlePreviewListAdapter discussionFIlePreviewListAdapter = new DiscussionCommentFIlePreviewListAdapter(ctx, discussionsFile, userDetailsObject);
                    holder.discussionsCommentItemLayoutBinding.rcVerticalLay.rcVertical.setAdapter(discussionFIlePreviewListAdapter);
                    discussionFIlePreviewListAdapter.notifyDataSetChanged();
                }
            }
        }

        if (!TextUtils.isEmpty(model.getCreatetime())) {
            TimeAgoClass timeAgo = new TimeAgoClass();
            String getDate = getDate(model.getCreatetime());
            String timeagotxt = timeAgo.covertTimeToText(getDate);
            holder.discussionsCommentItemLayoutBinding.txtcommenttime.setText(timeagotxt);
        }

        holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(model.getLikecount()));
        holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(model.getDislikecount()));
        holder.discussionsCommentItemLayoutBinding.thumbUpImage.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (model.isLiked()) {
                    JsonObject jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.commentid, model.getId());
                    jsonObject.addProperty(Const.like, false);
                    jsonObject.addProperty(Const.dislike, false);
                    callApiForlikeDislike(jsonObject, holder, model);
                } else {
                    JsonObject jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.commentid, model.getId());
                    jsonObject.addProperty(Const.like, true);
                    jsonObject.addProperty(Const.dislike, false);
                    callApiForlikeDislike(jsonObject, holder, model);
                }

            }

        });

        holder.discussionsCommentItemLayoutBinding.thumbDownImage.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (model.isDisliked()) {
                    JsonObject jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.commentid, model.getId());
                    jsonObject.addProperty(Const.like, false);
                    jsonObject.addProperty(Const.dislike, false);
                    callApiForlikeDislike(jsonObject, holder, model);
                } else {
                    JsonObject jsonObject = new JsonObject();
                    jsonObject.addProperty(Const.commentid, model.getId());
                    jsonObject.addProperty(Const.like, false);
                    jsonObject.addProperty(Const.dislike, true);
                    callApiForlikeDislike(jsonObject, holder, model);
                }
            }
        });
    }

    private void callApiForlikeDislike(JsonObject jsonObject, MyViewHolder holder, GetAllComment.Data model) {
        CompositeDisposable disposable = new CompositeDisposable();
        disposable.add(apiService.getDiscussionCommentLike(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<TopicLike>() {
                    @Override
                    public void onSuccess(TopicLike like) {
                        if (like.getResponse_code().equals("0")) {
                            if ((model.isLiked() && jsonObject.get(Const.like).getAsBoolean()) || ((model.isDisliked() && jsonObject.get(Const.dislike).getAsBoolean())))
                                return;
                            int likeCounts = model.getLikecount();
                            int dislikeCount = model.getDislikecount();
                            if (!jsonObject.get(Const.like).getAsBoolean() && !jsonObject.get(Const.dislike).getAsBoolean()) {
                                if (model.isDisliked()) {
                                    dislikeCount--;
                                    model.setDisliked(false);
                                    model.setDislikecount(dislikeCount);
                                    holder.discussionsCommentItemLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
                                } else if (model.isLiked()) {
                                    likeCounts--;
                                    model.setLiked(false);
                                    model.setLikecount(likeCounts);
                                    holder.discussionsCommentItemLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
                                }
                            } else if (jsonObject.get(Const.like).getAsBoolean()) {
                                likeCounts++;
                                if (model.isDisliked()) {
                                    dislikeCount--;
                                }
                                model.setLiked(true);
                                model.setDisliked(false);
                                model.setLikecount(likeCounts);
                                model.setDislikecount(dislikeCount);
                                holder.discussionsCommentItemLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up);
                                holder.discussionsCommentItemLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
                            } else {
                                dislikeCount++;
                                if (model.isLiked()) {
                                    likeCounts--;
                                }
                                model.setLiked(false);
                                model.setDisliked(true);
                                model.setLikecount(likeCounts);
                                model.setDislikecount(dislikeCount);
                                holder.discussionsCommentItemLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
                                holder.discussionsCommentItemLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down_sky);
                            }
                            holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
                            holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
                        }
//                        holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(likeCounts));
//                        holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
//                        if (like.getResponse_code().equals("0")) {
//                            int likeCount = 0;
//                            int dislikeCount = 0;
//                            if (jsonObject.get(Const.like).getAsBoolean()) {
//
//                                if (!model.isLiked()) {
//                                    likeCount = model.getLikecount() + 1;
//                                    holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(likeCount));
//                                    if (model.isDisliked()) {
//                                        dislikeCount = model.getDislikecount() - 1;
//                                        holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
//                                    } else if (!model.isLiked() && !model.isDisliked()) {
//                                        if (model.getDislikecount()!=0) {
//                                            dislikeCount = model.getDislikecount() - 1;
//                                            holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
//                                        }else {
//                                            holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(model.getDislikecount()));
//                                        }
//                                    }
//                                } else {
//                                    holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(model.getDislikecount()));
//                                    holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(model.getLikecount()));
//                                }
//
//                                holder.discussionsCommentItemLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up);
//                                holder.discussionsCommentItemLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down);
//                            } else {
//
//                                if (!model.isDisliked()) {
//                                    dislikeCount = model.getDislikecount() + 1;
//                                    holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(dislikeCount));
//                                    if (model.isLiked()) {
//                                        likeCount = model.getLikecount() - 1;
//                                        holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(likeCount));
//                                    } else if (!model.isLiked() && !model.isDisliked()) {
//                                        if (model.getLikecount() != 0) {
//                                            likeCount = model.getLikecount() - 1;
//                                            holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(likeCount));
//                                        } else {
//                                            holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(model.getLikecount()));
//                                        }
//                                    }
//                                } else {
//                                    holder.discussionsCommentItemLayoutBinding.txtDisLikeCount.setText(String.valueOf(model.getDislikecount()));
//                                    holder.discussionsCommentItemLayoutBinding.txtLikeCount.setText(String.valueOf(model.getLikecount()));
//                                }
//                                holder.discussionsCommentItemLayoutBinding.thumbDownImage.setImageResource(R.drawable.ic_thumb_down_sky);
//                                holder.discussionsCommentItemLayoutBinding.thumbUpImage.setImageResource(R.drawable.ic_thumb_up_gray);
//                            }
//                        }

                    }

                    @Override
                    public void onError(Throwable e) {


                    }
                }));
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
    public int getItemViewType(int position) {
        return position;
    }

    public static boolean isNetworkAvailable() {
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
            android.support.v7.app.AlertDialog.Builder builder = new android.support.v7.app.AlertDialog.Builder(activity);
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