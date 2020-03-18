package com.ibl.apps.Model.assignment;

import java.util.ArrayList;

public class AssignmentData {
    private String score;

    private boolean isapproved;

    private boolean issubmission;

    private String remark;

    private String comment;

    private String id;

    private String datecreated;

    private String assignmentid;

    private String userid;

    private User user;

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

    private ArrayList<SubmissionFiles> submissionfiles;

    public String getScore() {
        return score;
    }

    public void setScore(String score) {
        this.score = score;
    }

    public boolean getIsapproved() {
        return isapproved;
    }

    public void setIsapproved(boolean isapproved) {
        this.isapproved = isapproved;
    }

    public boolean getIssubmission() {
        return issubmission;
    }

    public void setIssubmission(boolean issubmission) {
        this.issubmission = issubmission;
    }

    public String getRemark() {
        return remark;
    }

    public void setRemark(String remark) {
        this.remark = remark;
    }

    public String getComment() {
        return comment;
    }

    public void setComment(String comment) {
        this.comment = comment;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getDatecreated() {
        return datecreated;
    }

    public void setDatecreated(String datecreated) {
        this.datecreated = datecreated;
    }

    public String getAssignmentid() {
        return assignmentid;
    }

    public void setAssignmentid(String assignmentid) {
        this.assignmentid = assignmentid;
    }

    public String getUserid() {
        return userid;
    }

    public void setUserid(String userid) {
        this.userid = userid;
    }

    public ArrayList<SubmissionFiles> getSubmissionfiles() {
        return submissionfiles;
    }

    public void setSubmissionfiles(ArrayList<SubmissionFiles> submissionfiles) {
        this.submissionfiles = submissionfiles;
    }

    @Override
    public String toString() {
        return "AssignmentData{" +
                "score='" + score + '\'' +
                ", isapproved=" + isapproved +
                ", issubmission=" + issubmission +
                ", remark='" + remark + '\'' +
                ", comment='" + comment + '\'' +
                ", id='" + id + '\'' +
                ", datecreated='" + datecreated + '\'' +
                ", assignmentid='" + assignmentid + '\'' +
                ", userid='" + userid + '\'' +
                ", submissionfiles=" + submissionfiles +
                '}';
    }
}
