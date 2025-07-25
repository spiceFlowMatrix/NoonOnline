package tcking.github.com.giraffeplayer2;

import android.app.Activity;
import android.content.Context;
import android.net.Uri;
import android.os.Build;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.core.view.ScrollingView;
import android.util.AttributeSet;
import android.util.Log;
import android.view.ViewGroup;
import android.view.ViewParent;
import android.widget.AbsListView;
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.ScrollView;
import android.widget.Toast;

import com.github.tcking.giraffeplayer2.R;

/**
 * Created by tcking on 2017
 */

public class VideoView extends FrameLayout implements DefaultMediaController.Onbackclick {


    private MediaController mediaController;
    private PlayerListener playerListener;
    private DefaultMediaController.Onbackclick onbackclick;
    private ViewGroup container;
    private Activity activity;

    public PlayerListener getPlayerListener() {
        return playerListener;
    }

    public VideoView setPlayerListener(PlayerListener playerListener, DefaultMediaController.Onbackclick onbackclick) {
        this.playerListener = playerListener;
        this.onbackclick = onbackclick;
        return this;
    }

    public VideoInfo getVideoInfo() {
        return videoInfo;
    }

    public VideoView videoInfo(VideoInfo videoInfo) {
        if (this.videoInfo.getUri() != null && !this.videoInfo.getUri().equals(videoInfo.getUri())) {
            PlayerManager.getInstance().releaseByFingerprint(this.videoInfo.getFingerprint());
        }
        this.videoInfo = videoInfo;
        return this;
    }

    private VideoInfo videoInfo = VideoInfo.createFromDefault();

    public VideoView(@NonNull Context context) {
        super(context);
        init(context);
    }

    public VideoView(@NonNull Context context, @Nullable AttributeSet attrs) {
        super(context, attrs);
        init(context);
    }

    public VideoView(@NonNull Context context, @Nullable AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        init(context);
    }

    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    public VideoView(@NonNull Context context, @Nullable AttributeSet attrs, int defStyleAttr, int defStyleRes) {
        super(context, attrs, defStyleAttr, defStyleRes);
        init(context);
    }

    private void init(Context context) {
        activity = (Activity) context;
        container = new FrameLayout(context);
        addView(container, new LayoutParams(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.MATCH_PARENT));
        initMediaController();
        setBackgroundColor(videoInfo.getBgColor());
    }


    private void initMediaController() {
        mediaController = new DefaultMediaController(getContext(), this);
        mediaController.bind(this);
    }


    public VideoView setFingerprint(Object fingerprint) {
        videoInfo.setFingerprint(fingerprint);
        return this;
    }

    public VideoView setVideoPath(String uri) {
        videoInfo.setUri(Uri.parse(uri));
        return this;
    }

    public GiraffePlayer getPlayer() {
        if (videoInfo.getUri() == null) {
            //throw new RuntimeException("player uri is null");
            try {
                // Toast.makeText(activity, "Unknown error occurred...! Please Try Again.", Toast.LENGTH_LONG).show();
                Log.e("ERROR", "Unknown error occurred...! Please Try Again.");
            } catch (RuntimeException e) {
                e.printStackTrace();
                Toast.makeText(activity, e.getMessage(), Toast.LENGTH_LONG).show();
            }
        }

        return PlayerManager.getInstance().getPlayer(this);
    }

    /**
     * is current active player (in list controllerView there are many players)
     *
     * @return boolean
     */
    public boolean isCurrentActivePlayer() {
        return PlayerManager.getInstance().isCurrentPlayer(videoInfo.getFingerprint());
    }

    public MediaController getMediaController() {
        return mediaController;
    }

    /**
     * is video controllerView in 'list' controllerView
     *
     * @return
     */
    public boolean inListView() {
        for (ViewParent vp = getParent(); vp != null; vp = vp.getParent()) {
            if (vp instanceof AbsListView || vp instanceof ScrollingView || vp instanceof ScrollView) {
                return true;
            }
        }
        return false;
    }

    public ViewGroup getContainer() {
        return container;
    }

    public ImageView getCoverView() {
        return (ImageView) findViewById(R.id.app_video_cover);
    }

    @Override
    public void backperfome() {
        onbackclick.backperfome();
    }

    @Override
    public void fullscreenperfome() {
        onbackclick.fullscreenperfome();
    }
}