package com.ibl.apps.Adapter;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.FrameLayout;
import android.widget.LinearLayout;

import com.ibl.apps.Interface.QuizItemClickInterface;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.noon.R;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import io.github.kexanie.library.MathView;

/**
 * Created by iblinfotech on 20/09/18.
 */

public class QuestionViewAdapter extends PagerAdapter {

    Context context;
    List<QuizMainObject.Questions> quizQuestionsObjectList = new ArrayList<>();
    String quizID;
    QuizItemClickInterface quizItemClickInterface;

    public QuestionViewAdapter(Context context, List<QuizMainObject.Questions> quizQuestionsObjectList, String quizID, QuizItemClickInterface quizItemClickInterface) {
        this.context = context;
        this.quizQuestionsObjectList = quizQuestionsObjectList;
        this.quizID = quizID;
        this.quizItemClickInterface = quizItemClickInterface;
    }

    @Override
    public int getCount() {
        return quizQuestionsObjectList.size();
    }

    @Override
    public boolean isViewFromObject(View view, Object object) {
        return view == ((LinearLayout) object);
    }

    @NonNull
    @Override
    public Object instantiateItem(@NonNull ViewGroup container, int position) {
        LayoutInflater inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View itemView = inflater.inflate(R.layout.questionitem_layout, container, false);

        QuizMainObject.Questions model = quizQuestionsObjectList.get(position);

        MathView textView = (MathView) itemView.findViewById(R.id.txtQuestion);
        ViewPager questionImagePager = (ViewPager) itemView.findViewById(R.id.questionImagePager);
        /*CircleIndicator indicator = (CircleIndicator) itemView.findViewById(R.id.indicator);*/
        RecyclerView rcVertical = (RecyclerView) itemView.findViewById(R.id.rcVertical);
        FrameLayout layQuestionimag = (FrameLayout) itemView.findViewById(R.id.layQuestionimag);


        String questionText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\">";
        questionText += model.getQuestiontext();
        questionText += "</body></html>";
        textView.setText(questionText);
        textView.getSettings().setDefaultFontSize((int) context.getResources().getDimension(R.dimen._5sdp));

       /* String questionText = "<html><body dir=\"rtl\"; style=\"text-align:justify;\">";
        questionText += "<p> sp<sup>3</sup></p>";
        questionText += "</body></html>";
        textView.setText(questionText);*/

        //Log.e(Const.LOG_NOON_TAG, "===ID==" + model.getId());

        if (model.getImages().length != 0) {
            layQuestionimag.setVisibility(View.VISIBLE);
            ArrayList<QuizMainObject.Images> listimage = new ArrayList<>();
            Collections.addAll(listimage, model.getImages());
            ImageviewAdapter imageviewAdapter = new ImageviewAdapter(context, listimage, 0);
            questionImagePager.setAdapter(imageviewAdapter);
            //indicator.setViewPager(questionImagePager);

        } else {
            layQuestionimag.setVisibility(View.GONE);
        }

        if (model.getAnswers() != null) {
            loadData(rcVertical, model);
        }

        container.addView(itemView);
        return itemView;
    }

    private void loadData(RecyclerView rcVertical, QuizMainObject.Questions model) {

        ArrayList<QuizMainObject.Answers> listProg = new ArrayList<>();
        if (model.getAnswers() != null) {
            Collections.addAll(listProg, model.getAnswers());
            rcVertical.setHasFixedSize(true);
            rcVertical.setAdapter(new AnswerViewAdapter(context, listProg, quizID, model.getQuestiontypeid(), model.getId(), new AnswerViewAdapter.OnItemClickListener() {
                @Override
                public void onItemClick(String nextID, String answerID) {
                    quizItemClickInterface.quizItemClick(context, nextID, answerID, model.getQuestiontypeid());
                }
            }));
        }
    }

    @Override
    public void destroyItem(ViewGroup container, int position, @NonNull Object object) {
        container.removeView((LinearLayout) object);
    }
}
