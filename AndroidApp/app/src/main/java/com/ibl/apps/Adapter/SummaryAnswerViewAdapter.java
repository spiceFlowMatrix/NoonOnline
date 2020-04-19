package com.ibl.apps.Adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.QuizAnswerSelect;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.SummaryAnswerItemLayoutBinding;

import java.util.ArrayList;
import java.util.Collections;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class SummaryAnswerViewAdapter extends RecyclerView.Adapter<SummaryAnswerViewAdapter.MyViewHolder> {

    ArrayList<QuizMainObject.Answers> list;
    Context ctx;
    String quizID, questionID;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        SummaryAnswerItemLayoutBinding summaryAnswerItemLayoutBinding;

        public MyViewHolder(SummaryAnswerItemLayoutBinding summaryAnswerItemLayoutBinding) {
            super(summaryAnswerItemLayoutBinding.getRoot());
            this.summaryAnswerItemLayoutBinding = summaryAnswerItemLayoutBinding;
        }
    }

    public SummaryAnswerViewAdapter(Context ctx, ArrayList<QuizMainObject.Answers> list, String quizID, String questionID) {
        this.list = list;
        this.ctx = ctx;
        this.quizID = quizID;
        this.questionID = questionID;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        SummaryAnswerItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                R.layout.summary_answer_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        QuizMainObject.Answers model = list.get(position);
        String answerText = "";

        holder.summaryAnswerItemLayoutBinding.txtAnswerText.getSettings().setDefaultFontSize((int) ctx.getResources().getDimension(R.dimen._5sdp));

        if (model.getImages().length != 0) {
            holder.summaryAnswerItemLayoutBinding.layanswerimag.setVisibility(View.VISIBLE);
            ArrayList<QuizMainObject.Images> listimage = new ArrayList<>();
            Collections.addAll(listimage, model.getImages());
            ImageviewAdapter imageviewAdapter = new ImageviewAdapter(ctx, listimage, 0);
            holder.summaryAnswerItemLayoutBinding.answerImagePager.setAdapter(imageviewAdapter);

        } else {
            holder.summaryAnswerItemLayoutBinding.layanswerimag.setVisibility(View.GONE);
        }

        if (model.getIscorrect().equals("true")) {
            answerText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\";> <font color='green'>";
            answerText += model.getAnswer();
            answerText += "</font></body></html>";
            holder.summaryAnswerItemLayoutBinding.txtAnswerText.setText(answerText);
        } else {
            answerText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\">";
            answerText += model.getAnswer();
            answerText += "</body></html>";
            holder.summaryAnswerItemLayoutBinding.txtAnswerText.setText(answerText);
        }

        try {
            QuizDatabaseRepository quizDatabaseRepository = new QuizDatabaseRepository();
            QuizAnswerSelect quizAnswerSelect = quizDatabaseRepository.getItemAnswerSelect(questionID);
            if (quizAnswerSelect != null) {
                if (quizAnswerSelect.getSelectedAnswerId().equals(model.getId())) {
                    if (!model.getIscorrect().equals("true")) {
                        answerText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\";> <font color='red'>";
                        answerText += model.getAnswer();
                        answerText += "</font></body></html>";
                        holder.summaryAnswerItemLayoutBinding.txtAnswerText.setText(answerText);
                    }
                } else {
                    if (model.getIscorrect().equals("true")) {
                        answerText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\";> <font color='green'>";
                        answerText += model.getAnswer();
                        answerText += "</font></body></html>";
                        holder.summaryAnswerItemLayoutBinding.txtAnswerText.setText(answerText);
                    }
                }
            } else {
                if (model.getIscorrect().equals("true")) {
                    answerText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\";> <font color='green'>";
                    answerText += model.getAnswer();
                    answerText += "</font></body></html>";
                    holder.summaryAnswerItemLayoutBinding.txtAnswerText.setText(answerText);
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
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