package com.ibl.apps.RoomDatabase.database;

import android.content.Context;

import androidx.room.Database;
import androidx.room.Room;
import androidx.room.RoomDatabase;
import androidx.room.TypeConverters;
import androidx.room.migration.Migration;
import androidx.sqlite.db.SupportSQLiteDatabase;

import com.ibl.apps.DeviceManagement.UserDeviceModel;
import com.ibl.apps.DeviceManagement.UserDeviceProfileModel;
import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.IntervalTableObject;
import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.Model.LoginObject;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.Model.StatisticsObject;
import com.ibl.apps.RoomDatabase.dao.chapterManagementDatabase.ChapterProgressDao;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDao;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.CourseDetailsDao;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.GradeProgressDao;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.IntervalDao;
import com.ibl.apps.RoomDatabase.dao.courseManagementDatabase.SyncTimeTrackingDao;
import com.ibl.apps.RoomDatabase.dao.deviceManagementDatabase.DeviceManagementDataDao;
import com.ibl.apps.RoomDatabase.dao.deviceManagementDatabase.DeviceManagementProfileDao;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.FileDownloadStatusDao;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.FileProgressDao;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.LessonNewProgressDao;
import com.ibl.apps.RoomDatabase.dao.lessonManagementDatabase.LessonProgressDao;
import com.ibl.apps.RoomDatabase.dao.libraryManagementDatabase.LibraryDao;
import com.ibl.apps.RoomDatabase.dao.libraryManagementDatabase.LibraryGradeDao;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizAnswerDao;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizProgressDao;
import com.ibl.apps.RoomDatabase.dao.quizManagementDatabase.QuizUserResultDao;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.AuthTokenDao;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.LoginDao;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.StatisticsDao;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.UserDetailDao;
import com.ibl.apps.RoomDatabase.entity.BookImageTable;
import com.ibl.apps.RoomDatabase.entity.ChapterProgress;
import com.ibl.apps.RoomDatabase.entity.CourseImageTable;
import com.ibl.apps.RoomDatabase.entity.FileDownloadStatus;
import com.ibl.apps.RoomDatabase.entity.FileProgress;
import com.ibl.apps.RoomDatabase.entity.GradeProgress;
import com.ibl.apps.RoomDatabase.entity.LessonNewProgress;
import com.ibl.apps.RoomDatabase.entity.LessonProgress;
import com.ibl.apps.RoomDatabase.entity.LessonSelectedStatus;
import com.ibl.apps.RoomDatabase.entity.NetworkStatus;
import com.ibl.apps.RoomDatabase.entity.QuizAnswerSelect;
import com.ibl.apps.RoomDatabase.entity.QuizProgress;
import com.ibl.apps.RoomDatabase.entity.QuizUserResult;
import com.ibl.apps.RoomDatabase.entity.SyncTimeTrackingObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;

@Database(entities = {LessonProgress.class,
        GradeProgress.class,
        FileDownloadStatus.class,
        QuizAnswerSelect.class,
        CourseObject.class,
        CoursePriviewObject.class,
        LessonSelectedStatus.class,
        CourseImageTable.class,
        LoginObject.class,
        UserDetails.class,
        StatisticsObject.class,
        QuizMainObject.class,
        AuthTokenObject.class,
        QuizUserResult.class,
        LibraryObject.class,
        BookImageTable.class,
        LibraryGradeObject.class,
        NetworkStatus.class,
        IntervalTableObject.class,
        SyncTimeTrackingObject.class,
        ChapterProgress.class,
        LessonNewProgress.class,
        FileProgress.class,
        QuizProgress.class,
        UserDeviceModel.class,
        UserDeviceProfileModel.class
},
        version = 5,
        exportSchema = false)
@TypeConverters(DataTypeConverter.class)
public abstract class AppDatabase extends RoomDatabase {

    private static AppDatabase INSTANCE;

    public static AppDatabase getAppDatabase(Context context) {
        if (INSTANCE == null) {
            INSTANCE = Room.databaseBuilder(
                    context.getApplicationContext(),
                    AppDatabase.class, "NoonDatabase")
                    .fallbackToDestructiveMigration()
                    //.addMigrations(MIGRATION_1_2)
                    .allowMainThreadQueries()
                    .build();
        }

        return INSTANCE;
    }

    //Add column
    static final Migration MIGRATION_1_2 = new Migration(3, 4) {
        @Override
        public void migrate(SupportSQLiteDatabase database) {

            //  database.execSQL("ALTER TABLE 'UserDetails' ADD COLUMN 'isallowmap' BOOLEAN");

//            database.execSQL("ALTER TABLE AuthTokenObject "
//                    + " ADD COLUMN isDeleted INTEGER default 0 NOT NULL");
//
//            database.execSQL("CREATE TABLE IF NOT EXISTS `BookImageTable`" +
//                    " (`BookImageId` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
//                    "`bookId` TEXT NOT NULL, " +
//                    "`userId` TEXT NOT NULL, " +
//                    "`BookImage` BLOB NOT NULL)");
//
//            database.execSQL("CREATE TABLE IF NOT EXISTS `LibraryObject`" +
//                    " (`libraryObjectID` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
//                    "`response_code` TEXT NOT NULL, " +
//                    "`message` TEXT NOT NULL, " +
//                    "`status` TEXT NOT NULL, " +
//                    "`userId` TEXT NOT NULL, " +
//                    "`ListData` TEXT NOT NULL)");
//
//            database.execSQL("CREATE TABLE IF NOT EXISTS `LibraryGradeObject`" +
//                    " (`libraryGradeObjectID` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
//                    "`response_code` TEXT NOT NULL, " +
//                    "`message` TEXT NOT NULL, " +
//                    "`status` TEXT NOT NULL, " +
//                    "`userId` TEXT NOT NULL, " +
//                    "`ListData` TEXT NOT NULL)");
//
//            database.execSQL("CREATE TABLE IF NOT EXISTS `SyncTimeTracking`" +
//                    " (`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
//                    "`latitude` TEXT NOT NULL, " +
//                    "`longitude` TEXT NOT NULL, " +
//                    "`serviceprovider` TEXT NOT NULL, " +
//                    "`school` TEXT NOT NULL, " +
//                    "`subjectstaken` TEXT NOT NULL, " +
//                    "`grade` TEXT NOT NULL ," +
//                    "`hardwareplatform` TEXT NOT NULL ," +
//                    "`operatingsystem` TEXT NOT NULL ," +
//                    "`version` TEXT NOT NULL)");
//            // You can only add one column at a time. Split it into two ALTER TABLE statements and you should be fine.
            //database.execSQL("ALTER TABLE UserDetails " + " ADD COLUMN is_assignment_authorized TEXT NOT NULL");
////
            //database.execSQL("ALTER TABLE UserDetails " + " ADD COLUMN is_library_authorized TEXT NOT NULL");
//
////            database.execSQL("ALTER TABLE QuizUserResult "
////                    + " ADD COLUMN `totalQuitions` TEXT NOT NULL "
////            );
////            database.execSQL("ALTER TABLE QuizUserResult "
////                    + " ADD COLUMN `totalAnswers` TEXT NOT NULL "
////            );
////            database.execSQL("ALTER TABLE QuizUserResult "
////                    + " ADD COLUMN `quizDate` TEXT NOT NULL "
////            );
////
////            database.execSQL("ALTER TABLE SyncTimeTrackingObject "
////                    + " ADD COLUMN userid INTEGER NOT NULL"
////            );
////            database.execSQL("ALTER TABLE SyncTimeTrackingObject "
////                    + " ADD COLUMN networkspeed TEXT NOT NULL"
////            );
////
////            database.execSQL("ALTER TABLE SyncTimeTrackingObject "
////                    + " ADD COLUMN isp TEXT NOT NULL"
////            );
//            database.execSQL("CREATE TABLE IF NOT EXISTS `NetworkStatus`" +
//                    " (`networkstatusid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
//                    "`networkStatus` BOOLEAN NOT NULL)");
//
//            database.execSQL("CREATE TABLE IF NOT EXISTS `IntervalTableObject`" +
//                    " (`IntervalTableID` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
//                    "`interval` TEXT NOT NULL, " +
//                    "`localinterval` TEXT NOT NULL)");

//            database.execSQL("ALTER TABLE SyncTimeTrackingObject "
//                   + " RENAME `userid` TO `u_id`");
        }
    };

    public static void destroyInstance() {
        INSTANCE.clearAllTables();
        INSTANCE = null;
    }

    //DONE
    public abstract LessonProgressDao lessonProgressDao();

    //DONE
    public abstract GradeProgressDao gradeProgressDao();

    //DONE
    public abstract FileDownloadStatusDao fileDownloadStatusDao();

    //DONE
    public abstract QuizAnswerDao quizAnswerDao();

    //DONE
    public abstract CourseDao courseDao();

    //DONE
    public abstract CourseDetailsDao courseDetailsDao();

    //DONE
    public abstract LoginDao loginDao();

    //DONE
    public abstract UserDetailDao userDetailDao();

    //DONE
    public abstract StatisticsDao statisticsDao();

    //DONE
    public abstract AuthTokenDao authTokenDao();

    //DONE
    public abstract QuizUserResultDao quizUserResultDao();

    //DONE
    public abstract LibraryDao libraryDao();

    //DONE
    public abstract LibraryGradeDao libraryGradeDao();

    //DONE
    public abstract IntervalDao intervalDao();

    //DONE
    public abstract SyncTimeTrackingDao syncTimeTrackingDao();

    //DONE
    public abstract ChapterProgressDao chapterProgressDao();

    //DONE
    public abstract LessonNewProgressDao lessonnewProgressDao();

    //DONE
    public abstract FileProgressDao fileProgressDao();

    //DONE
    public abstract QuizProgressDao quizProgressDao();

    //DONE
    public abstract DeviceManagementDataDao deviceManagementDataDao();

    //DONE
    public abstract DeviceManagementProfileDao deviceManagementProfileDao();
}
