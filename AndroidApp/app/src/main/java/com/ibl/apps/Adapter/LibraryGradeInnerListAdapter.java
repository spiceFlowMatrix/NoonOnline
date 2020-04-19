package com.ibl.apps.Adapter;

import android.app.Activity;
import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.graphics.Typeface;
import android.os.AsyncTask;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.core.content.res.ResourcesCompat;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

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
import com.google.gson.Gson;
import com.ibl.apps.DownloadFileManagement.DownloadFileRepository;
import com.ibl.apps.Interface.EncryptDecryptAsyncResponse;
import com.ibl.apps.Model.EncryptDecryptObject;
import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.SignedUrlObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.DialogViewerItemLayoutBinding;
import com.ibl.apps.noon.databinding.LibraryGradeInnerItemLayoutBinding;
import com.ibl.apps.noon.databinding.LibraryProgressDialogBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.util.VideoEncryptDecrypt.EncrypterDecryptAlgo;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.util.List;

import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class LibraryGradeInnerListAdapter extends RecyclerView.Adapter<LibraryGradeInnerListAdapter.MyViewHolder> implements DroidListener {

    List<LibraryGradeObject.Books> list;
    Context ctx;
    UserDetails userDetailsObject;
    ProgressDialog mProgressDialog;
    String userIdSlash = "";
    File inFile;
    File outFile;
    SecretKey key;
    byte[] keyData;
    byte[] iv;
    String encrypted_path;
    String selectedVideoPath;
    SecretKey keyFromKeydata;
    AlgorithmParameterSpec paramSpec;
    private ProgressDialog progress;
    int TotalPDFpage = 0;
    int SelectPDFpage = 0;
    private CompositeDisposable disposable = new CompositeDisposable();
    public static ApiService apiService;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;
    private TextView tv_per, tv_progress, tv_total_progress;
    private ImageView img_cancle;
    private ProgressBar progressbar;
    private Dialog dialog;
    LibraryProgressDialogBinding libraryProgressDialogBinding;
    private DownloadFileRepository downloadFileRepository;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        LibraryGradeInnerItemLayoutBinding libraryGradeInnerItemLayoutBinding;

        public MyViewHolder(LibraryGradeInnerItemLayoutBinding libraryGradeInnerItemLayoutBinding) {
            super(libraryGradeInnerItemLayoutBinding.getRoot());
            this.libraryGradeInnerItemLayoutBinding = libraryGradeInnerItemLayoutBinding;
        }
    }

    public LibraryGradeInnerListAdapter(Context ctx, List<LibraryGradeObject.Books> list, UserDetails userDetailsObject) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;

        if (userDetailsObject != null) {
            userIdSlash = userDetailsObject.getId() + "/";
        }
        downloadFileRepository = new DownloadFileRepository();
        apiService = ApiClient.getClient(ctx).create(ApiService.class);
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);

        try {
            key = new SecretKeySpec(Const.ALGO_SECRATE_KEY_NAME.getBytes(), Const.ALGO_SECRET_KEY_GENERATOR);
            keyData = key.getEncoded();
            keyFromKeydata = new SecretKeySpec(keyData, 0, keyData.length, Const.ALGO_SECRET_KEY_GENERATOR); //if you want to store key bytes to db so its just how to //recreate back key from bytes array
            iv = new byte[Const.IV_LENGTH];
            paramSpec = new IvParameterSpec(iv);

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        LibraryGradeInnerItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()), R.layout.library_grade_inner_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        LibraryGradeObject.Books model = list.get(position);

        File directory = new File(Const.destPath + userIdSlash);
        if (!directory.exists()) {
            directory.mkdirs();
        }

        if (!TextUtils.isEmpty(model.getTitle())) {
            holder.libraryGradeInnerItemLayoutBinding.txtBookName.setText(model.getTitle());
        }

        if (!TextUtils.isEmpty(model.getAuthor())) {
            holder.libraryGradeInnerItemLayoutBinding.txtBookAuthorName.setText(model.getAuthor());
        }

        if (!TextUtils.isEmpty(model.getSubject())) {
            holder.libraryGradeInnerItemLayoutBinding.txtBookSubjectName.setText(model.getSubject());
        }

        if (isNetworkAvailable(ctx)) {

            GlideApp.with(ctx)
                    .load(model.getBookcoverimage())
                    .placeholder(R.drawable.ic_no_image_found)
                    .error(R.drawable.ic_no_image_found)
                    .dontAnimate()
                    .fitCenter()
                    .into(holder.libraryGradeInnerItemLayoutBinding.libraryBookImage);

            holder.libraryGradeInnerItemLayoutBinding.disableLay.setVisibility(View.GONE);
            holder.libraryGradeInnerItemLayoutBinding.bookInnerCardview.setClickable(true);

        } else {
            GlideApp.with(ctx)
                    .asBitmap()
                    .load(model.getBookGradecoverImageBitmap())
                    .placeholder(R.drawable.ic_no_image_found)
                    .error(R.drawable.ic_no_image_found)
                    .fitCenter()
                    .into(holder.libraryGradeInnerItemLayoutBinding.libraryBookImage);

            try {

                String str = model.getFile().getId() + "_" + model.getFile().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + model.getFile().getFiletypename() + Const.extension;
                File file = new File(Const.destPath + userIdSlash, str);
                if (file.exists()) {
                    holder.libraryGradeInnerItemLayoutBinding.disableLay.setVisibility(View.GONE);
                    holder.libraryGradeInnerItemLayoutBinding.bookInnerCardview.setClickable(true);
                } else {
                    holder.libraryGradeInnerItemLayoutBinding.disableLay.setVisibility(View.VISIBLE);
                    holder.libraryGradeInnerItemLayoutBinding.bookInnerCardview.setClickable(false);
                }
            } catch (Exception e) {
                e.printStackTrace();
            }

        }


        holder.libraryGradeInnerItemLayoutBinding.bookInnerCardview.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                if (model.getFile() != null) {

                    String str = model.getFile().getId() + "_" + model.getFile().getFilename().replaceFirst(".*-(\\w+).*", "$1") + "_" + model.getFile().getFiletypename() + Const.extension;
                    File file = new File(Const.destPath + userIdSlash, str);
                    //Log.e(Const.LOG_NOON_TAG, "======file===000" + file.toString());

                    if (file.exists()) {
                        try {
                            selectedVideoPath = Const.destPath + userIdSlash + str;
                            outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str);
                            encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + str;
                            String decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
                            openDialogViewer(ctx, decryptpath, model);

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

                    } else {
                        if (isNetworkAvailable(ctx)) {
                            callApifetchSignedUrl(model.getFile().getId(), "0", holder, model);
                        }
                    }
                }
            }
        });

        holder.libraryGradeInnerItemLayoutBinding.disableLay.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                showNetworkAlert(ctx);
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

    public void callApifetchSignedUrl(String fileID, String lessionID, MyViewHolder holder, LibraryGradeObject.Books model) {

        //showDialog(ctx.getString(R.string.loading));

        disposable.add(downloadFileRepository.fetchSignedUrl(fileID, lessionID)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<SignedUrlObject>() {
                    @Override
                    public void onSuccess(SignedUrlObject signedUrlObject) {

                        new DownloadStatusTask(ctx,
                                signedUrlObject.getData().getUrl(),
                                holder,
                                model).execute();
                    }

                    @Override
                    public void onError(Throwable e) {

                        try {
                            HttpException error = (HttpException) e;
                            SignedUrlObject signedUrlObject = new Gson().fromJson(error.response().errorBody().string(), SignedUrlObject.class);
                            // Log.e(Const.LOG_NOON_TAG, "==SignedUrl==" + signedUrlObject.getMessage());
                            Toast.makeText(ctx, signedUrlObject.getMessage(), Toast.LENGTH_SHORT).show();
                        } catch (Exception e1) {
                            //Log.e(Const.LOG_NOON_TAG, "==SignedUrl==" + e.getMessage());

                            Toast.makeText(ctx, e1.getMessage(), Toast.LENGTH_SHORT).show();
                            //showError(e);
                        }
                        //hideDialog();
                        Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                    }
                }));
    }

    public class DownloadStatusTask extends AsyncTask<String, String, String> {

        String fileURL;
        MyViewHolder holder;
        LibraryGradeObject.Books model;
        int downloadId = 0;
        long progressval = 0;
        Context context;

        public DownloadStatusTask(Context context, String fileURL, MyViewHolder holder, LibraryGradeObject.Books model) {
            this.fileURL = fileURL;
            this.holder = holder;
            this.model = model;
            this.context = context;
        }

        @Override
        protected void onPreExecute() {

            dialog = new Dialog(context);
            //library_progress_dialog
            dialog.setContentView(R.layout.library_progress_dialog);
            Window window = dialog.getWindow();
            window.setLayout(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
            tv_per = dialog.findViewById(R.id.tv_per);
            tv_progress = dialog.findViewById(R.id.tv_progress);
            progressbar = dialog.findViewById(R.id.progressbar);
            img_cancle = dialog.findViewById(R.id.img_cancle);

//            progressDialogBinding  = DataBindingUtil.setContentView((Activity) context,R.layout.progress_dialog);
            dialog.setCancelable(false);
            dialog.show();
            super.onPreExecute();

        }

        @Override
        protected String doInBackground(String... strings) {


            String EncryptFileName = model.getFile().getFilename();
            String filetype = model.getFile().getFiletypename();
            String fileName = model.getFile().getId() + "_" + EncryptFileName.substring(EncryptFileName.lastIndexOf('-') + 1);
            String downloadFilePath = Const.destPath + userIdSlash + fileName;

            try {

                String str = model.getFile().getId() + "_" + EncryptFileName.replaceFirst(".*-(\\w+).*", "$1") + "_" + filetype + Const.extension;
                PRDownloader.cleanUp(1);

                downloadId = PRDownloader.download(fileURL, Const.destPath + userIdSlash, fileName)
                        .build()
                        .setOnStartOrResumeListener(new OnStartOrResumeListener() {
                            @Override
                            public void onStartOrResume() {
                                Toast.makeText(ctx, ctx.getString(R.string.downloading) + model.getTitle(), Toast.LENGTH_SHORT).show();
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
                                tv_per.setText(String.valueOf(progressval + "%"));
                                tv_progress.setText(String.valueOf(progressval));
                                progressbar.setProgress((int) progressval);
                                oncancleclick(downloadId);
//                                progressval = (progress.currentBytes * 100) / progress.totalBytes;
//                                mProgressDialog.setProgress((int) progressval);
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

                                            encrypted_path = selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename;
                                            String decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFile), encrypted_path);
                                            openDialogViewer(context, decryptpath, model);
                                            mProgressDialog.dismiss();


                                        } catch (NoSuchAlgorithmException e) {
                                            Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (NoSuchPaddingException e) {
                                            Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidKeyException e) {
                                            Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (InvalidAlgorithmParameterException e) {
                                            Toast.makeText(ctx, e.getMessage(), Toast.LENGTH_SHORT).show();
                                            e.printStackTrace();
                                        } catch (IOException e) {
                                            if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                                                showNoSpaceAlert(ctx);
                                            }
                                            Crashlytics.log(Log.ERROR, ctx.getString(R.string.app_name), e.getMessage());
                                            e.printStackTrace();
                                        }

                                        return null;
                                    }
                                }, encryptDecryptObject).execute();
                            }

                            @Override
                            public void onError(Error error) {
                                if (error.isServerError()) {
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

    private void oncancleclick(int downloadId) {
        img_cancle.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                PRDownloader.cancel(downloadId);
                dialog.dismiss();
                Toast.makeText(ctx, "Cancel Downloading", Toast.LENGTH_SHORT).show();
            }
        });
    }

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }

    public static void showNoSpaceAlert(Context activity) {

        ((Activity) activity).runOnUiThread(new Runnable() {
            public void run() {
                try {
                    SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.error_no_space));
                    androidx.appcompat.app.AlertDialog.Builder builder = new androidx.appcompat.app.AlertDialog.Builder(activity);
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
                inFile = new File(selectedVideoPath);
                outFile = new File(selectedVideoPath.substring(0, selectedVideoPath.lastIndexOf("/")) + "/" + filename);
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

    public void openDialogViewer(Context context, String pdffileURI, LibraryGradeObject.Books model) {

        Dialog dialog = new Dialog(context, android.R.style.Theme_Translucent_NoTitleBar);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
        dialog.setCancelable(true);
        DialogViewerItemLayoutBinding dialogViewerItemLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(context), R.layout.dialog_viewer_item_layout, null, false);
        dialog.setContentView(dialogViewerItemLayoutBinding.getRoot());
        dialog.show();

        showDialog(context.getString(R.string.loading));
        dialogViewerItemLayoutBinding.pdfViewLayout.FullScreenImage.setImageResource(R.drawable.ic_fullscreen_exit);
        dialogViewerItemLayoutBinding.videoViewLayout.videoViewLay.setVisibility(View.GONE);
        dialogViewerItemLayoutBinding.quizViewLayout.quizViewer.setVisibility(View.GONE);
        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewLay.setVisibility(View.VISIBLE);
        dialogViewerItemLayoutBinding.imageViewLayout.imageViewer.setVisibility(View.GONE);
        dialogViewerItemLayoutBinding.pdfViewLayout.FullScreenImage.setVisibility(View.GONE);


        String yourFilePath = context.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath();
        try {

            // For FileData Delete from dataDir
            File file1 = new File(yourFilePath);
            String[] files;
            files = file1.list();
            for (int i = 0; i < files.length; i++) {
                File myFile = new File(file1, files[i]);
                myFile.delete();
            }

            // For FileData Delete from catch (Clear catch)
            File dir = context.getCacheDir();
            deleteDir(dir);
        } catch (Exception e) {
            e.printStackTrace();
        }


        PRDownloader.download(pdffileURI, yourFilePath, Const.dir_fileName + Const.PDFextension).build().start(new OnDownloadListener() {
            @Override
            public void onDownloadComplete() {

                File file = new File(context.getDir(Const.dir_fileName, Context.MODE_PRIVATE).getAbsolutePath() + File.separator + Const.dir_fileName + Const.PDFextension);
                dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fromFile(file)
                        .enableSwipe(true) // allows to block changing pages using swipe
                        .enableDoubletap(true)
                        .defaultPage(0)
                        .onLoad(new OnLoadCompleteListener() {
                            @Override
                            public void loadComplete(int i) {
                                dialogViewerItemLayoutBinding.pdfViewLayout.progressDialogLay.placeholder.setVisibility(View.GONE);
                                hideDialog();
                                //Log.e(Const.LOG_NOON_TAG, "=====loadComplete==11==");
                            }
                        })
                        .onPageChange(new OnPageChangeListener() {
                            @Override
                            public void onPageChanged(int page, int pageCount) {

                                int countper = (int) ((page + 1) * 100 / pageCount);
                                TotalPDFpage = pageCount;
                                SelectPDFpage = page;
                                dialogViewerItemLayoutBinding.pdfViewLayout.progressBarLayout.progressBar.setVisibility(View.GONE);
                                if (page == 0) {
                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setText(R.string.previous_lesson);
                                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewNext.setText(R.string.greterThen);
                                } else {

                                    if (pageCount == (page + 1)) {
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setText(R.string.lessThen);
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewNext.setText(R.string.Finish);
                                    } else {
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setText(R.string.lessThen);
                                        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewNext.setText(R.string.greterThen);
                                    }
                                }

                            }
                        })
                        .enableAnnotationRendering(true)
                        .onRender(new OnRenderListener() {
                            @Override
                            public void onInitiallyRendered(int i) {
                                //dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.fitToWidth(fragmentCourseItemLayoutBinding.pdfViewLayout.pdfViewPager.getCurrentPage());
                            }
                        })
                        .swipeHorizontal(true)
                        .enableAntialiasing(true)
                        .load();
            }

            @Override
            public void onError(Error error) {

                //Log.e(Const.LOG_NOON_TAG, "0000" + "NO Space ----" + error.toString());
            }
        });


        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewNext.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                int nextSelectPDFpage = (SelectPDFpage + 1);
                if (nextSelectPDFpage == TotalPDFpage) {
                    Toast.makeText(ctx, R.string.no_page_found, Toast.LENGTH_LONG).show();
                } else {
                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.jumpTo(nextSelectPDFpage);
                }
            }
        });
        dialogViewerItemLayoutBinding.pdfViewLayout.pdfviewPrivious.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                if (SelectPDFpage == 0) {
                    Toast.makeText(ctx, R.string.no_page_found, Toast.LENGTH_LONG).show();

                } else {
                    dialogViewerItemLayoutBinding.pdfViewLayout.pdfViewPager.jumpTo(SelectPDFpage - 1);
                }
            }
        });
        dialogViewerItemLayoutBinding.pdfViewLayout.pdfCourseName.setText(model.getTitle());
        dialogViewerItemLayoutBinding.pdfViewLayout.pdflessonName.setText(model.getAuthor());
        dialogViewerItemLayoutBinding.pdfViewLayout.backPdfButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                dialog.dismiss();
            }
        });
    }

    protected void showDialog(String message) {
        if (progress == null) {
            progress = new ProgressDialog(ctx);
            progress.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            progress.setCancelable(true);
            progress.setCanceledOnTouchOutside(false);
        }
        progress.setMessage(message);
        progress.show();
    }

    protected void hideDialog() {
        if (progress != null && progress.isShowing()) {
            progress.dismiss();
        }
    }

    public boolean deleteDir(File dir) {
        if (dir != null && dir.isDirectory()) {
            String[] children = dir.list();
            for (int i = 0; i < children.length; i++) {
                boolean success = deleteDir(new File(dir, children[i]));
                if (!success) {
                    return false;
                }
            }
            return dir.delete();
        } else if (dir != null && dir.isFile()) {
            return dir.delete();
        } else {
            return false;
        }
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

    public static void showNetworkAlert(Context activity) {
        try {
            SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.validation_Connect_internet));
            androidx.appcompat.app.AlertDialog.Builder builder = new androidx.appcompat.app.AlertDialog.Builder(activity);
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
}
