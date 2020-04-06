package com.ibl.apps.Adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import android.os.AsyncTask;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.ibl.apps.Interface.CourseHideResponse;
import com.ibl.apps.Interface.CourseInnerItemInterface;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.GradeProgress;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.WrapContentLinearLayoutManager;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CourseItemLayoutBinding;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class CourseItemListAdapter extends RecyclerView.Adapter<CourseItemListAdapter.MyViewHolder> {

    ArrayList<CoursePriviewObject.Chapters> list;
    CourseInnerItemInterface courseInnerItemInterface;
    CourseHideResponse courseHideResponse;
    boolean isPlayFirstItem = true;
    Context ctx;
    String GradeId;
    UserDetails userDetailsObject;
    String userId = "0";
    String activityFlag = "";
    String lessonID = "";
    String QuizID = "";
    Boolean isNotification = false;
    private String courseName;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        public CourseItemLayoutBinding courseItemLayoutBinding;

        public MyViewHolder(CourseItemLayoutBinding itemhistorylayoutBinding) {
            super(itemhistorylayoutBinding.getRoot());
            this.courseItemLayoutBinding = itemhistorylayoutBinding;
            courseItemLayoutBinding.executePendingBindings();
        }
    }

    public CourseItemListAdapter(Context ctx, ArrayList<CoursePriviewObject.Chapters> list, CourseInnerItemInterface courseInnerItemInterface, String GradeId, UserDetails userDetailsObject, String activityFlag, String lessonID, String QuizID, Boolean isNotification, CourseHideResponse courseHideResponse, String courseName) {
        this.list = list;
        this.ctx = ctx;
        this.courseInnerItemInterface = courseInnerItemInterface;
        this.courseHideResponse = courseHideResponse;
        this.GradeId = GradeId;
        this.userDetailsObject = userDetailsObject;
        this.activityFlag = activityFlag;
        this.lessonID = lessonID;
        this.QuizID = QuizID;
        this.isNotification = isNotification;
        this.courseName = courseName;

        if (userDetailsObject != null) {
            this.userId = userDetailsObject.getId();
        }


    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        CourseItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.course_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        CoursePriviewObject.Chapters model = list.get(position);
        holder.courseItemLayoutBinding.txtChapterName.setText(model.getName());
        ArrayList<CoursePriviewObject.Assignment> assignments = model.getAssignments();
        if (model.getLessons() != null) {

            loadData(holder, model, isLastItemViewed(position), isPlayFirstItem, assignments, GradeId);

        }
        isPlayFirstItem = false;
    }

    private void loadData(MyViewHolder holder, CoursePriviewObject.Chapters model, boolean isisLastItemViewed, boolean isPlayFirstItem, ArrayList<CoursePriviewObject.Assignment> assignments, String gradeId) {
        ArrayList<CoursePriviewObject.Lessons> listProg = new ArrayList<>();


        if (model.getLessons() != null) {
            for (int i = 0; i < model.getLessons().length; i++) {
                if (model.getLessons() != null && model.getLessons()[i].getLessonfiles() != null && model.getLessons()[i].getLessonfiles().length != 0) {
                    listProg.add(model.getLessons()[i]);
                } else if (model.getLessons()[i].getNumquestions()!=null) {
                    listProg.add(model.getLessons()[i]);
                }
            }
            //Collections.addAll(listProg, model.getLessons());

            Collections.sort(listProg, new Comparator() {
                @Override
                public int compare(Object o1, Object o2) {
                    CoursePriviewObject.Lessons p1 = (CoursePriviewObject.Lessons) o1;
                    CoursePriviewObject.Lessons p2 = (CoursePriviewObject.Lessons) o2;
                    //return Integer.valueOf(p1.getItemorder().compareToIgnoreCase(p2.getItemorder()));

                    Integer p1order = Integer.parseInt(p1.getItemorder());
                    Integer p2order = Integer.parseInt(p2.getItemorder());

                    return p1order.compareTo(p2order);
                }
            });


            if (!listProg.isEmpty()) {
                if (assignments != null && !assignments.isEmpty()) {
                    for (int i = 0; i < assignments.size(); i++) {
                        CoursePriviewObject.Lessons temp = new CoursePriviewObject.Lessons();
                        temp.setLessonfiles(null);
                        temp.setAssignment(assignments.get(i));
                        temp.setNumquestions(null);
                        listProg.add(listProg.size() - 1, temp);
                    }
                }
            }

            /*for (int i = 0; i < listProg.size(); i++) {
                ArrayList<CoursePriviewObject.Lessonfiles> array = new ArrayList<>();
                if (listProg.get(i).getLessonfiles() != null && listProg.get(i).getLessonfiles().length != 0) {
                    for (int j = 0; j < listProg.get(i).getLessonfiles().length; j++) {
                        if (listProg.get(i).getLessonfiles()[j].getFiles().getFiletypename().equals(Const.VideofileType)) {
                            array.add(listProg.get(i).getLessonfiles()[j]);
                        } else if (listProg.get(i).getLessonfiles()[j].getFiles().getFiletypename().equals(Const.ImagefileType)) {
                            array.add(listProg.get(i).getLessonfiles()[j]);
                        }
                    }
                    listProg.get(i).setLessonfiles(array.toArray(new CoursePriviewObject.Lessonfiles[array.size()]));
                }
            }*/

            holder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
            holder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new WrapContentLinearLayoutManager(NoonApplication.getContext(), LinearLayoutManager.VERTICAL, false));
            CourseItemInnerListAdapter adp = new CourseItemInnerListAdapter(ctx, listProg, courseInnerItemInterface, isisLastItemViewed, isPlayFirstItem, model.getId(), userDetailsObject, activityFlag, lessonID, QuizID, isNotification, courseHideResponse, assignments, courseName, gradeId);
            holder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
            //   holder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.post(() -> adp.notifyDataSetChanged());
        }
    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    public ArrayList<CoursePriviewObject.Chapters> getItems() {
        return list;
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }

    public boolean isLastItemViewed(int position) {
        if (position == 0) {
            return true;
        } else {
            CoursePriviewObject.Chapters modelPrv = list.get(position - 1);
            ArrayList<CoursePriviewObject.Lessons> listProg = new ArrayList<>();
            if (modelPrv.getLessons() != null) {
                Collections.addAll(listProg, modelPrv.getLessons());
                if (listProg.size() != 0) {
                    int progressVal = listProg.get(listProg.size() - 1).getProgressVal();
                    if (progressVal == 100) {

                        //Log.e(Const.LOG_NOON_TAG, "=====list====" + position + "=====SIZE====" + list.size());

                        int TotalLessonItem = list.size();
                        int countper = (int) ((position) * 100 / TotalLessonItem);
                        new ProgressTask(GradeId, String.valueOf(countper)).execute();
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }
    }

    public class ProgressTask extends AsyncTask<Void, Void, String> {

        private String gradeID;
        private String progressval;

        ProgressTask(String gradeID, String progressval) {
            this.gradeID = gradeID;
            this.progressval = progressval;
        }

        @Override
        protected String doInBackground(Void... params) {
            /*TokenObject tokenObject = PrefUtils.getTokenDetails(ctx);
            String userID = "";
            if (tokenObject.getId() != null) {
                userID = tokenObject.getId();
            }*/
            GradeProgress gradeProgress = AppDatabase.getAppDatabase(ctx).gradeProgressDao().getItemGradeProgress(gradeID);
            if (gradeProgress != null) {
                AppDatabase.getAppDatabase(ctx).gradeProgressDao().updateItemGradeProgress(gradeID, progressval);
            } else {
                GradeProgress gradeProgress1 = new GradeProgress();
                gradeProgress1.setUserid(userId);
                gradeProgress1.setGradeid(gradeID);
                gradeProgress1.setGradeprogress(progressval);
                gradeProgress1.setIsStatus("0");
                AppDatabase.getAppDatabase(ctx).gradeProgressDao().insertAll(gradeProgress1);
                //Log.e(Const.LOG_NOON_TAG, "===INSERT DATA===" + gradeProgress1.toString());
            }
            return null;
        }

        @Override
        protected void onPostExecute(final String success) {

        }
    }
}