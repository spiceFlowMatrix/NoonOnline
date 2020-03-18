package ir.sohreco.androidfilechooser;

import android.graphics.drawable.Drawable;
import android.support.annotation.NonNull;

import java.io.File;

class Item extends File implements Comparable<File> {
    private Drawable icon;
    private boolean isfile;

    Item(String path, Drawable icon, boolean isfile) {
        super(path);
        this.icon = icon;
        this.isfile = isfile;
    }

    Drawable getIcon() {
        return icon;
    }


    public void setIcon(Drawable icon) {
        this.icon = icon;
    }

    public boolean isIsfile() {
        return isfile;
    }

    public void setIsfile(boolean isfile) {
        this.isfile = isfile;
    }

    // We want our list to show directories first and then show files below them
    // so we need to sort our items. Since Collections.sort() sorts items in ascending order
    // we should assume that directories are smaller than files.
    @Override
    public int compareTo(@NonNull File pathname) {
        if (isDirectory() && pathname.isFile()) {
            return -1;
        }
        if (isFile() && pathname.isDirectory()) {
            return 1;
        }
        return 0;
    }
}