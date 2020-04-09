package com.ibl.apps.RoomDatabase.database;

import androidx.room.TypeConverter;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.devicemanagement.Tags;
import com.ibl.apps.devicemanagement.UserDeviceProfileModel;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * Created by iblinfotech on 02/11/18.
 */

public class DataTypeConverter {

    private Gson gson = new Gson();

    @TypeConverter
    public List<CourseObject.Data> stringToList(String data) {
        if (data == null) {
            return Collections.emptyList();
        }

        Type listType = new TypeToken<List<CourseObject.Data>>() {
        }.getType();

        return gson.fromJson(data, listType);
    }

    @TypeConverter
    public String ListToString(List<CourseObject.Data> someObjects) {
        return gson.toJson(someObjects);
    }

    @TypeConverter
    public List<CoursePriviewObject.Chapters> stringToList2(String data) {
        if (data == null) {
            return Collections.emptyList();
        }

        Type listType = new TypeToken<List<CoursePriviewObject.Chapters>>() {
        }.getType();

        return gson.fromJson(data, listType);
    }

    @TypeConverter
    public String ListToString2(List<CoursePriviewObject.Chapters> someObjects) {
        return gson.toJson(someObjects);
    }

    @TypeConverter
    public List<String> stringToList3(String data) {
        if (data == null) {
            return Collections.emptyList();
        }

        Type listType = new TypeToken<List<String>>() {
        }.getType();

        return gson.fromJson(data, listType);
    }

    @TypeConverter
    public String ListToString3(List<String> someObjects) {
        return gson.toJson(someObjects);
    }

    @TypeConverter
    public List<QuizMainObject.Questions> stringToList4(String data) {
        if (data == null) {
            return Collections.emptyList();
        }

        Type listType = new TypeToken<List<QuizMainObject.Questions>>() {
        }.getType();

        return gson.fromJson(data, listType);
    }

    @TypeConverter
    public String ListToString4(List<QuizMainObject.Questions> someObjects) {
        return gson.toJson(someObjects);
    }


    @TypeConverter
    public List<LibraryObject.Data> stringToList5(String data) {
        if (data == null) {
            return Collections.emptyList();
        }

        Type listType = new TypeToken<List<LibraryObject.Data>>() {
        }.getType();

        return gson.fromJson(data, listType);
    }

    @TypeConverter
    public String ListToString5(List<LibraryObject.Data> someObjects) {
        return gson.toJson(someObjects);
    }

    @TypeConverter
    public List<LibraryGradeObject.Data> stringToList6(String data) {
        if (data == null) {
            return Collections.emptyList();
        }

        Type listType = new TypeToken<List<LibraryGradeObject.Data>>() {
        }.getType();

        return gson.fromJson(data, listType);
    }

    @TypeConverter
    public String ListToString6(List<LibraryGradeObject.Data> someObjects) {
        return gson.toJson(someObjects);
    }

    @TypeConverter
    public ArrayList<Tags> stringToList7(String data) {
        if (data == null) {
            return new ArrayList<Tags>();
        }

        Type listType = new TypeToken<ArrayList<Tags>>() {
        }.getType();

        return gson.fromJson(data, listType);
    }

    @TypeConverter
    public String ListToString7(ArrayList<Tags> someObjects) {
        return gson.toJson(someObjects);
    }

    @TypeConverter
    public ArrayList<UserDeviceProfileModel.CurrentDevices> stringToList8(String data) {
        if (data == null) {
            return new ArrayList<UserDeviceProfileModel.CurrentDevices>();
        }

        Type listType = new TypeToken<ArrayList<UserDeviceProfileModel.CurrentDevices>>() {
        }.getType();

        return gson.fromJson(data, listType);
    }

    @TypeConverter
    public String ListToString8(ArrayList<UserDeviceProfileModel.CurrentDevices> someObjects) {
        return gson.toJson(someObjects);
    }
}
