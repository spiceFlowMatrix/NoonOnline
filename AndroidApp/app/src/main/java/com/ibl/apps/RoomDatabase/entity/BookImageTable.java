package com.ibl.apps.RoomDatabase.entity;

import android.arch.persistence.room.ColumnInfo;
import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

import java.util.Arrays;

/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Entity
public class BookImageTable {

    @PrimaryKey(autoGenerate = true)
    private int BookImageId;

    private String bookId;

    private String userId;

    @ColumnInfo(typeAffinity = ColumnInfo.BLOB)
    private byte[] BookImage;

    public int getBookImageId() {
        return BookImageId;
    }

    public void setBookImageId(int bookImageId) {
        BookImageId = bookImageId;
    }

    public String getBookId() {
        return bookId;
    }

    public void setBookId(String bookId) {
        this.bookId = bookId;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public byte[] getBookImage() {
        return BookImage;
    }

    public void setBookImage(byte[] bookImage) {
        BookImage = bookImage;
    }

    @Override
    public String toString() {
        return "BookImageTable{" +
                "BookImageId=" + BookImageId +
                ", bookId='" + bookId + '\'' +
                ", userId='" + userId + '\'' +
                ", BookImage=" + Arrays.toString(BookImage) +
                '}';
    }
}
