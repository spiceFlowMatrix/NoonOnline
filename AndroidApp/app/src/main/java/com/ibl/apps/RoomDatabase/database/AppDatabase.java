package com.ibl.apps.RoomDatabase.database;

import android.arch.persistence.db.SupportSQLiteDatabase;
import android.arch.persistence.room.Database;
import android.arch.persistence.room.Room;
import android.arch.persistence.room.RoomDatabase;
import android.arch.persistence.room.TypeConverters;
import android.arch.persistence.room.migration.Migration;
import android.content.Context;

import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.IntervalTableObject;
import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.Model.LoginObject;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.Model.StatisticsObject;
import com.ibl.apps.RoomDatabase.dao.AuthTokenDao;
import com.ibl.apps.RoomDatabase.dao.ChapterProgressDao;
import com.ibl.apps.RoomDatabase.dao.CourseDao;
import com.ibl.apps.RoomDatabase.dao.CourseDetailsDao;
import com.ibl.apps.RoomDatabase.dao.FileDownloadStatusDao;
import com.ibl.apps.RoomDatabase.dao.FileProgressDao;
import com.ibl.apps.RoomDatabase.dao.GradeProgressDao;
import com.ibl.apps.RoomDatabase.dao.IntervalDao;
import com.ibl.apps.RoomDatabase.dao.LessonNewProgressDao;
import com.ibl.apps.RoomDatabase.dao.LessonProgressDao;
import com.ibl.apps.RoomDatabase.dao.LibraryDao;
import com.ibl.apps.RoomDatabase.dao.LibraryGradeDao;
import com.ibl.apps.RoomDatabase.dao.LoginDao;
import com.ibl.apps.RoomDatabase.dao.QuizAnswerDao;
import com.ibl.apps.RoomDatabase.dao.QuizProgressDao;
import com.ibl.apps.RoomDatabase.dao.QuizUserResultDao;
import com.ibl.apps.RoomDatabase.dao.StatisticsDao;
import com.ibl.apps.RoomDatabase.dao.SyncTimeTrackingDao;
import com.ibl.apps.RoomDatabase.dao.UserDetailDao;
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
        QuizProgress.class
},
        version = 4,
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

    public abstract LessonProgressDao lessonProgressDao();

    public abstract GradeProgressDao gradeProgressDao();

    public abstract FileDownloadStatusDao fileDownloadStatusDao();

    public abstract QuizAnswerDao quizAnswerDao();

    public abstract CourseDao courseDao();

    public abstract CourseDetailsDao courseDetailsDao();

    public abstract LoginDao loginDao();

    public abstract UserDetailDao userDetailDao();

    public abstract StatisticsDao statisticsDao();

    public abstract AuthTokenDao authTokenDao();

    public abstract QuizUserResultDao quizUserResultDao();

    public abstract LibraryDao libraryDao();

    public abstract LibraryGradeDao libraryGradeDao();

    public abstract IntervalDao intervalDao();

    public abstract SyncTimeTrackingDao syncTimeTrackingDao();

    public abstract ChapterProgressDao chapterProgressDao();

    public abstract LessonNewProgressDao lessonnewProgressDao();

    public abstract FileProgressDao fileProgressDao();

    public abstract QuizProgressDao quizProgressDao();
}
