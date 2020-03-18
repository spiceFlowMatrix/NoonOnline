package com.ibl.apps.Adapter;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.databinding.DataBindingUtil;
import android.media.MediaPlayer;
import android.net.Uri;
import android.os.AsyncTask;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
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
import com.ibl.apps.Interface.EncryptDecryptAsyncResponse;
import com.ibl.apps.Model.EncryptDecryptObject;
import com.ibl.apps.Model.feedback.FileData;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.Utils.VideoEncryptDecrypt.EncrypterDecryptAlgo;
import com.ibl.apps.noon.AssignmentDetailActivity;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.ImageUploadListBinding;

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

import fr.maxcom.libmedia.Licensing;

public class ImageUploadAdapterList extends RecyclerView.Adapter<ImageUploadAdapterList.MyViewHolder> implements DroidListener {
    public static boolean isNetworkConnected = false;
    private final DroidNet mDroidNet;
    byte[] iv;
    byte[] keyData;
    private List<FileData> images;
    private int flagid;
    private Context context;
    private AlgorithmParameterSpec paramSpec;
    private SecretKey key;
    private SecretKey keyFromKeydata;


    public ImageUploadAdapterList(List<FileData> images, int flagid) {
        this.images = images;
        this.flagid = flagid;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);

        Licensing.allow(NoonApplication.getContext());
    }

    public static boolean isNetworkAvailable(Context context) {
        return isNetworkConnected;
    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int i) {
        ImageUploadListBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()), R.layout.image_upload_list, parent, false);
        context = parent.getContext();
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        FileData model = images.get(position);

        try {
            key = new SecretKeySpec(Const.ALGO_SECRATE_KEY_NAME.getBytes(), Const.ALGO_SECRET_KEY_GENERATOR);
            keyData = key.getEncoded();
            keyFromKeydata = new SecretKeySpec(keyData, 0, keyData.length, Const.ALGO_SECRET_KEY_GENERATOR); //if you want to store key bytes to db so its just how to //recreate back key from bytes array
            iv = new byte[Const.IV_LENGTH];
            paramSpec = new IvParameterSpec(iv);

        } catch (Exception e) {
            e.printStackTrace();
        }

        if (model.getFiles().getFilename() != null) {
            String filename = model.getFiles().getFilename();
            String[] strings = filename.split("\\.");
            String extension = strings[1];

            new DownloadStatusTask(context, model.getFiles().getUrl(), extension, holder.binding, model, Integer.parseInt(String.valueOf(model.getFiles().getFiletypeid()))).execute();
        }
    }

    @SuppressLint("StaticFieldLeak")
    public class DownloadStatusTask extends AsyncTask<String, String, String> {
        int downloadId = 0;
        long progressval = 0;
        Context context;
        private String url;
        String encrypted_path;
        String decryptpath = "";
        private int type;
        private String extension;
        private ImageUploadListBinding binding;
        private FileData model;


        DownloadStatusTask(Context context, String url, String extension, ImageUploadListBinding binding, FileData model, int type) {
            this.context = context;
            this.url = url;
            this.extension = extension;
            this.binding = binding;
            this.model = model;
            this.type = type;
        }


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
                downloadId = PRDownloader.download(url, Const.destPath, fileName)
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
                                            String selectedVideoPathVideo = encryptDecryptObject.getSelectedVideoPath();
                                            String filenameVideo = encryptDecryptObject.getFilename();
                                            File outFileVideo = new File(selectedVideoPathVideo.substring(0, selectedVideoPathVideo.lastIndexOf("/")) + "/" + str);
                                            encrypted_path = selectedVideoPathVideo.substring(0, selectedVideoPathVideo.lastIndexOf("/")) + "/" + filenameVideo;
                                            decryptpath = EncrypterDecryptAlgo.decrypt(keyFromKeydata, paramSpec, new FileInputStream(outFileVideo), encrypted_path);

                                            switch (type) {
                                                case 2:

                                                    binding.uploadedImg.setVisibility(View.GONE);
                                                    binding.videoView.setVisibility(View.VISIBLE);

                                                    binding.videoView.setVideoURI(Uri.parse(decryptpath));
                                                    binding.videoView.setOnPreparedListener(new MediaPlayer.OnPreparedListener() {
                                                        @Override
                                                        public void onPrepared(MediaPlayer mediaPlayer) {
                                                            mediaPlayer.setLooping(true);
                                                            mediaPlayer.setVolume(0f, 0f);
//                                                            mediaPlayer.setOnInfoListener(new MediaPlayer.OnInfoListener() {
//                                                                @Override
//                                                                public boolean onInfo(MediaPlayer mediaPlayer, int i, int i1) {
//                                                                    if (i == MediaPlayer.MEDIA_INFO_VIDEO_RENDERING_START) {
//                                                                        // video started; hide the placeholder.
//                                                                        binding.placeholderLay.setVisibility(View.GONE);
//                                                                        binding.videoView.setVisibility(View.VISIBLE);
//                                                                        return true;
//                                                                    }
//                                                                    return false;
//                                                                }
//                                                            });
                                                        }
                                                    });
                                                    binding.videoView.setVisibility(View.VISIBLE);
                                                    binding.videoView.requestFocus();
//                                                    binding.videoView.setZOrderOnTop(true);
                                                    binding.videoView.start();
                                                    break;

                                                case 3:
                                                    binding.uploadedImg.setVisibility(View.VISIBLE);
                                                    binding.videoView.setVisibility(View.GONE);

                                                    GlideApp.with(context)
                                                            .load(decryptpath)
                                                            .error(R.drawable.ic_no_image_found)
                                                            .placeholder(R.drawable.ic_no_image_found)
                                                            .into(binding.uploadedImg);

                                                    break;
                                            }

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
                                                AssignmentDetailActivity.showNoSpaceAlert(context);
                                            }
                                            Crashlytics.log(Log.ERROR, context.getString(R.string.app_name), e.getMessage());
                                            e.printStackTrace();
                                        }

                                        return null;
                                    }
                                }, encryptDecryptObject).

                                        execute();
                            }

                            @Override
                            public void onError(Error error) {
                                if (error.isServerError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ServerError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isConnectionError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ConnectionError===", Toast.LENGTH_SHORT).show();
                                } else if (error.isENOSPCError()) {
                                    //Toast.makeText(ctx, "=== DOWNLAOD ENOSPCError (No space left on device)===", Toast.LENGTH_SHORT).show();
                                    AssignmentDetailActivity.showNoSpaceAlert(context);
                                }
                                //hideDialog();
                                Crashlytics.log(Log.ERROR, context.getString(R.string.app_name), error.getError());
                            }
                        });


            } catch (Exception e) {
                e.printStackTrace();
            }

            return null;
        }

        @Override
        protected void onPostExecute(String unused) {
            // mProgressDialog.dismiss();
        }
    }

    @SuppressLint("StaticFieldLeak")
    public class SetEncryptDecryptTask extends AsyncTask<Void, Void, EncryptDecryptObject> {

        EncryptDecryptObject encryptDecryptObject;
        EncryptDecryptAsyncResponse delegate = null;

        SetEncryptDecryptTask(EncryptDecryptAsyncResponse delegate, EncryptDecryptObject encryptDecryptObject) {
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
                ((Activity) context).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(context, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (NoSuchPaddingException e) {
                ((Activity) context).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(context, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (InvalidKeyException e) {
                ((Activity) context).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(context, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (InvalidAlgorithmParameterException e) {
                ((Activity) context).runOnUiThread(new Runnable() {
                    public void run() {
                        Toast.makeText(context, e.getMessage(), Toast.LENGTH_SHORT).show();
                    }
                });
                e.printStackTrace();
            } catch (IOException e) {
                //encryptArray = new ArrayList<>();
                //downloadArray = new ArrayList<>();
                if (e.getMessage().contains("ENOSPC") || e.getMessage().contains("No space left on device")) {
                    AssignmentDetailActivity.showNoSpaceAlert(context);
                }
                Crashlytics.log(Log.ERROR, context.getString(R.string.app_name), e.getMessage());
                e.printStackTrace();
            }
            return encryptDecryptObject;
        }

        @Override
        protected void onPostExecute(EncryptDecryptObject encryptDecryptObject) {
            delegate.getEncryptDecryptObjects(encryptDecryptObject);
        }

    }

    @Override
    public int getItemCount() {
        return images.size();
    }

    @Override
    public void onInternetConnectivityChanged(boolean isConnected) {
        isNetworkConnected = isConnected;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        ImageUploadListBinding binding;

        public MyViewHolder(ImageUploadListBinding imageUploadListBinding) {
            super(imageUploadListBinding.getRoot());
            this.binding = imageUploadListBinding;
        }
    }
}
