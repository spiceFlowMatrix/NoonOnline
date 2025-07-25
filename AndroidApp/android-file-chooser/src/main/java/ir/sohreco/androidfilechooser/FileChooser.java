package ir.sohreco.androidfilechooser;


import android.os.Bundle;
import android.os.Environment;
import androidx.annotation.ColorRes;
import androidx.annotation.DrawableRes;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.appcompat.app.AppCompatDelegate;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import java.io.File;
import java.io.FileFilter;
import java.io.Serializable;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class FileChooser extends Fragment
        implements ItemHolder.OnItemClickListener, View.OnClickListener {
    public final static String FILE_NAMES_SEPARATOR = ":";

    private final static String KEY_CHOOSER_TYPE = "chooserType";
    private final static String KEY_FILE_FORMATS = "fileFormats";
    private final static String KEY_MULTIPLE_FILE_SELECTION_ENABLED = "multipleFileSelectionEnabled";
    private final static String KEY_INITIAL_DIRECTORY = "initialDirectory";
    private final static String KEY_SELECT_DIRECTORY_BUTTON_TEXT = "selectDirectoryButtonText";
    private final static String KEY_SELECT_DIRECTORY_BUTTON_TEXT_SIZE = "selectDirectoryButtonTextSize";
    private final static String KEY_SELECT_DIRECTORY_BUTTON_TEXT_COLOR_RES_ID = "selectDirectoryButtonTextColorResId";
    private final static String KEY_SELECT_DIRECTORY_BUTTON_BACKGROUND_RES_ID = "selectDirectoryButtonBackgroundResId";
    private final static String KEY_LIST_ITEMS_TEXT_COLOR_RES_ID = "listItemsTextColorResId";
    private final static String KEY_FILE_ICON_RES_ID = "fileIconResId";
    private final static String KEY_DIRECTORY_ICON_RES_ID = "directoryIconResId";
    private final static String KEY_PREVIOUS_DIRECTORY_BUTTON_ICON_RES_ID = "previousDirectoryButtonIconResId";

    public interface ChooserListener extends Serializable {
        /**
         * This method gets called when user selects a file or a directory depending on the chooser type.
         *
         * @param path path of the selected file or directory.
         */
        void onSelect(String path);
    }

    public enum ChooserType {
        FILE_CHOOSER,
        DIRECTORY_CHOOSER
    }

    public static class Builder {
        // Required parameters
        private ChooserType chooserType;
        private ChooserListener chooserListener;

        // Optional parameters
        private String[] fileFormats;
        private boolean multipleFileSelectionEnabled;
        private String initialDirectory;
        private String selectDirectoryButtonText;
        private float selectDirectoryButtonTextSize;
        @ColorRes
        private int selectDirectoryButtonTextColorResId;
        @DrawableRes
        private int selectDirectoryButtonBackgroundResId;
        @ColorRes
        private int listItemsTextColorResId;
        @DrawableRes
        private int fileIconResId = R.drawable.ic_file;


        @DrawableRes
        private int directoryIconResId = R.drawable.ic_directory;
        @DrawableRes
        private int previousDirectoryButtonIconResId = R.drawable.ic_prev_dir;

        /**
         * Creates a builder for a FileChooser fragment.
         *
         * @param chooserType You can choose to create either a FileChooser or a DirectoryChooser
         */
        public Builder(ChooserType chooserType, ChooserListener chooserListener) {
            if (chooserType == null)
                throw new IllegalArgumentException("chooserType can not be null.");

            if (chooserListener == null)
                throw new IllegalArgumentException("chooserListener can not be null.");

            this.chooserType = chooserType;
            this.chooserListener = chooserListener;
        }

        /**
         * Set file formats which are going to be shown by this FileChooser.
         * All types of files will be shown if you don't set it.
         *
         * @param fileFormats A string array of file formats
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalArgumentException if fileFormats is null or if it doesn't have any elements
         */
        public Builder setFileFormats(String[] fileFormats) {
            if (chooserType == ChooserType.DIRECTORY_CHOOSER)
                throw new IllegalStateException("Can't set file formats when chooser type is DIRECTORY_CHOOSER.");

            if (fileFormats == null)
                throw new IllegalArgumentException("File formats can't be null. If you want all types of files to be shown, simply don't set this parameter.");

            if (fileFormats.length == 0)
                throw new IllegalArgumentException("File formats can't be empty. If you want all types of files to be shown, simply don't set this parameter.");

            this.fileFormats = fileFormats;
            return this;
        }

        /**
         * Set whether multiple file selection should be enabled or not.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is DIRECTORY_CHOOSER
         */
        public Builder setMultipleFileSelectionEnabled(boolean enabled) {
            if (chooserType == ChooserType.DIRECTORY_CHOOSER)
                throw new IllegalStateException("Multiple file selection can't be enabled when chooser type is DIRECTORY_CHOOSER.");

            this.multipleFileSelectionEnabled = enabled;
            return this;
        }

        /**
         * Set the initial directory of this FileChooser.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalArgumentException if:<br>
         *                                  initialDirectory is null.<br>
         *                                  initialDirectory does not exist.<br>
         *                                  initialDirectory is not a directory.<br>
         *                                  initialDirectory is not accessible due to access restrictions.
         */
        public Builder setInitialDirectory(File initialDirectory) {
            if (initialDirectory == null)
                throw new IllegalArgumentException("initialDirectory can't be null.");

            if (!initialDirectory.exists())
                throw new IllegalArgumentException(initialDirectory.getPath() + " Does not exist.");

            if (!initialDirectory.isDirectory())
                throw new IllegalArgumentException(initialDirectory.getPath() + " Is not a directory.");

            if (!initialDirectory.canRead())
                throw new IllegalArgumentException("Can't access " + initialDirectory.getPath());

            this.initialDirectory = initialDirectory.getPath();
            return this;
        }

        /**
         * Set select files button's text. You will see this button when chooser type is FILE_CHOOSER with multiple file selection enabled.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is DIRECTORY_CHOOSER
         */
        public Builder setSelectMultipleFilesButtonText(String text) {
            if (chooserType == ChooserType.DIRECTORY_CHOOSER)
                throw new IllegalStateException("Can't set select multiple files button's text when chooser type is DIRECTORY_CHOOSER.");
            selectDirectoryButtonText = text;
            return this;
        }

        /**
         * Set select files button's text size. You will see this button when chooser type is FILE_CHOOSER with multiple file selection enabled.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is DIRECTORY_CHOOSER
         */
        public Builder setSelectMultipleFilesButtonTextSize(float textSize) {
            if (chooserType == ChooserType.DIRECTORY_CHOOSER)
                throw new IllegalStateException("Can't set select multiple files button's text size when chooser type is DIRECTORY_CHOOSER.");

            if (textSize <= 0)
                throw new IllegalArgumentException("textSize can't be less than or equal to zero.");

            selectDirectoryButtonTextSize = textSize;
            return this;
        }

        /**
         * Set select files button's text color. You will see this button when chooser type is FILE_CHOOSER with multiple file selection enabled.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is DIRECTORY_CHOOSER
         */
        public Builder setSelectMultipleFilesButtonTextColor(@ColorRes int resId) {
            if (chooserType == ChooserType.DIRECTORY_CHOOSER)
                throw new IllegalStateException("Can't set select multiple files button's text color when chooser type is DIRECTORY_CHOOSER.");

            if (resId <= 0)
                throw new IllegalArgumentException("resId can't be less than or equal to zero.");

            selectDirectoryButtonTextColorResId = resId;
            return this;
        }

        /**
         * Set select files button's background. You will see this button when chooser type is FILE_CHOOSER with multiple file selection enabled.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is DIRECTORY_CHOOSER
         */
        public Builder setSelectMultipleFilesButtonBackground(@DrawableRes int resId) {
            if (chooserType == ChooserType.DIRECTORY_CHOOSER)
                throw new IllegalStateException("Can't set select multiple files button's background when chooser type is DIRECTORY_CHOOSER.");

            if (resId <= 0)
                throw new IllegalArgumentException("resId can't be less than or equal to zero.");

            selectDirectoryButtonBackgroundResId = resId;
            return this;
        }

        /**
         * Set select directory button's text. You will see this button when chooser type is DIRECTORY_CHOOSER.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is FILE_CHOOSER
         */
        public Builder setSelectDirectoryButtonText(String text) {
            if (chooserType == ChooserType.FILE_CHOOSER)
                throw new IllegalStateException("Can't set select directory button's text when chooser type is FILE_CHOOSER.");

            selectDirectoryButtonText = text;
            return this;
        }

        /**
         * Set select directory button's text size.
         *
         * @param textSize must be based on scaled pixel(SP) unit.
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is FILE_CHOOSER
         */
        public Builder setSelectDirectoryButtonTextSize(float textSize) {
            if (chooserType == ChooserType.FILE_CHOOSER)
                throw new IllegalStateException("Can't set select directory button's text size when chooser type is FILE_CHOOSER.");

            if (textSize <= 0)
                throw new IllegalArgumentException("textSize can't be less than or equal to zero.");

            selectDirectoryButtonTextSize = textSize;
            return this;
        }

        /**
         * Set select directory button's text color. You will see this button when chooser type is DIRECTORY_CHOOSER.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is FILE_CHOOSER
         */
        public Builder setSelectDirectoryButtonTextColor(@ColorRes int resId) {
            if (chooserType == ChooserType.FILE_CHOOSER)
                throw new IllegalStateException("Can't set select directory button's text color when chooser type is FILE_CHOOSER.");

            if (resId <= 0)
                throw new IllegalArgumentException("resId can't be less than or equal to zero.");

            selectDirectoryButtonTextColorResId = resId;
            return this;
        }

        /**
         * Set select directory button's background. You will see this button when chooser type is DIRECTORY_CHOOSER.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is FILE_CHOOSER
         */
        public Builder setSelectDirectoryButtonBackground(@DrawableRes int resId) {
            if (chooserType == ChooserType.FILE_CHOOSER)
                throw new IllegalStateException("Can't set select directory button's background when chooser type is FILE_CHOOSER.");

            if (resId <= 0)
                throw new IllegalArgumentException("resId can't be less than or equal to zero.");

            selectDirectoryButtonBackgroundResId = resId;
            return this;
        }

        /**
         * Set the color of list item titles in this FileChooser.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         */
        public Builder setListItemsTextColor(@ColorRes int resId) {
            if (resId <= 0)
                throw new IllegalArgumentException("resId can't be less than or equal to zero.");

            listItemsTextColorResId = resId;
            return this;
        }

        /**
         * Set the icon for files in this FileChooser's list
         * Default icon will be used if you don't set it.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         * @throws IllegalStateException if you call this method when chooser type is DIRECTORY_CHOOSER
         */
        public Builder setFileIcon(@DrawableRes int resId) {
            if (chooserType == ChooserType.DIRECTORY_CHOOSER)
                throw new IllegalStateException("Can't set file icon when chooser type is DIRECTORY_CHOOSER.");

            if (resId <= 0)
                throw new IllegalArgumentException("resId can't be less than or equal to zero.");

            fileIconResId = resId;
            return this;
        }

        /**
         * Set the icon for directories in this FileChooser's list.
         * Default icon will be used if you don't set it.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         */
        public Builder setDirectoryIcon(@DrawableRes int resId) {
            if (resId <= 0)
                throw new IllegalArgumentException("resId can't be less than or equal to zero.");

            directoryIconResId = resId;
            return this;
        }

        /**
         * Set the icon for the button that is going to be used to go to the parent of the current directory.
         * Default icon will be used if you don't set it.
         *
         * @return This Builder object to allow for chaining of calls to set methods
         */
        public Builder setPreviousDirectoryButtonIcon(@DrawableRes int resId) {
            if (resId <= 0)
                throw new IllegalArgumentException("resId can't be less than or equal to zero.");

            previousDirectoryButtonIconResId = resId;
            return this;
        }

        /**
         * Returns an instance of FileChooser with the given configurations.
         *
         * @throws ExternalStorageNotAvailableException If there is no external storage available on user's device
         */
        public FileChooser build() throws ExternalStorageNotAvailableException {
            String externalStorageState = Environment.getExternalStorageState();
            boolean externalStorageAvailable = externalStorageState.equals(Environment.MEDIA_MOUNTED)
                    || externalStorageState.equals(Environment.MEDIA_MOUNTED_READ_ONLY);
            if (!externalStorageAvailable) {
                throw new ExternalStorageNotAvailableException();
            }

            FileChooser fragment = new FileChooser();

            Bundle args = new Bundle();
            args.putSerializable(KEY_CHOOSER_TYPE, chooserType);
            fragment.chooserListener = chooserListener;
            args.putStringArray(KEY_FILE_FORMATS, fileFormats);
            args.putBoolean(KEY_MULTIPLE_FILE_SELECTION_ENABLED, multipleFileSelectionEnabled);
            args.putString(KEY_INITIAL_DIRECTORY, initialDirectory);
            args.putString(KEY_SELECT_DIRECTORY_BUTTON_TEXT, selectDirectoryButtonText);
            args.putFloat(KEY_SELECT_DIRECTORY_BUTTON_TEXT_SIZE, selectDirectoryButtonTextSize);
            args.putInt(KEY_SELECT_DIRECTORY_BUTTON_TEXT_COLOR_RES_ID, selectDirectoryButtonTextColorResId);
            args.putInt(KEY_SELECT_DIRECTORY_BUTTON_BACKGROUND_RES_ID, selectDirectoryButtonBackgroundResId);
            args.putInt(KEY_LIST_ITEMS_TEXT_COLOR_RES_ID, listItemsTextColorResId);
            args.putInt(KEY_FILE_ICON_RES_ID, fileIconResId);
            args.putInt(KEY_DIRECTORY_ICON_RES_ID, directoryIconResId);
            args.putInt(KEY_PREVIOUS_DIRECTORY_BUTTON_ICON_RES_ID, previousDirectoryButtonIconResId);

            fragment.setArguments(args);
            return fragment;
        }
    }

    private ImageView ibPrevDirectory;
    private ImageView btnSelectDirectory;
    private RecyclerView rvItems;
    private TextView tvCurrentDirectory;
    private ChooserType chooserType;
    private ChooserListener chooserListener;
    private ItemsAdapter itemsAdapter;
    private String currentDirectoryPath;
    private String[] fileFormats;
    private boolean multipleFileSelectionEnabled;
    private String initialDirectory;
    private String selectDirectoryButtonText;
    private float selectDirectoryButtonTextSize;
    @ColorRes
    private int selectDirectoryButtonTextColorResId;
    @DrawableRes
    private int selectDirectoryButtonBackgroundResId;
    @ColorRes
    private int listItemsTextColorResId;
    @DrawableRes
    private int fileIconResId;
    @DrawableRes
    private int directoryIconResId;
    @DrawableRes
    private int previousDirectoryButtonIconResId;


    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        getGivenArguments();
        AppCompatDelegate.setCompatVectorFromResourcesEnabled(true);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_file_chooser, container, false);
        findViews(view);
        setListeners();

        // When multiple file selection is enabled, we use the select directory button to let the user
        // finish the file selection process.
        // So select directory button must be available in both situations.
        if (chooserType == ChooserType.DIRECTORY_CHOOSER || multipleFileSelectionEnabled) {
            btnSelectDirectory.setVisibility(View.VISIBLE);
           /* btnSelectDirectory.setText(selectDirectoryButtonText);
            if (selectDirectoryButtonBackgroundResId != 0)
                btnSelectDirectory.setBackgroundResource(selectDirectoryButtonBackgroundResId);
            if (selectDirectoryButtonTextColorResId != 0)
                btnSelectDirectory.setTextColor(ContextCompat.getColor(getContext(),
                        selectDirectoryButtonTextColorResId));
            if (selectDirectoryButtonTextSize != 0)
                btnSelectDirectory.setTextSize(selectDirectoryButtonTextSize);*/
        }
        ibPrevDirectory.setImageResource(previousDirectoryButtonIconResId);
//        ibPrevDirectory.setBackgroundResource(previousDirectoryButtonIconResId);

        itemsAdapter = new ItemsAdapter(this, multipleFileSelectionEnabled, listItemsTextColorResId);
        rvItems.setLayoutManager(new LinearLayoutManager(getActivity()));
        rvItems.setAdapter(itemsAdapter);

        loadItems(initialDirectory != null ? initialDirectory : Environment.getExternalStorageDirectory().getPath());

        return view;
    }

    @Override
    public void onItemClick(Item item) {
        if (item.isDirectory()) {
            loadItems(item.getPath());
        } else {
            chooserListener.onSelect(item.getPath());
        }
    }

    @Override
    public void onClick(View view) {
        int id = view.getId();
        if (id == R.id.previous_dir_imagebutton) {
            File parent = new File(currentDirectoryPath).getParentFile();
            if (parent != null && parent.canRead()) {
                loadItems(parent.getPath());
            } else {
                chooserListener.onSelect("");
            }
        } else if (id == R.id.select_dir_button) {
            if (chooserType == ChooserType.DIRECTORY_CHOOSER) {
                chooserListener.onSelect(currentDirectoryPath);
            } else if (multipleFileSelectionEnabled) {
                // Now select directory button acts as select files button
                chooserListener.onSelect(itemsAdapter.getSelectedItems());
            }
        }
    }

    private void loadItems(String path) {
        currentDirectoryPath = path;

        String currentDir = path.substring(path.lastIndexOf(File.separator) + 1);
        tvCurrentDirectory.setText(currentDir);

        File[] files = new File(path).listFiles(new FileFilter() {
            @Override
            public boolean accept(File file) {
                if (file.canRead()) {
                    if (chooserType == ChooserType.FILE_CHOOSER && file.isFile()) {
                        if (fileFormats != null) {
                            for (String fileFormat : fileFormats) {
                                if (file.getName().endsWith(fileFormat)) {
                                    return true;
                                }
                            }
                            return false;
                        }
                        return true;
                    }
                    return file.isDirectory();
                }
                return false;
            }
        });

        List<Item> items = new ArrayList<>();
        for (File f : files) {
            int drawableId = f.isFile() ? fileIconResId : directoryIconResId;

            boolean isFile = f.isFile() ? true : false;

            //Drawable drawable = ContextCompat.getDrawable(getActivity().getApplicationContext(), drawableId);
            items.add(new Item(f.getPath(), null, isFile));
        }
        Collections.sort(items);
        itemsAdapter.setItems(items);
    }

    private void getGivenArguments() {
        Bundle args = getArguments();
        chooserType = (ChooserType) args.getSerializable(KEY_CHOOSER_TYPE);
        fileFormats = args.getStringArray(KEY_FILE_FORMATS);
        multipleFileSelectionEnabled = args.getBoolean(KEY_MULTIPLE_FILE_SELECTION_ENABLED);
        initialDirectory = args.getString(KEY_INITIAL_DIRECTORY);
        selectDirectoryButtonText = args.getString(KEY_SELECT_DIRECTORY_BUTTON_TEXT);
        selectDirectoryButtonTextSize = args.getFloat(KEY_SELECT_DIRECTORY_BUTTON_TEXT_SIZE);
        selectDirectoryButtonTextColorResId = args.getInt(KEY_SELECT_DIRECTORY_BUTTON_TEXT_COLOR_RES_ID);
        selectDirectoryButtonBackgroundResId = args.getInt(KEY_SELECT_DIRECTORY_BUTTON_BACKGROUND_RES_ID);
        listItemsTextColorResId = args.getInt(KEY_LIST_ITEMS_TEXT_COLOR_RES_ID);
        fileIconResId = args.getInt(KEY_FILE_ICON_RES_ID);
        directoryIconResId = args.getInt(KEY_DIRECTORY_ICON_RES_ID);
        previousDirectoryButtonIconResId = args.getInt(KEY_PREVIOUS_DIRECTORY_BUTTON_ICON_RES_ID);
    }

    private void setListeners() {
        ibPrevDirectory.setOnClickListener(this);
        btnSelectDirectory.setOnClickListener(this);
    }

    private void findViews(View v) {
        rvItems = v.findViewById(R.id.items_recyclerview);
        ibPrevDirectory = v.findViewById(R.id.previous_dir_imagebutton);
        btnSelectDirectory = v.findViewById(R.id.select_dir_button);
        tvCurrentDirectory = v.findViewById(R.id.current_dir_textview);
    }


}