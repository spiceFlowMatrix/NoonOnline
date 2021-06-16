package com.ibl.apps.Interface;

import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;

import java.util.List;

/**
 * Created by iblinfotech on 17/12/18.
 */

public interface CourseAsyncResponse {
    List<CourseObject.Data> getLocalUserDetails(List<CourseObject.Data> courseListl);
}
