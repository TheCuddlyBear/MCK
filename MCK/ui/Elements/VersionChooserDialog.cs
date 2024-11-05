using Adw;
using Gtk;
using ApplicationWindow = Gtk.ApplicationWindow;
using HeaderBar = Adw.HeaderBar;

namespace MCK.ui.Elements;

public class VersionChooserDialog : Adw.Dialog
{
    private String _currentVersion;
    private WindowTitle _windowTitle;
    public string CurrentVersion
    {
        get
        {
            return _currentVersion;
        }
        set
        {
            _currentVersion = value;
        }
    }

    public VersionChooserDialog()
    {
        CurrentVersion = "1.21";
        BuildUi();
    }

    private void BuildUi()
    {
        // TODO: UI

        var toolBarView = new ToolbarView();
        toolBarView.AddTopBar(GetHeaderBar());
        toolBarView.SetContent(GetListBox());

        HeaderBar GetHeaderBar()
        {
            var headerBar = new HeaderBar();
            var titleBox = new Gtk.Box();
            titleBox.Append(_windowTitle = new WindowTitle()
            {
                Title = "Choose Version",
            });
            headerBar.SetTitleWidget(titleBox);
            
            return headerBar;
        }

        SetChild(toolBarView);

        ListBox GetListBox()
        {
            var listBox = new ListBox();
            var testButton = new Gtk.Button();
            testButton.SetLabel("hello");
            listBox.Append(testButton);
            return listBox;
        }

    }

    public static String ChooseVersion(ApplicationWindow parent, String currentSelected)
    {
        var dialog = new VersionChooserDialog();
        dialog.Present(parent);
        
        return "";
    }

}