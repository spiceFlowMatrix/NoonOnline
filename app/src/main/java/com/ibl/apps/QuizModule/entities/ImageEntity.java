package com.ibl.apps.QuizModule.entities;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.ForeignKey;
import androidx.room.PrimaryKey;

@Entity(tableName = "images",
        foreignKeys = @ForeignKey(entity = QuestionEntity.class,
                parentColumns = "id",
                childColumns = "question_id"
        )
)
public class ImageEntity {

    @PrimaryKey
    @ColumnInfo(name = "id")
    private Long id;

    @ColumnInfo(name = "question_id", index = true)
    private Long questionId;

    @ColumnInfo(name = "url")
    private String url;

    public ImageEntity() {
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Long getQuestionId() {
        return questionId;
    }

    public void setQuestionId(Long questionId) {
        this.questionId = questionId;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }
}
