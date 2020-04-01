package com.ibl.apps.Utils;

import android.os.Build;
import android.os.Environment;

import com.ibl.apps.noon.BuildConfig;

import java.io.File;

/**
 * Created by iblinfotech on 16/02/18.
 */


public class Const {

    // For Staging [ Fabric Deployment ] API URL [DOMAIN ADDRESS]
//    public final static String BASE_URL = "https://coreapistaging.noon-online.com/api/v1/";;

    // For Dari Production [Pointing to PLAY STORE] API URL
    public final static String BASE_URL = BuildConfig.BASE_URL;

    // For Production Point Dari Feedback app
//    public final static String FEEDBACK_URL = "https://feedbackdari.noon-online.com/feedback?hl=fa";

    // For Production Point Pasto Feedback app
    // public final static String FEEDBACK_URL = "https://feedbackpashto.noon-online.com/feedback?hl=ps";

    // For Auth0 Token Decode parameter Value
    public final static String LOG_NOON_EXP = "exp";
    public final static String LOG_NOON_SUB = "sub";
    public final static String LOG_NOON_AUD = "aud";
    public final static String LOG_NOON_ISS = "iss";
    public final static String LOG_NOON_IAT = "iat";
    public final static String LOG_NOON_NAME = "name";
    public final static String LOG_NOON_NICKNAME = "nickname";
    public final static String LOG_NOON_ROLES = "https://noon-online/roles";

    // TAG
    public final static String LOG_NOON_TAG = "NOON_TAG";

    // PrefUtils Const Value
    public final static String PREF_NAME = "noon";
    public final static String PREF_AUTH_ID = "AUTHID";
    public final static String PREF_refreshedToken = "PREF_refreshedToken";

    // Intent Pass FillesData Key
    public final static String QuizID = "QuizID";
    public final static String LessonID = "LessonID";
    public final static String ActivityFlag = "ActivityFlag";
    public final static String GradeID = "GradeID";
    public final static String CourseName = "CourseName";
    public final static String userDetailsObjectString = "userDetailsObjectString";
    public final static String AssignmentId = "AssignmentId";
    public final static String isDiscussions = "isDiscussions";
    public final static String isNotification = "isNotification";
    public final static String Addtionalservice = "Addtionalservice";
    public final static String AddtionalDiscussions = "AddtionalDiscussions";
    public final static String AddtionalLibrary = "AddtionalLibrary";
    public final static String AddtionalAssignment = "AddtionalAssignment";

    // For Device uniqe value
    public final static String var_deviceType = "Android";
    public final static String var_androidOSversion = Build.VERSION.RELEASE;

    // API Parameter Key Value
    public final static String isforgot = "isforgot";
    public final static String forgotkey = "forgotkey";
    public final static String authId = "authId";
    public final static String email = "email";
    public final static String password = "password";
    public final static String confirmPassword = "confirmPassword";
    public final static String deviceType = "deviceType";
    public final static String deviceToken = "deviceToken";
    public final static String version = "version";
    public final static String new_password = "new_password";
    public final static String old_password = "old_password";
    public final static String confirm_password = "confirm_password";
    public final static String username = "username";
    public final static String fullname = "fullname";
    public final static String bio = "bio";
    public final static String id = "id";
    public final static String phonenumber = "phonenumber";
    public final static String lessonid = "lessonid";
    public final static String lessonprogress = "lessonprogress";
    public final static String uploadImagePara = "file";

    // For SecretKey
    public final static String ALGO_SECRATE_KEY_NAME = "SHAPRNG12345NOON";
    public final static String ALGO_SECRET_KEY_GENERATOR = "AES";
    public final static int IV_LENGTH = 16;

    // For Time
    public final static String currantTime = String.valueOf(System.currentTimeMillis());

    //External directory path to save file
    public final static String destPath = Environment.getExternalStorageDirectory() + File.separator + ".Noon/";
    public final static String PDFextension = ".pdf";
    public final static String VIDEOextension = ".mp4";
    public final static String dir_fileName = "Noon";
    public final static String extension = ".noon";
    public final static String PDFfileType = "PDF";
    public final static String VideofileType = "Video";
    public final static String ImagefileType = "Image";

    //For Firebase Log events
    public final static String NOON_USERID = "NOON_USERID";
    public final static String NOON_USERNAME = "NOON_USERNAME";
    public final static String NOON_FULLNAME = "NOON_FULLNAME";
    public final static String NOON_GRADEID = "NOON_GRADEID";
    public final static String NOON_CHAPTERID = "NOON_CHAPTERID";
    public final static String NOON_LESSONID = "NOON_LESSONID";
    public final static String NOON_FILEID = "NOON_FILEID";
    public final static String NOON_QUIZID = "NOON_QUIZID";

    // For Reminder
    public final static String PREF_REMINDER_DATA = "Reminder_Data";

    // Sync Key Value
    public final static String PROGRESSDATA = "progressdata";
    public final static String TIMERDATA = "timerdata";

    // Discussions Key Value
    public final static String title = "title";
    public final static String description = "description";
    public final static String isprivate = "isprivate";
    public final static String topicId = "topicId";
    public final static String topicname = "topicname";
    public final static String Id = "Id";
    public final static String comment = "comment";
    public final static String teacherid = "teacherid";
    public final static String uploadTopicFile = "files";
    public final static String fileTypeId = "fileTypeId";
    public final static String duration = "duration";
    public final static String filesize = "filesize";
    public final static String filesid = "filesid";
    public final static String createduserid = "createduserid";
    public final static String comments = "comments";
    public final static String user = "user";
    public final static String files = "files";
    public final static String courseid = "courseid";
    public final static String ispublic = "ispublic";

    // Notification
    public final static String noti_alert = "alert";
    public final static String noti_n = "n";
    public final static String noti_type = "Type";
    public final static String DOWNLOAD_ALERT = "Download_Alert";
    public final static String StudentKEY = "Student";
    public final static String TeacherKEY = "Teacher";

    //For SignUp
    public final static String Email = "Email";
    public final static String Password = "Password";
    public final static String RepeatPassword = "RepeatPassword";
    public final static String StudentName = "StudentName";
    public final static String FatherName = "FatherName";
    public final static String Gender = "Gender";
    public final static String DateOfBirth = "DateOfBirth";
    public final static String PlaceOfBirth = "PlaceOfBirth";
    public final static String Province = "Province";
    public final static String City = "City";
    public final static String Village = "Village";
    public final static String SchoolName = "SchoolName";
    public final static String GradeId = "GradeId";
    public final static String TazkiraNo = "TazkiraNo";
    public final static String Phone = "Phone";
    public final static String SoicalMediaLinked = "SoicalMediaLinked";
    public final static String SocialMediaAccount = "SocialMediaAccount";
    public final static String Reference = "Reference";

    //For Assignment
    public final static String Assignment = "Assignment";
    public final static String Flag = "flag";
    public final static String assignmentid = "assignmentid";
    public final static String userid = "userid";
    public final static String issubmission = "issubmission";
    public final static String isapproved = "isapproved";
    public final static String score = "score";
    public final static String remark = "remark";

    //For Student Analysis
    public final static String latitude = "latitude";
    public final static String longitude = "longitude";
    public final static String serviceprovider = "serviceprovider";
    public final static String hardwareplatform = "hardwareplatform";
    public final static String operatingsystem = "operatingsystem";
    public final static String ISP = "isp";
    public final static String activitytime = "activitytime";
    public final static String outtime = "outtime";
    public final static String networkspeed = "networkspeed";
    public final static String topicid = "topicid";
    public final static String like = "like";
    public final static String dislike = "dislike";
    public final static String profileurl = "profileurl";
    public final static String userName = "userName";
    public final static String createdtime = "createdtime";
    public final static String iseditable = "iseditable";
    public final static String likecount = "likecount";
    public final static String dislikecount = "dislikecount";
    public final static String liked = "liked";
    public final static String disliked = "disliked";
    public final static String commentid = "commentid";

    //For Privacy Policy
    public final static String isAgree = "isAgree";
    public final static String privacyPolicy = "privacyPolicy";

    //For Feedback Module
    public final static int capturevideo = 1;
    public final static int pickvideo = 2;
    public final static String categoryid = "categoryid";
    public final static String gradeid = "gradeid";
    public final static String time = "time";
    public final static String device = "device";
    public final static String appversion = "appversion";
    public final static String filesids = "filesids";
    public final static String chapterid = "chapterid";
}
