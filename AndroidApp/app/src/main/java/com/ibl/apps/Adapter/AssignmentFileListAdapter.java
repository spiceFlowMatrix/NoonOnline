package com.ibl.apps.Adapter;

import android.app.Activity;
import android.app.Dialog;
import android.content.ActivityNotFoundException;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.graphics.Typeface;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Environment;
import android.support.annotation.NonNull;
import android.support.v4.content.FileProvider;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.webkit.MimeTypeMap;
import android.widget.Toast;

import com.crashlytics.android.Crashlytics;
import com.downloader.Error;
import com.downloader.OnCancelListener;
import com.downloader.OnDownloadListener;
import com.downloader.OnPauseListener;
import com.downloader.OnProgressListener;
import com.downloader.OnStartOrResumeListener;
import com.downloader.PRDownloader;
import com.downloader.Progress;
import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.github.barteksc.pdfviewer.listener.OnLoadCompleteListener;
import com.github.barteksc.pdfviewer.listener.OnPageChangeListener;
import com.github.barteksc.pdfviewer.listener.OnRenderListener;
import com.ibl.apps.Fragment.CourseItemFragment;
import com.ibl.apps.Interface.EncryptDecryptAsyncResponse;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.EncryptDecryptObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.CustomTypefaceSpan;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.Utils.VideoEncryptDecrypt.EncrypterDecryptAlgo;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.AssignmentFileItemLayoutBinding;
import com.ibl.apps.noon.databinding.AssignmentfilesItemLayoutBinding;
import com.ibl.apps.noon.databinding.DialogViewerItemLayoutBinding;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;

import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

import tcking.github.com.giraffeplayer2.DefaultMediaController;
import tcking.github.com.giraffeplayer2.GiraffePlayer;
import tcking.github.com.giraffeplayer2.PlayerListener;
import tcking.github.com.giraffeplayer2.VideoInfo;
import tv.danmaku.ijk.media.player.IjkTimedText;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class AssignmentFileListAdapter extends RecyclerView.Adapter<AssignmentFileListAdapter.MyViewHolder> implements DroidListener {

    List<CoursePriviewObject.Assignmentfiles> list;
    Context ctx;
    UserDetails userDetailsObject;
    private String courseName;
    private String lessonName;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;
    SecretKey key;
    SecretKey keyFromKeydata;
    byte[] keyData;
    byte[] iv;
    AlgorithmParameterSpec paramSpec;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        AssignmentFileItemLayoutBinding assignmentFileItemLayoutBinding;

        public MyViewHolder(AssignmentFileItemLayoutBinding assignmentFileItemLayoutBinding) {
            super(assignmentFileItemLayoutBinding.getRoot());
            this.assignmentFileItemLayoutBinding = assignmentFileItemLayoutBinding;
        }
    }

    public AssignmentFileListAdapter(Context ctx, List<CoursePriviewObject.Assignmentfiles> list, UserDetails userDetailsObject, String courseName, String lessonName) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        this.courseName = courseName;
        this.lessonName = lessonName;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        AssignmentFileItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.assignment_file_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        CoursePriviewObject.Assignmentfiles assignmentfiles = list.get(position);
        try {
            key = new SecretKeySpec(Const.ALGO_SECRATE_KEY_NAME.getBytes(), Const.ALGO_SECRET_KEY_GENERATOR);
            keyData = key.getEncoded();
            keyFromKeydata = new SecretKeySpec(keyData, 0, keyData.length, Const.ALGO_SECRET_KEY_GENERATOR); //if you want to store key bytes to db so its just how to //recreate back key from bytes array
            iv = new byte[Const.IV_LENGTH];
            paramSpec = new IvParameterSpec(iv);

        } catch (Exception e) {
            e.printStackTrace();
        }

        int imageDrawable = R.drawable.ic_file;
        switch (list.get(position).getFiles().getFiletypeid()) {
            case "1":
                imageDrawable = R.drawable.ic_pdf_icon;
                break;
            case "2":
                imageDrawable = R.drawable.ic_video;
                break;
            case "3":
                imageDrawable = R.drawable.ic_photo;
                break;
            case "6":
                imageDrawable = R.drawable.ic_excel;
                break;
            case "7":
                imageDrawable = R.drawable.ic_docs;
                break;

        }
        holder.assignmentFileItemLayoutBinding.assignmentImgIcon.setImageResource(imageDrawable);
        holder.assignmentFileItemLayoutBinding.docLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String EncryptFileName = assignmentfiles.getFiles().getFilename();
                //String filetype = assignmentfiles.getFiles().getFiletypename();
                String fileName = assignmentfiles.getFiles().getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);
                String str = assignmentfiles.getFiles().getId() + "_" + assignmentfiles.getFiles().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + assignmentfiles.getFiles().getFiletypename() + Const.extension;
                File file = new File(Const.destPath + "/" + fileName);
                switch (list.get(position).getFiles().getFiletypeid()) {

                    case "1":
                        //PDF
                        final Dialog dialog = new Dialog(ctx, android.R.style.Theme_Translucent_NoTitleBar);
                        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                        dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                        dialog.setCancelable(true);
                        DialogViewerItemLayoutBinding dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(ctx), R.layout.dialog_viewer_item_layout, null, false);
                        dialog.setContentView(dialogViewerItemLayoutBinding.getRoot());
                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(courseName);
                        dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(lessonName);
                        openPdf(position, dialogViewerItemLayoutBinding, dialog);
                        break;
                    case "2":
                        //
                        final Dialog dialogVideo = new Dialog(ctx, android.R.style.Theme_Translucent_NoTitleBar);
                        dialogVideo.requestWindowFeature(Window.FEATURE_NO_TITLE);
                        dialogVideo.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                        dialogVideo.setCancelable(true);
                        DialogViewerItemLayoutBinding dialogVideoItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(ctx), R.layout.dialog_viewer_item_layout, null, false);
                        dialogVideo.setContentView(dialogVideoItemLayoutBinding.getRoot());
                        dialogVideoItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.GONE);
                        dialogVideoItemLayoutBinding.videoViewLayout.videoViewLay.setVisibility(View.VISIBLE);
                        dialogVideoItemLayoutBinding.videoViewLayout.videoView.setVisibility(View.VISIBLE);
                        openVideo(position, dialogVideoItemLayoutBinding, dialogVideo);
                        break;
                    case "3":
                        //Image
                        final Dialog dialogImage = new Dialog(ctx, android.R.style.Theme_Translucent_NoTitleBar);
                        dialogImage.requestWindowFeature(Window.FEATURE_NO_TITLE);
                        //dialogimage.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
                        dialogImage.setCancelable(true);
                        AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(ctx), R.layout.assignmentfiles_item_layout, null, false);
                        dialogImage.setContentView(assignmentfilesItemLayoutBinding.getRoot());
                        openImage(assignmentfiles, dialogImage, assignmentfilesItemLayoutBinding);

                        break;
                    case "6":
                        OpenAnyFile(file, str, list.get(position).getFiles().getFiletypeid(), assignmentfiles);
                        break;
                    case "7":
                        OpenAnyFile(file, str, list.get(position).getFiles().getFiletypeid(), assignmentfiles);
                        break;
                    case "8":
                        OpenAnyFile(file, str, list.get(position).getFiles().getFiletypeid(), assignmentfiles);
                        break;

                }

            }
        });
    }

    private void openVideo(int position, DialogViewerItemLayoutBinding dialogVideoItemLayoutBinding, Dialog dialogVideo) {
        String yourFilePath = ctx.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath();
        String EncryptFileName = list.get(position).getFiles().getFilename();
        String filetype = list.get(position).getFiles().getFiletypename();
        String str = list.get(position).getFiles().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
        String selectedVideoPath = Const.destPath + str;
        File outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
        String encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str;
        String decryptpath = null;
        try {
            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (NoSuchPaddingException e) {
            e.printStackTrace();
        } catch (InvalidKeyException e) {
            e.printStackTrace();
        } catch (InvalidAlgorithmParameterException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        PRDownloader.download(decryptpath, String.valueOf(Environment.getExternalStorageDirectory()), Const.dir_fileName + Const.VIDEOextension)
                .build()
                .start(new OnDownloadListener() {
                    @Override
                    public void onDownloadComplete() {
                        String path = Environment.getExternalStorageDirectory() + File.separator + Const.dir_fileName + Const.VIDEOextension;
                        dialogVideoItemLayoutBinding.videoViewLayout.videoView.getVideoInfo()
                                .setAspectRatio(VideoInfo.AR_ASPECT_WRAP_CONTENT)
                                .setTitle(courseName) //config title
                                .setSubTitle(lessonName) //config subtitle
                                .setShowTopBar(true);
                        dialogVideoItemLayoutBinding.videoViewLayout.videoView.setVideoPath(path);
                        dialogVideoItemLayoutBinding.videoViewLayout.videoView.getPlayer().start();
                        dialogVideoItemLayoutBinding.videoViewLayout.appvideoloadingLay.setVisibility(View.GONE);
                        DefaultMediaController defaultMediaController=new DefaultMediaController(ctx);
                        dialogVideo.show();
                        dialogVideoItemLayoutBinding.videoViewLayout.videoView.setPlayerListener(new PlayerListener() {
                            @Override
                            public void onPrepared(GiraffePlayer giraffePlayer) {

                            }

                            @Override
                            public void onBufferingUpdate(GiraffePlayer giraffePlayer, int percent) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===02");
                            }

                            @Override
                            public boolean onInfo(GiraffePlayer giraffePlayer, int what, int extra) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===03");
                                return false;
                            }

                            @Override
                            public void onCompletion(GiraffePlayer giraffePlayer) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===04" +giraffePlayer.getCurrentPosition());

                                try {
                                    giraffePlayer.stop();

                                } catch (Exception e) {
                                    e.printStackTrace();
                                }
                            }

                            @Override
                            public void onSeekComplete(GiraffePlayer giraffePlayer) {
                                //  Log.e(Const.LOG_NOON_TAG, "===>>==percent===05" + giraffePlayer.getCurrentPosition());
                            }

                            @Override
                            public boolean onError(GiraffePlayer giraffePlayer, int what, int extra) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===06");
                                return false;
                            }

                            @Override
                            public void onPause(GiraffePlayer giraffePlayer) {

                            }

                            @Override
                            public void onRelease(GiraffePlayer giraffePlayer) {

                            }

                            @Override
                            public void onStart(GiraffePlayer giraffePlayer) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===07");

                            }

                            @Override
                            public void onTargetStateChange(int oldState, int newState) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===08");
                            }

                            @Override
                            public void onCurrentStateChange(int oldState, int newState) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===09");
                            }

                            @Override
                            public void onDisplayModelChange(int oldModel, int newModel) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===010");
                            }

                            @Override
                            public void onPreparing(GiraffePlayer giraffePlayer) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===011");
                            }

                            @Override
                            public void onTimedText(GiraffePlayer giraffePlayer, IjkTimedText text) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===012" + text);
                            }

                            @Override
                            public void onLazyLoadProgress(GiraffePlayer giraffePlayer, int progress) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===013");
                            }

                            @Override
                            public void onLazyLoadError(GiraffePlayer giraffePlayer, String message) {
                                //Log.e(Const.LOG_NOON_TAG, "===>>==percent===014");

                            }

                        }, new DefaultMediaController.Onbackclick() {
                            @Override
                            public void backperfome() {
                                dialogVideo.dismiss();
                            }

                            @Override
                            public void fullscreenperfome() {

                            }
                        });
                    }

                    @Override
                    public void onError(Error error) {

                    }
                });

    }

    public class DownloadAssignmentStatusTask extends AsyncTask<String, String, String> {

        String fileURL;

        private CoursePriviewObject.Assignmentfiles model;
        private Dialog dialog;
        private AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding;
        int downloadId = 0;
        long progressval = 0;
        Context context;
        String encrypted_path;
        String decryptpath = "";

        public DownloadAssignmentStatusTask(Context context, String url, CoursePriviewObject.Assignmentfiles model, Dialog dialog, AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding) {
            this.context = context;
            this.fileURL = url;
            this.model = model;
            this.dialog = dialog;
            this.assignmentfilesItemLayoutBinding = assignmentfilesItemLayoutBinding;
        }


//        @Override
//        protected void onPreExecute() {
//
//            mProgressDialog = new ProgressDialog(context);
//            mProgressDialog.setMessage("Downloading file…");
//            mProgressDialog.setIndeterminate(false);
//            mProgressDialog.setMax(100);
//            mProgressDialog.setProgressStyle(ProgressDialog.STYLE_HORIZONTAL);
//            mProgressDialog.setCancelable(true);
//            mProgressDialog.show();
//            super.onPreExecute();
//
//        }

        @Override
        protected String doInBackground(String... strings) {
            String userIdSlash = "";

            String EncryptFileName = model.getFiles().getFilename();
            String filetype = model.getFiles().getFiletypename();
            String fileName = model.getFiles().getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);
            String downloadFilePath = Const.destPath + userIdSlash + fileName;

            try {

                String str = model.getFiles().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
                PRDownloader.cleanUp(1);

                downloadId = PRDownloader.download(fileURL, Const.destPath + userIdSlash, fileName)
                        .build()
                        .setOnStartOrResumeListener(new OnStartOrResumeListener() {
                            @Override
                            public void onStartOrResume() {
                                // Toast.makeText(AssignmentDetailActivity.this, AssignmentDetailActivity.this.getString(R.string.downloading) + model.get(), Toast.LENGTH_SHORT).show();
                            }
                        })
                        .setOnPauseListener(new OnPauseListener() {
                            @Override
                            public void onPause() {

                            }
                        })
                        .setOnCancelListener(new OnCancelListener() {
                            @Override
                            public void onCancel() {

                            }
                        })
                        .setOnProgressListener(new OnProgressListener() {
                            @Override
                            public void onProgress(Progress progress) {
                                progressval = (progress.currentBytes * 100) / progress.totalBytes;
                                // mProgressDialog.setProgress((int) progressval);
                                //Log.e("-------------", "==== 4444444444 =====" + progressval);
                            }
                        })
                        .start(new OnDownloadListener() {
                            @Override
                            public void onDownloadComplete() {


                                EncryptDecryptObject encryptDecryptObject = new EncryptDecryptObject();
                                encryptDecryptObject.setSelectedVideoPath(downloadFilePath);
                                encryptDecryptObject.setFilename(str);

                                new SetEncryptDecryptTask(new EncryptDecryptAsyncResponse() {
                                    @Override
                                    public EncryptDecryptObject getEncryptDecryptObjects(EncryptDecryptObject encryptDecryptObjects) {

                                        try {

                                            String selectedVideoPath = encryptDecryptObject.getSelectedVideoPath();
                                            String filename = encryptDecryptObject.getFilename();
                                            File outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
                                            encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename;
                                            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);

                                            dialog.show();
                                            GlideApp.with(context)
                                                    .load(decryptpath)
                                                    .error(R.drawable.ic_no_image_found)
                                                    .into(assignmentfilesItemLayoutBinding.quizImage);
                                            //openDialogViewer(context, decryptpath, model);
                                            //mProgressDialog.dismiss();


                                        } catch (NoSuchAlgorithmException e) {
                                            Toast.makeText(context, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (NoSuchPaddingException e) {
                                            Toast.makeText(context, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidKeyException e) {
                                            Toast.makeText(context, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidAlgorithmParameterException e) {
                                            Toast.makeText(context, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (IOException e) {
                                            if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                                                showNoSpaceAlert(context);
                                            }
                                            Crashlytics.log(Log.ERROR, context.getString(R.string.app_name), e.getMessage());
                                            e.printStackTrace();
                                        }

                                        return null;
                                    }
                                }, encryptDecryptObject).execute();
                            }

                            @Override
                            public void onError(Error error) {
                                if (error.isServerError()) {
                                    Log.e("downloaderror---", "onError: " + error);
                                    //Toast.makeText(ctx, "=== DOWNLAOD ServerError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isConnectionError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ConnectionError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isENOSPCError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ENOSPCError (No space left on device)===", Toast.LENGTH_SHORT).show();
                                    showNoSpaceAlert(ctx);
                                }
                                //hideDialog();
                                Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), error.getError());
                            }
                        });

            } catch (Exception e) {

            }
            return null;
        }

        @Override
        protected void onPostExecute(String unused) {
            // mProgressDialog.dismiss();
        }
    }

    public class SetEncryptDecryptTask extends AsyncTask<Void, Void, EncryptDecryptObject> {

        EncryptDecryptObject encryptDecryptObject;
        EncryptDecryptAsyncResponse delegate = null;

        public SetEncryptDecryptTask(EncryptDecryptAsyncResponse delegate, EncryptDecryptObject encryptDecryptObject) {
            this.encryptDecryptObject = encryptDecryptObject;
            this.delegate = delegate;
        }

        @Override

        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected EncryptDecryptObject doInBackground(Void... params) {

            try {
                String selectedVideoPath = encryptDecryptObject.getSelectedVideoPath();
                String filename = encryptDecryptObject.getFilename();
                File inFile = new File(selectedVideoPath);
                File outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename);
                EncrypterDecryptAlgo.encrypt(key, paramSpec, new FileInputStream(inFile), new FileOutputStream(outFile));
                inFile.delete();

            } catch (NoSuchAlgorithmException e) {
                ((Activity) ctx).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (NoSuchPaddingException e) {
                ((Activity) ctx).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (InvalidKeyException e) {
                ((Activity) ctx).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (InvalidAlgorithmParameterException e) {
                ((Activity) ctx).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (IOException e) {
                //encryptArray = new ArrayList<>();
                //downloadArray = new ArrayList<>();
                if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                    showNoSpaceAlert(ctx);
                }
                Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                e.printStackTrace();
            }
            return encryptDecryptObject;
        }

        @Override
        protected void onPostExecute(EncryptDecryptObject encryptDecryptObject) {
            delegate.getEncryptDecryptObjects(encryptDecryptObject);
        }

    }

    public static void showNoSpaceAlert(Context activity) {

        ((Activity) activity).runOnUiThread(new Runnable() {
            public void run() {
                try {

                    SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.error_no_space));
                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    builder.setTitle(activity.getResources().getString(R.string.validation_warning));
                    builder.setMessage(message)
                            .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int which) {
                                    // Yes button clicked, do something
                                    dialog.dismiss();
                                }
                            });

                    builder.show();
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    private void openImage(CoursePriviewObject.Assignmentfiles assignmentfiles, Dialog dialogImage, AssignmentfilesItemLayoutBinding assignmentfilesItemLayoutBinding) {
        new DownloadAssignmentStatusTask(ctx, assignmentfiles.getFiles().getUrl(), assignmentfiles, dialogImage, assignmentfilesItemLayoutBinding).execute();
    }

    public void OpenAnyFile(File file, String str, String filetypeid, CoursePriviewObject.Assignmentfiles assignmentfiles) {
        String selectpath = String.valueOf(file);
        File outFile = new File(selectpath.substring(0, selectpath.lastIndexOf("/")) + "/" + str);
        String encrypted_path = selectpath.substring(0, selectpath.lastIndexOf("/")) + "/" + str;
        String decryptpath = null;
        try {
            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);

        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (NoSuchPaddingException e) {
            e.printStackTrace();
        } catch (InvalidKeyException e) {
            e.printStackTrace();
        } catch (InvalidAlgorithmParameterException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        String[] extension = selectpath.split("\\.");
        for (int i = 0; i < extension.length; i++) {
            Log.e("extension", "OpenAnyFile: " + extension[i]);
        }
        String ext = "." + extension[2];
//        Log.e(extension, "OpenAnyFile: " + extension);

        if (filetypeid.equals("6") || filetypeid.equals("7") || filetypeid.equals("8")) {
            new DownloadDocFile(decryptpath, selectpath, ext).execute();
        }
//        } else if (filetypeid.equals("7")) {
//            new DownloadDocFile(decryptpath, selectpath,extension).execute();
//        } else if (filetypeid.equals("8")) {
//            new DownloadPPTFile(decryptpath, selectpath).execute();
//        }

    }

    public static String getMimeType(@NonNull File file, String selectedFilePath) {
        String type = null;
        int fileType = -1;
        final String url = file.toString();
        final String extension = MimeTypeMap.getFileExtensionFromUrl(url);
        if (extension != null) {
            type = MimeTypeMap.getSingleton().getMimeTypeFromExtension(extension.toLowerCase());
        }

        return type;
    }

    private void openPdf(int position, DialogViewerItemLayoutBinding dialogViewerItemLayoutBinding, Dialog dialog) {

        String yourFilePath = ctx.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath();
        String EncryptFileName = list.get(position).getFiles().getFilename();
        String filetype = list.get(position).getFiles().getFiletypename();
        String str = list.get(position).getFiles().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
        String selectedVideoPath = Const.destPath + str;
        File outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
        String encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str;
        String decryptpath = null;
        try {
            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (NoSuchPaddingException e) {
            e.printStackTrace();
        } catch (InvalidKeyException e) {
            e.printStackTrace();
        } catch (InvalidAlgorithmParameterException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        Log.d("file_____", String.valueOf(decryptpath));
        try {
            PRDownloader.download(decryptpath, yourFilePath, Const.dir_fileName + Const.PDFextension).build().start(new OnDownloadListener() {
                @Override
                public void onDownloadComplete() {
                    dialog.show();
                    Log.e("Dialog", "-----");
                    File file = new File(ctx.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath() + File.separator + Const.dir_fileName + Const.PDFextension);
                    Log.e("Dialog", "--FileData---" + file);
                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fromFile(file)
                            .enableSwipe(true)
                            .enableDoubletap(true)
                            .defaultPage(0)
                            .onLoad(new OnLoadCompleteListener() {
                                @Override
                                public void loadComplete(int i) {
                                    dialogViewerItemLayoutBinding.pdfViewLayout.progressDialogLay.placeholder.setVisibility(View.GONE);
                                }
                            })
                            .onPageChange(new OnPageChangeListener() {
                                @Override
                                public void onPageChanged(int page, int pageCount) {
                                    dialogViewerItemLayoutBinding.pdfViewLayout.txtPageCount.setText(ctx.getString(R.string.page) + " " + (page + 1) + " " + ctx.getString(R.string.of) + "  " + pageCount);
                                    int countper = (int) ((page + 1) * 100 / pageCount);
                                    dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setProgress(countper);
                                           /* if (currProgress[0] < countper) {
                                                new CourseItemFragment.ProgressTask(lessonID, String.valueOf(countper), position, quizID, fileid).execute();
                                                currProgress[0] = countper;
                                                newcurrantProgress = countper;
                                            }

                                            TotalPDFpage = pageCount;
                                            SelectPDFpage = page;*/
                                }
                            })
                            .enableAnnotationRendering(true)
                            .swipeHorizontal(true)
                            .onRender(new OnRenderListener() {
                                @Override
                                public void onInitiallyRendered(int i) {
                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fitToWidth(dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.getCurrentPage());
                                }
                            })
                            .enableAntialiasing(true)
                            .password(null)
                            .load();
                }

                @Override
                public void onError(Error error) {
                    //Log.e(Const.LOG_NOON_TAG, "0000" + "NO Space ----" + error.toString());
                }
            });
        } catch (Exception e) {
            e.printStackTrace();

        }

//                dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(CourseName);
//                dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(LessonName);
        dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialog.dismiss();
            }
        });

    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }

    public static boolean isNetworkAvailable(Context context) {
        return isNetworkConnected;
    }

    @Override
    public void onInternetConnectivityChanged(boolean isConnected) {
        if (isConnected) {
            isNetworkConnected = true;
        } else {
            isNetworkConnected = false;
        }
    }

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }

    public static void showNetworkAlert(Context activity) {
        try {
            SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.validation_Connect_internet));
            android.support.v7.app.AlertDialog.Builder builder = new android.support.v7.app.AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                        }
                    });

            builder.show();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private class DownloadFile extends AsyncTask<Void, Void, String> {
        private String decryptpath;
        private String selectpath;
        private int downloadId;

        public DownloadFile(String decryptpath, String selectpath) {

            this.decryptpath = decryptpath;
            this.selectpath = selectpath;
        }

        @Override
        protected String doInBackground(Void... voids) {
            Log.d("xldownload---", "doInBackground: " + decryptpath);
            downloadId = PRDownloader.download(decryptpath, String.valueOf(Environment.getExternalStorageDirectory()), Const.dir_fileName + ".xlsx")
                    .build()
                    .start(new OnDownloadListener() {
                        @Override
                        public void onDownloadComplete() {
                            Log.d("xldownload----", "file_onDownloadComplete: " + downloadId);
                            MimeTypeMap myMime = MimeTypeMap.getSingleton();
                            Intent newIntent = new Intent(Intent.ACTION_VIEW);
                            String path = Environment.getExternalStorageDirectory() + File.separator + Const.dir_fileName + ".xlsx";
                            String mimeType = getMimeType(new File(path), path);

                            Log.e("mimeType", "onDownloadComplete: " + mimeType);
                            Log.e("mimeType", "onDownloadComplete: " + Uri.parse(path));

                            Uri apkURI = FileProvider.getUriForFile(
                                    ctx,
                                    ctx.getApplicationContext()
                                            .getPackageName() + ".provider", new File(path));
                            newIntent.setDataAndType(Uri.parse(path), "*/*");

                            newIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);

                            try {
                                ctx.startActivity(newIntent);
                            } catch (ActivityNotFoundException e) {
                                Toast.makeText(ctx, "No handler for this type of file.", Toast.LENGTH_LONG).show();
                            }
                        }

                        @Override
                        public void onError(Error error) {

                        }
                    });
            return null;
        }
    }

    private class DownloadDocFile extends AsyncTask<Void, Void, String> {
        private String decryptpath;
        private String selectpath;
        private String extension;
        private int downloadId;

        public DownloadDocFile(String decryptpath, String selectpath, String extension) {

            this.decryptpath = decryptpath;
            this.selectpath = selectpath;
            this.extension = extension;
        }

        @Override
        protected String doInBackground(Void... voids) {
            Log.d("xldownload---", "doInBackground: " + decryptpath);
            downloadId = PRDownloader.download(decryptpath, String.valueOf(Environment.getExternalStorageDirectory()), Const.dir_fileName + extension)
                    .build()
                    .start(new OnDownloadListener() {
                        @Override
                        public void onDownloadComplete() {
                            Log.d("xldownload----", "file_onDownloadComplete: " + downloadId);
                            MimeTypeMap myMime = MimeTypeMap.getSingleton();
                            Intent newIntent = new Intent(Intent.ACTION_VIEW);
                            String path = Environment.getExternalStorageDirectory() + File.separator + Const.dir_fileName + extension;
                            Log.e("path", "onDownloadComplete: " + path);
                            String mimeType = getMimeType(new File(path), path);

                            Log.e("mimeType", "onDownloadComplete: " + mimeType);
                            Log.e("mimeType", "onDownloadComplete: " + Uri.parse(path));

                            Uri apkURI = FileProvider.getUriForFile(
                                    ctx,
                                    ctx.getApplicationContext()
                                            .getPackageName() + ".provider", new File(path));
                            newIntent.setDataAndType(Uri.parse(path), "*/*");

                            newIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);

                            try {
                                ctx.startActivity(newIntent);
                            } catch (ActivityNotFoundException e) {
                                Toast.makeText(ctx, "No handler for this type of file.", Toast.LENGTH_LONG).show();
                            }
                        }

                        @Override
                        public void onError(Error error) {

                        }
                    });
            return null;
        }
    }

    private class DownloadPPTFile extends AsyncTask<Void, Void, String> {
        private String decryptpath;
        private String selectpath;
        private int downloadId;

        public DownloadPPTFile(String decryptpath, String selectpath) {

            this.decryptpath = decryptpath;
            this.selectpath = selectpath;
        }

        @Override
        protected String doInBackground(Void... voids) {
            Log.d("xldownload---", "doInBackground: " + decryptpath);
            downloadId = PRDownloader.download(decryptpath, String.valueOf(Environment.getExternalStorageDirectory()), Const.dir_fileName + ".pptx")
                    .build()
                    .start(new OnDownloadListener() {
                        @Override
                        public void onDownloadComplete() {
                            Log.d("xldownload----", "file_onDownloadComplete: " + downloadId);
                            MimeTypeMap myMime = MimeTypeMap.getSingleton();
                            Intent newIntent = new Intent(Intent.ACTION_VIEW);
                            String path = Environment.getExternalStorageDirectory() + File.separator + Const.dir_fileName + ".pptx";
                            String mimeType = getMimeType(new File(path), path);

                            Log.e("mimeType", "onDownloadComplete: " + mimeType);
                            Log.e("mimeType", "onDownloadComplete: " + Uri.parse(path));

                            Uri apkURI = FileProvider.getUriForFile(
                                    ctx,
                                    ctx.getApplicationContext()
                                            .getPackageName() + ".provider", new File(path));
                            newIntent.setDataAndType(Uri.parse(path), "*/*");

                            newIntent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);

                            try {
                                ctx.startActivity(newIntent);
                            } catch (ActivityNotFoundException e) {
                                Toast.makeText(ctx, "No handler for this type of file.", Toast.LENGTH_LONG).show();
                            }
                        }

                        @Override
                        public void onError(Error error) {

                        }
                    });
            return null;
        }
    }
}