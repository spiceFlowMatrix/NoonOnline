package com.ibl.apps.Adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import com.ibl.apps.Interface.QuizQuestionItemClickInterface;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.QuestionsummaryitemLayoutBinding;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * Created by iblinfotech on 20/09/18.
 */

public class SummaryQuestionViewAdapter extends RecyclerView.Adapter<SummaryQuestionViewAdapter.MyViewHolder> {

    Context context;
    List<QuizMainObject.Questions> quizQuestionsObjectList = new ArrayList<>();
    String quizID;
    QuizQuestionItemClickInterface quizQuestionItemClickInterface;


    public SummaryQuestionViewAdapter(Context context, List<QuizMainObject.Questions> quizQuestionsObjectList, String quizID, QuizQuestionItemClickInterface quizQuestionItemClickInterface) {
        this.context = context;
        this.quizQuestionsObjectList = quizQuestionsObjectList;
        this.quizID = quizID;
        this.quizQuestionItemClickInterface = quizQuestionItemClickInterface;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        QuestionsummaryitemLayoutBinding QuestionsummaryitemLayoutBinding;

        public MyViewHolder(QuestionsummaryitemLayoutBinding QuestionsummaryitemLayoutBinding) {
            super(QuestionsummaryitemLayoutBinding.getRoot());
            this.QuestionsummaryitemLayoutBinding = QuestionsummaryitemLayoutBinding;
        }
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        QuestionsummaryitemLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                R.layout.questionsummaryitem_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {

        QuizMainObject.Questions model = quizQuestionsObjectList.get(position);
        String questionExplainationText = "";

        String questionText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\">";
        questionText += model.getQuestiontext();
        questionText += "</body></html>";
        holder.QuestionsummaryitemLayoutBinding.txtQuestion.setText(questionText);
        holder.QuestionsummaryitemLayoutBinding.txtQuestion.getSettings().setDefaultFontSize((int) context.getResources().getDimension(R.dimen._7sdp));


//        if(quizQuestionsObjectList.get(position).getAnswers())
        holder.QuestionsummaryitemLayoutBinding.txtQuestion.setWebViewClient(new WebViewClient() {

            @Override
            public boolean shouldOverrideUrlLoading(WebView view, String url) {
                // Start PROGRESS DIALOG
                view.loadUrl(url);
                return true;
            }

            @Override
            public void onPageFinished(WebView view, String url) {
                holder.QuestionsummaryitemLayoutBinding.itemProgressbar.setVisibility(View.GONE);
                //HIDE PROGRESS DIALOG LOADING IT HAS FINISHED
            }

        });

        if (model.getImages().length != 0) {
            holder.QuestionsummaryitemLayoutBinding.layQuestionimag.setVisibility(View.VISIBLE);
            ArrayList<QuizMainObject.Images> listimage = new ArrayList<>();
            Collections.addAll(listimage, model.getImages());
            ImageviewAdapter imageviewAdapter = new ImageviewAdapter(context, listimage, 0);
            holder.QuestionsummaryitemLayoutBinding.questionImagePager.setAdapter(imageviewAdapter);
            //indicator.setViewPager(questionImagePager);
        } else {
            holder.QuestionsummaryitemLayoutBinding.layQuestionimag.setVisibility(View.GONE);
        }

        if (model.getAnswers() != null) {
            loadData(holder.QuestionsummaryitemLayoutBinding.rcVertical, model);
        }

        if (position == (getItemCount() - 1)) {
            holder.QuestionsummaryitemLayoutBinding.view1.setVisibility(View.GONE);
        } else {
            holder.QuestionsummaryitemLayoutBinding.view1.setVisibility(View.VISIBLE);
        }

        questionExplainationText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\">";
        questionExplainationText += model.getExplanation();
        questionExplainationText += "</body></html>";
        String finalQuestionExplainationText = questionExplainationText;
        String finalQuestionText = questionText;

        holder.QuestionsummaryitemLayoutBinding.summarylayid.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (!TextUtils.isEmpty(model.getExplanation())) {
                    quizQuestionItemClickInterface.quizQuestionItemClick(context, model.getId(), model.getImages(), finalQuestionText, finalQuestionExplainationText);
                }
            }
        });

        if (!TextUtils.isEmpty(model.getExplanation())) {
            holder.QuestionsummaryitemLayoutBinding.helpImage.setVisibility(View.VISIBLE);
        } else {
            holder.QuestionsummaryitemLayoutBinding.helpImage.setVisibility(View.GONE);
        }
    }

    @Override
    public int getItemCount() {
        return quizQuestionsObjectList.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }

    private void loadData(RecyclerView rcVertical, QuizMainObject.Questions model) {

        ArrayList<QuizMainObject.Answers> listProg = new ArrayList<>();
        if (model.getAnswers() != null) {
            Collections.addAll(listProg, model.getAnswers());
            rcVertical.setHasFixedSize(true);

            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(context);
            linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
            rcVertical.setLayoutManager(linearLayoutManager);

            SummaryAnswerViewAdapter adapter = new SummaryAnswerViewAdapter(context, listProg, quizID, model.getId());
            rcVertical.setAdapter(adapter);
        }
    }

}
