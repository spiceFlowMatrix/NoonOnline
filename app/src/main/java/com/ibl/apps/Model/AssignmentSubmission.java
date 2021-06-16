package com.ibl.apps.Model;

public class AssignmentSubmission {

    private String response_code;

    private Data data;

    private String message;

    private String status;

    public String getResponse_code ()
    {
        return response_code;
    }

    public void setResponse_code (String response_code)
    {
        this.response_code = response_code;
    }

    public Data getData ()
    {
        return data;
    }

    public void setData (Data data)
    {
        this.data = data;
    }

    public String getMessage ()
    {
        return message;
    }

    public void setMessage (String message)
    {
        this.message = message;
    }

    public String getStatus ()
    {
        return status;
    }

    public void setStatus (String status)
    {
        this.status = status;
    }

    @Override
    public String toString()
    {
        return "ClassPojo [response_code = "+response_code+", data = "+data+", message = "+message+", status = "+status+"]";
    }

    public class Data
    {
        private String score;

        private String isapproved;

        private String issubmission;

        private String remark;

        private String comment;

        private String id;

        private String datecreated;

        private String assignmentid;

        private String userid;

        private String submissionfiles;

        public String getScore ()
        {
            return score;
        }

        public void setScore (String score)
        {
            this.score = score;
        }

        public String getIsapproved ()
        {
            return isapproved;
        }

        public void setIsapproved (String isapproved)
        {
            this.isapproved = isapproved;
        }

        public String getIssubmission ()
        {
            return issubmission;
        }

        public void setIssubmission (String issubmission)
        {
            this.issubmission = issubmission;
        }

        public String getRemark ()
    {
        return remark;
    }

        public void setRemark (String remark)
        {
            this.remark = remark;
        }

        public String getComment ()
        {
            return comment;
        }

        public void setComment (String comment)
        {
            this.comment = comment;
        }

        public String getId ()
        {
            return id;
        }

        public void setId (String id)
        {
            this.id = id;
        }

        public String getDatecreated ()
    {
        return datecreated;
    }

        public void setDatecreated (String datecreated)
        {
            this.datecreated = datecreated;
        }

        public String getAssignmentid ()
        {
            return assignmentid;
        }

        public void setAssignmentid (String assignmentid)
        {
            this.assignmentid = assignmentid;
        }

        public String getUserid ()
        {
            return userid;
        }

        public void setUserid (String userid)
        {
            this.userid = userid;
        }

        public String getSubmissionfiles ()
    {
        return submissionfiles;
    }

        public void setSubmissionfiles (String submissionfiles)
        {
            this.submissionfiles = submissionfiles;
        }

        @Override
        public String toString()
        {
            return "ClassPojo [score = "+score+", isapproved = "+isapproved+", issubmission = "+issubmission+", remark = "+remark+", comment = "+comment+", id = "+id+", datecreated = "+datecreated+", assignmentid = "+assignmentid+", userid = "+userid+", submissionfiles = "+submissionfiles+"]";
        }
    }

}
