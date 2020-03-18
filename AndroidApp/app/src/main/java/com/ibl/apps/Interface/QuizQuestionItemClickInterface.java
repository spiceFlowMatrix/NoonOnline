package com.ibl.apps.Interface;

import android.content.Context;

import com.ibl.apps.Model.QuizMainObject;

/**
 * Created by iblinfotech on 29/08/18.
 */

public interface QuizQuestionItemClickInterface {
    public void quizQuestionItemClick(Context ctx, String questionID, QuizMainObject.Images[] images, String questiontext, String explanation);
}
