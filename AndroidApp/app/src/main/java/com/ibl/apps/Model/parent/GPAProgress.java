package com.ibl.apps.Model.parent;

public class GPAProgress
{
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
        private String grade;

        public String getGrade ()
        {
            return grade;
        }

        public void setGrade (String grade)
        {
            this.grade = grade;
        }

        @Override
        public String toString()
        {
            return "ClassPojo [grade = "+grade+"]";
        }
    }
}
			
		