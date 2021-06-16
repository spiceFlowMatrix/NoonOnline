package com.ibl.apps.Model;

import androidx.room.ColumnInfo;
import androidx.room.Embedded;
import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.List;

/**
 * Created by iblinfotech on 20/09/18.
 */

@Entity
public class QuizMainObject {

    @PrimaryKey(autoGenerate = true)
    private int autoQuizID;

    private String message;

    private String status;

    private String response_code;

    private String newquizId;

    private String userId;

    @Embedded
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

    public int getAutoQuizID() {
        return autoQuizID;
    }

    public void setAutoQuizID(int autoQuizID) {
        this.autoQuizID = autoQuizID;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getNewquizId() {
        return newquizId;
    }

    public void setNewquizId(String newquizId) {
        this.newquizId = newquizId;
    }

    @Override
    public String toString() {
        return "QuizMainObject{" +
                "autoQuizID=" + autoQuizID +
                ", message='" + message + '\'' +
                ", status='" + status + '\'' +
                ", response_code='" + response_code + '\'' +
                ", newquizId='" + newquizId + '\'' +
                ", userId='" + userId + '\'' +
                ", data=" + data +
                '}';
    }

    public static class Data {

        private String quizid;

        private String passmark;

        @ColumnInfo(name = "ListQuestions")
        @TypeConverters(DataTypeConverter.class)
        private List<Questions> questions;

        @Embedded
        private QuizSummaryResponse quizSummaryResponse;

        public String getQuizid() {
            return quizid;
        }

        public void setQuizid(String quizid) {
            this.quizid = quizid;
        }

        public String getPassmark() {
            return passmark;
        }

        public void setPassmark(String passmark) {
            this.passmark = passmark;
        }

        public List<Questions> getQuestions() {
            return questions;
        }

        public void setQuestions(List<Questions> questions) {
            this.questions = questions;
        }

        public QuizSummaryResponse getQuizSummaryResponse() {
            return quizSummaryResponse;
        }

        public void setQuizSummaryResponse(QuizSummaryResponse quizSummaryResponse) {
            this.quizSummaryResponse = quizSummaryResponse;
        }

        @Override
        public String toString() {
            return "ClassPojo [quizid = " + quizid + ", questions = " + questions + ", quizSummaryResponse = " + quizSummaryResponse + "]";
        }
    }

    public static class Questions {

        private String id;

        private String questiontypeid;

        private String questiontype;

        private String explanation;

        private Answers[] answers;

        private String ismultianswer;

        private Images[] images;

        private String questiontext;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getQuestiontypeid() {
            return questiontypeid;
        }

        public void setQuestiontypeid(String questiontypeid) {
            this.questiontypeid = questiontypeid;
        }

        public String getQuestiontype() {
            return questiontype;
        }

        public void setQuestiontype(String questiontype) {
            this.questiontype = questiontype;
        }

        public String getExplanation() {
            return explanation;
        }

        public void setExplanation(String explanation) {
            this.explanation = explanation;
        }

        public Answers[] getAnswers() {
            return answers;
        }

        public void setAnswers(Answers[] answers) {
            this.answers = answers;
        }

        public String getIsmultianswer() {
            return ismultianswer;
        }

        public void setIsmultianswer(String ismultianswer) {
            this.ismultianswer = ismultianswer;
        }

        public Images[] getImages() {
            return images;
        }

        public void setImages(Images[] images) {
            this.images = images;
        }

        public String getQuestiontext() {
            return questiontext;
        }

        public void setQuestiontext(String questiontext) {
            this.questiontext = questiontext;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", questiontypeid = " + questiontypeid + ", questiontype = " + questiontype + ", explanation = " + explanation + ", answers = " + answers + ", ismultianswer = " + ismultianswer + ", images = " + images + ", questiontext = " + questiontext + "]";
        }
    }

    public static class Answers {

        private String id;

        private String iscorrect;

        private String extratext;

        private String answer;

        private Images[] images;

        private String questionid;

        private String quizid;

        public String getQuizid() {
            return quizid;
        }

        public void setQuizid(String quizid) {
            this.quizid = quizid;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getIscorrect() {
            return iscorrect;
        }

        public void setIscorrect(String iscorrect) {
            this.iscorrect = iscorrect;
        }

        public String getExtratext() {
            return extratext;
        }

        public void setExtratext(String extratext) {
            this.extratext = extratext;
        }

        public String getAnswer() {
            return answer;
        }

        public void setAnswer(String answer) {
            this.answer = answer;
        }

        public Images[] getImages() {
            return images;
        }

        public void setImages(Images[] images) {
            this.images = images;
        }

        public String getQuestionid() {
            return questionid;
        }

        public void setQuestionid(String questionid) {
            this.questionid = questionid;
        }


    }

    public static class Images {

        private String fileid;

        private String url;

        public String getFileid() {
            return fileid;
        }

        public void setFileid(String fileid) {
            this.fileid = fileid;
        }

        public String getUrl() {
            return url;
        }

        public void setUrl(String url) {
            this.url = url;
        }

        @Override
        public String toString() {
            return "ClassPojo [fileid = " + fileid + ", url = " + url + "]";
        }
    }

    public static  class QuizSummaryResponse {
        private String qSummary;

        private String id;

        private String attempts;

        public String getQSummary() {
            return qSummary;
        }

        public void setQSummary(String qSummary) {
            this.qSummary = qSummary;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getAttempts() {
            return attempts;
        }

        public void setAttempts(String attempts) {
            this.attempts = attempts;
        }

        @Override
        public String toString() {
            return "ClassPojo [qSummary = " + qSummary + ", id = " + id + ", attempts = " + attempts + "]";
        }
    }


}
