package com.ibl.apps.Model;

import android.arch.persistence.room.ColumnInfo;
import android.arch.persistence.room.Embedded;
import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;
import android.arch.persistence.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Created by iblinfotech on 21/09/18.
 */

@Entity
public class CoursePriviewObject {

    @PrimaryKey(autoGenerate = true)
    private int CoursedetailsID;

    private String message;

    private String status;

    private String response_code;

    private String userId;

    @Embedded
    public Data data;

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

    public int getCoursedetailsID() {
        return CoursedetailsID;
    }

    public void setCoursedetailsID(int coursedetailsID) {
        CoursedetailsID = coursedetailsID;
    }

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
        this.data = data;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    @Override
    public String toString() {
        return "CoursePriviewObject{" +
                "CoursedetailsID=" + CoursedetailsID +
                ", message='" + message + '\'' +
                ", status='" + status + '\'' +
                ", response_code='" + response_code + '\'' +
                ", userId='" + userId + '\'' +
                ", data=" + data +
                '}';
    }

    public static class Data {

        private String id;

        @ColumnInfo(name = "ListChapters")
        @TypeConverters(DataTypeConverter.class)
        private List<Chapters> chapters = null;

        private String description;

        private String name;

        private String image;

        private String code;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public List<Chapters> getChapters() {
            return chapters;
        }

        public void setChapters(List<Chapters> chapters) {
            this.chapters = chapters;
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

        public String getImage() {
            return image;
        }

        public void setImage(String image) {
            this.image = image;
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", chapters = " + chapters + ", description = " + description + ", name = " + name + ", image = " + image + ", code = " + code + "]";
        }
    }

    public class Chapters {

        private String id;

        private Lessons[] lessons;

        private Quizs[] quizs;

        private ArrayList<Assignment> assignments;

        private String name;

        private String code;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public Lessons[] getLessons() {
            return lessons;
        }

        public void setLessons(Lessons[] lessons) {
            this.lessons = lessons;
        }

        public Quizs[] getQuizs() {
            return quizs;
        }

        public void setQuizs(Quizs[] quizs) {
            this.quizs = quizs;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public ArrayList<Assignment> getAssignments() {
            return assignments;
        }

        public void setAssignments(ArrayList<Assignment> assignments) {
            this.assignments = assignments;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", lessons = " + lessons + ", quizs = " + quizs + ", name = " + name + ", code = " + code + "]";
        }
    }

    public class Quizs {

        private String id;

        private String itemorder;

        private String name;

        private String code;

        private String numquestions;

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

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public String getNumquestions() {
            return numquestions;
        }

        public void setNumquestions(String numquestions) {
            this.numquestions = numquestions;
        }

        public String getItemorder() {
            return itemorder;
        }

        public void setItemorder(String itemorder) {
            this.itemorder = itemorder;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", name = " + name + ", code = " + code + ", numquestions = " + numquestions + "]";
        }
    }

    public static class Lessons {

        private String id;

        private String description;

        private String name;

        private Lessonfiles[] lessonfiles;

        private Assignment assignment;

        private String code;

        private int selectedItem = 0;

        private int progressVal = 0;

        private String numquestions;

        private String itemorder;

        public String getItemorder() {
            return itemorder;
        }

        public void setItemorder(String itemorder) {
            this.itemorder = itemorder;
        }

        public int getSelectedItem() {
            return selectedItem;
        }

        public void setSelectedItem(int selectedItem) {
            this.selectedItem = selectedItem;
        }

        public String getNumquestions() {
            return numquestions;
        }

        public void setNumquestions(String numquestions) {
            this.numquestions = numquestions;
        }

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

        public Lessonfiles[] getLessonfiles() {
            return lessonfiles;
        }

        public void setLessonfiles(Lessonfiles[] lessonfiles) {
            this.lessonfiles = lessonfiles;
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public int getProgressVal() {
            return progressVal;
        }

        public void setProgressVal(int progressVal) {
            this.progressVal = progressVal;
        }

        @Override
        public String toString() {
            return "Lessons{" +
                    "id='" + id + '\'' +
                    ", description='" + description + '\'' +
                    ", name='" + name + '\'' +
                    ", lessonfiles=" + Arrays.toString(lessonfiles) +
                    ", code='" + code + '\'' +
                    ", selectedItem=" + selectedItem +
                    ", progressVal=" + progressVal +
                    ", numquestions='" + numquestions + '\'' +
                    ", itemorder='" + itemorder + '\'' +
                    '}';
        }

        public Assignment getAssignment() {
            return assignment;
        }

        public void setAssignment(Assignment assignment) {
            this.assignment = assignment;
        }
    }

   /* public static class Assignmentfiles {

        private String id;

        private Files files;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public Files getFiles() {
            return files;
        }

        public void setFiles(Files files) {
            this.files = files;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", files = " + files + "]";
        }
    }*/

    public class Assignmentfiles {
        private Files files;

        private Progress progress;

        private String id;

        public Files getFiles() {
            return files;
        }

        public void setFiles(Files files) {
            this.files = files;
        }

        public Progress getProgress() {
            return progress;
        }

        public void setProgress(Progress progress) {
            this.progress = progress;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        @Override
        public String toString() {
            return "ClassPojo [files = " + files + ", progress = " + progress + ", id = " + id + "]";
        }
    }

    public class Lessonfiles {
        private Progress progress;

        private String id;

        private Files files;

        public Progress getProgress() {
            return progress;
        }

        public void setProgress(Progress progress) {
            this.progress = progress;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public Files getFiles() {
            return files;
        }

        public void setFiles(Files files) {
            this.files = files;
        }

        @Override
        public String toString() {
            return "Lessonfiles{" +
                    "progress=" + progress +
                    ", id='" + id + '\'' +
                    ", files=" + files +
                    '}';
        }
    }

    public class Files {

        private String id;

        private String filesize;

        private String description;

        private String name;

        private String filetypename;

        private String filename;

        private String filetypeid;

        private String url;

        private String signedUrl;

        public String getSignedUrl() {
            return signedUrl;
        }

        public void setSignedUrl(String signedUrl) {
            this.signedUrl = signedUrl;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getFilesize() {
            return filesize;
        }

        public void setFilesize(String filesize) {
            this.filesize = filesize;
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

        public String getFiletypename() {
            return filetypename;
        }

        public void setFiletypename(String filetypename) {
            this.filetypename = filetypename;
        }

        public String getFilename() {
            return filename;
        }

        public void setFilename(String filename) {
            this.filename = filename;
        }

        public String getFiletypeid() {
            return filetypeid;
        }

        public void setFiletypeid(String filetypeid) {
            this.filetypeid = filetypeid;
        }

        public String getUrl() {
            return url;
        }

        public void setUrl(String url) {
            this.url = url;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", filesize = " + filesize + ", description = " + description + ", name = " + name + ", filetypename = " + filetypename + ", filename = " + filename + ", filetypeid = " + filetypeid + ", url = " + url + "]";
        }
    }

    public class Progress {
        private String id;

        private String duration;

        private String lessonstatus;

        private String lessonprogress;

        private String lessonid;

        private String studentid;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getDuration() {
            return duration;
        }

        public void setDuration(String duration) {
            this.duration = duration;
        }

        public String getLessonstatus() {
            return lessonstatus;
        }

        public void setLessonstatus(String lessonstatus) {
            this.lessonstatus = lessonstatus;
        }

        public String getLessonprogress() {
            return lessonprogress;
        }

        public int getLessonprogressAsInt() {
            try {
                return Integer.parseInt(lessonprogress);
            } catch (Exception e) {
                return 0;
            }
        }

        public void setLessonprogress(String lessonprogress) {
            this.lessonprogress = lessonprogress;
        }

        public String getLessonid() {
            return lessonid;
        }

        public void setLessonid(String lessonid) {
            this.lessonid = lessonid;
        }

        public String getStudentid() {
            return studentid;
        }

        public void setStudentid(String studentid) {
            this.studentid = studentid;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", duration = " + duration + ", lessonstatus = " + lessonstatus + ", lessonprogress = " + lessonprogress + ", lessonid = " + lessonid + ", studentid = " + studentid + "]";
        }
    }

    public class Assignment {
        private ArrayList<Assignmentfiles> assignmentfiles;

        private String code;

        private String name;

        private String description;

        private String id;

        public ArrayList<Assignmentfiles> getAssignmentfiles() {
            return assignmentfiles;
        }

        public void setAssignmentfiles(ArrayList<Assignmentfiles> assignmentfiles) {
            this.assignmentfiles = assignmentfiles;
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        @Override
        public String toString() {
            return "ClassPojo [assignmentfiles = " + assignmentfiles + ", code = " + code + ", name = " + name + ", description = " + description + ", id = " + id + "]";
        }
    }

}

