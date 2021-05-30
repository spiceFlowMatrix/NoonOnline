package com.ibl.apps.QuizModule.entities;

import androidx.room.ColumnInfo;
import androidx.room.Embedded;
import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;
import com.ibl.apps.QuizModule.helpers.DateConverter;

import java.util.Date;
import java.util.List;

@Entity(tableName = "quizzes")
public class QuizEntity {

    @PrimaryKey
    @ColumnInfo(name = "id")
    private Long id;

    @ColumnInfo(name = "code", defaultValue = "NULL")
    private String code;

    @ColumnInfo(name = "name", defaultValue = "NULL")
    private String name;

    @ColumnInfo(name = "number_of_questions", defaultValue = "10")
    private Float numberOfQuestions;

    @ColumnInfo(name = "timeout", defaultValue = "NULL")
    private Integer timeout;

    @ColumnInfo(name = "passmark", defaultValue = "10")
    private Float passMark;

    @ColumnInfo(name = "quiz_summary", defaultValue = "NULL")
    private String quizSummary;

    @ColumnInfo(name = "creation_time", defaultValue = "NULL")
    @TypeConverters(DateConverter.class)
    private Date creationTime;

    @ColumnInfo(name = "last_modification_time", defaultValue = "NULL")
    @TypeConverters(DateConverter.class)
    private Date lastModificationTime;

    @ColumnInfo(name = "is_deleted", defaultValue = "false")
    private Boolean isDeleted;

    @ColumnInfo(name = "deletion_time", defaultValue = "NULL")
    @TypeConverters(DateConverter.class)
    private Date deletionTime;

    public QuizEntity() {
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
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

    public Float getNumberOfQuestions() {
        return numberOfQuestions;
    }

    public void setNumberOfQuestions(Float numberOfQuestions) {
        this.numberOfQuestions = numberOfQuestions;
    }

    public Integer getTimeout() {
        return timeout;
    }

    public void setTimeout(Integer timeout) {
        this.timeout = timeout;
    }

    public Float getPassMark() {
        return passMark;
    }

    public void setPassMark(Float passMark) {
        this.passMark = passMark;
    }

    public String getQuizSummary() {
        return quizSummary;
    }

    public void setQuizSummary(String quizSummary) {
        this.quizSummary = quizSummary;
    }

    public Date getCreationTime() {
        return creationTime;
    }

    public void setCreationTime(Date creationTime) {
        this.creationTime = creationTime;
    }

    public Date getLastModificationTime() {
        return lastModificationTime;
    }

    public void setLastModificationTime(Date lastModificationTime) {
        this.lastModificationTime = lastModificationTime;
    }

    public Boolean getDeleted() {
        return isDeleted;
    }

    public void setDeleted(Boolean deleted) {
        isDeleted = deleted;
    }

    public Date getDeletionTime() {
        return deletionTime;
    }

    public void setDeletionTime(Date deletionTime) {
        this.deletionTime = deletionTime;
    }
}
