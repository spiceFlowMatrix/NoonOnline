package com.ibl.apps.Model;

import androidx.room.ColumnInfo;

import java.util.Arrays;

/**
 * Created by iblinfotech on 27/10/18.
 */

public class SearchObject {

    private String message;

    private String status;

    private String response_code;

    private Data data;

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
    }

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
        this.data = data;
    }

    @Override
    public String toString() {
        return "ClassPojo [message = " + message + ", status = " + status + ", response_code = " + response_code + ", data = " + data + "]";
    }

    public class Data {
        private Assignment[] assignment;

        private Lessons[] lessons;

        private Courses[] courses;

        public Assignment[] getAssignment() {
            return assignment;
        }

        public void setAssignment(Assignment[] assignment) {
            this.assignment = assignment;
        }

        public Lessons[] getLessons() {
            return lessons;
        }

        public void setLessons(Lessons[] lessons) {
            this.lessons = lessons;
        }

        public Courses[] getCourses() {
            return courses;
        }

        public void setCourses(Courses[] courses) {
            this.courses = courses;
        }

        @Override
        public String toString() {
            return "ClassPojo [assignment = " + assignment + ", lessons = " + lessons + ", courses = " + courses + "]";
        }
    }

    public class Assignment {
        private String id;

        private Coursewithgrade coursewithgrade;

        private String name;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public Coursewithgrade getCoursewithgrade() {
            return coursewithgrade;
        }

        public void setCoursewithgrade(Coursewithgrade coursewithgrade) {
            this.coursewithgrade = coursewithgrade;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", coursewithgrade = " + coursewithgrade + ", name = " + name + "]";
        }
    }

    public class Coursewithgrade {
        private String id;

        private GradeDetails[] gradeDetails;

        private String name;

        private String image;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public GradeDetails[] getGradeDetails() {
            return gradeDetails;
        }

        public void setGradeDetails(GradeDetails[] gradeDetails) {
            this.gradeDetails = gradeDetails;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getImage() {
            return image;
        }

        public void setImage(String image) {
            this.image = image;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", gradeDetails = " + gradeDetails + ", name = " + name + ", image = " + image + "]";
        }
    }

    public static class GradeDetails {

        private String id;

        private String description;

        private String name;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", description = " + description + ", name = " + name + "]";
        }
    }

    public static class Lessons {

        private String id;

        private String name;

        private String coursename;

        private String chapterid;

        private String courseid;

        private String progressval;

        private String lessonfileid;

        private String filetypename;

        private String lessonquizid;

        private String lessonfilename;

        private String filetypeid;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getCoursename() {
            return coursename;
        }

        public void setCoursename(String coursename) {
            this.coursename = coursename;
        }

        public String getChapterid() {
            return chapterid;
        }

        public void setChapterid(String chapterid) {
            this.chapterid = chapterid;
        }

        public String getCourseid() {
            return courseid;
        }

        public void setCourseid(String courseid) {
            this.courseid = courseid;
        }

        public String getProgressval() {
            return progressval;
        }

        public void setProgressval(String progressval) {
            this.progressval = progressval;
        }

        public String getLessonfileid() {
            return lessonfileid;
        }

        public void setLessonfileid(String lessonfileid) {
            this.lessonfileid = lessonfileid;
        }

        public String getFiletypename() {
            return filetypename;
        }

        public void setFiletypename(String filetypename) {
            this.filetypename = filetypename;
        }

        public String getLessonquizid() {
            return lessonquizid;
        }

        public void setLessonquizid(String lessonquizid) {
            this.lessonquizid = lessonquizid;
        }

        public String getLessonfilename() {
            return lessonfilename;
        }

        public void setLessonfilename(String lessonfilename) {
            this.lessonfilename = lessonfilename;
        }

        public String getFiletypeid() {
            return filetypeid;
        }

        public void setFiletypeid(String filetypeid) {
            this.filetypeid = filetypeid;
        }


        @Override
        public String toString() {
            return "Lessons{" +
                    "id='" + id + '\'' +
                    ", name='" + name + '\'' +
                    ", coursename='" + coursename + '\'' +
                    ", chapterid='" + chapterid + '\'' +
                    ", courseid='" + courseid + '\'' +
                    ", progressval='" + progressval + '\'' +
                    ", lessonfileid='" + lessonfileid + '\'' +
                    ", filetypename='" + filetypename + '\'' +
                    ", lessonquizid='" + lessonquizid + '\'' +
                    ", lessonfilename='" + lessonfilename + '\'' +
                    ", filetypeid='" + filetypeid + '\'' +
                    '}';
        }
    }

    public static class Courses {

        private String id;

        private GradeDetails[] gradeDetails;

        private String name;

        private String image;

        private int progressVal;

        @ColumnInfo(typeAffinity = ColumnInfo.BLOB)
        private byte[] courseImage;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public GradeDetails[] getGradeDetails() {
            return gradeDetails;
        }

        public void setGradeDetails(GradeDetails[] gradeDetails) {
            this.gradeDetails = gradeDetails;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getImage() {
            return image;
        }

        public void setImage(String image) {
            this.image = image;
        }

        public int getProgressVal() {
            return progressVal;
        }

        public void setProgressVal(int progressVal) {
            this.progressVal = progressVal;
        }

        public byte[] getCourseImage() {
            return courseImage;
        }

        public void setCourseImage(byte[] courseImage) {
            this.courseImage = courseImage;
        }

        @Override
        public String toString() {
            return "Courses{" +
                    "id='" + id + '\'' +
                    ", gradeDetails=" + Arrays.toString(gradeDetails) +
                    ", name='" + name + '\'' +
                    ", image='" + image + '\'' +
                    ", progressVal=" + progressVal +
                    ", courseImage=" + Arrays.toString(courseImage) +
                    '}';
        }
    }


}

