package com.ibl.apps.Model;

import com.ibl.apps.Adapter.CourseItemInnerListAdapter;

/**
 * Created by iblinfotech on 07/12/18.
 */

public class DownloadQueueObject {

    private String fileid;

    private String lessonID;

    CourseItemInnerListAdapter.MyViewHolder holder;

    CoursePriviewObject.Lessonfiles lessonsModel;

    CoursePriviewObject.Lessons lemodel;


    public String getFileid() {
        return fileid;
    }

    public void setFileid(String fileid) {
        this.fileid = fileid;
    }

    public String getLessonID() {
        return lessonID;
    }

    public void setLessonID(String lessonID) {
        this.lessonID = lessonID;
    }

    public CourseItemInnerListAdapter.MyViewHolder getHolder() {
        return holder;
    }

    public void setHolder(CourseItemInnerListAdapter.MyViewHolder holder) {
        this.holder = holder;
    }

    public CoursePriviewObject.Lessonfiles getLessonsModel() {
        return lessonsModel;
    }

    public void setLessonsModel(CoursePriviewObject.Lessonfiles lessonsModel) {
        this.lessonsModel = lessonsModel;
    }

    public CoursePriviewObject.Lessons getLemodel() {
        return lemodel;
    }

    public void setLemodel(CoursePriviewObject.Lessons lemodel) {
        this.lemodel = lemodel;
    }

    @Override
    public String toString() {
        return "DownloadQueueObject{" +
                "fileid='" + fileid + '\'' +
                ", lessonID='" + lessonID + '\'' +
                ", holder=" + holder +
                ", lessonsModel=" + lessonsModel +
                ", lemodel=" + lemodel +
                '}';
    }
}
