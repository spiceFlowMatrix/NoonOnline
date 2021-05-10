package com.ibl.apps.Interface;

import android.content.Context;

/**
 * Created by iblinfotech on 29/08/18.
 */

public interface CourseInnerItemInterface {
    public void courseInnerItem(Context ctx, String fileType, String fileTypeName,
                                String videoUri, int position, String lessionID,
                                int currantProgress, String quizID,
                                int playpushflag,
                                String chapterid,
                                String fileid,
                                String LessonName);
}
