package com.ibl.apps.Model.assignment;

import java.util.ArrayList;

public class CommentResponse {
    private boolean isapproved;
    private ArrayList<AssignmentData> details;

    public boolean isIsapproved() {
        return isapproved;
    }

    public void setIsapproved(boolean isapproved) {
        this.isapproved = isapproved;
    }

    public ArrayList<AssignmentData> getDetails() {
        return details;
    }

    public void setDetails(ArrayList<AssignmentData> details) {
        this.details = details;
    }
}
