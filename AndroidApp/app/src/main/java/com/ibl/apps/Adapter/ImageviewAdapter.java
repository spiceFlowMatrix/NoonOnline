package com.ibl.apps.Adapter;

import android.app.Dialog;
import android.content.Context;
import androidx.databinding.DataBindingUtil;
import androidx.annotation.NonNull;
import androidx.viewpager.widget.PagerAdapter;

import android.net.Uri;
import android.os.Environment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.widget.FrameLayout;
import android.widget.ImageView;

import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.ImageviewpagerLayoutBinding;

import java.io.File;
import java.util.ArrayList;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class ImageviewAdapter extends PagerAdapter {
    ArrayList<QuizMainObject.Images> list;
    Context ctx;
    int flag = 0;

    public ImageviewAdapter(Context ctx, ArrayList<QuizMainObject.Images> list, int flag) {
        this.list = list;
        this.ctx = ctx;
        this.flag = flag;
    }


    @Override
    public Object instantiateItem(ViewGroup container, int position) {
        LayoutInflater inflater = (LayoutInflater) ctx.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View itemView = inflater.inflate(R.layout.image_item_layout, container, false);

        QuizMainObject.Images model = list.get(position);

        FrameLayout frameImage = (FrameLayout) itemView.findViewById(R.id.frameImage);
        ImageView quizImage = (ImageView) itemView.findViewById(R.id.quizImage);
        ImageView quizprvImage = (ImageView) itemView.findViewById(R.id.quizprvImage);
        ImageView quiznextImage = (ImageView) itemView.findViewById(R.id.quiznextImage);

        if (model.getUrl() != null) {
            Log.e("BBBBB", "url is not null, can load image");
            String fileUrl = model.getUrl();
            String extension = fileUrl.substring(fileUrl.lastIndexOf("."), fileUrl.indexOf("?"));
            Log.e("BBBBBB", Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES)+"/noon/"+model.getFileid()+extension);
            File file = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES), "noon/"+model.getFileid()+extension);
            Log.e("BBBBBB", file.getAbsolutePath());
            if(file.exists()){
                Log.e("BBBBBB", "file exists");
                GlideApp.with(ctx)
                        .load(Uri.fromFile(file))
                        .error(R.drawable.ic_no_image_found)
                        .into(quizImage);
            } else{
                Log.e("BBBBBB", "file not exists");
                GlideApp.with(ctx)
                        .load(model.getUrl())
                        .error(R.drawable.ic_no_image_found)
                        .into(quizImage);
            }
//            GlideApp.with(ctx)
//                        .load(model.getUrl())
//                        .error(R.drawable.ic_no_image_found)
//                        .into(quizImage);

            if (flag != 0) {
                frameImage.setBackgroundColor(ctx.getResources().getColor(R.color.colorTransparent));
            } else {
                frameImage.setBackgroundColor(0);
            }

            quizImage.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    if (flag == 0) {
                        openDialogViewer();
                    }
                }
            });
        }
        container.addView(itemView);
        return itemView;
    }


    @Override
    public int getCount() {
        return list.size();
    }

    @Override
    public boolean isViewFromObject(@NonNull View view, @NonNull Object object) {
        return view == ((FrameLayout) object);
    }

    @Override
    public void destroyItem(ViewGroup container, int position, Object object) {
        container.removeView((FrameLayout) object);
    }

    public void openDialogViewer() {

        final Dialog dialog = new Dialog(ctx, android.R.style.Theme_Translucent_NoTitleBar);
        dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        dialog.getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.MATCH_PARENT);
        dialog.setCancelable(true);
        ImageviewpagerLayoutBinding ImageviewpagerLayoutBinding = DataBindingUtil.inflate(LayoutInflater.from(ctx), R.layout.imageviewpager_layout, null, false);
        dialog.setContentView(ImageviewpagerLayoutBinding.getRoot());
        dialog.show();

        ImageviewAdapter imageviewAdapter = new ImageviewAdapter(ctx, list, 1);
        ImageviewpagerLayoutBinding.imagePager.setAdapter(imageviewAdapter);

       /* GlideApp.with(ctx)
                .load(imageUri)
                .error(R.drawable.ic_no_image_found)
                .into(dialogViewerItemLayoutBinding.imageViewLayout.imageViewDialog);*/

    }
}