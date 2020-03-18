package com.ibl.apps.Adapter;

import android.content.Context;
import android.databinding.DataBindingUtil;
import android.os.AsyncTask;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.QuizAnswerSelect;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.AnswerItemLayoutBinding;

import java.util.ArrayList;
import java.util.Collections;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class AnswerViewAdapter extends RecyclerView.Adapter<AnswerViewAdapter.MyViewHolder> {

    ArrayList<QuizMainObject.Answers> list;
    Context ctx;
    OnItemClickListener onItemClickListener;
    String quizID, questionID, nextID;

    public interface OnItemClickListener {
        void onItemClick(String nextID, String answerID);
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        AnswerItemLayoutBinding answerItemLayoutBinding;

        public MyViewHolder(AnswerItemLayoutBinding answerItemLayoutBinding) {
            super(answerItemLayoutBinding.getRoot());
            this.answerItemLayoutBinding = answerItemLayoutBinding;
        }
    }

    public AnswerViewAdapter(Context ctx, ArrayList<QuizMainObject.Answers> list, String quizID, String questionID, String nextID, OnItemClickListener onItemClickListener) {
        this.list = list;
        this.ctx = ctx;
        this.quizID = quizID;
        this.questionID = questionID;
        this.nextID = nextID;
        this.onItemClickListener = onItemClickListener;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        AnswerItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                R.layout.answer_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        QuizMainObject.Answers model = list.get(position);

        String answerText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\">";
        answerText += model.getAnswer();
        answerText += "</body></html>";

        holder.answerItemLayoutBinding.txtAnswerText.setText(answerText);
        holder.answerItemLayoutBinding.txtAnswerText.getSettings().setDefaultFontSize((int) ctx.getResources().getDimension(R.dimen._5sdp));

        if (model.getImages().length != 0) {
            holder.answerItemLayoutBinding.layanswerimag.setVisibility(View.VISIBLE);
            ArrayList<QuizMainObject.Images> listimage = new ArrayList<>();
            Collections.addAll(listimage, model.getImages());
            ImageviewAdapter imageviewAdapter = new ImageviewAdapter(ctx, listimage, 0);
            holder.answerItemLayoutBinding.answerImagePager.setAdapter(imageviewAdapter);
            //holder.answerItemLayoutBinding.indicator.setViewPager(holder.answerItemLayoutBinding.answerImagePager);
        } else {
            holder.answerItemLayoutBinding.layanswerimag.setVisibility(View.GONE);
        }


        try {
            QuizAnswerSelect quizAnswerSelect = AppDatabase.getAppDatabase(ctx).quizAnswerDao().getItemAnswerSelect(nextID);
            if (quizAnswerSelect != null) {
                if (quizAnswerSelect.getSelectedAnswerId().equals(model.getId())) {
                    holder.answerItemLayoutBinding.mainitemLessonLayout.setCardBackgroundColor(ctx.getResources().getColor(R.color.colorPrimary));
                } else {
                    holder.answerItemLayoutBinding.mainitemLessonLayout.setCardBackgroundColor(ctx.getResources().getColor(R.color.colorWhite));
                }
            } else {
                holder.answerItemLayoutBinding.mainitemLessonLayout.setCardBackgroundColor(ctx.getResources().getColor(R.color.colorWhite));
            }
        } catch (Exception e) {
            e.printStackTrace();
        }


        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                new AsyncTask<Void, Void, String>() {

                    @Override
                    protected String doInBackground(Void... voids) {
                        try {

                            QuizAnswerSelect quizAnswerSelect = AppDatabase.getAppDatabase(ctx).quizAnswerDao().getItemAnswerSelect(nextID);
                            if (quizAnswerSelect != null) {
                                AppDatabase.getAppDatabase(ctx).quizAnswerDao().updateItemQuizAnswerSelect(model.getId(), nextID);
                            } else {
                                QuizAnswerSelect answerSelect = new QuizAnswerSelect();
                                answerSelect.setQuizID(quizID);
                                answerSelect.setQuestionID(questionID);
                                answerSelect.setSelectedAnswerId(model.getId());
                                answerSelect.setNextID(nextID);
                                answerSelect.setIscorrect(model.getIscorrect());
                                answerSelect.setIsStatus("0");
                                AppDatabase.getAppDatabase(ctx).quizAnswerDao().insertAll(answerSelect);
                            }
                        } catch (Exception e) {
                            e.printStackTrace();
                        }
                        return null;
                    }
                }.execute();

                notifyDataSetChanged();
                onItemClickListener.onItemClick(nextID, model.getId());
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
}